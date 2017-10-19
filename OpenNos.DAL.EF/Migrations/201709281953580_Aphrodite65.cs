namespace OpenNos.DAL.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Aphrodite65 : DbMigration
    {
        public override void Up() => AddColumn("dbo.Mail", "AttachmentLevel", c => c.Byte(nullable: false));

        public override void Down() => DropColumn("dbo.Mail", "AttachmentLevel");
    }
}
