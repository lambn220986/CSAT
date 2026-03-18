# 🏥 CSAT - BV DaLieu TPHCM

Ứng dụng khảo sát mức độ hài lòng khách hàng (Customer Satisfaction - CSAT) cho Bệnh viện Da Liễu TP.HCM.

---

## 📌 Mô tả

Project được xây dựng bằng **.NET Framework WinForms**, phục vụ:

- Thu thập phản hồi người bệnh
- Đánh giá chất lượng dịch vụ
- Hỗ trợ báo cáo nội bộ

---

## 🛠️ Công nghệ sử dụng

- .NET Framework (WinForms)
- C#
- SQL Server (nếu có)
- Visual Studio

---

## 📂 Cấu trúc project
CSAT/
├── CSAT/                    # Project chính (WinForms UI)
│   ├── Data/               # Data
│   ├── Models/             # Model / Entity
│   ├── Resources/          # Resource (ảnh, file)
│   ├── Svg/                # Icon SVG
│   ├── Properties/         # Cấu hình project
│   ├── CurrentUser.cs      # Thông tin user hiện tại
│   ├── DeviceHelper.cs     # Xử lý thiết bị
│   ├── IconSizeOptions.cs  # Config icon
│   ├── MainForm.cs         # Form chính
│   ├── SvgIconHelper.cs    # Xử lý icon SVG
│   ├── Program.cs          # Entry point
│   ├── App.config          # Config app
│   ├── packages.config     # NuGet packages
│   └── README.md
│
├── CSAT.Launcher/          # Project launcher riêng
│   ├── Program.cs
│   ├── App.config
│   └── packages.config
│
├── bin/                    # Output build (ignore)
├── obj/                    # File tạm build (ignore)
├── packages/               # Thư viện NuGet
└── .gitignore