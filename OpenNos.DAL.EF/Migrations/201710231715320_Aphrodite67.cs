namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aphrodite67 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShellEffect", "ItemInstance_Id", "dbo.ItemInstance");
            DropIndex("dbo.ShellEffect", new[] { "ItemInstance_Id" });
            DropColumn("dbo.ShellEffect", "ItemInstance_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShellEffect", "ItemInstance_Id", c => c.Guid());
            CreateIndex("dbo.ShellEffect", "ItemInstance_Id");
            AddForeignKey("dbo.ShellEffect", "ItemInstance_Id", "dbo.ItemInstance", "Id");
        }
    }
}
