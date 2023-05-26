namespace HotelExamples.Services
{
    public static class Secret
    {
        private static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelExamples23;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private static string _connectionString = "";
        public static string ConnectionString
        {
            get { return _connectionString; }

        }

    }

}
