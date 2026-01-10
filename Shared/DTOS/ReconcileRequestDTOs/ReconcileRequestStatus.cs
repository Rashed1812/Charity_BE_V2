namespace Shared.DTOS.ReconcileRequestDTOs
{
    public enum ReconcileRequestStatus
    {
        NewRequest = 1,                    // طلب جديد
        AssignedToSupervisor = 2,          // محال إلى مشرف
        AssignedToMediation = 3,           // محال إلى مستشار
        InProgress = 4,                    // قيد التنفيذ
        PendingSupervisorReview = 5,       // بانتظار مراجعة المشرف
        SupervisorReviewed = 6,            // تم مراجعة المشرف
        Completed = 7,                     // مكتمل
        Cancelled = 8                      // ملغي
    }
}

