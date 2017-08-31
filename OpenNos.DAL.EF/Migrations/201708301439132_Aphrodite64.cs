namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Aphrodite64 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "ReferrerId", c => c.Long(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Account", "ReferrerId");
        }
    }
}