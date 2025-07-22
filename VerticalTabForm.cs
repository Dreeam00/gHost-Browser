using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace IFoxer
{
    public class VerticalTabForm : Form
    {
        private TabControl tabControl;
        private WebView2 webView;

        public VerticalTabForm()
        {
            this.Text = "縦タブモード";
            this.Width = 900;
            this.Height = 700;

            tabControl = new TabControl();
            tabControl.Alignment = TabAlignment.Left;
            tabControl.Dock = DockStyle.Left;
            tabControl.Width = 150;
            tabControl.Multiline = true;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;

            this.Controls.Add(webView);
            this.Controls.Add(tabControl);
        }
    }
} 