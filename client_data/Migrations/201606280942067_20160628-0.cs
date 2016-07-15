namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606280 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocName = c.String(),
                        RequestDeposit_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestDeposits", t => t.RequestDeposit_Id)
                .Index(t => t.RequestDeposit_Id);
            
            CreateTable(
                "dbo.RequestDeposits",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OutNumber = c.String(),
                        OutDate = c.DateTime(nullable: false),
                        DuNumDate = c.String(),
                        ClientId = c.Int(nullable: false),
                        PortfolioId = c.Int(nullable: false),
                        RubricaOut = c.Int(nullable: false),
                        KoId = c.Int(nullable: false),
                        FilialId = c.Int(),
                        ValueTypes = c.Int(nullable: false),
                        DepositSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositCurrency = c.Int(nullable: false),
                        SettlementCurrency = c.Int(nullable: false),
                        BalanceMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositDogNum = c.String(),
                        DepositDogDate = c.DateTime(nullable: false),
                        AgreementContractNum = c.String(),
                        AgreementContractDate = c.DateTime(),
                        ChangesDogDate = c.DateTime(),
                        TerminationDogDate = c.DateTime(),
                        ContributionType = c.Int(nullable: false),
                        DepositDogDateEnd = c.DateTime(nullable: false),
                        DepositAccount = c.String(),
                        TransferDateEnd = c.DateTime(),
                        ContributionDogType = c.Int(nullable: false),
                        RateValue = c.Int(nullable: false),
                        PeriodPayment = c.Int(nullable: false),
                        PeriodsInterestDate = c.DateTime(nullable: false),
                        DepositSubordinated = c.Boolean(nullable: false),
                        AccountReturn = c.Int(nullable: false),
                        KoAccountOpen = c.Int(),
                        ExistenceContractConditions = c.Boolean(nullable: false),
                        NoExistenceContractConditions = c.Boolean(nullable: false),
                        AuthorizedPersonFIO = c.String(),
                        AuthorizedPersonPost = c.String(),
                        RequestStatus = c.Boolean(),
                        RequestNum = c.String(),
                        RequestDate = c.DateTime(),
                        RequestDescription = c.String(),
                        AppUserId = c.String(),
                        StatusObrobotki = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PercentPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PercentRate = c.Int(nullable: false),
                        RequestDeposit_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestDeposits", t => t.RequestDeposit_Id)
                .Index(t => t.RequestDeposit_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientFansyId = c.Int(),
                        JoinDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PercentPeriods", "RequestDeposit_Id", "dbo.RequestDeposits");
            DropForeignKey("dbo.DocumentCollections", "RequestDeposit_Id", "dbo.RequestDeposits");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PercentPeriods", new[] { "RequestDeposit_Id" });
            DropIndex("dbo.DocumentCollections", new[] { "RequestDeposit_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PercentPeriods");
            DropTable("dbo.RequestDeposits");
            DropTable("dbo.DocumentCollections");
        }
    }
}
