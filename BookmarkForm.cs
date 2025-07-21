using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace IFoxer
{
    public partial class BookmarkForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BookmarkTitle { get; set; } = string.Empty;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BookmarkUrl { get; set; } = string.Empty;

        public BookmarkForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "ブックマークの追加";
            this.Size = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblTitle = new Label
            {
                Text = "タイトル:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            TextBox txtTitle = new TextBox
            {
                Location = new Point(120, 15),
                Size = new Size(240, 25)
            };

            Label lblUrl = new Label
            {
                Text = "URL:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            TextBox txtUrl = new TextBox
            {
                Location = new Point(120, 55),
                Size = new Size(240, 25)
            };

            Button btnOK = new Button
            {
                Text = "OK",
                Location = new Point(180, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) =>
            {
                BookmarkTitle = txtTitle.Text;
                BookmarkUrl = txtUrl.Text;
            };

            Button btnCancel = new Button
            {
                Text = "キャンセル",
                Location = new Point(280, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] {
                lblTitle, txtTitle,
                lblUrl, txtUrl,
                btnOK, btnCancel
            });
        }
    }
} 