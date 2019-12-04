namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tags", "TagName", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String());
            AlterColumn("dbo.Tags", "TagName", c => c.String());
        }
    }
}
