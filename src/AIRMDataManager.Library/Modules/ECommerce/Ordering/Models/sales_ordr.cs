using System;
using System.Collections.Generic;

namespace AIRMDataManager.Library.Modules.ECommerce.Ordering.Models;

public partial class sales_ordr
{
    public int id { get; set; }

    public string Address_Street { get; set; }

    public string Address_City { get; set; }

    public string Address_State { get; set; }

    public string Address_Country { get; set; }

    public string Address_ZipCode { get; set; }

    public string Description { get; set; }

    public int? BuyerId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? PaymentMethodId { get; set; }

    public string OrderStatus { get; set; } = null!;
}
