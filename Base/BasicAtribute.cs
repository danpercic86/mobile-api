using System;

namespace itec_mobile_api_final.Base
{
    public class BasicAttribute: Attribute
    {
        public string Name { get; }
        public object Value { get; }

        public BasicAttribute(string name, object value)
        {
            Value = value;
            Name = name;
        }
    }
}