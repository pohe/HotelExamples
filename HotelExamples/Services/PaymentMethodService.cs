using HotelExamples.Interfaces;
using HotelExamples.Models;
using System.Data.SqlClient;
using System.Windows.Input;

namespace HotelExamples.Services
{
    public class PaymentMethodService : Connection, IPaymentService 
    {
        private String queryString = "select * from paymentmethods";
        private string queryStringForHotelNo = "select paymentmethods.P_id, paymentmethods.P_method from paymentmethods, Hotel_PaymentsMethods where paymentmethods.P_id = Hotel_PaymentsMethods.P_id AND Hotel_PaymentsMethods.Hotel_No = @HotelNO";
        private String queryStringFromID = "select * from paymentmethods where P_id = @ID";

        public PaymentMethodService(IConfiguration configuration) : base(configuration)
        {

        }
        public PaymentMethodService() : base()
        {

        }

        public async Task<List<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            List<PaymentMethod> p_methods = new List<PaymentMethod>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        await command.Connection.OpenAsync();//aSynkront
                        SqlDataReader reader = await command.ExecuteReaderAsync();//aSynkront
                        while (await reader.ReadAsync())
                        {
                            int p_id = reader.GetInt32(0);
                            String p_method = reader.GetString(1);
                            PaymentMethod pm = new PaymentMethod(p_id, p_method);
                            p_methods.Add(pm);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine("Database error " + sqlEx.Message);
                        throw sqlEx;
                        //return null;
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Generel fejl" + exp.Message);
                        throw exp;
                        //return null;
                    }
                }
            }
            return p_methods;
        }


        public async Task<List<PaymentMethod>> GetAllPaymentMethodsForHotelAsync(int hotelNo)
        {
            List<PaymentMethod> p_methods = new List<PaymentMethod>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryStringForHotelNo, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@HotelNO", hotelNo);
                        await command.Connection.OpenAsync();//aSynkront
                        SqlDataReader reader = await command.ExecuteReaderAsync();//aSynkront
                        while (await reader.ReadAsync())
                        {
                            int p_id = reader.GetInt32(0);
                            String p_method = reader.GetString(1);
                            PaymentMethod pm = new PaymentMethod(p_id, p_method);
                            p_methods.Add(pm);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine("Database error " + sqlEx.Message);
                        throw sqlEx;
                        //return null;
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Generel fejl" + exp.Message);
                        throw exp;
                        //return null;
                    }
                }
            }
            return p_methods;
        }

        public async Task<PaymentMethod> GetPaymentMethodFromIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryStringFromID, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int P_id = reader.GetInt32(0);
                        string P_method = reader.GetString(1);
                        PaymentMethod pm = new PaymentMethod(P_id, P_method);
                        return pm;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                }
                finally
                {
                    //her kommer man altid
                }
            }
            return null;
        }
    }
}
