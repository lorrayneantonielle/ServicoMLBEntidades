--liquibase formatted sql

--changeset lorrayne.antonielle:001-refresh-tokens
CREATE TABLE refresh_tokens
(
    id         uuid        NOT NULL DEFAULT gen_random_uuid(),
    usuario_id uuid        NOT NULL,
    token_hash text        NOT NULL,
    expires_at timestamptz NOT NULL,
    revoked_at timestamptz NULL,
    created_at timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT pk_refresh_tokens PRIMARY KEY (id),
    CONSTRAINT fk_refresh_tokens_usuario FOREIGN KEY (usuario_id) REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX ix_refresh_tokens_usuario_id ON refresh_tokens (usuario_id);
CREATE UNIQUE INDEX ux_refresh_tokens_token_hash ON refresh_tokens (token_hash);
