using System;
using System.Collections.Generic;
using System.Text;
using libLS1APISpec;

namespace LS1ReferenceExporter
{
    public class APISpecs
    {
        protected APISpecs()
        {

        }

        public LS1APISpec LavishScript { get; private set; }

        public LS1APISpec LavishGUI2 { get; private set; }

        public LS1APISpec IS_Uplink { get; private set; }
        public LS1APISpec IS_Kernel { get; private set; }
        public LS1APISpec IS_Session { get; private set; }

        public LS1APISpec JMB_Kernel { get; private set; }
        public LS1APISpec JMB_Session { get; private set; }

        public string SpecFolder { get; private set; }

        LS1APISpec ReadSpec(string filename)
        {
            return LS1APISpec.GetFromFile(System.IO.Path.Combine(SpecFolder,filename));
        }

        public void Load()
        {
            //SpecFolder = "D:\\Lavish\\vscode\\apispec";
            SpecFolder = ".";

            LavishScript = ReadSpec("LavishScript.json");
            LavishGUI2 = ReadSpec("LavishGUI2.json");
            IS_Uplink = ReadSpec("IS-Uplink.json");
            IS_Kernel = ReadSpec("IS-Kernel.json");
            IS_Session = ReadSpec("IS-Session.json");
            JMB_Kernel = ReadSpec("JMB-Kernel.json");
            JMB_Session = ReadSpec("JMB-Session.json");
        }

        static APISpecs _Instance;
        static public APISpecs Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new APISpecs();
                return _Instance;
            }
        }
    }
}
