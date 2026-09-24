CREATE TABLE [dbo].[Team]
(
    [Id]          bigint          IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name]        varchar(150)    NOT NULL,
    [Description] varchar(500)    NULL,
    [ManagerId]   bigint          NOT NULL, -- Assigned manager responsible for the team
    [IsActive]    bit             NOT NULL DEFAULT 1,
    [CreatedBy]   bigint          NULL,
    [CreatedOn]   datetimeoffset  NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [ModifiedBy]  bigint          NULL,
    [ModifiedOn]  datetimeoffset  NULL,

    CONSTRAINT [FK_Team_Manager] FOREIGN KEY ([ManagerId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Team_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Team_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([Id])
);
