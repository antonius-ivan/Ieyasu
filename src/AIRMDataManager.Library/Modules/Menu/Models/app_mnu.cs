using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRMDataManager.Library.Modules.Menu.Models
{
    public class app_mnu
    {
        [Key]
        public int mnu_id { get; set; }
        public int app_id { get; set; }
        public int? par_mnu_id { get; set; }
        public byte disp_ord { get; set; }
        [Required]
        [StringLength(35)]
        public string mnu_nm { get; set; }
        [Required]
        [StringLength(500)]
        public string mnu_desc { get; set; }
        public bool head_fg { get; set; }
        public int? df_mnu_id { get; set; }
        public int? frm_id { get; set; }
        [Required]
        [StringLength(250)]
        public string url_par { get; set; }
        [Required]
        [StringLength(50)]
        public string mnu_path { get; set; }
        public byte depth_lvl { get; set; }
        [StringLength(50)]
        public string par_mnu_ord_path { get; set; }
        [StringLength(50)]
        public string mnu_ord_path { get; set; }
        [Required]
        [StringLength(255)]
        public string auth_dept { get; set; }
        [Required]
        [StringLength(255)]
        public string unauth_dept { get; set; }
        [Required]
        [StringLength(1000)]
        public string auth_staff { get; set; }
        [Required]
        [StringLength(1000)]
        public string unauth_staff { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime cre_tm { get; set; }
        [Required]
        [StringLength(5)]
        public string cre_by { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? upd_tm { get; set; }
        [StringLength(5)]
        public string upd_by { get; set; }

    }
}
