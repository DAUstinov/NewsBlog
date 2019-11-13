namespace NewsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Picture = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            //DropTable("dbo.Pictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Images");
        }
    }
}
