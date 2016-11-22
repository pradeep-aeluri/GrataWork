namespace GrataWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripeCustomerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AccountBalance", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "StripeCustomerId", c => c.String());
            AddColumn("dbo.AspNetUsers", "JIRAEpicId", c => c.String());
            AddColumn("dbo.AspNetUsers", "StripePlanId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "StripePlanId");
            DropColumn("dbo.AspNetUsers", "JIRAEpicId");
            DropColumn("dbo.AspNetUsers", "StripeCustomerId");
            DropColumn("dbo.AspNetUsers", "AccountBalance");
        }
    }
}
