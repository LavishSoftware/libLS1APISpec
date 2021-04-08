using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libLS1APISpec
{
    public class LS1APISpec : LS1APISpecObject
    {
        public string Description { get; set; }
        public ObservableCollection<LS1Event> Events { get; private set; }
        public ObservableCollection<LS1Type> Types { get; private set; }
        public ObservableCollection<LS1TopLevelObject> TopLevelObjects { get; private set; }
        public ObservableCollection<LS1Command> Commands { get; private set; }

        public static LS1APISpec GetFromFile(string filename)
        {
            JObject jo = Utils.JObjectFromFile(filename);
            if (jo == null)
                return null;

            LS1APISpec spec = new LS1APISpec();
            if (!spec.FromJObject(jo))
                return null;

            return spec;
        }

        public void WriteToFile(string filename)
        {
            JObject jo = GetJObject();
            if (jo == null)
                return;
            Utils.WriteAllTextSafe(filename, jo.ToString(), Encoding.UTF8);
        }

        public override bool FromJObject(JObject jo)
        {
            Name = jo.GetValueString("name");
            Description = jo.GetValueString("description");

            Commands = jo.ToObservableCollection<LS1Command>("commands");
            Events = jo.ToObservableCollection<LS1Event>("events");
            Types = jo.ToObservableCollection<LS1Type>("types");
            TopLevelObjects = jo.ToObservableCollection<LS1TopLevelObject>("topLevelObjects");
            return true;
        }

        public override JObject GetJObject()
        {
            JObject jo = new JObject();
            jo.Add("name", Name);
            jo.Add("description", Description);
            jo.AddObject("commands", Commands);
            jo.AddObject("events", Events);
            jo.AddObject("types", Types);
            jo.AddObject("topLevelObjects", TopLevelObjects);
            return jo;
        }
    }
}
