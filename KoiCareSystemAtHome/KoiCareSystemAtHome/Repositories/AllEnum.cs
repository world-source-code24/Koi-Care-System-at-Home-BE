namespace KoiCareSystemAtHome.Repositories
{
    public class AllEnum
    {
        public enum OrderStatus
        {
            Pending,
            Cancelled,
            Completed
        }
        
        public enum StatusPayment
        {
            Paid,
            Unpaid,
            Refund
        }

        public enum UserRole
        {
            Member,
            Admin,
            Guest
        }
    }
}
