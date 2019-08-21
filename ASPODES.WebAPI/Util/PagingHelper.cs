using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.DTO;

namespace ASPODES.WebAPI.Util
{
    public class PagingHelper
    {
        public static PagingListDTO<T> PagingWrapper<T>(IEnumerable<T> items, int page, int limit)
        {
            PagingListDTO<T> pagingList = new PagingListDTO<T>();
            pagingList.TotalNum = items.Count();
            pagingList.TotalPageNum = (pagingList.TotalNum + limit - 1) / limit;
            if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
            pagingList.NowNum = limit;
            pagingList.NowPage = page;
            pagingList.ItemDTOs = items
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return pagingList;
        }
    }
}