﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libLS1APISpec
{
    public class LS1TypeMemberForm : LS1APISpecObject
    {
        public string Type { get; set; }
        public uint MinimumBuild { get; set; }
        public string Description { get; set; }

        public ObservableCollection<LS1Parameter> Parameters { get; set; }

        public ObservableCollection<LS1Example> Examples { get; set; }

        public override bool FromJObject(JObject jo)
        {
            Type = jo.GetValueString("type");
            MinimumBuild = jo.GetValueUInt("minimumBuild");
            Description = jo.GetValueString("description");
            Parameters = jo.ToObservableCollection<LS1Parameter>("parameters");

            Examples = jo.ToObservableCollection<LS1Example>("examples");

            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            if (!string.IsNullOrEmpty(Type))
                jo.Add("type", Type);

            if (MinimumBuild > 0)
                jo.Add("minimumBuild", MinimumBuild);

            if (!string.IsNullOrEmpty(Description))
                jo.Add("description", Description);

            if (Parameters != null && Parameters.Count > 0)
                jo.Add("parameters", Parameters);

            jo.AddObject("examples", Examples);
            return jo;
        }
    }

    public class LS1TypeMember : LS1APISpecObject
    {
        public ObservableCollection<LS1TypeMemberForm> Forms { get; set; }

        public string Category { get; set; }
        public bool Restricted { get; set; }

        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");
            Forms = jo.ToObservableCollection<LS1TypeMemberForm>("forms");
            Category = jo.GetValueString("category");
            Restricted = jo.GetValueBool("restricted");
            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            if (!string.IsNullOrEmpty(Name))
                jo.Add("name", Name);

            if (!string.IsNullOrEmpty(Category))
                jo.Add("category", Category);

            if (Restricted)
                jo.Add("restricted", Restricted);

            if (Forms != null && Forms.Count > 0)
                jo.Add("forms", Forms);
            return jo;
        }
    }
}
