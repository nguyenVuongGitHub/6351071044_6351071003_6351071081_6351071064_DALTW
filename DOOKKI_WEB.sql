create database DOOKKI_WEB;
use DOOKKI_WEB;

create table BookingRequests
(
	ID int identity(1,1) primary key,
	[Name] nvarchar(255),
	[Phone] nvarchar(15),
	NumberOfSeat int,
	[Date] date,
	[Time] time,
	[Status] nvarchar(15)
);

CREATE TABLE [ACCOUNT]
(
	ID int identity(0,1) primary key, 
	UserName nvarchar(255) unique not null, -- phone
	[Password] nvarchar(255) not null,
	[Role] nvarchar(20) not null -- admin, employee, customer
);
insert into [ACCOUNT] (UserName,[Password],[Role]) values
('*&^D3Y#*^&T*Gd','1','non-customer'),
('admin1','123111','admin'),
('0123456789','123','customer');


CREATE TABLE [Admin]
(
	ID int identity(1,1) primary key,
	Phone nvarchar(12) not null unique,
	[Name] nvarchar(255) not null,
	IDAccount int,
	foreign key (IDAccount) references Account(ID)
);
insert into [Admin] ([Name],Phone,IDAccount) values
(N'Nguyễn Đình Vương','0328651594', 1);

create table Customer
(
	ID int identity(0,1) primary key, -- khách hàng nào có id = 0 -> ko có tài khoản ( KHÁCH VÃNG LAI)
	Phone nvarchar(15) not null unique,
	Email nvarchar(255),
	[Address] nvarchar(255),
	[Name] nvarchar(255) not null,
	[Marks] int default(0), -- điểm của khách hàng.mỗi 300k thì được 1 điểm, và có thể dùng điểm để đổi discount
	IDAccount int,
	foreign key (IDAccount) references Account(ID)
);
insert into Customer ([Name],Phone,Email,[Address],Marks,IDAccount) values
('','','','',0,0),
('Test customer','0123456789','email@gmail.com','HCM',0,2);

--------------------------------------------------------
create table PaymentMethod
(
	ID int identity(1,1) primary key,
	[Name] nvarchar(255) not null
);
insert into PaymentMethod ([Name])values
(N'Chuyển khoản'),
(N'Tiền mặt');


------------------------------------
create table CategoryTicket(
	ID int identity(1,1) primary key,
	[Name] nvarchar(50)
);
insert into CategoryTicket([Name]) values
(N'Take-away'),
(N'Stay-here');
create table Ticket
(
	ID int identity(1,1) primary key,
	[Name] nvarchar(255) not null unique,
	Price decimal(18,0) not null,
	ID_Category int,
	constraint FK_ID_Category foreign key (ID_Category) references CategoryTicket(ID),
	urlImage nvarchar(255)
);
insert into Ticket ([Name],Price,ID_Category,urlImage) values
(N'Giá vé trẻ em',70000,2,''),
(N'Giá vé người lớn',150000,2,''),
(N'Hotdog',15000,1,N'Hotdog.jpg'),
(N'Vòng phô mai',80000,2,''),
(N'Phô mai hoa tuyết',45000,2,''),
(N'Bánh trứng',15000,1,N'Bánh trứng.jpg'),
(N'Thịt bò ba chỉ',70000,2,''),
(N'Takeout Combo 1 (1 loại gà)',140000,1,N'Takeout Combo 1 (1 loại gà).jpg'),
(N'Takeout Combo 2 (mix 2 vị)',140000,1,N'Takeout Combo 2 (mix 2 vị).jpg'),
(N'Takeout Combo 3 (mix 3 vị)',140000,1,N'Takeout Combo 3 (mix 3 vị).jpg'),
(N'Takeout Combo 4 (đầy đủ)',250000,1,N'Takeout Combo 4 (đầy đủ).jpg');

create table [Order]
(
	ID int identity(1,1) primary key,
	[Time] Time not null,

	customerID int,
	constraint FK_Order_customerID foreign key(customerID)
	references Customer(ID),

	discount int default(0),
	[Status] nvarchar(50)
);
insert into [Order]([Time],customerID,[Status]) values
('11:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('18:00:00',0,'Finish'),
('17:45:00',0,'Finish'),
('16:21:00',0,'Finish'),

('15:00:00',0,'Finish'),
('13:45:00',0,'Finish'),
('11:21:00',0,'Finish'),

('13:21:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('12:41:00',0,'Finish'),
('11:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('11:24:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish'),

('13:00:00',0,'Finish'),
('14:45:00',0,'Finish'),
('15:21:00',0,'Finish');

create table Payment
(
	ID int identity(1,1) primary key,
	[day] date not null,
	amount decimal(18,0) not null,
	paymentMethodID int,
	constraint FK_Payment_PaymentMethod foreign key (paymentMethodID)
	references PaymentMethod(ID)
);
insert into Payment ([day],amount,paymentMethodID) values
('2024-01-03', 2250000, 2),
('2024-01-03', 4800000, 2),
('2024-01-03', 15000000, 2),

('2024-02-03', 300000, 2),
('2024-02-03', 6000000, 2),
('2024-02-03', 3900000, 2),

('2024-03-03', 4350000, 2),
('2024-03-03', 1500000, 2),
('2024-03-03', 15450000, 2),

('2024-04-03', 3300000, 2),
('2024-04-03', 6750000, 2),
('2024-04-03', 150000, 2),

('2024-05-03', 7650000, 2),
('2024-05-03', 2550000, 2),
('2024-05-03', 12300000, 2),

('2024-06-03', 3600000, 2),
('2024-06-03', 10800000, 2),
('2024-06-03', 1650000, 2),

('2024-07-03', 7950000, 2),
('2024-07-03', 1800000, 2),
('2024-07-03', 14700000, 2),

('2024-08-03', 12000000, 2),
('2024-08-03', 13500000, 2),
('2024-08-03', 15000000, 2),

('2024-09-03', 18450000, 2),
('2024-09-03', 18000000, 2),
('2024-09-03', 18300000, 2),

('2024-10-03', 15000000, 2),
('2024-10-03', 3150000, 2),
('2024-10-03', 6150000, 2),

('2024-11-03', 1650000, 2),
('2024-11-03', 6150000, 2),
('2024-11-03', 1800000, 2),

('2024-12-03', 15000000, 2),
('2024-12-03', 30000000, 2),
('2024-12-03', 45000000, 2);

create table OrderDetail
(
	ID int identity(1,1) primary key,
	quantily decimal(18,0) not null,
	ticketID int,
	paymentID int,
	orderID int,
	constraint FK_OrderDetail_Ticket foreign key (ticketID)
	references Ticket(ID),

	constraint FK_OrderDetail_Payment foreign key (paymentID)
	references Payment(ID),

	constraint FK_OrderDetail_Order foreign key (orderID)
	references [Order](ID),
);
insert into OrderDetail (quantily,ticketID,paymentID,orderID) values
(15,2,1,1),
(32,2,2,2),
(100,2,3,3),

(2,2,4,4),
(40,2,5,5),
(26,2,6,6),

(29,2,7,7),
(10,2,8,8),
(103,2,9,9),

(22,2,10,10),
(45,2,11,11),
(1,2,12,12),

(51,2,13,13),
(17,2,14,14),
(82,2,15,15),

(24,2,16,16),
(72,2,17,17),
(11,2,18,18),

(53,2,19,19),
(12,2,20,20),
(98,2,21,21),

(80,2,22,22),
(90,2,23,23),
(100,2,24,24),

(123,2,25,25),
(120,2,26,26),
(122,2,27,27),

(100,2,28,28),
(21,2,29,29),
(41,2,30,30),

(11,2,31,31),
(41,2,32,32),
(12,2,33,33),

(100,2,34,34),
(200,2,35,35),
(300,2,36,36);

select*from[Account]
