using System;
using System.Json;
using System.Collections;
using System.Collections.Generic;

namespace Geckoboard
{
    public class Dataset
    {
        public string Id;
        public Dictionary<string, Field> Fields = new Dictionary<string, Field>();
        public DatasetsClient client;

        public Dataset(DatasetsClient client, JsonObject Dataset)
        {
            this.Id = (string)Dataset["id"];
            this.client = client;
            InitialiseFieldsFromJson((JsonObject)Dataset["fields"]);
        }

        public void InitialiseFieldsFromJson(JsonObject fields)
        {
            foreach (var key in fields.Keys)
            {
                var field = fields[key];
                switch((string)field["type"])
                {
                    case "string":
                        Fields.Add(key, new StringField(key, field["name"]));
                        break;

                    case "number":
                        Fields.Add(key, new NumberField(key, field["name"], (bool)field["optional"]));
                        break;

					case "date":
                        Fields.Add(key, new DateField(key, field["name"]));
						break;

					case "datetime":
                        Fields.Add(key, new DateTimeField(key, field["name"]));
						break;

					case "money":
                        Fields.Add(key, new MoneyField(key, field["name"], (bool)field["optional"], field["currency_code"]));
						break;

					case "percentage":
                        Fields.Add(key, new PercentageField(key, field["name"], (bool)field["optional"]));
						break;
                }
            }
        }

        public bool Put(IEnumerable<IDictionary<string, object>> data)
        {
            client.PutData(this, data);

            return true;
        }

        public bool Post(IEnumerable<IDictionary<string, object>> data, string deleteBy)
        {
            client.PostData(this, data);

            return true;
        }
    }
}
