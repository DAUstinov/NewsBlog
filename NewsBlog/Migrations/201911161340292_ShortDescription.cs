namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShortDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogItems", "ShortDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogItems", "ShortDescription");
        }
    }
}
