--liquibase formatted sql

--changeset lorrayne.antonielle:006-unidades-tables
CREATE TABLE unidades_habitacionais
(
    id                  uuid          NOT NULL DEFAULT gen_random_uuid(),
    identificador       text          NOT NULL,
    metragem            decimal(8,2)  NOT NULL,
    localizacao_terreno text          NOT NULL,
    status              varchar(20)   NOT NULL DEFAULT 'Livre',
    familia_id          uuid          NULL,
    CONSTRAINT pk_unidades_habitacionais PRIMARY KEY (id),
    CONSTRAINT fk_unidades_habitacionais_familia FOREIGN KEY (familia_id) REFERENCES familias (id)
);

CREATE UNIQUE INDEX ux_unidades_habitacionais_identificador ON unidades_habitacionais (identificador);
-- FR-017: uma unidade não-Livre só pode estar vinculada a uma família por vez.
CREATE UNIQUE INDEX ux_unidades_habitacionais_familia_nao_livre ON unidades_habitacionais (familia_id) WHERE status <> 'Livre';
