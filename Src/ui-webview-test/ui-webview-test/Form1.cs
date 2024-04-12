using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.WinForms;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Wpf;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;

namespace ui_webview_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public async Task Initialize()
        {
            await webView21.EnsureCoreWebView2Async();
        }
        public void LoadHtml(string html)
        {
            webView21.NavigateToString(html);
        }
        public void OpenUrl(string url)
        {
            webView21.Source = new Uri(url);
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            webView21.NavigationStarting += WebView21_NavigationStarting;
            webView21.NavigationCompleted += WebView21_NavigationCompleted;
            webView21.LocationChanged += WebView21_LocationChanged;
            await Initialize();
            string currentPath = Directory.GetCurrentDirectory();
            this.webView21.Source = new System.Uri(currentPath + @"\ui-library-test.html");
            await this.webView21.CoreWebView2.ExecuteScriptAsync(@"
	                document.body.onmouseup = function()
	                {
		                alert('mouse up');
	                }
                ".Replace("\r\n", " "));
            this.webView21.CoreWebView2.AddHostObjectToScript("bridge", new Bridge());
        }
        private void WebView21_LocationChanged(object sender, EventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void WebView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void WebView21_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            int x = 4;
            int y = 10;
            await this.webView21.CoreWebView2.ExecuteScriptAsync($"alert(eval({x}+{y}))");
            await webView21.ExecuteScriptFunctionAsync("JavaScripFunctionName1", new object[] { "msg1", "msg2" });
        }
    }
    public static class Extensions
    {
        public static async Task<string> ExecuteScriptFunctionAsync(this WebView2 webView2, string functionName, params object[] parameters)
        {
            string script = functionName + "(";
            for (int i = 0; i < parameters.Length; i++)
            {
                script += JsonConvert.SerializeObject(parameters[i]);
                if (i < parameters.Length - 1)
                {
                    script += ", ";
                }
            }
            script += ");";
            return await webView2.ExecuteScriptAsync(script);
        }
    }
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Bridge
    {
        public string Func(string param)
        {
            MessageBox.Show(param);
            return param;
        }
    }
}