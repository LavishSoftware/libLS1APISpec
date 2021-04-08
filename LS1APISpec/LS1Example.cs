using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libLS1APISpec
{
    public class LS1Example : LS1APISpecObject
    {
        public string Description { get; set; }

        public ObservableCollection<string> Code { get; private set; }

        public ObservableCollection<string> Result { get; private set; }

        ObservableCollection<string> GetOneOrMoreStrings(JObject jo, string name)
        {
            JArray ja = jo.GetValueJArray(name);
            if (ja != null)
            {
                // todo
                ObservableCollection<string> res = new ObservableCollection<string>();
                for (int i = 0; i < ja.Count; i++)
                {
                    res.Add(ja.GetStringAt(i));
                }
                return res;

            }
            else
            {
                string code = jo.GetValueString("code");
                if (!string.IsNullOrEmpty(code))
                {
                    ObservableCollection<string> res = new ObservableCollection<string>();
                    res.Add(code);
                    return res;
                }
            }

            return null;
        }

        JToken StringsToJSON(ObservableCollection<string> list)
        {
            if (list == null || list.Count==0)
                return null;

            if (list.Count == 1)
                return list[0];

            JArray ja = new JArray();
            foreach (string s in list)
                ja.Add(s);
            return ja;
        }

        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");

            Description = jo.GetValueString("description");

            Code = GetOneOrMoreStrings(jo,"code");
            Result = GetOneOrMoreStrings(jo,"result");

            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            if (!string.IsNullOrEmpty(Name))
                jo.Add("name", Name);

            if (!string.IsNullOrEmpty(Description))
                jo.Add("description", Description);

            JToken strings = StringsToJSON(Code);
            if (strings != null)
                jo.Add("code", strings);

            strings = StringsToJSON(Result);
            if (strings != null)
                jo.Add("result", strings);

            return jo;            
        }
    }
}
