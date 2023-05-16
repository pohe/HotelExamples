using HotelExamples.Interfaces;
using HotelExamples.Models;
using HotelExamples.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Services
{
    public class UserService : IUserService
    {
        public List<User> GetAllUsers()
        {
            return MockData.UserData;
        }



        public User GetLoggedUser(string email)
        {
            if (email != null)
            {
                return GetAllUsers().Find(u => u.Email == email);
            }
            else
                return null;
        }

        public User VerifyUser(string email, string passWord)
        {
            foreach (var user in GetAllUsers())
            {
                if (email.Equals(user.Email) && passWord.Equals(user.Password))
                {
                    return user;
                }

            }
            return null;

        }
    }
}
