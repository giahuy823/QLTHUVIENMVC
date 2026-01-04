# 📚 QLThuVienMVC – Hệ thống Quản lý Thư viện

QLThuVienMVC là một ứng dụng web **ASP.NET Core MVC** dùng để quản lý hoạt động thư viện như: quản lý sách, độc giả, mượn – trả sách và phân quyền người dùng.

## 🚀 Công nghệ sử dụng
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Identity** (xác thực & phân quyền)
- **Bootstrap / jQuery**
- **Razor View**

## 🎯 Chức năng chính
### 👤 Quản lý người dùng
- Đăng nhập / Đăng xuất
- Phân quyền (Admin, Nhân viên, Độc giả)

### 📖 Quản lý sách
- Thêm / sửa / xóa sách
- Tìm kiếm sách theo tên, tác giả, thể loại
- Xem chi tiết sách (slug URL thân thiện)

### 🧾 Quản lý mượn – trả
- Tạo phiếu mượn sách
- Xác nhận trả sách theo từng cuốn
- Theo dõi tình trạng mượn sách

### 🗂️ Quản lý danh mục
- Thể loại sách
- Nhà xuất bản

## 🏗️ Kiến trúc hệ thống
Ứng dụng được xây dựng theo mô hình **MVC (Model – View – Controller)**:
- **Model**: Quản lý dữ liệu và nghiệp vụ
- **View**: Giao diện người dùng (Razor Pages)
- **Controller**: Xử lý request và điều hướng
