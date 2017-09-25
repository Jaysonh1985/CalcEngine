namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTeams : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalcTeams", "TeamOwner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CalcTeams", "TeamOwner_Id");
            AddForeignKey("dbo.CalcTeams", "TeamOwner_Id", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalcTeams", "TeamOwner_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.CalcTeams", new[] { "TeamOwner_Id" });
            DropColumn("dbo.CalcTeams", "TeamOwner_Id");
        }
    }
}
