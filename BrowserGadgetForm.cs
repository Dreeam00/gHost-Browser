using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace IFoxer
{
    public class BrowserGadgetForm : Form
    {
        private WebView2 webView;

        public BrowserGadgetForm()
        {
            this.Text = "ブラウザガジェット";
            this.Width = 400;
            this.Height = 300;
            this.TopMost = true;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            this.Controls.Add(webView);
        }
    }
} 