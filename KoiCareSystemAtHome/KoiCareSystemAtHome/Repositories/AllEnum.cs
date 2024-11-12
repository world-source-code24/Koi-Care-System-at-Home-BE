using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KoiCareSystemAtHome.Repositories
{
    public class AllEnum
    {
        public enum OrderStatus
        {
            Pending,
            Processing,
            Cancelled,
            Completed,
            Shiping,
            [Display(Name = "Shipping Completed")]
            ShipCompleted
        }
        
        public enum StatusPayment
        {
            Paid,
            Unpaid,
            Cancelled
        }

        public enum UserRole
        {
            Member,
            Admin,
            Guest
        }


    }
}
