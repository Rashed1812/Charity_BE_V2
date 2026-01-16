-- إصلاح قاعدة البيانات لنظام إصلاح ذات البين
-- إضافة الجداول والأعمدة المفقودة

USE [CharityOfficialDataBase2];
GO

-- إنشاء جدول ReconcileRequestTypes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ReconcileRequestTypes' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ReconcileRequestTypes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [Description] [nvarchar](1000) NULL,
        [IsActive] [bit] NOT NULL DEFAULT ((1)),
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_ReconcileRequestTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- إنشاء جدول Supervisors
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Supervisors' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Supervisors](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [nvarchar](450) NOT NULL,
        [FullName] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [PhoneNumber] [nvarchar](20) NOT NULL,
        [Specialty] [nvarchar](200) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT ((1)),
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Supervisors] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Supervisors_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_Supervisors_UserId] ON [dbo].[Supervisors]([UserId]);
END
GO

-- إضافة الأعمدة المفقودة لجدول ReconcileRequests
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'ReconcileRequestTypeId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [ReconcileRequestTypeId] [int] NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'Status')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [Status] [int] NOT NULL DEFAULT ((0));
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'SupervisorId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [SupervisorId] [int] NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'MediationId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [MediationId] [int] NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'ConsultantNotes')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [ConsultantNotes] [nvarchar](max) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'AssignedToSupervisorAt')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [AssignedToSupervisorAt] [datetime2](7) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'AssignedToMediationAt')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [AssignedToMediationAt] [datetime2](7) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'StartedAt')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [StartedAt] [datetime2](7) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ReconcileRequests') AND name = 'CompletedAt')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD [CompletedAt] [datetime2](7) NULL;
END
GO

-- إضافة Foreign Key Constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ReconcileRequests_ReconcileRequestTypes_ReconcileRequestTypeId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_ReconcileRequestTypes_ReconcileRequestTypeId]
        FOREIGN KEY([ReconcileRequestTypeId]) REFERENCES [dbo].[ReconcileRequestTypes] ([Id]) ON DELETE SET NULL;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ReconcileRequests_Supervisors_SupervisorId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_Supervisors_SupervisorId]
        FOREIGN KEY([SupervisorId]) REFERENCES [dbo].[Supervisors] ([Id]) ON DELETE SET NULL;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ReconcileRequests_Mediations_MediationId')
BEGIN
    ALTER TABLE [dbo].[ReconcileRequests] ADD CONSTRAINT [FK_ReconcileRequests_Mediations_MediationId]
        FOREIGN KEY([MediationId]) REFERENCES [dbo].[Mediations] ([Id]) ON DELETE NO ACTION;
END
GO

-- إضافة Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReconcileRequests_ReconcileRequestTypeId')
BEGIN
    CREATE INDEX [IX_ReconcileRequests_ReconcileRequestTypeId] ON [dbo].[ReconcileRequests]([ReconcileRequestTypeId]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReconcileRequests_Status')
BEGIN
    CREATE INDEX [IX_ReconcileRequests_Status] ON [dbo].[ReconcileRequests]([Status]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReconcileRequests_SupervisorId')
BEGIN
    CREATE INDEX [IX_ReconcileRequests_SupervisorId] ON [dbo].[ReconcileRequests]([SupervisorId]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReconcileRequests_MediationId')
BEGIN
    CREATE INDEX [IX_ReconcileRequests_MediationId] ON [dbo].[ReconcileRequests]([MediationId]);
END
GO

-- إدراج بيانات تجريبية
INSERT INTO [dbo].[ReconcileRequestTypes] ([Name], [Description], [IsActive])
VALUES
('استشارة أسرية', 'استشارة تتعلق بالمشاكل الأسرية', 1),
('استشارة نفسية', 'استشارة تتعلق بالمشاكل النفسية', 1),
('وساطة تجارية', 'وساطة في النزاعات التجارية', 1),
('استشارة زوجية', 'استشارة تتعلق بالمشاكل الزوجية', 1);
GO

PRINT 'تم إصلاح قاعدة البيانات بنجاح!';
PRINT 'تم إضافة الجداول والأعمدة المطلوبة لنظام إصلاح ذات البين.';
GO