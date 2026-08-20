--liquibase formatted sql

--changeset lorrayne.antonielle:001-identity-tables
CREATE TABLE "AspNetRoles"
(
    "Id"               uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Name"             varchar(256) NULL,
    "NormalizedName"   varchar(256) NULL,
    "ConcurrencyStamp" text         NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE TABLE "AspNetUsers"
(
    "Id"                   uuid         NOT NULL DEFAULT gen_random_uuid(),
    "NomeCompleto"         varchar(256) NOT NULL,
    "UserName"             varchar(256) NULL,
    "NormalizedUserName"   varchar(256) NULL,
    "Email"                varchar(256) NULL,
    "NormalizedEmail"      varchar(256) NULL,
    "EmailConfirmed"       boolean      NOT NULL DEFAULT false,
    "PasswordHash"         text         NULL,
    "SecurityStamp"        text         NULL,
    "ConcurrencyStamp"     text         NULL,
    "PhoneNumber"          text         NULL,
    "PhoneNumberConfirmed" boolean      NOT NULL DEFAULT false,
    "TwoFactorEnabled"     boolean      NOT NULL DEFAULT false,
    "LockoutEnd"           timestamptz  NULL,
    "LockoutEnabled"       boolean      NOT NULL DEFAULT true,
    "AccessFailedCount"    integer      NOT NULL DEFAULT 0,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE TABLE "AspNetUserRoles"
(
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE TABLE "AspNetUserClaims"
(
    "Id"         serial NOT NULL,
    "UserId"     uuid   NOT NULL,
    "ClaimType"  text   NULL,
    "ClaimValue" text   NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE TABLE "AspNetRoleClaims"
(
    "Id"         serial NOT NULL,
    "RoleId"     uuid   NOT NULL,
    "ClaimType"  text   NULL,
    "ClaimValue" text   NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE TABLE "AspNetUserLogins"
(
    "LoginProvider"       varchar(128) NOT NULL,
    "ProviderKey"         varchar(128) NOT NULL,
    "ProviderDisplayName" text         NULL,
    "UserId"              uuid         NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE TABLE "AspNetUserTokens"
(
    "UserId"        uuid         NOT NULL,
    "LoginProvider" varchar(128) NOT NULL,
    "Name"          varchar(128) NOT NULL,
    "Value"         text         NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

--changeset lorrayne.antonielle:002-identity-seed-roles
INSERT INTO "AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
VALUES (gen_random_uuid(), 'AdminGeral', 'ADMINGERAL', gen_random_uuid()::text),
       (gen_random_uuid(), 'AssistenteSocial', 'ASSISTENTESOCIAL', gen_random_uuid()::text),
       (gen_random_uuid(), 'TecnicoObra', 'TECNICOOBRA', gen_random_uuid()::text);
