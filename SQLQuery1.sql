﻿create table tuotteet (id integer identity (1,1) primary key, nimi Text, hinta integer);

INSERT INTO tuotteet (nimi, hinta) VALUES ('kinkku', 4)

SELECT * FROM tuotteet