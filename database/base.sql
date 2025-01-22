DROP TABLE if exists tennis_score;

CREATE TABLE tennis_score(
   id_score SERIAL,
   id_joueur INTEGER,
   valeur VARCHAR(10),
   date_score TIMESTAMPTZ,
   PRIMARY KEY(id_score)
);
