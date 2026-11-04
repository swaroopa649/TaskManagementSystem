CREATE TABLE Portfolio (
    PortfolioId    BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name           VARCHAR(MAX) NULL,
    Code           VARCHAR(MAX) NULL,
    Description    TEXT NULL,
    OwnerId        BIGINT NOT NULL, -- Changed from INT to BIGINT to match [User].[Id]
    CreatedBy      BIGINT NULL,
    CreatedOn      DATETIMEOFFSET NULL,
    ModifiedBy     BIGINT NULL,
    ModifiedOn     DATETIMEOFFSET NULL,
    IsActive       BIT NULL,
    CreatedDate    DATETIME2 DEFAULT GETDATE(),
    
    CONSTRAINT FK_Portfolio_User FOREIGN KEY (OwnerId) REFERENCES [User](Id)
);