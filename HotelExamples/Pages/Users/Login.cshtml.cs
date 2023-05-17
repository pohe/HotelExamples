using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Pages.Users
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string PassWord { get; set; }

        public string Message { get; set; }

        private IUserService _userService;
        public LoginModel(IUserService userservice)
        {
            _userService = userservice;
        }

        public void OnGet()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email != null)
            {
                ViewData["Email"] = email;
            }
            else
            {
                ViewData["Email"] = null;
            }
        }

        public void OnGetLogout()
        {
            HttpContext.Session.Remove("Email");

        }

        public IActionResult OnPost()
        {
            try
            {


                User loginUser = _userService.VerifyUser(Email, PassWord);
                if (loginUser != null)
                {
                    HttpContext.Session.SetString("Email", loginUser.Email);
                    return RedirectToPage("/Hotels/GetAllHotels");
                }
                else
                {
                    Message = "Invalid email or password";
                    Email = "";
                    PassWord = "";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }

        }
    }
}

