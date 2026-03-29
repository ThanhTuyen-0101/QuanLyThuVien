CREATE DATABASE QuanLyThuVien;
GO
USE QuanLyThuVien;
GO
CREATE TABLE TheLoai (
    MaTheLoai INT PRIMARY KEY IDENTITY(1,1),
    TenTheLoai NVARCHAR(100) NOT NULL CONSTRAINT UK_TheLoai_TenTheLoai UNIQUE
);
GO
INSERT INTO TheLoai (TenTheLoai) VALUES
(N'Khoa học viễn tưởng'),
(N'Tiểu thuyết'),
(N'Lịch sử'),
(N'Kinh tế'),
(N'Truyện tranh'),
(N'Tâm lý học'),
(N'Công nghệ thông tin'),
(N'Văn học thiếu nhi');
GO
CREATE TABLE NhaXuatBan (
    MaNhaXuatBan INT PRIMARY KEY IDENTITY(1,1),
    TenNhaXuatBan NVARCHAR(255) NOT NULL CONSTRAINT UK_NhaXuatBan_TenNhaXuatBan UNIQUE,
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20),
    Email VARCHAR(100) UNIQUE
);
GO
INSERT INTO NhaXuatBan (TenNhaXuatBan, DiaChi, SoDienThoai, Email) VALUES
(N'Nhà xuất bản Kim Đồng', N'248 Cống Quỳnh, Q.1, TP.HCM', '02838399000', 'kimdong@nxbkimdong.com.vn'),
(N'Nhà xuất bản Trẻ', N'161B Lý Chính Thắng, P.7, Q.3, TP.HCM', '02839316289', 'tre@nxbtre.com.vn'),
(N'Nhà xuất bản Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, P.Đa Kao, Q.1, TP.HCM', '02838225340', 'tonghop@nxbtonghop.com.vn'),
(N'Nhà xuất bản Lao Động', N'175 Giảng Võ, Ba Đình, Hà Nội', '02438453457', 'laodong@nxblaodong.com'),
(N'Nhà xuất bản Giáo dục Việt Nam', N'81 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', '02438253111', 'gd@nxb.edu.vn');
GO
CREATE TABLE TacGia (
    MaTacGia INT PRIMARY KEY IDENTITY(1,1),
    TenTacGia NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    QuocTich NVARCHAR(50),
    TieuSu NVARCHAR(MAX)
);
GO
INSERT INTO TacGia (TenTacGia, NgaySinh, QuocTich, TieuSu) VALUES
(N'Nguyễn Nhật Ánh', '1955-05-07', N'Việt Nam', N'Nhà văn chuyên viết cho tuổi mới lớn và thiếu nhi.'),
(N'Haruki Murakami', '1949-01-12', N'Nhật Bản', N'Tiểu thuyết gia nổi tiếng với phong cách siêu thực.'),
(N'Yuval Noah Harari', '1976-02-24', N'Israel', N'Nhà sử học và tác giả của các cuốn sách bán chạy về lịch sử loài người.'),
(N'Adam Khoo', '1974-04-08', N'Singapore', N'Doanh nhân, tác giả và diễn giả truyền cảm hứng.'),
(N'Stephen King', '1947-09-21', N'Hoa Kỳ', N'Bậc thầy của thể loại kinh dị và giả tưởng.');
GO
CREATE TABLE ThuThu (
    MaThuThu INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    TenDangNhap VARCHAR(50) NOT NULL CONSTRAINT UK_ThuThu_TenDangNhap UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    SoDienThoai VARCHAR(20),
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
    NgayTaoTaiKhoan DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_OneThuThu CHECK (MaThuThu = 1)
);
GO
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ThuThu'
    AND COLUMN_NAME = 'GioiTinh'
)
BEGIN
    ALTER TABLE ThuThu
    ADD GioiTinh NVARCHAR(10);
END
GO
INSERT INTO ThuThu (HoTen, TenDangNhap, MatKhau, Email, SoDienThoai, DiaChi, NgaySinh, GioiTinh) VALUES
(N'Nguyen Thanh Tuyen', 'Tuyen', '1234@Ntt', 'admin@thuvien.com', '0901234567', N'123 Đường ABC, Quận XYZ, TP.HCM', '1980-01-01', N'Nam');
GO
CREATE TABLE DocGia (
    MaDocGia VARCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    TenDangNhap VARCHAR(50) NULL CONSTRAINT UK_DocGia_TenDangNhap UNIQUE,
    MatKhau VARCHAR(255) NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20),
    Email VARCHAR(100) UNIQUE,
    NgayDangKy DATE DEFAULT GETDATE()
);
GO
INSERT INTO DocGia (MaDocGia, HoTen, TenDangNhap, MatKhau, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, NgayDangKy) VALUES
('GN0001', N'Trần Văn An', 'antran', 'Pass@123', '1995-03-15', N'Nam', N'123 Đường Láng, Hà Nội', '0901234567', 'antran@gmail.com', '2025-01-10'),
('GN0002', N'Nguyễn Thị Bình', 'binhnguyen', 'Secure#456', '1998-07-22', N'Nữ', N'45 Nguyễn Huệ, TP.HCM', '0912345678', 'binhnguyen@gmail.com', '2025-02-15'),
('GN0003', N'Phạm Minh Châu', 'chaupham', 'Chau$789', '2000-11-30', N'Nữ', N'78 Lê Lợi, Đà Nẵng', '0923456789', 'chaupham@gmail.com', '2025-03-20'),
('GN0004', N'Lê Quốc Đạt', 'datle', 'Dat#2023', '1993-05-10', N'Nam', N'56 Trần Phú, Hải Phòng', '0934567890', 'datle@gmail.com', '2025-04-05'),
('GN0005', N'Hoàng Thị Mai', 'maihoang', 'Mai@4567', '1997-09-12', N'Nữ', N'89 Phạm Văn Đồng, Cần Thơ', '0945678901', 'maihoang@gmail.com', '2025-04-10'),
('GN0006', N'Vũ Văn Hùng', 'hungvu', 'Hung$123', '1994-02-25', N'Nam', N'12 Nguyễn Trãi, Hà Nội', '0956789012', 'hungvu@gmail.com', '2025-05-01'),
('GN0007', N'Đỗ Thị Lan', 'lando', 'Lan#7890', '1999-12-05', N'Nữ', '34 Bạch Đằng, Đà Nẵng', '0967890123', 'lando@gmail.com', '2025-05-15'),
('GN0008', N'Nguyễn Văn Nam', 'namnguyen', 'Nam@2023', '1996-08-18', N'Nam', N'67 Lý Thường Kiệt, TP.HCM', '0978901234', 'namnguyen@gmail.com', '2025-06-02'),
('GN0009', N'Trần Thị Hồng', 'hongtran', 'Hong$456', '2001-04-20', N'Nữ', N'23 Hùng Vương, Huế', '0989012345', 'hongtran@gmail.com', '2025-06-10'),
('GN0010', N'Phan Văn Tâm', 'tamphan', 'Tam#1234', '1992-06-07', N'Nam', N'45 Nguyễn Đình Chiểu, Hà Nội', '0990123456', 'tamphan@gmail.com', '2025-06-20');
GO
CREATE TABLE Sach (
    MaSach INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(255) NOT NULL,
    MaTacGia INT NOT NULL,
    MaNhaXuatBan INT,
    NamXuatBan INT,
    MaTheLoai INT,
    ISBN VARCHAR(20) UNIQUE,
    SoLuong INT NOT NULL DEFAULT 0,
    SoLuongConLai INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia),
    FOREIGN KEY (MaNhaXuatBan) REFERENCES NhaXuatBan(MaNhaXuatBan),
    FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai)
);
GO
INSERT INTO Sach (TieuDe, MaTacGia, MaNhaXuatBan, NamXuatBan, MaTheLoai, ISBN, SoLuong, SoLuongConLai) VALUES
(N'Mắt biếc', 1, 2, 2019, 2, '978-604-1-12345-6', 10, 10),
(N'Rừng Na Uy', 2, 1, 1987, 2, '978-0-375-70402-4', 8, 7),     -- Đang mượn 1
(N'Sapiens: Lược sử loài người', 3, 3, 2014, 3, '978-604-56-1111-1', 15, 14), -- Đang mượn 1
(N'Tôi tài giỏi, bạn cũng thế!', 4, 4, 2005, 6, '978-604-56-2222-2', 12, 11), -- Đang mượn 1
(N'It', 5, 5, 1986, 1, '978-0-451-16951-8', 7, 7), 
(N'Hoàng tử bé', 1, 2, 1943, 8, '978-604-1-33333-3', 20, 20),
(N'Kafka bên bờ biển', 2, 1, 2002, 2, '978-0-375-72502-9', 9, 9), 
(N'21 bài học cho thế kỷ 21', 3, 3, 2018, 3, '978-604-56-4444-4', 14, 13),  -- Đang mượn 1
(N'Bí mật tư duy triệu phú', 4, 4, 2005, 4, '978-604-56-5555-5', 11, 10),  -- Đang mượn 1
(N'The Shining', 5, 5, 1977, 1, '978-0-385-12167-5', 6, 5);       -- Đang mượn 1
GO
CREATE TABLE MuonTra (
    MaMuonTra INT PRIMARY KEY IDENTITY(1,1),
    MaDocGia VARCHAR(10) NOT NULL,
    MaSach INT NOT NULL,
    NgayMuon DATE DEFAULT GETDATE(),
    NgayTra DATE,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'Đang Mượn' CHECK (TrangThai IN (N'Đang Mượn', N'Đã Trả', N'Quá Hạn')),
    GhiChu NVARCHAR(MAX),
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);
GO
INSERT INTO MuonTra (MaDocGia, MaSach, NgayMuon, NgayTra, TrangThai, GhiChu) VALUES
('GN0001', 1, '2026-01-05', '2026-01-12', N'Đã Trả', N'Độc giả trả sách đúng hạn.'),
('GN0002', 3, '2026-02-25', NULL, N'Đang Mượn', N'Độc giả đang mượn sách, còn trong hạn.'),
('GN0003', 5, '2026-01-10', '2026-01-25', N'Đã Trả', N'Đã trả sách (Bị phạt do muộn 1 ngày).'),
('GN0004', 2, '2026-02-28', NULL, N'Đang Mượn', N'Độc giả mới mượn sách.'),
('GN0005', 7, '2026-02-15', '2026-02-20', N'Đã Trả', N'Độc giả trả sách sớm.'),
('GN0006', 4, '2026-01-18', NULL, N'Quá Hạn', N'Mượn từ tháng 1 chưa trả, đã phạt quá hạn.'),
('GN0007', 9, '2026-03-01', NULL, N'Đang Mượn', N'Sách mới mượn.'),
('GN0008', 6, '2026-02-05', '2026-02-10', N'Đã Trả', N''),
('GN0009', 8, '2026-02-20', NULL, N'Đang Mượn', N''),
('GN0010', 10, '2025-12-28', NULL, N'Quá Hạn', N'Độc giả làm mất hoặc cố tình chưa trả.');
GO
