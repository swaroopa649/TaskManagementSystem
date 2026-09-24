CREATE TABLE [dbo].[Role]
(
	[Id]			bigint			NOT NULL		PRIMARY KEY,
	[Name]			varchar(max)		NULL,
	[Code]			varchar(max)		NULL,
	[CreatedBy]     bigint				NULL,
	[CreatedOn]     datetimeoffset      NULL,
	[ModifiedBy]    bigint				NULL,
	[ModifiedOn]     datetimeoffset     NULL,
	[IsActive]       bit                NULL
)
