using CSAT.Data;
using CSAT.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CSAT
{
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private Timer resetTimer;
        private Label thankLabel;
        private readonly CustomerSatisfactionRepository _repo;
        private Screen ScreenWorking
        {
            get
            {
                var secondScreen = Screen.AllScreens
                        .FirstOrDefault(s => !s.Primary);
                var primaryScreen = Screen.PrimaryScreen;

                return secondScreen ?? primaryScreen;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            var factory = new DbConnectionFactory();
            _repo = new CustomerSatisfactionRepository(factory);
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);

                this.StartPosition = FormStartPosition.Manual;
                this.Location = ScreenWorking.WorkingArea.Location;

                this.FormBorderStyle = FormBorderStyle.None;
                this.Size = ScreenWorking.WorkingArea.Size;

                this.Text = "Customer Satisfaction";
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg/very_good.svg");
                this.Icon = SvgIconHelper.SvgToIcon(iconPath, 64);
                InitializeUI();

                toolStripLabel1.Text = $"Phòng khám: {CurrentUser.DepartmentName}";
                toolStripLabel2.Text = $"Bác sĩ: {CurrentUser.FullName}";
            }
            catch (Exception ex)
            {
                log.Error("Lỗi khi xử lý OnLoad", ex);

                MessageBox.Show(
                     "Đã xảy ra lỗi ngoài ý muốn.\nVui lòng thử lại sau.",
                     "Lỗi hệ thống",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }

        }

        //private void InitializeUI()
        //{
        //    tableLayoutPanel1.SuspendLayout();

        //    tableLayoutPanel1.Dock = DockStyle.Fill;
        //    tableLayoutPanel1.Padding = new Padding(20);
        //    tableLayoutPanel1.RowCount = 1;
        //    tableLayoutPanel1.ColumnCount = 5;
        //    tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

        //    tableLayoutPanel1.ColumnStyles.Clear();
        //    tableLayoutPanel1.RowStyles.Clear();
        //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        //    for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
        //        tableLayoutPanel1.ColumnStyles.Add(
        //            new ColumnStyle(SizeType.Percent, (float)100 / tableLayoutPanel1.ColumnCount));


        //    //CreateButton("Rất không hài lòng", "svg/very_bad.svg", Color.Red, 1);
        //    //CreateButton("Không hài lòng", "svg/bad.svg", Color.OrangeRed, 2);
        //    //CreateButton("Bình thường", "svg/neutral.svg", Color.Goldenrod, 3);
        //    //CreateButton("Hài lòng", "svg/good.svg", Color.ForestGreen, 4);
        //    //CreateButton("Rất hài lòng", "svg/very_good.svg", Color.Green, 5);
        //    CreateButton("Rất không hài lòng", "svg/very_bad.png", Color.Red, 1);
        //    CreateButton("Không hài lòng", "svg/bad.png", Color.OrangeRed, 2);
        //    CreateButton("Bình thường", "svg/neutral.png", Color.Goldenrod, 3);
        //    CreateButton("Hài lòng", "svg/good.png", Color.ForestGreen, 4);
        //    CreateButton("Rất hài lòng", "svg/very_good.png", Color.Green, 5);

        //    tableLayoutPanel1.ResumeLayout();
        //    resetTimer = new Timer();
        //    resetTimer.Interval = 3000;
        //    resetTimer.Tick += ResetTimer_Tick;

        //}
        private void InitializeUI()
        {
            tableLayoutPanel1.SuspendLayout();

            // 1. Giảm Padding của TableLayoutPanel xuống tối thiểu
            // Việc để Padding(20) sẽ làm mất 40px diện tích quý giá, khiến icon bị nhỏ lại
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Padding = new Padding(5);
            tableLayoutPanel1.Margin = new Padding(0);

            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            // Đảm bảo hàng chiếm 100% chiều cao
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 20f)); // 100 / 5 = 20
            }

            // Xóa các control cũ nếu có (đề phòng trường hợp gọi InitializeUI nhiều lần)
            tableLayoutPanel1.Controls.Clear();

            // Gọi hàm CreateButton với file PNG
            CreateButton("Rất không hài lòng", "svg/very_bad.png", Color.Red, 1);
            CreateButton("Không hài lòng", "svg/bad.png", Color.OrangeRed, 2);
            CreateButton("Bình thường", "svg/neutral.png", Color.Goldenrod, 3);
            CreateButton("Hài lòng", "svg/good.png", Color.ForestGreen, 4);
            CreateButton("Rất hài lòng", "svg/very_good.png", Color.Green, 5);

            tableLayoutPanel1.ResumeLayout();

            // Tối ưu Timer: Kiểm tra nếu đã tồn tại thì không khởi tạo mới để tránh rác bộ nhớ
            if (resetTimer == null)
            {
                resetTimer = new Timer();
                resetTimer.Tick += ResetTimer_Tick;
            }
            resetTimer.Interval = 3000;
        }
        private void CreateButton(string text, string imagePath, Color color, int value)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Tag = value;

            PictureBox pic = new PictureBox();
            pic.SizeMode = PictureBoxSizeMode.Zoom; // Rất quan trọng cho PNG
            pic.Cursor = Cursors.Hand;

            Label lbl = new Label();
            lbl.Text = text;
            lbl.ForeColor = color;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Cursor = Cursors.Hand;

            // Nạp ảnh PNG
            imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

            if (File.Exists(imagePath))
            {
                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    pic.Image = Image.FromStream(stream);
                }
            }

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);

            pic.Click += Vote_ClickAsync;
            lbl.Click += Vote_ClickAsync;

            // Gọi hàm tính toán lại vị trí mỗi khi kích thước màn hình thay đổi
            panel.Resize += (s, e) =>
            {
                ResizeAndRender(panel, pic, lbl);
            };

            tableLayoutPanel1.Controls.Add(panel);
        }
        //private void CreateButton(string text, string svgFile, Color color, int value)
        //{
        //    Panel panel = new Panel();
        //    panel.Dock = DockStyle.Fill;
        //    panel.Margin = new Padding(20);
        //    panel.Tag = value;
        //    panel.BackColor = Color.Transparent;

        //    PictureBox pic = new PictureBox();
        //    pic.SizeMode = PictureBoxSizeMode.Zoom;
        //    pic.Anchor = AnchorStyles.None;
        //    pic.Cursor = Cursors.Hand;

        //    Label lbl = new Label();
        //    lbl.Text = text;
        //    lbl.ForeColor = color;
        //    lbl.TextAlign = ContentAlignment.TopCenter;
        //    lbl.Cursor = Cursors.Hand;

        //    var svg = new SKSvg();
        //    svg.Load(svgFile);

        //    pic.Tag = new SvgInfo
        //    {
        //        Picture = svg.Picture,
        //        Color = color
        //    };

        //    panel.Controls.Add(lbl);
        //    panel.Controls.Add(pic);

        //    pic.Click += Vote_ClickAsync;
        //    lbl.Click += Vote_ClickAsync;

        //    panel.Resize += (s, e) =>
        //    {
        //        ResizeAndRender(panel, pic, lbl);
        //    };

        //    tableLayoutPanel1.Controls.Add(panel);
        //}
        private void ResizeAndRender(Panel panel, PictureBox pic, Label lbl)
        {
            try
            {
                if (panel.Width <= 0 || panel.Height <= 0) return;

                // 1. Chia lại không gian: Dành 85% chiều cao cho Icon và 15% cho chữ
                // Việc này ép Icon phải nở to hết mức có thể.
                int iconSectionHeight = (int)(panel.Height * 0.85f);
                int textSectionHeight = panel.Height - iconSectionHeight;

                // 2. Tính toán kích thước Icon (Square)
                // Lấy giá trị nhỏ hơn giữa chiều rộng panel và chiều cao khu vực icon
                int size = Math.Min(panel.Width, iconSectionHeight);

                // Nới rộng thêm 1 chút bằng cách giảm padding biên
                pic.Width = size;
                pic.Height = size;

                // 3. Đặt vị trí Icon (Căn giữa ngang, và nằm ở giữa phần iconSection)
                pic.Left = (panel.Width - pic.Width) / 2;
                pic.Top = (iconSectionHeight - pic.Height) / 2;

                // 4. ÉP CHỮ SÁT VÀO ICON
                lbl.Width = panel.Width;
                lbl.Left = 0;

                // Đặt Top của Label bằng Bottom của PictureBox trừ đi một khoảng nhỏ 
                // để bù cho phần khoảng trắng thừa (transparent) trong file PNG
                lbl.Top = pic.Bottom - (int)(size * 0.05f); // Trích lên 5% để sát rạt icon
                lbl.Height = textSectionHeight + (int)(size * 0.05f);

                // 5. FontSize cực đại
                // Tính toán font size dựa trên 1/8 chiều rộng panel để đảm bảo độ to
                float fontSize = panel.Width / 9.5f;

                // Giới hạn để chữ không quá lố khi màn hình quá lớn
                if (fontSize < 12) fontSize = 12;
                if (fontSize > 32) fontSize = 32;

                lbl.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.TopCenter; // Luôn căn chữ lên đỉnh của Label
            }
            catch (Exception ex)
            {
                // log.Error(ex);
            }
        }
        //private void ResizeAndRender(Panel panel, PictureBox pic, Label lbl)
        //{
        //    try
        //    {

        //        if (panel.Width <= 0 || panel.Height <= 0) return;

        //        int totalTableWidth = tableLayoutPanel1.ClientSize.Width -
        //            (tableLayoutPanel1.Padding.Left + tableLayoutPanel1.Padding.Right);

        //        int panelWidth = totalTableWidth / tableLayoutPanel1.ColumnCount;

        //        int usableWidth = panelWidth - (panel.Margin.Left + panel.Margin.Right);

        //        int size = Math.Max(IconSizeOptions.Min,
        //                    Math.Min(usableWidth, IconSizeOptions.Max));

        //        pic.Width = size;
        //        pic.Height = size;

        //        pic.Left = (panel.ClientSize.Width - pic.Width) / 2;
        //        pic.Top = (panel.ClientSize.Height - pic.Height) / 2;

        //        lbl.Left = 0;
        //        lbl.Width = panel.ClientSize.Width;
        //        lbl.Top = pic.Bottom;

        //        int fontSize = Math.Max(12, usableWidth / 18);
        //        lbl.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
        //        lbl.Height = fontSize + 20;

        //        if (pic.Tag is SvgInfo info)
        //        {
        //            RenderSvg(pic, info.Picture);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Lỗi khi xử lý ResizeAndRender", ex);
        //    }

        //}
        //private void RenderSvg(PictureBox pic, SKPicture picture)
        //{
        //    if (pic.Width <= 0 || pic.Height <= 0) return;

        //    using (var bitmap = new SKBitmap(pic.Width, pic.Height))
        //    using (var canvas = new SKCanvas(bitmap))
        //    {
        //        canvas.Clear(SKColors.Transparent);

        //        var scaleX = pic.Width / picture.CullRect.Width;
        //        var scaleY = pic.Height / picture.CullRect.Height;
        //        var scale = Math.Min(scaleX, scaleY);

        //        canvas.Scale(scale);
        //        canvas.DrawPicture(picture);

        //        using (var image = SKImage.FromBitmap(bitmap))
        //        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        //        using (var ms = new MemoryStream(data.ToArray()))
        //        {
        //            var old = pic.Image;
        //            pic.Image = new Bitmap(ms);
        //            old?.Dispose();
        //        }
        //    }
        //}
        //private void Panel_Resize(object sender, EventArgs e)
        //{
        //    Panel p = sender as Panel;
        //    if (p.Width <= 0 || p.Height <= 0) return;
        //    var lbl = p.Controls.OfType<Label>().FirstOrDefault();
        //    if (lbl == null) return;

        //    float fontSize = p.Width / 25f;

        //    if (fontSize < 12f) fontSize = 12f;
        //    if (fontSize > 20f) fontSize = 20f;

        //    lbl.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);

        //}
        private void ShowThankYou()
        {
            this.SuspendLayout();
            tableLayoutPanel1.Visible = false;

            thankLabel = new Label();
            thankLabel.Text = "Cảm ơn sự đánh giá của Quý khách!";
            thankLabel.ForeColor = Color.FromArgb(0, 120, 60);
            thankLabel.Dock = DockStyle.Fill;
            thankLabel.Font = new Font("Segoe UI", 48, FontStyle.Bold);
            thankLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(thankLabel);
            thankLabel.BringToFront();
            this.ResumeLayout();

            resetTimer.Start();
        }

        private void ResetTimer_Tick(object sender, EventArgs e)
        {
            this.SuspendLayout();
            resetTimer.Stop();

            if (thankLabel != null)
                this.Controls.Remove(thankLabel);

            tableLayoutPanel1.Visible = true;
            this.ResumeLayout();

        }
        private async void Vote_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Panel panel = null;
                if (sender is PictureBox pic)
                {
                    panel = pic.Parent as Panel;
                }
                else if (sender is Label lbl)
                {
                    panel = lbl.Parent as Panel;
                }

                if (panel != null)
                {
                    int voteValue = (int)(panel.Tag ?? -1);
                    var deviceInfo = DeviceHelper.GetDeviceInfo();

                    var csf = new CustomerSatisfaction()
                    {
                        VoteValue = voteValue,
                        UserId = CurrentUser.UserId,
                        DepartmentId = CurrentUser.DepartmentId,
                        DeviceName = deviceInfo.DeviceName,
                        IPAddress = deviceInfo.IPAddress,
                    };
                    await _repo.Insert(csf);
                    log.Info("Insert CustomerSatisfaction: " +
                    JsonConvert.SerializeObject(csf));
                    ShowThankYou();
                }
            }
            catch (Exception ex)
            {
                log.Error("Lỗi khi xử lý Vote", ex);

                MessageBox.Show(
                     "Đã xảy ra lỗi ngoài ý muốn.\nVui lòng thử lại sau.",
                     "Lỗi hệ thống",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }

        }

    }
}
