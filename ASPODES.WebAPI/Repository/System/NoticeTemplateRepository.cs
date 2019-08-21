using ASPODES.Database;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Repository
{
    public class NoticeTemplateRepository
    {
        private AspodesDB _context;
        public NoticeTemplateRepository(AspodesDB context)
        {
            this._context = context;
        }

        /// <summary>
        /// 通过模板Id获取通知模板的内容
        /// </summary>
        /// <param name="noticeTemplateId"></param>
        /// <returns></returns>
        public string GetContentById(int noticeTemplateId)
        {
            return _context.NoticeTemplates.Find(noticeTemplateId).Content;
        }
    }
}