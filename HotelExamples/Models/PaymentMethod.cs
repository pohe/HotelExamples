namespace HotelExamples.Models
{
    public class PaymentMethod
    {
        public int P_id { get; set; }
        public string P_method { get; set; }

        public PaymentMethod(int p_id, string p_method)
        {
            P_id = p_id;
            P_method = p_method;
        }
    }
}
