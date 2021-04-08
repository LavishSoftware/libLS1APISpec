using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace libLS1APISpec
{
    public abstract class LS1APISpecObject
    {
        public abstract bool FromJObject(JObject jo);
        public abstract JObject GetJObject();

        public string Name { get; set; }

        public static JObject GetJObject(LS1APISpecObject item)
        {
            if (item == null)
                return null;
            return item.GetJObject();
        }

        public static T FromJObject<T>(JObject obj) where T : LS1APISpecObject, new()
        {
            if (obj == null)
                return null;
            T item = new T();
            item.FromJObject(obj);
            return item;
        }

        public static T FromJObject<T>(JObject parentObject, string name) where T : LS1APISpecObject, new()
        {
            if (parentObject == null)
                return null;

            JObject obj = parentObject.GetValueJObject(name);
            if (obj == null)
                return null;

            T item = new T();
            item.FromJObject(obj);
            return item;
        }
    }
}
