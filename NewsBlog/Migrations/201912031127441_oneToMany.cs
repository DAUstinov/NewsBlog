namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oneToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BlogItems", "CategoryId", c => c.Int());
            CreateIndex("dbo.BlogItems", "CategoryId");
            AddForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories", "Id");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Picture = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BlogItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.BlogItems", new[] { "CategoryId" });
            DropColumn("dbo.BlogItems", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
