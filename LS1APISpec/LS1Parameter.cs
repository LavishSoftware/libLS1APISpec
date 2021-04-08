using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libLS1APISpec
{
    public class LS1Parameter : LS1APISpecObject
    {
        public string Type { get; set; }
        public string Constant { get; set; }
        public string Description { get; set; }
        public bool Greedy { get; set; }
        public bool Optional { get; set; }
        public JToken Default { get; set; }
        public ObservableCollection<LS1Parameter> FollowingParameters { get; set; }

        public ObservableCollection<LS1Parameter> ParameterGroup { get; set; }

        public ObservableCollection<string> Enum { get; set; }

        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");
            Type = jo.GetValueString("type");
            Constant = jo.GetValueString("constant");
            Description = jo.GetValueString("description");
            Greedy = jo.GetValueBool("greedy");
            Optional = jo.GetValueBool("optional");
            Default = jo.GetValue("default");

            FollowingParameters = jo.ToObservableCollection<LS1Parameter>("followingParameters");
            ParameterGroup = jo.ToObservableCollection<LS1Parameter>("parameterGroup");
            //LS1Parameter.FromJObject<LS1Parameter>(jo, "followingParameter");


            JArray ja = jo.GetValueJArray("enum");
            if (ja!=null)
            {
                Enum = new ObservableCollection<string>();
                foreach(JToken jt in ja)
                {
                    Enum.Add(jt.ToString());
                }
            }



            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();
            if (!string.IsNullOrEmpty(Name))
                jo.Add("name", Name);

            if (!string.IsNullOrEmpty(Type))
                jo.Add("type", Type);

            if (!string.IsNullOrEmpty(Constant))
                jo.Add("constant", Constant);            

            if (!string.IsNullOrEmpty(Description))
                jo.Add("description", Description);

            if (Greedy)
                jo.Add("greedy", Greedy);
            if (Optional)
                jo.Add("optional", Optional);

            if (Default != null)
                jo.Add("default", Default);
            if (FollowingParameters!=null)
                jo.Add("followingParameter", FollowingParameters);

            if (ParameterGroup != null)
                jo.Add("parameterGroup", ParameterGroup);

            if (Enum!=null)
            {
                JArray ja = new JArray();
                foreach(string s in Enum)
                {
                    ja.Add(s);
                }
                jo.Add("enum", ja);
            }
            return jo;
        }
    }
}
