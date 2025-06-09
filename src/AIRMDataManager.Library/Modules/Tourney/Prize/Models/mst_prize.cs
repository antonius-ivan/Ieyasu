using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AIRMDataManager.Library.Modules.Tourney.Prize.Models
{
    public partial class mst_prize
    {
        [Key]
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("prizeNumber")]
        public int? prize_number { get; set; }

        [StringLength(100)]
        [JsonPropertyName("prizeName")]
        public string prize_nm { get; set; }

        [Column(TypeName = "decimal(19, 4)")]
        [JsonPropertyName("prizeAmount")]
        public decimal prize_amt { get; set; }

        [Column(TypeName = "decimal(19, 4)")]
        [JsonPropertyName("prizePercentage")]
        public decimal prize_pctg { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("createdDate")]
        public DateTime? cre_dttm { get; set; }

        [StringLength(5)]
        [JsonPropertyName("createdBy")]
        public string? cre_by { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("updatedDate")]
        public DateTime? upd_dttm { get; set; }

        [StringLength(5)]
        [JsonPropertyName("updatedBy")]
        public string? upd_by { get; set; }
    }
}
