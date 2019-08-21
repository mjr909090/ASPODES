using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Database;
using AutoMapper;
using System.Web.Http;
using ASPODES.WebAPI.Common;
using System.Net.Http;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.Model;
using ASPODES.WebAPI.Authorize;

namespace ASPODES.WebAPI.Repository
{
    [AspodesAuthorize]
    public class ExpertFieldRepository
    {
        /// <summary>
        /// 获取专家的研究领域
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        public List<GetExpertFieldDTO> GetExpertField(int personId)
        {
            using (var ctx = new AspodesDB())
            {
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                if (user == null) throw new NotFoundException();
                var fieldDTOs = ctx.ExpertFields
                    .Where(ef => ef.UserId == user.UserId)
                    .Select(Mapper.Map<GetExpertFieldDTO>)
                    .ToList();
                return fieldDTOs;
            }
        }

        /// <summary>
        /// 通过用户ID获取专家的研究领域
        /// </summary>
        /// <param name="userId">专家（用户）的人员ID</param>
        /// <returns></returns>
        public List<GetExpertFieldDTO> GetExpertFieldByUserId(string userId)
        {
            using (var ctx = new AspodesDB())
            {
                int personId = ctx.Users.Find(userId).PersonId.Value;
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                if (user == null) throw new NotFoundException();
                var fieldDTOs = ctx.ExpertFields
                    .Where(ef => ef.UserId == user.UserId)
                    .Select(Mapper.Map<GetExpertFieldDTO>)
                    .ToList();
                return fieldDTOs;
            }
        }

        /// <summary>
        /// 添加或者更新专家领域
        /// </summary>
        /// <param name="fieldDTOs"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public List<GetExpertFieldDTO> AddOrUpdateExpertField(List<AddExpertFieldDTO> fieldDTOs, Func<User, bool> privilege)
        {
            List<ExpertField> fields = new List<ExpertField>();
            using (var ctx = new AspodesDB())
            {
                foreach (var fieldDTO in fieldDTOs)
                {
                    var field = ctx.ExpertFields.FirstOrDefault(ef => ef.ExpertFieldId == fieldDTO.ExpertFieldId);
                    var user = ctx.Users.FirstOrDefault(u => u.PersonId == fieldDTO.PersonId);

                    var newField = Mapper.Map<ExpertField>(fieldDTO);
                    newField.UserId = user.UserId;

                    if (!privilege(user)) throw new UnauthorizationException();

                    if (field == null)
                    {
                        if (ctx.ExpertFields.Where(ef => ef.UserId == user.UserId).Count() >= SystemConfig.ExpertFieldAmount) break;
                        fields.Add(ctx.ExpertFields.Add(newField));

                    }
                    else
                    {
                        ctx.Entry(field).CurrentValues.SetValues(newField);
                        fields.Add(newField);
                    }
                }

                ctx.SaveChanges();

                return fields.Select(Mapper.Map<GetExpertFieldDTO>).ToList();
            }
        }

        /// <summary>
        /// 删除专家的研究领域
        /// </summary>
        /// <param name="expertFieldId"></param>
        /// <returns></returns>
        public void DeleteExpertField(int? expertFieldId, Func<User, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var field = ctx.ExpertFields.FirstOrDefault(ef => ef.ExpertFieldId == expertFieldId);
                if (null == field) throw new NotFoundException();
                if (!privilege(field.User)) throw new UnauthorizationException();
                ctx.ExpertFields.Remove(field);
                ctx.SaveChanges();
            }
        }
    }
}