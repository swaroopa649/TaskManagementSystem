CREATE TABLE [dbo].[TaskStatus]
(
    [Id]          bigint          NOT NULL PRIMARY KEY,
    [Code]        varchar(50)     NOT NULL UNIQUE, -- e.g. 'TODO', 'IN_PROGRESS', 'DONE'
    [Name]        varchar(100)    NOT NULL,        -- e.g. 'To Do', 'In Progress', 'Done'
    [IsActive]    bit             NOT NULL DEFAULT 1,
    [CreatedBy]   bigint          NULL,
    [CreatedOn]   datetimeoffset  NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [ModifiedBy]  bigint          NULL,
    [ModifiedOn]  datetimeoffset  NULL
);
