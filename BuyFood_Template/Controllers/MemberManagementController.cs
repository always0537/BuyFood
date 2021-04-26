using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuyFood_Template.ViewModels;
using BuyFood_Template.ViewModel;
using Microsoft.AspNetCore.Hosting; //圖片
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using ClosedXML.Excel;


namespace BuyFood_Template.Controllers
{
    public class MemberManagementController : Controller
    {
        private 擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult List()
        {
            ViewBag.All = db.TMembers.Where(n=>n.CMemberId!=16).Select(n => n).Count();
            ViewBag.male = db.TMembers.Where(n => n.CGender == "男").Select(n => n).Count();
            ViewBag.female = db.TMembers.Where(n => n.CGender == "女").Select(n => n).Count();
            ViewBag.deposit = db.TMembers.Where(n => n.CDeposit > 0).Select(n => n).Count();
            ViewBag.Undeposited = db.TMembers.Where(n => n.CDeposit == 0).Select(n => n).Count();
            return View();
        }

        public JsonResult jsonList(string str)
        {       
            if (str == "All")
            {
                var table = from c in db.TMembers
                            orderby c.CMemberId descending
                            where c.CMemberId !=16
                            select new
                            {
                                c.CMemberId,
                                c.CPicture,
                                c.CName,
                                c.CGender,
                                cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                cAge = DateTime.Now.Year-c.CAge.Year,
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                                c.CFreezeCount
                            };
                return Json(table);
            }
            else if (str == "male")
            {
                var table = from c in db.TMembers where c.CGender == "男"
                            select new
                            {
                                c.CMemberId,
                                c.CPicture,
                                c.CName,
                                c.CGender,
                                cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                cAge = DateTime.Now.Year - c.CAge.Year,
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                                c.CFreezeCount
                            };
                return Json(table);
            }
            else if(str== "female")
            {
                var table  = from c in db.TMembers where c.CGender == "女"
                             select new
                             {
                                 c.CMemberId,
                                 c.CPicture,
                                 c.CName,
                                 c.CGender,
                                 cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                 cAge = DateTime.Now.Year - c.CAge.Year,
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                                 c.CFreezeCount
                             };
                return Json(table);
            }
            else if(str== "deposit")
            {
                var table  = from c in db.TMembers where c.CDeposit > 0
                             select new
                             {
                                 c.CMemberId,
                                 c.CPicture,
                                 c.CName,
                                 c.CGender,
                                 cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                 cAge = DateTime.Now.Year - c.CAge.Year,
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                                 c.CFreezeCount
                             };
                return Json(table);
            }
            else if(str== "Undeposited")
            {
               var table = from c in db.TMembers where c.CDeposit == 0
                           select new
                           {
                               c.CMemberId,
                               c.CPicture,
                               c.CName,
                               c.CGender,
                               cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                               cAge = DateTime.Now.Year - c.CAge.Year,
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                               c.CFreezeCount
                           };
                return Json(table);
            }
            else
            {
              var  table = from c in db.TMembers
                        where c.CName.Contains(str)   //???????
                           select new
                           {
                               c.CMemberId,
                               c.CPicture,
                               c.CName,
                               c.CGender,
                               cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                               cAge = DateTime.Now.Year - c.CAge.Year,
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                               c.CFreezeCount
                           };
                return Json(table);
            }          
        }

        //以上新增圖片
        private IHostingEnvironment iv_host;
        public MemberManagementController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MemberManagementViewModel newMember)
        {
            string photoName = Guid.NewGuid().ToString() + ".jpg";
            if (newMember.image != null)
            {
                using (var photo = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoName, FileMode.Create))
                {
                    newMember.image.CopyTo(photo);
                }
            }            
            newMember.CPicture = @"/MemberPhoto/" + photoName;

            db.TMembers.Add(newMember.member);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {           
            if (id != null)
            {                
                TMember table = db.TMembers.FirstOrDefault(a => a.CMemberId == id);
                if (table != null)
                {
                    return View(new MemberManagementViewModel(table));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(MemberManagementViewModel p)
        {          
            
            if (p.image != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                using (var photo = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoName, FileMode.Create))
                {
                    p.image.CopyTo(photo);
                } 
                p.CPicture = @"/MemberPhoto/" + photoName;
            }
           

            if (p != null)
            {                       
                TMember table = db.TMembers.FirstOrDefault(t => t.CMemberId == p.CMemberId);
                if (table != null)
                {
                    table.CName = p.CName;
                    table.CEmail = p.CEmail;
                    table.CPhone = p.CPhone;
                    table.CGender = p.CGender;
                    table.CAddress = p.CAddress;
                    table.CBlackList = p.CBlackList;
                    table.CAge = p.CAge;
                    table.CPicture = p.CPicture==null? table.CPicture:p.CPicture;  //←要存改照片的話就解開
                    //table.CRegisteredTime = DateTime.Now; //加註冊時間進資料庫
                    db.SaveChanges();
                }
            }            
            return RedirectToAction("List");
        }        

        public IActionResult Delete(int? id)
        {        
            var result = db.TMembers.Where(m => m.CMemberId == id).FirstOrDefault();
            db.TMembers.Remove(result);
            db.SaveChanges();          
            return RedirectToAction("List");
        }
        
        public JsonResult detail(int? id)
        {                      
            var table = from o in db.TOrders                         
                        join m in db.TMembers on o.CMemberId equals m.CMemberId
                        where m.CMemberId==id
                        select new {cOrderId=o.COrderId,CArrivedAddress = o.CArrivedAddress, cOrderDate = o.COrderDate, cTransportMinute = o.CTransportMinute,
                            cName = m.CName};
          
            return Json(table);
        }

        public bool changeblacklist(int? id)
        {
            TMember l_黑名單修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == id);
            bool 狀態 = false;
            if (l_黑名單修改 != null)
            {
                if (l_黑名單修改.CBlackList == 1)
                {
                    l_黑名單修改.CBlackList = 0;                   
                }
                else
                {
                    l_黑名單修改.CBlackList = 1;
                    狀態 = true;
                }
            }
            db.SaveChanges();
            return 狀態;
        }
        public bool changefreeze(int? id)
        {
            TMember l_凍結修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == id);
            bool 狀態 = false;
            if (l_凍結修改!=null)
            {
                if (l_凍結修改.CFreezeCount==4)
                {
                    l_凍結修改.CFreezeCount = 0;
                }
                else
                {
                    l_凍結修改.CFreezeCount = 4;
                    狀態 = true;
                }                    
            }
            db.SaveChanges();
            return 狀態;
        }

        public IActionResult downloadExcelDocument()
        {
            var table = from m in (new 擺腹BuyFoodContext()).TMembers where m.CMemberId!=16
                        select m;
            List<MemberManagementViewModel> exldata = new List<MemberManagementViewModel>();
            foreach (TMember t in table)
            {
                exldata.Add(new MemberManagementViewModel(t));
            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "會員資料表.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("會員列表");
                    worksheet.Cell(1, 1).Value = "編號";
                    worksheet.Cell(1, 2).Value = "姓名";
                    worksheet.Cell(1, 3).Value = "姓別";
                    worksheet.Cell(1, 4).Value = "E-mail帳號";
                    worksheet.Cell(1, 5).Value = "生日";
                    worksheet.Cell(1, 6).Value = "電話";
                    worksheet.Cell(1, 7).Value = "地址";
                    worksheet.Cell(1, 8).Value = "儲值餘額";
                    for (int index = 1; index <= exldata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = exldata[index - 1].CMemberId;
                        worksheet.Cell(index + 1, 2).Value = exldata[index - 1].CName;
                        worksheet.Cell(index + 1, 3).Value = exldata[index - 1].CGender;
                        worksheet.Cell(index + 1, 4).Value = exldata[index - 1].CEmail;
                        worksheet.Cell(index + 1, 5).Value = exldata[index - 1].CAge;
                        worksheet.Cell(index + 1, 6).Value = exldata[index - 1].CPhone;
                        worksheet.Cell(index + 1, 7).Value = exldata[index - 1].CAddress;
                        worksheet.Cell(index + 1, 8).Value = exldata[index - 1].CDeposit;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);// error()
            }
        }

        [HttpPost]
        public void sendEmail(string mailtoAddress, string mailtoName, string subject, string body)
        {
            (new ShareFunction()).sendGrid(mailtoAddress, mailtoName, subject, body);         
        }

        //public void test123()
        //{
        //    (new ShareFunction()).sendGrid("s736828@gmail.com", "小亮", "寄信測試", "testBody");
        //}

    }
}
