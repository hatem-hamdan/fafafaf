using MyStore.DataAccess.DTOs;
using MyStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreBis
{
    public class AddOrderBusiness
    {
   

      // 1. تعريف المتغير الخاص بالـ Context اللي بنستخدمه في الكلاس
        private readonly MyStoreContext _context;

        public AddOrderBusiness(MyStoreContext context)
        {
            _context = context; // نخزن النسخة المحقونة جوه المتغير حقنا
        }

        public async Task<int> InsertOrderAsync(OrderCreateDto dto)
        {
            var newOrder = new Order
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Region = dto.Region,
                City = dto.City,
                AddressLine = dto.AddressLine,
                CreatedAt = DateTime.Now,
                OrderStatus = "Pending",
                PaymentMethod = "Credit Card",
                TotalPrice = dto.Items.Sum(x => x.Price * x.Quantity),
               UserId = dto.UserId


            };

            foreach (var item in dto.Items)
            {
                newOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                         Color =  item.Color,

                    Price = item.Price
                });
            }

            await _context.Orders.AddAsync(newOrder);

            // الانتظار هنا أثناء الحفظ في قاعدة البيانات بدون حجز السيرفر
            await _context.SaveChangesAsync();

            return newOrder.OrderId;
        }






    }
}

