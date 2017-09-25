namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcalcowner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalcConfigurations", "CalcOwner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalcConfigurations", "CalcOwner");
        }
    }
}
