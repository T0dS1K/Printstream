namespace Printstream.Models
{
    public class UserSession
    {
        public string? SessionID { get; set; }
        public UserProfile Data { get; set; }

        public UserSession() { Data = new UserProfile(); }
        public UserSession(string SessionID, UserDTO Data)
        {
            this.SessionID = SessionID;
            this.Data = new UserProfile(Data);
        }
    }
}