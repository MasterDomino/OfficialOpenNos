namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Aphrodite63 : DbMigration
    {
        public override void Up() => AddColumn("dbo.NpcMonster", "Catch", c => c.Boolean(nullable: false));

        public override void Down() => DropColumn("dbo.NpcMonster", "Catch");
    }
}
