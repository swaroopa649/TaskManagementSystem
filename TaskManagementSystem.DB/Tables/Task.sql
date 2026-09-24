CREATE TABLE [dbo].[Task]
(
    [Id]           bigint          IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title]        varchar(200)    NOT NULL,
    [Description]  varchar(max)    NULL,
    [StatusId]     bigint          NOT NULL DEFAULT 1, -- References TaskStatus (Default: 1 - 'To Do')
    [Priority]     varchar(20)     NOT NULL DEFAULT 'Medium', -- 'Low', 'Medium', 'High', 'Urgent'
    [TeamId]       bigint          NULL, -- Optional: If task belongs to a specific team
    [AssigneeId]   bigint          NULL, -- Assigned User
    [DueDate]      datetimeoffset  NULL,
    [IsActive]     bit             NOT NULL DEFAULT 1,
    [CreatedBy]    bigint          NOT NULL, -- Task Creator
    [CreatedOn]    datetimeoffset  NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [ModifiedBy]   bigint          NULL,
    [ModifiedOn]   datetimeoffset  NULL,

    CONSTRAINT [FK_Task_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[TaskStatus] ([Id]),
    CONSTRAINT [FK_Task_Team] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id]),
    CONSTRAINT [FK_Task_Assignee] FOREIGN KEY ([AssigneeId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Task_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Task_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([Id])
);