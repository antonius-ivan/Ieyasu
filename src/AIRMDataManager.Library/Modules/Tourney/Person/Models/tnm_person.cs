using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AIRMDataManager.Library.Modules.Tourney.Person.Models
{
    public partial class tnm_person
    {
        [Key]
        public int id { get; set; }
        [StringLength(50)]
        public string person_firstname { get; set; }
        [StringLength(50)]
        public string person_lastname { get; set; }
        [Required]
        [StringLength(100)]
        public string person_fullname { get; set; }
        [StringLength(100)]
        public string person_email { get; set; }
        [StringLength(50)]
        public string w_cellphone1 { get; set; }
        [StringLength(50)]
        public string h_cellphone1 { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? cre_dttm { get; set; }
        [StringLength(5)]
        public string cre_by { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? upd_dttm { get; set; }
        [StringLength(5)]
        public string upd_by { get; set; }
    }
}
