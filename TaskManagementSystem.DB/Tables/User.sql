CREATE TABLE [dbo].[User]
(
    [Id]            bigint              NOT NULL PRIMARY KEY,
    [FirstName]     varchar(100)        NOT NULL,
    [LastName]      varchar(100)        NOT NULL,
    [Email]         varchar(150)        NOT NULL,
    [Phone]         varchar(20)         NULL,
    [RoleId]        bigint              NULL,
    [PasswordHash]  varchar(max)        NOT NULL,
    [PasswordSalt]  varchar(max)        NOT NULL,
    [LastLogin]     datetimeoffset      NULL,
    [IsActive]      bit                 NOT NULL DEFAULT 1,
    [CreatedBy]     bigint              NULL,
    [CreatedOn]     datetimeoffset      NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [ModifiedBy]    bigint              NULL,
    [ModifiedOn]    datetimeoffset      NULL,

    CONSTRAINT [FK_Users_Role]        FOREIGN KEY ([RoleId])        REFERENCES [dbo].[Role] ([Id])
);