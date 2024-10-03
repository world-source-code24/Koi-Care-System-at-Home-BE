use master
go

create database KoiCareSystemDB
go

drop database KoiCareSystemDB
use KoiCareSystemDB
go


create table account_tbl(
	accId int identity primary key,
	name nvarchar(50) default '',
	email varchar(50) not null,
	password varchar(50) not null,
	image varchar(200),
	phone varchar(15) not null default '',
	address nvarchar(200) not null default '',
	role varchar(15) not null default 'Guest', /* Role: Admin, Staff, Guest, Member*/
	startDate date not null,
	endDate date,
	status bit not null default 1
)

create table notes_tbl(
	noteId int identity primary key,
	noteName nvarchar(50) default '',
	noteText text not null,
	accId int not null foreign key references account_tbl on delete cascade
)

create table ponds_tbl(
	pondId int identity primary key,
	name nvarchar(50) not null,
	image varchar(200),
	depth decimal(5,2),
	volume int,
	drain_count int,
	pump_capacity int,
	accId int not null foreign key references account_tbl on delete cascade

)

create table water_parameters_tbl(
	parameterId int identity primary key,
	temperature decimal(5,2),
	salt decimal(5,2),
	phLevel decimal(5,2),
	o2Level decimal(5,2),
	no2Level decimal(5,2),
	no3Level decimal(5,2),
	po4Level decimal(5,2),
	totalChlorines decimal(5,2),
	date datetime not null,
	note text,
	pondId int not null foreign key references ponds_tbl on delete cascade
)

create table kois_tbl(
	koiId int identity primary key,
	name nvarchar(50) not null,
	image varchar(200),
	physique nvarchar(50),
	age int not null,
	length decimal(5,2) not null,
	weight decimal(5,2) not null,
	sex bit not null,
	breed nvarchar(50) not null,
	pondId int null foreign key references ponds_tbl on delete set null
)

create table koi_growth_charts_tbl (
	chart_id int identity primary key,
	date date not null,
	length decimal(5,2) not null,
	weight decimal(5,2) not null,
	healthStatus varchar(20),
	koiId int not null foreign key references kois_tbl on delete cascade
)

create table shops_tbl(
	shopId int identity primary key,
	name nvarchar(50) not null,
	phone varchar(15) not null default '',
	address nvarchar(200) default '',
)

create table orders_tbl(
	orderId int identity primary key,
	date date not null,
	statusOrder nvarchar(100),
	statusPayment nvarchar(100),
	totalAmount decimal(10,2),
	accId int foreign key references account_tbl on delete set null
)

create table products_tbl(
	productId int identity primary key,
	name nvarchar(50) not null,
	price decimal(10,2) not null,
	stock int not null default 0,
	image varchar(200),
	category nvarchar(50) not null,
	productInfo text,
	status bit default 1,
	shopId int foreign key references shops_tbl on delete set null
)

create table order_details_tbl(
	orderId int,
	productId int,
	quantity int,
	totalPrice decimal(10,2),
	primary key(orderId, productId),
	foreign key (orderId) references orders_tbl(orderId) on delete cascade,
	foreign key (productId) references products_tbl(productId) on delete cascade
)

create table refresh_token(
	tokenId nvarchar(100) primary key,
	accId int,
	token nvarchar(100),
	jwtId nvarchar(100),
	isUsed bit,
	isRevoked bit,
	issueAt datetime,
	expiredAt datetime,
	foreign key (accId) references account_tbl(accId) on delete cascade
)

create table cart_tbl(
	accId int not null,
	productId int not null,
	quantity int not null default 1,
	primary key (accId, productId),
	foreign key (accId) references account_tbl(accId) on delete cascade,
	foreign key (productId) references products_tbl(productId) on delete cascade
)
go

--Add data to the tables--
INSERT INTO shops_tbl (name, phone, address) VALUES
('Koi Saigon', '028-1234-5678', '123 Nguyen Van Cu Street, District 1, Ho Chi Minh City, Vietnam'),
('Koi & Aquatic Ho Chi Minh', '028-2345-6789', '456 Le Van Sy Street, District 3, Ho Chi Minh City, Vietnam'),
('Beautiful Koi Store', '028-3456-7890', '789 Nguyen Thi Minh Khai Street, District 1, Ho Chi Minh City, Vietnam'),
('Koi Garden Saigon', '028-4567-8901', '101 Tran Hung Dao Street, District 5, Ho Chi Minh City, Vietnam'),
('Koi World Ho Chi Minh', '028-5678-9012', '202 Cach Mang Thang Tam Street, District 10, Ho Chi Minh City, Vietnam'),
('Koi Shop Saigon', '028-6789-0123', '303 Phan Dang Luu Street, Phu Nhuan District, Ho Chi Minh City, Vietnam'),
('Koi Plaza Ho Chi Minh', '028-7890-1234', '404 Vo Van Tan Street, District 3, Ho Chi Minh City, Vietnam');



INSERT INTO products_tbl (name, price, stock, category, productInfo, status, shopId) VALUES
('Koi Pond Bacteria Treatment', 200.00, 30, 'Treatments', 'Beneficial bacteria to maintain pond health.', 1, 1),
('Koi Anti-Parasite Medicine', 150.00, 40, 'Medicines', 'Medication for treating parasites in koi.', 1, 2),
('High-Efficiency Pond Filter Media', 300.00, 25, 'Filter Media', 'Premium filter media for superior filtration.', 1, 3),
('pH Adjuster for Koi Ponds', 120.00, 60, 'Water Parameters', 'Adjusts pH levels to keep water balanced.', 1, 4),
('Koi Pond Algae Treatment', 180.00, 50, 'Treatments', 'Effective algae treatment for clear water.', 1, 5),
('Koi Health Booster', 130.00, 70, 'Medicines', 'Supplement to boost koi health and immunity.', 1, 1),
('Activated Carbon Filter Media', 250.00, 35, 'Filter Media', 'Activated carbon for removing impurities from water.', 1, 2),
('Ammonia Test Kit', 100.00, 90, 'Water Parameters', 'Test kit for monitoring ammonia levels in pond water.', 1, 3),
('Koi Pond Clarifier', 160.00, 45, 'Treatments', 'Clarifier to remove suspended particles and improve water clarity.', 1, 4),
('Water Hardness Test Kit', 110.00, 80, 'Water Parameters', 'Test kit for measuring water hardness levels.', 1, 5);
