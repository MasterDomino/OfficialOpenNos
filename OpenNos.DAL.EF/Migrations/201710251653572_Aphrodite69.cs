namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aphrodite69 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CellonOption",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Byte(nullable: false),
                        Type = c.Byte(nullable: false),
                        Value = c.Int(nullable: false),
                        EquipmentSerialId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CellonOption");
        }
    }
}
