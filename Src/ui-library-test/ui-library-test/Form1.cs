using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Management;
using EO.WebBrowser;
namespace ui_library_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        EO.WebBrowser.DOM.Document document;

        private void Form1_Shown(object sender, EventArgs e)
        {
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            EO.WebBrowser.Runtime.DefaultEngineOptions.SetDefaultBrowserOptions(options);
            this.webView1.Create(pictureBox1.Handle);
            string path = @"ui-library-test.txt";
            //string path = @"index.txt";
            string readText = File.ReadAllText(path, Encoding.UTF8);
            webView1.LoadHtml(readText, Application.StartupPath); 
            webView1.RegisterJSExtensionFunction("demoAbout", new JSExtInvokeHandler(WebView_JSDemoAbout));
            webView1.RegisterJSExtensionFunction("receiveInSearch", new JSExtInvokeHandler(WebView_JSReceiveInSearch));
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            webView1.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            document = webView1.GetDOMWindow().document;
            TraverseElementTree(document, (currentElement) =>
            {
                string id = currentElement.GetID();
                if (id.StartsWith("search"))
                {
                    textBox1.Text = currentElement.GetValue();
                }
            });
        }
        private void TraverseElementTree(JSObject root, Action<JSObject> action)
        {
            action(root);
            foreach (var child in root.GetChildren())
                TraverseElementTree(child, action);
        }
        private void webView1_MouseClick(object sender, EO.Base.UI.MouseEventArgs e)
        {
            if (e.Button.ToString() == "Left")
            {
            }
        }
        void WebView_JSDemoAbout(object sender, JSExtInvokeArgs e)
        {
            string browserEngine = e.Arguments[0] as string;
            string search = e.Arguments[1] as string;
            MessageBox.Show("Browser Engine: " + browserEngine + ", Search: " + search);
        }
        void WebView_JSReceiveInSearch(object sender, JSExtInvokeArgs e)
        {
            Task.Run(() => ReceiveInSearch());
        }
        public void ReceiveInSearch()
        {
            try
            {
                document = webView1.GetDOMWindow().document;
                TraverseElementTree(document, (currentElement) =>
                {
                    string id = currentElement.GetID();
                    if (id.StartsWith("search"))
                    {
                        currentElement.SetValue(textBox1.Text);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            document = webView1.GetDOMWindow().document;
            TraverseElementTree(document, (currentElement) =>
            {
                string id = currentElement.GetID();
                if (id.StartsWith("search"))
                {
                    currentElement.SetValue(textBox1.Text);
                }
            });
        }
    }
    public static class JSObjectExtensions
    {
        public static void SetValue(this JSObject jsObj, string value)
        {
            jsObj["value"] = value;
        }
        public static string GetValue(this JSObject jsObj)
        {
            return jsObj["value"] as string ?? string.Empty;
        }

        public static string GetTagName(this JSObject jsObj)
        {
            return (jsObj["tagName"] as string ?? string.Empty).ToUpper();
        }

        public static string GetID(this JSObject jsObj)
        {
            return jsObj["id"] as string ?? string.Empty;
        }

        public static string GetAttribute(this JSObject jsObj, string attribute)
        {
            return jsObj.InvokeFunction("getAttribute", attribute) as string ?? string.Empty;
        }

        public static JSObject GetParent(this JSObject jsObj)
        {
            return jsObj["parentElement"] as JSObject;
        }

        public static IEnumerable<JSObject> GetChildren(this JSObject jsObj)
        {
            var childrenCollection = (JSObject)jsObj["children"];
            int childObjectCount = (int)childrenCollection["length"];
            for (int i = 0; i < childObjectCount; i++)
            {
                yield return (JSObject)childrenCollection[i];
            }
        }
    }
}
