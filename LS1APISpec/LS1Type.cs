using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;

namespace libLS1APISpec
{
    public class LS1TypeAsString : LS1APISpecObject
    {
        public string Constant { get; set; }

        public string Member { get; set; }

        public string Description { get; set; }

        public ObservableCollection<LS1Example> Examples { get; set; }


        public override bool FromJObject(JObject jo)
        {
            Constant = jo.GetValueString("constant");
            Member = jo.GetValueString("member");
            Description = jo.GetValueString("description");
            Examples = jo.ToObservableCollection<LS1Example>("examples");

            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            if (!string.IsNullOrEmpty(Constant))
                jo.Add("constant", Constant);

            if (!string.IsNullOrEmpty(Member))
                jo.Add("member", Member);

            if (!string.IsNullOrEmpty(Description))
                jo.Add("description", Description);

            if (Examples!=null)
                jo.AddObject("examples", Examples);

            return jo;
        }
    }

    public class LS1Type : LS1APISpecObject
    {
        public string Description { get; set; }
        public string BaseType { get; set; }

        public string VariableType { get; set; }

        public bool UsesSubType { get; set; }

        public ObservableCollection<LS1TypeMember> Members { get; set; }

        public ObservableCollection<LS1TypeMethod> Methods { get; set; }

        public ObservableCollection<LS1TypeMember> StaticMembers { get; set; }

        public ObservableCollection<LS1TypeMethod> StaticMethods { get; set; }

        public LS1TypeIndex Index { get; set; }

        public LS1TypeInitializer Initializer { get; set; }

        public bool Persistent { get; set; }
        public bool Restricted { get; set; }
        public uint MinimumBuild { get; set; }
        public string Category { get; set; }

        public LS1TypeAsString AsString { get; set; }


        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");
            Description = jo.GetValueString("description");
            BaseType = jo.GetValueString("baseType");
            VariableType = jo.GetValueString("variableType");
            UsesSubType = jo.GetValueBool("usesSubType");
            Members = jo.ToObservableCollection<LS1TypeMember>("members");
            Methods = jo.ToObservableCollection<LS1TypeMethod>("methods");
            StaticMembers = jo.ToObservableCollection<LS1TypeMember>("staticMembers");
            StaticMethods = jo.ToObservableCollection<LS1TypeMethod>("staticMethods");
            Index = LS1TypeIndex.FromJObject<LS1TypeIndex>(jo,"index");

            if (Members != null)
            {
                foreach (LS1TypeMember m in Members)
                    m.Static = false;
            }
            if (StaticMembers != null)
            {
                foreach (LS1TypeMember m in StaticMembers)
                    m.Static = true;
            }

            if (Methods != null)
            {
                foreach (LS1TypeMethod m in Methods)
                    m.Static = false;
            }
            if (StaticMethods != null)
            {
                foreach (LS1TypeMethod m in StaticMethods)
                    m.Static = true;
            }

            Initializer = LS1TypeInitializer.FromJObject<LS1TypeInitializer>(jo, "initializer");

            Persistent = jo.GetValueBool("persistent",true);
            Restricted = jo.GetValueBool("restricted");
            MinimumBuild = jo.GetValueUInt("minimumBuild");
            Category = jo.GetValueString("category");

            AsString = LS1TypeAsString.FromJObject<LS1TypeAsString>(jo, "asString");

            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();

            jo.Add("name", Name);

            if (!string.IsNullOrEmpty(Description))
                jo.Add("description", Description);

            if (!string.IsNullOrEmpty(VariableType))
                jo.Add("variableType", VariableType);

            if (!string.IsNullOrEmpty(BaseType))
                jo.Add("baseType", BaseType);

            if (UsesSubType)
                jo.Add("usesSubType", UsesSubType);

            if (Members != null && Members.Count > 0)
                jo.AddObject("members", Members);

            if (Methods != null && Methods.Count > 0)
                jo.AddObject("methods", Methods);

            if (StaticMembers != null && StaticMembers.Count > 0)
                jo.AddObject("staticMembers", StaticMembers);

            if (StaticMethods != null && StaticMethods.Count > 0)
                jo.AddObject("staticMethods", StaticMethods);

            if (Index != null)
                jo.Add("index", Index);

            if (Initializer != null)
                jo.Add("initializer", Initializer);

            if (!Persistent)
                jo.Add("persistent", Persistent);

            if (Restricted)
                jo.Add("restricted", Restricted);

            if (MinimumBuild > 0)
                jo.Add("minimumBuild", MinimumBuild);

            if (!string.IsNullOrEmpty(Category))
                jo.Add("category", Category);

            if (AsString != null)
                jo.Add("asString", AsString);

            return jo;
        }
    }
}
