using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Pages.Users
{
    public class LogoutModel : PageModel
    {

        public string Message { get; set; }
        public LogoutModel()
        {
            HttpContext.Session.Remove("Email");
            ViewData["Email"] = null;
        }
        public void OnGet()
        {
            Message = "You have been logout - com back soon!";
        }
    }
}
