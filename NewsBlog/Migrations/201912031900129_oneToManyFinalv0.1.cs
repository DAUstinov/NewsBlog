namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oneToManyFinalv01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.BlogItems", new[] { "CategoryId" });
            AlterColumn("dbo.BlogItems", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogItems", "CategoryId");
            AddForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.BlogItems", new[] { "CategoryId" });
            AlterColumn("dbo.BlogItems", "CategoryId", c => c.Int());
            CreateIndex("dbo.BlogItems", "CategoryId");
            AddForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories", "Id");
        }
    }
}
