using Microsoft.EntityFrameworkCore;
using MyStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreBis
{
    public class GetOrders
    {
        private readonly MyStoreContext _context;

        public GetOrders(MyStoreContext context)
        {
            _context = context; // نخزن النسخة المحقونة جوه المتغير حقنا
        }

        // الدالة جاهزة ومطابقة تماماً لسياق مشروعك
        public async Task<object> GetUserOrdersAsync(int userId)
        {
            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId) // تأكد إذا كان الـ Id عندك string أو int وتوافق معه
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new
                {

                    OrderId = o.OrderId,
                    City = o.City,
                    AddressLine = o.AddressLine,
                    OrderStatus = o.OrderStatus,
                    CreatedAt = o.CreatedAt,
                    OrderTotalPrice = o.TotalPrice,

                    Items = o.OrderItems.Select(oi => new
                    {
                        ProductName = oi.Product.ProductName,
                        ProductImage = oi.Product.ProductImage,
                        Quantity = oi.Quantity,
                        ItemPrice = oi.Price
                    })
                })
                .ToListAsync();

            return userOrders;
        }
    }
}