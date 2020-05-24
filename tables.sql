drop table Klient

delete from Klient where ID = '0123456789'

create table Klient(ID nvarchar(10) not null, Imie nvarchar(64), Nazwisko nvarchar(64), Miasto nvarchar(64), PESEL nvarchar(11), Saldo float, ID_Oddzial nvarchar(64) foreign key references Oddzial(ID));

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02, 'KR1234');
insert into Klient values('0000000000', 'Monika', 'Nowak', 'Warszawa', '11111111111', 23414.90, 'WA1234');

insert into Oddzial values('WA1234', 'Warszawa 1')

SELECT * FROM Klient WHERE ID = '0123456789'

drop table Klient

create table Klient(ID nvarchar(10) not null, Imie nvarchar(64), Naziwsko nvarchar(64), Miasto nvarchar(64), PESEL nvarchar(11), Saldo float);

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02);

insert into Klient values('0000000000', 'Monika', 'Nowak', 'Warszawa', '11111111111', 23414.90);

select * from Transakcje

drop table Transakcje

create table Transakcje(ID int identity(1,1), Kwota float, Nadawca nvarchar(64), Odbiorca nvarchar(64), Data datetime);

select * from Klient

delete from Klient where ID = '0123456789'


SELECT COUNT(*) FROM Klient WHERE ID_Oddzial LIKE 'KR%' AND ID = '0123456789'

SELECT k.ID, Imie, Nazwisko, Miasto, PESEL, Saldo, ID_Oddzial, Nazwa FROM Klient k join Oddzial o on  k.ID_Oddzial = o.ID WHERE k.ID = '0000000000'

UPDATE Klient SET Saldo = Saldo - 2 where ID = '0123456789'

UPDATE Klient SET Saldo = Saldo + 5 where ID = '0000000000'


INSERT INTO Transakcje VALUES(5, '0123456789', '0000000000', '2020-05-24');

