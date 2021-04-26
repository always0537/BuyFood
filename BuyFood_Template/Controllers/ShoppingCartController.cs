using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuyFood_Template.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult CurrentCartItem()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }


            if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                return RedirectToAction("登入", "HomePage");
            }
            return View();
        }
        //抓取對應商品的庫存數量
        [HttpPost]
        public JsonResult CurrentItemCount([FromBody] List<int> CartItemID)
        {
            List<CartProductCheckStore> quantityList = new List<CartProductCheckStore>();
            foreach(var i in CartItemID)
            {
                var result = new 擺腹BuyFoodContext().TProducts.Where(x => x.CProductId == i).FirstOrDefault();
                CartProductCheckStore CartItem = new CartProductCheckStore()
                {
                    cProductId = result.CProductId,
                    quantityInCart = (int)result.CQuantity,
                    finishTime = (int)result.CFinishedTime
                };
                quantityList.Add(CartItem);
            }
            return Json(quantityList);
        }
        //檢查優惠券是否可使用
        [HttpPost]
        public JsonResult CheckCouponCode([FromBody] string Code)
        {
            擺腹BuyFoodContext BuyFoodDB = new 擺腹BuyFoodContext();
            var result = (from i in BuyFoodDB.TCupons
                          join k in BuyFoodDB.TCuponCategories
                          on i.CCuponCategoryId equals k.CCuponCategoryId
                          where i.CDiscountCode == Code && i.CBeUsed == 0
                          select new CouponProperty()
                          {
                              CCuponId = i.CCuponId,
                              CCutPrice = (decimal)k.CCutPrice,
                          }).FirstOrDefault();
            if (result == null)
            {
                return Json("0");
            }
            else
            {
                return Json(result);
            }
        }
        //搜尋該會員可以使用的優惠券
        [HttpPost]
        public JsonResult SearchCouponCanUse([FromBody] int MemberID)
        {
            var result = from i in new 擺腹BuyFoodContext().TCupons
                         where i.CMenberId == MemberID && i.CBeUsed == 0 && i.CValidDate >= DateTime.Today
                         select new
                         {
                             CouponName = i.CCuponCategory.CategoryName,
                             CouponCode = i.CDiscountCode
                         };
            if (result == null)
            {
                return Json("0");
            }
            return Json(result);
        }
        //搜尋該會員最常購買的商品
        [HttpPost]
        public JsonResult GetMemberFavoriteItem([FromBody] int MemberID)
        {
            DateTime TimeNow = DateTime.Now;

            擺腹BuyFoodContext BuyFoodDB = new 擺腹BuyFoodContext();
            var result = from i in BuyFoodDB.TOrderDetails
                         join x in BuyFoodDB.TProducts
                         on i.CProductId equals x.CProductId
                         where i.COrder.CMemberId == MemberID 
                         select new
                         {
                             i.CProductId,
                             x.CProductTagId
                         };
            //依產品風格統計曾經購買最多次的商品
            var GroupResult = (from u in result
                               group u by u.CProductTagId into g
                               orderby g.Count() descending
                               select new
                               {
                                   g.Key,
                                   OrderCount = g.Count(),
                               }).Take(1).FirstOrDefault();
            if(GroupResult == null)
            {
                var HotItemList = HttpContext.Session.GetObject<List<TProduct>>("TopItem");
                return Json(HotItemList.Take(5));
            }
            //將該會員購買最多次的商品風格作為選擇條件、隨機選取出來 
            var FavorItem = BuyFoodDB.TProducts.Where(x => x.CProductTagId == GroupResult.Key && x.CIsOnSaleId == 1 && x.CIsBreakFast == 1 && x.CIsLunch == 1).
                OrderBy(x => Guid.NewGuid()).Select(x => x).ToList().Take(5);
            return Json(FavorItem);
        }

    }
}
