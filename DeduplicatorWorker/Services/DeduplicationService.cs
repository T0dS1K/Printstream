using Microsoft.EntityFrameworkCore;
using Printstream.Infrastructure;
using Printstream.Infrastructure.Configurations;
using Printstream.Models;

namespace DeduplicatorWorker.Services
{
    public interface IDeduplicator
    {
        Task<string> TryAddUser(UserSession User);
    }

    public class DeduplicationService : IDeduplicator
    {
        private Bunch newBunch { get; }
        private Person newPerson { get; }
        private AppDbContext _context { get; }

        public DeduplicationService(AppDbContext context)
        {
            _context = context;
            newBunch = new Bunch();
            newPerson = new Person();
        }

        public async Task<string> TryAddUser(UserSession User)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var personData = await _context.PersonData
                    .FirstOrDefaultAsync(z =>
                                         z.LastName == User.Data.LastName &&
                                         z.FirstName == User.Data.FirstName &&
                                         z.MiddleName == User.Data.MiddleName &&
                                         z.DateOfBirth == User.Data.DateOfBirth &&
                                         z.IsMale == User.Data.IsMale);

                if (personData == null)
                {
                    newBunch.bunch = await BunchGenerator();
                    var newPersonData = new PersonData
                    {
                        LastName = User.Data.LastName,
                        FirstName = User.Data.FirstName,
                        MiddleName = User.Data.MiddleName,
                        DateOfBirth = User.Data.DateOfBirth,
                        IsMale = User.Data.IsMale
                    };

                    _context.Bunch.Add(newBunch);
                    _context.PersonData.Add(newPersonData);
                    await _context.SaveChangesAsync();

                    newPerson.BunchID = newBunch.ID;
                    newPerson.PersonDataID = newPersonData.ID;
                    _context.Person.Add(newPerson);
                    await _context.SaveChangesAsync();

                    _context.Bunch_History.Add(new Bunch_History
                    {
                        PersonID = newPerson.ID,
                        BunchID = newBunch.ID,
                        DateFrom = DateTime.UtcNow,
                        Operation = "ADD"
                    });

                    await TryAddContacts(User.Data);
                }
                else
                {
                    var dataExistingPersons = await _context.Person
                        .Include(z => z.Address)
                        .Include(z => z.Phone)
                        .Include(z => z.Email)
                        .Where(z => z.PersonDataID == personData.ID)
                        .ToListAsync();

                    var subsetMatch = dataExistingPersons.FirstOrDefault(z =>
                        CheckContactSetRelation(z.Address.Select(v => v.address), User.Data.Addresses, false) &&
                        CheckContactSetRelation(z.Phone.Select(v => v.phone),     User.Data.Phones,    false) &&
                        CheckContactSetRelation(z.Email.Select(v => v.email),     User.Data.Emails,    false)
                    );

                    if (subsetMatch != null)
                    {
                        throw new InvalidOperationException("Duplicate detected");
                    }

                    var crossingPersons = dataExistingPersons.Where(z =>
                        CheckContactSetRelation(z.Address.Select(v => v.address), User.Data.Addresses, true) &&
                        CheckContactSetRelation(z.Phone.Select(v => v.phone),     User.Data.Phones,    true) &&
                        CheckContactSetRelation(z.Email.Select(v => v.email),     User.Data.Emails,    true)
                    ).ToList();

                    newBunch.bunch = await BunchGenerator();
                    _context.Bunch.Add(newBunch);
                    await _context.SaveChangesAsync();
                    newPerson.BunchID = newBunch.ID;

                    if (crossingPersons.Any())
                    {
                        var mergeEntries = await MergeBunches(crossingPersons, newPerson.BunchID);
                        
                        if (mergeEntries.Any())
                        {
                            _context.Bunch_History.AddRange(mergeEntries);
                        }
                    }

                    newPerson.PersonDataID = personData.ID;
                    _context.Person.Add(newPerson);
                    await _context.SaveChangesAsync();

                    _context.Bunch_History.Add(new Bunch_History
                    {
                        PersonID = newPerson.ID,
                        BunchID = newPerson.BunchID,
                        DateFrom = DateTime.UtcNow,
                        Operation = crossingPersons.Any() ? "JOIN" : "ADD"
                    });

                    await TryAddContacts(User.Data);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Entry added successfully";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> BunchGenerator()
        {
            string bunch;

            do
            {
                bunch = Guid.NewGuid().ToString("N").Substring(0, 8);
            }
            while (await _context.Bunch.AnyAsync(z => z.bunch == bunch));

            return bunch;
        }

        private async Task<List<Bunch_History>> MergeBunches(List<Person> crossingPersons, int newBunchID)
        {
            var oldBunchIDs = crossingPersons
                .Select(z => z.BunchID)
                .Where(z => z != newBunchID)
                .Distinct()
                .ToList();

            if (!oldBunchIDs.Any()) return new List<Bunch_History>();

            var CurrentTime = DateTime.UtcNow;

            await _context.Bunch_History
                .Where(z => oldBunchIDs.Contains(z.BunchID) && z.DateTo == null)
                .ExecuteUpdateAsync(z => z.SetProperty(z => z.DateTo, CurrentTime));

            var PersonIDsToUpdate = await _context.Person
                .Where(z => oldBunchIDs.Contains(z.BunchID))
                .Select(z => z.ID)
                .ToListAsync();

            var newHistoryEntries = PersonIDsToUpdate.Select(PersonID => new Bunch_History
            {
                PersonID = PersonID,
                BunchID = newBunchID,
                DateFrom = CurrentTime,
                Operation = "MERGE"
            }).ToList();

            await _context.Person
                .Where(z => oldBunchIDs.Contains(z.BunchID))
                .ExecuteUpdateAsync(z => z.SetProperty(z => z.BunchID, newBunchID));

            /*await _context.Bunch
                .Where(z => oldBunchIDs.Contains(z.ID))
                .ExecuteDeleteAsync();*/

            return newHistoryEntries;
        }

        private bool CheckContactSetRelation(IEnumerable<string?> existingContacts, ICollection<string>? newContacts, bool flag)
        {
            var existingSet = (existingContacts ?? Enumerable.Empty<string?>())
                .Where(z => !string.IsNullOrEmpty(z))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (flag)
            {
                if (!existingSet.Any())
                {
                    return true;
                }
                else
                {
                    if (newContacts?.Any() != true)
                    {
                        return false;
                    }
                }

                return newContacts.Any(existingSet.Contains);
            }
            else
            {
                if (newContacts?.Any() != true)
                {
                    return true;
                }

                return newContacts.All(existingSet.Contains);
            }
        }

        private async Task TryAddContacts(UserProfile User)
        {
            if (User.Addresses != null)
            {
                var existingAddresses = newPerson.Address.Select(z => z.address).ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var addressString in User.Addresses)
                {
                    if (existingAddresses.Contains(addressString)) continue;

                    var addressData = await _context.Address.FirstOrDefaultAsync(z => z.address == addressString);

                    if (addressData == null)
                    {
                        addressData = new Address { address = addressString };
                        _context.Address.Add(addressData);
                    }

                    newPerson.Address.Add(addressData);
                }
            }

            if (User.Phones != null)
            {
                var existingPhones = newPerson.Phone.Select(z => z.phone).ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var phoneString in User.Phones)
                {
                    if (existingPhones.Contains(phoneString)) continue;

                    var phoneData = await _context.Phone.FirstOrDefaultAsync(z => z.phone == phoneString);

                    if (phoneData == null)
                    {
                        phoneData = new Phone { phone = phoneString };
                        _context.Phone.Add(phoneData);
                    }
                    newPerson.Phone.Add(phoneData);
                }
            }

            if (User.Emails != null)
            {
                var existingEmails = newPerson.Email.Select(z => z.email).ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var emailString in User.Emails)
                {
                    if (existingEmails.Contains(emailString)) continue;

                    var emailData = await _context.Email.FirstOrDefaultAsync(z => z.email == emailString);

                    if (emailData == null)
                    {
                        emailData = new Email { email = emailString };
                        _context.Email.Add(emailData);
                    }
                    newPerson.Email.Add(emailData);
                }
            }
        }
    } 
}