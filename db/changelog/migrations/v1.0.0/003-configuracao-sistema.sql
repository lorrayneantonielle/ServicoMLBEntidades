--liquibase formatted sql

--changeset lorrayne.antonielle:001-configuracao-sistema
CREATE TABLE configuracao_sistema
(
    id                               uuid    NOT NULL DEFAULT gen_random_uuid(),
    limite_minimo_pontuacao_mutirao integer NOT NULL,
    CONSTRAINT pk_configuracao_sistema PRIMARY KEY (id)
);

--changeset lorrayne.antonielle:002-configuracao-sistema-seed
-- Valor padrão inicial definido na implementação (spec.md Assumptions); ajustável
-- pelo Administrador Geral em tempo de execução.
INSERT INTO configuracao_sistema (id, limite_minimo_pontuacao_mutirao)
VALUES (gen_random_uuid(), 4);
