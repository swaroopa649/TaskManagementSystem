CREATE TABLE [dbo].[TeamMember]
(
    [Id]          bigint          IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TeamId]      bigint          NOT NULL,
    [UserId]      bigint          NOT NULL,
    [AssignedBy]  bigint          NOT NULL, -- Manager/Admin who added the user to the team
    [AssignedOn]  datetimeoffset  NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [IsActive]    bit             NOT NULL DEFAULT 1,

    CONSTRAINT [FK_TeamMember_Team] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id]),
    CONSTRAINT [FK_TeamMember_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_TeamMember_AssignedBy] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [UK_Team_User] UNIQUE ([TeamId], [UserId])
);