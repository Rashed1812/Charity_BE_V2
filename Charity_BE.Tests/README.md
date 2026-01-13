# نظام اختبار إصلاح ذات البين (Reconcile System Tests)

## نظرة عامة

يحتوي هذا المشروع على مجموعة شاملة من اختبارات API لنظام إصلاح ذات البين. تغطي الاختبارات جميع endpoints في النظام وتتحقق من صحة التدفق الكامل للعمليات.

## المتطلبات الأساسية

1. **تشغيل قاعدة البيانات**: تأكد من تشغيل SQL Server وأن قاعدة البيانات `CharityOfficialDataBase2` متاحة
2. **تشغيل التطبيق**: يجب أن يكون التطبيق يعمل على `http://localhost:5210`
3. **بيانات الاختبار**: يجب أن تحتوي قاعدة البيانات على بيانات الاختبار التالية:
   - Admin: `Admin@gmail.com` / `P@ssw0rd123`
   - Supervisor: `supervisor@gmail.com` / `P@ssw0rd123`
   - Mediation: `mediation@gmail.com` / `P@ssw0rd123`

## تشغيل الاختبارات

### من Visual Studio
1. افتح Test Explorer (Test → Test Explorer)
2. اختر "Run All Tests" أو "Run Selected Tests"

### من Command Line
```bash
# في مجلد Charity_BE.Tests
dotnet test

# لتشغيل اختبارات محددة
dotnet test --filter "ReconcileSystemTests"

# لتشغيل اختبار محدد
dotnet test --filter "ReconcileSystemTests.Login_Admin_ShouldReturnToken"
```

### من Visual Studio Code
```bash
# تشغيل جميع الاختبارات
dotnet test

# تشغيل مع تفاصيل أكثر
dotnet test -v n
```

## هيكل الاختبارات

### 1. اختبارات المصادقة (Authentication Tests)
- `Login_Admin_ShouldReturnToken`
- `Login_Supervisor_ShouldReturnToken`
- `Login_Mediation_ShouldReturnToken`

### 2. اختبارات أنواع الطلبات (ReconcileRequestType Tests)
- `GetAll_ReconcileRequestTypes_ShouldReturnSuccess`
- `GetActive_ReconcileRequestTypes_ShouldReturnOnlyActive`
- `GetById_ReconcileRequestType_ShouldReturnType`
- `GetById_ReconcileRequestType_InvalidId_ShouldReturnNotFound`

### 3. اختبارات الطلبات العامة (Public ReconcileRequest Tests)
- `Create_ReconcileRequest_Public_ShouldSucceed`
- `Create_ReconcileRequest_InvalidData_ShouldReturnBadRequest`

### 4. اختبارات المدير (Admin ReconcileRequest Tests)
- `GetAll_ReconcileRequests_Admin_ShouldReturnAllRequests`
- `GetById_ReconcileRequest_Admin_ShouldReturnRequest`
- `AssignToSupervisor_Admin_ShouldChangeStatus`
- `CancelRequest_Admin_ShouldChangeStatusToCancelled`

### 5. اختبارات المشرف (Supervisor Tests)
- `GetBySupervisorId_Supervisor_ShouldReturnHisRequests`
- `AssignToMediation_Supervisor_ShouldChangeStatus`
- `MarkAsReviewed_Supervisor_ShouldChangeStatus`

### 6. اختبارات المستشار (Mediation Tests)
- `GetByMediationId_Mediation_ShouldReturnHisRequests`
- `StartRequest_Mediation_ShouldChangeStatus`
- `CompleteExecution_Mediation_ShouldChangeStatus`
- `SendSMS_Mediation_ShouldSucceed`
- `GetSMSTemplates_Mediation_ShouldReturnTemplates`

### 7. اختبارات إدارة المشرفين (Supervisor Management Tests)
- `GetAll_Supervisors_Admin_ShouldReturnAll`
- `GetById_Supervisor_Admin_ShouldReturnSupervisor`

### 8. اختبارات إدارة المستشارين (Mediation Management Tests)
- `GetAll_Mediations_ShouldReturnAll`
- `GetById_Mediation_ShouldReturnMediation`

### 9. اختبار التدفق الكامل (Complete Workflow Test)
- `CompleteWorkflow_ShouldWorkEndToEnd`

## تغطية الاختبارات

### Endpoints المختبرة:

#### ReconcileRequestType Controller
- ✅ GET `/api/reconcilerequesttype`
- ✅ GET `/api/reconcilerequesttype/active`
- ✅ GET `/api/reconcilerequesttype/{id}`

#### ReconcileRequest Controller
- ✅ POST `/api/reconcilerequest` (Public)
- ✅ GET `/api/reconcilerequest` (Admin)
- ✅ GET `/api/reconcilerequest/{id}` (Admin/Supervisor/Mediation)
- ✅ POST `/api/reconcilerequest/{id}/assign-supervisor` (Admin)
- ✅ POST `/api/reconcilerequest/{id}/cancel` (Admin)
- ✅ POST `/api/reconcilerequest/{id}/complete` (Admin)
- ✅ GET `/api/reconcilerequest/supervisor/{supervisorId}` (Admin/Supervisor)
- ✅ POST `/api/reconcilerequest/{id}/assign-mediation` (Supervisor)
- ✅ POST `/api/reconcilerequest/{id}/mark-reviewed` (Supervisor)
- ✅ GET `/api/reconcilerequest/mediation/{mediationId}` (Admin/Mediation)
- ✅ POST `/api/reconcilerequest/{id}/start` (Mediation)
- ✅ POST `/api/reconcilerequest/{id}/complete-execution` (Mediation)
- ✅ POST `/api/reconcilerequest/{id}/send-sms` (Mediation)
- ✅ GET `/api/reconcilerequest/sms-templates` (Mediation)

#### Authentication Controller
- ✅ POST `/api/authentication/login`

#### Supervisor Controller
- ✅ GET `/api/supervisor` (Admin)
- ✅ GET `/api/supervisor/{id}` (Admin)

#### Mediation Controller
- ✅ GET `/api/mediation`
- ✅ GET `/api/mediation/{id}`

## سيناريوهات الأخطاء المختبرة

### Validation Errors
- بيانات إدخال غير صحيحة
- حقول مطلوبة مفقودة
- IDs غير موجودة

### Authorization Errors
- الوصول إلى endpoints محمية بدون token
- الوصول إلى endpoints لأدوار أخرى

### Business Logic Errors
- محاولة تغيير حالة غير صحيحة
- إسناد طلب لمشرف/مستشار غير موجود

## إعداد بيانات الاختبار

لتشغيل الاختبارات بنجاح، تأكد من وجود البيانات التالية في قاعدة البيانات:

### Users
```sql
-- Admin User
INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, FullName, IsActive)
VALUES ('admin-user-id', 'Admin@gmail.com', 'Admin@gmail.com', 1, 'مدير النظام', 1);

-- Supervisor User
INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, FullName, IsActive)
VALUES ('supervisor-user-id', 'supervisor@gmail.com', 'supervisor@gmail.com', 1, 'المشرف الأول', 1);

-- Mediation User
INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, FullName, IsActive)
VALUES ('mediation-user-id', 'mediation@gmail.com', 'mediation@gmail.com', 1, 'المستشار الأول', 1);
```

### Roles and User Roles
```sql
-- Roles
INSERT INTO AspNetRoles (Id, Name, NormalizedName)
VALUES
('admin-role-id', 'Admin', 'ADMIN'),
('supervisor-role-id', 'Supervisor', 'SUPERVISOR'),
('mediation-role-id', 'Mediation', 'MEDIATION');

-- User Roles
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES
('admin-user-id', 'admin-role-id'),
('supervisor-user-id', 'supervisor-role-id'),
('mediation-user-id', 'mediation-role-id');
```

### ReconcileRequestTypes
```sql
INSERT INTO ReconcileRequestTypes (Name, Description, IsActive, CreatedAt)
VALUES
('استشارة أسرية', 'استشارة تتعلق بالمشاكل الأسرية', 1, GETDATE()),
('استشارة نفسية', 'استشارة تتعلق بالمشاكل النفسية', 1, GETDATE());
```

### Supervisors and Mediations
```sql
-- Supervisor
INSERT INTO Supervisors (UserId, FullName, Specialty, IsActive, CreatedAt)
VALUES ('supervisor-user-id', 'المشرف الأول', 'استشاري أسري', 1, GETDATE());

-- Mediation
INSERT INTO Mediations (UserId, FullName, Specialty, IsActive, IsAvailable, CreatedAt)
VALUES ('mediation-user-id', 'المستشار الأول', 'استشاري نفسي', 1, 1, GETDATE());
```

## ملاحظات مهمة

1. **قاعدة البيانات**: تأكد من أن قاعدة البيانات تحتوي على البيانات المطلوبة قبل تشغيل الاختبارات
2. **التطبيق**: يجب أن يكون التطبيق يعمل أثناء تشغيل الاختبارات
3. **المنافذ**: التطبيق يجب أن يعمل على المنفذ 5210
4. **الأداء**: بعض الاختبارات قد تستغرق وقتاً أطول بسبب التفاعل مع قاعدة البيانات

## استكشاف الأخطاء

### مشاكل شائعة وحلولها:

1. **"No connection could be made"**
   - تأكد من تشغيل التطبيق على المنفذ الصحيح

2. **"401 Unauthorized"**
   - تأكد من صحة بيانات تسجيل الدخول
   - تأكد من وجود المستخدمين في قاعدة البيانات

3. **"404 Not Found"**
   - تأكد من وجود البيانات المطلوبة (مثل ReconcileRequestTypes)

4. **Database connection errors**
   - تأكد من تشغيل SQL Server
   - تأكد من صحة connection string

## إضافة اختبارات جديدة

لإضافة اختبارات جديدة:

1. أضف method جديد في `ReconcileSystemTests` class
2. استخدم `[Fact]` attribute للاختبارات البسيطة أو `[Theory]` للاختبارات المعلمة
3. اتبع نمط التسمية: `Action_Condition_ShouldExpectedResult`
4. استخدم FluentAssertions للتحقق من النتائج

### مثال:
```csharp
[Fact]
public async Task Create_ReconcileRequestType_Admin_ShouldSucceed()
{
    // Arrange
    var token = await GetAdminToken();
    var dto = new CreateReconcileRequestTypeDTO
    {
        Name = "نوع جديد",
        Description = "وصف النوع الجديد"
    };

    // Act
    var request = new HttpRequestMessage(HttpMethod.Post, "/api/reconcilerequesttype");
    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    request.Content = JsonContent.Create(dto);
    var response = await _client.SendAsync(request);
    var content = await response.Content.ReadFromJsonAsync<ApiResponse<ReconcileRequestTypeDTO>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    content.Should().NotBeNull();
    content.Success.Should().BeTrue();
    content.Data.Should().NotBeNull();
    content.Data.Name.Should().Be("نوع جديد");
}
```

## التقارير والتغطية

للحصول على تقارير تغطية الكود:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

لعرض التقارير:
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport
```

---

**تم إنشاء هذا الدليل بواسطة نظام اختبار إصلاح ذات البين**
**تاريخ الإنشاء: 13 يناير 2026**