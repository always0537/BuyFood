using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BuyFood_Template.Controllers
{
    public class ProductDetailController : Controller
    {

       

        public IActionResult ProductData(int? id)  //在商品頁面所選擇的產品顯示出來
        {

            //var product = db.TProducts.Where(x => x.CProductId == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }
            var product = new 擺腹BuyFoodContext().TProducts.Where(x => x.CProductId == id).FirstOrDefault();
            return View(product);

            //int? A = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Count();
            //int? B = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1).Sum(n => n.CScores);
            //ViewBag.Bcount = A;

            //try
            //{
            //    ViewBag.Bagv = B / A;
            //}
            //catch
            //{
            //    ViewBag.Bagv = 0;
            //}

            //return View(product);
        } 
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public JsonResult Boardget(int id)
        {
            var table = db.TOrderDetails.Where(n => n.CFeedBackStatus == 1 && n.CProductId == id).Select(n => n);


            return Json(table);
        }
        public JsonResult startJson(int id)
        {
            //抓出當前CProductId的留言數量及平均值
            int? Count = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1&&n.CScores!=null).Count();
            int? Sum = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores != null).Sum(n => n.CScores);

            int average = Count <= 0 ? 0 : (int)(Sum / Count);

            var result = db.TOrderDetails.Select(n => new { bcount = Count, bavg = average });

            return Json(result.FirstOrDefault());
        }
        public JsonResult productBoards(int id, int star)   //抓出留言內容
        {

            if (star == 99)
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1&&n.CScores!=null).OrderByDescending(n => n.CScores)
                    .Select(n => new
                    {
                        membersphoto = n.COrder.CMember.CPicture,
                        membername = n.COrder.CMember.CName,
                        memberproduct = n.CProduct.CProductName,
                        memberstar = n.CScores,
                        memberreview = n.CReview,
                    });
                return Json(table);
            }
            else if (star == 0)
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1&&n.CReview!=null ).OrderByDescending(n => n.CScores)
                .Select(n => new
                {
                  membersphoto = n.COrder.CMember.CPicture,
                  membername = n.COrder.CMember.CName,
                  memberproduct = n.CProduct.CProductName,
                  memberstar = n.CScores,
                 memberreview = n.CReview,
                });
                return Json(table);
            }
            else
            {
                var table = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == star).OrderByDescending(n => n.CScores)
            .Select(n => new
            {
                membersphoto = n.COrder.CMember.CPicture,
                membername = n.COrder.CMember.CName,
                memberproduct = n.CProduct.CProductName,
                memberstar = n.CScores,
                memberreview = n.CReview,
            });
                return Json(table);
            }
        }
        public JsonResult productstar(int id)  //動態抓出留言數量
        {
            int? all = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1&&n.CScores!=null).Count();
            int? one = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 1).Count();
            int? two = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 2).Count();
            int? three = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 3).Count();
            int? four = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 4).Count();
            int? five = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores == 5).Count();
            int? hasreview = db.TOrderDetails.Where(n => n.CProductId == id && n.CFeedBackStatus == 1 && n.CScores != null).Select(n => n.CReview).Count();

            var table = db.TOrderDetails
               .Select(n => new
               {
                   all = all,
                   one = one,
                   two = two,
                   three = three,
                   four = four,
                   five = five,
                   hasreview = hasreview,
               });
            return Json(table.FirstOrDefault());
        }
        public JsonResult smelltit(int id)  //動態抓出該品項詳細
        {
            var table = db.TProducts.Where(n => n.CProductId == id).FirstOrDefault();
            var tablea=db.TProducts.Where(n => n.CProductId == id).Select(n=>n.CCategory.CCategoryName).ToList();
            var x = db.TProducts.Select(n => new { table, tablea }).FirstOrDefault();
            return Json(x);
        }
        public JsonResult gethotproduct(int id)  //相關風格商品
        {
            var table = from x in db.TProducts
                        where x.CProductId == id
                        join a in db.TProducts
                        on x.CProductTagId equals a.CProductTagId
                        orderby a.CIsOnSaleId
                        select new 
                        {
                            product=a,
                            coun = a.TOrderDetails.Count(x => x.CFeedBackStatus == 1 && x.CScores != null),
                            sum = a.TOrderDetails.Where(x => x.CFeedBackStatus == 1 && x.CScores != null).Sum(m => m.CScores)
                        };
            return Json(table);
        }
        public JsonResult getProductsByCategory(string id, string br, string lu, string di)
        {
            var data = (new 擺腹BuyFoodContext()).TProducts.Where(n => n.CCategoryId == int.Parse(id) && n.CIsOnSaleId != 3);
            return Json(data);
        }
    }

}
