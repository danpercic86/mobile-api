using System;
using System.ComponentModel;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Helpers
{
    public static class ReflectionHelper
    {
        public static Entity PatchObject(Entity car, dynamic car1)
        {
            foreach (var prop in car.GetType().GetProperties())
            {
                try
                {
                    var attrs = prop.GetCustomAttributes(typeof(ReadOnlyAttribute), true);
                    if (attrs.Length > 0)
                    {
                        continue;
                    }

                    var target = prop.Name[0].ToString().ToLower() + prop.Name.Substring(1);
                    var z = JsonConvert.DeserializeObject(Convert.ToString(car1));
                    var pn = (string) z[target];
                    if (pn is null) continue;
                    Type t = prop.PropertyType;
                    var value = Convert.ChangeType(pn, t);
                    car.GetType().GetProperty(prop.Name)?.SetValue(car, value, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return car;
        }
    }
}