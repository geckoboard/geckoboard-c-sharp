using System;
using System.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Globalization;

namespace geckoboardcsharp
{
    public class DatasetsClient
    {
        public Connection connection;

        public DatasetsClient(Connection connection)
        {
            this.connection = connection;
        }

        public Dataset FindOrCreate(string datasetId, IEnumerable<Field> fields)
        {
            return FindOrCreate(datasetId, fields, null);
        }

        public Dataset FindOrCreate(string datasetId, IEnumerable<Field> fields, string uniqueBy)
        {
            string path = "/datasets/" + Uri.EscapeDataString(datasetId);
            JsonObject json = new JsonObject();
            JsonObject jsonFields = new JsonObject();

            foreach (var field in fields)
            {
                Console.WriteLine(field.Id);
                jsonFields[field.Id] = field.ToJson();
            }

            json["fields"] = jsonFields;

            if (!String.IsNullOrEmpty(uniqueBy))
            {
                json["unique_by"] = uniqueBy;
            }

            var response = connection.Put(path, json.ToString());

            return new Dataset(this, (JsonObject)JsonValue.Parse(response.Content.ReadAsStringAsync().Result));           
        }

        public bool Delete(string datasetId)
        {
            string path = "/datasets/" + Uri.EscapeDataString(datasetId);
            connection.Delete(path);

            return true;
        }

        public bool PutData(Dataset dataset, List<IDictionary<string, object>> data)
        {
            string path = "/datasets/" + Uri.EscapeDataString(dataset.Id) + "/data";

            connection.Put(path, FormatData(dataset, data));

            return true;
        }

        public bool PostData(Dataset dataset, List<IDictionary<string, object>> data)
        {
            return PostData(dataset, data, null);
        }

        public bool PostData(Dataset dataset, List<IDictionary<string, object>> data, string deleteBy)
        {
            string path = "/datasets/" + Uri.EscapeUriString(dataset.Id) + "/data";

            connection.Post(path, FormatData(dataset, data, deleteBy));

            return true;
        }

        public string FormatData(Dataset dataset, List<IDictionary<string, object>> data)
        {
            return FormatData(dataset, data, null);
        }

        public string FormatData(Dataset dataset, List<IDictionary<string, object>> data, string deleteBy)
        {
            JsonArray jsonArray = new JsonArray();

            Console.WriteLine("Starting Data Formatting");

            foreach (var dataPoint in data)
            {
                JsonObject json = new JsonObject();

                Console.WriteLine("Data Point: " + dataPoint.ToString());
                foreach (var key in dataPoint.Keys)
                {
                    switch (dataset.Fields[key].Type)
                    {
                        case "date":
                            Console.WriteLine(key + " is a " + dataset.Fields[key].Type);
                            DateTime date = (DateTime)dataPoint[key];
                            json[dataset.Fields[key].Id] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            break;
                        case "datetime":
                            Console.WriteLine(key + " is a " + dataset.Fields[key].Type);
                            DateTime datetime = (DateTime)dataPoint[key];
                            json[dataset.Fields[key].Id] = datetime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                            break;
                        case "number":
                        case "money":
                            Console.WriteLine(key + " is a " + dataset.Fields[key].Type);
                            json[dataset.Fields[key].Id] = (int)dataPoint[key];
                            break;
						case "string":
                            Console.WriteLine(key + " is a " + dataset.Fields[key].Type);
                            json[dataset.Fields[key].Id] = (string)dataPoint[key];
							break;
						case "percentage":
                            Console.WriteLine(key + " is a " + dataset.Fields[key].Type);
                            json[dataset.Fields[key].Id] = (double)dataPoint[key];
							break;
                    }
                }

                jsonArray.Add(json);
            }

            var wrapper = new JsonObject();
            wrapper["data"] = jsonArray;

            if (!String.IsNullOrEmpty(deleteBy))
            {
                wrapper["delete_by"] = deleteBy;
            }

			Console.WriteLine("Formatted Data: " + wrapper.ToString());

            return wrapper.ToString();
        }
    }
}
