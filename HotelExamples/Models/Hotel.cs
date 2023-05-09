namespace HotelExamples.Models
{

    
    public class Hotel
    {
        public int HotelNr { get; set; }
        public String Navn { get; set; }
        public String Adresse { get; set; }

        public bool? VIP { get; set; }

        public string HotelImage { get; set; }

        public HotelTypes HotelType { get; set; }
        public Hotel()
        {
        }

        public Hotel(int hotelNr, string navn, string adresse,  HotelTypes hotelType, bool vIP)
        {
            HotelNr = hotelNr;
            Navn = navn;
            Adresse = adresse;
            VIP = vIP;
            HotelType = hotelType;
        }

        public Hotel(int hotelNr, string navn, string adresse, HotelTypes hotelType)
        {
            HotelNr = hotelNr;
            Navn = navn;
            Adresse = adresse;
            HotelType= hotelType;
        }

        public override string ToString()
        {
            return $"{nameof(HotelNr)}: {HotelNr}, {nameof(Navn)}: {Navn}, {nameof(Adresse)}: {Adresse} ";
        }

    }
}
