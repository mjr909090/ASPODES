namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_NoticeTemplate_Id : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplate");
            //DropPrimaryKey("dbo.NoticeTemplate");
            //AlterColumn("dbo.NoticeTemplate", "Id", c => c.Int(nullable: false));
            //AddPrimaryKey("dbo.NoticeTemplate", "Id");
            //AddForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplate", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplate");
            //DropPrimaryKey("dbo.NoticeTemplate");
            //AlterColumn("dbo.NoticeTemplate", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.NoticeTemplate", "Id");
            //AddForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplate", "Id");
        }
    }
}
