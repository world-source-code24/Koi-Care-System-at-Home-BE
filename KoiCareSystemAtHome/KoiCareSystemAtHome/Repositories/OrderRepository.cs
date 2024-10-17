//using KoiCareSystemAtHome.Entities;
//using KoiCareSystemAtHome.Models;
//using KoiCareSystemAtHome.Repositories.IRepositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Text;

//namespace KoiCareSystemAtHome.Repositories
//{
//    public class OrderRepository : IOrderRepository
//    {
//        private readonly KoiCareSystemDbContext _context;

//        public OrderRepository(KoiCareSystemDbContext context)
//        {
//            _context = context;
//        }

//        public List<OrderDTO> GetListOrder(int id)
//        {
//            var orderDb = _context.OrdersTbls.Where(o => o.Equals(id)).ToList();
            
//            // Check dk
//            if (orderDb.Count > 0) { return null; }
            
//            foreach (var orderStore in orderDb) {
//                OrderDTO order = new OrderDTO
//                {
//                    AccId = orderTbl.,
//                    Date = DateOnly.FromDateTime(DateTime.Now),
//                    StatusOrder = AllEnum.OrderStatus.Pending.ToString(),
//                    StatusPayment = AllEnum.StatusPayment.Unpaid.ToString(),
//                    TotalAmount = totalAmount

//                };
//        }

//        public List<int> GetOrderId(int id)
//        {
//            var orderTbl = _context.OrdersTbls.Select(o => o.OrderId).Where(o => o.Equals(id)).ToList();
//            if (orderTbl == null)
//            {
//                return null;
//            }
//            else
//            {
//                return orderTbl;
//            }
//        }

        
//    }
//}
