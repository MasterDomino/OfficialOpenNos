namespace OpenNos.DAL.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Aphrodite71 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipe", "MapNpcId", "dbo.MapNpc");
            DropIndex("dbo.Recipe", new[] { "MapNpcId" });
            CreateTable(
                "dbo.RecipeList",
                c => new
                    {
                        RecipeListId = c.Int(nullable: false),
                        RecipeId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.RecipeListId)
                .ForeignKey("dbo.Recipe", t => t.RecipeId)
                .Index(t => t.RecipeId);

            AddColumn("dbo.Item", "RecipeListId", c => c.Int());
            AddColumn("dbo.MapNpc", "RecipeListId", c => c.Int());
            CreateIndex("dbo.Item", "RecipeListId");
            CreateIndex("dbo.MapNpc", "RecipeListId");
            AddForeignKey("dbo.MapNpc", "RecipeListId", "dbo.RecipeList", "RecipeListId");
            AddForeignKey("dbo.Item", "RecipeListId", "dbo.RecipeList", "RecipeListId");
            DropColumn("dbo.Recipe", "MapNpcId");
        }

        public override void Down()
        {
            AddColumn("dbo.Recipe", "MapNpcId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Item", "RecipeListId", "dbo.RecipeList");
            DropForeignKey("dbo.MapNpc", "RecipeListId", "dbo.RecipeList");
            DropForeignKey("dbo.RecipeList", "RecipeId", "dbo.Recipe");
            DropIndex("dbo.RecipeList", new[] { "RecipeId" });
            DropIndex("dbo.MapNpc", new[] { "RecipeListId" });
            DropIndex("dbo.Item", new[] { "RecipeListId" });
            DropColumn("dbo.MapNpc", "RecipeListId");
            DropColumn("dbo.Item", "RecipeListId");
            DropTable("dbo.RecipeList");
            CreateIndex("dbo.Recipe", "MapNpcId");
            AddForeignKey("dbo.Recipe", "MapNpcId", "dbo.MapNpc", "MapNpcId");
        }
    }
}
