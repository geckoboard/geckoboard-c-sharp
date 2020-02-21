using System;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Collections.Generic;
using System.Json;

namespace Geckoboard
{
    public class Field
    {
        public string id;
        public string name;
        public virtual string type
        {
            get { return ""; }
        }

        public Field(string Id, string Name)
        {
            this.id = Id;
            this.name = Name;
        }

        public JsonValue ToJson()
        {
            dynamic json = new ExpandoObject();
            json.name = this.name;
            json.type = this.type;

            json = AddJsonProperties(json);

            return JsonValue.Parse(new JavaScriptSerializer().Serialize(json));
        }

        protected virtual ExpandoObject AddJsonProperties(dynamic json)
        {
            return json;
        }
    }

    public class OptionalField : Field
    {
        public bool optional;

        public OptionalField(string Id, string Name, bool Optional) : base(Id, Name)
        {
            this.optional = Optional;
        }

        protected override ExpandoObject AddJsonProperties(dynamic json)
        {
            json.optional = this.optional;
            return json;
        }
    }

    public class StringField : Field
    {
        public override string type
        {
            get { return "string"; }
        }

        public StringField(string Id, string Name) : base(Id, Name) { }
    }

    public class NumberField : OptionalField
    {
        public override string type
        {
            get { return "number"; }
        }

        public NumberField(string Id, string Name, bool Optional) : base(Id, Name, Optional) { }
    }

    public class DateField : Field
    {
        public override string type
        {
            get { return "date"; }
        }

        public DateField(string Id, string Name) : base(Id, Name) { }
    }

    public class DateTimeField : Field
    {
        public override string type
        {
            get { return "datetime"; }
        }

        public DateTimeField(string Id, string Name) : base(Id, Name) { }
    }

    public class PercentageField : OptionalField
    {
        public override string type
        {
            get { return "percentage"; }
        }

        public PercentageField(string Id, string Name, bool Optional) : base(Id, Name, Optional) { }
    }

    public class MoneyField : OptionalField
    {
        public override string type
        {
            get { return "money"; }
        }
        public string CurrencyCode;

        public MoneyField(string Id, string Name, bool Optional, string CurrencyCode) : base(Id, Name, Optional)
        {
            if (String.IsNullOrEmpty(CurrencyCode))
            {
                throw new ArgumentException("currency_code is a required argument");
            }

            this.CurrencyCode = CurrencyCode;
        }

        protected override ExpandoObject AddJsonProperties(dynamic json)
        {
            json = base.AddJsonProperties((object)json);
            json.currency_code = this.CurrencyCode;
            return json;
        }
    }
}
