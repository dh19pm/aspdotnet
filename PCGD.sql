use PCGD
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
	HocKi TINYINT NOT NULL,
	NgayTao DATE NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (NguoiDung_ID) REFERENCES NguoiDung(ID) ON DELETE CASCADE
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
	NhomHT INT,
	GhiChu NVARCHAR(255),
	PRIMARY KEY (ID),
	FOREIGN KEY (Lop_ID) REFERENCES Lop(ID) ON DELETE CASCADE,
	FOREIGN KEY (PhanCong_ID) REFERENCES PhanCong(ID) ON DELETE CASCADE,
	FOREIGN KEY (HocPhan_ID) REFERENCES HocPhan(ID) ON DELETE CASCADE,
	FOREIGN KEY (GiangVien_ID) REFERENCES GiangVien(ID) ON DELETE CASCADE
);

INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE101',N'Giới thiệu ngành – ĐH KTPM',1,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAX101',N'Những nguyên lý cơ bản của chủ nghĩa Mác – Lênin 1',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAT104',N'Toán A1',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS106',N'Lập trình căn bản',4,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS107',N'Nền tảng công nghệ thông tin',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('PHY109',N'Vật lý đại cương – Tin học',4,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('PHT110',N'Giáo dục thể chất 1',1,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAX102',N'Những nguyên lý cơ bản của chủ nghĩa Mác – Lênin 2',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS302',N'Ngôn ngữ lập trình Java',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAT105',N'Toán A2',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS304',N'Cấu trúc dữ liệu',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CON301',N'Mạng máy tính',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE102',N'Lập trình Python',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAT503',N'Toán rời rạc',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('PHT121',N'Giáo dục thể chất 2',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('ENG101',N'Tiếng Anh 1',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('FSL101',N'Tiếng Pháp 1',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CHI101',N'Tiếng Trung 1',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('HCM101',N'Tư tưởng Hồ Chí Minh',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MAT106',N'Toán A3',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS330',N'Kiến trúc máy tính và Hợp ngữ',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS311',N'Cơ sở dữ liệu',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS313',N'Phương pháp lập trình hướng đối tượng',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS326',N'Kỹ năng giao tiếp ngành nghề',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MIS150',N'Giáo dục quốc phòng – an ninh 1',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('ENG102',N'Tiếng Anh 2',4,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('FSL102',N'Tiếng Pháp 2',4,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CHI102',N'Tiếng Trung 2',4,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('VRP101',N'Đường lối cách mạng của Đảng Cộng sản Việt Nam',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS309',N'Phân tích và thiết kế giải thuật',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE301',N'Nhập môn công nghệ phần mềm',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS310',N'Hệ điều hành',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('PRS302',N'Xác suất thống kê A - TH',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('ESP305',N'Tiếng Anh chuyên ngành TH',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MIS160',N'Giáo dục quốc phòng – an ninh 2',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS503',N'Lý thuyết đồ thị',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS302',N'Phân tích thiết kế hệ thống thông tin',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('TIE501',N'Lập trình .Net',4,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CON501',N'Lập trình Web',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS521',N'Trí tuệ nhân tạo',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE511',N'Hệ quản trị CSDL DB2',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE514',N'Chuyên đề NoSQL',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE510',N'Hệ thống thông tin địa lý (GIS) – TH',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MIS170',N'Giáo dục quốc phòng – an ninh 3',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('MOR303',N'Phương pháp nghiên cứu khoa học – TH',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS301',N'Hệ quản trị cơ sở dữ liệu – TH',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE501',N'Phân tích yêu cầu phần mềm',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE505',N'Phân tích và thiết kế phần mềm hướng đối tượng',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS501',N'Lập trình quản lý',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS505',N'Thiết kế đồ họa',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS515',N'Khai khoáng dữ liệu',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS912',N'Chuyên đề Java',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS522',N'Xử lý ngôn ngữ tự nhiên',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('COS523',N'Giao diện người máy',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE515',N'Đồ án 1',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE513',N'Thiết kế phần mềm',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE518',N'Công nghệ Web – nền tảng PHP',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE517',N'Công nghệ Web – nền tảng ASP.NET',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE512',N'Kiểm thử và đảm bảo chất lượng phần mềm',3,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE507',N'Bảo trì phần mềm',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE516',N'Đồ án 2',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE519',N'Phân tích dữ liệu và ứng dụng',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE508',N'Quản lý dự án phần mềm',2,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('TIE903',N'Thực tập cuối khóa – TH',5,0);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('TIE913',N'Khóa luận tốt nghiệp – TH',10,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CON914',N'Lập trình truyền thông',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('IMS914',N'Hệ quản trị CSDL Oracle',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('BUS528',N'Thương mại điện tử – TH',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE504',N'Phát triển phần mềm mã nguồn mở',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE910',N'Điện toán đám mây',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('CON502',N'Lập trình cho các thiết bị di động',3,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE911',N'An toàn bảo mật CSDL',2,1);
INSERT INTO HocPhan(MaHP,TenHP,SoTC,LoaiHP) VALUES('SEE912',N'Xây dựng phần mềm hướng dịch vụ',3,1);

DROP TABLE ChiTietHocPhan;
DROP TABLE NhomHocPhan;
DROP TABLE HocKi;
DROP TABLE NhiemVu;
DROP TABLE PhanCong;
DROP TABLE Lop;
DROP TABLE ChuongTrinh;
DROP TABLE Nganh;
DROP TABLE Khoa;
DROP TABLE ChiTietGiangVien;
DROP TABLE HocPhan;
DROP TABLE GiangVien;
DROP TABLE NguoiDung;