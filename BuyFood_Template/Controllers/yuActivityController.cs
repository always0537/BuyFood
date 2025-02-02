﻿using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuActivityController : Controller
    {
        [Obsolete]
        private IHostingEnvironment iv_host;

        [Obsolete]
        public yuActivityController(IHostingEnvironment p)
        {
            iv_host = p;
        }

        public IActionResult ActivityList()
        {
            return View();
        }
        public JsonResult ActivityupdateTOP()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            int All = db.TActivities.Select(n => n).Count();
            int Going = db.TActivities.Where(n => n.CStatus == 1).Select(n => n).Count();
            int close = db.TActivities.Where(n => n.CStatus == 0).Select(n => n).Count();
            int mouth = db.TActivities.Where(n => n.CTime.Contains(DateTime.Now.Year.ToString()) && n.CTime.Contains(DateTime.Now.Month.ToString())).Select(n => n).Count();
            int year = db.TActivities.Where(n => n.CTime.Contains(DateTime.Now.Year.ToString())).Select(n => n).Count();

            var my_num = new { All= All, Going= Going, close= close, mouth= mouth, year= year };
            return Json(my_num);
        }
        public JsonResult ActivityListJson(string str)
        {
            List<yuActivityViewModel> activitie = new List<yuActivityViewModel>();
            IEnumerable<TActivity> table = null;

            if (str == "All")
            {
                table = from c in (new 擺腹BuyFoodContext()).TActivities
                        orderby c.CActivityId descending
                        select c;
            }
            else if (str == "Going")
            {
                table = from c in (new 擺腹BuyFoodContext()).TActivities where c.CStatus == 1 orderby c.CActivityId descending select c;
            }
            else if (str == "Close")
            {
                table = from c in (new 擺腹BuyFoodContext()).TActivities where c.CStatus == 0 orderby c.CActivityId descending select c;
            }

            if (table != null)
            {
                foreach (TActivity x in table)
                {
                    activitie.Add(new yuActivityViewModel(x));
                }
            }

            return Json(activitie);
        }
        public IActionResult ActivityCreat()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ActivityCreat(yuActivityViewModel p)
        {
            string pohotoname = Guid.NewGuid().ToString() + ".jpg";

            using (var photo = new FileStream(iv_host.WebRootPath + @"\imgs\" + pohotoname, FileMode.Create))
            {
                p.img.CopyTo(photo);
            }
            
            p.CPicture = @"../imgs/" + pohotoname;
            p.CTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            db.TActivities.Add(p.Activity);
            db.SaveChanges();
            return RedirectToAction("ActivityList");
        }
        public IActionResult ActionEdit(int? id)
        {
            if (id != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                TActivity table = db.TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    return View(new yuActivityViewModel(table));
                }
            }
            return RedirectToAction("ActivityList");
        }
        [HttpPost]
        public IActionResult ActionEdit(yuActivityViewModel p)
        {
            if (p != null)
            {
                if (p.img != null)
                {
                    string pohotoname = Guid.NewGuid().ToString() + ".jpg";

                    using (var photo = new FileStream(iv_host.WebRootPath + @"\imgs\" + pohotoname, FileMode.Create))
                    {
                        p.img.CopyTo(photo);
                    }
                    p.CPicture = @"../imgs/" + pohotoname;
                }
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                TActivity table = db.TActivities.FirstOrDefault(a => a.CActivityId == p.CActivityId);
                if (table != null)
                {
                    table.CActivityName = p.CActivityName;
                    table.CDescription = p.CDescription;
                    table.CLink = p.CLink;
                    table.CStatus = p.CStatus;
                    if (p.CPicture != null)
                        table.CPicture = p.CPicture;

                    db.SaveChanges();
                }
            }
            return RedirectToAction("ActivityList");
        }
        public void ActionDeletealone(int? id)
        {
            if (id != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                var table = db.TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    db.Remove(table);
                }
                db.SaveChanges();
            }

        }
        public void ActionDelete([FromBody] List<yupushstrViewModel> Array)
        {
            if (Array != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                for (int i = 0; i < Array.Count; i++)
                {
                    TActivity table = db.TActivities.FirstOrDefault(a => a.CActivityId.ToString() == Array[i].strmember);
                    if (table != null)
                    {
                        if (table.CPicture != null)
                        {
                            FileInfo file = new FileInfo(table.CPicture);
                            if (file.Exists)
                                file.Delete();
                        }
                        db.Remove(table);
                        db.SaveChanges();
                    }
                }
            }
        }
        public IActionResult ActionUpDelete(int? id)
        {
            if (id != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                TActivity table = db.TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    if (table.CStatus == 1)
                    {
                        table.CStatus = 0;
                        table.CRank = 99;
                    }
                    else
                        table.CStatus = 1;
                }
                db.SaveChanges();
            }
            return RedirectToAction("ActivityList");
        }
        public IActionResult goto_where_page_todo(int todopage)
        {
            switch (todopage)
            {
                case 1: //儲值
                    {
                        TempData[CDictionary.REDIRECT_FROM_WHERE] = 1;
                        return RedirectToAction("MemberCenter", "Member");
                    }
                case 2:  //套餐
                    {
                        TempData[CDictionary.REDIRECT_FROM_WHERE] = 2;
                        return RedirectToAction("MemberCenter", "Member");
                    }
                case 3:  //點餐
                    {
                       TempData[CDictionary.REDIRECT_FROM_WHERE] = 4;
                        return RedirectToAction("Home", "HomePage");
                    }
                case 4:  //註冊
                    {
                         return RedirectToAction("Create", "Customer");
                    }
                case 5:  //活動
                    {
                        return RedirectToAction("ShowActivity", "Activity");
                    }
            }

            return RedirectToAction("Home", "HomePage");
        }
    }
}
