namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_NoticeTemplate_Name : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NoticeTemplates", newName: "NoticeTemplate");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.NoticeTemplate", newName: "NoticeTemplates");
        }
    }
}
