namespace ASPODES.Database.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPODES.Database.AspodesDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ASPODES.Database.AspodesDB";
        }

        protected override void Seed(ASPODES.Database.AspodesDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            // ��֪ͨģ�������������
            context.NoticeTemplates.AddOrUpdate(
              p => p.Id,
              //new NoticeTemplate { Id = 1, Content = "����һ�����������ύ�����ȴ���λ����Ա���", NoticeType = NoticeTypes.Success, URL = "applicationSubmit" , ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 2, Content = "����һ���������ѱ���λ����Ա���ͨ�������ڵȴ�Ժ����Ա����", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 3, Content = "����������δͨ����λ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "applicationSaved", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 4, Content = "����һ���������ѱ���λ����Ա���أ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "applicationSaved", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 5, Content = "����һ��������δͨ��Ժ����Ա����", NoticeType = NoticeTypes.Error, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 6, Content = "����һ���������ѱ�Ժ����Ա����", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 7, Content = "����һ���������ѱ��ɹ�ָ��ר��", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 8, Content = "����һ���������Ѿ��������", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 9, Content = "����Number����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "divisionApplication", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 10, Content = "����λ��Number��������δͨ��Ժ����Ա���", NoticeType = NoticeTypes.Error, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 11, Content = "����λ��Number����������ͨ��Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 12, Content = "�����ڵĵ�λ���������ѱ��ɹ�ָ��ר��", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 13, Content = "�����ڵĵ�λ���������Ѿ��������", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 14, Content = "����Number����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "applicationUnhandled", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 15, Content = "DepartmentName��λ�ύ��ApplicationName�ѱ���λ����Ա����", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 16, Content = "����Number��������ȴ��ֶ�ָ��ר�ң��뾡��ָ��", NoticeType = NoticeTypes.Warning, URL = "assignmentRecommendation", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 17, Content = "ȫ����������ָ��ר�ң��뾡��ȷ��", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 18, Content = "ȫ����������ָ��ר�ң��뾡��ȷ��", NoticeType = NoticeTypes.Warning, URL = "assignmentRecommendation", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 19, Content = "����Number����������������ϣ������ڿ��Ե���ר������������ѯ����ģ��", NoticeType = NoticeTypes.Warning, URL = "firstReviewResult", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 20, Content = "����", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 21, Content = "����Number����������Ҫ�����뾡������", NoticeType = NoticeTypes.Warning, URL = "preReview", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 22, Content = "����ApplicationName���ձ���Ϊ��������", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 23, Content = "����ApplicationName���ձ���Ϊ����,����������Ŀ����ǰ����Ŀҳ����в鿴", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 24, Content = "����ApplicationName���ձ���Ϊ���" , NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 25, Content = "�����µ����������д���뾡����д", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 26, Content = "����һ�����������ύ�����ڵȴ���λ����Ա���", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 27, Content = "����һ���������ѱ���λ����Ա���ͨ�������ڵȴ�Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 28, Content = "����������δͨ����λ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 29, Content = "����һ���������ѱ�Ժ����Ա���ͨ��", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 30, Content = "����������δͨ��Ժ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 31, Content = "������Ŀ���ύ�������ȱ���", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 32, Content = "���������ȱ������ύ�����ڵȴ���λ����Ա���", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 33, Content = "���������ȱ����ѱ���λ����Ա���ͨ�������ڵȴ�Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 34, Content = "���������ȱ���δͨ����λ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 35, Content = "���������ȱ����ѱ�Ժ����Ա���ͨ��", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 36, Content = "���������ȱ���δͨ��Ժ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 37, Content = "������Ŀ���ύ���ⱨ��", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 38, Content = "���Ľ��ⱨ�����ύ�����ڵȴ���λ����Ա���", NoticeType =NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 39, Content = "���ύ��ApplicationName�Ľ��������ѱ���λ����Ա���ͨ��", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 40, Content = "��Ľ�������δͨ����λ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 41, Content = "���ύ��ApplicationName�Ľ��������ѱ�Ժ����Ա���ͨ��", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 42, Content = "���Ľ�������δͨ��Ժ����Ա��ˣ��뾡���޸ĺ������ύ", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 43, Content = "����������������׶��Ѿ���������������Ϣ�鿴����", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 44, Content = "����������������׶��Ѿ���������������Ϣ�鿴����", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 45, Content = "����������������׶��Ѿ���������������Ϣ�鿴����", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 46, Content = "����Number����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "auditAssignBook", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 47, Content = "����λ��Number����������ͨ��Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 48, Content = "����λ��Number��������δͨ��Ժ����Ա���", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 49, Content = "����Number����ȱ�����Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "auditAnnualSummary", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 50, Content = "����λ��Number����ȱ�����ͨ��Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 51, Content = "����λ��Number����ȱ���δͨ��Ժ����Ա���", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 52, Content = "����Number�����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "auditConcludeReport", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 53, Content = "����λ��Number�����������ͨ��Ժ����Ա���", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 54, Content = "����λ��Number���������δͨ��Ժ����Ա���", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 55, Content = "����Number����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditAssignBook", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 56, Content = "����Number����ȱ�����Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditAnnualSummary", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 57, Content = "����Number�����������Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditConcludeReport", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 101, Content = "�����ڵ�λ�ĵ�λ����Ա�Ѿ��޸������ĸ�����Ϣ���뾡��鿴���˶�", NoticeType = NoticeTypes.Warning, URL = "personal_setting", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 102, Content = "�����ڵ�λ�ĵ�λ����Ա�Ѿ����������ĵ�¼����", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 103, Content = "���ѱ������ڵ�λ�ĵ�λ����Ա�Ƽ���Ϊר��", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 104, Content = "�����ڵ�λ�ĵ�λ����Ա�Ѿ�����������ר���Ƽ�", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 105, Content = "Ժ����Ա�Ѿ�ͨ���˶�����ר���Ƽ��������ڿ�����ר�ҵ���ݵ�¼ϵͳ", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 106, Content = "Ժ����Ա�Ѿ�ȡ��������ר���ʸ������ڽ��޷�ִ��ר����ص�����", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 107, Content = "���ѱ�ϵͳ����Ա����ΪԺ����Ա", NoticeType = NoticeTypes.Normal, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 108, Content = "���ѱ�ϵͳ����Ա����ΪԺ����Ա", NoticeType = NoticeTypes.Normal, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 109, Content = "���ѱ�ϵͳ����Աȡ��Ժ����Ա�ʸ�", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 110, Content = "ϵͳ����Ա������������Ŀ�ֹ�����", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 111, Content = "���ѱ�ϵͳ����Ա����Ϊ��λ����Ա", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 112, Content = "���ѱ�ϵͳ����Աȡ����λ����Ա�ʸ�", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 113, Content = "����UserName��ר���Ƽ��ѱ�Ժ����Ա����", NoticeType = NoticeTypes.Error, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 114, Content = "����UserName��ר���Ƽ��ѱ�Ժ����Աͨ��", NoticeType = NoticeTypes.Success, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 115, Content = "�����ڵ�λ��UserName�ѱ�Ժ����Աȡ����ר���ʸ�", NoticeType = NoticeTypes.Error, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 116, Content = "�����ڵ�λ����ϵ���ѱ��滻ΪUserName", NoticeType = NoticeTypes.Warning, URL = "infoMaintain", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 117, Content = "ϵͳ����Ա�޸��������ڵ�λ�ĵ�λ��Ϣ���뼰ʱ���к˶�", NoticeType = NoticeTypes.Warning, URL = "infoMaintain", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 118, Content = "DepartmentName��λ��UserName�����һ���µ�Ժ�ⵥλ����NewDepartmentName����λ����", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 119, Content = "DepartmentName1��λ��UserName1ΪDepartmentName2��λ�����һ���µ���Ա����UserName2", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 120, Content = "����Number���µ�ר���Ƽ���Ҫ��ˣ��뾡�����", NoticeType = NoticeTypes.Warning, URL = "exportRecommendAudited", ReceiverType = ReceiverType.DeptManager }
              //new NoticeTemplate { Id = 121, Content = "DepartmentName��λ�ύ�Ķ�UserName��ר���Ƽ��ѱ���λ����Ա����", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager }
              );
            //
        }
    }
}
