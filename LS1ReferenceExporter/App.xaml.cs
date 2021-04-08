using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            APISpecs.Instance.Load();
            base.OnStartup(e);
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
