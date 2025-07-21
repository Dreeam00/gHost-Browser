using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace IFoxer
{
    public partial class Form1 : Form
    {
        private bool isDragging = false;
        private TabPage draggedTab;
        private Point dragStartPoint;
        private Settings settings;
        private List<DownloadItem> downloads = new List<DownloadItem>();
        private List<HistoryItem> history = new List<HistoryItem>();
        private bool isSearchVisible = false;
        // ゴーストモード有効フラグ
        private bool ghostModeEnabled = false;
        // シークレットモード有効フラグ
        private bool secretModeEnabled = false;
        // --- ゴーストモード用マウス監視 ---
        private bool wasMouseOver = false;
        // 完全透明トグル用
        private bool isFullyTransparent = false;
        private double lastOpacity = 1.0;
        private bool ghostModeEnabledBeforeTransparent = false;

        public Form1()
        {
            InitializeComponent();
            settings = Settings.Load();
            ghostModeEnabled = settings.EnableGhostMode;
            secretModeEnabled = settings.EnableSecretMode;
            this.TopMost = settings.AlwaysOnTop;
            InitializeWebView2();
            // 起動時はindex.htmlのみを開く
            AddNewTab();
            tabControl1.MouseDown += new MouseEventHandler(tabControl1_MouseDown);
            tabControl1.MouseMove += new MouseEventHandler(tabControl1_MouseMove);
            tabControl1.MouseUp += new MouseEventHandler(tabControl1_MouseUp);
            tabControl1.AllowDrop = true;
            tabControl1.DragOver += new DragEventHandler(tabControl1_DragOver);
            tabControl1.DragDrop += new DragEventHandler(tabControl1_DragDrop);
            ApplyTheme();
            if (secretModeEnabled)
            {
                this.Text += " - シークレットモード";
                this.BackColor = Color.FromArgb(40, 40, 40);
                panel1.BackColor = Color.FromArgb(40, 40, 40);
                panel2.BackColor = Color.FromArgb(40, 40, 40);
                if (!string.IsNullOrEmpty(settings.SecretIconPath) && System.IO.File.Exists(settings.SecretIconPath))
                {
                    try
                    {
                        this.Icon = new Icon(settings.SecretIconPath);
                    }
                    catch { }
                }
            }

            // キーボードショートカットの設定
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            // ダウンロードボタンの追加
            Button btnDownloads = new Button
            {
                Text = "📥",
                Size = new Size(30, 30),
                Location = new Point(btnBookmark.Right + 5, btnBookmark.Top),
                FlatStyle = FlatStyle.Flat
            };
            btnDownloads.Click += BtnDownloads_Click;
            panel2.Controls.Add(btnDownloads);

            // 履歴ボタンの追加
            Button btnHistory = new Button
            {
                Text = "🕒",
                Size = new Size(30, 30),
                Location = new Point(btnDownloads.Right + 5, btnDownloads.Top),
                FlatStyle = FlatStyle.Flat
            };
            btnHistory.Click += BtnHistory_Click;
            panel2.Controls.Add(btnHistory);

            // 検索ボタンの追加
            Button btnSearch = new Button
            {
                Text = "🔍",
                Size = new Size(30, 30),
                Location = new Point(btnHistory.Right + 5, btnHistory.Top),
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;
            panel2.Controls.Add(btnSearch);

            // ホームボタンの追加
            Button btnHome = new Button
            {
                Text = "🏠",
                Size = new Size(30, 30),
                Location = new Point(btnSearch.Right + 5, btnSearch.Top),
                FlatStyle = FlatStyle.Flat
            };
            btnHome.Click += BtnHome_Click;
            panel2.Controls.Add(btnHome);

            // Application.Idleでマウス監視
            Application.Idle += Application_Idle;
        }

        private async void InitializeWebView2()
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.Settings.AreDevToolsEnabled = true;

                // ダウンロードイベントの処理
                webView.CoreWebView2.DownloadStarting += (sender, e) =>
                {
                    if (secretModeEnabled) return; // シークレット時は記録しない
                    var download = new DownloadItem
                    {
                        FileName = e.DownloadOperation.ResultFilePath,
                        StartTime = DateTime.Now,
                        Status = "ダウンロード中"
                    };
                    downloads.Add(download);

                    e.DownloadOperation.StateChanged += (s, args) =>
                    {
                        var operation = (CoreWebView2DownloadOperation)s;
                        if (operation.State == CoreWebView2DownloadState.Completed)
                        {
                            download.Status = "完了";
                        }
                        else if (operation.State == CoreWebView2DownloadState.Interrupted)
                        {
                            download.Status = "中断";
                        }
                    };
                };

                // 履歴の記録
                webView.CoreWebView2.NavigationCompleted += (sender, e) =>
                {
                    if (secretModeEnabled) return; // シークレット時は記録しない
                    if (e.IsSuccess)
                    {
                        var historyItem = new HistoryItem
                        {
                            Url = webView.Source.ToString(),
                            Title = webView.CoreWebView2.DocumentTitle,
                            VisitTime = DateTime.Now
                        };
                        history.Add(historyItem);
                    }
                };

                // 既存のイベントハンドラ
                webView.CoreWebView2.NavigationStarting += (sender, e) =>
                {
                    if (e.IsRedirected)
                    {
                        e.Cancel = true;
                        webView.Source = new Uri(e.Uri);
                    }
                };

                webView.CoreWebView2.NewWindowRequested += (sender, e) =>
                {
                    e.Handled = true;
                    AddNewTab(e.Uri);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2の初期化に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NavigateToUrl(string url)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            webView.Source = new Uri(url);
        }

        private async void AddNewTab(string url = null)
        {
            try
            {
                // 既に10個以上タブがある場合は新規作成しない
                if (tabControl1.TabPages.Count >= 10) return;
                TabPage newTab = new TabPage("New Tab");
                WebView2 webView = new WebView2();
                webView.Dock = DockStyle.Fill;
                await webView.EnsureCoreWebView2Async(null);

                // 新しいタブのWebView2にも同じ設定を適用
                webView.CoreWebView2.NavigationStarting += (sender, e) =>
                {
                    if (e.IsRedirected)
                    {
                        e.Cancel = true;
                        webView.Source = new Uri(e.Uri);
                    }
                };

                webView.CoreWebView2.NewWindowRequested += (sender, e) =>
                {
                    e.Handled = true;
                    AddNewTab(e.Uri);
                };

                // ページの読み込み完了時にタイトルとfaviconを更新
                webView.CoreWebView2.NavigationCompleted += async (sender, e) =>
                {
                    if (e.IsSuccess)
                    {
                        try
                        {
                            var tab = tabControl1.SelectedTab;
                            if (tab != null)
                            {
                                tab.Text = webView.CoreWebView2.DocumentTitle;

                                try
                                {
                                    var faviconStream = await webView.CoreWebView2.GetFaviconAsync(CoreWebView2FaviconImageFormat.Png);
                                    if (faviconStream != null)
                                    {
                                        using (var bitmap = new Bitmap(faviconStream))
                                        {
                                            tab.ImageIndex = imageList1.Images.Count;
                                            imageList1.Images.Add(bitmap);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Faviconの取得に失敗しました: {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"タブの更新に失敗しました: {ex.Message}");
                        }
                    }
                };

                newTab.Controls.Add(webView);
                tabControl1.TabPages.Add(newTab);
                tabControl1.SelectedTab = newTab;

                // URLが指定されていない場合はindex.htmlを表示
                if (string.IsNullOrEmpty(url))
                {
                    string indexPath;
                    if (secretModeEnabled && !string.IsNullOrEmpty(settings.SecretIconPath))
                    {
                        // シークレットモード用のindex.htmlのパス
                        indexPath = Path.Combine(Application.StartupPath, "index_secret.html");
                        if (!File.Exists(indexPath))
                        {
                            // シークレットモード用のファイルが存在しない場合は通常のindex.htmlを使用
                            indexPath = Path.Combine(Application.StartupPath, "index.html");
                        }
                    }
                    else
                    {
                        // 通常のindex.htmlのパス
                        indexPath = Path.Combine(Application.StartupPath, "index.html");
                    }

                    if (File.Exists(indexPath))
                    {
                        string fileUrl = "file:///" + indexPath.Replace("\\", "/");
                        webView.Source = new Uri(fileUrl);
                    }
                    else
                    {
                        webView.Source = new Uri("about:blank");
                    }
                }
                else
                {
                    webView.Source = new Uri(url);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新しいタブの作成に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveCurrentTab()
        {
            if (tabControl1.TabPages.Count > 1)
            {
                var tab = tabControl1.SelectedTab;
                if (tab.Controls.Count > 0 && tab.Controls[0] is WebView2 webView)
                {
                    webView.Dispose();
                }
                tabControl1.TabPages.Remove(tab);
                GC.Collect();
            }
        }

        private async void DisplayFavicon()
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            var faviconStream = await webView.CoreWebView2.GetFaviconAsync(Microsoft.Web.WebView2.Core.CoreWebView2FaviconImageFormat.Png);
            if (faviconStream != null)
            {
                var favicon = new System.Drawing.Bitmap(faviconStream);
                this.Icon = System.Drawing.Icon.FromHandle(favicon.GetHicon());
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            webView.Reload();
        }

        private async void btnNavigate_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            string input = txtUrl.Text;

            if (IsValidUrl(input))
            {
                webView.Source = new Uri(input);
            }
            else
            {
                string searchUrl = GetBingSearchUrl(input);
                webView.Source = new Uri(searchUrl);
            }

            DisplayFavicon();
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private string GetBingSearchUrl(string query)
        {
            return settings.SearchEngine + Uri.EscapeDataString(query);
        }

        private void btnAddTab_Click(object sender, EventArgs e)
        {
            AddNewTab();
        }

        private void btnRemoveTab_Click(object sender, EventArgs e)
        {
            RemoveCurrentTab();
        }

        private void btnLoadHtml_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            string htmlFilePath = txtUrl.Text;
            webView.Source = new Uri(htmlFilePath);
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.GetTabRect(i).Contains(e.Location))
                {
                    isDragging = true;
                    draggedTab = tabControl1.TabPages[i];
                    dragStartPoint = e.Location;
                    break;
                }
            }
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                if (Math.Abs(e.Location.X - dragStartPoint.X) > SystemInformation.DragSize.Width ||
                    Math.Abs(e.Location.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height)
                {
                    tabControl1.DoDragDrop(draggedTab, DragDropEffects.Move);
                }
            }
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void tabControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                TabPage tabPage = (TabPage)e.Data.GetData(typeof(TabPage));
                if (tabPage != null && !tabPage.IsDisposed)
                {
                    Form1 newForm = new Form1();
                    newForm.Show();
                    newForm.AddExistingTab(tabPage);
                    if (!tabPage.IsDisposed)
                    {
                        tabControl1.TabPages.Remove(tabPage);
                    }
                }
            }
        }

        public void AddExistingTab(TabPage tabPage)
        {
            if (tabPage != null && !tabPage.IsDisposed)
            {
                tabControl1.TabPages.Add(tabPage);
                tabControl1.SelectedTab = tabPage;
                if (tabPage.Controls.Count > 0 && tabPage.Controls[0] is WebView2 webView)
                {
                    webView.Source = new Uri(settings.HomePage);
                }
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigateToUrl(settings.HomePage);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // 設定画面表示前にTopMost解除
            bool restoreTopMost = this.TopMost;
            this.TopMost = false;
            using (var settingsForm = new SettingsForm(settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    ghostModeEnabled = settings.EnableGhostMode;
                    secretModeEnabled = settings.EnableSecretMode;
                    this.TopMost = settings.AlwaysOnTop;
                    ApplyTheme();
                }
            }
            // 設定画面終了後にTopMost復元
            this.TopMost = settings.AlwaysOnTop;
        }

        private void ApplyTheme()
        {
            if (ColorTranslator.FromHtml(settings.ThemeColor) is Color themeColor)
            {
                this.BackColor = themeColor;
                panel1.BackColor = themeColor;
                panel2.BackColor = themeColor;
            }

            // 透明度の設定を適用
            this.Opacity = settings.Opacity;

            // 常に前面モード
            this.TopMost = settings.AlwaysOnTop;

            // 縦タブの設定を適用
            if (settings.VerticalTabs)
            {
                tabControl1.Alignment = TabAlignment.Left;
                tabControl1.Multiline = true;
                tabControl1.SizeMode = TabSizeMode.Fixed;
                tabControl1.ItemSize = new Size(40, 120);
            }
            else
            {
                tabControl1.Alignment = TabAlignment.Top;
                tabControl1.Multiline = false;
                tabControl1.SizeMode = TabSizeMode.Normal;
                tabControl1.ItemSize = new Size(100, 24);
            }
        }

        private void btnBookmark_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            using (var form = new BookmarkForm())
            {
                form.BookmarkTitle = webView.CoreWebView2.DocumentTitle;
                form.BookmarkUrl = webView.Source.ToString();
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    settings.Bookmarks.Add(new Bookmark
                    {
                        Title = form.BookmarkTitle,
                        Url = form.BookmarkUrl
                    });
                    settings.Save();
                }
            }
        }

        private void ShowBookmarks()
        {
            using (var form = new BookmarkListForm(settings.Bookmarks))
            {
                if (form.ShowDialog() == DialogResult.OK && form.SelectedBookmark != null)
                {
                    NavigateToUrl(form.SelectedBookmark.Url);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + T: 新しいタブを開く
            if (e.Control && e.KeyCode == Keys.T)
            {
                e.Handled = true;
                AddNewTab();
            }
            // Ctrl + W: 現在のタブを閉じる
            else if (e.Control && e.KeyCode == Keys.W)
            {
                e.Handled = true;
                RemoveCurrentTab();
            }
            // Ctrl + R: ページを更新
            else if (e.Control && e.KeyCode == Keys.R)
            {
                e.Handled = true;
                WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
                webView.Reload();
            }
            // Ctrl + L: URLバーにフォーカス
            else if (e.Control && e.KeyCode == Keys.L)
            {
                e.Handled = true;
                txtUrl.Focus();
                txtUrl.SelectAll();
            }
            // Ctrl + D: ブックマークに追加
            else if (e.Control && e.KeyCode == Keys.D)
            {
                e.Handled = true;
                btnBookmark_Click(sender, e);
            }
            // Ctrl + Tab: 次のタブに移動
            else if (e.Control && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                if (tabControl1.SelectedIndex < tabControl1.TabPages.Count - 1)
                {
                    tabControl1.SelectedIndex++;
                }
                else
                {
                    tabControl1.SelectedIndex = 0;
                }
            }
            // Ctrl + Shift + Tab: 前のタブに移動
            else if (e.Control && e.Shift && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                if (tabControl1.SelectedIndex > 0)
                {
                    tabControl1.SelectedIndex--;
                }
                else
                {
                    tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                }
            }
            // Ctrl + Shift + T: 閉じたタブを復元
            else if (e.Control && e.Shift && e.KeyCode == Keys.T)
            {
                e.Handled = true;
                // TODO: 閉じたタブの履歴を実装
            }
            // Ctrl + ?: ショートカット一覧を表示
            else if (e.Control && e.KeyCode == Keys.OemQuestion)
            {
                e.Handled = true;
                ShowShortcutsList();
            }
            // Shift + Z: 完全透明トグル
            else if (e.Shift && e.KeyCode == Keys.Z)
            {
                e.Handled = true;
                if (!isFullyTransparent)
                {
                    lastOpacity = this.Opacity;
                    ghostModeEnabledBeforeTransparent = ghostModeEnabled;
                    ghostModeEnabled = false;
                    this.Opacity = 0.01;
                    isFullyTransparent = true;
                }
                else
                {
                    this.Opacity = lastOpacity;
                    ghostModeEnabled = ghostModeEnabledBeforeTransparent;
                    isFullyTransparent = false;
                }
                return;
            }
        }

        private void ShowShortcutsList()
        {
            // ショートカット一覧表示前にTopMost解除
            bool restoreTopMost = this.TopMost;
            this.TopMost = false;
            string shortcuts = "キーボードショートカット一覧:\n\n" +
                             "Ctrl + T: 新しいタブを開く\n" +
                             "Ctrl + W: 現在のタブを閉じる\n" +
                             "Ctrl + R: ページを更新\n" +
                             "Ctrl + L: URLバーにフォーカス\n" +
                             "Ctrl + D: ブックマークに追加\n" +
                             "Ctrl + Tab: 次のタブに移動\n" +
                             "Ctrl + Shift + Tab: 前のタブに移動\n" +
                             "Ctrl + Shift + T: 閉じたタブを復元\n" +
                             "Ctrl + ?: このショートカット一覧を表示";
            MessageBox.Show(shortcuts, "ショートカット一覧", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // 復元
            this.TopMost = settings.AlwaysOnTop;
        }

        private void BtnDownloads_Click(object sender, EventArgs e)
        {
            var form = new Form
            {
                Text = "ダウンロード",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("ファイル名", 200);
            listView.Columns.Add("ステータス", 100);
            listView.Columns.Add("開始時間", 150);

            foreach (var download in downloads)
            {
                var item = new ListViewItem(download.FileName);
                item.SubItems.Add(download.Status);
                item.SubItems.Add(download.StartTime.ToString());
                listView.Items.Add(item);
            }

            form.Controls.Add(listView);
            form.ShowDialog();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            var form = new Form
            {
                Text = "履歴",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("タイトル", 300);
            listView.Columns.Add("URL", 400);
            listView.Columns.Add("訪問時間", 150);

            foreach (var item in history.OrderByDescending(h => h.VisitTime))
            {
                var listItem = new ListViewItem(item.Title);
                listItem.SubItems.Add(item.Url);
                listItem.SubItems.Add(item.VisitTime.ToString());
                listView.Items.Add(listItem);
            }

            listView.DoubleClick += (s, args) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    var url = listView.SelectedItems[0].SubItems[1].Text;
                    WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
                    webView.Source = new Uri(url);
                    form.Close();
                }
            };

            form.Controls.Add(listView);
            form.ShowDialog();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!isSearchVisible)
            {
                var searchPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 40
                };

                var searchBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    PlaceholderText = "ページ内検索..."
                };

                var closeButton = new Button
                {
                    Text = "×",
                    Dock = DockStyle.Right,
                    Width = 30,
                    FlatStyle = FlatStyle.Flat
                };
                closeButton.Click += (s, args) =>
                {
                    searchPanel.Parent.Controls.Remove(searchPanel);
                    isSearchVisible = false;
                };

                searchBox.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
                        webView.CoreWebView2.ExecuteScriptAsync($"window.find('{searchBox.Text}')");
                    }
                };

                searchPanel.Controls.Add(searchBox);
                searchPanel.Controls.Add(closeButton);
                this.Controls.Add(searchPanel);
                searchPanel.BringToFront();
                searchBox.Focus();
                isSearchVisible = true;
            }
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            WebView2 webView = (WebView2)tabControl1.SelectedTab.Controls[0];
            string indexPath;
            
            if (secretModeEnabled && !string.IsNullOrEmpty(settings.SecretIconPath))
            {
                // シークレットモード用のindex.htmlのパス
                indexPath = Path.Combine(Application.StartupPath, "index_secret.html");
                if (!File.Exists(indexPath))
                {
                    // シークレットモード用のファイルが存在しない場合は通常のindex.htmlを使用
                    indexPath = Path.Combine(Application.StartupPath, "index.html");
                }
            }
            else
            {
                // 通常のindex.htmlのパス
                indexPath = Path.Combine(Application.StartupPath, "index.html");
            }

            if (File.Exists(indexPath))
            {
                string fileUrl = "file:///" + indexPath.Replace("\\", "/");
                webView.Source = new Uri(fileUrl);
            }
            else
            {
                // TopMost解除してエラー表示
                bool restoreTopMost = this.TopMost;
                this.TopMost = false;
                MessageBox.Show("index.htmlが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.TopMost = settings.AlwaysOnTop;
            }
        }

        private class DownloadItem
        {
            public string FileName { get; set; } = string.Empty;
            public DateTime StartTime { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        private class HistoryItem
        {
            public string Url { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public DateTime VisitTime { get; set; }
        }

        // --- ゴーストモード用マウス監視 ---
        private void Application_Idle(object sender, EventArgs e)
        {
            if (!ghostModeEnabled) return;
            Point clientPos = this.PointToClient(Cursor.Position);
            bool isOver = this.ClientRectangle.Contains(clientPos);
            if (isOver != wasMouseOver)
            {
                if (isOver)
                {
                    this.Opacity = settings.GhostOpacityActive;
                }
                else
                {
                    this.Opacity = settings.GhostOpacityInactive;
                }
                wasMouseOver = isOver;
            }
        }
    }
}
