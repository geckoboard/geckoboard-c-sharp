using System;
using System.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Geckoboard
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
            return FindOrCreate(datasetId, fields, new string[0]);
        }

        public class Format
        {
            public string[] unique_by { get; set; }
            public Dictionary<string, Field> fields { get; set; }
        }

        public Dataset FindOrCreate(string datasetId, IEnumerable<Field> fields, string[] uniqueBy)
        {
            string path = "/datasets/" + Uri.EscapeDataString(datasetId);
            var format = new Format();
            var jsonFields = new Dictionary<string, Field>();

            foreach (var field in fields)
            {
                jsonFields.Add(field.name.ToLowerInvariant(), field);
            }

            format.fields = jsonFields;

            if (uniqueBy.Length > 0)
            {
                format.unique_by = uniqueBy;
            }

            var response = connection.Put(path, new JavaScriptSerializer().Serialize(format));

            return new Dataset(this, (JsonObject)JsonValue.Parse(response.Content.ReadAsStringAsync().Result));
        }

        public bool Delete(string datasetId)
        {
            string path = "/datasets/" + Uri.EscapeDataString(datasetId);
            connection.Delete(path);

            return true;
        }

        public bool PutData(Dataset dataset, IEnumerable<IDictionary<string, object>> data)
        {
            string path = "/datasets/" + Uri.EscapeDataString(dataset.Id) + "/data";

            connection.Put(path, FormatData(dataset, data));

            return true;
        }

        public bool PostData(Dataset dataset, IEnumerable<IDictionary<string, object>> data)
        {
            return PostData(dataset, data, null);
        }

        public bool PostData(Dataset dataset, IEnumerable<IDictionary<string, object>> data, string deleteBy)
        {
            string path = "/datasets/" + Uri.EscapeUriString(dataset.Id) + "/data";

            connection.Post(path, FormatData(dataset, data, deleteBy));

            return true;
        }

        public string FormatData(Dataset dataset, IEnumerable<IDictionary<string, object>> data)
        {
            return FormatData(dataset, data, null);
        }

        public string FormatData(Dataset dataset, IEnumerable<IDictionary<string, object>> data, string deleteBy)
        {
            JsonArray jsonArray = new JsonArray();

            foreach (var dataPoint in data)
            {
                JsonObject json = new JsonObject();

                foreach (var key in dataPoint.Keys)
                {
                    switch (dataset.Fields[key].type)
                    {
                        case "date":
                            DateTime date = (DateTime)dataPoint[key];
                            json[dataset.Fields[key].id] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            break;
                        case "datetime":
                            DateTime datetime = (DateTime)dataPoint[key];
                            json[dataset.Fields[key].id] = datetime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                            break;
                        case "number":
                        case "money":
                            json[dataset.Fields[key].id] = (int)dataPoint[key];
                            break;
                        case "string":
                            json[dataset.Fields[key].id] = (string)dataPoint[key];
                            break;
                        case "percentage":
                            json[dataset.Fields[key].id] = (double)dataPoint[key];
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

            return wrapper.ToString();
        }
    }

    public static class DatasetClientExtensions
    {
        public static string ToValidDatasetName(this string name)
        {
            var replaceSpaces = name.ToLowerInvariant().Replace(" ", "_");
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            var result = rgx.Replace(replaceSpaces, "_");
            return result;
        }
    }
}
