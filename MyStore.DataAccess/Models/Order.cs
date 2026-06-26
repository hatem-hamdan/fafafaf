using System;
using System.Collections.Generic;

namespace MyStore.DataAccess.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal TotalPrice { get; set; }

    public string FullName { get; set; }
    public string Region { get; set; } = null!;

    public string City { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
