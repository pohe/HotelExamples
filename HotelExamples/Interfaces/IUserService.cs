using HotelExamples.Models;

namespace HotelExamples.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public User VerifyUser(string email, string passWord);

        public User GetLoggedUser(string email);
    }
}
