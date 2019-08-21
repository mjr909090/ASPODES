using ASPODES.Common;
using ASPODES.Database;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 用于生成Excel表格和导出excel表格
    /// </summary>
    public class ExportExcelRepository
    {
        private AspodesDB _context;
        /// <summary>
        /// 导出excel的构造函数
        /// </summary>
        /// <param name="context"></param>
        public ExportExcelRepository(AspodesDB context)
        {
            this._context = context;
        }

        /// <summary>
        /// 根据传入的数据生成并导出excel表格
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public HttpResponseMessage DownloadExportExcel(string [][] strs, string sheetName)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.General;
            style.VerticalAlignment = VerticalAlignment.Center;
            ISheet sheet = workbook.CreateSheet(sheetName);

            int insertRow = 0;
            foreach (string[] cell in strs)
            {
                IRow row = sheet.CreateRow(insertRow);
                for (int i=0; i<cell.Length;i++)
                {
                    row.CreateCell(i).SetCellValue(cell[i]);
                }
                insertRow++;
            }

            string desName = string.Format("{0}_{1}.{2}", sheetName, DateTime.Now.ToFileTime(), "xls");
            string desPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ExportExcel);
            if (!Directory.Exists(desPath)) Directory.CreateDirectory(desPath);

            var stream = File.OpenWrite(desPath+desName);
            workbook.Write(stream);
            stream.Close();
            workbook.Close();
            return FileHelper.Download(HttpContext.Current, "\\" + SystemConfig.ExportExcel + desName, desName);
        }

    }
}