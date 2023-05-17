using HotelExamples.Data;
using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        public string  Message { get; set; }
        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public string Password2 { get; set; }
        private IUserService uService;
        public CreateUserModel(IUserService userservice)
        {
            uService= userservice;
        }
        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email == null || email == "")
            {
                return RedirectToPage("/Users/Login");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (User.Password != Password2)
            {
                Message = "Passwords er ikke ens";
                return Page();
            }
            else
            {
                MockData.UserData.Add(User);
                return RedirectToPage("Login");
            }
        }
    }
}
