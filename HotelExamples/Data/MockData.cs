using HotelExamples.Models;

namespace HotelExamples.Data
{
    public class MockData
    {
        public static List<User> UserData = new List<User> { new User { Id = 1, Email = "Test@test.dk", Password = "123", Adm=false }
        , new User { Id = 2, Email = "Poul@rhs.dk", Password = "123", Adm = true}
        ,new User { Id = 3, Email = "Charlotte@rhs.dk", Password = "123" , Adm=true}};
    }
}
