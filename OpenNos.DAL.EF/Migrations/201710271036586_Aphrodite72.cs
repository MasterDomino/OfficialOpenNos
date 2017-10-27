namespace OpenNos.DAL.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Aphrodite72 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RecipeList");
            AlterColumn("dbo.RecipeList", "RecipeListId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RecipeList", "RecipeListId");
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.RecipeList");
            AlterColumn("dbo.RecipeList", "RecipeListId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RecipeList", "RecipeListId");
        }
    }
}
