namespace CalculationCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectUpdates", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectTasks", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectComments", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectStories", "ProjectColumns_ColumnId", "dbo.ProjectColumns");
            DropForeignKey("dbo.ProjectColumns", "ProjectBoard_BoardId", "dbo.ProjectBoards");
            DropIndex("dbo.ProjectUpdates", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectComments", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectStories", new[] { "ProjectColumns_ColumnId" });
            DropIndex("dbo.ProjectColumns", new[] { "ProjectBoard_BoardId" });
            DropTable("dbo.UserSessions");
            DropTable("dbo.Schemes");
            DropTable("dbo.ProjectBoards");
            DropTable("dbo.FileRepositories");
            DropTable("dbo.CalcReleases");
            DropTable("dbo.CalcRegressionInputs");
            DropTable("dbo.CalcHistories");
            DropTable("dbo.CalcConfigurations");
            CreateTable(
                "dbo.CalcConfigurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Scheme = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Version = c.Decimal(nullable: false, precision: 16, scale: 3),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcFunctions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Scheme = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Version = c.Decimal(nullable: false, precision: 16, scale: 3),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CalcID = c.Int(nullable: false),
                        Scheme = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                        Version = c.Decimal(nullable: false, precision: 16, scale: 3),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcRegressionInputs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CalcID = c.Int(nullable: false),
                        Scheme = c.String(),
                        Type = c.String(),
                        Reference = c.String(),
                        Input = c.String(storeType: "xml"),
                        Comment = c.String(),
                        OriginalRunDate = c.DateTime(),
                        LatestRunDate = c.DateTime(),
                        OutputOld = c.String(storeType: "xml"),
                        OutputNew = c.String(storeType: "xml"),
                        Difference = c.String(storeType: "xml"),
                        Pass = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CalcReleases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CalcID = c.Int(nullable: false),
                        Scheme = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                        Version = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FileRepositories",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        Description = c.String(),
                        FileSize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileID);
            
            CreateTable(
                "dbo.ProjectBoards",
                c => new
                    {
                        BoardId = c.Int(nullable: false, identity: true),
                        Client = c.String(),
                        Name = c.String(),
                        User = c.String(),
                        Configuration = c.String(storeType: "xml"),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BoardId);
            
            CreateTable(
                "dbo.ProjectColumns",
                c => new
                    {
                        ColumnId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        ProjectBoard_BoardId = c.Int(),
                    })
                .PrimaryKey(t => t.ColumnId)
                .ForeignKey("dbo.ProjectBoards", t => t.ProjectBoard_BoardId)
                .Index(t => t.ProjectBoard_BoardId);
            
            CreateTable(
                "dbo.ProjectStories",
                c => new
                    {
                        StoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Requested = c.String(),
                        Moscow = c.String(),
                        User = c.String(),
                        Timebox = c.String(),
                        AcceptanceCriteria = c.String(),
                        RAG = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        RequestedDate = c.DateTime(nullable: false),
                        SLADays = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ElapsedTime = c.String(),
                        Complexity = c.String(),
                        Effort = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        ProjectColumns_ColumnId = c.Int(),
                    })
                .PrimaryKey(t => t.StoryId)
                .ForeignKey("dbo.ProjectColumns", t => t.ProjectColumns_ColumnId)
                .Index(t => t.ProjectColumns_ColumnId);
            
            CreateTable(
                "dbo.ProjectComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentName = c.String(),
                        CommentType = c.String(),
                        CommentDateTime = c.DateTime(nullable: false),
                        CommentUser = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        ProjectStories_StoryId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.ProjectStories", t => t.ProjectStories_StoryId)
                .Index(t => t.ProjectStories_StoryId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        TaskUser = c.String(),
                        RemainingTime = c.String(),
                        Status = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        ProjectStories_StoryId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.ProjectStories", t => t.ProjectStories_StoryId)
                .Index(t => t.ProjectStories_StoryId);
            
            CreateTable(
                "dbo.ProjectUpdates",
                c => new
                    {
                        UpdateId = c.Int(nullable: false, identity: true),
                        UpdateField = c.String(),
                        UpdateValue = c.String(),
                        UpdateDateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        ProjectStories_StoryId = c.Int(),
                    })
                .PrimaryKey(t => t.UpdateId)
                .ForeignKey("dbo.ProjectStories", t => t.ProjectStories_StoryId)
                .Index(t => t.ProjectStories_StoryId);
            
            CreateTable(
                "dbo.Schemes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 4),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Section = c.String(),
                        Record = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUpdates", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectTasks", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectComments", "ProjectStories_StoryId", "dbo.ProjectStories");
            DropForeignKey("dbo.ProjectStories", "ProjectColumns_ColumnId", "dbo.ProjectColumns");
            DropForeignKey("dbo.ProjectColumns", "ProjectBoard_BoardId", "dbo.ProjectBoards");
            DropIndex("dbo.ProjectUpdates", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectComments", new[] { "ProjectStories_StoryId" });
            DropIndex("dbo.ProjectStories", new[] { "ProjectColumns_ColumnId" });
            DropIndex("dbo.ProjectColumns", new[] { "ProjectBoard_BoardId" });
            DropTable("dbo.UserSessions");
            DropTable("dbo.Schemes");
            DropTable("dbo.ProjectUpdates");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.ProjectComments");
            DropTable("dbo.ProjectStories");
            DropTable("dbo.ProjectColumns");
            DropTable("dbo.ProjectBoards");
            DropTable("dbo.FileRepositories");
            DropTable("dbo.CalcReleases");
            DropTable("dbo.CalcRegressionInputs");
            DropTable("dbo.CalcHistories");
            DropTable("dbo.CalcFunctions");
            DropTable("dbo.CalcConfigurations");
        }
    }
}
