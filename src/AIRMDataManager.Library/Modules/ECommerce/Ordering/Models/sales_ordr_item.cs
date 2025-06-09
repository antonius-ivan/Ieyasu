using System;
using System.Collections.Generic;

namespace AIRMDataManager.Library.Modules.ECommerce.Ordering.Models;

public partial class sales_ordr_item
{
    public int id { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public decimal Discount { get; set; }

    public string PictureUrl { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Units { get; set; }
}
