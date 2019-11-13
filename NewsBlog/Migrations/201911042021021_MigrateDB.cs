namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogItems",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.NewsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BlogItems");
        }
    }
}
