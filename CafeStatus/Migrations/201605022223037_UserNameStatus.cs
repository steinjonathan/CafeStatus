namespace CafeStatus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNameStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatusLog", "UserName", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatusLog", "UserName");
        }
    }
}
