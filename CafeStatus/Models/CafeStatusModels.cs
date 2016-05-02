using CafeStatus.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [StringLength(256)]
        public string UserName { get; set; }
    }

    public class PushDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public string Endpoint { get; set; }
    }


    public class CafeDBContext : IdentityDbContext<ApplicationUser>
    {
        public CafeDBContext() : base("name=StatusCafeDB")
        {

        }
        public DbSet<CafeStatusModels> Status { get; set; }

        public DbSet<PushDevice> PushDevices { get; set; }

        public static CafeDBContext Create()
        {
            return new CafeDBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CafeDBContext, Configuration>());
        }
    }
}