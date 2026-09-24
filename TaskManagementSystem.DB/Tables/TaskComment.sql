CREATE TABLE [dbo].[TaskComment]
(
    [Id]          bigint          IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TaskId]      bigint          NOT NULL,
    [UserId]      bigint          NOT NULL, -- User who wrote the comment
    [Comment]     varchar(max)    NOT NULL,
    [IsActive]    bit             NOT NULL DEFAULT 1,
    [CreatedBy]   bigint          NOT NULL,
    [CreatedOn]   datetimeoffset  NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [ModifiedBy]  bigint          NULL,
    [ModifiedOn]  datetimeoffset  NULL,

    CONSTRAINT [FK_TaskComment_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([Id]),
    CONSTRAINT [FK_TaskComment_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_TaskComment_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_TaskComment_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([Id])
);