namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReviewComment_Imburse_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReviewComment", "Imburse", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReviewComment", "Imburse");
        }
    }
}
