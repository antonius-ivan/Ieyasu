using System;
using System.Collections.Generic;

namespace AIRMDataManager.Library.Modules.ECommerce.Payment.Models;

public partial class payment_method
{
    public int id { get; set; }

    public string payment_code { get; set; } = null!;

    public string payment_desc { get; set; } = null!;

    public string payment_desc_ina { get; set; }

    public string payment_desc_nm { get; set; } = null!;

    public byte? rec_df { get; set; }

    public byte info_ty { get; set; }

    public bool hide_fg { get; set; }
}
