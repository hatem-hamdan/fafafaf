using System;
using System.Collections.Generic;

namespace MyStore.DataAccess.Models;

public partial class OrderItem
{
   public int OrderItemId { get; set; }

  public int OrderId { get; set; }

  public int ProductId { get; set; }

  public int Quantity { get; set; }

  public string Color { get; set; } = string.Empty;

  public decimal Price { get; set; }

  public virtual Order Order { get; set; } = null!;

  public virtual Product Product { get; set; } = null!;
}
