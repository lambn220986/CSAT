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
CSAT
├── CSAT/ # WinForms UI chính
│ ├── Data/ # Xử lý dữ liệu
│ ├── Models/ # Model / Entity
│ ├── Resources/ # Resource
│ ├── Svg/ # Icon SVG
│ ├── MainForm.cs # Form chính
│ ├── Program.cs # Entry point
│ └── App.config # Config
│
├── CSAT.Launcher/ # Project launcher
│ ├── Program.cs
│ └── App.config
│
├── bin/ # Output build (ignore)
├── obj/ # File tạm (ignore)
└── packages/ # NuGet packages