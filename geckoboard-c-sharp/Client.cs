using System;
using System.Threading.Tasks;

namespace geckoboardcsharp
{
    public class Client
    {
        private Connection connection;

        public Client(string apiKey)
        {
            connection = new Connection(apiKey);
        }

        public bool ping() {
            return connection.Get("").IsSuccessStatusCode;
        }

        public DatasetsClient datasets() {
            return new DatasetsClient(connection);
        }
    }
}
