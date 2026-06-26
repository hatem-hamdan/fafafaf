using Microsoft.EntityFrameworkCore;
using MyStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreBis
{
 

    public class GetAllOrders
    {



        private readonly MyStoreContext _context;

        public GetAllOrders(MyStoreContext context)
        {
            _context = context; // نخزن النسخة المحقونة جوه المتغير حقنا
        }



        public async Task<object> GetAllOrdersAsync()
        {
            var orders = await _context.Orders

                .OrderByDescending(o => o.CreatedAt)

                .Select(o => new
                {
                    OrderId = o.OrderId,

                    Username = o.User.UserName,

                    Email = o.User.Email,

                    City = o.City,

                    TotalPrice = o.TotalPrice,

                    Status = o.OrderStatus,

                    CreatedAt = o.CreatedAt
                })

                .ToListAsync();

            return orders;
        }








    }



}
 