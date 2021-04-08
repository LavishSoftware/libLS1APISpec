using libLS1APISpec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LS1ReferenceExporter
{
    public class ReferenceBuilder
    {
        public ObservableCollection<LS1APISpec> APISpecs = new ObservableCollection<LS1APISpec>();

        public void AddAPISpec(LS1APISpec spec)
        {
            APISpecs.Add(spec);           
        }

        public string Get(bool hide_restricted)
        {
            string output = string.Empty;

            foreach(LS1APISpec spec in APISpecs)
            {
                APISpecEmitter emitter = new APISpecEmitter(spec);
                emitter.HideRestricted = hide_restricted;
                emitter.Go();
                output += emitter.Output;
            }

            return output;
        }
    }
}
