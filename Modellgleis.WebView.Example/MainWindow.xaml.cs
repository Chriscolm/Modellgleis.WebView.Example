using Microsoft.Web.WebView2.Core;
using Modellgleis.WebView.Example.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Modellgleis.WebView.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //wv.CoreWebView2Ready += (o, e) =>
            //{
            //    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*", CoreWebView2WebResourceContext.Document);
            //    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*.js", CoreWebView2WebResourceContext.Script);
            //    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*.json", CoreWebView2WebResourceContext.Fetch);
            //    wv.CoreWebView2.WebResourceRequested += OnWebResourceRequested;
            //    wv.CoreWebView2.Navigate("http://local.modellgleis.de");
            //};
        }

         

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //await wv.EnsureCoreWebView2Async();
            //wv.CoreWebView2.Navigate("http://local.modellgleis.de");
        }

        private void OnWebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            var uri = new Uri(e.Request.Uri);
            var contentType = e.ResourceContext switch
            {
                CoreWebView2WebResourceContext.Document => "text/html",
                CoreWebView2WebResourceContext.Script => "text/javascript",
                CoreWebView2WebResourceContext.Stylesheet => "test/css",
                CoreWebView2WebResourceContext.Fetch => "application/json",
                _ => string.Empty
            };
             var path = uri.LocalPath;
            if(path.Equals("/", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "/Html/Index.html";
            }
            if (path.EndsWith(".ico"))
            {                
                return;
            }
            var resourceIdentifier = new Uri(@$"pack://application:,,,{path}", UriKind.Absolute);
            var stream = Application.GetResourceStream(resourceIdentifier).Stream;
            
            //var response = wv.CoreWebView2.Environment.CreateWebResourceResponse(stream, 200, "Ok", $"Content-Type: {contentType}");  
            
            //e.Response = response;            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 2D view
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //await wv.EnsureCoreWebView2Async();
            //wv.CoreWebView2.Navigate("https://www.s21-modellgleis.de");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ViewModelHolder();
            //await wv.EnsureCoreWebView2Async();
        }
    }
}
