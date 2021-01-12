namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentBlogItems",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false),
                        BlogItem_NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Comment_Id, t.BlogItem_NewsId })
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .ForeignKey("dbo.BlogItems", t => t.BlogItem_NewsId, cascadeDelete: true)
                .Index(t => t.Comment_Id)
                .Index(t => t.BlogItem_NewsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentBlogItems", "BlogItem_NewsId", "dbo.BlogItems");
            DropForeignKey("dbo.CommentBlogItems", "Comment_Id", "dbo.Comments");
            DropIndex("dbo.CommentBlogItems", new[] { "BlogItem_NewsId" });
            DropIndex("dbo.CommentBlogItems", new[] { "Comment_Id" });
            DropTable("dbo.CommentBlogItems");
            DropTable("dbo.Comments");
        }
    }
}
