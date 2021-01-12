namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop_old_comment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentBlogItems", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.CommentBlogItems", "BlogItem_NewsId", "dbo.BlogItems");
            DropIndex("dbo.CommentBlogItems", new[] { "Comment_Id" });
            DropIndex("dbo.CommentBlogItems", new[] { "BlogItem_NewsId" });
            DropTable("dbo.Comments");
            DropTable("dbo.CommentBlogItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CommentBlogItems",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false),
                        BlogItem_NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Comment_Id, t.BlogItem_NewsId });
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CommentBlogItems", "BlogItem_NewsId");
            CreateIndex("dbo.CommentBlogItems", "Comment_Id");
            AddForeignKey("dbo.CommentBlogItems", "BlogItem_NewsId", "dbo.BlogItems", "NewsId", cascadeDelete: true);
            AddForeignKey("dbo.CommentBlogItems", "Comment_Id", "dbo.Comments", "Id", cascadeDelete: true);
        }
    }
}
