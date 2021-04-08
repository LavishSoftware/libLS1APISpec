using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LS1ReferenceExporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonJMBSession_Click(object sender, RoutedEventArgs e)
        {
            App.BuildPlatformAPISpec(LS1Platform.JMBSession, "LS1APISpec-JMB-Session.md");
        }

        private void buttonJMBUplink_Click(object sender, RoutedEventArgs e)
        {
            App.BuildPlatformAPISpec(LS1Platform.JMBUplink, "LS1APISpec-JMB-Uplink.md");
        }

        private void buttonISSession_Click(object sender, RoutedEventArgs e)
        {
            App.BuildPlatformAPISpec(LS1Platform.ISSession, "LS1APISpec-IS-Session.md");
        }

        private void buttonISUplink_Click(object sender, RoutedEventArgs e)
        {
            App.BuildPlatformAPISpec(LS1Platform.ISUplink, "LS1APISpec-IS-Uplink.md");
        }
    }
}
