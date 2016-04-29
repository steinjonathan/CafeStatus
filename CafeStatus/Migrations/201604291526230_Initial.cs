namespace CafeStatus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusLog",
                c => new
                    {
                        CodigoCafeStatus = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Pronto = c.Boolean(nullable: false),
                        Observacao = c.String(),
                    })
                .PrimaryKey(t => t.CodigoCafeStatus);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StatusLog");
        }
    }
}
