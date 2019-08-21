using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.DTO.Review;
using ASPODES.Database;
using AutoMapper;
using ASPODES.WebAPI.Common;
using System.Security.Principal;
using ASPODES.WebAPI;
using System.Text;
using ASPODES.DTO;
using ASPODES.DTO.Inst_Person_User;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 专家推荐的操作类
    /// </summary>
    public class RecommendationRepository
    {

        /// <summary>
        /// 获取专家推荐列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public PagingListDTO<GetRecommendationDTO> GetPagingRecommendationList(Func<Recommendation, bool> predicate, int page)
        {
            PagingListDTO<GetRecommendationDTO> pagingList = new PagingListDTO<GetRecommendationDTO>();
            using (var ctx = new AspodesDB())
            {
                pagingList.TotalNum = ctx.Recommendations.Count(predicate);
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.RecommendationPageCount - 1) / SystemConfig.RecommendationPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page <= 0 ? 1 : page;

                pagingList.ItemDTOs = ctx.Recommendations
                    .Where(predicate)
                    .Select(Mapper.Map<GetRecommendationDTO>)
                    .OrderBy(rd => rd.Time)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.RecommendationPageCount)
                    .Take(SystemConfig.RecommendationPageCount)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;

            }

        }

        /// <summary>
        /// 获取未进行专家推荐的人员列表
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagingListDTO<GetPersonDTO> GetPagingUnRecommendationList(int instId, int page)
        {
            PagingListDTO<GetPersonDTO> pagingList = new PagingListDTO<GetPersonDTO>();
            using (var ctx = new AspodesDB())
            {
                //因修改bug比较着急，以后要写成委托方法
                pagingList.TotalNum = ctx.Users.Where(u => u.InstituteId == instId)
                    .Where(u => !(ctx.Recommendations.Where(r=>r.InstituteId==instId)
                                    .Where(r=>r.Adopt==null || r.Adopt==true)
                                    .Select(r => r.CandidateId))
                                .Contains(u.UserId)).Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.RecommendationPageCount - 1) / SystemConfig.RecommendationPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page <= 0 ? 1 : page;

                pagingList.ItemDTOs = ctx.Users.Where(u => u.InstituteId == instId)
                    .Where(u => !(ctx.Recommendations.Where(r => r.InstituteId == instId)
                                    .Where(r => r.Adopt == null || r.Adopt == true)
                                    .Select(r => r.CandidateId))
                                .Contains(u.UserId))
                    .Where(u=>u.Person.Status=="C" && u.Person.Email != "system@126.com")
                    .Select(Mapper.Map<GetPersonDTO>)
                    .OrderBy(pd=>pd.Name)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.RecommendationPageCount)
                    .Take(SystemConfig.RecommendationPageCount)
                    .ToList();
                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;
            }

        }


        /// <summary>
        /// 单位管理员推荐专家
        /// </summary>
        /// <param name="instId">单位ID</param>
        /// <param name="recommenderId">推荐人ID</param>
        /// <param name="users">候选人ID列表</param>
        /// <returns></returns>
        public GetRecommendationDTO AddRecommendation(string recommenderId, int candidateId, Func<User, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                //验证被推荐人的用户状态
                var candidate = ctx.Users.FirstOrDefault(u => u.PersonId == candidateId);
                if (UserHelper.UnnormalUser(candidate))
                {
                    throw new NotFoundException("该用户不存在");
                }
                //操作权限
                if (!privilege(candidate))
                {
                    throw new UnauthorizationException();
                }
                //被推荐人是否是专家
                if (ctx.Authorizes.Any(a => a.UserId == candidate.UserId && a.RoleId == 5))
                {
                    throw new OtherException("该用户已经是专家");
                }

                //判断重复推荐
                if (ctx.Recommendations.Any(r => r.Adopt == null && r.CandidateId == candidate.UserId))
                {
                    throw new OtherException("该用户的推荐已经存在<br/>");
                }
                if (ctx.Recommendations.Any(r => r.Adopt == null && r.CandidateId == candidate.UserId))
                {
                    throw new OtherException("该用户的推荐已经存在<br/>");
                }

                if (ctx.Recommendations.Any(r => r.Adopt == false && r.CandidateId == candidate.UserId))
                {
                    var Rec = ctx.Recommendations.FirstOrDefault(r => r.Adopt == false && r.CandidateId == candidate.UserId);
                    Rec.Adopt = null;
                    ctx.SaveChanges();
                    return Mapper.Map<GetRecommendationDTO>(Rec);
                }
                else
                {
                    Recommendation recomm = new Recommendation
                    {
                        RecommenderId = recommenderId,
                        CandidateId = candidate.UserId,
                        InstituteId = candidate.InstituteId,
                        Agree = true,
                        Time = DateTime.Now
                    };
                    var Rec = ctx.Recommendations.Add(recomm);
                    ctx.SaveChanges();
                    return Mapper.Map<GetRecommendationDTO>(Rec);
                } 
            }
        }

        /// <summary>
        /// 院管理员同意专家推荐
        /// </summary>
        /// <param name="recommendationId"></param>
        /// <returns></returns>
        public GetRecommendationDTO AdoptRecommendation(int recommendationId, Func<Recommendation, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                //取出专家推荐
                var recommendation = ctx.Recommendations.FirstOrDefault(r => r.RecommendationId == recommendationId);
                if (recommendation == null)
                    throw new NotFoundException("未找到该用户");

                //取出专家候选人
                var candidate = ctx.Users.FirstOrDefault(u => u.UserId == recommendation.CandidateId);
                //用户状态
                if (UserHelper.UnnormalUser(candidate))
                {
                    throw new NotFoundException("未找到用户或者用户被删除锁定<br/>");
                }
                //检查专家信息
                if ( !PersonRepository.ExpertFieldComplete(candidate.PersonId.Value) )
                {
                    throw new OtherException("请完善专家研究领域");
                }
                //检查是否已经是专家，如果不是，添加专家角色
                if (!ctx.Authorizes.Any(a => a.UserId == recommendation.CandidateId && a.RoleId == 5))
                {
                    PersonRepository.GrantRole(candidate.PersonId.Value, 5, p => true);
                }
                //接受推荐
                recommendation.Adopt = true;

                ctx.SaveChanges();
                return Mapper.Map<GetRecommendationDTO>(recommendation);
            }
        }

        /// <summary>
        /// 院管理员拒绝专家推荐
        /// </summary>
        /// <param name="recommendationId">专家推荐ID</param>
        /// <returns></returns>
        public GetRecommendationDTO RefuseRecommendation(int recommendationId)
        {
            using (var ctx = new AspodesDB())
            {
                var recommendation = ctx.Recommendations.FirstOrDefault(r => r.RecommendationId == recommendationId);
                if (recommendation == null)
                    throw new NotFoundException("未找到该用户");
                recommendation.Adopt = false;
                ctx.SaveChanges();
                return Mapper.Map<GetRecommendationDTO>(recommendation);
            }
        }

        /// <summary>
        /// 根据人员查找专家推荐
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        public Recommendation GetRecommendation(int personId)
        {
            using (var ctx = new AspodesDB())
            {
                var recommendation = ctx.Recommendations.FirstOrDefault(r => r.Candidate.PersonId == personId);
                return recommendation;
            }
        }

        /// <summary>
        /// 删除专家推荐
        /// </summary>
        /// <param name="recommendationId">专家推荐的ID</param>
        /// <returns></returns>
        public void DeleteRecommendation(int recommendationId, Func<Recommendation, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var recommendation = ctx.Recommendations.FirstOrDefault(r => r.RecommendationId == recommendationId);
                if (null == recommendation) throw new NotFoundException("未找到该用户");
                if (!privilege(recommendation))
                {
                    throw new UnauthorizationException();
                }
                if (recommendation.Adopt != null)
                {
                    throw new OtherException("院管理员已处理，不可撤销");
                }
                ctx.Recommendations.Remove(recommendation);
                ctx.SaveChanges();
            }

        }

        /// <summary>
        /// 通过专家推荐的Id获取专家推荐
        /// </summary>
        /// <param name="recommendationId"></param>
        /// <returns></returns>
        internal Recommendation GetRecommendationById(int recommendationId)
        {
            using (var _context = new AspodesDB())
            {
                return _context.Recommendations.Find(recommendationId);
            }
        }
    }
}