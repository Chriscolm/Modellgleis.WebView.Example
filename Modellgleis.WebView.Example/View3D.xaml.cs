using Microsoft.Web.WebView2.Core;
using Modellgleis.WebView.Example.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// Interaktionslogik für View3D.xaml
    /// </summary>
    public partial class View3D : UserControl
    {
        public View3D()
        {
            InitializeComponent();

            var b0 = wv.CanGoBack;
            wv.GoBack();
            var b1 = wv.CanGoForward;
            wv.GoForward();            
            //wv.Reload();
            //wv.Stop();
            //wv.NavigateToString("<html><Body>Ich bin eine ganz tolle Webseite</body></html>");
            //wv.ExecuteScriptAsync("alert('Ich bin ein Javascript');");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            wv.CoreWebView2Ready += (o, e) =>
            {
                if(DataContext is ViewModel3D view)
                {
                    view.IsViewReady = true;
                    // verodern funzt.NET, ergibt CoreWebView2WebResourceContext.CspViolationReport für context
                    // var context = CoreWebView2WebResourceContext.Document | CoreWebView2WebResourceContext.Script | CoreWebView2WebResourceContext.Fetch;
                    // wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*", context);

                    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*", CoreWebView2WebResourceContext.Document);
                    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*.js", CoreWebView2WebResourceContext.Script);
                    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*.json", CoreWebView2WebResourceContext.Fetch);
                    wv.CoreWebView2.AddWebResourceRequestedFilter("*local.modellgleis*.json", CoreWebView2WebResourceContext.XmlHttpRequest);
                    wv.CoreWebView2.WebResourceRequested += OnWebResourceRequested;
                    view.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == nameof(view.ViewSource))
                        {
                            wv.CoreWebView2.Navigate(view.ViewSource);
                        }
                    };
                }
            };
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
                CoreWebView2WebResourceContext.XmlHttpRequest => "application/json",
                _ => string.Empty
            };
            var path = uri.LocalPath;
            if (path.Equals("/", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "/Html/Index.html";
            }
            if (path.EndsWith(".ico", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            //var resourceIdentifier = new Uri(@$"pack://application:,,,{path}", UriKind.Absolute);
            //var stream = Application.GetResourceStream(resourceIdentifier).Stream;
            var stream = GetContent(e.ResourceContext, path);

            var response = wv.CoreWebView2.Environment.CreateWebResourceResponse(stream, 200, "Ok", $"Content-Type: {contentType}");

            e.Response = response;
        }

        private Stream GetContent(CoreWebView2WebResourceContext context, string path)
        {
            if(context == CoreWebView2WebResourceContext.Fetch || context == CoreWebView2WebResourceContext.XmlHttpRequest)
            {
                return GetDataStream(path);
            }
            else
            {
                return GetResourceStream(path);
            }
        }

        private Stream GetResourceStream(string path)
        {
            var resourceIdentifier = new Uri(@$"pack://application:,,,{path}", UriKind.Absolute);
            var stream = Application.GetResourceStream(resourceIdentifier).Stream;
            return stream;
        }

        private Stream GetDataStream(string path)
        {
            var o = new { Element = "Qube", Width = 1, Height = 1, Depth = 1 };
            var json = JsonSerializer.Serialize(o);
            var bytes = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(bytes);
        }
    }
}
