using Microsoft.EntityFrameworkCore;
using Printstream.Infrastructure;
using Printstream.Models;

namespace Printstream.Services
{
    public interface IDBService
    {
        Task<List<PersonAggregatedDTO>> FindPersonsDataFIO(string lastName, string? firstName, string? middleName);
        Task<List<PersonAggregatedDTO>> FindPersonsDataValue(string type, string Data);
    }
    
    public class DBService : IDBService
    {
        private readonly AppDbContext _context;

        public DBService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PersonAggregatedDTO>> FindPersonsDataFIO(string lastName, string? firstName, string? middleName)
        {
            try
            {
                var query = _context.Person.AsQueryable();

                query = query.Where(z =>
                                    z.PersonData.LastName   == lastName   &&
                                   (z.PersonData.FirstName  == firstName  || string.IsNullOrEmpty(firstName)) &&
                                   (z.PersonData.MiddleName == middleName || string.IsNullOrEmpty(middleName)));

                return await query.Select(z => new PersonAggregatedDTO
                {
                    Person_ID = z.ID,
                    LastName = z.PersonData.LastName,
                    FirstName = z.PersonData.FirstName,
                    MiddleName = z.PersonData.MiddleName,
                    DateOfBirth = z.PersonData.DateOfBirth,
                    IsMale = z.PersonData.IsMale,
                    Bunch_Value = z.Bunch == null ? null : z.Bunch.bunch,
                    Addresses = z.Address.Select(z => z.address).Distinct().ToList(),
                    Phones = z.Phone.Select(z => z.phone).Distinct().ToList(),
                    Emails = z.Email.Select(z => z.email).Distinct().ToList()
                }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<PersonAggregatedDTO>> FindPersonsDataValue(string type, string Value)
        {
            try
            {
                var query = _context.Person.AsQueryable();

                query = type switch
                {
                    "Address" => query.Where(z => z.Address.Any(z => z.address != null && EF.Functions.ILike(z.address, Value.Trim()))),
                    "Phone"   => query.Where(z => z.Phone.Any(z => z.phone != null && EF.Functions.ILike(z.phone, Value.Trim()))),
                    "Email"   => query.Where(z => z.Email.Any(z => z.email != null && EF.Functions.ILike(z.email, Value.Trim()))),
                    _ => query
                };

                return await query.Select(z => new PersonAggregatedDTO
                {
                    Person_ID = z.ID,
                    LastName = z.PersonData.LastName,
                    FirstName = z.PersonData.FirstName,
                    MiddleName = z.PersonData.MiddleName,
                    DateOfBirth = z.PersonData.DateOfBirth,
                    IsMale = z.PersonData.IsMale,
                    Bunch_Value = z.Bunch == null ? null : z.Bunch.bunch,
                    Addresses = z.Address.Select(z => z.address).Distinct().ToList(),
                    Phones = z.Phone.Select(z => z.phone).Distinct().ToList(),
                    Emails = z.Email.Select(z => z.email).Distinct().ToList()
                }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}