using HotelExamples.Models;

namespace HotelExamples.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentMethod>> GetAllPaymentMethodsAsync();

        Task<PaymentMethod> GetPaymentMethodFromIdAsync(int id);

    }
}
