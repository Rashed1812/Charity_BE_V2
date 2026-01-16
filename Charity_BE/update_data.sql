-- تحديث بيانات أنواع الطلبات بالترميز الصحيح
USE [CharityOfficialDataBase2];
GO

-- حذف البيانات القديمة
DELETE FROM [dbo].[ReconcileRequestTypes];
GO

-- إدراج البيانات الصحيحة
INSERT INTO [dbo].[ReconcileRequestTypes] ([Name], [Description], [IsActive])
VALUES
(N'استشارة أسرية', N'استشارة تتعلق بالمشاكل الأسرية', 1),
(N'استشارة نفسية', N'استشارة تتعلق بالمشاكل النفسية', 1),
(N'وساطة تجارية', N'وساطة في النزاعات التجارية', 1),
(N'استشارة زوجية', N'استشارة تتعلق بالمشاكل الزوجية', 1);
GO

-- عرض البيانات المحدثة
SELECT * FROM [dbo].[ReconcileRequestTypes];
GO