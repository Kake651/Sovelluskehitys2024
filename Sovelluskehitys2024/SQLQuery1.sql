create table tuotteet (id integer identity (1,1) primary key, nimi VARChAR(50), hinta integer);

create table asiakkaat (id integer identity (1,1) Primary key, nimi varchar(50), osoite varchar(150), puhelin varchar(50))

create table tilaukset (id integer identity(1,1) primary key, asiakas_id integer references asiakkaat on delete cascade, tuote_id integer references tuotteet on delete cascade)


insert into asiakkaat (nimi, osoite, puhelin) values ('Pena', 'Mäkitie 7', '+3584668992')
insert into tuotteet (nimi, hinta) values ('olut', '2')
INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (2, 1)


SELECT * FROM asiakkaat
SELECT * FROM tilaukset
SELECT * FROM tuotteet

delete from tuotteet where id = 4
