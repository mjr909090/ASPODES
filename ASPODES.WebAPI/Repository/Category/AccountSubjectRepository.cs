using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;

using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using System.Net;

namespace ASPODES.WebAPI.Repository
{

    public class AccountSubjectRepository
    {
        /// <summary>
        /// 获取支出科目列表
        /// </summary>
        /// <returns></returns>
        public List<AccountingSubject> GetAccountSubjects()
        {
                using (var ctx = new AspodesDB())
                {
                    return ctx.AccountingSubjects.ToList();
                }
        }
    }
}