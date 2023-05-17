using HotelExamples.Interfaces;
using HotelExamples.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data.SqlClient;

namespace HotelExamples.Services
{
    public class HotelService : Connection, IHotelService
    {
        private String queryString = "select * from Hotel";
        private String queryNameString = "select * from Hotel where  Name like @Navn";
        private String queryStringFromID = "select * from Hotel where Hotel_No = @ID";
        private String insertSql = "insert into Hotel Values (@ID, @Navn, @Adresse, @VIP , @HotelType, @Image)";
        private String deleteSql = "delete from Hotel where Hotel_No = @ID";
        private String updateSql = "update Hotel " +
                                   "set Hotel_No= @HotelID, Name=@Navn, Address=@Adresse " +
                                   "where Hotel_No = @ID";
        private String insertPaymentMethodSql = "insert into Hotel_PaymentsMethods Values (@P_id, @Hotel_No)";


        public HotelService(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<bool> CreateHotelPaymentMethodAsync(int payId, int hotelNo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertPaymentMethodSql, connection))
                {
                    command.Parameters.AddWithValue("@P_id", payId);
                    command.Parameters.AddWithValue("@Hotel_No", hotelNo);
                    try
                    {
                        command.Connection.Open();
                        int noOfRows = await command.ExecuteNonQueryAsync(); //bruges ved update, delete, insert

                        if (noOfRows == 1)
                        {
                            return true;
                        }

                        return false;
                    }
                    catch (SqlException sqlex)
                    {
                        Console.WriteLine("Database error");
                        throw sqlex;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel error");
                        throw ex;
                    }
                }

            }
            return false;
        }


        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    if (hotel.VIP == null)
                        command.Parameters.AddWithValue("@VIP", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@VIP", hotel.VIP);

                    if (hotel.HotelImage == null)
                        command.Parameters.AddWithValue("@Image", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Image", hotel.HotelImage);
                    int htypeInt= (int)hotel.HotelType;
                    command.Parameters.AddWithValue("@HotelType",htypeInt );

                    try
                    {
                        command.Connection.Open();
                        int noOfRows = await command.ExecuteNonQueryAsync(); //bruges ved update, delete, insert
                        
                        
                        foreach(PaymentMethod p in hotel.PaymentMethods)
                        {
                            CreateHotelPaymentMethodAsync(p.P_id, hotel.HotelNr);
                        }
                        if (noOfRows == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (SqlException sqlex)
                    {
                        Console.WriteLine("Database error");
                        throw sqlex;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel error");
                        throw ex;
                    }
                }

            }
            return false;
        }

        public async Task<Hotel> DeleteHotelAsync(int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    Hotel hotelToReturn = await GetHotelFromIdAsync(hotelNr);
                    command.Parameters.AddWithValue("@ID", hotelNr);
                    try
                    {
                        command.Connection.Open();
                        int noOfRows = await command.ExecuteNonQueryAsync(); //bruges ved update, delete, insert
                        if (noOfRows == 1)
                        {
                            return hotelToReturn;
                        }

                        return null;
                    }
                    catch (SqlException sqlex)
                    {
                        Console.WriteLine("Database error");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel error");
                    }
                }

            }
            return null;

        }

        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
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
                            int hotelNr = reader.GetInt32(0);
                            String hotelNavn = reader.GetString(1);
                            String hotelAdr = reader.GetString(2);
                            HotelTypes hotelType = (HotelTypes) reader.GetInt32(4);
                            Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr, hotelType); 
                            if (!reader.IsDBNull(3))
                            {
                                hotel.VIP = reader.GetBoolean(3);
                            }
                            
                            if (!reader.IsDBNull(5))
                            {
                                hotel.HotelImage = reader.GetString(5);
                            }
                            hotel.PaymentMethods = await new PaymentMethodService().GetAllPaymentMethodsForHotelAsync(hotelNr);
                            hoteller.Add(hotel);
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
            return hoteller;
        }



        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand commmand = new SqlCommand(queryStringFromID, connection);
                    commmand.Parameters.AddWithValue("@ID", hotelNr);
                    await commmand.Connection.OpenAsync();

                    SqlDataReader reader = await commmand.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int hotelNo = reader.GetInt32(0);
                        string hotelNavn = reader.GetString(1);
                        string hotelAdr = reader.GetString(2);
                        HotelTypes hotelType = (HotelTypes) reader.GetInt32(5);
                        Hotel hotel; 
                        if (!reader.IsDBNull(3))
                        {
                            bool hotelVIP = reader.GetBoolean(3);
                            hotel = new Hotel(hotelNr, hotelNavn, hotelAdr, hotelType, hotelVIP);
                        }
                        else
                            hotel = new Hotel(hotelNo, hotelNavn, hotelAdr, hotelType);
                        return hotel;
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


        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryNameString, connection);
                    string nameWild = "%" + name + "%";
                    command.Parameters.AddWithValue("@Navn", nameWild);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        int hotelnr = reader.GetInt32(0);
                        string hotelNavn = reader.GetString(1);
                        string hotelAdresse = reader.GetString(2);
                        HotelTypes hotelType = (HotelTypes)reader.GetInt32(5);
                        Hotel hotel;
                        if (!reader.IsDBNull(3))
                        {
                            bool hotelVIP = reader.GetBoolean(3);
                            hotel = new Hotel(hotelnr, hotelNavn, hotelAdresse, hotelType, hotelVIP);
                        }
                        else
                            hotel = new Hotel(hotelnr, hotelNavn, hotelAdresse, hotelType);
                        hoteller.Add(hotel);
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Der skete en database fejl! " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Der skete en generel fejl! " + ex.Message);
                }
                return hoteller;
            }
            return null;

        }




        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@HotelID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    command.Parameters.AddWithValue("@ID", hotelNr);
                    try
                    {
                        command.Connection.Open();
                        int noOfRows = await command.ExecuteNonQueryAsync(); //bruges ved update, delete, insert
                        if (noOfRows == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (SqlException sqlex)
                    {
                        Console.WriteLine("Database error");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel error");
                    }
                }
            }
            return false;
        }
    }
}
