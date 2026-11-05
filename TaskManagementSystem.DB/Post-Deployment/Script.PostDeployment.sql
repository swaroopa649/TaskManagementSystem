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

 /*
    Default users ,with password Admin@2021
*/

MERGE [dbo].[User] AS Target
USING
(
    VALUES
        -- Current admin user
        (1,  'Durga Prasad', 'Baghuthu',   'admin@tms.com', '9704299683', 1),

        -- RoleId = 2 (Manager) – 50 users
        (2,  'Arjun',        'Reddy',      'user2@tms.com',  '9000000002', 2),
        (3,  'Priya',        'Sharma',     'user3@tms.com',  '9000000003', 2),
        (4,  'Rahul',        'Verma',      'user4@tms.com',  '9000000004', 2),
        (5,  'Sneha',        'Iyer',       'user5@tms.com',  '9000000005', 2),
        (6,  'Vikram',       'Nair',       'user6@tms.com',  '9000000006', 2),
        (7,  'Ananya',       'Gupta',      'user7@tms.com',  '9000000007', 2),
        (8,  'Karthik',      'Menon',      'user8@tms.com',  '9000000008', 2),
        (9,  'Meera',        'Pillai',     'user9@tms.com',  '9000000009', 2),
        (10, 'Rohan',        'Desai',      'user10@tms.com', '9000000010', 2),
        (11, 'Divya',        'Krishnan',   'user11@tms.com', '9000000011', 2),
        (12, 'Aditya',       'Joshi',      'user12@tms.com', '9000000012', 2),
        (13, 'Kavya',        'Rao',        'user13@tms.com', '9000000013', 2),
        (14, 'Siddharth',    'Mehta',      'user14@tms.com', '9000000014', 2),
        (15, 'Ishita',       'Banerjee',   'user15@tms.com', '9000000015', 2),
        (16, 'Nikhil',       'Chatterjee', 'user16@tms.com', '9000000016', 2),
        (17, 'Pooja',        'Agarwal',    'user17@tms.com', '9000000017', 2),
        (18, 'Manish',       'Tiwari',     'user18@tms.com', '9000000018', 2),
        (19, 'Shruti',       'Mishra',     'user19@tms.com', '9000000019', 2),
        (20, 'Gaurav',       'Pandey',     'user20@tms.com', '9000000020', 2),
        (21, 'Nisha',        'Kapoor',     'user21@tms.com', '9000000021', 2),
        (22, 'Varun',        'Malhotra',   'user22@tms.com', '9000000022', 2),
        (23, 'Ayesha',       'Khan',       'user23@tms.com', '9000000023', 2),
        (24, 'Sameer',       'Sheikh',     'user24@tms.com', '9000000024', 2),
        (25, 'Ritika',       'Saxena',     'user25@tms.com', '9000000025', 2),
        (26, 'Harsh',        'Patel',      'user26@tms.com', '9000000026', 2),
        (27, 'Tanvi',        'Shah',       'user27@tms.com', '9000000027', 2),
        (28, 'Deepak',       'Chauhan',    'user28@tms.com', '9000000028', 2),
        (29, 'Riya',         'Sinha',      'user29@tms.com', '9000000029', 2),
        (30, 'Abhishek',     'Kumar',      'user30@tms.com', '9000000030', 2),
        (31, 'Neha',         'Bhat',       'user31@tms.com', '9000000031', 2),
        (32, 'Suresh',       'Naidu',      'user32@tms.com', '9000000032', 2),
        (33, 'Lakshmi',      'Srinivas',   'user33@tms.com', '9000000033', 2),
        (34, 'Prakash',      'Kulkarni',   'user34@tms.com', '9000000034', 2),
        (35, 'Swati',        'Deshmukh',   'user35@tms.com', '9000000035', 2),
        (36, 'Anil',         'Patil',      'user36@tms.com', '9000000036', 2),
        (37, 'Sunita',       'Jadhav',     'user37@tms.com', '9000000037', 2),
        (38, 'Ramesh',       'Yadav',      'user38@tms.com', '9000000038', 2),
        (39, 'Geeta',        'Choudhary',  'user39@tms.com', '9000000039', 2),
        (40, 'Vijay',        'Solanki',    'user40@tms.com', '9000000040', 2),
        (41, 'Kiran',        'Rathore',    'user41@tms.com', '9000000041', 2),
        (42, 'Sarita',       'Pawar',      'user42@tms.com', '9000000042', 2),
        (43, 'Ajay',         'Gaikwad',    'user43@tms.com', '9000000043', 2),
        (44, 'Rekha',        'Sawant',     'user44@tms.com', '9000000044', 2),
        (45, 'Santosh',      'Bhosale',    'user45@tms.com', '9000000045', 2),
        (46, 'Madhuri',      'Shinde',     'user46@tms.com', '9000000046', 2),
        (47, 'Rajesh',       'More',       'user47@tms.com', '9000000047', 2),
        (48, 'Vaishali',     'Kadam',      'user48@tms.com', '9000000048', 2),
        (49, 'Prashant',     'Salunkhe',   'user49@tms.com', '9000000049', 2),
        (50, 'Archana',      'Bhosale',    'user50@tms.com', '9000000050', 2),
        (51, 'Pravin',       'Deshpande',  'user51@tms.com', '9000000051', 2),

        -- RoleId = 3 (User) – 50 users
        (52, 'Amit',         'Bhatt',      'user52@tms.com', '9000000052', 3),
        (53, 'Anjali',       'Vyas',       'user53@tms.com', '9000000053', 3),
        (54, 'Ravi',         'Trivedi',    'user54@tms.com', '9000000054', 3),
        (55, 'Komal',        'Parekh',     'user55@tms.com', '9000000055', 3),
        (56, 'Jignesh',      'Modi',       'user56@tms.com', '9000000056', 3),
        (57, 'Bhavesh',      'Panchal',    'user57@tms.com', '9000000057', 3),
        (58, 'Hetal',        'Thakkar',    'user58@tms.com', '9000000058', 3),
        (59, 'Chirag',       'Mehta',      'user59@tms.com', '9000000059', 3),
        (60, 'Foram',        'Shah',       'user60@tms.com', '9000000060', 3),
        (61, 'Ketan',        'Patel',      'user61@tms.com', '9000000061', 3),
        (62, 'Dhruv',        'Pandya',     'user62@tms.com', '9000000062', 3),
        (63, 'Krishna',      'Joshi',      'user63@tms.com', '9000000063', 3),
        (64, 'Rushi',        'Dave',       'user64@tms.com', '9000000064', 3),
        (65, 'Prachi',       'Bhatt',      'user65@tms.com', '9000000065', 3),
        (66, 'Mihir',        'Solanki',    'user66@tms.com', '9000000066', 3),
        (67, 'Riddhi',       'Vaghela',    'user67@tms.com', '9000000067', 3),
        (68, 'Yash',         'Chauhan',    'user68@tms.com', '9000000068', 3),
        (69, 'Parth',        'Rathod',     'user69@tms.com', '9000000069', 3),
        (70, 'Jinal',        'Parmar',     'user70@tms.com', '9000000070', 3),
        (71, 'Dev',          'Soni',       'user71@tms.com', '9000000071', 3),
        (72, 'Dhara',        'Patel',      'user72@tms.com', '9000000072', 3),
        (73, 'Fenil',        'Gajjar',     'user73@tms.com', '9000000073', 3),
        (74, 'Hirva',        'Patel',      'user74@tms.com', '9000000074', 3),
        (75, 'Jay',          'Shah',       'user75@tms.com', '9000000075', 3),
        (76, 'Krupa',        'Desai',      'user76@tms.com', '9000000076', 3),
        (77, 'Miten',        'Mehta',      'user77@tms.com', '9000000077', 3),
        (78, 'Nikita',       'Jain',       'user78@tms.com', '9000000078', 3),
        (79, 'Om',           'Trivedi',    'user79@tms.com', '9000000079', 3),
        (80, 'Palak',        'Shah',       'user80@tms.com', '9000000080', 3),
        (81, 'Ronak',        'Patel',      'user81@tms.com', '9000000081', 3),
        (82, 'Shivani',      'Pandey',     'user82@tms.com', '9000000082', 3),
        (83, 'Tushar',       'Kumar',      'user83@tms.com', '9000000083', 3),
        (84, 'Urvi',         'Sharma',     'user84@tms.com', '9000000084', 3),
        (85, 'Vivek',        'Yadav',      'user85@tms.com', '9000000085', 3),
        (86, 'Yashika',      'Gupta',      'user86@tms.com', '9000000086', 3),
        (87, 'Zeel',         'Patel',      'user87@tms.com', '9000000087', 3),
        (88, 'Aarav',        'Mehta',      'user88@tms.com', '9000000088', 3),
        (89, 'Bhoomi',       'Shah',       'user89@tms.com', '9000000089', 3),
        (90, 'Chintan',      'Jain',       'user90@tms.com', '9000000090', 3),
        (91, 'Darshan',      'Raval',      'user91@tms.com', '9000000091', 3),
        (92, 'Esha',         'Kothari',    'user92@tms.com', '9000000092', 3),
        (93, 'Girish',       'Soni',       'user93@tms.com', '9000000093', 3),
        (94, 'Hina',         'Parekh',     'user94@tms.com', '9000000094', 3),
        (95, 'Indra',        'Pillai',     'user95@tms.com', '9000000095', 3),
        (96, 'Jinal',        'Doshi',      'user96@tms.com', '9000000096', 3),
        (97, 'Karan',        'Sheth',      'user97@tms.com', '9000000097', 3),
        (98, 'Lata',         'Kulkarni',   'user98@tms.com', '9000000098', 3),
        (99, 'Mayur',        'Patel',      'user99@tms.com', '9000000099', 3),
        (100,'Nidhi',        'Agarwal',    'user100@tms.com','9000000100', 3),
        (101,'Paresh',       'Naik',       'user101@tms.com','9000000101', 3)
) AS Source
(
    Id,
    FirstName,
    LastName,
    Email,
    Phone,
    RoleId
)
ON Target.Id = Source.Id

WHEN MATCHED THEN
    UPDATE SET
        Target.FirstName    = Source.FirstName,
        Target.LastName     = Source.LastName,
        Target.Email        = Source.Email,
        Target.Phone        = Source.Phone,
        Target.RoleId       = Source.RoleId,
        Target.IsActive     = 1,
        Target.ModifiedBy   = 1,
        Target.ModifiedOn   = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Id,
        FirstName,
        LastName,
        Email,
        Phone,
        RoleId,
        PasswordHash,
        PasswordSalt,
        LastLogin,
        IsActive,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        Source.Id,
        Source.FirstName,
        Source.LastName,
        Source.Email,
        Source.Phone,
        Source.RoleId,
        'vvFrThq6ZrmvJmg+0nDYc5vP3yNyPLzp5WS8lEVeHho=',
        'VR9qCeoff7StfUAnQSdUMuWJOxr5nPl8W8ZwGcoV6CU=',
        NULL,
        1,
        1,
        SYSDATETIMEOFFSET()
    );


 /*
    Default Teams
*/
SET IDENTITY_INSERT [dbo].[Team] ON;

MERGE [dbo].[Team] AS Target
USING
(
    VALUES
        (1,  'Alpha Squad',   'Core platform engineering and infrastructure team',        2),
        (2,  'Beta Squad',    'Frontend web and mobile application development team',     3),
        (3,  'Gamma Squad',   'Backend services and API development team',                4),
        (4,  'Delta Squad',   'Quality assurance and automated testing team',             5),
        (5,  'Epsilon Squad', 'DevOps, CI/CD and cloud operations team',                  6),
        (6,  'Zeta Squad',    'Data engineering and analytics team',                      7),
        (7,  'Eta Squad',     'Machine learning and AI research team',                    8),
        (8,  'Theta Squad',   'Cybersecurity and compliance team',                        9),
        (9,  'Iota Squad',    'Customer support and success team',                        10),
        (10, 'Kappa Squad',   'Sales and business development team',                      11),
        (11, 'Lambda Squad',  'Marketing and brand communications team',                  12),
        (12, 'Mu Squad',      'Human resources and talent acquisition team',              13),
        (13, 'Nu Squad',      'Finance and accounting team',                               14),
        (14, 'Xi Squad',      'Legal and contracts team',                                  15),
        (15, 'Omicron Squad', 'Product management and strategy team',                     16),
        (16, 'Pi Squad',      'UI/UX design and research team',                           17),
        (17, 'Rho Squad',     'Database administration and optimization team',            18),
        (18, 'Sigma Squad',   'Integration and third-party partnerships team',             19),
        (19, 'Tau Squad',     'Release management and deployment team',                    20),
        (20, 'Upsilon Squad', 'Innovation lab and proof-of-concept team',                  21)
) AS Source
(
    Id,
    Name,
    Description,
    ManagerId
)
ON Target.Id = Source.Id

WHEN MATCHED THEN
    UPDATE SET
        Target.Name        = Source.Name,
        Target.Description = Source.Description,
        Target.ManagerId   = Source.ManagerId,
        Target.IsActive    = 1,
        Target.ModifiedBy  = 1,
        Target.ModifiedOn  = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Id,
        Name,
        Description,
        ManagerId,
        IsActive,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        Source.Id,
        Source.Name,
        Source.Description,
        Source.ManagerId,
        1,
        1,
        SYSDATETIMEOFFSET()
    );

SET IDENTITY_INSERT [dbo].[Team] OFF;

 /*
    Default Team members
*/


MERGE [dbo].[TeamMember] AS Target
USING
(
    VALUES
        -- Each team's manager is also a member (assigned by admin = 1)
        ( 1,  2, 1),   -- Team 1  / User 2
        ( 2,  3, 1),   -- Team 2  / User 3
        ( 3,  4, 1),
        ( 4,  5, 1),
        ( 5,  6, 1),
        ( 6,  7, 1),
        ( 7,  8, 1),
        ( 8,  9, 1),
        ( 9, 10, 1),
        (10, 11, 1),
        (11, 12, 1),
        (12, 13, 1),
        (13, 14, 1),
        (14, 15, 1),
        (15, 16, 1),
        (16, 17, 1),
        (17, 18, 1),
        (18, 19, 1),
        (19, 20, 1),
        (20, 21, 1),

        -- Regular users (RoleId=3, ids 52-101) assigned by team managers
        -- Team 1 members: users 52, 53  (assigned by manager 2)
        ( 1, 52,  2),
        ( 1, 53,  2),
        -- Team 2 members: users 54, 55  (assigned by manager 3)
        ( 2, 54,  3),
        ( 2, 55,  3),
        -- Team 3 members: users 56, 57  (assigned by manager 4)
        ( 3, 56,  4),
        ( 3, 57,  4),
        -- Team 4 members: users 58, 59  (assigned by manager 5)
        ( 4, 58,  5),
        ( 4, 59,  5),
        -- Team 5 members: users 60, 61  (assigned by manager 6)
        ( 5, 60,  6),
        ( 5, 61,  6),
        -- Team 6 members: users 62, 63  (assigned by manager 7)
        ( 6, 62,  7),
        ( 6, 63,  7),
        -- Team 7 members: users 64, 65  (assigned by manager 8)
        ( 7, 64,  8),
        ( 7, 65,  8),
        -- Team 8 members: users 66, 67  (assigned by manager 9)
        ( 8, 66,  9),
        ( 8, 67,  9),
        -- Team 9 members: users 68, 69  (assigned by manager 10)
        ( 9, 68, 10),
        ( 9, 69, 10),
        -- Team 10 members: users 70, 71 (assigned by manager 11)
        (10, 70, 11),
        (10, 71, 11),
        -- Team 11 members: users 72, 73 (assigned by manager 12)
        (11, 72, 12),
        (11, 73, 12),
        -- Team 12 members: users 74, 75 (assigned by manager 13)
        (12, 74, 13),
        (12, 75, 13),
        -- Team 13 members: users 76, 77 (assigned by manager 14)
        (13, 76, 14),
        (13, 77, 14),
        -- Team 14 members: users 78, 79 (assigned by manager 15)
        (14, 78, 15),
        (14, 79, 15),
        -- Team 15 members: users 80, 81 (assigned by manager 16)
        (15, 80, 16),
        (15, 81, 16),
        -- Team 16 members: users 82, 83 (assigned by manager 17)
        (16, 82, 17),
        (16, 83, 17),
        -- Team 17 members: users 84, 85 (assigned by manager 18)
        (17, 84, 18),
        (17, 85, 18),
        -- Team 18 members: users 86, 87 (assigned by manager 19)
        (18, 86, 19),
        (18, 87, 19),
        -- Team 19 members: users 88, 89 (assigned by manager 20)
        (19, 88, 20),
        (19, 89, 20),
        -- Team 20 members: users 90, 91 (assigned by manager 21)
        (20, 90, 21),
        (20, 91, 21),

        -- Remaining RoleId=3 users (92-101) distributed as extra members
        ( 1, 92,  2),
        ( 2, 93,  3),
        ( 3, 94,  4),
        ( 4, 95,  5),
        ( 5, 96,  6),
        ( 6, 97,  7),
        ( 7, 98,  8),
        ( 8, 99,  9),
        ( 9,100, 10),
        (10,101, 11)
) AS Source
(
    TeamId,
    UserId,
    AssignedBy
)
ON Target.TeamId = Source.TeamId
   AND Target.UserId = Source.UserId

WHEN MATCHED THEN
    UPDATE SET
        Target.AssignedBy = Source.AssignedBy,
        Target.IsActive   = 1

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        TeamId,
        UserId,
        AssignedBy,
        AssignedOn,
        IsActive
    )
    VALUES
    (
        Source.TeamId,
        Source.UserId,
        Source.AssignedBy,
        SYSDATETIMEOFFSET(),
        1
    );



/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
--------------------------------------------------------------------------------------
*/

-- ============================================================
-- 1) TaskStatus
-- ============================================================
MERGE [dbo].[TaskStatus] AS Target
USING
(
    VALUES
        (1, 'TODO',        'To Do'),
        (2, 'IN_PROGRESS', 'In Progress'),
        (3, 'IN_REVIEW',   'In Review'),
        (4, 'BLOCKED',     'Blocked'),
        (5, 'DONE',        'Done')
) AS Source
(
    Id,
    Code,
    Name
)
ON Target.Id = Source.Id

WHEN MATCHED THEN
    UPDATE SET
        Target.Code       = Source.Code,
        Target.Name       = Source.Name,
        Target.IsActive   = 1,
        Target.ModifiedBy = 1,
        Target.ModifiedOn = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Id,
        Code,
        Name,
        IsActive,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        Source.Id,
        Source.Code,
        Source.Name,
        1,
        1,
        SYSDATETIMEOFFSET()
    );


-- ============================================================
-- 2) Task  (100 tasks)
--    Match on Title (natural key) so re-runs update, not duplicate.
--    Columns in Source: Title, Description, StatusId, Priority,
--                        TeamId, AssigneeId, DueDate, CreatedBy
-- ============================================================
MERGE [dbo].[Task] AS Target
USING
(
    VALUES
        -- ---------- Team 1 (manager 2, members 52/53/92) ----------
        ('Set up CI pipeline for core platform',      'Configure GitHub Actions for build/test/deploy.',        2, 'High',    1, 52, DATEADD(day, 10, SYSDATETIMEOFFSET()), 2),
        ('Design infrastructure architecture',        'Draft architecture diagram for staging & prod.',          1, 'Medium',  1, 53, DATEADD(day, 20, SYSDATETIMEOFFSET()), 2),
        ('Migrate servers to cloud',                  'Move on-prem servers to Azure.',                          2, 'Urgent',  1, 92, DATEADD(day, 30, SYSDATETIMEOFFSET()), 2),
        ('Write platform onboarding guide',           'Docs for new engineers.',                                 5, 'Low',     1, 52, NULL,                                   2),
        ('Review security patches',                   'Apply latest OS patches.',                                3, 'High',    1, 53, DATEADD(day,  5, SYSDATETIMEOFFSET()), 2),

        -- ---------- Team 2 (manager 3, members 54/55/93) ----------
        ('Build responsive login page',               'Mobile-first login UI.',                                  2, 'High',    2, 54, DATEADD(day,  7, SYSDATETIMEOFFSET()), 3),
        ('Implement dark mode',                       'Theme toggle across web app.',                            1, 'Medium',  2, 55, DATEADD(day, 15, SYSDATETIMEOFFSET()), 3),
        ('Fix mobile nav bug',                        'Hamburger menu not closing.',                             4, 'Urgent',  2, 93, DATEADD(day,  2, SYSDATETIMEOFFSET()), 3),
        ('Write component tests',                     'Add Jest tests for shared components.',                   2, 'Medium',  2, 54, DATEADD(day, 12, SYSDATETIMEOFFSET()), 3),
        ('Accessibility audit',                       'WCAG 2.1 AA review of key screens.',                      3, 'Low',     2, 55, NULL,                                   3),

        -- ---------- Team 3 (manager 4, members 56/57/94) ----------
        ('Build user REST API',                       'CRUD endpoints for users.',                               2, 'High',    3, 56, DATEADD(day,  9, SYSDATETIMEOFFSET()), 4),
        ('Add JWT refresh tokens',                    'Short-lived access + refresh.',                           1, 'Urgent',  3, 57, DATEADD(day,  4, SYSDATETIMEOFFSET()), 4),
        ('Rate-limit public endpoints',               'Throttle per IP.',                                        3, 'Medium',  3, 94, DATEADD(day, 18, SYSDATETIMEOFFSET()), 4),
        ('API documentation',                         'Swagger/OpenAPI spec.',                                   5, 'Low',     3, 56, NULL,                                   4),
        ('Fix pagination bug',                        'Off-by-one in /items.',                                   4, 'High',    3, 57, DATEADD(day,  3, SYSDATETIMEOFFSET()), 4),

        -- ---------- Team 4 (manager 5, members 58/59/95) ----------
        ('Automate regression suite',                 'Selenium end-to-end suite.',                              2, 'High',    4, 58, DATEADD(day, 14, SYSDATETIMEOFFSET()), 5),
        ('Load testing plan',                         'JMeter scenarios.',                                       1, 'Medium',  4, 59, DATEADD(day, 21, SYSDATETIMEOFFSET()), 5),
        ('Fix flaky login test',                      'Intermittent timeout.',                                   2, 'High',    4, 95, DATEADD(day,  6, SYSDATETIMEOFFSET()), 5),
        ('QA sign-off release 1.0',                   'Full regression pass.',                                   3, 'Urgent',  4, 58, DATEADD(day,  8, SYSDATETIMEOFFSET()), 5),
        ('Document test strategy',                    'Test pyramid & coverage goals.',                          5, 'Low',     4, 59, NULL,                                   5),

        -- ---------- Team 5 (manager 6, members 60/61/96) ----------
        ('Set up Kubernetes cluster',                 'Prod-ready K8s on AKS.',                                  2, 'High',    5, 60, DATEADD(day, 11, SYSDATETIMEOFFSET()), 6),
        ('Implement blue-green deploy',               'Zero-downtime release strategy.',                         1, 'Urgent',  5, 61, DATEADD(day, 25, SYSDATETIMEOFFSET()), 6),
        ('Centralized logging',                       'ELK stack setup.',                                        3, 'Medium',  5, 96, DATEADD(day, 17, SYSDATETIMEOFFSET()), 6),
        ('CI/CD secret rotation',                     'Move secrets to Key Vault.',                              2, 'High',    5, 60, DATEADD(day,  5, SYSDATETIMEOFFSET()), 6),
        ('Cost optimization review',                  'Identify idle resources.',                                5, 'Low',     5, 61, NULL,                                   6),

        -- ---------- Team 6 (manager 7, members 62/63/97) ----------
        ('Build ETL for sales data',                  'Daily sales load to warehouse.',                          2, 'High',    6, 62, DATEADD(day, 13, SYSDATETIMEOFFSET()), 7),
        ('Create KPI dashboard',                      'Power BI dashboard for execs.',                           1, 'Medium',  6, 63, DATEADD(day, 22, SYSDATETIMEOFFSET()), 7),
        ('Fix data type mismatch',                    'Orders.amount truncation.',                               4, 'Urgent',  6, 97, DATEADD(day,  2, SYSDATETIMEOFFSET()), 7),
        ('Data quality checks',                       'Add validation rules.',                                   3, 'Medium',  6, 62, DATEADD(day, 16, SYSDATETIMEOFFSET()), 7),
        ('Archive old data',                          'Move >2y data to cold storage.',                          5, 'Low',     6, 63, NULL,                                   7),

        -- ---------- Team 7 (manager 8, members 64/65/98) ----------
        ('Train churn model',                         'XGBoost on customer data.',                               2, 'High',    7, 64, DATEADD(day, 19, SYSDATETIMEOFFSET()), 8),
        ('Evaluate embedding model',                  'Compare BERT variants.',                                  1, 'Medium',  7, 65, DATEADD(day, 28, SYSDATETIMEOFFSET()), 8),
        ('Deploy recommendation service',             'Serve model via REST.',                                   2, 'Urgent',  7, 98, DATEADD(day,  9, SYSDATETIMEOFFSET()), 8),
        ('ML pipeline docs',                          'Feature store & training docs.',                          5, 'Low',     7, 64, NULL,                                   8),
        ('Fix prediction drift alert',                'Drift threshold too low.',                                3, 'High',    7, 65, DATEADD(day,  6, SYSDATETIMEOFFSET()), 8),

        -- ---------- Team 8 (manager 9, members 66/67/99) ----------
        ('Penetration test web app',                  'External pentest scope.',                                 2, 'High',    8, 66, DATEADD(day, 15, SYSDATETIMEOFFSET()), 9),
        ('Implement MFA',                             'TOTP for all admins.',                                    1, 'Urgent',  8, 67, DATEADD(day,  7, SYSDATETIMEOFFSET()), 9),
        ('SOC 2 evidence collection',                 'Gather compliance artifacts.',                            3, 'Medium',  8, 99, DATEADD(day, 35, SYSDATETIMEOFFSET()), 9),
        ('Vulnerability triage',                      'Prioritize scanner findings.',                            2, 'High',    8, 66, DATEADD(day,  4, SYSDATETIMEOFFSET()), 9),
        ('Security awareness training',               'Phishing simulation.',                                    5, 'Low',     8, 67, NULL,                                   9),

        -- ---------- Team 9 (manager 10, members 68/69/100) ----------
        ('Improve ticket response SLA',               'Automate first-response.',                                2, 'High',    9, 68, DATEADD(day, 12, SYSDATETIMEOFFSET()), 10),
        ('Customer onboarding playbook',              'Step-by-step guide.',                                     1, 'Medium',  9, 69, DATEADD(day, 24, SYSDATETIMEOFFSET()), 10),
        ('Fix chat widget crash',                     'Edge browser issue.',                                     4, 'Urgent',  9,100, DATEADD(day,  2, SYSDATETIMEOFFSET()), 10),
        ('CSAT survey automation',                    'Trigger after ticket close.',                             3, 'Medium',  9, 68, DATEADD(day, 18, SYSDATETIMEOFFSET()), 10),
        ('Knowledge base cleanup',                    'Remove outdated articles.',                               5, 'Low',     9, 69, NULL,                                   10),

        -- ---------- Team 10 (manager 11, members 70/71/101) ----------
        ('Build lead scoring model',                  'Score inbound leads.',                                    2, 'High',    10, 70, DATEADD(day, 14, SYSDATETIMEOFFSET()), 11),
        ('CRM integration',                           'Two-way sync with HubSpot.',                              1, 'Medium',  10, 71, DATEADD(day, 26, SYSDATETIMEOFFSET()), 11),
        ('Fix quote PDF bug',                         'Totals rounding error.',                                  4, 'Urgent',  10,101, DATEADD(day,  3, SYSDATETIMEOFFSET()), 11),
        ('Sales pipeline report',                     'Weekly pipeline snapshot.',                               3, 'Medium',  10, 70, DATEADD(day, 11, SYSDATETIMEOFFSET()), 11),
        ('Partner outreach list',                     'Compile 100 targets.',                                    5, 'Low',     10, 71, NULL,                                   11),

        -- ---------- Team 11 (manager 12, members 72/73) ----------
        ('Launch email campaign Q1',                  'Draft + schedule.',                                       2, 'High',    11, 72, DATEADD(day,  8, SYSDATETIMEOFFSET()), 12),
        ('Redesign landing page',                     'New hero + CTA.',                                         1, 'Medium',  11, 73, DATEADD(day, 20, SYSDATETIMEOFFSET()), 12),
        ('SEO audit',                                 'Fix meta tags & sitemap.',                                3, 'High',    11, 72, DATEADD(day, 13, SYSDATETIMEOFFSET()), 12),
        ('Social media calendar',                     'Plan 30 days of posts.',                                  5, 'Low',     11, 73, NULL,                                   12),
        ('Fix broken marketing links',                '404 sweep on site.',                                      4, 'Medium',  11, 72, DATEADD(day,  4, SYSDATETIMEOFFSET()), 12),

        -- ---------- Team 12 (manager 13, members 74/75) ----------
        ('Update employee handbook',                  'Policy refresh 2026.',                                    2, 'Medium',  12, 74, DATEADD(day, 21, SYSDATETIMEOFFSET()), 13),
        ('Recruitment drive',                         'Hire 5 engineers.',                                       1, 'High',    12, 75, DATEADD(day, 40, SYSDATETIMEOFFSET()), 13),
        ('Onboarding automation',                     'Auto-provision accounts.',                                2, 'High',    12, 74, DATEADD(day, 18, SYSDATETIMEOFFSET()), 13),
        ('Performance review cycle',                  'Q1 reviews.',                                             3, 'Medium',  12, 75, DATEADD(day, 30, SYSDATETIMEOFFSET()), 13),
        ('Wellness program',                          'Launch gym stipend.',                                     5, 'Low',     12, 74, NULL,                                   13),

        -- ---------- Team 13 (manager 14, members 76/77) ----------
        ('Close monthly books',                       'Feb close.',                                              2, 'Urgent',  13, 76, DATEADD(day,  6, SYSDATETIMEOFFSET()), 14),
        ('AP/AR reconciliation',                      'Match invoices to POs.',                                  1, 'High',    13, 77, DATEADD(day, 10, SYSDATETIMEOFFSET()), 14),
        ('Budget forecast FY26',                      'Update rolling forecast.',                                3, 'Medium',  13, 76, DATEADD(day, 22, SYSDATETIMEOFFSET()), 14),
        ('Tax filing prep',                           'Gather supporting docs.',                                 2, 'High',    13, 77, DATEADD(day, 15, SYSDATETIMEOFFSET()), 14),
        ('Expense policy update',                     'Refresh travel rules.',                                   5, 'Low',     13, 76, NULL,                                   14),

        -- ---------- Team 14 (manager 15, members 78/79) ----------
        ('Review SaaS contracts',                     'Renewal T&Cs.',                                           2, 'Medium',  14, 78, DATEADD(day, 17, SYSDATETIMEOFFSET()), 15),
        ('GDPR compliance review',                    'DPA updates.',                                            1, 'High',    14, 79, DATEADD(day, 28, SYSDATETIMEOFFSET()), 15),
        ('Trademark filing',                          'Register new logo.',                                      3, 'Low',     14, 78, DATEADD(day, 45, SYSDATETIMEOFFSET()), 15),
        ('NDA template update',                       'Standardize NDA.',                                        2, 'Medium',  14, 79, DATEADD(day, 12, SYSDATETIMEOFFSET()), 15),
        ('Vendor risk assessment',                    'Tier-1 vendor review.',                                   5, 'Medium',  14, 78, NULL,                                   15),

        -- ---------- Team 15 (manager 16, members 80/81) ----------
        ('Product roadmap 2026',                      'Define quarterly OKRs.',                                  2, 'High',    15, 80, DATEADD(day, 19, SYSDATETIMEOFFSET()), 16),
        ('User interviews',                           '10 discovery calls.',                                     1, 'Medium',  15, 81, DATEADD(day, 25, SYSDATETIMEOFFSET()), 16),
        ('Competitive analysis',                      'Top 5 competitors.',                                      3, 'Medium',  15, 80, DATEADD(day, 14, SYSDATETIMEOFFSET()), 16),
        ('Pricing experiment',                        'A/B test new tiers.',                                     2, 'High',    15, 81, DATEADD(day, 30, SYSDATETIMEOFFSET()), 16),
        ('PRD for v2',                                'Write detailed PRD.',                                     5, 'Low',     15, 80, NULL,                                   16),

        -- ---------- Team 16 (manager 17, members 82/83) ----------
        ('Design system v2',                          'Tokens + components.',                                    2, 'High',    16, 82, DATEADD(day, 20, SYSDATETIMEOFFSET()), 17),
        ('Usability testing round 2',                 '5 participants.',                                         1, 'Medium',  16, 83, DATEADD(day, 16, SYSDATETIMEOFFSET()), 17),
        ('Icon set refresh',                          'New 200-icon set.',                                       3, 'Low',     16, 82, DATEADD(day, 27, SYSDATETIMEOFFSET()), 17),
        ('Fix contrast issues',                       'WCAG contrast audit fixes.',                              4, 'High',    16, 83, DATEADD(day,  5, SYSDATETIMEOFFSET()), 17),
        ('Prototype onboarding flow',                 'Figma clickable proto.',                                  2, 'Medium',  16, 82, DATEADD(day, 11, SYSDATETIMEOFFSET()), 17),

        -- ---------- Team 17 (manager 18, members 84/85) ----------
        ('Index tuning sprint',                       'Add missing indexes.',                                    2, 'High',    17, 84, DATEADD(day,  9, SYSDATETIMEOFFSET()), 18),
        ('Backup/restore drill',                      'Test PITR restore.',                                      1, 'Urgent',  17, 85, DATEADD(day,  3, SYSDATETIMEOFFSET()), 18),
        ('Partition large tables',                    'Range partition by date.',                                3, 'Medium',  17, 84, DATEADD(day, 23, SYSDATETIMEOFFSET()), 18),
        ('Query performance review',                  'Top 20 slow queries.',                                    2, 'High',    17, 85, DATEADD(day, 13, SYSDATETIMEOFFSET()), 18),
        ('DB upgrade plan',                           'SQL Server 2022 upgrade.',                                5, 'Low',     17, 84, NULL,                                   18),

        -- ---------- Team 18 (manager 19, members 86/87) ----------
        ('Partner API integration',                   'Connect Stripe webhooks.',                                2, 'High',    18, 86, DATEADD(day, 15, SYSDATETIMEOFFSET()), 19),
        ('Webhook retry logic',                       'Exponential backoff.',                                    1, 'Medium',  18, 87, DATEADD(day, 21, SYSDATETIMEOFFSET()), 19),
        ('Fix webhook signature bug',                 'HMAC mismatch.',                                          4, 'Urgent',  18, 86, DATEADD(day,  2, SYSDATETIMEOFFSET()), 19),
        ('Partner onboarding docs',                   'Integration guide.',                                      3, 'Low',     18, 87, DATEADD(day, 32, SYSDATETIMEOFFSET()), 19),
        ('Marketplace listing',                       'Publish integration listing.',                            2, 'Medium',  18, 86, DATEADD(day, 18, SYSDATETIMEOFFSET()), 19),

        -- ---------- Team 19 (manager 20, members 88/89) ----------
        ('Release v1.5 checklist',                    'Pre-release verification.',                               2, 'High',    19, 88, DATEADD(day,  4, SYSDATETIMEOFFSET()), 20),
        ('Changelog automation',                      'Generate from commits.',                                  1, 'Medium',  19, 89, DATEADD(day, 12, SYSDATETIMEOFFSET()), 20),
        ('Rollback runbook',                          'Document rollback steps.',                                3, 'High',    19, 88, DATEADD(day,  7, SYSDATETIMEOFFSET()), 20),
        ('Release notes v1.5',                        'Customer-facing notes.',                                  5, 'Low',     19, 89, NULL,                                   20),
        ('Fix deploy pipeline timeout',               'Increase runner timeout.',                                4, 'Medium',  19, 88, DATEADD(day,  3, SYSDATETIMEOFFSET()), 20),

        -- ---------- Team 20 (manager 21, members 90/91) ----------
        ('POC: AI assistant',                         'Evaluate LLM for support.',                               2, 'High',    20, 90, DATEADD(day, 30, SYSDATETIMEOFFSET()), 21),
        ('POC: voice search',                         'Speech-to-text prototype.',                               1, 'Medium',  20, 91, DATEADD(day, 35, SYSDATETIMEOFFSET()), 21),
        ('Innovation demo day',                       'Plan internal demo.',                                     3, 'Low',     20, 90, DATEADD(day, 40, SYSDATETIMEOFFSET()), 21),
        ('Evaluate vector DBs',                       'Pinecone vs pgvector.',                                   2, 'Medium',  20, 91, DATEADD(day, 22, SYSDATETIMEOFFSET()), 21),
        ('Hackathon logistics',                       'Organize quarterly hackathon.',                           5, 'Low',     20, 90, NULL,                                   21),

        -- ---------- Personal / team-less tasks (TeamId NULL) ----------
        ('Submit timesheet',                          'Weekly timesheet.',                                       2, 'Low',     NULL, 52, DATEADD(day,  2, SYSDATETIMEOFFSET()), 1),
        ('Complete security training',                'Annual compliance module.',                               1, 'Medium',  NULL, 53, DATEADD(day, 10, SYSDATETIMEOFFSET()), 1),
        ('Update profile picture',                    'HR portal.',                                              5, 'Low',     NULL, 54, NULL,                                   1),
        ('Read new expense policy',                   'Acknowledge in HR portal.',                               3, 'Low',     NULL, 55, DATEADD(day, 14, SYSDATETIMEOFFSET()), 1),
        ('Set quarterly goals',                       'Align with manager.',                                     1, 'Medium',  NULL, 56, DATEADD(day, 21, SYSDATETIMEOFFSET()), 1)
) AS Source
(
    Title,
    Description,
    StatusId,
    Priority,
    TeamId,
    AssigneeId,
    DueDate,
    CreatedBy
)
ON Target.Title = Source.Title

WHEN MATCHED THEN
    UPDATE SET
        Target.Description = Source.Description,
        Target.StatusId    = Source.StatusId,
        Target.Priority    = Source.Priority,
        Target.TeamId      = Source.TeamId,
        Target.AssigneeId  = Source.AssigneeId,
        Target.DueDate     = Source.DueDate,
        Target.IsActive    = 1,
        Target.ModifiedBy  = 1,
        Target.ModifiedOn  = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Title,
        Description,
        StatusId,
        Priority,
        TeamId,
        AssigneeId,
        DueDate,
        IsActive,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        Source.Title,
        Source.Description,
        Source.StatusId,
        Source.Priority,
        Source.TeamId,
        Source.AssigneeId,
        Source.DueDate,
        1,
        Source.CreatedBy,
        SYSDATETIMEOFFSET()
    );


    /*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
--------------------------------------------------------------------------------------
*/

MERGE [dbo].[Portfolio] AS Target
USING
(
    VALUES
        ('Digital Transformation',      'DIG-TRANS',   'Modernize core business systems and migrate to cloud-native architecture.',            2),
        ('Customer Experience',         'CUST-EXP',    'Unified CRM, support tooling, and self-service portal initiatives.',                    3),
        ('Data & Analytics',            'DATA-ANAL',   'Data warehouse, BI dashboards, and self-service analytics platform.',                   4),
        ('Cloud Migration',             'CLD-MIG',     'Move on-prem workloads to Azure/AWS with cost and resilience targets.',                 5),
        ('Cybersecurity Program',       'CYBER-SEC',   'MFA rollout, SOC 2 readiness, and continuous vulnerability management.',                6),
        ('Mobile First',                'MOBILE-1',    'Native and hybrid mobile apps for customers and field staff.',                          7),
        ('AI & Machine Learning',       'AI-ML',       'Churn prediction, recommendation engine, and LLM assistant POCs.',                      8),
        ('DevOps Enablement',           'DEVOPS-EN',   'CI/CD pipelines, IaC, and observability tooling across teams.',                         9),
        ('Compliance & Governance',     'COMP-GOV',    'GDPR, SOC 2, and internal audit readiness programs.',                                   10),
        ('E-Commerce Platform',         'ECOM-PLAT',   'Storefront, checkout, and payment gateway integrations.',                               11),
        ('ERP Modernization',           'ERP-MOD',     'Finance, HR, and procurement ERP consolidation.',                                       12),
        ('Supply Chain Visibility',     'SCM-VIS',     'Real-time tracking and supplier collaboration portal.',                                 13),
        ('HR Transformation',           'HR-TRANS',    'Onboarding automation, talent management, and self-service HR.',                       14),
        ('Marketing Automation',        'MKT-AUTO',    'Campaign orchestration, lead scoring, and attribution reporting.',                      15),
        ('IoT & Edge',                  'IOT-EDGE',    'Sensor telemetry ingestion and edge analytics for operations.',                         16),
        ('API Platform',                'API-PLAT',    'Developer portal, API gateway, and partner integration ecosystem.',                    17),
        ('Legacy Decommission',         'LEG-DECOM',   'Retire mainframe and EOL applications with data archival.',                             18),
        ('Customer 360',                'CUST-360',    'Single view of customer across sales, support, and marketing.',                         19),
        ('Fraud Detection',             'FRAUD-DET',   'Real-time scoring and alerting for suspicious transactions.',                           20),
        ('Sustainability Reporting',    'SUST-REP',    'Carbon tracking and ESG reporting dashboards.',                                         21),
        ('Partner Ecosystem',           'PART-ECO',    'Marketplace listings, partner APIs, and joint go-to-market tooling.',                   2),
        ('Field Service Mobility',      'FSM-MOB',     'Mobile app for field technicians with offline capability.',                             3),
        ('Contact Center Modernization','CC-MOD',      'Cloud contact center with AI-assisted agents.',                                         4),
        ('Pricing & Revenue Mgmt',      'PRICING-RM',  'Dynamic pricing engine and revenue optimization.',                                      5),
        ('Quality Engineering',         'QA-ENG',      'Test automation, performance testing, and QA tooling.',                                 6),
        ('Identity & Access Mgmt',      'IAM',         'SSO, provisioning, and privileged access management.',                                  7),
        ('Network Upgrade',             'NET-UPG',     'SD-WAN and zero-trust network segmentation.',                                           8),
        ('Document Management',         'DOC-MGMT',    'DMS migration and e-signature integration.',                                             9),
        ('Workflow Automation',         'WF-AUTO',     'RPA and low-code workflow orchestration.',                                              10),
        ('Finance Analytics',           'FIN-ANAL',    'FP&A forecasting and variance dashboards.',                                             11),
        ('Risk Management',             'RISK-MGMT',   'Enterprise risk register and scenario modeling.',                                       12),
        ('Procurement Digitization',    'PROC-DIG',    'e-Procurement rollout and supplier portal.',                                             13),
        ('Inventory Optimization',      'INV-OPT',     'Demand forecasting and safety stock tuning.',                                           14),
        ('Loyalty Program',             'LOYALTY',     'Points engine, tiering, and personalized offers.',                                      15),
        ('Subscription Billing',        'SUB-BILL',    'Recurring billing, dunning, and revenue recognition.',                                  16),
        ('Content Management',          'CMS',         'Headless CMS migration for web and mobile.',                                            17),
        ('SEO & Web Presence',          'SEO-WEB',     'Site performance, SEO, and content strategy.',                                          18),
        ('Chatbot & Virtual Agent',     'V-AGENT',     'NLP chatbot for tier-1 support deflection.',                                            19),
        ('Knowledge Management',        'KMS',         'Centralized knowledge base and search.',                                                20),
        ('Customer Feedback Loop',      'CUST-FB',     'NPS, CSAT, and voice-of-customer analytics.',                                           21),
        ('Sales Enablement',            'SALES-EN',    'CRM playbooks, proposal automation, and coaching.',                                     2),
        ('Channel Management',          'CHAN-MGT',    'Partner portal and deal registration.',                                                 3),
        ('B2B Integration',             'B2B-INT',     'EDI, AS2, and partner onboarding.',                                                     4),
        ('Warehouse Automation',        'WMS-AUTO',    'WMS upgrade and robotics integration.',                                                 5),
        ('Transportation Mgmt',         'TMS',         'TMS rollout and carrier onboarding.',                                                   6),
        ('Trade Compliance',            'TRADE-COMP',  'Customs classification and restricted party screening.',                               7),
        ('Pricing Analytics',           'PRICE-ANAL',  'Competitive price monitoring and elasticity models.',                                  8),
        ('Product Lifecycle Mgmt',      'PLM',         'PLM consolidation and BOM management.',                                                9),
        ('Quality Management',          'QMS',         'Non-conformance tracking and CAPA workflows.',                                         10),
        ('Maintenance & Reliability',   'CMMS',        'Predictive maintenance and asset registry.',                                            11),
        ('Energy Management',           'ENERGY-MGT',  'Utility monitoring and efficiency initiatives.',                                        12),
        ('Facilities Management',       'FAC-MGT',     'Space planning and work-order automation.',                                             13),
        ('Travel & Expense',            'T&E',         'Expense automation and policy enforcement.',                                            14),
        ('Payroll Modernization',       'PAYROLL',     'Payroll engine replacement and integrations.',                                          15),
        ('Benefits Administration',     'BENEFITS',    'Benefits portal and carrier integrations.',                                             16),
        ('Learning Management',         'LMS',         'LMS rollout and content library.',                                                      17),
        ('Recruitment Tech',            'RECRUIT',     'ATS, sourcing, and interview scheduling.',                                              18),
        ('Employee Engagement',         'ENGAGE',      'Pulse surveys and recognition platform.',                                               19),
        ('Workforce Planning',          'WF-PLAN',     'Headcount planning and skills inventory.',                                              20),
        ('Time & Attendance',           'T&A',         'Clock-in automation and shift scheduling.',                                             21),
        ('Contract Lifecycle Mgmt',     'CLM',         'Contract authoring, approvals, and renewals.',                                          2),
        ('Legal Matter Mgmt',           'LEGAL-MGT',   'Matter intake and e-billing.',                                                          3),
        ('eDiscovery',                  'EDISC',       'Litigation hold and data preservation.',                                                4),
        ('Privacy Management',          'PRIV-MGT',    'DSAR automation and consent management.',                                               5),
        ('Records Management',          'REC-MGT',     'Retention schedules and archival.',                                                     6),
        ('Business Continuity',         'BCP',         'DR testing and continuity planning.',                                                   7),
        ('Incident Management',         'INC-MGT',     'On-call, incident response, and postmortems.',                                          8),
        ('Service Catalog',             'SVC-CAT',     'ITSM service catalog and request fulfillment.',                                         9),
        ('Asset Management',            'ASSET-MGT',   'IT asset inventory and lifecycle.',                                                     10),
        ('License Optimization',        'LIC-OPT',     'Software license true-up and SAM.',                                                     11),
        ('Vendor Management',           'VEND-MGT',    'Vendor risk, scorecards, and renewals.',                                                12),
        ('Cloud Cost Optimization',     'CLOUD-COST',  'FinOps dashboards and rightsizing.',                                                    13),
        ('Observability Platform',      'OBS-PLAT',    'Logs, metrics, traces, and alerting.',                                                  14),
        ('Release Management',          'REL-MGT',     'Release trains and change advisory.',                                                   15),
        ('Environment Management',      'ENV-MGT',     'Ephemeral environments and cost controls.',                                             16),
        ('Test Data Management',        'TDM',         'Synthetic data and masking.',                                                           17),
        ('API Security',                'API-SEC',     'Gateway authZ, rate limiting, and schema validation.',                                  18),
        ('Secrets Management',          'SECRETS',     'Vault rollout and secret rotation.',                                                     19),
        ('Endpoint Security',           'ENDP-SEC',    'EDR, patching, and device compliance.',                                                 20),
        ('SIEM & SOC',                  'SIEM-SOC',    'SIEM tuning and 24x7 SOC coverage.',                                                    21),
        ('Threat Intel',                'THREAT-INT',  'IOC feeds and detection engineering.',                                                  2),
        ('Red Team Program',            'RED-TEAM',    'Adversary simulation and purple teaming.',                                              3),
        ('Security Awareness',          'SEC-AWARE',   'Phishing simulations and training.',                                                    4),
        ('Cloud Security Posture',      'CSPM',        'Misconfiguration detection and remediation.',                                           5),
        ('Data Governance',             'DATA-GOV',    'Catalog, lineage, and data quality.',                                                   6),
        ('Master Data Management',      'MDM',         'Customer and product golden records.',                                                  7),
        ('Data Lakehouse',              'LAKEHOUSE',   'Delta Lake / Iceberg consolidation.',                                                   8),
        ('Streaming Platform',          'STREAMING',   'Kafka/Event Hub and real-time pipelines.',                                              9),
        ('BI Standardization',          'BI-STD',      'Consolidate BI tools and semantic layer.',                                              10),
        ('Self-Service Analytics',      'SS-ANAL',     'Data catalog and governed self-service.',                                               11),
        ('Experimentation Platform',    'EXP-PLAT',    'A/B testing and feature flags.',                                                        12),
        ('Feature Store',               'FEAT-STORE',  'Online/offline features for ML.',                                                       13),
        ('MLOps',                       'MLOPS',       'Model registry, monitoring, and retraining.',                                           14),
        ('GenAI Enablement',            'GENAI',       'LLM guardrails, RAG, and prompt ops.',                                                  15),
        ('Computer Vision',             'CV',          'Document and image classification.',                                                    16),
        ('Speech & NLP',                'SPEECH-NLP',  'Transcription and intent detection.',                                                   17),
        ('Recommendation Engine',       'REC-ENG',     'Personalization and cross-sell.',                                                       18),
        ('Forecasting Platform',        'FORECAST',    'Demand and revenue forecasting.',                                                       19),
        ('Optimization Engine',         'OPT-ENG',     'Routing, scheduling, and inventory optimization.',                                      20),
        ('Innovation Lab',              'INNO-LAB',    'POCs, hackathons, and emerging tech evaluation.',                                       21)
) AS Source
(
    Name,
    Code,
    Description,
    OwnerId
)
ON Target.Code = Source.Code

WHEN MATCHED THEN
    UPDATE SET
        Target.Name        = Source.Name,
        Target.Description = Source.Description,
        Target.OwnerId     = Source.OwnerId,
        Target.IsActive    = 1,
        Target.ModifiedBy  = 1,
        Target.ModifiedOn  = SYSDATETIMEOFFSET()

WHEN NOT MATCHED BY TARGET THEN
    INSERT
    (
        Name,
        Code,
        Description,
        OwnerId,
        CreatedBy,
        CreatedOn,
        IsActive
    )
    VALUES
    (
        Source.Name,
        Source.Code,
        Source.Description,
        Source.OwnerId,
        1,
        SYSDATETIMEOFFSET(),
        1
    );