namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNametoCalcTeamMembers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalcTeamMembers", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalcTeamMembers", "UserName");
        }
    }
}
