using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelExamples.Pages.Hotels
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Hotel Hotel { get; set; }

        private IHotelService hservice;
        
        [BindProperty]
        public IFormFile Photo { get; set; }

        private IWebHostEnvironment webHostEnvironment;

        public CreateModel(IHotelService hotelService, IWebHostEnvironment webHost)
        {
            hservice = hotelService;
            webHostEnvironment = webHost;
        }

       
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string vip_id)
        {
            //Hotel.HotelType = HotelTypes.Business;
            if (vip_id == "VIP")
                Hotel.VIP = true;
            else if (vip_id == "NOTVIP")
                Hotel.VIP = false;
            else
                Hotel.VIP = null;

            if (Photo != null)
            {
                if (Hotel.HotelImage != null)
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "/images/Hotels", Hotel.HotelImage);
                    System.IO.File.Delete(filePath);
                }

                Hotel.HotelImage = ProcessUploadedFile();
            }
            await hservice.CreateHotelAsync(Hotel);
            return RedirectToPage("GetAllHotels");
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Hotels");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
