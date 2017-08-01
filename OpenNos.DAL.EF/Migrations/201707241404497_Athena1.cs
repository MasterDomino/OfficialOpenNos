namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Athena1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GeneralLog", "LogType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GeneralLog", "LogType", c => c.String());
        }
    }
}
