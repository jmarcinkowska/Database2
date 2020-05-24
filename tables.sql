drop table Klient

create table Klient(ID nvarchar(10) not null, Imie nvarchar(64), Nazwisko nvarchar(64), Miasto nvarchar(64), PESEL nvarchar(11), Saldo float, ID_Oddzial nvarchar(64) foreign key references Oddzial(ID));

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02, 'KR1234');
insert into Klient values('0000000000', 'Monika', 'Nowak', 'Warszawa', '11111111111', 23414.90, 'WA1234');

insert into Oddzial values('WA1234', 'Warszawa 1')

SELECT * FROM Klient WHERE ID = '0123456789'

drop table Klient

create table Klient(ID nvarchar(10) not null, Imie nvarchar(64), Naziwsko nvarchar(64), Miasto nvarchar(64), PESEL nvarchar(11), Saldo float);

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02);

