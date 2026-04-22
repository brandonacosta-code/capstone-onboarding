CREATE TABLE tblProducts(
Id int IDENTITY(1, 1) PRIMARY KEY,
Name nvarchar(50),
UnitPrice decimal(18,6))

insert into tblProducts values ('Candy', '5')
insert into tblProducts values ('Dark Chocolate', '10')
insert into tblProducts values ('Milk Chocolate', '8')
insert into tblProducts values ('White Chocolate', '9')