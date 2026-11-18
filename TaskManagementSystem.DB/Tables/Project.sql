CREATE TABLE Project
(
    ProjectId        BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProjectKey       VARCHAR(MAX)             NULL,
    Name             VARCHAR(MAX)             NULL,
    Description      VARCHAR(MAX)             NULL,
    LeadUserId       BIGINT                   NOT NULL, -- Changed from INT to BIGINT to match User.Id
    CreatedBy        BIGINT                   NULL,
    CreatedOn        DATETIMEOFFSET           NULL,
    ModifiedBy       BIGINT                   NULL,
    ModifiedOn       DATETIMEOFFSET           NULL,
    IsActive         BIT                      NULL,
    CreatedDate      DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET(),
    FOREIGN KEY (LeadUserId) REFERENCES "User"(Id)
);