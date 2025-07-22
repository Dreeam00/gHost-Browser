using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace IFoxer
{
    public class ShortcutLauncherForm : Form
    {
        public ShortcutLauncherForm()
        {
            this.Text = "ショートカットランチャー";
            this.Width = 300;
            this.Height = 400;
            this.TopMost = true;

            var flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            this.Controls.Add(flow);

            // サンプルショートカット
            AddShortcut(flow, "Google", "https://www.google.com");
            AddShortcut(flow, "YouTube", "https://www.youtube.com");
            AddShortcut(flow, "Twitter", "https://twitter.com");
        }

        private void AddShortcut(FlowLayoutPanel panel, string name, string url)
        {
            var btn = new Button();
            btn.Text = name;
            btn.Width = 250;
            btn.Height = 40;
            btn.Click += (s, e) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            panel.Controls.Add(btn);
        }
    }
} 