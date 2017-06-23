using System;
using System.Text;
using System.Net.Http;
using System.Json;

namespace geckoboardcsharp
{
    public class Connection
    {
        private HttpClient client;

        public Connection(string apiKey)
        {
            initialiseHttpClient(apiKey);
        }

        public HttpResponseMessage Get(string path)
        {
            return client.GetAsync(path).Result;
        }

        public HttpResponseMessage Delete(string path)
        {
            return client.DeleteAsync(path).Result;
        }

        public HttpResponseMessage Put(string path, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return client.PutAsync(path, content).Result;
        }

        public HttpResponseMessage Post(string path, string body)
        {
			var content = new StringContent(body, Encoding.UTF8, "application/json");
            return client.PostAsync(path, content).Result;
        }

        private void CheckResponseForErrors(HttpResponseMessage response)
        {
            if ((int)response.StatusCode < 400)
                return;

            string errorMessage;
            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            if (IsValidJson(jsonResponse))
            {
                var ParsedResponse = JsonValue.Parse(jsonResponse);
                errorMessage = ParsedResponse["error"]["message"];
            }
            else
            {
				errorMessage = $"\"Server responded with unexpected status code (#{response.StatusCode})\"";
            }

            throw new Exception(errorMessage);
        }

        private bool IsValidJson(string json) {
			try
			{
                var ParsedResponse = JsonValue.Parse(json);
                return true;
			}
			catch (FormatException)
			{
                return false;
			}
        }

        private void initialiseHttpClient(string apiKey)
        {
			client = new HttpClient();
			var byteArray = Encoding.ASCII.GetBytes(apiKey + ":");
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}
