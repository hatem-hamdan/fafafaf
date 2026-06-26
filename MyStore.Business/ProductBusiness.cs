using MyStore.DataAccess.Models; // ريفرنس للأكسس عشان يقدر يشوف الموديلز والـ Context
using Microsoft.EntityFrameworkCore;

namespace MyStoreBis
{
    public class ProductBusiness
    {
        // 1. تعريف المتغير الخاص بالـ Context اللي بنستخدمه في الكلاس
        private readonly MyStoreContext _context;

        // 2. الـ Constructor: هنا المكان اللي الـ .NET بيحقن فيه الـ Context تلقائياً
        public ProductBusiness(MyStoreContext context)
        {
            _context = context; // نخزن النسخة المحقونة جوه المتغير حقنا
        }

        // 3. دالة جلب المنتجات من قاعدة البيانات باستخدام الـ Context المحقون
        public async Task<List<object>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.ProductPrice,
                    p.ProductImage,
                    p.Stock
                })
                .Cast<object>()
                .ToListAsync();
        }
    }
}