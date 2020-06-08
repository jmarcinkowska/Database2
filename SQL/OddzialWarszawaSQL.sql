-- utworzenie tabeli Klient w bazie OddzialWarszawa

create table Klient(ID nvarchar(10) not null primary key, 
                    Imie nvarchar(64), 
                    Nazwisko nvarchar(64), 
                    Miasto nvarchar(64), 
                    PESEL nvarchar(11), 
                    Saldo float);

-- utworzenie tabeli Transakcje w bazie OddzialWarszawa

create table Transakcje(ID nvarchar(36) not null primary key, 
                        Kwota float, 
                        Nadawca nvarchar(64), 
                        Odbiorca nvarchar(64), 
                        Data datetime, 
                        Nazwa nvarchar(64));

-- wprowadzenie przykładowych danych do tabeli Klient w bazie OddzialWaraszawa

insert into Klient values('0123456789', 'Jan', 'Kowalski', 'Krakow', '09876543210', 9803.02);