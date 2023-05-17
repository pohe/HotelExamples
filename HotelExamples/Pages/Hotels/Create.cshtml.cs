using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace HotelExamples.Pages.Hotels
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Hotel Hotel { get; set; }

        private IHotelService hservice;
        private IPaymentService pService;
        
        public List<PaymentMethod> PaymentMethods { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public List<int> AreChecked { get; set; }

        [BindProperty]
        public int SelectedPaymentMethod { get; set; }

        private IWebHostEnvironment webHostEnvironment;

        public CreateModel(IHotelService hotelService, IPaymentService paymentService, IWebHostEnvironment webHost)
        {
            hservice = hotelService;
            pService = paymentService;
            webHostEnvironment = webHost;
        }

       
        public async Task<IActionResult> OnGetAsync()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email == null || email == "")
            {
                return RedirectToPage("/Users/Login");
            }
            PaymentMethods = await pService.GetAllPaymentMethodsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string vip_id)
        {
            
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
            List <int> pIds= new List<int>();
            pIds.AddRange(AreChecked);
            
            foreach (int id in pIds)
            {
                PaymentMethod p = await pService.GetPaymentMethodFromIdAsync(id);
                Hotel.PaymentMethods.Add(p);
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
