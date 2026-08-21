--liquibase formatted sql

--changeset lorrayne.antonielle:001-familias-tables
CREATE TABLE familias
(
    id                       uuid          NOT NULL DEFAULT gen_random_uuid(),
    renda_familiar           decimal(12,2) NOT NULL,
    situacao_vulnerabilidade text          NOT NULL,
    status                   varchar(30)   NOT NULL DEFAULT 'PreCadastro',
    pontuacao_acumulada      integer       NOT NULL DEFAULT 0,
    excluida                 boolean       NOT NULL DEFAULT false,
    created_at               timestamptz   NOT NULL DEFAULT now(),
    updated_at               timestamptz   NOT NULL DEFAULT now(),
    CONSTRAINT pk_familias PRIMARY KEY (id)
);

CREATE INDEX ix_familias_status ON familias (status);

CREATE TABLE membros
(
    id               uuid        NOT NULL DEFAULT gen_random_uuid(),
    familia_id       uuid        NOT NULL,
    nome             text        NOT NULL,
    data_nascimento  date        NOT NULL,
    vinculo          text        NOT NULL,
    cpf              varchar(11) NOT NULL,
    familia_excluida boolean     NOT NULL DEFAULT false,
    CONSTRAINT pk_membros PRIMARY KEY (id),
    CONSTRAINT fk_membros_familia FOREIGN KEY (familia_id) REFERENCES familias (id) ON DELETE CASCADE
);

CREATE INDEX ix_membros_familia_id ON membros (familia_id);
-- FR-033: CPF único entre membros de famílias não excluídas (soft delete futuro).
CREATE UNIQUE INDEX ux_membros_cpf_familia_ativa ON membros (cpf) WHERE familia_excluida = false;

CREATE TABLE documentos
(
    id                uuid        NOT NULL DEFAULT gen_random_uuid(),
    familia_id        uuid        NOT NULL,
    tipo              varchar(30) NOT NULL,
    status            varchar(20) NOT NULL DEFAULT 'Pendente',
    arquivo_path      text        NULL,
    arquivo_mime_type text        NULL,
    updated_at        timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT pk_documentos PRIMARY KEY (id),
    CONSTRAINT fk_documentos_familia FOREIGN KEY (familia_id) REFERENCES familias (id) ON DELETE CASCADE
);

CREATE INDEX ix_documentos_familia_id ON documentos (familia_id);
CREATE UNIQUE INDEX ux_documentos_familia_tipo ON documentos (familia_id, tipo);

CREATE TABLE familia_status_historico
(
    id              uuid        NOT NULL DEFAULT gen_random_uuid(),
    familia_id      uuid        NOT NULL,
    status_anterior varchar(30) NOT NULL,
    status_novo     varchar(30) NOT NULL,
    motivo          text        NULL,
    usuario_id      uuid        NOT NULL,
    data_transicao  timestamptz NOT NULL,
    CONSTRAINT pk_familia_status_historico PRIMARY KEY (id),
    CONSTRAINT fk_familia_status_historico_familia FOREIGN KEY (familia_id) REFERENCES familias (id) ON DELETE CASCADE,
    CONSTRAINT fk_familia_status_historico_usuario FOREIGN KEY (usuario_id) REFERENCES "AspNetUsers" ("Id")
);

CREATE INDEX ix_familia_status_historico_familia_id ON familia_status_historico (familia_id);
