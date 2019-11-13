namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogItems", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogItems", "Image");
        }
    }
}
