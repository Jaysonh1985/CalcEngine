namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkCalcTeamstoCalcConfiguratiosn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalcConfigurations", "CalcTeams_CalcTeamID", c => c.Int());
            CreateIndex("dbo.CalcConfigurations", "CalcTeams_CalcTeamID");
            AddForeignKey("dbo.CalcConfigurations", "CalcTeams_CalcTeamID", "dbo.CalcTeams", "CalcTeamID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalcConfigurations", "CalcTeams_CalcTeamID", "dbo.CalcTeams");
            DropIndex("dbo.CalcConfigurations", new[] { "CalcTeams_CalcTeamID" });
            DropColumn("dbo.CalcConfigurations", "CalcTeams_CalcTeamID");
        }
    }
}
