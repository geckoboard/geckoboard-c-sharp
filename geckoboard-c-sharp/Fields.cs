using System;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Collections.Generic;
using System.Json;

namespace Geckoboard
{
    public class Field
    {
        public string Id;
        public string Name;
        public virtual string Type
        {
            get { return ""; }
        }

        public Field(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public JsonValue ToJson()
        {
            dynamic json = new ExpandoObject();
            json.name = this.Name;
            json.type = this.Type;

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
        public bool Optional;

        public OptionalField(string Id, string Name, bool Optional) : base(Id, Name)
        {
            this.Optional = Optional;
        }

        protected override ExpandoObject AddJsonProperties(dynamic json)
        {
            json.optional = this.Optional;
            return json;
        }
    }

    public class StringField : Field
    {
        public override string Type
        {
            get { return "string"; }
        }

        public StringField(string Id, string Name) : base(Id, Name) { }
    }

    public class NumberField : OptionalField
    {
        public override string Type
        {
            get { return "number"; }
        }

        public NumberField(string Id, string Name, bool Optional) : base(Id, Name, Optional) { }
    }

    public class DateField : Field
    {
        public override string Type
        {
            get { return "date"; }
        }

        public DateField(string Id, string Name) : base(Id, Name) { }
    }

    public class DateTimeField : Field
    {
        public override string Type
        {
            get { return "datetime"; }
        }

        public DateTimeField(string Id, string Name) : base(Id, Name) { }
    }

    public class PercentageField : OptionalField
    {
        public override string Type
        {
            get { return "percentage"; }
        }

        public PercentageField(string Id, string Name, bool Optional) : base(Id, Name, Optional) { }
    }

    public class MoneyField : OptionalField
    {
        public override string Type
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
