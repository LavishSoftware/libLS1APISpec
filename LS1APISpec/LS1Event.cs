using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libLS1APISpec
{
    public class LS1EventContext : LS1APISpecObject
    {
        public string Type { get; set; }
        public string Description { get; set; }


        public override bool FromJObject(JObject jo)
        {
            Type = jo.GetValueString("type");
            Description = jo.GetValueString("description");
            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();
            jo.Add("type", Type);
            jo.Add("description", Description);
            return jo;
        }

    }

    public class LS1Event : LS1APISpecObject
    {
        public string Description { get; set; }

        public ObservableCollection<LS1Parameter> Parameters { get; private set; }

        public LS1EventContext Context { get; set; }

        public bool Restricted { get; set; }
        public uint MinimumBuild { get; set; }
        public string Category { get; set; }

        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");
            Description = jo.GetValueString("description");

            Parameters = jo.ToObservableCollection<LS1Parameter>("parameters");
            Context = LS1EventContext.FromJObject<LS1EventContext>(jo, "context");
            Restricted = jo.GetValueBool("restricted");
            MinimumBuild = jo.GetValueUInt("minimumBuild");
            Category = jo.GetValueString("category");
            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            jo.Add("name", Name);
            jo.Add("description", Description);
            if (Parameters!=null && Parameters.Count>0)
                jo.Add("parameters", Parameters);

            if (Context!=null)
                jo.Add("context", Context);

            if (Restricted)
                jo.Add("restricted", Restricted);

            if (MinimumBuild>0)
                jo.Add("minimumBuild", MinimumBuild);

            if (!string.IsNullOrEmpty(Category))
                jo.Add("category", Category);
            return jo;
        }
    }
}
