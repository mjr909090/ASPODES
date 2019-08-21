namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingSubject",
                c => new
                    {
                        SubjectCode = c.String(nullable: false, maxLength: 30),
                        SubjectName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SubjectCode);
            
            CreateTable(
                "dbo.AnnualBudgetItem",
                c => new
                    {
                        AnnualBudgetItemId = c.Int(nullable: false, identity: true),
                        SubjectId = c.String(nullable: false, maxLength: 30),
                        Amount = c.Double(nullable: false),
                        Reason = c.String(maxLength: 1024),
                        AnnualBudgetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnualBudgetItemId)
                .ForeignKey("dbo.AnnualBudget", t => t.AnnualBudgetId, cascadeDelete: true)
                .ForeignKey("dbo.AccountingSubject", t => t.SubjectId)
                .Index(t => t.SubjectId)
                .Index(t => t.AnnualBudgetId);
            
            CreateTable(
                "dbo.AnnualBudget",
                c => new
                    {
                        AnnualBudgetId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        Amount = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnualBudgetId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        ProjectName = c.String(maxLength: 128),
                        Period = c.Int(),
                        ProjectTypeId = c.Int(nullable: false),
                        SupportCategoryId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        LeaderId = c.Int(nullable: false),
                        LeaderPhone = c.String(maxLength: 16),
                        LeaderEmail = c.String(maxLength: 64),
                        ContactId = c.Int(nullable: false),
                        ContactPhone = c.String(maxLength: 16),
                        ContactEmail = c.String(maxLength: 64),
                        TotalBudget = c.Double(),
                        FirstYearBudget = c.Double(),
                        YearCreated = c.Int(),
                        CurrentYear = c.Int(),
                        AppliyTimes = c.Int(),
                        Status = c.Int(nullable: false),
                        DeleageType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.Person", t => t.ContactId)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .ForeignKey("dbo.Person", t => t.LeaderId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId)
                .ForeignKey("dbo.SupportCategory", t => t.SupportCategoryId)
                .Index(t => t.ProjectTypeId)
                .Index(t => t.SupportCategoryId)
                .Index(t => t.InstituteId)
                .Index(t => t.LeaderId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        IDCard = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 64),
                        FirstName = c.String(maxLength: 64),
                        LastName = c.String(maxLength: 64),
                        EnglishName = c.String(maxLength: 64),
                        Birthday = c.DateTime(nullable: false),
                        Male = c.String(maxLength: 4),
                        Phone = c.String(maxLength: 32),
                        Email = c.String(maxLength: 256),
                        InstituteId = c.Int(nullable: false),
                        Duty = c.String(maxLength: 256),
                        Title = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Zip = c.String(maxLength: 32),
                        OfficePhone = c.String(maxLength: 32),
                        Status = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .Index(t => t.InstituteId);
            
            CreateTable(
                "dbo.Institute",
                c => new
                    {
                        InstituteId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        ShortName = c.String(maxLength: 256),
                        ParentId = c.Int(),
                        Type = c.Int(nullable: false),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.InstituteId);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        ProjectTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        Limit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.SupportCategory",
                c => new
                    {
                        SupportCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        ProjectTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupportCategoryId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.ApplicationDoc",
                c => new
                    {
                        ApplicationDocId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        RelativeURL = c.String(nullable: false, maxLength: 256),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.ApplicationDocId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ApplicationField",
                c => new
                    {
                        ApplicationFieldId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        SubFieldId = c.String(nullable: false, maxLength: 256),
                        KeyWordsCN = c.String(maxLength: 256),
                        KeyWordsEN = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ApplicationFieldId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.SubField", t => t.SubFieldId)
                .Index(t => t.ApplicationId)
                .Index(t => t.SubFieldId);
            
            CreateTable(
                "dbo.SubField",
                c => new
                    {
                        SubFieldName = c.String(nullable: false, maxLength: 256),
                        ParentName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.SubFieldName)
                .ForeignKey("dbo.Field", t => t.ParentName, cascadeDelete: true)
                .Index(t => t.ParentName);
            
            CreateTable(
                "dbo.Field",
                c => new
                    {
                        FieldName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.FieldName);
            
            CreateTable(
                "dbo.ApplicationLog",
                c => new
                    {
                        ApplicationLogId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        UserId = c.String(nullable: false, maxLength: 256),
                        Operation = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.ApplicationLogId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.ApplicationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 256),
                        Password = c.String(nullable: false, maxLength: 32),
                        IDCard = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 64),
                        SessionId = c.String(maxLength: 64),
                        InstituteId = c.Int(),
                        Login = c.Boolean(),
                        LastLogin = c.DateTime(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.InstituteId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Authorize",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Consultation",
                c => new
                    {
                        ConsultationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Score = c.Double(nullable: false),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        LeaderId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Budget = c.Double(nullable: false),
                        FirstYearBudget = c.Double(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        DelegateType = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                        Opinion = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.ConsultationId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .ForeignKey("dbo.Person", t => t.LeaderId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId)
                .Index(t => t.ApplicationId)
                .Index(t => t.LeaderId)
                .Index(t => t.InstituteId)
                .Index(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        EmailId = c.Int(nullable: false, identity: true),
                        SenderId = c.String(nullable: false, maxLength: 256),
                        ReceiverId = c.String(nullable: false, maxLength: 256),
                        ReciveAddress = c.String(nullable: false, maxLength: 256),
                        Content = c.String(nullable: false, maxLength: 256),
                        SendTime = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.EmailId)
                .ForeignKey("dbo.User", t => t.ReceiverId)
                .ForeignKey("dbo.User", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.ExpertField",
                c => new
                    {
                        ExpertFieldId = c.Int(nullable: false, identity: true),
                        SubFieldId = c.String(nullable: false, maxLength: 256),
                        ExpertInfoId = c.String(nullable: false, maxLength: 256),
                        KeyWordsCN = c.String(maxLength: 256),
                        KeyWordsEN = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ExpertFieldId)
                .ForeignKey("dbo.ExpertInfo", t => t.ExpertInfoId, cascadeDelete: true)
                .ForeignKey("dbo.SubField", t => t.SubFieldId, cascadeDelete: true)
                .Index(t => t.SubFieldId)
                .Index(t => t.ExpertInfoId);
            
            CreateTable(
                "dbo.ExpertInfo",
                c => new
                    {
                        ExpertInfoId = c.String(nullable: false, maxLength: 256),
                        IDCard = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 64),
                        EducationHistor = c.String(nullable: false, maxLength: 256),
                        WorkingHistory = c.String(nullable: false, maxLength: 256),
                        Achievements = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.ExpertInfoId);
            
            CreateTable(
                "dbo.InstAnnualBudget",
                c => new
                    {
                        InstAnnualBudgetId = c.Int(nullable: false, identity: true),
                        InstBudgetId = c.Int(),
                        Year = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.InstAnnualBudgetId)
                .ForeignKey("dbo.InstBudget", t => t.InstBudgetId)
                .Index(t => t.InstBudgetId);
            
            CreateTable(
                "dbo.InstBudget",
                c => new
                    {
                        InstBudgetId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        InstituteId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.InstBudgetId)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .Index(t => t.ApplicationId)
                .Index(t => t.InstituteId);
            
            CreateTable(
                "dbo.LoginLog",
                c => new
                    {
                        LoginLogId = c.String(nullable: false, maxLength: 64),
                        UserId = c.String(nullable: false, maxLength: 256),
                        Roles = c.String(maxLength: 64),
                        LoginIP = c.String(maxLength: 64),
                        LoginTime = c.DateTime(),
                        LastActiveTime = c.DateTime(),
                        IsLogout = c.Boolean(nullable: false),
                        TimeStamp = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.LoginLogId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        TableName = c.String(maxLength: 64),
                        EditRecordId = c.Int(),
                        Type = c.Int(nullable: false),
                        OldRecord = c.String(nullable: false, maxLength: 256),
                        NewRecord = c.String(nullable: false, maxLength: 256),
                        EditTime = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 256),
                        UserName = c.String(maxLength: 64),
                        IpAddress = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.LogId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        Task = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ApplicationId })
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.Notice",
                c => new
                    {
                        NoticeID = c.Int(nullable: false, identity: true),
                        SenderId = c.String(nullable: false, maxLength: 256),
                        Receiver = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 256),
                        SendTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NoticeID)
                .ForeignKey("dbo.User", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        ResourceId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ResourceId, t.RoleId })
                .ForeignKey("dbo.Resource", t => t.ResourceId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.ResourceId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        URL = c.String(nullable: false, maxLength: 256),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ResourceId);
            
            CreateTable(
                "dbo.ProjectDoc",
                c => new
                    {
                        ProjectDocId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        RelativeURL = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ProjectDocId)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        LeaderId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Budget = c.Double(nullable: false),
                        FirstYearBudget = c.Double(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        DelegateType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Application_ApplicationId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Application", t => t.Application_ApplicationId)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .ForeignKey("dbo.Person", t => t.LeaderId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId)
                .Index(t => t.LeaderId)
                .Index(t => t.InstituteId)
                .Index(t => t.ProjectTypeId)
                .Index(t => t.Application_ApplicationId);
            
            CreateTable(
                "dbo.ProjectMember",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Task = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.PersonId })
                .ForeignKey("dbo.Person", t => t.PersonId)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Recommendation",
                c => new
                    {
                        RecommendationId = c.Int(nullable: false, identity: true),
                        CandidateId = c.String(nullable: false, maxLength: 256),
                        RecommenderId = c.String(nullable: false, maxLength: 256),
                        Agree = c.Boolean(),
                        Adopt = c.Boolean(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecommendationId)
                .ForeignKey("dbo.User", t => t.CandidateId)
                .ForeignKey("dbo.User", t => t.RecommenderId)
                .Index(t => t.CandidateId)
                .Index(t => t.RecommenderId);
            
            CreateTable(
                "dbo.ReviewAssignment",
                c => new
                    {
                        ReviewAssignmentId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        ExpertId = c.String(nullable: false, maxLength: 256),
                        Accept = c.Boolean(),
                        PDFId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Overdue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewAssignmentId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.User", t => t.ExpertId)
                .ForeignKey("dbo.ApplicationDoc", t => t.PDFId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ExpertId)
                .Index(t => t.PDFId);
            
            CreateTable(
                "dbo.ReviewComment",
                c => new
                    {
                        ReviewCommentId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        ExpertId = c.String(nullable: false, maxLength: 256),
                        Comment = c.String(maxLength: 1024),
                        Level = c.String(maxLength: 8),
                        Score = c.Int(),
                        Status = c.Int(nullable: false),
                        ReviewAssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewCommentId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.User", t => t.ExpertId)
                .ForeignKey("dbo.ReviewAssignment", t => t.ReviewAssignmentId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ExpertId)
                .Index(t => t.ReviewAssignmentId);
            
            CreateTable(
                "dbo.Sms",
                c => new
                    {
                        SmsId = c.Int(nullable: false, identity: true),
                        SmsType = c.String(maxLength: 128),
                        Phone = c.String(nullable: false, maxLength: 20),
                        SmsContent = c.String(nullable: false, maxLength: 250),
                        SendTime = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 32),
                        UserId = c.String(nullable: false, maxLength: 256),
                        UserName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.SmsId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SysSetting",
                c => new
                    {
                        SetId = c.Int(nullable: false, identity: true),
                        SetName = c.String(nullable: false, maxLength: 50),
                        SetValue = c.String(nullable: false, maxLength: 150),
                        Remark = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.SetId);
            
            CreateTable(
                "dbo.TemplateDoc",
                c => new
                    {
                        DocId = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 256),
                        Name = c.String(nullable: false, maxLength: 256),
                        RelativeURL = c.String(nullable: false, maxLength: 256),
                        Remark = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.DocId);
            
            CreateTable(
                "dbo.TemplateReason",
                c => new
                    {
                        ReasonId = c.Int(nullable: false, identity: true),
                        EditorId = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ReasonId)
                .ForeignKey("dbo.User", t => t.EditorId)
                .Index(t => t.EditorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemplateReason", "EditorId", "dbo.User");
            DropForeignKey("dbo.Sms", "UserId", "dbo.User");
            DropForeignKey("dbo.ReviewComment", "ReviewAssignmentId", "dbo.ReviewAssignment");
            DropForeignKey("dbo.ReviewComment", "ExpertId", "dbo.User");
            DropForeignKey("dbo.ReviewComment", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ReviewAssignment", "PDFId", "dbo.ApplicationDoc");
            DropForeignKey("dbo.ReviewAssignment", "ExpertId", "dbo.User");
            DropForeignKey("dbo.ReviewAssignment", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Recommendation", "RecommenderId", "dbo.User");
            DropForeignKey("dbo.Recommendation", "CandidateId", "dbo.User");
            DropForeignKey("dbo.ProjectMember", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectMember", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ProjectDoc", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Project", "LeaderId", "dbo.Person");
            DropForeignKey("dbo.Project", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.Project", "Application_ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Permission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Permission", "ResourceId", "dbo.Resource");
            DropForeignKey("dbo.Notice", "SenderId", "dbo.User");
            DropForeignKey("dbo.Member", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Member", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Log", "UserId", "dbo.User");
            DropForeignKey("dbo.LoginLog", "UserId", "dbo.User");
            DropForeignKey("dbo.InstAnnualBudget", "InstBudgetId", "dbo.InstBudget");
            DropForeignKey("dbo.InstBudget", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.InstBudget", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ExpertField", "SubFieldId", "dbo.SubField");
            DropForeignKey("dbo.ExpertField", "ExpertInfoId", "dbo.ExpertInfo");
            DropForeignKey("dbo.Email", "SenderId", "dbo.User");
            DropForeignKey("dbo.Email", "ReceiverId", "dbo.User");
            DropForeignKey("dbo.Consultation", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Consultation", "LeaderId", "dbo.Person");
            DropForeignKey("dbo.Consultation", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.Consultation", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Authorize", "UserId", "dbo.User");
            DropForeignKey("dbo.Authorize", "RoleId", "dbo.Role");
            DropForeignKey("dbo.ApplicationLog", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "PersonId", "dbo.Person");
            DropForeignKey("dbo.User", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.ApplicationLog", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationField", "SubFieldId", "dbo.SubField");
            DropForeignKey("dbo.SubField", "ParentName", "dbo.Field");
            DropForeignKey("dbo.ApplicationField", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationDoc", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.AnnualBudgetItem", "SubjectId", "dbo.AccountingSubject");
            DropForeignKey("dbo.AnnualBudgetItem", "AnnualBudgetId", "dbo.AnnualBudget");
            DropForeignKey("dbo.AnnualBudget", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Application", "SupportCategoryId", "dbo.SupportCategory");
            DropForeignKey("dbo.SupportCategory", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Application", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Application", "LeaderId", "dbo.Person");
            DropForeignKey("dbo.Application", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.Application", "ContactId", "dbo.Person");
            DropForeignKey("dbo.Person", "InstituteId", "dbo.Institute");
            DropIndex("dbo.TemplateReason", new[] { "EditorId" });
            DropIndex("dbo.Sms", new[] { "UserId" });
            DropIndex("dbo.ReviewComment", new[] { "ReviewAssignmentId" });
            DropIndex("dbo.ReviewComment", new[] { "ExpertId" });
            DropIndex("dbo.ReviewComment", new[] { "ApplicationId" });
            DropIndex("dbo.ReviewAssignment", new[] { "PDFId" });
            DropIndex("dbo.ReviewAssignment", new[] { "ExpertId" });
            DropIndex("dbo.ReviewAssignment", new[] { "ApplicationId" });
            DropIndex("dbo.Recommendation", new[] { "RecommenderId" });
            DropIndex("dbo.Recommendation", new[] { "CandidateId" });
            DropIndex("dbo.ProjectMember", new[] { "PersonId" });
            DropIndex("dbo.ProjectMember", new[] { "ProjectId" });
            DropIndex("dbo.Project", new[] { "Application_ApplicationId" });
            DropIndex("dbo.Project", new[] { "ProjectTypeId" });
            DropIndex("dbo.Project", new[] { "InstituteId" });
            DropIndex("dbo.Project", new[] { "LeaderId" });
            DropIndex("dbo.ProjectDoc", new[] { "ProjectId" });
            DropIndex("dbo.Permission", new[] { "RoleId" });
            DropIndex("dbo.Permission", new[] { "ResourceId" });
            DropIndex("dbo.Notice", new[] { "SenderId" });
            DropIndex("dbo.Member", new[] { "ApplicationId" });
            DropIndex("dbo.Member", new[] { "PersonId" });
            DropIndex("dbo.Log", new[] { "UserId" });
            DropIndex("dbo.LoginLog", new[] { "UserId" });
            DropIndex("dbo.InstBudget", new[] { "InstituteId" });
            DropIndex("dbo.InstBudget", new[] { "ApplicationId" });
            DropIndex("dbo.InstAnnualBudget", new[] { "InstBudgetId" });
            DropIndex("dbo.ExpertField", new[] { "ExpertInfoId" });
            DropIndex("dbo.ExpertField", new[] { "SubFieldId" });
            DropIndex("dbo.Email", new[] { "ReceiverId" });
            DropIndex("dbo.Email", new[] { "SenderId" });
            DropIndex("dbo.Consultation", new[] { "ProjectTypeId" });
            DropIndex("dbo.Consultation", new[] { "InstituteId" });
            DropIndex("dbo.Consultation", new[] { "LeaderId" });
            DropIndex("dbo.Consultation", new[] { "ApplicationId" });
            DropIndex("dbo.Authorize", new[] { "UserId" });
            DropIndex("dbo.Authorize", new[] { "RoleId" });
            DropIndex("dbo.User", new[] { "PersonId" });
            DropIndex("dbo.User", new[] { "InstituteId" });
            DropIndex("dbo.ApplicationLog", new[] { "UserId" });
            DropIndex("dbo.ApplicationLog", new[] { "ApplicationId" });
            DropIndex("dbo.SubField", new[] { "ParentName" });
            DropIndex("dbo.ApplicationField", new[] { "SubFieldId" });
            DropIndex("dbo.ApplicationField", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationDoc", new[] { "ApplicationId" });
            DropIndex("dbo.SupportCategory", new[] { "ProjectTypeId" });
            DropIndex("dbo.SupportCategory", new[] { "Name" });
            DropIndex("dbo.Person", new[] { "InstituteId" });
            DropIndex("dbo.Application", new[] { "ContactId" });
            DropIndex("dbo.Application", new[] { "LeaderId" });
            DropIndex("dbo.Application", new[] { "InstituteId" });
            DropIndex("dbo.Application", new[] { "SupportCategoryId" });
            DropIndex("dbo.Application", new[] { "ProjectTypeId" });
            DropIndex("dbo.AnnualBudget", new[] { "ApplicationId" });
            DropIndex("dbo.AnnualBudgetItem", new[] { "AnnualBudgetId" });
            DropIndex("dbo.AnnualBudgetItem", new[] { "SubjectId" });
            DropTable("dbo.TemplateReason");
            DropTable("dbo.TemplateDoc");
            DropTable("dbo.SysSetting");
            DropTable("dbo.Sms");
            DropTable("dbo.ReviewComment");
            DropTable("dbo.ReviewAssignment");
            DropTable("dbo.Recommendation");
            DropTable("dbo.ProjectMember");
            DropTable("dbo.Project");
            DropTable("dbo.ProjectDoc");
            DropTable("dbo.Resource");
            DropTable("dbo.Permission");
            DropTable("dbo.Notice");
            DropTable("dbo.Member");
            DropTable("dbo.Log");
            DropTable("dbo.LoginLog");
            DropTable("dbo.InstBudget");
            DropTable("dbo.InstAnnualBudget");
            DropTable("dbo.ExpertInfo");
            DropTable("dbo.ExpertField");
            DropTable("dbo.Email");
            DropTable("dbo.Consultation");
            DropTable("dbo.Role");
            DropTable("dbo.Authorize");
            DropTable("dbo.User");
            DropTable("dbo.ApplicationLog");
            DropTable("dbo.Field");
            DropTable("dbo.SubField");
            DropTable("dbo.ApplicationField");
            DropTable("dbo.ApplicationDoc");
            DropTable("dbo.SupportCategory");
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.Institute");
            DropTable("dbo.Person");
            DropTable("dbo.Application");
            DropTable("dbo.AnnualBudget");
            DropTable("dbo.AnnualBudgetItem");
            DropTable("dbo.AccountingSubject");
        }
    }
}
