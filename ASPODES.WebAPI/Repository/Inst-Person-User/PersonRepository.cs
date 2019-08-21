using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Http;
using AutoMapper;
using System.Text.RegularExpressions;
using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Inst_Person_User;
using System.IO;
using Newtonsoft.Json;
using ASPODES.Common;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;
using System.Data.Entity;
using System.Security.Principal;
using System.Data.Entity.Migrations;
using ASPODES.Common.Util;
using ASPODES.WebAPI.Models;
using ASPODES.DTO;
using ASPODES.DTO.Application;
using ASPODES.DTO.Category;
using NPinyin;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 人员操作类
    /// </summary>
    public class PersonRepository
    {
        //获取用户列表的方法过多，考虑合并方法
        //-----------------------User---------------------------
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success人员信息
        /// 未找到数据，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetPersonDTO GetOnePerson(int personId)
        {
            using (var ctx = new AspodesDB())
            {
                var person = ctx.Persons.FirstOrDefault(p => p.PersonId == personId);
                if (person == null)
                    throw new NotFoundException();
                return Mapper.Map<GetPersonDTO>(person);
            }

        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success人员信息
        /// 未找到数据，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetPersonDTO GetOnePersonByUserId(string userId)
        {
            using (var ctx = new AspodesDB())
            {
                int personId = ctx.Users.Find(userId).PersonId.Value;
                var person = ctx.Persons.FirstOrDefault(p => p.PersonId == personId);
                if (person == null)
                    throw new NotFoundException();
                return Mapper.Map<GetPersonDTO>(person);
            }
        }

        /// <summary>
        /// 从正常人员中查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public List<GetComboPersonDTO> GetNormalPersonComboList(Func<Person, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Persons
                    .Where(p => p.Status == "C")
                    .Where(predicate)
                    .OrderBy(p => p.InstituteId)
                    .ThenBy(p => p.Name)
                    .Select(Mapper.Map<GetComboPersonDTO>)
                    .ToList();
            }
        }

        /// <summary>
        /// 获取全部非农科院人员列表的ComboPerson
        /// </summary>
        /// <returns></returns>
        public List<GetComboPersonDTO> GetPartnersPersonComboList()
        {
            using (var ctx = new AspodesDB())
            {
                List<int> instIdList = ctx.Institutes.Where(c => c.Type == InstituteType.PARTNER)
                    .Select(c => c.InstituteId).ToList();
                return ctx.Persons
                    .Where(p => p.Status == "C" && instIdList.Contains(p.InstituteId.Value))
                    .OrderBy(p => p.InstituteId)
                    .ThenBy(p => p.Name)
                    .Select(Mapper.Map<GetComboPersonDTO>)
                    .ToList();
            }
        }

        /// <summary>
        /// 获取全部非农科院人员列表
        /// </summary>
        /// <returns></returns>
        public List<GetPersonDTO> GetPartnersPersonList()
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Persons.Include("Institute")
                    .Where(p => p.Status == "C" && p.Institute.Type == InstituteType.PARTNER)
                    .OrderBy(p => p.InstituteId)
                    .ThenBy(p => p.Name)
                    .Select(Mapper.Map<GetPersonDTO>)
                    .ToList();
            }
        }


        /// <summary>
        /// 检索人员
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public PagingListDTO<GetPersonDTO> GetNormalQueryPersonList(QueryPersonDTO query, int page)
        {

            Func<Person, bool> predicate = p =>
                ((query.InstituteId == 0) || (p.InstituteId == query.InstituteId))
                && (string.IsNullOrWhiteSpace(query.Name) || Regex.IsMatch(p.Name, string.Format("%{0}%", query.Name)))
                && (string.IsNullOrWhiteSpace(query.IDCard) || Regex.IsMatch(p.IDCard, string.Format("%{0}%", query.IDCard)))
                && (string.IsNullOrWhiteSpace(query.Email) || Regex.IsMatch(p.Email, string.Format("%{0}%", query.Email)))
                && (string.IsNullOrWhiteSpace(query.Phone) || Regex.IsMatch(p.Phone, string.Format("%{0}%", query.Phone)));
            return GetPagingPersonList(predicate, page);

        }
        /// <summary>
        /// 从正常人员中查询，分页
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public PagingListDTO<GetPersonDTO> GetPagingPersonList(Func<Person, bool> predicate, int page)
        {
            page = page <= 0 ? 1 : page;
            PagingListDTO<GetPersonDTO> pagingList = new PagingListDTO<GetPersonDTO>();
            using (var ctx = new AspodesDB())
            {
                pagingList.TotalNum = ctx.Persons.Count<Person>(predicate);
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.PersonPageCount - 1) / SystemConfig.PersonPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page;

                pagingList.ItemDTOs = ctx.Persons
                    .Where(predicate)
                    .Select(Mapper.Map<GetPersonDTO>)
                    .OrderBy(p => p.InstituteId)
                    .ThenBy(p => p.Name)
                    .Skip((page - 1) * SystemConfig.PersonPageCount)
                    .Take(SystemConfig.PersonPageCount)
                    .ToList();
                pagingList.NowNum = pagingList.ItemDTOs.Count();
            }
            return pagingList;
        }

        //-----------------InstAdmin-----------------------

        //单位管理员上传人员信息
        public void UploadPersonInfoFile(int instId)
        {
            StringBuilder responseMsg = new StringBuilder();
            List<Person> persons = new List<Person>();
            //读取文件

            //获取绝对路径
            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.UploadFilePathWeb, "InstInfo"));
            //保存文件
            //string newName = DateTime.Now.ToFileTime() + ".xlsx";
            var fileTitle = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "PersonInfoFileTitle.json"), Encoding.UTF8);
            //获取文件标题映射
            Dictionary<string, string> titleMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileTitle);
            string newName = FileHelper.Upload(HttpContext.Current, dir.FullName);
            //读取上传的Excel
            var file = Path.Combine(dir.FullName, newName);
            var workbook = new XSSFWorkbook(file);
            var sheet = workbook.GetSheetAt(0);
            //第0行是标题
            int titleRowIndex = 0;
            var titleRow = sheet.GetRow(titleRowIndex);
            Dictionary<string, int> propertyIndex = new Dictionary<string, int>();
            foreach (var map in titleMap)
            {
                var cell = titleRow.Cells.First(c => !string.IsNullOrEmpty(c.StringCellValue) && c.StringCellValue.Trim().Equals(map.Value));
                if (cell != null)
                {
                    propertyIndex.Add(map.Key, cell.ColumnIndex);
                }
            }
            //删除标题行
            sheet.RemoveRow(titleRow);

            //检查数据是否合法
            Type modelType = typeof(Person);
            foreach (IRow row in sheet)
            {
                Person person = new Person() { InstituteId = instId };
                foreach (var map in propertyIndex)
                {
                    ICell cell = row.GetCell(map.Value);
                    if (cell == null) continue;

                    var property = modelType.GetProperty(map.Key);
                    property.SetValue(person, cell.StringCellValue.Trim());
                }

                var context = new ValidationContext(person, null, null);
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(person, context, results, true);
                foreach (var validationResult in results)
                {
                    responseMsg.Append(string.Format("第{0}行：{1}<br/>", row.RowNum, validationResult.ErrorMessage));
                }
                persons.Add(person);
            }



            DbContextTransaction trans = null;
            using (var ctx = new AspodesDB())
            {

                //检查人员是否存在
                foreach (var person in persons)
                {
                    if (null != ctx.Persons.FirstOrDefault(hp => hp.IDCard == person.IDCard))
                    {
                        responseMsg.Append(string.Format("身份证为{0}的人员已存在<br/>", person.IDCard));
                    }
                    if (null != ctx.Users.FirstOrDefault(u => u.UserId == person.Email))
                    {
                        responseMsg.Append(string.Format("用户名{0}的用户已存在<br/>", person.IDCard));
                    }
                }
                if (responseMsg.Length > 0)
                    throw new ModelValidException(responseMsg.ToString());

                try
                {
                    //添加人员
                    trans = ctx.Database.BeginTransaction();
                    foreach (var person in persons)
                    {
                        CompletePersonInfo(person);
                        AddPerson(ctx, person);
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    if (null != trans) 
                        trans.Rollback();
                    throw e;
                }
                finally
                {
                    if (null != trans) trans.Dispose();
                }
            }
        }

        private Person CompletePersonInfo(Person person)
        {
            //p.Status = "C"; //设置默认值为C
            person.Name = person.FirstName + person.LastName;
            if (person.EnglishName == null)
            {
                person.EnglishName = Pinyin.GetPinyin(person.FirstName) + "" + Pinyin.GetPinyin(person.LastName).Replace(" ", "");
            }
            if (person.IDCard != null && person.IDCard.Length == 18)
            {
                DateTime birthDay;
                if (!DateTime.TryParseExact(person.IDCard.Substring(6, 8), "yyyyMMdd", new CultureInfo("zh-CN", true), DateTimeStyles.None, out birthDay))
                {
                    birthDay = DateTime.Now;
                }

                person.Birthday = birthDay;

                //数字的奇偶和对应ASC码的奇偶一致
                person.Male = person.IDCard.ElementAt(16) % 2 == 1 ? "男" : "女";
            }
            return person;
        }

        private Person AddPerson(AspodesDB ctx, Person person)
        {
            ctx.Persons.Add(person);
            ctx.SaveChanges();
            Person newPerson = ctx.Persons.Where(c => c.IDCard == person.IDCard && c.Email == person.Email).First();
            Institute personInst = ctx.Institutes.Where(c => c.InstituteId == newPerson.InstituteId).First();
            if (personInst.Type != InstituteType.PARTNER)   //添加外单位人员，不需要建立用户
            {
                //添加用户
                User user = Mapper.Map<User>(newPerson);
                User newUser = ctx.Users.Add(user);
                //添加用户角色
                Model.Authorize authorize = new Model.Authorize()
                {
                    UserId = newUser.UserId,
                    RoleId = 1
                };
                ctx.Authorizes.Add(authorize);
                ctx.SaveChanges();
            }
            
            return newPerson;
        }

        internal Person GetPersion(int personId)
        {
            using (var _context = new AspodesDB())
            {
                return _context.Persons.Find(personId);
            }
        }

        //-----------------DepartAdmin----------------------


        /// <summary>
        /// 给PersonId用户赋予roleId角色
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="privilege">权限验证</param>
        /// <returns></returns>
        public static bool GrantRole(int personId, int roleId, Func<Person, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                //用户状态是否正常
                if (!ctx.Persons.Where(p => p.Status == "C").Any(p => p.PersonId == personId))
                {
                    throw new OtherException("用户状态错误");
                }
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);

                //用户是否已经拥有该角色
                var role = ctx.Authorizes.FirstOrDefault(a => a.UserId == user.UserId && a.RoleId == roleId);
                if (ctx.Authorizes.Any(a => a.UserId == user.UserId && a.RoleId == roleId))
                {
                    throw new OtherException("用户已经拥有该角色");
                }
                //操作人是否有权限
                if (!privilege(user.Person))
                {
                    throw new UnauthorizationException();
                }

                Model.Authorize instAdmin = new Model.Authorize
                {
                    RoleId = roleId,
                    UserId = user.UserId
                };
                ctx.Authorizes.Add(instAdmin);
                ctx.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 撤销PersonID用户roleId角色
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="roleId"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public static bool RevokeRole(int personId, int roleId, Func<Person, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                //用户状态为正常
                //用户状态是否正常
                if (!ctx.Persons.Where(p => p.Status == "C").Any(p => p.PersonId == personId))
                {
                    throw new OtherException("用户状态错误");
                }
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                //取出权限记录
                var authorize = ctx.Authorizes.FirstOrDefault(a => a.UserId == user.UserId && a.RoleId == roleId);
                if (null == authorize)
                {
                    throw new OtherException("用户没有该角色");
                }
                //验证操作者的权限
                if (!privilege(user.Person))
                {
                    throw new UnauthorizationException();
                }
                //删除
                ctx.Authorizes.Remove(authorize);
                ctx.SaveChanges();
            }
            return true;
        }


        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <param name="privilege">权限验证</param>
        /// <returns></returns>
        public void DeletePerson(int id, Func<Person, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                Person person = ctx.Persons.FirstOrDefault(p => p.PersonId == id);
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == person.PersonId);
                if (user != null)
                {
                    if (ctx.Authorizes.Any(au => au.UserId == user.UserId && au.RoleId != 1))
                    {
                        throw new OtherException("用户有多个角色不允许删除");
                    }
                    if (ctx.Institutes.Any(i => i.ContactId == user.UserId))
                    {
                        throw new OtherException("单位联系人不允许删除");
                    }
                }

                //验证操作权限
                if (!privilege(person))
                {
                    throw new UnauthorizationException();
                }
                //全部使用软删除
                person.Status = "D";
                //逻辑删除
                //if (LogicDelete(ctx, person))
                //{
                //    person.Status = "D";
                //}
                //else
                //{
                //    if (null != user)
                //    {
                //        var authorizes = ctx.Authorizes.Where(au => au.UserId == user.UserId).ToList();
                //        ctx.Authorizes.RemoveRange(authorizes);
                //        var login = ctx.LoginLogs.Where(ll => ll.UserId == user.UserId).ToList();
                //        ctx.LoginLogs.RemoveRange(login);
                //        ctx.Users.Remove(user);
                //    }
                //    ctx.Persons.Remove(person);
                //}
                ctx.SaveChanges();
            }

        }

        /// <summary>
        /// 判断是否逻辑删除用户（人员）
        /// </summary>
        /// <param name="ctx">数据库操作句柄</param>
        /// <param name="person">人员实体</param>
        /// <returns></returns>
        private bool LogicDelete(AspodesDB ctx, Person person)
        {
            var member = ctx.Members.FirstOrDefault(m => m.PersonId == person.PersonId);
            if (ctx.Members.Any(m => m.PersonId == person.PersonId)) return true;
            var user = ctx.Users.FirstOrDefault(u => u.PersonId == person.PersonId);
            if (user != null)
            {
                if (ctx.Recommendations.Any(r => r.RecommenderId == user.UserId || r.CandidateId == user.UserId)) return true;
                if (ctx.ReviewAssignments.Any(ra => ra.ExpertId == user.UserId)) return true;
                if (ctx.ReviwerComments.Any(rc => rc.ExpertId == user.UserId)) return true;
            }
            return false;
        }

        /// <summary>
        /// 启用被禁的用户
        /// </summary>
        /// <param name="personId">用户人员的ID</param>
        /// <returns></returns>
        public GetPersonDTO RevivePerson(int personId, Func<Person, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var person = ctx.Persons.FirstOrDefault(p => p.PersonId == personId && p.Status != "C");
                if (null == person) throw new NotFoundException();
                if (!privilege(person)) throw new UnauthorizationException();
                person.Status = "C";
                ctx.SaveChanges();
                return Mapper.Map<GetPersonDTO>(person);
            }
        }


        /// <summary>
        /// 添加用户（人员）
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        public GetPersonDTO AddPerson(AddPersonDTO personDTO, Func<Person, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                //检查人员信息重复添加
                if (ctx.Persons.Any(p => p.IDCard == personDTO.IDCard))
                    throw new OtherException("人员信息已经存在");
                //检查用户是否已经存在
                if (ctx.Users.Any(u => u.UserId == personDTO.Email))
                    throw new OtherException("邮箱已注册");

                Person person = Mapper.Map<Person>(personDTO);

                //操作人是否有添加该用户的权限
                if (!privilege(person)) throw new UnauthorizationException();

                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        //添加进数据库
                        CompletePersonInfo(person);
                        var newPerson = AddPerson(ctx, person);
                        transaction.Commit();

                        return Mapper.Map<GetPersonDTO>(newPerson);
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
        /// 重置用户密码
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public void ResetPassword(int personId, Func<Person, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                User user = ctx.Users.FirstOrDefault(p => p.PersonId == personId);
                if (null == user) throw new NotFoundException("未找到用户");

                if (!privilege(user.Person)) throw new UnauthorizationException();

                user.Password = HashHelper.IntoMd5("123456");
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 更改用户信息，不允许更改邮箱（用户ID），所属单位
        /// </summary>
        /// <param name="ctx">数据库操作句柄</param>
        /// <param name="personDTO"></param>
        public GetPersonDTO UpdatePerson(UpdatePersonDTO personDTO, Func<Person, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                //取出Person
                var oldValue = ctx.Persons.FirstOrDefault(p => p.PersonId == personDTO.PersonId);
                if (null == oldValue) throw new NotFoundException();

                //修改内容权限
                if (oldValue.Status == "S" || !privilege(oldValue)) throw new UnauthorizationException();

                var newValue = Mapper.Map<Person>(personDTO);

                //检查身份证号码是否冲突
                if (newValue.IDCard != oldValue.IDCard && ctx.Persons.Any(p => p.IDCard == newValue.IDCard))
                    throw new OtherException("身份证号码冲突");

                //不允许更新的内容
                newValue.PersonId = oldValue.PersonId;
                newValue.Email = oldValue.Email;
                newValue.InstituteId = oldValue.InstituteId;
                newValue.Status = oldValue.Status;
                //ctx.Entry(person).Property("Email").IsModified = false;
                //ctx.Entry(person).Property("InstituteId").IsModified = false;
                //ctx.Entry(person).Property("Status").IsModified = false;

                newValue.Name = newValue.FirstName + newValue.LastName;
                if (newValue.Name != oldValue.Name && newValue.EnglishName == null)
                {
                    newValue.EnglishName = Pinyin.GetPinyin(newValue.FirstName) + ""
                        + Pinyin.GetPinyin(newValue.LastName).Replace(" ", "");
                }

                //更新Person内容
                ctx.Entry(oldValue).CurrentValues.SetValues(newValue);

                //更新关联用户的相关信息
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == oldValue.PersonId);
                if (null != user)
                {
                    user.Name = newValue.Name;
                    user.IDCard = newValue.IDCard;
                }
                ctx.SaveChanges();
                return Mapper.Map<GetPersonDTO>(newValue);
            }

        }

        /// <summary>
        /// 添加院管理员
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        public GetPersonDTO AddDepartAdmin(AddPersonDTO personDTO)
        {
            //人员属于农科院
            personDTO.InstituteId = 1;
            using (var ctx = new AspodesDB())
            {
                //是否检查人员信息重复添加
                var old = ctx.Persons.FirstOrDefault(p => p.IDCard == personDTO.IDCard);
                if (null != old) ResponseWrapper.ExceptionResponse(new OtherException("人员已经存在"));

                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        //添加人员
                        Person person = Mapper.Map<Person>(personDTO);
                        person.Name = person.FirstName + person.LastName;
                        person.Status = "C";
                        Person newPerson = ctx.Persons.Add(person);
                        ctx.SaveChanges();
                        //添加用户
                        User user = new User
                        {
                            UserId = newPerson.Email,
                            IDCard = newPerson.IDCard,
                            Name = newPerson.Name,
                            PersonId = newPerson.PersonId,
                            //Password = HashHelper.IntoMd5(newPerson.IDCard.Substring(newPerson.IDCard.Length - 6, 6)),
                            Password = HashHelper.IntoMd5("123456"),
                            InstituteId = newPerson.InstituteId
                        };
                        User newUser = ctx.Users.Add(user);
                        //添加用户角色
                        Model.Authorize authorize = new Model.Authorize()
                        {
                            UserId = newUser.UserId,
                            RoleId = 1
                        };
                        ctx.Authorizes.Add(authorize);

                        Model.Authorize departAdmin = new Model.Authorize()
                        {
                            UserId = newUser.UserId,
                            RoleId = 3
                        };
                        ctx.Authorizes.Add(departAdmin);

                        var instAdmins = from au in ctx.Authorizes
                                         where au.RoleId == 2 && au.User.InstituteId == 1
                                         select au.UserId;
                        if (instAdmins.Count() == 0)
                        {
                            Model.Authorize instAdmin = new Model.Authorize()
                            {
                                UserId = newUser.UserId,
                                RoleId = 2
                            };
                            ctx.Authorizes.Add(instAdmin);

                            var inst = ctx.Institutes.FirstOrDefault(i => i.InstituteId == 1);
                            inst.ContactId = newUser.UserId;
                        }
                        ctx.SaveChanges();
                        transaction.Commit();
                        return Mapper.Map<GetPersonDTO>(newPerson);
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
        /// 将PersonId设置为单位联系人
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        public void SetInstContact(int personId)
        {
            using (var ctx = new AspodesDB())
            {
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                if (UserHelper.UnnormalUser(user)) throw new NotFoundException();
                user.Institute.ContactId = user.UserId;
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 根据人员角色获取人员列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public PagingListDTO<GetPersonDTO> GetPagingPersonListByRole(int roleId, int instId, int page)
        {
            PagingListDTO<GetPersonDTO> pagingList = new PagingListDTO<GetPersonDTO>();

            using (var ctx = new AspodesDB())
            {
                var instRolePerson = from authorize in ctx.Authorizes
                                     join user in ctx.Users on authorize.UserId equals user.UserId
                                     where (instId == 0 || user.InstituteId == instId) && authorize.RoleId == roleId
                                     select user.Person;
                var result = instRolePerson.Where(p => p.Status == "C");


                pagingList.TotalNum = result.Count();
                pagingList.NowPage = page <= 0 ? 1 : page;
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.PersonPageCount - 1) / SystemConfig.PersonPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;

                pagingList.ItemDTOs = result
                    .OrderBy(nrp => nrp.Name)
                    .Skip(((pagingList.NowPage - 1)) * SystemConfig.PersonPageCount)
                    .Take(SystemConfig.PersonPageCount)
                    .Select(Mapper.Map<GetPersonDTO>)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;
            }

        }

        /// <summary>
        /// 院管理员候选人列表，简单信息列表
        /// </summary>
        /// <returns></returns>
        public List<GetComboPersonDTO> GetDedpartAdminCandidate()
        {
            List<GetComboPersonDTO> personDTOs = new List<GetComboPersonDTO>();

            using (var ctx = new AspodesDB())
            {
                var headquaterInst = from inst in ctx.Institutes where inst.Type == InstituteType.HEADQUATER select inst.InstituteId;
                var departAdminUserIds = from authorize in ctx.Authorizes where authorize.RoleId == 3 select authorize.UserId;
                var departAdminPersonIds = from user in ctx.Users where departAdminUserIds.Contains(user.UserId) select user.PersonId;
                var departAdminCandidate = from person in ctx.Persons.Where(p => p.Status == "C") where headquaterInst.Contains(person.InstituteId.Value) && !departAdminPersonIds.Contains(person.PersonId) select person;

                foreach (var person in departAdminCandidate)
                {
                    personDTOs.Add(Mapper.Map<GetComboPersonDTO>(person));
                }
            }
            return personDTOs;
        }

        /// <summary>
        /// 验证专家领域信息是否完整
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static bool ExpertFieldComplete(int personId)
        {

            using (var ctx = new AspodesDB())
            {
                var user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                if (user != null)
                {
                    return ctx.ExpertFields.Count(ef => ef.UserId == user.UserId) >= SystemConfig.ExpertFieldAmount;
                }
                return false;
            }

        }

        /// <summary>
        /// 完善校外专家的个人信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static void ExpertFieldCompletePartners(int personId)
        {
            using (var ctx = new AspodesDB())
            {
                User user = ctx.Users.FirstOrDefault(u => u.PersonId == personId);
                if (user == null)
                {
                    //增加登录账户
                    Person person = ctx.Persons.Where(c => c.PersonId == personId && c.Status == "C").FirstOrDefault();
                    if (person == null)
                    {
                        throw new OtherException("人员信息不存在");
                    }
                    user = Mapper.Map<User>(person);
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                }

                if (ctx.Authorizes.Any(a => a.UserId == user.UserId && a.RoleId == 5))
                {
                    throw new OtherException("该成员已是专家");
                }

                //初始化登录密码
                user = ctx.Users.First(u => u.PersonId == personId);
                user.Password = HashHelper.IntoMd5("123456");

                //判断研究领域是否满足，不足则增加研究领域
                int expertFieldsCount = ctx.ExpertFields.Count(ef => ef.UserId == user.UserId);
                if (expertFieldsCount < SystemConfig.ExpertFieldAmount)
                {
                    expertFieldsCount = SystemConfig.ExpertFieldAmount - expertFieldsCount;
                    for (int i = 0; i < expertFieldsCount; i++)
                    {
                        ctx.ExpertFields.Add(new ExpertField() { SubFieldId = "农学其他学科", UserId = user.UserId });
                    }
                }

                ctx.SaveChanges();
            }
        }


        /// <summary>
        /// 获取专家列表，按研究领域，单位筛选，分页
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="fieldId"></param>
        /// <param name="subFieldId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagingListDTO<GetExpertInfoDTO> GetPagingExpertList(int instId, string fieldId, string subFieldId, int page)
        {
            PagingListDTO<GetExpertInfoDTO> pagingList = new PagingListDTO<GetExpertInfoDTO>();

            using (var ctx = new AspodesDB())
            {
                List<string> subFields = new List<string>();
                if (fieldId == "all")
                {
                    subFields.AddRange(ctx.SubFields.Select(sf => sf.SubFieldName).ToList());
                }
                else if (subFieldId == "all")
                {
                    subFields.AddRange(ctx.SubFields.Where(sf => sf.ParentName == fieldId).Select(sf => sf.SubFieldName).ToList());
                }
                else
                {
                    subFields.Add(subFieldId);
                }

                var fieldExpertIds = from expertField in ctx.ExpertFields
                                     join authorize in ctx.Authorizes on expertField.UserId equals authorize.UserId
                                     where authorize.RoleId == 5 && subFields.Contains(expertField.SubFieldId)
                                     select expertField.UserId;

                //领域和单位筛选专家
                var expertList = ctx.Users.Include("ExpertFields")
                    .Where(c => fieldExpertIds.Contains(c.UserId))
                    .Where(u => instId == 0 || u.InstituteId == instId)
                    .OrderBy(e => e.Name);

                pagingList.NowPage = page <= 0 ? 1 : page;
                pagingList.TotalNum = expertList.Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ApplicationPageCount - 1) / SystemConfig.ApplicationPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;

                var query = expertList
                            .Skip((page - 1) * SystemConfig.ApplicationPageCount)
                            .Take(SystemConfig.ApplicationPageCount)
                            .ToList()
                            .Select(expert => new GetExpertInfoDTO
                            {
                                UserId = expert.UserId,
                                Name = expert.Name,
                                InstituteId = expert.InstituteId.Value,
                                InstituteName = expert.Institute.Name,
                                PersonId = expert.PersonId.Value,
                                FirstExpertFieldId = expert.ExpertFields.First().ExpertFieldId.Value,
                                FirstSubFieldId = expert.ExpertFields.First().SubFieldId,
                                SecondExpertFieldId = expert.ExpertFields.Last().ExpertFieldId.Value,
                                SecondSubFieldId = expert.ExpertFields.Last().SubFieldId
                            });

                pagingList.ItemDTOs = query.ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count;
            }
            return pagingList;
        }


        /// <summary>
        /// 系统管理员获取申请书分类
        /// </summary>
        /// <returns></returns>
        public List<SysSupportCategoryGetDTO> GetSupportCategorys()
        {
            List<SysSupportCategoryGetDTO> supportCategoryList = new List<SysSupportCategoryGetDTO>();
            SysSupportCategoryGetDTO supportCategory;

            using (var db = new AspodesDB())
            {
                var pjdata = db.ProjectTypes.OrderBy(c => c.Name);
                var scdata = db.SupportCategorys;

                foreach (var temp in pjdata)
                {
                    supportCategory = new SysSupportCategoryGetDTO();
                    supportCategory.Name = temp.Name;
                    supportCategory.SupportCategoryId = temp.ProjectTypeId.Value;
                    supportCategory.Categorys = scdata.Where(c => c.ProjectTypeId == temp.ProjectTypeId)
                        .OrderBy(c => c.Name)
                        .ToList();
                    supportCategoryList.Add(supportCategory);
                }
            }
            return supportCategoryList;
        }

        /// <summary>
        /// 管理员添加申请书分类
        /// </summary>
        /// <param name="supportCategoryData"></param>
        /// <returns></returns>
        public void GreateSupportCategorys(SysSupportCategoryAddDTO supportCategoryData)
        {

            using (var db = new AspodesDB())
            {
                if (supportCategoryData.SupportCategoryId == 0)
                {
                    //添加项目类别
                    ProjectType pj = new ProjectType();
                    pj.Name = supportCategoryData.Name;
                    pj.Enable = true;
                    db.ProjectTypes.Add(pj);
                }
                else
                {
                    //添加支持类别
                    if (db.ProjectTypes.Any(c => c.ProjectTypeId == supportCategoryData.SupportCategoryId))
                    {
                        //存在项目类别
                        SupportCategory sc = new SupportCategory();
                        sc.Name = supportCategoryData.Name;
                        sc.ProjectTypeId = supportCategoryData.SupportCategoryId;
                        db.SupportCategorys.Add(sc);
                    }
                    else
                    {
                        //不存在项目类别
                        throw new OtherException("不存在项目类别");
                    }
                }
                db.SaveChanges();
            }

        }

    }
}