using System;
using System.Collections.Generic;

namespace AIRMDataManager.Library.Modules.ECommerce.Customer.Models;

public partial class mst_customer
{
    public int id { get; set; }

    public string identity_guid { get; set; }

    public string cust_id { get; set; } = null!;

    public string main_username { get; set; } = null!;

    public string fullname { get; set; }

    public DateTime? birth_date { get; set; }

    public string nation_code { get; set; }

    public string id_no { get; set; }

    public string group_code { get; set; }

    public string cust_type { get; set; }

    public string cust_status { get; set; }

    public string pref_language { get; set; }

    public string staff_id { get; set; }

    public string company { get; set; }

    public string w_address1 { get; set; }

    public string w_address2 { get; set; }

    public string w_address3 { get; set; }

    public string w_city { get; set; }

    public string w_country { get; set; }

    public string w_zip { get; set; }

    public string w_phone1 { get; set; } = null!;

    public string w_phone2 { get; set; }

    public string w_fax { get; set; }

    public string h_address1 { get; set; }

    public string h_address2 { get; set; }

    public string h_address3 { get; set; }

    public string h_city { get; set; }

    public string h_country { get; set; }

    public string h_zip { get; set; }

    public string h_phone1 { get; set; }

    public string h_phone2 { get; set; }

    public string h_fax { get; set; }

    public string other_email { get; set; }

    public string service_code { get; set; } = null!;

    public string referred_by { get; set; }

    public DateTime created_date { get; set; }

    public string billing_code { get; set; }

    public decimal prev_balance { get; set; }

    public string last_inv_no { get; set; }

    public DateTime? last_pmt_received { get; set; }

    public DateTime? activation_date { get; set; }

    public string gen_pmt_stat { get; set; } = null!;

    public DateTime? gen_inv_date { get; set; }

    public string payment_code { get; set; }

    public bool taxable_fg { get; set; }

    public string tax_code { get; set; }

    public string curr_code { get; set; }

    public bool faktur_fg { get; set; }

    public string npwp { get; set; }

    public DateTime? frozen_date { get; set; }

    public short hours_free { get; set; }

    public int time_usage { get; set; }

    public DateTime? expired_date { get; set; }

    public string software_code { get; set; }

    public string note { get; set; }

    public string sort_1 { get; set; }

    public string msg_cd { get; set; }

    public byte? cust_grp_ty { get; set; }

    public string sex_cd { get; set; }

    public string mob_no_lst { get; set; }

    public short? pos_cd { get; set; }

    public short? last_edu_cd { get; set; }

    public short? hobby_cd { get; set; }

    public string w_prov_cd { get; set; }

    public string h_prov_cd { get; set; }

    public bool? monthly_chg_fg { get; set; }

    public int? monthly_crg_ty { get; set; }

    public DateTime? faktur_start_dt { get; set; }

    public string po_no { get; set; }

    public bool pay_in_advance_fg { get; set; }

    public string home_id { get; set; }

    public string alias_cust_id { get; set; } = null!;
}
