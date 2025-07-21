namespace IFoxer
{
    partial class Form1
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnNavigate;
        private System.Windows.Forms.Button btnAddTab;
        private System.Windows.Forms.Button btnRemoveTab;
        private System.Windows.Forms.Button btnToggleMode;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TabControl tabControl1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnBookmark;
        private System.Windows.Forms.Button btnBookmarkList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;

        private void InitializeComponent()
        {
            panel1 = new Panel();
            button1 = new Button();
            btnSettings = new Button();
            btnBack = new Button();
            btnForward = new Button();
            btnReload = new Button();
            btnNavigate = new Button();
            txtUrl = new TextBox();
            panel2 = new Panel();
            btnAddTab = new Button();
            btnRemoveTab = new Button();
            tabControl1 = new TabControl();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            richTextBox1 = new RichTextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            btnBookmark = new Button();
            btnBookmarkList = new Button();
            this.imageList1 = new System.Windows.Forms.ImageList();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnSettings);
            panel1.Controls.Add(btnBack);
            panel1.Controls.Add(btnForward);
            panel1.Controls.Add(btnReload);
            panel1.Controls.Add(btnNavigate);
            panel1.Controls.Add(txtUrl);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnBookmark);
            panel1.Controls.Add(btnBookmarkList);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1057, 50);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Symbol", 15F);
            button1.Location = new Point(55, 8);
            button1.Name = "button1";
            button1.Size = new Size(37, 35);
            button1.TabIndex = 9;
            button1.Text = "🏠";
            button1.TextAlign = ContentAlignment.BottomCenter;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnSettings
            // 
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI Symbol", 15F);
            btnSettings.Location = new Point(12, 6);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(37, 35);
            btnSettings.TabIndex = 10;
            btnSettings.Text = "⚙️";
            btnSettings.TextAlign = ContentAlignment.BottomCenter;
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnBack
            // 
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI Symbol", 12F);
            btnBack.Location = new Point(143, 7);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 35);
            btnBack.TabIndex = 0;
            btnBack.Text = "◀";
            btnBack.TextAlign = ContentAlignment.BottomCenter;
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.BackColor = Color.Transparent;
            btnForward.FlatAppearance.BorderSize = 0;
            btnForward.FlatStyle = FlatStyle.Flat;
            btnForward.Font = new Font("Segoe UI Symbol", 12F);
            btnForward.Location = new Point(224, 7);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(75, 35);
            btnForward.TabIndex = 1;
            btnForward.Text = "▶";
            btnForward.UseVisualStyleBackColor = false;
            btnForward.Click += btnForward_Click;
            // 
            // btnReload
            // 
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.FlatStyle = FlatStyle.Flat;
            btnReload.Font = new Font("Segoe UI Symbol", 12F);
            btnReload.Location = new Point(287, 6);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(75, 35);
            btnReload.TabIndex = 2;
            btnReload.Text = "";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // btnNavigate
            // 
            btnNavigate.BackColor = Color.White;
            btnNavigate.FlatAppearance.BorderSize = 0;
            btnNavigate.FlatStyle = FlatStyle.Flat;
            btnNavigate.Font = new Font("Segoe UI Symbol", 10F);
            btnNavigate.Location = new Point(744, 6);
            btnNavigate.Name = "btnNavigate";
            btnNavigate.Size = new Size(75, 27);
            btnNavigate.TabIndex = 3;
            btnNavigate.Text = "🔍";
            btnNavigate.UseVisualStyleBackColor = false;
            btnNavigate.Click += btnNavigate_Click;
            // 
            // txtUrl
            // 
            txtUrl.Font = new Font("Yu Gothic UI", 13F);
            txtUrl.Location = new Point(386, 4);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "URLを入力してください";
            txtUrl.Size = new Size(433, 31);
            txtUrl.TabIndex = 7;
            txtUrl.TextChanged += txtUrl_TextChanged;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlDark;
            panel2.Controls.Add(btnAddTab);
            panel2.Controls.Add(btnRemoveTab);
            panel2.ForeColor = SystemColors.ControlLightLight;
            panel2.Location = new Point(877, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(180, 47);
            panel2.TabIndex = 8;
            panel2.Paint += panel2_Paint;
            // 
            // btnAddTab
            // 
            btnAddTab.FlatAppearance.BorderSize = 0;
            btnAddTab.FlatStyle = FlatStyle.Flat;
            btnAddTab.Font = new Font("Segoe UI Symbol", 12F);
            btnAddTab.Location = new Point(29, 6);
            btnAddTab.Name = "btnAddTab";
            btnAddTab.Size = new Size(75, 35);
            btnAddTab.TabIndex = 4;
            btnAddTab.Text = "";
            btnAddTab.UseVisualStyleBackColor = true;
            btnAddTab.Click += btnAddTab_Click;
            // 
            // btnRemoveTab
            // 
            btnRemoveTab.FlatAppearance.BorderSize = 0;
            btnRemoveTab.FlatStyle = FlatStyle.Flat;
            btnRemoveTab.Font = new Font("Segoe UI Symbol", 12F);
            btnRemoveTab.Location = new Point(102, 6);
            btnRemoveTab.Name = "btnRemoveTab";
            btnRemoveTab.Size = new Size(75, 35);
            btnRemoveTab.TabIndex = 5;
            btnRemoveTab.Text = "";
            btnRemoveTab.UseVisualStyleBackColor = true;
            btnRemoveTab.Click += btnRemoveTab_Click;
            // 
            // tabControl1
            // 
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 50);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1057, 437);
            tabControl1.TabIndex = 8;
            tabControl1.ImageList = this.imageList1;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Location = new Point(0, 0);
            webView.Name = "webView";
            webView.Size = new Size(0, 0);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(100, 96);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(200, 24);
            menuStrip1.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(32, 19);
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(32, 19);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(32, 19);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(32, 19);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(32, 19);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(32, 19);
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(32, 19);
            // 
            // btnBookmark
            // 
            btnBookmark.FlatAppearance.BorderSize = 0;
            btnBookmark.FlatStyle = FlatStyle.Flat;
            btnBookmark.Font = new Font("Segoe UI Symbol", 15F);
            btnBookmark.Location = new Point(98, 6);
            btnBookmark.Name = "btnBookmark";
            btnBookmark.Size = new Size(37, 35);
            btnBookmark.TabIndex = 11;
            btnBookmark.Text = "⭐";
            btnBookmark.TextAlign = ContentAlignment.BottomCenter;
            btnBookmark.UseVisualStyleBackColor = true;
            btnBookmark.Click += btnBookmark_Click;
            // 
            // btnBookmarkList
            // 
            btnBookmarkList.FlatAppearance.BorderSize = 0;
            btnBookmarkList.FlatStyle = FlatStyle.Flat;
            btnBookmarkList.Font = new Font("Segoe UI Symbol", 15F);
            btnBookmarkList.Location = new Point(141, 6);
            btnBookmarkList.Name = "btnBookmarkList";
            btnBookmarkList.Size = new Size(37, 35);
            btnBookmarkList.TabIndex = 12;
            btnBookmarkList.Text = "📚";
            btnBookmarkList.TextAlign = ContentAlignment.BottomCenter;
            btnBookmarkList.UseVisualStyleBackColor = true;
            btnBookmarkList.Click += (s, e) => ShowBookmarks();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            ClientSize = new Size(1057, 487);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Name = "Form1";
            Opacity = 0.95D;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
        }
    }
}
