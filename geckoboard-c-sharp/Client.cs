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

        public bool Ping() {
            return connection.Get("").IsSuccessStatusCode;
        }

        public DatasetsClient Datasets() {
            return new DatasetsClient(connection);
        }
    }
}
