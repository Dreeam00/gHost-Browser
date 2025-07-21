using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace IFoxer
{
    public partial class BookmarkListForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bookmark SelectedBookmark { get; set; } = new Bookmark();

        public BookmarkListForm(List<Bookmark> bookmarks)
        {
            InitializeComponent(bookmarks);
        }

        private void InitializeComponent(List<Bookmark> bookmarks)
        {
            this.Text = "ブックマーク";
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            ListView lvBookmarks = new ListView
            {
                Location = new Point(20, 20),
                Size = new Size(540, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            lvBookmarks.Columns.Add("タイトル", 250);
            lvBookmarks.Columns.Add("URL", 250);

            foreach (var bookmark in bookmarks)
            {
                var item = new ListViewItem(bookmark.Title);
                item.SubItems.Add(bookmark.Url);
                item.Tag = bookmark;
                lvBookmarks.Items.Add(item);
            }

            Button btnOpen = new Button
            {
                Text = "開く",
                Location = new Point(380, 330),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnOpen.Click += (s, e) =>
            {
                if (lvBookmarks.SelectedItems.Count > 0)
                {
                    SelectedBookmark = (Bookmark)lvBookmarks.SelectedItems[0].Tag;
                }
            };

            Button btnCancel = new Button
            {
                Text = "キャンセル",
                Location = new Point(480, 330),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] {
                lvBookmarks,
                btnOpen,
                btnCancel
            });
        }
    }
} 