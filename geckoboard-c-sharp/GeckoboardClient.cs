using System;
using System.Threading.Tasks;

namespace Geckoboard
{
    public class GeckoboardClient
    {
        private Connection connection;

        public GeckoboardClient(string apiKey)
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
