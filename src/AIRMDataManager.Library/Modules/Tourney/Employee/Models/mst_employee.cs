using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AIRMDataManager.Library.Modules.Tourney.Employee.Models
{
    [Table("mst_employee")]
    public partial class mst_employee
    {
        [Key]
        [JsonPropertyName("id")]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        [JsonPropertyName("firstName")]
        public string first_name { get; set; }

        [Required]
        [StringLength(50)]
        [JsonPropertyName("lastName")]
        public string last_name { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("email")]
        public string email { get; set; }

        [StringLength(50)]
        [JsonPropertyName("department")]
        public string? department { get; set; }

        [StringLength(50)]
        [JsonPropertyName("jobTitle")]
        public string? job_title { get; set; }

        [Column(TypeName = "date")]
        [JsonPropertyName("hireDate")]
        public DateTime? hire_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [JsonPropertyName("salary")]
        public decimal salary { get; set; }

        [JsonPropertyName("isActive")]
        public bool is_active { get; set; }

        [JsonPropertyName("comments")]
        public string? comments { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("createdAt")]
        public DateTime? created_at { get; set; }

        [StringLength(50)]
        [JsonPropertyName("createdBy")]
        public string? created_by { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("updatedAt")]
        public DateTime? updated_at { get; set; }

        [StringLength(50)]
        [JsonPropertyName("updatedBy")]
        public string? updated_by { get; set; }
    }
}
