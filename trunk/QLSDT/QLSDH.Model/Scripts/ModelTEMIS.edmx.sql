
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/09/2023 15:20:35
-- Generated from EDMX file: F:\source\svn\DHSP\branches\2020 DHSP T9\Source\QLSDT\QLSDH.Model\ModelTEMIS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QLSDH_dev];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BaoVe_NCS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BaoVe_NCS];
GO
IF OBJECT_ID(N'[dbo].[BaoVeCapBoMon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BaoVeCapBoMon];
GO
IF OBJECT_ID(N'[dbo].[BaoVeCapTruong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BaoVeCapTruong];
GO
IF OBJECT_ID(N'[dbo].[BaoVeTongQuan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BaoVeTongQuan];
GO
IF OBJECT_ID(N'[dbo].[BieuMau]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BieuMau];
GO
IF OBJECT_ID(N'[dbo].[ChuongTrinhDaoTao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChuongTrinhDaoTao];
GO
IF OBJECT_ID(N'[dbo].[ChuyenMucVanBan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChuyenMucVanBan];
GO
IF OBJECT_ID(N'[dbo].[ChuyenNganhDaoTao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChuyenNganhDaoTao];
GO
IF OBJECT_ID(N'[dbo].[City]', 'U') IS NOT NULL
    DROP TABLE [dbo].[City];
GO
IF OBJECT_ID(N'[dbo].[CongTrinhKhoaHoc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CongTrinhKhoaHoc];
GO
IF OBJECT_ID(N'[dbo].[DangKyTuyenSinh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DangKyTuyenSinh];
GO
IF OBJECT_ID(N'[dbo].[DanhMucThongTin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhMucThongTin];
GO
IF OBJECT_ID(N'[dbo].[DanhMucTinTuc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhMucTinTuc];
GO
IF OBJECT_ID(N'[dbo].[DanhSachCanBoAddForm]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhSachCanBoAddForm];
GO
IF OBJECT_ID(N'[dbo].[DanhSachChuyenDe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhSachChuyenDe];
GO
IF OBJECT_ID(N'[dbo].[DanhSachHoiDong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhSachHoiDong];
GO
IF OBJECT_ID(N'[dbo].[DanToc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanToc];
GO
IF OBJECT_ID(N'[dbo].[Diem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Diem];
GO
IF OBJECT_ID(N'[dbo].[District]', 'U') IS NOT NULL
    DROP TABLE [dbo].[District];
GO
IF OBJECT_ID(N'[dbo].[DotTuyenSinh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DotTuyenSinh];
GO
IF OBJECT_ID(N'[dbo].[GiangVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiangVien];
GO
IF OBJECT_ID(N'[dbo].[GiayToBaoVeLuanAn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiayToBaoVeLuanAn];
GO
IF OBJECT_ID(N'[dbo].[HocPhan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HocPhan];
GO
IF OBJECT_ID(N'[dbo].[HocPhan_GiangVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HocPhan_GiangVien];
GO
IF OBJECT_ID(N'[dbo].[HocPhan_NCS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HocPhan_NCS];
GO
IF OBJECT_ID(N'[dbo].[HocPhi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HocPhi];
GO
IF OBJECT_ID(N'[dbo].[HocPhiNCS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HocPhiNCS];
GO
IF OBJECT_ID(N'[dbo].[HoiDongPhanBienDocLap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoiDongPhanBienDocLap];
GO
IF OBJECT_ID(N'[dbo].[HoSoThamDinh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoSoThamDinh];
GO
IF OBJECT_ID(N'[dbo].[KetQuaBaoVe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KetQuaBaoVe];
GO
IF OBJECT_ID(N'[dbo].[Khoa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Khoa];
GO
IF OBJECT_ID(N'[dbo].[KhoaHoc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhoaHoc];
GO
IF OBJECT_ID(N'[dbo].[KhoaHoc_HocVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhoaHoc_HocVien];
GO
IF OBJECT_ID(N'[dbo].[LichBaoVe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LichBaoVe];
GO
IF OBJECT_ID(N'[dbo].[LuanAnTienSi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LuanAnTienSi];
GO
IF OBJECT_ID(N'[dbo].[MonHoc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MonHoc];
GO
IF OBJECT_ID(N'[dbo].[MucHocPhi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MucHocPhi];
GO
IF OBJECT_ID(N'[dbo].[NCS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NCS];
GO
IF OBJECT_ID(N'[dbo].[NganhDaoTao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NganhDaoTao];
GO
IF OBJECT_ID(N'[dbo].[NguoiHuongDan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NguoiHuongDan];
GO
IF OBJECT_ID(N'[dbo].[PhanBienDocLap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhanBienDocLap];
GO
IF OBJECT_ID(N'[dbo].[PhongThi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhongThi];
GO
IF OBJECT_ID(N'[dbo].[QuyetDinh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuyetDinh];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[SauBaoVe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SauBaoVe];
GO
IF OBJECT_ID(N'[dbo].[SysComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysComment];
GO
IF OBJECT_ID(N'[dbo].[SysLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysLog];
GO
IF OBJECT_ID(N'[dbo].[SysNotification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysNotification];
GO
IF OBJECT_ID(N'[dbo].[SysSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysSetting];
GO
IF OBJECT_ID(N'[dbo].[ThamSoBieuMau]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThamSoBieuMau];
GO
IF OBJECT_ID(N'[dbo].[ThongBao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThongBao];
GO
IF OBJECT_ID(N'[dbo].[ThongTinDeTai]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThongTinDeTai];
GO
IF OBJECT_ID(N'[dbo].[ThuVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThuVien];
GO
IF OBJECT_ID(N'[dbo].[TieuBanChamChuyenDe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TieuBanChamChuyenDe];
GO
IF OBJECT_ID(N'[dbo].[TinTuc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TinTuc];
GO
IF OBJECT_ID(N'[dbo].[TruongThongTin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TruongThongTin];
GO
IF OBJECT_ID(N'[dbo].[TruongThongTin_NCS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TruongThongTin_NCS];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[VanBan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VanBan];
GO
IF OBJECT_ID(N'[dbo].[Ward]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ward];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'HocPhan_NCS'
CREATE TABLE [dbo].[HocPhan_NCS] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaNCS] nvarchar(50)  NULL,
    [TenHocPhan] nvarchar(max)  NULL,
    [TinChi] int  NULL,
    [Diem] float  NULL,
    [Status] bit  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [MaHocPhan] nvarchar(50)  NULL,
    [TuChon] bit  NULL,
    [MaMonHoc] nvarchar(50)  NULL,
    [TenMonHoc] nvarchar(50)  NULL,
    [DiemDieuKien] float  NULL,
    [DiemThi] float  NULL
);
GO

-- Creating table 'BieuMau'
CREATE TABLE [dbo].[BieuMau] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Template] nvarchar(500)  NULL,
    [FileUrl] nvarchar(max)  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL
);
GO

-- Creating table 'ChuongTrinhDaoTao'
CREATE TABLE [dbo].[ChuongTrinhDaoTao] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NghanhHoc] nvarchar(150)  NULL,
    [TenVietTat] nvarchar(50)  NULL,
    [ThuocNhomNghanh] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ChuyenMucVanBan'
CREATE TABLE [dbo].[ChuyenMucVanBan] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenChuyenMuc] nvarchar(500)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'DanhMucThongTin'
CREATE TABLE [dbo].[DanhMucThongTin] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] bit  NULL,
    [TenDanhMuc] nvarchar(500)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'DanhMucTinTuc'
CREATE TABLE [dbo].[DanhMucTinTuc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenDanhMuc] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'GiangVien'
CREATE TABLE [dbo].[GiangVien] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [NgaySinh] datetime  NULL,
    [GioiTinh] nvarchar(50)  NULL,
    [NoiSinh] nvarchar(250)  NULL,
    [HoKhau] nvarchar(250)  NULL,
    [DiaChi] nvarchar(350)  NULL,
    [SoDienThoai] nvarchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [ChucDanhId] int  NULL,
    [KhoaId] int  NULL,
    [RoleId] int  NULL,
    [Code] nvarchar(20)  NULL,
    [UserName] nvarchar(50)  NULL,
    [HocHamHocViId] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'HocPhan'
CREATE TABLE [dbo].[HocPhan] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenHocPhan] nvarchar(150)  NULL,
    [SoDVHT] int  NULL,
    [DieuKien] bit  NULL,
    [TuChon] bit  NULL,
    [SoTietLyThuyet] int  NULL,
    [SoTietThucHanh] int  NULL,
    [MaHocPhan] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [KhoaHocId] int  NULL,
    [KhoaId] int  NULL,
    [NganhId] int  NULL,
    [ChuyenNganhId] int  NULL,
    [SoTinChi] int  NULL,
    [LoaiHP] int  NULL
);
GO

-- Creating table 'Khoa'
CREATE TABLE [dbo].[Khoa] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenKhoa] nvarchar(150)  NULL,
    [TenVietTat] nvarchar(20)  NULL,
    [DiaChi] nvarchar(250)  NULL,
    [DienThoai] nvarchar(30)  NULL,
    [Email] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'KhoaHoc'
CREATE TABLE [dbo].[KhoaHoc] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MaKhoa] nvarchar(50)  NULL,
    [NgayKhaiGiang] datetime  NULL,
    [SoLuongHocVien] int  NULL,
    [TrangThai] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'KhoaHoc_HocVien'
CREATE TABLE [dbo].[KhoaHoc_HocVien] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [KhoaHocId] bigint  NULL,
    [HocVienId] bigint  NULL
);
GO

-- Creating table 'MonHoc'
CREATE TABLE [dbo].[MonHoc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaMon] nvarchar(50)  NULL,
    [TenMon] nvarchar(50)  NULL,
    [HocPhanId] int  NULL,
    [KhoaHocId] int  NULL,
    [KhoaId] int  NULL,
    [NganhId] int  NULL,
    [ChuyenNganhId] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'MucHocPhi'
CREATE TABLE [dbo].[MucHocPhi] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaKhoa] nvarchar(50)  NULL,
    [TenKhoa] nvarchar(50)  NULL,
    [NamHoc] nvarchar(50)  NULL,
    [HocPhi] float  NULL,
    [MaNganh] nvarchar(50)  NULL,
    [TenNganh] nvarchar(max)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'NCS'
CREATE TABLE [dbo].[NCS] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Ma] nvarchar(50)  NULL,
    [HoTen] nvarchar(500)  NULL,
    [NgaySinh] datetime  NULL,
    [NoiSinh] nvarchar(250)  NULL,
    [HoKhau] nvarchar(250)  NULL,
    [DiaChi] nvarchar(500)  NULL,
    [DienThoai] nvarchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [GioiTinh] nvarchar(50)  NULL,
    [DanToc] nvarchar(50)  NULL,
    [ChucDanhId] int  NULL,
    [KhoaId] int  NULL,
    [QuocTich] nvarchar(50)  NULL,
    [Type] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [KHoaHocId] int  NULL,
    [NganhId] int  NULL,
    [NganhDaoTaoId] int  NULL
);
GO

-- Creating table 'NganhDaoTao'
CREATE TABLE [dbo].[NganhDaoTao] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaNganh] nvarchar(20)  NULL,
    [TenNganh] nvarchar(150)  NULL,
    [KhoaId] int  NULL,
    [TenKhoa] nvarchar(150)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'PhongThi'
CREATE TABLE [dbo].[PhongThi] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MaPhongThi] nvarchar(30)  NULL,
    [MaNghanh] int  NULL,
    [ChuyenNghanh] nvarchar(50)  NULL,
    [NgoaiNgu] nvarchar(50)  NULL,
    [LoaiThiSinh] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [isLock] bit  NULL
);
GO

-- Creating table 'SysLog'
CREATE TABLE [dbo].[SysLog] (
    [Id] uniqueidentifier  NOT NULL,
    [Action] nvarchar(50)  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [LoggedAt] datetime  NULL,
    [Controller] nvarchar(150)  NULL,
    [IPAddress] nvarchar(150)  NULL,
    [AreaAccessed] nvarchar(max)  NULL
);
GO

-- Creating table 'ThamSoBieuMau'
CREATE TABLE [dbo].[ThamSoBieuMau] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenThamSo] nvarchar(50)  NULL,
    [GiaTriThamSo] nvarchar(max)  NULL,
    [KieuDuLieu] nvarchar(50)  NULL,
    [CauTrucHienThi] nvarchar(300)  NULL,
    [ThuTuHienThi] int  NULL,
    [BieuMau] nvarchar(max)  NULL,
    [BieuMauId] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ThuVien'
CREATE TABLE [dbo].[ThuVien] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [KhoaId] int  NULL,
    [KhoaHocId] int  NULL,
    [NganhId] int  NULL,
    [ChuyenNganhId] int  NULL,
    [HoTen] nvarchar(350)  NULL,
    [NgaySinh] datetime  NULL,
    [NopLan1] bit  NULL,
    [UrlFileLan1] nvarchar(400)  NULL,
    [NopLan2] bit  NULL,
    [UrlFileLan2] nvarchar(400)  NULL,
    [QDBV_CapTruong] datetime  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [MaNCS] nvarchar(50)  NULL
);
GO

-- Creating table 'TinTuc'
CREATE TABLE [dbo].[TinTuc] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TieuDe] nvarchar(300)  NULL,
    [MoTaNgan] nvarchar(550)  NULL,
    [NoiDung] nvarchar(max)  NULL,
    [AnhDaiDien] nvarchar(550)  NULL,
    [DanhMuc] int  NULL,
    [NguoiTao] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NULL,
    [CountView] int  NULL
);
GO

-- Creating table 'TruongThongTin'
CREATE TABLE [dbo].[TruongThongTin] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdDanhMuc] int  NULL,
    [TenTruongThongTin] nvarchar(max)  NULL,
    [LoaiTruongThongTin] nchar(300)  NULL,
    [Status] bit  NULL,
    [CreatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedAt] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'VanBan'
CREATE TABLE [dbo].[VanBan] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TieuDe] nvarchar(150)  NULL,
    [URL] nvarchar(250)  NULL,
    [ChuyenMuc] int  NULL,
    [DauMuc] nvarchar(50)  NULL,
    [Anh] nvarchar(350)  NULL,
    [HinhThuc] int  NULL,
    [TrangThai] bit  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'BaoVeCapBoMon'
CREATE TABLE [dbo].[BaoVeCapBoMon] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HinhThuc] int  NULL,
    [File] nvarchar(550)  NULL,
    [SoQD] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [HoTen] nvarchar(50)  NULL,
    [CoquanCT] nvarchar(50)  NULL,
    [VaiTro] nvarchar(50)  NULL,
    [TenLanBV] nvarchar(150)  NULL,
    [GioBV] datetime  NULL,
    [NgayBV] datetime  NULL,
    [DiaDiem] nvarchar(150)  NULL,
    [KetQua] bit  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'BaoVeCapTruong'
CREATE TABLE [dbo].[BaoVeCapTruong] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HinhThuc] int  NULL,
    [File] nvarchar(550)  NULL,
    [SoQD] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [HoTen] nvarchar(50)  NULL,
    [CoquanCT] nvarchar(150)  NULL,
    [VaiTro] nvarchar(50)  NULL,
    [TenLanBV] nvarchar(250)  NULL,
    [GioBV] nvarchar(50)  NULL,
    [NgayBV] datetime  NULL,
    [DiaDiem] nvarchar(250)  NULL,
    [KetQua] bit  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [TenGiayTo] nvarchar(150)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'LuanAnTienSi'
CREATE TABLE [dbo].[LuanAnTienSi] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [File] nvarchar(550)  NULL,
    [Khoa] int  NULL,
    [KhoaHoc] int  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [HoTen] nvarchar(50)  NULL,
    [NgaySinh] datetime  NULL,
    [Lop] int  NULL,
    [ChuyenNghanh] int  NULL,
    [NoiDung] nvarchar(max)  NULL,
    [TrangThai] bit  NULL,
    [QDBVTruong] nvarchar(150)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'PhanBienDocLap'
CREATE TABLE [dbo].[PhanBienDocLap] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [TrachNhiem] nvarchar(50)  NULL,
    [SDT] nvarchar(20)  NULL,
    [File] nvarchar(550)  NULL,
    [LanGui] nvarchar(50)  NULL,
    [NgayGui] datetime  NULL,
    [NgaNhan] datetime  NULL,
    [KetQua] nvarchar(150)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'SauBaoVe'
CREATE TABLE [dbo].[SauBaoVe] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MaNCS] nvarchar(50)  NULL,
    [File] nvarchar(550)  NULL,
    [NoiDung] nvarchar(max)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL,
    [UrlBienLai] nvarchar(max)  NULL
);
GO

-- Creating table 'CongTrinhKhoaHoc'
CREATE TABLE [dbo].[CongTrinhKhoaHoc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NamCB] nvarchar(50)  NULL,
    [TenCTKH] nvarchar(150)  NULL,
    [TenTapChi] nvarchar(150)  NULL,
    [NoiXB] nvarchar(150)  NULL,
    [File] nvarchar(550)  NULL,
    [VaiTro] nvarchar(50)  NULL,
    [HinhThuc] nvarchar(50)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'DanhSachChuyenDe'
CREATE TABLE [dbo].[DanhSachChuyenDe] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenChuyenDe] nvarchar(max)  NULL,
    [SoTinChi] int  NULL,
    [NgayBaoVe] datetime  NULL,
    [DiemSo] float  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'TieuBanChamChuyenDe'
CREATE TABLE [dbo].[TieuBanChamChuyenDe] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [CoQuanCongTac] nvarchar(150)  NULL,
    [VaiTroThamGia] nvarchar(150)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'BaoVeTongQuan'
CREATE TABLE [dbo].[BaoVeTongQuan] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [File] nvarchar(350)  NULL,
    [SoQD] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [LoaiQd] nvarchar(50)  NULL,
    [LyDoDieuChinh] nvarchar(max)  NULL,
    [VaiTro] nvarchar(50)  NULL,
    [CoquanCT] nvarchar(150)  NULL,
    [HoTen] nvarchar(50)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'BaoVe_NCS'
CREATE TABLE [dbo].[BaoVe_NCS] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Buoc1] int  NULL,
    [Buoc2] int  NULL,
    [Buoc3] int  NULL,
    [Buoc4] int  NULL,
    [Buoc5] int  NULL,
    [Buoc6] int  NULL,
    [Buoc7] int  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [Status] int  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL
);
GO

-- Creating table 'DanhSachHoiDong'
CREATE TABLE [dbo].[DanhSachHoiDong] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [CoQuanCongTac] nvarchar(max)  NULL,
    [VaiTroThamGia] nvarchar(150)  NULL,
    [Type] int  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [Tab] int  NULL,
    [MaNCS] nvarchar(50)  NULL
);
GO

-- Creating table 'KetQuaBaoVe'
CREATE TABLE [dbo].[KetQuaBaoVe] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenLanBaoVe] nvarchar(50)  NULL,
    [GioBaoVe] nvarchar(50)  NULL,
    [NgayBaoVe] datetime  NULL,
    [DiaDiem] nvarchar(50)  NULL,
    [KetQuaBaoVe] nvarchar(50)  NULL,
    [Type] int  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [Tab] int  NULL,
    [MaNCS] nvarchar(50)  NULL
);
GO

-- Creating table 'QuyetDinh'
CREATE TABLE [dbo].[QuyetDinh] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [SoQuyetDinh] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [Type] int  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [Tab] int  NULL,
    [MaNCS] nvarchar(50)  NULL
);
GO

-- Creating table 'NguoiHuongDan'
CREATE TABLE [dbo].[NguoiHuongDan] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [CoQuanCongtac] nvarchar(50)  NULL,
    [VaiTroThamGia] nvarchar(50)  NULL,
    [LoaiQuyetDinh] nvarchar(50)  NULL,
    [SoQDDieuChinhNHD] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL
);
GO

-- Creating table 'HoiDongPhanBienDocLap'
CREATE TABLE [dbo].[HoiDongPhanBienDocLap] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [DienThoai] nvarchar(50)  NULL,
    [TrachNhiem] nvarchar(150)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [ThuTuHienThi] int  NULL
);
GO

-- Creating table 'Diem'
CREATE TABLE [dbo].[Diem] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HocVienId] bigint  NULL,
    [MaHocVien] nvarchar(20)  NULL,
    [TenHocVien] nvarchar(50)  NULL,
    [HocPhanId] bigint  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [DiemHP1] float  NULL,
    [DiemHP2] float  NULL,
    [DiemHP3] float  NULL,
    [DiemHP4] float  NULL,
    [SoDiem] float  NULL
);
GO

-- Creating table 'GiayToBaoVeLuanAn'
CREATE TABLE [dbo].[GiayToBaoVeLuanAn] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenGiayTo] nvarchar(500)  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [MaNCS] nvarchar(50)  NULL
);
GO

-- Creating table 'LichBaoVe'
CREATE TABLE [dbo].[LichBaoVe] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenDeTai] nvarchar(max)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [TenNCS] nvarchar(50)  NULL,
    [KhoaHoc] nvarchar(max)  NULL,
    [ChuyenNganhId] int  NULL,
    [TenChuyenNganh] nvarchar(350)  NULL,
    [CapBaoVe] nvarchar(350)  NULL,
    [GioBaoVe] nvarchar(350)  NULL,
    [NgayBaoVe] datetime  NULL,
    [DiaDiem] nvarchar(max)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'TruongThongTin_NCS'
CREATE TABLE [dbo].[TruongThongTin_NCS] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TruongThongTinId] int  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [Url] nvarchar(max)  NULL,
    [Status] int  NULL,
    [CreatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedAt] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'HoSoThamDinh'
CREATE TABLE [dbo].[HoSoThamDinh] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MaNCS] nvarchar(50)  NULL,
    [DotTĐ] nvarchar(50)  NULL,
    [LoaiThamDinh] nvarchar(50)  NULL,
    [SoCV] int  NULL,
    [NgayKy] datetime  NULL,
    [KetQua] nvarchar(250)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'ThongTinDeTai'
CREATE TABLE [dbo].[ThongTinDeTai] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TenDeTai] nvarchar(500)  NULL,
    [SoQuyetDinh] nvarchar(50)  NULL,
    [NgayKy] datetime  NULL,
    [FileKiemChung] nvarchar(150)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [NHD1] nvarchar(500)  NULL,
    [NHD2] nvarchar(500)  NULL,
    [NhomBaoCao] nvarchar(50)  NULL,
    [NgayHop] datetime  NULL,
    [GioHop] nvarchar(50)  NULL,
    [DiaDiem] nvarchar(500)  NULL
);
GO

-- Creating table 'HocPhan_GiangVien'
CREATE TABLE [dbo].[HocPhan_GiangVien] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [YearId] int  NULL,
    [GradeId] int  NULL,
    [ConversionId] int  NULL,
    [Departmentid] int  NULL,
    [ForDepartmentId] int  NULL,
    [Name] nvarchar(500)  NULL,
    [LessionTime] float  NULL,
    [TeachingTime] nvarchar(500)  NULL,
    [Class] nvarchar(max)  NULL,
    [Size] int  NULL,
    [Paid] bit  NULL,
    [Course] nvarchar(50)  NULL,
    [Desc] nvarchar(max)  NULL,
    [Invited] bit  NULL,
    [Specializing] nvarchar(250)  NULL,
    [InvitedPartner] nvarchar(500)  NULL,
    [LinkedPartner] nvarchar(500)  NULL,
    [SubjectName] nvarchar(500)  NULL,
    [InvitedDegreeId] int  NULL,
    [StaffId] int  NULL,
    [SemesterId] int  NULL,
    [Status] bit  NULL,
    [IdHocPhan] bigint  NULL,
    [IdGiangVien] bigint  NULL,
    [LoaiGiangVien] int  NULL,
    [KhoaId] int  NULL
);
GO

-- Creating table 'SysSetting'
CREATE TABLE [dbo].[SysSetting] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SKey] nvarchar(50)  NULL,
    [Name] nvarchar(50)  NULL,
    [Value] nvarchar(max)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(150)  NULL,
    [Email] nvarchar(150)  NULL,
    [PassWord] nvarchar(250)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [UpdatedBy] nvarchar(150)  NULL,
    [CreatedBy] nvarchar(150)  NULL,
    [IsLock] bit  NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(250)  NULL,
    [Email] nvarchar(250)  NULL,
    [Role] nvarchar(250)  NULL,
    [IsLock] bit  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ChuyenNganhDaoTao'
CREATE TABLE [dbo].[ChuyenNganhDaoTao] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaChuyenNganh] nvarchar(20)  NULL,
    [TenChuyenNganh] nvarchar(150)  NULL,
    [KhoaId] int  NULL,
    [NganhId] int  NULL,
    [TenKhoa] nvarchar(150)  NULL,
    [TenNganh] nvarchar(150)  NULL,
    [MaSoCN_ThS] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ThongBao'
CREATE TABLE [dbo].[ThongBao] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(550)  NULL,
    [MaNCS] nvarchar(150)  NULL,
    [Email] nvarchar(150)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [TrangThai] int  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'City'
CREATE TABLE [dbo].[City] (
    [ItemID] int IDENTITY(1,1) NOT NULL,
    [CityCode] varchar(50)  NULL,
    [Name] nvarchar(250)  NULL,
    [Type] nvarchar(50)  NULL
);
GO

-- Creating table 'District'
CREATE TABLE [dbo].[District] (
    [ItemID] int IDENTITY(1,1) NOT NULL,
    [DistrictCode] nvarchar(50)  NULL,
    [Name] nvarchar(250)  NULL,
    [Type] nvarchar(50)  NULL,
    [Location] nvarchar(50)  NULL,
    [CityCode] nvarchar(50)  NULL
);
GO

-- Creating table 'Ward'
CREATE TABLE [dbo].[Ward] (
    [ItemID] bigint IDENTITY(1,1) NOT NULL,
    [WardCode] nvarchar(50)  NULL,
    [Name] nvarchar(250)  NULL,
    [Type] nvarchar(50)  NULL,
    [Location] nvarchar(50)  NULL,
    [DistrictCode] nvarchar(50)  NULL
);
GO

-- Creating table 'HocPhiNCS'
CREATE TABLE [dbo].[HocPhiNCS] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaHV] nvarchar(50)  NULL,
    [Email] nvarchar(500)  NULL,
    [HoTen] nvarchar(500)  NULL,
    [TongTien] float  NULL,
    [DaTra] float  NULL,
    [HoanThanh] bit  NULL,
    [QuaHan] bit  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'DotTuyenSinh'
CREATE TABLE [dbo].[DotTuyenSinh] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [idKhoahoc] bigint  NULL,
    [MaKhoaHoc] nvarchar(50)  NULL,
    [TenDot] nvarchar(150)  NULL,
    [NgayBatDau] datetime  NULL,
    [NgayKetThuc] datetime  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'SysNotification'
CREATE TABLE [dbo].[SysNotification] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(150)  NULL,
    [Email] nvarchar(150)  NULL,
    [Title] nvarchar(max)  NULL,
    [Message] nvarchar(max)  NULL,
    [AttachFile] nvarchar(max)  NULL,
    [Priority] int  NULL,
    [CreatedAt] datetime  NULL,
    [CreatedBy] nvarchar(150)  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'HocPhi'
CREATE TABLE [dbo].[HocPhi] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [File] nvarchar(550)  NULL,
    [Khoa] int  NULL,
    [TenKhoa] nvarchar(150)  NULL,
    [ChuyennNghanh] int  NOT NULL,
    [TenChuyenNganh] nvarchar(150)  NULL,
    [KhoaHoc] int  NULL,
    [TenKhoaHoc] nvarchar(50)  NULL,
    [HoTen] nvarchar(50)  NULL,
    [NoiDung] nvarchar(max)  NULL,
    [TrangThai] int  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [MucNop] float  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'SysComment'
CREATE TABLE [dbo].[SysComment] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MaNCS] nvarchar(50)  NULL,
    [EmailNCS] nvarchar(250)  NULL,
    [Status] int  NULL,
    [Description] nvarchar(max)  NULL,
    [Category] nvarchar(max)  NULL
);
GO

-- Creating table 'DangKyTuyenSinh'
CREATE TABLE [dbo].[DangKyTuyenSinh] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [GioiTinh] nvarchar(20)  NULL,
    [NgaySinh] datetime  NULL,
    [SoDienThoai] nvarchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [NoiSinh] nvarchar(150)  NULL,
    [DiaChiLienLac] nvarchar(250)  NULL,
    [NgheNghiep] nvarchar(50)  NULL,
    [CoQuanCongTacHienNay] nvarchar(250)  NULL,
    [NamBatDauCongTac] nvarchar(20)  NULL,
    [HienLaCanBo] nvarchar(50)  NULL,
    [ViTriConViecHienTai] nvarchar(50)  NULL,
    [ThamNiemNghieNghiep] nvarchar(50)  NULL,
    [ChuyenMon] nvarchar(50)  NULL,
    [Truong_TN_DaiHoc] nvarchar(150)  NULL,
    [Nam_TN_DaiHoc] int  NULL,
    [HeDaoTao_DaiHoc] nvarchar(50)  NULL,
    [Nghanh_TN_DaiHoc] nvarchar(150)  NULL,
    [DiemTrungBinh_DaiHoc] nvarchar(50)  NULL,
    [LoaiTotNghiep_DaiHoc] nvarchar(50)  NULL,
    [Url_FileUpload_DaiHoc] nvarchar(550)  NULL,
    [Truong_TN_ThacSi] nvarchar(150)  NULL,
    [Nam_TN_ThacSi] nvarchar(50)  NULL,
    [HeDaoTao_ThacSi] nvarchar(50)  NULL,
    [Nghanh_TN_ThacSi] nvarchar(150)  NULL,
    [DiemTrungBinh_ThacSi] nvarchar(50)  NULL,
    [Url_FileUpload_ThacSi] nvarchar(550)  NULL,
    [NgoaiNgu] nvarchar(50)  NULL,
    [LoaiVanBangNgoaiNgu] nvarchar(50)  NULL,
    [Url_ChungChiNgoaiNgu] nvarchar(550)  NULL,
    [BoTucKienThuc] nvarchar(550)  NULL,
    [ChuyenNghanhDuTuyenId] int  NOT NULL,
    [TenChuyenNghanhDuTuyen] nvarchar(100)  NULL,
    [DoiTuongDuTuyen] nvarchar(50)  NULL,
    [ThoiGianHinhThucDaoTao] nvarchar(150)  NULL,
    [Status] int  NULL,
    [TenDeTai] nvarchar(500)  NULL,
    [KhoaId_NHD1] int  NULL,
    [Id_NHD1] int  NULL,
    [NHD1] nvarchar(500)  NULL,
    [KhoaId_NHD2] int  NULL,
    [Id_NHD2] int  NULL,
    [NHD2] nvarchar(500)  NULL,
    [CoQuanCongTac_NHD2] nvarchar(500)  NULL,
    [NoiDungPhanHoi] nvarchar(max)  NULL,
    [TepFilePhanHoi] nvarchar(500)  NULL,
    [MaNCS] nvarchar(50)  NULL,
    [Truong_TN_VB2] nvarchar(500)  NULL,
    [Nam_TN_VB2] int  NULL,
    [HeDaoTao_VB2] nvarchar(500)  NULL,
    [Nganh_TN_VB2] nvarchar(500)  NULL,
    [LoaiTotNghiep_VB2] nvarchar(150)  NULL,
    [Url_FileUpload_VB2] nvarchar(150)  NULL,
    [DiemTrungBinh_VB2] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [CapQuyenTruyCap] bit  NULL,
    [SoCMND] nvarchar(50)  NULL,
    [Ngaycap_CMND] datetime  NULL,
    [Noicap_CMND] nvarchar(500)  NULL,
    [TinhThanh_CMND] nvarchar(500)  NULL,
    [Quan_CMND] nvarchar(500)  NULL,
    [Xa_CMND] nvarchar(500)  NULL,
    [Url_FileUpload_AnhSoYeuLyLich] nvarchar(500)  NULL,
    [Url_FileUpload_CongVanGioiThieu] nvarchar(500)  NULL,
    [Url_FileUpload_GiaySucKhoe] nvarchar(500)  NULL,
    [Url_FileUpload_HopDongLaoDong] nvarchar(500)  NULL,
    [Url_FileUpload_ThuGioiThieu] nvarchar(500)  NULL,
    [Url_FileUpload_BaiBaoKhoaHoc] nvarchar(500)  NULL,
    [Url_FileUpload_DeCuongNghienCuu] nvarchar(500)  NULL,
    [Url_FileUpload_Zip] nvarchar(500)  NULL,
    [TenNganh] nvarchar(500)  NULL,
    [TenKhoa] nvarchar(500)  NULL,
    [NganhId] int  NULL,
    [KhoaId] int  NULL,
    [IdDotTS] int  NULL,
    [DanToc] nvarchar(100)  NULL,
    [TrangThaiTuyenSinh] int  NULL,
    [DiemDeCuong] float  NULL,
    [DiemTong] float  NULL
);
GO

-- Creating table 'DanhSachCanBoAddForm'
CREATE TABLE [dbo].[DanhSachCanBoAddForm] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdCanBo] int  NULL,
    [TenCanBo] nvarchar(500)  NULL,
    [ChucVu] nvarchar(500)  NULL,
    [VaiTro] nvarchar(500)  NULL,
    [IdBieuMau] int  NULL,
    [HocHamHocVi] nvarchar(500)  NULL,
    [CoQuanCongTac] nvarchar(500)  NULL,
    [IdKhoa] int  NULL,
    [TenKhoa] nvarchar(500)  NULL,
    [IdKhoahoc] int  NULL,
    [IdDotTS] int  NULL,
    [Status] bit  NULL,
    [IdKhoaSelected] int  NULL,
    [IdChuyenNganh] int  NULL,
    [TenChuyenNganh] nvarchar(500)  NULL
);
GO

-- Creating table 'DanToc'
CREATE TABLE [dbo].[DanToc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Ma] nvarchar(10)  NULL,
    [TenDanToc] nvarchar(50)  NULL,
    [TenKhac] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'HocPhan_NCS'
ALTER TABLE [dbo].[HocPhan_NCS]
ADD CONSTRAINT [PK_HocPhan_NCS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BieuMau'
ALTER TABLE [dbo].[BieuMau]
ADD CONSTRAINT [PK_BieuMau]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ChuongTrinhDaoTao'
ALTER TABLE [dbo].[ChuongTrinhDaoTao]
ADD CONSTRAINT [PK_ChuongTrinhDaoTao]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ChuyenMucVanBan'
ALTER TABLE [dbo].[ChuyenMucVanBan]
ADD CONSTRAINT [PK_ChuyenMucVanBan]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanhMucThongTin'
ALTER TABLE [dbo].[DanhMucThongTin]
ADD CONSTRAINT [PK_DanhMucThongTin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanhMucTinTuc'
ALTER TABLE [dbo].[DanhMucTinTuc]
ADD CONSTRAINT [PK_DanhMucTinTuc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GiangVien'
ALTER TABLE [dbo].[GiangVien]
ADD CONSTRAINT [PK_GiangVien]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HocPhan'
ALTER TABLE [dbo].[HocPhan]
ADD CONSTRAINT [PK_HocPhan]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Khoa'
ALTER TABLE [dbo].[Khoa]
ADD CONSTRAINT [PK_Khoa]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KhoaHoc'
ALTER TABLE [dbo].[KhoaHoc]
ADD CONSTRAINT [PK_KhoaHoc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KhoaHoc_HocVien'
ALTER TABLE [dbo].[KhoaHoc_HocVien]
ADD CONSTRAINT [PK_KhoaHoc_HocVien]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MonHoc'
ALTER TABLE [dbo].[MonHoc]
ADD CONSTRAINT [PK_MonHoc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MucHocPhi'
ALTER TABLE [dbo].[MucHocPhi]
ADD CONSTRAINT [PK_MucHocPhi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NCS'
ALTER TABLE [dbo].[NCS]
ADD CONSTRAINT [PK_NCS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NganhDaoTao'
ALTER TABLE [dbo].[NganhDaoTao]
ADD CONSTRAINT [PK_NganhDaoTao]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhongThi'
ALTER TABLE [dbo].[PhongThi]
ADD CONSTRAINT [PK_PhongThi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysLog'
ALTER TABLE [dbo].[SysLog]
ADD CONSTRAINT [PK_SysLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThamSoBieuMau'
ALTER TABLE [dbo].[ThamSoBieuMau]
ADD CONSTRAINT [PK_ThamSoBieuMau]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThuVien'
ALTER TABLE [dbo].[ThuVien]
ADD CONSTRAINT [PK_ThuVien]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TinTuc'
ALTER TABLE [dbo].[TinTuc]
ADD CONSTRAINT [PK_TinTuc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TruongThongTin'
ALTER TABLE [dbo].[TruongThongTin]
ADD CONSTRAINT [PK_TruongThongTin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VanBan'
ALTER TABLE [dbo].[VanBan]
ADD CONSTRAINT [PK_VanBan]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BaoVeCapBoMon'
ALTER TABLE [dbo].[BaoVeCapBoMon]
ADD CONSTRAINT [PK_BaoVeCapBoMon]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BaoVeCapTruong'
ALTER TABLE [dbo].[BaoVeCapTruong]
ADD CONSTRAINT [PK_BaoVeCapTruong]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LuanAnTienSi'
ALTER TABLE [dbo].[LuanAnTienSi]
ADD CONSTRAINT [PK_LuanAnTienSi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhanBienDocLap'
ALTER TABLE [dbo].[PhanBienDocLap]
ADD CONSTRAINT [PK_PhanBienDocLap]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SauBaoVe'
ALTER TABLE [dbo].[SauBaoVe]
ADD CONSTRAINT [PK_SauBaoVe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CongTrinhKhoaHoc'
ALTER TABLE [dbo].[CongTrinhKhoaHoc]
ADD CONSTRAINT [PK_CongTrinhKhoaHoc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanhSachChuyenDe'
ALTER TABLE [dbo].[DanhSachChuyenDe]
ADD CONSTRAINT [PK_DanhSachChuyenDe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TieuBanChamChuyenDe'
ALTER TABLE [dbo].[TieuBanChamChuyenDe]
ADD CONSTRAINT [PK_TieuBanChamChuyenDe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BaoVeTongQuan'
ALTER TABLE [dbo].[BaoVeTongQuan]
ADD CONSTRAINT [PK_BaoVeTongQuan]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BaoVe_NCS'
ALTER TABLE [dbo].[BaoVe_NCS]
ADD CONSTRAINT [PK_BaoVe_NCS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanhSachHoiDong'
ALTER TABLE [dbo].[DanhSachHoiDong]
ADD CONSTRAINT [PK_DanhSachHoiDong]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KetQuaBaoVe'
ALTER TABLE [dbo].[KetQuaBaoVe]
ADD CONSTRAINT [PK_KetQuaBaoVe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuyetDinh'
ALTER TABLE [dbo].[QuyetDinh]
ADD CONSTRAINT [PK_QuyetDinh]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NguoiHuongDan'
ALTER TABLE [dbo].[NguoiHuongDan]
ADD CONSTRAINT [PK_NguoiHuongDan]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoiDongPhanBienDocLap'
ALTER TABLE [dbo].[HoiDongPhanBienDocLap]
ADD CONSTRAINT [PK_HoiDongPhanBienDocLap]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Diem'
ALTER TABLE [dbo].[Diem]
ADD CONSTRAINT [PK_Diem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GiayToBaoVeLuanAn'
ALTER TABLE [dbo].[GiayToBaoVeLuanAn]
ADD CONSTRAINT [PK_GiayToBaoVeLuanAn]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LichBaoVe'
ALTER TABLE [dbo].[LichBaoVe]
ADD CONSTRAINT [PK_LichBaoVe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TruongThongTin_NCS'
ALTER TABLE [dbo].[TruongThongTin_NCS]
ADD CONSTRAINT [PK_TruongThongTin_NCS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoSoThamDinh'
ALTER TABLE [dbo].[HoSoThamDinh]
ADD CONSTRAINT [PK_HoSoThamDinh]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThongTinDeTai'
ALTER TABLE [dbo].[ThongTinDeTai]
ADD CONSTRAINT [PK_ThongTinDeTai]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HocPhan_GiangVien'
ALTER TABLE [dbo].[HocPhan_GiangVien]
ADD CONSTRAINT [PK_HocPhan_GiangVien]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysSetting'
ALTER TABLE [dbo].[SysSetting]
ADD CONSTRAINT [PK_SysSetting]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ChuyenNganhDaoTao'
ALTER TABLE [dbo].[ChuyenNganhDaoTao]
ADD CONSTRAINT [PK_ChuyenNganhDaoTao]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ThongBao'
ALTER TABLE [dbo].[ThongBao]
ADD CONSTRAINT [PK_ThongBao]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ItemID] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [PK_City]
    PRIMARY KEY CLUSTERED ([ItemID] ASC);
GO

-- Creating primary key on [ItemID] in table 'District'
ALTER TABLE [dbo].[District]
ADD CONSTRAINT [PK_District]
    PRIMARY KEY CLUSTERED ([ItemID] ASC);
GO

-- Creating primary key on [ItemID] in table 'Ward'
ALTER TABLE [dbo].[Ward]
ADD CONSTRAINT [PK_Ward]
    PRIMARY KEY CLUSTERED ([ItemID] ASC);
GO

-- Creating primary key on [Id] in table 'HocPhiNCS'
ALTER TABLE [dbo].[HocPhiNCS]
ADD CONSTRAINT [PK_HocPhiNCS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DotTuyenSinh'
ALTER TABLE [dbo].[DotTuyenSinh]
ADD CONSTRAINT [PK_DotTuyenSinh]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysNotification'
ALTER TABLE [dbo].[SysNotification]
ADD CONSTRAINT [PK_SysNotification]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HocPhi'
ALTER TABLE [dbo].[HocPhi]
ADD CONSTRAINT [PK_HocPhi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysComment'
ALTER TABLE [dbo].[SysComment]
ADD CONSTRAINT [PK_SysComment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DangKyTuyenSinh'
ALTER TABLE [dbo].[DangKyTuyenSinh]
ADD CONSTRAINT [PK_DangKyTuyenSinh]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanhSachCanBoAddForm'
ALTER TABLE [dbo].[DanhSachCanBoAddForm]
ADD CONSTRAINT [PK_DanhSachCanBoAddForm]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DanToc'
ALTER TABLE [dbo].[DanToc]
ADD CONSTRAINT [PK_DanToc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------