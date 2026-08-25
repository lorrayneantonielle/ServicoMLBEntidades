--liquibase formatted sql

--changeset lorrayne.antonielle:005-obra-tables
CREATE TABLE etapas_obra
(
    id                   uuid          NOT NULL DEFAULT gen_random_uuid(),
    nome                 text          NOT NULL,
    ordem                integer       NOT NULL,
    percentual_conclusao decimal(5,2)  NOT NULL DEFAULT 0,
    CONSTRAINT pk_etapas_obra PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ux_etapas_obra_ordem ON etapas_obra (ordem);

CREATE TABLE medicoes
(
    id                        uuid          NOT NULL DEFAULT gen_random_uuid(),
    etapa_obra_id             uuid          NOT NULL,
    data                      date          NOT NULL,
    status_aprovacao          varchar(20)   NOT NULL DEFAULT 'Pendente',
    recursos_liberados        decimal(14,2) NULL,
    observacao                text          NULL,
    registrado_por_usuario_id uuid          NOT NULL,
    CONSTRAINT pk_medicoes PRIMARY KEY (id),
    CONSTRAINT fk_medicoes_etapa_obra FOREIGN KEY (etapa_obra_id) REFERENCES etapas_obra (id) ON DELETE CASCADE,
    CONSTRAINT fk_medicoes_usuario FOREIGN KEY (registrado_por_usuario_id) REFERENCES "AspNetUsers" ("Id")
);

CREATE INDEX ix_medicoes_etapa_obra_id ON medicoes (etapa_obra_id);

CREATE TABLE ocorrencias
(
    id                        uuid        NOT NULL DEFAULT gen_random_uuid(),
    etapa_obra_id             uuid        NOT NULL,
    descricao                 text        NOT NULL,
    data                      date        NOT NULL,
    registrado_por_usuario_id uuid        NOT NULL,
    CONSTRAINT pk_ocorrencias PRIMARY KEY (id),
    CONSTRAINT fk_ocorrencias_etapa_obra FOREIGN KEY (etapa_obra_id) REFERENCES etapas_obra (id) ON DELETE CASCADE,
    CONSTRAINT fk_ocorrencias_usuario FOREIGN KEY (registrado_por_usuario_id) REFERENCES "AspNetUsers" ("Id")
);

CREATE INDEX ix_ocorrencias_etapa_obra_id ON ocorrencias (etapa_obra_id);
