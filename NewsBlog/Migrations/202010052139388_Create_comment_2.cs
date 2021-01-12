namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_comment_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "CommentText", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "CommentText", c => c.Int(nullable: false));
        }
    }
}
