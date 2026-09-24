/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

MERGE [dbo].[Role] AS Target
USING
(
    VALUES
        (1, 'Admin',    'ADMIN'),
        (2, 'Manager',  'MANAGER'),
        (3, 'User',     'USER')
) AS Source
(
    Id,
    Name,
    Code
)
ON Target.Id = Source.Id

WHEN MATCHED THEN
    UPDATE SET
        Target.Name = Source.Name,
        Target.Code = Source.Code,
        Target.IsActive = 1,
        Target.ModifiedBy = 1,
        Target.ModifiedOn = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Id,
        Name,
        Code,
        CreatedBy,
        CreatedOn,
        IsActive
    )
    VALUES
    (
        Source.Id,
        Source.Name,
        Source.Code,
        1,
        SYSDATETIMEOFFSET(),
        1
    );

