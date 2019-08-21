namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NoticeTemplate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoticeTemplates", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoticeTemplates", "Content", c => c.String());
        }
    }
}
