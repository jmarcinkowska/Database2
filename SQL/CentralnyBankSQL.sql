-- utworzenie tabeli Klient

create table Klient(ID nvarchar(10) not null primary key, 
                    Imie nvarchar(64),
                    Nazwisko nvarchar(64), 
                    Miasto nvarchar(64), 
                    PESEL nvarchar(11), 
                    Saldo float);

-- utworzenie tabeli Oddział

create table Oddzial(ID nvarchar(10) not null primary key, 
                    nazwa nvarchar(32));

-- utworzenie tabeli asocjacyjnej Klient_Oddział

create table Klient_Oddzial(ID int identity(1,1) primary key, 
                            ID_Klient nvarchar(10) foreign key references Klient(ID), 
                            ID_Oddzial nvarchar(10) foreign key references Oddzial(ID));

-- wprowadzenie przykładowych danych do tabeli Klient

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02);
insert into Klient values('0000000000', 'Monika', 'Nowak', 'Warszawa', '11111111111', 23414.90);

-- wprowadzenie przykładowych danych do tabeli oddział

insert into Oddzial values('WA1234', 'Warszawa 1');
insert into Oddzial values('KR1234', 'Kraków 12');

-- wprowadzenie przykładowych danych do tabeli Klient_Oddzial

insert into Klient_Oddzial values('0000000000', 'KR1234');
insert into Klient_Oddzial values('0123456789', 'KR1234');
insert into Klient_Oddzial values('0123456789', 'WA1234'); 
