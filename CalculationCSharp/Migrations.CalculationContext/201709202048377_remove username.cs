namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeusername : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CalcTeamMembers", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CalcTeamMembers", "UserName", c => c.String());
        }
    }
}
