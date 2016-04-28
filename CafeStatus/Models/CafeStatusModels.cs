using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CafeStatus.Models
{
    [Table("StatusLog")]
    public class CafeStatusModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoCafeStatus { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM HH:mm:ss}")]
        [Display(Name = "Hora")]
        public DateTime Data { get; set; }
        
        [Display(Name = "Status")]
        public bool Pronto { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [NotMapped]
        public string Status {
            get
            {
                return Pronto ? "Pega lá!" : "Ta buildando...";
            }
            set { }
        }

    }

    public class CafeDBContext : DbContext
    {
        public CafeDBContext() : base("name=StatusCafeDB")
        {

        }
        public DbSet<CafeStatusModels> Status { get; set; }
    }
}