namespace CalculationCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalcConfigurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Scheme = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Version = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Scheme = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                        Version = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcReleases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Scheme = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Version = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalculationRegressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Scheme = c.String(),
                        Type = c.String(),
                        OriginalRunDate = c.DateTime(nullable: false),
                        LatestRunDate = c.DateTime(nullable: false),
                        Reference = c.String(),
                        Input = c.String(storeType: "xml"),
                        OutputOld = c.String(storeType: "xml"),
                        OutputNew = c.String(storeType: "xml"),
                        Difference = c.String(storeType: "xml"),
                        Pass = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CalculationResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(nullable: false, maxLength: 100),
                        Scheme = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 100),
                        RunDate = c.DateTime(nullable: false),
                        Reference = c.String(),
                        Input = c.String(storeType: "xml"),
                        Output = c.String(storeType: "xml"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Codes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        Code = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectBoards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectBoards");
            DropTable("dbo.Codes");
            DropTable("dbo.CalculationResults");
            DropTable("dbo.CalculationRegressions");
            DropTable("dbo.CalcReleases");
            DropTable("dbo.CalcHistories");
            DropTable("dbo.CalcConfigurations");
        }
    }
}
