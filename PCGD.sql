﻿use PCGD
go

CREATE TABLE Khoa
(
	ID INT NOT NULL IDENTITY(1,1),
	TenKhoa NVARCHAR(255) NOT NULL UNIQUE,
	PRIMARY KEY (ID)
);

CREATE TABLE HocPhan
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	MaHP CHAR(6) NOT NULL UNIQUE,
	TenHP NVARCHAR(255) NOT NULL,
	LoaiHP TINYINT NOT NULL,
	SoTC INT NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE Nganh
(
	ID INT NOT NULL IDENTITY(1,1),
	Khoa_ID INT NOT NULL,
	TenNganh NVARCHAR(255) NOT NULL UNIQUE,
	PRIMARY KEY (ID),
	FOREIGN KEY (Khoa_ID) REFERENCES Khoa(ID) ON DELETE CASCADE
);

CREATE TABLE ChuongTrinh
(
	ID INT NOT NULL IDENTITY(1,1),
	TenCT NVARCHAR(255) NOT NULL UNIQUE,
	NgayTao DATE NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE HocKi
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	ChuongTrinh_ID INT NOT NULL,
	SoHocKi TINYINT NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (ChuongTrinh_ID) REFERENCES ChuongTrinh(ID) ON DELETE CASCADE
);

CREATE TABLE NhomHocPhan
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	HocKi_ID BIGINT NOT NULL,
	HocPhanDieuKien TINYINT NOT NULL,
	HocPhanThayThe TINYINT NOT NULL,
	TongTC INT NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (HocKi_ID) REFERENCES HocKi(ID) ON DELETE CASCADE
);

CREATE TABLE ChiTietHocPhan
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	NhomHocPhan_ID BIGINT NOT NULL,
	HocPhan_ID BIGINT NOT NULL,
	SoTietLT INT,
	SoTietTH INT,
	PRIMARY KEY (id),
	FOREIGN KEY (NhomHocPhan_ID) REFERENCES NhomHocPhan(ID) ON DELETE CASCADE,
	FOREIGN KEY (HocPhan_ID) REFERENCES HocPhan(ID) ON DELETE CASCADE
);

CREATE TABLE Lop
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	Nganh_ID INT NOT NULL,
	ChuongTrinh_ID INT NOT NULL,
	TenLop VARCHAR(7) NOT NULL UNIQUE,
	SoSV INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (Nganh_ID) REFERENCES Nganh(ID) ON DELETE CASCADE,
	FOREIGN KEY (ChuongTrinh_ID) REFERENCES ChuongTrinh(ID) ON DELETE CASCADE,
);

CREATE TABLE GiangVien
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	TenGV NVARCHAR(255) NOT NULL UNIQUE,
	PRIMARY KEY (id)
);

CREATE TABLE ChiTietGiangVien
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	GiangVien_ID BIGINT NOT NULL,
	HocPhan_ID BIGINT NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (GiangVien_ID) REFERENCES GiangVien(ID) ON DELETE CASCADE,
	FOREIGN KEY (HocPhan_ID) REFERENCES HocPhan(ID) ON DELETE CASCADE
);

CREATE TABLE NguoiDung
(
	ID INT NOT NULL IDENTITY(1,1),
	QuyenHan TINYINT NOT NULL,
	TaiKhoan NVARCHAR(100) NOT NULL,
	MatKhau CHAR(40) NOT NULL,
	NgayTao DATETIME NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE PhanCong
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	NguoiDung_ID INT NOT NULL,
	NamHoc INT NOT NULL,
	HocKi TINYINT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (NguoiDung_ID) REFERENCES NguoiDung(ID) ON DELETE CASCADE
);

CREATE TABLE TongHop
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	NamHoc TINYINT NOT NULL UNIQUE,
	PRIMARY KEY (ID)
);

CREATE TABLE ChiTietTongHop
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	TongHop_ID BIGINT NOT NULL,
	GiangVien_ID BIGINT NOT NULL,
	DinhMucGioChuan INT,
	DinhMucCongTac INT,
	GiamDinhMuc FLOAT,
	GhiChu NVARCHAR(255),
	PRIMARY KEY (id),
	FOREIGN KEY (GiangVien_ID) REFERENCES GiangVien(ID) ON DELETE CASCADE,
	FOREIGN KEY (TongHop_ID) REFERENCES TongHop(ID) ON DELETE CASCADE
);

CREATE TABLE NhiemVu
(
	ID BIGINT NOT NULL IDENTITY(1,1),
	Lop_ID BIGINT NOT NULL,
	PhanCong_ID BIGINT NOT NULL,
	HocPhan_ID BIGINT NOT NULL,
	GiangVien_ID BIGINT NOT NULL,
	LoaiPhong TINYINT NOT NULL,
	NhomLT INT,
	NhomTH INT,
	GhiChu NVARCHAR(255),
	PRIMARY KEY (ID),
	FOREIGN KEY (Lop_ID) REFERENCES Lop(ID) ON DELETE CASCADE,
	FOREIGN KEY (PhanCong_ID) REFERENCES PhanCong(ID) ON DELETE CASCADE,
	FOREIGN KEY (HocPhan_ID) REFERENCES HocPhan(ID) ON DELETE CASCADE,
	FOREIGN KEY (GiangVien_ID) REFERENCES GiangVien(ID) ON DELETE CASCADE
);

DROP TABLE ChiTietHocPhan;
DROP TABLE NhomHocPhan;
DROP TABLE HocKi;
DROP TABLE NhiemVu;
DROP TABLE PhanCong
DROP TABLE ChiTietTongHop;
DROP TABLE TongHop;
DROP TABLE Lop;
DROP TABLE ChuongTrinh;
DROP TABLE Nganh;
DROP TABLE Khoa;
DROP TABLE ChiTietGiangVien;
DROP TABLE HocPhan;
DROP TABLE GiangVien;
DROP TABLE NguoiDung;