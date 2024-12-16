CREATE DATABASE QLSanpham;
GO

USE QLSanpham;
GO

CREATE TABLE LoaiSP (
    MaLoai CHAR(2) PRIMARY KEY,
    TenLoai NVARCHAR(30)
);


CREATE TABLE Sanpham (
    MaSP CHAR(6) PRIMARY KEY,
    TenSP NVARCHAR(30),
    Ngaynhap DATETIME,
    MaLoai CHAR(2),
    FOREIGN KEY (MaLoai) REFERENCES LoaiSP(MaLoai)
);

INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES
('L1', N'Điện tử'),
('L2', N'Gia dụng');

INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) VALUES
('SP001', N'TV Samsung', '2024-06-01', 'L1'),
('SP002', N'Máy giặt LG', '2024-06-02', 'L2'),
('SP003', N'Lò vi sóng', '2024-06-03', 'L2');


