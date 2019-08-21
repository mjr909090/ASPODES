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
using ASPODES.DTO.Inst_Person_User;
using System.IO;
using ASPODES.Common;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using System.Text;
using NPOI.SS.UserModel;
using System.Security.Principal;
using System.Data.Entity;
using ASPODES.Common.Util;

using System.ComponentModel.DataAnnotations;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 单位操作类
    /// </summary>
    public class InstRepository
    {
        /// <summary>
        /// 根据单位ID获取单位信息
        /// </summary>
        /// <param name="instId">单位ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success和单位信息
        /// 未找到数据，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetInstituteDTO GetOneInst(int instId)
        {
            using (var ctx = new AspodesDB())
            {
                var inst = ctx.Institutes.FirstOrDefault(i => i.InstituteId == instId);
                if (inst == null)
                    throw new NotFoundException();
                var instDTO = Mapper.Map<GetInstituteDTO>(inst);
                if (inst.ContactId != null)
                {
                    var contact = ctx.Users.FirstOrDefault(u => u.UserId == inst.ContactId);
                    if (null != contact)
                    {
                        instDTO.ContactEmail = contact.UserId;
                        instDTO.ContactName = contact.Name;
                        instDTO.ContactPhone = contact.Person.Phone;
                    }
                }
                return instDTO;
            }
        }

        /// <summary>
        /// 获取单位的简要信息列表，ComboBox下拉列表
        /// </summary>
        /// <param name="predicate">检索条件</param>
        /// <returns></returns>
        public List<GetComboInstDTO> GetComboInstList(Func<Institute, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Institutes
                    .Where(predicate)
                    .Select(Mapper.Map<GetComboInstDTO>)
                    .ToList();
            }
        }

        /// <summary>
        /// 获取单位的详细信息列表
        /// </summary>
        /// <param name="predicate">检索条件</param>
        /// <returns></returns>
        public List<GetInstituteDTO> GetInstList(Func<Institute, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Institutes.Include("Contact")
                    .Where(predicate)
                    .Select( Mapper.Map<GetInstituteDTO>)
                    .ToList();
            }
        }

        /// <summary>
        /// 根据单位Id获取名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetInstName(int id)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Institutes.Find(id).Name.ToString();
            }
        }

        /// <summary>
        /// 根据单位Id获取简称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetInstShortName(int id)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Institutes.Find(id).ShortName.ToString();
            }
        }

        /// <summary>
        /// 导入单位信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage UplodInstInfoFile()
        {
            IWorkbook workbook = null;
            ISheet sheet = null;

            StringBuilder responseMsg = new StringBuilder();
            Dictionary<string, int> propertyIndex = new Dictionary<string, int>();
            List<AddInstDTO> uploadInstDTOs = new List<AddInstDTO>();
            //读取文件
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.UploadFilePathWeb, "InstInfo");

                //保存文件
                //string newName = DateTime.Now.ToFileTime() + ".xlsx";
                string newName = FileHelper.Upload(HttpContext.Current, dir);

                //获取文件标题映射
                var fileTitle = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "InstInfoFileTitle.json"), Encoding.UTF8);
                Dictionary<string, string> titleMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileTitle);

                //读取上传的Excel
                workbook = new XSSFWorkbook(Path.Combine(dir, newName));
                sheet = workbook.GetSheetAt(0);

                //读取Excel表格标题
                int titleRowIndex = 0;
                var titleRow = sheet.GetRow(titleRowIndex);
                foreach (var map in titleMap)
                {
                    var cell = titleRow.Cells.First(c => !string.IsNullOrEmpty(c.StringCellValue) && c.StringCellValue.Trim().Equals(map.Value));
                    propertyIndex.Add(map.Key, cell.ColumnIndex);
                }
                //删除标题行
                sheet.RemoveRow(titleRow);

                //检查数据是否为空
                Type uploadType = typeof(AddInstDTO);
                using (var ctx = new AspodesDB())
                {

                    foreach (IRow row in sheet)
                    {
                        AddInstDTO uploadDTO = new AddInstDTO();
                        foreach (var map in propertyIndex)
                        {
                            //检查单元格是否为空
                            ICell cell = row.GetCell(map.Value);
                            var property = uploadType.GetProperty(map.Key);
                            property.SetValue(uploadDTO, cell == null ? null : cell.StringCellValue);
                        }

                        //检查属性值是否合法
                        var context = new ValidationContext(uploadDTO, null, null);
                        var results = new List<ValidationResult>();
                        Validator.TryValidateObject(uploadDTO, context, results, true);
                        foreach (var validationResult in results)
                        {
                            responseMsg.Append(string.Format("第{0}行：{1}<br/>", row.RowNum, validationResult.ErrorMessage));
                        }
                        uploadInstDTOs.Add(uploadDTO);
                    }
                }
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            finally
            {
                if (null != workbook) workbook.Close();
            }

            DbContextTransaction transaction = null;
            using (var ctx = new AspodesDB())
            {
                try
                {
                    foreach (var dto in uploadInstDTOs)
                    {
                        //检查联系人、单位是否存在
                        if (ctx.Institutes.Any(i => i.Code == dto.Code || i.Name == dto.Name))
                        {
                            responseMsg.Append(string.Format("代码：{0}，名称：{1}单位已存在\n", dto.Code, dto.Name));
                        }
                        if (ctx.Persons.Any(p => p.IDCard == dto.IDCard || p.Email == dto.Email))
                        {
                            responseMsg.Append(string.Format("身份证：{0}，邮箱：{1}人员已存在\n", dto.IDCard, dto.Email));
                        }
                    }
                    if (responseMsg.Length > 0) return ResponseWrapper.ExceptionResponse( new OtherException(responseMsg.ToString()));

                    //添加进数据库
                    transaction = ctx.Database.BeginTransaction();
                    foreach (var dto in uploadInstDTOs)
                    {
                        AddInst(ctx, dto);
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (null != transaction) transaction.Rollback();
                    return ResponseWrapper.ExceptionResponse(e);
                }
                finally
                {
                    if (null != transaction) transaction.Dispose();
                }
            }
            return ResponseWrapper.SuccessResponse("上传成功");
        }


        private Institute AddInst(AspodesDB ctx, AddInstDTO dto)
        {
            var inst = Mapper.Map<Institute>(dto);
            var person = Mapper.Map<Person>(dto);
            //添加单位
            var newInst = ctx.Institutes.Add(inst);
            ctx.SaveChanges();

            //添加联系人
            person.InstituteId = newInst.InstituteId;
            var newPerson = ctx.Persons.Add(person);
            ctx.SaveChanges();

            //给联系人添加账户
            User user = ctx.Users.Add(Mapper.Map<User>(newPerson));
            ctx.SaveChanges();

            //设置单位联系人
            newInst.ContactId = user.UserId;

            //给联系人添加用户角色
            Model.Authorize commen = new Model.Authorize
            {
                UserId = user.UserId,
                RoleId = 1
            };
            ctx.Authorizes.Add(commen);

            //给联系人添加单位管理员角色
            Model.Authorize instAdmin = new Model.Authorize
            {
                UserId = user.UserId,
                RoleId = 2
            };
            ctx.Authorizes.Add(instAdmin);
            ctx.SaveChanges();
            return newInst;
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="instDTO">单位信息</param>
        /// <returns>
        /// 添加成功，返回ResponseStatus.success
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetInstituteDTO AddInstitute(AddInstituteDTO instDTO)
        {
            Institute inst = Mapper.Map<Institute>(instDTO);

            if (instDTO.Type=="0")
            {
                inst.Type = InstituteType.PARTNER;
            }

            using (var ctx = new AspodesDB())
            {
                //检查机构编码是否已经存在
                if (ctx.Institutes.Any(p => p.Code == instDTO.Code))
                    throw new OtherException("机构编码已经存在");
                //检查单位名称是否已经存在
                if (ctx.Institutes.Any(u => u.Name == instDTO.Name))
                    throw new OtherException("单位名称已经存在");


                inst.Status = "C";
                var newInst = ctx.Institutes.Add(inst);
                ctx.SaveChanges();
                return Mapper.Map<GetInstituteDTO>(inst);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadDTO"></param>
        /// <returns></returns>
        public GetInstituteDTO AddInstitute(AddInstDTO uploadDTO)
        {

            using (var ctx = new AspodesDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        //检查联系人、单位是否存在
                        if (ctx.Institutes.Any(i => i.Code == uploadDTO.Code || i.Name == uploadDTO.Name))
                        {
                            throw new OtherException("单位已存在");
                        }
                        if (ctx.Persons.Any(p => p.IDCard == uploadDTO.IDCard || p.Email == uploadDTO.Email))
                        {
                            throw new OtherException("人员已存在");
                        }
                        var newInst = AddInst(ctx, uploadDTO);
                        transaction.Commit();
                        return Mapper.Map<GetInstituteDTO>(newInst);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public void DeleteInst(int id)
        {
            using (var ctx = new AspodesDB())
            {
                var inst = ctx.Institutes.FirstOrDefault(i => i.InstituteId == id);
                if (null == inst) throw new NotFoundException();
                if (ctx.Persons.Where(p => p.InstituteId == inst.InstituteId).Count() > 0)
                {
                    throw new OtherException("单位有人员记录");
                }
                if (ctx.Applications.Where(a => a.InstituteId == inst.InstituteId).Count() > 0)
                {
                    throw new OtherException("单位有申请书记录");
                }

                ctx.Institutes.Remove(inst);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 单位管理员更改单位信息
        /// </summary>
        /// <param name="instDTO">更新的单位信息</param>
        /// <returns></returns>
        public GetInstituteDTO UpdateInstitute(AddInstituteDTO instDTO, Func<Institute, bool> privilege)
        {
            var newValue = Mapper.Map<Institute>(instDTO);
            using (var ctx = new AspodesDB())
            {
                var inst = ctx.Institutes.FirstOrDefault(i => i.InstituteId == instDTO.InstituteId);
                if (null == inst) throw new NotFoundException();
                if (!privilege(inst)) throw new UnauthorizationException();
                //不允许在Update时更改的值
                newValue.Status = inst.Status;
                newValue.ParentId = inst.ParentId;

                ctx.Entry(inst).CurrentValues.SetValues(newValue);
                ctx.SaveChanges();
                return Mapper.Map<GetInstituteDTO>(inst);
            }
        }
    }
}