using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LS1ReferenceExporter
{
    public enum LS1Platform
    {
        ISUplink,
        ISSession,
        JMBUplink,
        JMBSession,
        dxLavish
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class App
    {

        public static void Main()
        {
            APISpecs.Instance.Load();

            BuildPlatformAPISpec(LS1Platform.JMBSession, "JMB-Session.md");
            BuildPlatformAPISpec(LS1Platform.JMBUplink, "JMB-Uplink.md");
            BuildPlatformAPISpec(LS1Platform.ISSession, "IS-Session.md");
            BuildPlatformAPISpec(LS1Platform.ISUplink, "IS-Uplink.md");
        }

        public static string BuildPlatformAPISpec(LS1Platform platform)
        {
            string output = string.Empty;

            switch (platform)
            {
                case LS1Platform.dxLavish:
                    break;
                case LS1Platform.ISUplink:
                    {
                        ReferenceBuilder builder = new ReferenceBuilder();
                        builder.AddAPISpec(APISpecs.Instance.LavishScript);
                        builder.AddAPISpec(APISpecs.Instance.LavishGUI2);
                        builder.AddAPISpec(APISpecs.Instance.IS_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.IS_Uplink);
                        return builder.Get(false);
                    }
                    break;
                case LS1Platform.ISSession:
                    {
                        ReferenceBuilder builder = new ReferenceBuilder();
                        builder.AddAPISpec(APISpecs.Instance.LavishScript);
                        builder.AddAPISpec(APISpecs.Instance.LavishGUI2);
                        builder.AddAPISpec(APISpecs.Instance.IS_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.IS_Session);
                        return builder.Get(false);
                    }
                    break;
                case LS1Platform.JMBUplink:
                    {

                        ReferenceBuilder builder = new ReferenceBuilder();
                        builder.AddAPISpec(APISpecs.Instance.LavishScript);
                        builder.AddAPISpec(APISpecs.Instance.LavishGUI2);
                        builder.AddAPISpec(APISpecs.Instance.IS_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.JMB_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.IS_Uplink);
                        return builder.Get(true);
                    }
                    break;
                case LS1Platform.JMBSession:
                    {
                        ReferenceBuilder builder = new ReferenceBuilder();
                        builder.AddAPISpec(APISpecs.Instance.LavishScript);
                        builder.AddAPISpec(APISpecs.Instance.LavishGUI2);
                        builder.AddAPISpec(APISpecs.Instance.IS_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.JMB_Kernel);
                        builder.AddAPISpec(APISpecs.Instance.IS_Session);
                        builder.AddAPISpec(APISpecs.Instance.JMB_Session);
                        return builder.Get(true);
                    }
                    break;
            }
            throw new NotImplementedException();
        }

        public static bool BuildPlatformAPISpec(LS1Platform platform, string filename)
        {
            string output = BuildPlatformAPISpec(platform);
            if (string.IsNullOrEmpty(output))
                return false;
            System.IO.File.WriteAllText(filename, output);
            return true;
        }
    }
}
