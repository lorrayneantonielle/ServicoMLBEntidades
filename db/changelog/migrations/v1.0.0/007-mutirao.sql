--liquibase formatted sql

--changeset lorrayne.antonielle:007-mutirao-tables
CREATE TABLE mutirao_escalas
(
    id                     uuid        NOT NULL DEFAULT gen_random_uuid(),
    data                   date        NOT NULL,
    turno                  varchar(20) NOT NULL,
    vagas_totais           integer     NOT NULL,
    pontuacao_por_presenca integer     NOT NULL,
    CONSTRAINT pk_mutirao_escalas PRIMARY KEY (id)
);

CREATE INDEX ix_mutirao_escalas_data ON mutirao_escalas (data);

CREATE TABLE presencas
(
    id                  uuid        NOT NULL DEFAULT gen_random_uuid(),
    mutirao_escala_id   uuid        NOT NULL,
    familia_id          uuid        NOT NULL,
    data_registro       timestamptz NOT NULL DEFAULT now(),
    pontuacao_concedida integer     NOT NULL,
    CONSTRAINT pk_presencas PRIMARY KEY (id),
    CONSTRAINT fk_presencas_mutirao_escala FOREIGN KEY (mutirao_escala_id) REFERENCES mutirao_escalas (id) ON DELETE CASCADE,
    CONSTRAINT fk_presencas_familia FOREIGN KEY (familia_id) REFERENCES familias (id) ON DELETE CASCADE
);

-- FR-025: uma família não registra presença duplicada na mesma escala.
CREATE UNIQUE INDEX ux_presencas_escala_familia ON presencas (mutirao_escala_id, familia_id);
CREATE INDEX ix_presencas_familia_id ON presencas (familia_id);
