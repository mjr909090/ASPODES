using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Http;
using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using ASPODES.WebAPI.Security;

namespace ASPODES.WebAPI.Repository
{
    public class ApplicationFieldRepository
    {
        /// <summary>
        /// 获取申请书的领域列表
        /// </summary>
        /// <param name="applicationId">申请书文档</param>
        /// <returns>
        /// 成功返回ResponseStatus.success和申请书领域列表
        /// 没有找到数据返回ResponseStatus.parameter_error,
        /// 出错返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public List<GetApplicationFieldDTO> GetApplicationFields(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                var fields = ctx.ApplicationFields
                    .Where(af => af.ApplicationId == applicationId)
                    .Select(Mapper.Map<GetApplicationFieldDTO>)
                    .ToList();
                return fields;
            }
        }

        /// <summary>
        /// 添加或者修改申请书领域
        /// </summary>
        /// <param name="fieldDTO">申请书领域信息</param>
        /// <returns></returns>
        public List<ApplicationField> AddOrUpdateApplicationField(List<AddApplicationFieldDTO> fieldDTOs, Func<Application, CurrentUser, bool> privilege)
        {
            var userInfo = UserHelper.GetCurrentUser();
            List<ApplicationField> fields = new List<ApplicationField>();

            using (var ctx = new AspodesDB())
            {
                foreach (var fieldDTO in fieldDTOs)
                {
                    var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == fieldDTO.ApplicationId);
                    if (null == application) throw new NotFoundException("未找到申请书");

                    if (!privilege(application, userInfo) /**/&& false/**/) throw new UnauthorizationException();//2018-03-15 取消填申请书草稿的年限验证

                    ApplicationField newField = Mapper.Map<ApplicationField>(fieldDTO);
                    var oldField = ctx.ApplicationFields.FirstOrDefault(ef => ef.ApplicationFieldId == newField.ApplicationFieldId);
                    if (null == oldField)
                    {
                        if (ctx.ApplicationFields.Where(af => af.ApplicationId == newField.ApplicationId).Count() >= SystemConfig.ApplicationFieldAmount)
                            throw new OtherException("申请书领域数目超过限制");
                        newField = ctx.ApplicationFields.Add(newField);
                    }
                    else
                    {
                        //不允许修改的属性
                        newField.ApplicationId = oldField.ApplicationId;
                        ctx.Entry(oldField).CurrentValues.SetValues(newField);
                    }
                    fields.Add(newField);
                }
                ctx.SaveChanges();
                return fields;
            }
        }

        /// <summary>
        /// 删除申请书领域
        /// </summary>
        /// <param name="applicationFieldId">申请书领域ID</param>
        /// <returns></returns>
        public void DeleteApplicationField(int applicationFieldId, Func<Application, CurrentUser, bool> privilege)
        {
            var userInfo = UserHelper.GetCurrentUser();
            using (var ctx = new AspodesDB())
            {
                var field = ctx.ApplicationFields.FirstOrDefault(af => af.ApplicationFieldId == applicationFieldId);
                if (null == field) throw new NotFoundException("未找到申请书领域");
                if (!privilege(field.Application, userInfo)) throw new UnauthorizationException();
                ctx.ApplicationFields.Remove(field);
                ctx.SaveChanges();
            }
        }
    }
}