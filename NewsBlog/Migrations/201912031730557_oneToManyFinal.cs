namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oneToManyFinal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BlogItems", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogItems", "Category", c => c.String());
        }
    }
}
