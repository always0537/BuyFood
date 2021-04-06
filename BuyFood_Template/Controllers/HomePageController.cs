﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModel;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Home()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }
            return View();
        }

        [HttpPost]
        public JsonResult getSearchKey(string key,string Category, string Min,string Max,string BreakFast, string Lunch, string Dinner)
        {

            List<TProduct> list = new List<TProduct>();
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var result = db.TProducts.AsEnumerable().Select(n => n);
            double getMin = Convert.ToDouble(Min);
            double getMax = Convert.ToDouble(Max);
            int getBreakFast = 0;
            int getLunch = 0;
            int getDinner = 0;
            int CategoryID = db.TProductCategories.Where(n => n.CCategoryName == Category).Select(n => n.CProductCategoryId).FirstOrDefault();

            if (BreakFast == "早餐")
                getBreakFast = 1;
            if (Lunch == "午餐")
                getLunch = 1;
            if (Dinner == "晚餐")
                getDinner = 1;
            if (string.IsNullOrEmpty(BreakFast) && string.IsNullOrEmpty(Lunch) && string.IsNullOrEmpty(Dinner))
            {
                getBreakFast = 1;getLunch = 1;getDinner = 1;
            }

            if (string.IsNullOrEmpty(Min))
                getMin = 0;
            
            if (string.IsNullOrEmpty(Max))
                getMax = 10000;
            

            if (string.IsNullOrEmpty(key))
            {
                if (Category == "全部")
                {
                    result=db.TProducts.AsEnumerable().Where(m=> (double)m.CPrice >= getMin && (double)m.CPrice <= getMax );
                }
                else
                {
                    result = db.TProducts.AsEnumerable().Where(m => m.CCategoryId == CategoryID &&
                       (double)m.CPrice >= getMin && (double)m.CPrice <= getMax);

                }
            }

            else
            {

                if (Category=="全部")
                {
                    result=db.TProducts.AsEnumerable().Where(m=>m.CProductName.Any(n=>m.CProductName.Contains(key)) &&
                          (double)m.CPrice >= getMin && (double)m.CPrice <= getMax);
                }

                else
                {

                    result = db.TProducts.AsEnumerable().Where(m => m.CProductName.Any(n => m.CProductName.Contains(key)) &&
                      m.CCategoryId == CategoryID && (double)m.CPrice >= getMin && (double)m.CPrice <= getMax);

                }
            }
            //時段判斷
            var product = db.TProducts.AsQueryable();
            var predicate = PredicateBuilder.False<TProduct>();
            if (getBreakFast == 1)
                predicate = predicate.Or(n => n.CIsBreakFast == getBreakFast);
            if (getLunch == 1)
                predicate = predicate.Or(n => n.CIsLunch == getLunch);
            if (getDinner == 1)
                predicate = predicate.Or(n => n.CIsDinner == getDinner);
            var resultOfTime = result.AsQueryable().AsExpandable().Where(c => predicate.Invoke(c));
            //return Json(result.ToList());
            //return Json(result.Where(n=>n.CIsBreakFast==getBreakFast&&n.CIsLunch==getLunch&&n.CIsDinner==getDinner));
            return Json(resultOfTime.ToList());

        }


        public string getSearchBar()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var nameQuery = db.TProducts.Select(n => n.CProductName);
            List<string> Pnames = new List<string>();

            foreach(var n in nameQuery)
            {
                Pnames.Add(n);
            }
            

            return string.Join(',', Pnames);

        }

        public JsonResult getCategory()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            List<string> list = new List<string>();

            var Categories = db.TProductCategories.Select(n => n.CCategoryName);

            foreach(var CateNames in Categories)
            {
                list.Add(CateNames);
            }

            return Json(list);
        }



            public IActionResult 登入()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
            //{
            //    Random r = new Random();
            //    string code = r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString();
            //    HttpContext.Session.SetString(CDictionary.LOGIN_AUTHTICATION_CODE, code);
            //}

            //ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
            return View();
        }
        [HttpPost]
        public IActionResult 登入(CLoginViewModel loginMember)
        {
            //if (loginMember.txtCode == null)
            //{
            //    loginMember.txtCode = "";
            //}
            //if (!loginMember.txtCode.Equals(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
            //{
            //    ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
            //    return View();
            //}

            TMember CheckMember = (new 擺腹BuyFoodContext()).TMembers.FirstOrDefault(p => p.CEmail.Equals(loginMember.CEmail) && p.CPassword.Equals(loginMember.CPassword));

            if (CheckMember != null)
            {
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, CheckMember.CName);
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, CheckMember.CPicture);
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, CheckMember.CMemberId.ToString());

                return RedirectToAction("Home");
            }
            //ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
            return View();
        }

        public bool facebookLogin(string id, string name)
        {
            var test = id + name;

            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            //檢查是否用此帳號登入過

            var checkID = from n in db.TMembers
                          select n.CFacebookId;

            if (checkID.Any(n => n == id) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BCheckLogin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public JsonResult 輪播牆()
        {
            var run = (new 擺腹BuyFoodContext()).TActivities.Where(n => n.CStatus == 1 && n.CRank != 99).OrderBy(n => n.CRank).Select(n => n);

            return Json(run);
        }
        public JsonResult getNoodels()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getNoodels = db.TProducts.Where(n => n.CProductId == 10 || n.CProductId == 17 || n.CProductId == 58).Select(n => n).ToList();
            return Json(getNoodels);
        }
        public JsonResult getArroz()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getArroz = db.TProducts.Where(n => n.CProductId == 19 || n.CProductId == 20).Select(n => n).ToList();
            return Json(getArroz);
        }
        public JsonResult getDrinks()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getDrinks = db.TProducts.Where(n => n.CProductId == 5 || n.CProductId == 26 || n.CProductId==24).Select(n => n).ToList();
            return Json(getDrinks);
        }

        public JsonResult getFried()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getFried = db.TProducts.Where(n => n.CProductId == 14 || n.CProductId == 30).Select(n => n).ToList();
            return Json(getFried);
        }
        public JsonResult getActivity()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getActivity = db.TActivities.Select(n => n).OrderByDescending(n => n).Where(n => n.CStatus == 1).Take(3).ToList();
            return Json(getActivity);
        }
        public JsonResult get_categorysname()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = db.TProductCategories.Select(n => n);
            return Json(table);
        }
        public JsonResult get_many_products(int? id)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = db.TProducts.Where(n => n.CCategoryId == id).Select(n => n);
            List<TProduct> list = new List<TProduct>();

            foreach (var x in table)
            {
                list.Add(x);
            }
            return Json(list);
        }
        public JsonResult getLastProducts()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var LastProducts = db.TProducts.OrderByDescending(n => n.CProductId).Select(n => n).ToList();
            return Json(LastProducts);
        }
        public JsonResult getTopRatedProducts()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var topProducts = from tp in db.TOrderDetails
                              group tp by tp.CProductId into g
                              select new
                              {
                                  g.Key,
                                  AvgScore = g.Sum(n => n.CScores) / g.Count()
                              };

            var topRate = topProducts.OrderByDescending(n => n.AvgScore).Select(n => new
            {
                cProductId = n.Key,
                n.AvgScore
            });

            var idList = topProducts.OrderByDescending(n => n.AvgScore).Select(n=>n.Key).ToList().Take(6);
            List<TProduct> ProductList = new List<TProduct>();
            foreach(var p in idList)
            {
                ProductList.Add(db.TProducts.Where(n => n.CProductId == p).Select(n => n).FirstOrDefault());
            }


            return Json(ProductList);
        }
        public JsonResult getReviewProducts()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var review = from od in db.TOrderDetails
                         where od.CReview != null
                         group od by od.CProductId into g
                         select new
                         {
                             g.Key,
                             ReviewCounts = g.Count()
                         };

            var reviewOrderBy = review.OrderByDescending(n => n.ReviewCounts).Select(n => new
            {
                cProductId = n.Key,
                n.ReviewCounts
            });
            var idList = review.OrderByDescending(n => n.ReviewCounts).Select(n => n.Key).ToList().Take(6);
            List<TProduct> ListProducts = new List<TProduct>();
            foreach(var p in idList)
            {
                ListProducts.Add(db.TProducts.Where(n => n.CProductId == p).Select(n => n).FirstOrDefault());
            }

            return Json(ListProducts.ToList());
        }


    }
}
