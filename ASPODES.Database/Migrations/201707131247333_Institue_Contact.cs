namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Institue_Contact : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Institute", name: "ComtactId", newName: "Contact_UserId");
            RenameIndex(table: "dbo.Institute", name: "IX_ComtactId", newName: "IX_Contact_UserId");
            AddColumn("dbo.Institute", "ContactId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institute", "ContactId");
            RenameIndex(table: "dbo.Institute", name: "IX_Contact_UserId", newName: "IX_ComtactId");
            RenameColumn(table: "dbo.Institute", name: "Contact_UserId", newName: "ComtactId");
        }
    }
}
