IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(50) NOT NULL,
        [Address] nvarchar(200) NULL,
        [ProfilePictureUrl] nvarchar(500) NULL,
        [CreatedAt] datetime2 NULL,
        [UpdatedAt] datetime2 NULL,
        [LastLoginAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Consultations] (
        [Id] int NOT NULL IDENTITY,
        [ConsultationName] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Consultations] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [DynamicPages] (
        [Id] int NOT NULL IDENTITY,
        [PageName] nvarchar(200) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Slug] nvarchar(100) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [CreatedBy] nvarchar(450) NULL,
        [UpdatedBy] nvarchar(450) NULL,
        CONSTRAINT [PK_DynamicPages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [HelpTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_HelpTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [HeroSections] (
        [Id] int NOT NULL IDENTITY,
        [BackgroundImageUrl] nvarchar(max) NOT NULL,
        [MainTitle] nvarchar(max) NULL,
        [Stats1Label] nvarchar(max) NULL,
        [Stats1Value] int NULL,
        [Stats2Label] nvarchar(max) NULL,
        [Stats2Value] int NULL,
        [Stats3Label] nvarchar(max) NULL,
        [Stats3Value] int NULL,
        [Stats4Label] nvarchar(max) NULL,
        [Stats4Value] int NULL,
        CONSTRAINT [PK_HeroSections] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [HomeVideoSections] (
        [Id] int NOT NULL IDENTITY,
        [VideoUrl] nvarchar(max) NOT NULL,
        [Title] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_HomeVideoSections] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [ImagesLibrary] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_ImagesLibrary] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [NavigationItems] (
        [Id] int NOT NULL IDENTITY,
        [label] nvarchar(max) NOT NULL,
        [href] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_NavigationItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [NewsItems] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Summary] nvarchar(500) NOT NULL,
        [ImageUrl] nvarchar(500) NOT NULL,
        [Author] nvarchar(100) NOT NULL,
        [Category] nvarchar(100) NOT NULL,
        [IsPublished] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [PublishedAt] datetime2 NULL,
        [UpdatedAt] datetime2 NULL,
        [ViewCount] int NOT NULL,
        [Tags] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_NewsItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(max) NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Message] nvarchar(2000) NOT NULL,
        [IsRead] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Type] int NOT NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [ServiceOfferings] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ServiceOfferings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [TrendSections] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [ImageUrl] nvarchar(max) NULL,
        [ButtonText] nvarchar(max) NULL,
        [ButtonUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_TrendSections] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [VideosLibraries] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [VideoUrl] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_VideosLibraries] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Admins] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [FullName] nvarchar(50) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Admins] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Admins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Complaints] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [Category] int NOT NULL,
        [Status] int NOT NULL,
        [Priority] nvarchar(20) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [ResolvedAt] datetime2 NULL,
        [Resolution] nvarchar(2000) NULL,
        CONSTRAINT [PK_Complaints] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Complaints_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Mediations] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [FullName] nvarchar(50) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsAvailable] bit NOT NULL,
        [CreatedAt] datetime2 NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Mediations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Mediations_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [ReconcileRequests] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [RequestText] nvarchar(2000) NOT NULL,
        [UserId] nvarchar(450) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ReconcileRequests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReconcileRequests_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [VolunteerApplications] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [Address] nvarchar(200) NOT NULL,
        [Education] nvarchar(100) NOT NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_VolunteerApplications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_VolunteerApplications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Advisors] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [FullName] nvarchar(50) NOT NULL,
        [Specialty] nvarchar(100) NOT NULL,
        [Description] nvarchar(1000) NULL,
        [ZoomRoomUrl] nvarchar(500) NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [IsAvailable] bit NOT NULL,
        [CreatedAt] datetime2 NULL,
        [UpdatedAt] datetime2 NULL,
        [ConsultationId] int NULL,
        [ImageUrl] nvarchar(max) NULL,
        [ConsultationType] int NOT NULL,
        CONSTRAINT [PK_Advisors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Advisors_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Advisors_Consultations_ConsultationId] FOREIGN KEY ([ConsultationId]) REFERENCES [Consultations] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Lectures] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [VideoUrl] nvarchar(500) NOT NULL,
        [IsPublished] bit NOT NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [PublishedAt] datetime2 NULL,
        [UpdatedAt] datetime2 NULL,
        [ApplicationUserId] nvarchar(450) NULL,
        [ConsultationId] int NULL,
        CONSTRAINT [PK_Lectures] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Lectures_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Lectures_Consultations_ConsultationId] FOREIGN KEY ([ConsultationId]) REFERENCES [Consultations] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [DynamicPageItems] (
        [Id] int NOT NULL IDENTITY,
        [DynamicPageId] int NOT NULL,
        [Type] nvarchar(50) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(500) NULL,
        [FileUrl] nvarchar(500) NULL,
        [FileName] nvarchar(255) NULL,
        [VideoUrl] nvarchar(500) NULL,
        [Order] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_DynamicPageItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DynamicPageItems_DynamicPages_DynamicPageId] FOREIGN KEY ([DynamicPageId]) REFERENCES [DynamicPages] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [HelpRequests] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [Notes] nvarchar(1000) NOT NULL,
        [HelpTypeId] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_HelpRequests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HelpRequests_HelpTypes_HelpTypeId] FOREIGN KEY ([HelpTypeId]) REFERENCES [HelpTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [Pages] (
        [Id] int NOT NULL IDENTITY,
        [subTilte] nvarchar(max) NOT NULL,
        [subLink] nvarchar(max) NOT NULL,
        [NavItemsId] int NOT NULL,
        CONSTRAINT [PK_Pages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pages_NavigationItems_NavItemsId] FOREIGN KEY ([NavItemsId]) REFERENCES [NavigationItems] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [NewsImages] (
        [Id] int NOT NULL IDENTITY,
        [ImageUrl] nvarchar(500) NOT NULL,
        [NewsItemId] int NOT NULL,
        CONSTRAINT [PK_NewsImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_NewsImages_NewsItems_NewsItemId] FOREIGN KEY ([NewsItemId]) REFERENCES [NewsItems] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [ServiceOfferingItems] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(1000) NOT NULL,
        [Url] nvarchar(50) NOT NULL,
        [ImageUrl] nvarchar(500) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [ServiceOfferingId] int NOT NULL,
        CONSTRAINT [PK_ServiceOfferingItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ServiceOfferingItems_ServiceOfferings_ServiceOfferingId] FOREIGN KEY ([ServiceOfferingId]) REFERENCES [ServiceOfferings] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AdviceRequests] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [AdvisorId] int NULL,
        [AdvisorAvailabilityId] int NULL,
        [ConsultationType] int NOT NULL,
        [ConsultationId] int NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [Status] int NOT NULL,
        [Priority] nvarchar(20) NOT NULL,
        [RequestDate] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ConfirmedDate] datetime2 NULL,
        [CompletedDate] datetime2 NULL,
        [Response] nvarchar(2000) NOT NULL,
        [Rating] int NULL,
        [Review] nvarchar(500) NULL,
        CONSTRAINT [PK_AdviceRequests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AdviceRequests_Advisors_AdvisorId] FOREIGN KEY ([AdvisorId]) REFERENCES [Advisors] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AdviceRequests_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AdviceRequests_Consultations_ConsultationId] FOREIGN KEY ([ConsultationId]) REFERENCES [Consultations] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE TABLE [AdvisorAvailabilities] (
        [Id] int NOT NULL IDENTITY,
        [AdvisorId] int NOT NULL,
        [Date] datetime2 NOT NULL,
        [Time] time NOT NULL,
        [Duration] time NOT NULL,
        [ConsultationType] int NOT NULL,
        [IsBooked] bit NOT NULL,
        [AdviceRequestId] int NULL,
        [Notes] nvarchar(200) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_AdvisorAvailabilities] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AdvisorAvailabilities_AdviceRequests_AdviceRequestId] FOREIGN KEY ([AdviceRequestId]) REFERENCES [AdviceRequests] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AdvisorAvailabilities_Advisors_AdvisorId] FOREIGN KEY ([AdvisorId]) REFERENCES [Advisors] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BackgroundImageUrl', N'MainTitle', N'Stats1Label', N'Stats1Value', N'Stats2Label', N'Stats2Value', N'Stats3Label', N'Stats3Value', N'Stats4Label', N'Stats4Value') AND [object_id] = OBJECT_ID(N'[HeroSections]'))
        SET IDENTITY_INSERT [HeroSections] ON;
    EXEC(N'INSERT INTO [HeroSections] ([Id], [BackgroundImageUrl], [MainTitle], [Stats1Label], [Stats1Value], [Stats2Label], [Stats2Value], [Stats3Label], [Stats3Value], [Stats4Label], [Stats4Value])
    VALUES (1, N''/images/hero-bg.jpg'', N''مرحباً بكم في موقعنا'', N''عدد المستفيدين'', 1500, N''المشاريع المكتملة'', 120, N''المتطوعين'', 300, N''سنوات الخبرة'', 10)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BackgroundImageUrl', N'MainTitle', N'Stats1Label', N'Stats1Value', N'Stats2Label', N'Stats2Value', N'Stats3Label', N'Stats3Value', N'Stats4Label', N'Stats4Value') AND [object_id] = OBJECT_ID(N'[HeroSections]'))
        SET IDENTITY_INSERT [HeroSections] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Title', N'VideoUrl') AND [object_id] = OBJECT_ID(N'[HomeVideoSections]'))
        SET IDENTITY_INSERT [HomeVideoSections] ON;
    EXEC(N'INSERT INTO [HomeVideoSections] ([Id], [Description], [Title], [VideoUrl])
    VALUES (1, N''فيديو قصير يوضح أهم الأنشطة والخدمات التي نقدمها.'', N''تعرف على خدماتنا'', N''https://www.youtube.com/watch?v=abcd1234'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Title', N'VideoUrl') AND [object_id] = OBJECT_ID(N'[HomeVideoSections]'))
        SET IDENTITY_INSERT [HomeVideoSections] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'href', N'label') AND [object_id] = OBJECT_ID(N'[NavigationItems]'))
        SET IDENTITY_INSERT [NavigationItems] ON;
    EXEC(N'INSERT INTO [NavigationItems] ([Id], [href], [label])
    VALUES (1, N''/home'', N''الرئيسية''),
    (2, N''/about-layout/about'', N''عن الجمعية''),
    (3, N''/governance/regulations'', N''الحوكمة''),
    (4, N''/help-layout/we-offer'', N''طلبات المساعدة''),
    (5, N''/support-layout/bank-accounts'', N''المشاركة في الدعم''),
    (6, N''/blank-page'', N''التنمية وإصلاح ذات البين''),
    (7, N''/investment-layout/real-state'', N''النشاط الاستثماري''),
    (8, N''/medcineLayout/GeneralDefinition'', N''الخدمات الطبية''),
    (9, N''/volunteer-layout/unit'', N''التطوع''),
    (10, N''/about-layout/contact-phone'', N''التواصل والشكاوى'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'href', N'label') AND [object_id] = OBJECT_ID(N'[NavigationItems]'))
        SET IDENTITY_INSERT [NavigationItems] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Title') AND [object_id] = OBJECT_ID(N'[ServiceOfferings]'))
        SET IDENTITY_INSERT [ServiceOfferings] ON;
    EXEC(N'INSERT INTO [ServiceOfferings] ([Id], [Description], [Title])
    VALUES (1, N''Default Description'', N''Default Title'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Title') AND [object_id] = OBJECT_ID(N'[ServiceOfferings]'))
        SET IDENTITY_INSERT [ServiceOfferings] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ButtonText', N'ButtonUrl', N'Description', N'ImageUrl', N'Title') AND [object_id] = OBJECT_ID(N'[TrendSections]'))
        SET IDENTITY_INSERT [TrendSections] ON;
    EXEC(N'INSERT INTO [TrendSections] ([Id], [ButtonText], [ButtonUrl], [Description], [ImageUrl], [Title])
    VALUES (1, N''المزيد'', N''/initiatives'', N''تعرف على آخر المبادرات والبرامج التي أطلقناها لخدمة المجتمع.'', N''/images/trend.jpg'', N''أحدث مبادراتنا'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ButtonText', N'ButtonUrl', N'Description', N'ImageUrl', N'Title') AND [object_id] = OBJECT_ID(N'[TrendSections]'))
        SET IDENTITY_INSERT [TrendSections] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'NavItemsId', N'subLink', N'subTilte') AND [object_id] = OBJECT_ID(N'[Pages]'))
        SET IDENTITY_INSERT [Pages] ON;
    EXEC(N'INSERT INTO [Pages] ([Id], [NavItemsId], [subLink], [subTilte])
    VALUES (1001, 2, N''/about-layout/vision-mission'', N'' الرسالة والرؤية''),
    (1002, 2, N''/about-layout/about-summary'', N''نبذة عن الجمعية''),
    (1003, 2, N''/about-layout/tasks-authorities'', N''مهام الجمعية''),
    (1004, 2, N''/about-layout/general-assembly'', N''الجمعية العمومية''),
    (1005, 2, N''/about-layout/board-members'', N''مجلس الأدارة''),
    (1006, 2, N''/about-layout/organizational-structure'', N''الهيكل النظيمى''),
    (1007, 2, N''/about-layout/service-locations'', N''مواقع العمل''),
    (1008, 2, N''/about-layout/ImageLibrary'', N''مكتبة الصور والفيديوهات''),
    (1101, 3, N''/governance/regulations'', N''الأنظمة واللوائح ''),
    (1102, 3, N''/governance/policies'', N''السياسات''),
    (1103, 3, N''/blank-page'', N''التقارير السنوية''),
    (1104, 3, N''/governance/quarterly-reports'', N''التقارير الربعية''),
    (1105, 3, N''/governance/financial-reports'', N''القوائم المالية''),
    (1106, 3, N''/blank-page'', N''معايير الحوكمة''),
    (1107, 3, N''/governance/governance-evaluation'', N''نتائج تقييم الحوكمة''),
    (1108, 3, N''/governance/strategic-plans'', N''الخطة الاستراتيجية''),
    (1109, 3, N''/governance/operational-plan'', N''الخطة التشغيلية''),
    (1110, 3, N''/governance/Goals'', N''الأهداف''),
    (1201, 4, N''/help-layout/we-offer'', N''ماذا نقدم؟''),
    (1202, 4, N''/help-layout/eligible'', N''الفئات المستحقة''),
    (1203, 4, N''/help-layout/requirements'', N''المستندات المطلوبة''),
    (1204, 4, N''/HelpPeopole'', N''الحصول على الخدمة''),
    (1205, 4, N''/blank-page'', N''إحصاءات''),
    (1301, 5, N''/blank-page'', N''نشاطات الجمعية''),
    (1302, 5, N''/support-layout/bank-accounts'', N''حسابات الجمعية''),
    (1303, 5, N''https://jkmm.org.sa/ElectronicServices/Donate#'', N''الحصول على الخدمة''),
    (1304, 5, N''/blank-page'', N''إحصاءات''),
    (1401, 6, N''/blank-page'', N''تعريف عام بالنشاط''),
    (1402, 6, N''/all-consultants'', N''طلب استشارة أون لاين''),
    (1403, 6, N''/RequesrRepair'', N''طلب إصلاح ذات البين''),
    (1404, 6, N''/complaints'', N''تقديم شكوى''),
    (1405, 6, N''/awarness-lecture'', N''محاضرات توعوية''),
    (1406, 6, N''/blank-page'', N''إحصاءات''),
    (1501, 7, N''/blank-page'', N''تعريف عام بالنشاط''),
    (1502, 7, N''/investment-layout/real-state'', N''الاستثمارات العقارية''),
    (1503, 7, N''/investment-layout/invest-project'', N''المشروعات الاستثمارية''),
    (1504, 7, N''/blank-page'', N''مشروعات التنمية المستدامة''),
    (1505, 7, N''/blank-page'', N''إحصاءات''),
    (1601, 8, N''/medcineLayout/GeneralDefinition'', N''تعريف عام بالنشاط''),
    (1602, 8, N''/medcineLayout/MedicalCenter'', N''مركز غسيل الكلى''),
    (1603, 8, N''/medcineLayout/HairingCenter'', N''مركز السمع والنطق''),
    (1604, 8, N''/blank-page'', N''الخدمات الطبية من خارج الجمعية'');
    INSERT INTO [Pages] ([Id], [NavItemsId], [subLink], [subTilte])
    VALUES (1605, 8, N''/blank-page'', N''إحصاءات''),
    (1701, 9, N''/volunteer-layout/unit'', N''تعريف بالنشاط''),
    (1702, 9, N''/blank-page'', N''ميثاق التطوع''),
    (1703, 9, N''/volunteer-layout/volunteer-medical'', N''مجالات التطوع''),
    (1704, 9, N''/Voulenteer'', N''الحصول على الخدمة''),
    (1705, 9, N''/blank-page'', N''إحصاءات التطوع''),
    (1801, 10, N''/blank-page'', N''تقديم مقترح''),
    (1802, 10, N''/complaints'', N''تقديم شكوى''),
    (1803, 10, N''/Satisfaction'', N''قياسات الرضا'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'NavItemsId', N'subLink', N'subTilte') AND [object_id] = OBJECT_ID(N'[Pages]'))
        SET IDENTITY_INSERT [Pages] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Admins_UserId] ON [Admins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AdviceRequests_AdvisorAvailabilityId] ON [AdviceRequests] ([AdvisorAvailabilityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AdviceRequests_AdvisorId] ON [AdviceRequests] ([AdvisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AdviceRequests_ConsultationId] ON [AdviceRequests] ([ConsultationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AdviceRequests_UserId] ON [AdviceRequests] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_AdvisorAvailabilities_AdviceRequestId] ON [AdvisorAvailabilities] ([AdviceRequestId]) WHERE [AdviceRequestId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AdvisorAvailabilities_AdvisorId] ON [AdvisorAvailabilities] ([AdvisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_Advisors_ConsultationId] ON [Advisors] ([ConsultationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Advisors_UserId] ON [Advisors] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_Complaints_UserId] ON [Complaints] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_DynamicPageItems_DynamicPageId] ON [DynamicPageItems] ([DynamicPageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_HelpRequests_HelpTypeId] ON [HelpRequests] ([HelpTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_Lectures_ApplicationUserId] ON [Lectures] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_Lectures_ConsultationId] ON [Lectures] ([ConsultationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Mediations_UserId] ON [Mediations] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_NewsImages_NewsItemId] ON [NewsImages] ([NewsItemId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_Pages_NavItemsId] ON [Pages] ([NavItemsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_ReconcileRequests_UserId] ON [ReconcileRequests] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_ServiceOfferingItems_ServiceOfferingId] ON [ServiceOfferingItems] ([ServiceOfferingId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    CREATE INDEX [IX_VolunteerApplications_UserId] ON [VolunteerApplications] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    ALTER TABLE [AdviceRequests] ADD CONSTRAINT [FK_AdviceRequests_AdvisorAvailabilities_AdvisorAvailabilityId] FOREIGN KEY ([AdvisorAvailabilityId]) REFERENCES [AdvisorAvailabilities] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250809160217_last versio'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250809160217_last versio', N'8.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] DROP CONSTRAINT [FK_ReconcileRequests_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    DROP INDEX [IX_ReconcileRequests_UserId] ON [ReconcileRequests];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ReconcileRequests]') AND [c].[name] = N'UserId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ReconcileRequests] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ReconcileRequests] DROP COLUMN [UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [AssignedToMediationAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [AssignedToSupervisorAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [CompletedAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [ConsultantNotes] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [MediationId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [ReconcileRequestTypeId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [StartedAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [Status] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD [SupervisorId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [Mediations] ADD [Specialty] nvarchar(200) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE TABLE [ReconcileRequestAttachments] (
        [Id] int NOT NULL IDENTITY,
        [ReconcileRequestId] int NOT NULL,
        [FileUrl] nvarchar(500) NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [FileType] nvarchar(50) NOT NULL,
        [FileSize] bigint NOT NULL,
        [UploadedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_ReconcileRequestAttachments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReconcileRequestAttachments_ReconcileRequests_ReconcileRequestId] FOREIGN KEY ([ReconcileRequestId]) REFERENCES [ReconcileRequests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE TABLE [ReconcileRequestTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Description] nvarchar(1000) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ReconcileRequestTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE TABLE [Supervisors] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [FullName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [Specialty] nvarchar(200) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Supervisors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Supervisors_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE INDEX [IX_ReconcileRequests_MediationId] ON [ReconcileRequests] ([MediationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE INDEX [IX_ReconcileRequests_ReconcileRequestTypeId] ON [ReconcileRequests] ([ReconcileRequestTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE INDEX [IX_ReconcileRequests_SupervisorId] ON [ReconcileRequests] ([SupervisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE INDEX [IX_ReconcileRequestAttachments_ReconcileRequestId] ON [ReconcileRequestAttachments] ([ReconcileRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Supervisors_UserId] ON [Supervisors] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_Mediations_MediationId] FOREIGN KEY ([MediationId]) REFERENCES [Mediations] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_ReconcileRequestTypes_ReconcileRequestTypeId] FOREIGN KEY ([ReconcileRequestTypeId]) REFERENCES [ReconcileRequestTypes] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    ALTER TABLE [ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260110121656_UpdateReconcileRequestSystem'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260110121656_UpdateReconcileRequestSystem', N'8.0.17');
END;
GO

COMMIT;
GO

