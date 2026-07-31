namespace MyStore.DataAccess.DTOs
{
    public class OrderCreateDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; } // تعدل ليطابق الداتا بيز
        public string Region { get; set; }
        public string City { get; set; }
        public string AddressLine { get; set; } // تعدل ليطابق الداتا بيز
        public int UserId { get; set; }
        // مصفوفة المنتجات اللي جاية من سلة الـ React
        public List<OrderItemDto> Items { get; set; }
public decimal TotalPrice { get; set; }
        
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
                public string Color { get; set; }

    }
}
