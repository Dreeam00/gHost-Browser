using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace IFoxer
{
    public partial class SettingsForm : Form
    {
        private readonly Settings settings;
        private TabControl tabControl;
        private TabPage generalTab;
        private TabPage bookmarkTab;
        private ListView lvBookmarks;
        private Button btnThemeColor;
        private ColorDialog colorDialog;
        private FolderBrowserDialog folderDialog;
        public event EventHandler? SettingsApplied;

        public SettingsForm(Settings currentSettings)
        {
            settings = currentSettings;
            InitializeComponent();
            colorDialog = new ColorDialog();
            folderDialog = new FolderBrowserDialog();
        }

        private void InitializeComponent()
        {
            this.Text = "設定";
            this.Size = new Size(600, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // 一般設定タブ
            generalTab = new TabPage("一般");
            generalTab.Padding = new Padding(10, 10, 10, 10);

            Label lblThemeColor = new Label
            {
                Text = "テーマカラー:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            btnThemeColor = new Button
            {
                Text = "選択",
                Location = new Point(120, 20),
                Size = new Size(80, 30)
            };
            btnThemeColor.Click += BtnThemeColor_Click;

            Label lblSearchEngine = new Label
            {
                Text = "検索エンジン:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            ComboBox cmbSearchEngine = new ComboBox
            {
                Location = new Point(120, 60),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSearchEngine.Items.AddRange(new string[] { "Google", "Bing", "Yahoo" });
            cmbSearchEngine.SelectedIndex = 0;
            cmbSearchEngine.SelectedIndexChanged += CmbSearchEngine_SelectedIndexChanged;

            Label lblOpacity = new Label
            {
                Text = "透明度:",
                Location = new Point(20, 160),
                AutoSize = true
            };

            TrackBar trackBarOpacity = new TrackBar
            {
                Location = new Point(120, 160),
                Size = new Size(200, 45),
                Maximum = 100,
                Minimum = 10,
                Value = (int)(settings.Opacity * 100)
            };

            Label lblOpacityValue = new Label
            {
                Text = $"{trackBarOpacity.Value}%",
                Location = new Point(330, 160),
                AutoSize = true
            };

            trackBarOpacity.ValueChanged += (s, e) =>
            {
                lblOpacityValue.Text = $"{trackBarOpacity.Value}%";
                settings.Opacity = trackBarOpacity.Value / 100.0;
            };

            // ゴーストモードON/OFFチェックボックス
            CheckBox chkGhostMode = new CheckBox
            {
                Text = "ゴーストモードを有効にする",
                Location = new Point(20, 190),
                AutoSize = true,
                Checked = settings.EnableGhostMode
            };
            chkGhostMode.CheckedChanged += (s, e) =>
            {
                settings.EnableGhostMode = chkGhostMode.Checked;
            };

            // ゴーストモード透明度（マウス外）
            Label lblGhostInactive = new Label
            {
                Text = "ゴーストモード透明度（離れた時）:",
                Location = new Point(40, 220),
                AutoSize = true
            };
            TrackBar trackGhostInactive = new TrackBar
            {
                Location = new Point(240, 215),
                Size = new Size(120, 45),
                Minimum = 30,
                Maximum = 100,
                TickFrequency = 10,
                Value = (int)(settings.GhostOpacityInactive * 100)
            };
            Label lblGhostInactiveValue = new Label
            {
                Text = $"{trackGhostInactive.Value}%",
                Location = new Point(370, 220),
                AutoSize = true
            };
            trackGhostInactive.ValueChanged += (s, e) =>
            {
                lblGhostInactiveValue.Text = $"{trackGhostInactive.Value}%";
                settings.GhostOpacityInactive = trackGhostInactive.Value / 100.0;
            };

            // ゴーストモード透明度（マウス内）
            Label lblGhostActive = new Label
            {
                Text = "ゴーストモード透明度（乗せた時）:",
                Location = new Point(40, 260),
                AutoSize = true
            };
            TrackBar trackGhostActive = new TrackBar
            {
                Location = new Point(240, 255),
                Size = new Size(120, 45),
                Minimum = 30,
                Maximum = 100,
                TickFrequency = 10,
                Value = (int)(settings.GhostOpacityActive * 100)
            };
            Label lblGhostActiveValue = new Label
            {
                Text = $"{trackGhostActive.Value}%",
                Location = new Point(370, 260),
                AutoSize = true
            };
            trackGhostActive.ValueChanged += (s, e) =>
            {
                lblGhostActiveValue.Text = $"{trackGhostActive.Value}%";
                settings.GhostOpacityActive = trackGhostActive.Value / 100.0;
            };

            // 常に前面モードON/OFFチェックボックス
            CheckBox chkAlwaysOnTop = new CheckBox
            {
                Text = "常に前面モード",
                Location = new Point(20, 300),
                AutoSize = true,
                Checked = settings.AlwaysOnTop
            };
            chkAlwaysOnTop.CheckedChanged += (s, e) =>
            {
                settings.AlwaysOnTop = chkAlwaysOnTop.Checked;
            };

            // シークレットモードON/OFFチェックボックス
            CheckBox chkSecretMode = new CheckBox
            {
                Text = "シークレットモード",
                Location = new Point(20, 330),
                AutoSize = true,
                Checked = settings.EnableSecretMode
            };
            chkSecretMode.CheckedChanged += (s, e) =>
            {
                settings.EnableSecretMode = chkSecretMode.Checked;
            };

            // シークレットモードアイコン設定
            Label lblSecretIcon = new Label
            {
                Text = "シークレットモードアイコン:",
                Location = new Point(40, 360),
                AutoSize = true
            };

            TextBox txtSecretIcon = new TextBox
            {
                Location = new Point(200, 360),
                Size = new Size(200, 23),
                Text = settings.SecretIconPath ?? string.Empty,
                ReadOnly = true
            };

            Button btnSelectIcon = new Button
            {
                Text = "選択",
                Location = new Point(410, 360),
                Size = new Size(60, 23)
            };

            OpenFileDialog iconDialog = new OpenFileDialog
            {
                Filter = "アイコンファイル|*.ico|すべてのファイル|*.*",
                Title = "シークレットモードアイコンの選択"
            };

            btnSelectIcon.Click += (s, e) =>
            {
                if (iconDialog.ShowDialog() == DialogResult.OK)
                {
                    settings.SecretIconPath = iconDialog.FileName;
                    txtSecretIcon.Text = iconDialog.FileName;
                }
            };

            generalTab.Controls.AddRange(new Control[] {
                lblThemeColor, btnThemeColor,
                lblSearchEngine, cmbSearchEngine,
                lblOpacity, trackBarOpacity, lblOpacityValue,
                chkGhostMode,
                lblGhostInactive, trackGhostInactive, lblGhostInactiveValue,
                lblGhostActive, trackGhostActive, lblGhostActiveValue,
                chkAlwaysOnTop,
                chkSecretMode,
                lblSecretIcon, txtSecretIcon, btnSelectIcon
            });

            // ブックマーク設定タブ
            bookmarkTab = new TabPage("ブックマーク");
            bookmarkTab.Padding = new Padding(10, 10, 10, 10);

            lvBookmarks = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            lvBookmarks.Columns.AddRange(new ColumnHeader[] {
                new ColumnHeader { Text = "タイトル", Width = 200 },
                new ColumnHeader { Text = "URL", Width = 300 }
            });

            Button btnAddBookmark = new Button
            {
                Text = "追加",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnAddBookmark.Click += (s, e) =>
            {
                var form = new BookmarkForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    settings.Bookmarks.Add(new Bookmark { Title = form.BookmarkTitle, Url = form.BookmarkUrl });
                    RefreshBookmarksList(lvBookmarks);
                }
            };

            Button btnRemoveBookmark = new Button
            {
                Text = "削除",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnRemoveBookmark.Click += (s, e) =>
            {
                if (lvBookmarks.SelectedItems.Count > 0)
                {
                    var index = lvBookmarks.SelectedIndices[0];
                    settings.Bookmarks.RemoveAt(index);
                    RefreshBookmarksList(lvBookmarks);
                }
            };

            bookmarkTab.Controls.AddRange(new Control[] {
                lvBookmarks,
                btnAddBookmark,
                btnRemoveBookmark
            });

            tabControl.TabPages.AddRange(new TabPage[] {
                generalTab,
                bookmarkTab
            });

            this.Controls.Add(tabControl);

            // 設定の読み込み
            btnThemeColor.BackColor = ColorTranslator.FromHtml(settings.ThemeColor);
            cmbSearchEngine.SelectedItem = settings.SearchEngine;
            chkGhostMode.Checked = settings.EnableGhostMode;

            // ブックマークリストの初期化
            RefreshBookmarksList(lvBookmarks);

            Button btnApply = new Button
            {
                Text = "今すぐ適用",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnApply.Click += (s, e) =>
            {
                settings.Save();
                SettingsApplied?.Invoke(this, EventArgs.Empty);
            };

            Button btnSave = new Button
            {
                Text = "設定を保存",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnSave.Click += (s, e) =>
            {
                settings.Save();
                MessageBox.Show("設定を保存しました。", "保存完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // 再起動ボタン
            Button btnRestart = new Button
            {
                Text = "再起動",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnRestart.Click += (s, e) =>
            {
                settings.Save();
                Application.Restart();
            };

            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 110
            };
            buttonPanel.Controls.Add(btnApply);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnRestart);

            this.Controls.Add(buttonPanel);
        }

        private void BtnThemeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                settings.ThemeColor = ColorTranslator.ToHtml(colorDialog.Color);
                btnThemeColor.BackColor = colorDialog.Color;
            }
        }

        private void CmbSearchEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            switch (cmb.SelectedIndex)
            {
                case 0: // Google
                    settings.SearchEngine = "https://www.google.com/search?q=";
                    settings.HomePage = "https://www.google.com";
                    break;
                case 1: // Bing
                    settings.SearchEngine = "https://www.bing.com/search?q=";
                    settings.HomePage = "https://www.bing.com";
                    break;
                case 2: // Yahoo!
                    settings.SearchEngine = "https://search.yahoo.com/search?p=";
                    settings.HomePage = "https://www.yahoo.co.jp";
                    break;
            }
        }

        private void RefreshBookmarksList(ListView listView)
        {
            if (listView == null || settings == null) return;
            
            listView.Items.Clear();
            foreach (var bookmark in settings.Bookmarks)
            {
                var item = new ListViewItem(bookmark.Title);
                item.SubItems.Add(bookmark.Url);
                listView.Items.Add(item);
            }
        }
    }
} 