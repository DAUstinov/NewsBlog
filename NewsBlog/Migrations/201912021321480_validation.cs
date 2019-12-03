namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogItems", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.BlogItems", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogItems", "Description", c => c.String());
            AlterColumn("dbo.BlogItems", "Name", c => c.String());
        }
    }
}
