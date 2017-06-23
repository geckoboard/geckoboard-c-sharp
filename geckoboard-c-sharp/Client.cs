using System;
using System.Threading.Tasks;

namespace geckoboardcsharp
{
    public class Client
    {
        private static string URL = "https://api.geckoboard.com";
        private Connection connection;

        public Client(string apiKey)
        {
            connection = new Connection(apiKey);
        }

        public bool ping() {
            return connection.Get(URL).IsSuccessStatusCode;
        }
    }
}
