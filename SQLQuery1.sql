create table tuotteet (id integer identity (1,1) primary key, nimi VARChAR(50), hinta integer);

INSERT INTO tuotteet (nimi, hinta) VALUES ('kinkku', 4)

SELECT * FROM tuotteet
