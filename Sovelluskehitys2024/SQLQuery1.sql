create table tuotteet (id integer identity (1,1) primary key, nimi VARChAR(50), hinta integer);

create table huoltopalvelut (id integer identity (1,1) primary key, nimi VARChAR(50), hinta integer);

create table asiakkaat (id integer identity (1,1) Primary key, nimi varchar(50), osoite varchar(150), puhelin varchar(50))

create table Asentajat (id integer identity (1,1) Primary key, nimi varchar(50), puhelin varchar(50))

create table tilaukset (id integer identity(1,1) primary key, asiakas_id integer references asiakkaat on delete cascade, tuote_id integer references tuotteet on delete cascade, toimitettu BIT DEFAULT 0)

create table huoltotilaukset (id integer identity(1,1) primary key,asentaja_id integer references Asentajat on delete cascade, huoltopalvelu_id integer references huoltopalvelut on delete cascade, asiakas_id integer references asiakkaat on delete cascade, valmis BIT DEFAULT 0)


insert into asiakkaat (nimi, osoite, puhelin) values ('Pena', 'Mäkitie 7', '+3584668992')
insert into Asentajat (nimi, puhelin) values ('Pertti', '+3588522146')
insert into tuotteet (nimi, hinta) values ('olut', '2')
insert into huoltopalvelut(nimi, hinta) values ('laitteen korjaus', '50')
INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (1, 1)


SELECT * FROM asiakkaat
Delete FROM tuotteet
SELECT * FROM tilaukset
SELECT * FROM huoltopalvelut
SELECT * FROM Asentajat
SELECT * FROM huoltotilaukset


UPDATE tilaukset set toimitettu = 1 WHERE id =1

SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id

delete from asiakkaat where id = 4


SELECT t.hinta FROM tuotteet t JOIN tilaukset tl ON t.id = tl.tuote_id WHERE tl.toimitettu = 1;
SELECT sum(t.hinta) FROM tuotteet t JOIN tilaukset tl ON t.id = tl.tuote_id WHERE tl.toimitettu = 1;