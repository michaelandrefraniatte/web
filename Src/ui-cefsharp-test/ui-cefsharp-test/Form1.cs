using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;

namespace ui_cefsharp_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public ChromiumWebBrowser chromeBrowser;
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Environment.CurrentDirectory,
                    hostName: "cefsharp",
                    defaultPage: "ui-library-test.html"
                )
            });
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser("localfolder://cefsharp/");
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.WindowlessFrameRate = 21;
            chromeBrowser.BrowserSettings = browserSettings;
            this.pictureBox1.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;
            chromeBrowser.JavascriptMessageReceived += OnBrowserJavascriptMessageReceived;
            chromeBrowser.FrameLoadEnd += OnFrameLoadEnd;
            chromeBrowser.LoadingStateChanged += OnLoadingStateChanged;
            chromeBrowser.AddressChanged += OnAddressChanged;
        }
        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            chromeBrowser.ShowDevTools();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeChromium();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                chromeBrowser.ExecuteScriptAsync(@"
	                document.body.onmouseup = function()
	                {
		                alert('mouse up');
	                }
	           ");
            }
        }
        private void OnAddressChanged(object s, AddressChangedEventArgs e)
        {
            if (e.Address != null)
            {
            }
        }
        private async void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                string HTML = await chromeBrowser.GetSourceAsync();
                MessageBox.Show("loaded");
                MessageBox.Show(HTML);
            }
        }
        private void OnBrowserJavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            var msg = e.ConvertMessageTo<PostMessageExample>();
            var callback = (IJavascriptCallback)msg.Callback;
            var type = msg.Type;
            var property = msg.Data.Property;
            callback.ExecuteAsync(type);
            MessageBox.Show(type + ", " + property);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            chromeBrowser.ExecuteScriptAsync("JavaScripFunctionName1", new object[] { "msg1", "msg2" });
        }
    }
    public class PostMessageExample
    {
        public string Type { get; set; }
        public PostMessageExampleData Data { get; set; }
        public IJavascriptCallback Callback { get; set; }
    }
    public class PostMessageExampleData
    {
        public string Property { get; set; }
    }
}