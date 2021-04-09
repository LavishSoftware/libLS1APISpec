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

        LS1APISpec DownloadSpec(string filename)
        {
            return LS1APISpec.GetFromURL(string.Format("https://raw.githubusercontent.com/LavishSoftware/LS1-Platform-Specifications/master/{0}",filename));
        }

        public void Load()
        {
            //SpecFolder = "D:\\Lavish\\vscode\\apispec";
            SpecFolder = ".";

            LavishScript = DownloadSpec("LavishScript.json");
            LavishGUI2 = DownloadSpec("LavishGUI2.json");
            IS_Uplink = DownloadSpec("IS-Uplink.json");
            IS_Kernel = DownloadSpec("IS-Kernel.json");
            IS_Session = DownloadSpec("IS-Session.json");
            JMB_Kernel = DownloadSpec("JMB-Kernel.json");
            JMB_Session = DownloadSpec("JMB-Session.json");
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
