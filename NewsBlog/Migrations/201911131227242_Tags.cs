namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagBlogItems",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        BlogItem_NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.BlogItem_NewsId })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.BlogItems", t => t.BlogItem_NewsId, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.BlogItem_NewsId);
            
            DropColumn("dbo.BlogItems", "Tags");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogItems", "Tags", c => c.String());
            DropForeignKey("dbo.TagBlogItems", "BlogItem_NewsId", "dbo.BlogItems");
            DropForeignKey("dbo.TagBlogItems", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagBlogItems", new[] { "BlogItem_NewsId" });
            DropIndex("dbo.TagBlogItems", new[] { "Tag_Id" });
            DropTable("dbo.TagBlogItems");
            DropTable("dbo.Tags");
        }
    }
}
