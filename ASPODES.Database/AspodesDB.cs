using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;


namespace ASPODES.Database
{

    public class AspodesDB : DbContext
    {
        //申请书及相关实体
        public virtual DbSet<AnnualBudget> AnnualBudgets { get; set; }
        public virtual DbSet<AnnualBudgetItem> AnnualBudgetItems { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationDoc> ApplicationDocs { get; set; }
        public virtual DbSet<ApplicationField> ApplicationFields { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<InstAnnualBudget> InstAnnualBudgets { get; set; }
        public virtual DbSet<InstBudget> InstBudgets { get; set; }
        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<AccountingSubject> AccountingSubjects { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<SubField> SubFields { get; set; }
        public virtual DbSet<SupportCategory> SupportCategorys { get; set; }
        public virtual DbSet<ProjectType> ProjectTypes { get; set; }

        public virtual DbSet<ExpertField> ExpertFields { get; set; }
        public virtual DbSet<Institute> Institutes { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<User> Users { get; set; }


        public virtual DbSet<Consultation> Consultations { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDoc> ProjectDocs { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectLog> ProjectLogs { get; set; }

        //任务书相关实体
        public virtual DbSet<AnnualTask> AnnualTasks { get; set; }
        public virtual DbSet<AnnualTaskBudgetItem> AnnualTaskBudgetItems { get; set; }
        public virtual DbSet<AnnualTaskDoc> AnnualTaskDocs { get; set; }
        public virtual DbSet<AnnualTaskInstBudget> AnnualTaskInstBudgets { get; set; }

        public virtual DbSet<Recommendation> Recommendations { get; set; }
        public virtual DbSet<ReviewAssignment> ReviewAssignments { get; set; }
        public virtual DbSet<ReviewComment> ReviwerComments { get; set; }


        public virtual DbSet<Authorize> Authorizes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ApplicationAssignment> ApplicationAssignments { get; set; }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<NoticeTemplate> NoticeTemplates { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<AnnouncementAttachment> AnnouncementAttachments { get; set; }
        public virtual DbSet<Sms> Sms { get; set; }
        public virtual DbSet<SysSetting> SysSettings { get; set; }

        public virtual DbSet<SysSettingHistory> SysSettingHistorys { get; set; }

        public virtual DbSet<TemplateDoc> TemplateDocs { get; set; }
        public virtual DbSet<TemplateReason> TemplateReasons { get; set; }

        public virtual DbSet<LoginLog> LoginLogs { get; set; }

        public virtual DbSet<VisitLog> VisitLogs { get; set; }

        public AspodesDB(): base("name=AspodesDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ApplicationMap());
            builder.Configurations.Add(new AnnualBudgetMap());
            builder.Configurations.Add(new ApplicationDocMap());
            builder.Configurations.Add(new AnnualBudgetItemMap());
            builder.Configurations.Add(new ApplicationFieldMap());
            builder.Configurations.Add(new ApplicationLogMap());
            builder.Configurations.Add(new InstAnnualBudgetMap());
            builder.Configurations.Add(new InstBudgetMap());
            builder.Configurations.Add(new MemberMap());

            builder.Configurations.Add(new AccountingSubjectMap());
            builder.Configurations.Add(new FieldMap());
            builder.Configurations.Add(new SubFieldMap());
            builder.Configurations.Add(new SupportCategoryMap());

            builder.Configurations.Add(new ExpertFieldMap());
            builder.Configurations.Add(new InstituteMap());
            builder.Configurations.Add(new PersonMap());
            builder.Configurations.Add(new UserMap());

            //builder.Configurations.Add(new ConsultationMap());
            builder.Configurations.Add(new ProjectMap());
            builder.Configurations.Add(new ProjectDocMap());
            builder.Configurations.Add(new ProjectMemberMap());
            builder.Configurations.Add(new ProjectLogMap());

            builder.Configurations.Add(new ApplicationConsultationMap());
            builder.Configurations.Add(new ProjectConsultationMap());

            builder.Configurations.Add(new AnnualTaskMap());
            builder.Configurations.Add(new AnnualTaskDocMap());
            builder.Configurations.Add(new AnnualTaskBudgetItemMap());
            builder.Configurations.Add(new AnnualTaskInstBudgetMap());

            builder.Configurations.Add(new RecommendationMap());
            builder.Configurations.Add(new ReviewAssignmentMap());
            builder.Configurations.Add(new ReviewCommentMap());


            builder.Configurations.Add(new AuthorizeMap());
            builder.Configurations.Add(new PermissionMap());
            builder.Configurations.Add(new ResourceMap());
            builder.Configurations.Add(new RoleMap());
            builder.Configurations.Add(new ApplicationAssignmentMap());

            builder.Configurations.Add(new EmailMap());
            builder.Configurations.Add(new LogMap());
            builder.Configurations.Add(new NoticeMap());
            builder.Configurations.Add(new NoticeTemplateMap());
            builder.Configurations.Add(new AnnouncementMap());
            builder.Configurations.Add(new AnnouncementAttachmentMap());
            builder.Configurations.Add(new SmsMap());
            builder.Configurations.Add(new SysSettingMap());
            builder.Configurations.Add(new SysSettingHistoryMap());
            builder.Configurations.Add(new TemplateDocMap());
            builder.Configurations.Add(new TemplateReasonMap());
            builder.Configurations.Add(new LoginLogMap());
            builder.Configurations.Add(new VisitLogMap());
        }
    }
}
