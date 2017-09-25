namespace CalculationCSharp.Migrations.CalculationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class calcteamsv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.CalcTeamMembers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.CalcTeams", "TeamOwner_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropIndex("dbo.CalcTeamMembers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.CalcTeams", new[] { "TeamOwner_Id" });
            AddColumn("dbo.CalcTeamMembers", "UserId", c => c.String());
            AddColumn("dbo.CalcTeams", "TeamOwner", c => c.String());
            DropColumn("dbo.CalcTeamMembers", "ApplicationUser_Id");
            DropColumn("dbo.CalcTeams", "TeamOwner_Id");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId });
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Scheme = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CalcTeams", "TeamOwner_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.CalcTeamMembers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.CalcTeams", "TeamOwner");
            DropColumn("dbo.CalcTeamMembers", "UserId");
            CreateIndex("dbo.CalcTeams", "TeamOwner_Id");
            CreateIndex("dbo.IdentityUserRoles", "IdentityRole_Id");
            CreateIndex("dbo.IdentityUserRoles", "ApplicationUser_Id");
            CreateIndex("dbo.IdentityUserLogins", "ApplicationUser_Id");
            CreateIndex("dbo.IdentityUserClaims", "ApplicationUser_Id");
            CreateIndex("dbo.CalcTeamMembers", "ApplicationUser_Id");
            AddForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles", "Id");
            AddForeignKey("dbo.CalcTeams", "TeamOwner_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.CalcTeamMembers", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
        }
    }
}
