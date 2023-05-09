using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelExamples.Pages.Hotels
{
    public class GetAllHotelsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        public List<Hotel> Hotels { get; set; }

        private IHotelService hService;
        public GetAllHotelsModel(IHotelService hotelService)
        {
            hService = hotelService;
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                Hotels = await hService.GetHotelsByNameAsync(FilterCriteria);
            }
            else
            {
                Hotels = await hService.GetAllHotelAsync();
            }

        }
    }
}
