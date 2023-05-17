using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Pages.Hotels
{
    public class GetAllHotelsModel : PageModel
    {
        public string UserName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        public List<Hotel> Hotels { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        private IHotelService hService;
        
        public GetAllHotelsModel(IHotelService hotelService)
        {
            hService = hotelService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email == null || email== "")
            {
                return RedirectToPage("/Users/Login");
            }
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                Hotels = await hService.GetHotelsByNameAsync(FilterCriteria);
            }
            else
            {
                Hotels = await hService.GetAllHotelAsync();
            }
            return Page();
        }
    }
}
