using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class cOrderDetailsController : Controller
    {

        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

        public IActionResult allorderIdList()
        {
            return View();
        }
        //產品販售狀況數量
        public JsonResult orderSelectStateNum()
        {
            int order運送中數 = db.TOrders.Where(n => n.COrderStatusId == 1).Count();
            int order到達數 = db.TOrders.Where(n => n.COrderStatusId == 4).Count();
            int order取消數 = db.TOrders.Where(n => n.COrderStatusId == 3).Count();
            int order完成數 = db.TOrders.Where(n => n.COrderStatusId == 2).Count();
            int order訂單總數 = db.TOrders.Where(n => n.COrderStatusId == 1 || n.COrderStatusId == 2||n.COrderStatusId==3||n.COrderStatusId==4).Count();


            var orderstate數 = new { order到達數 = order到達數, order取消數 = order取消數, order完成數 = order完成數, order運送中數 = order運送中數, order訂單總數= order訂單總數 };
            return Json(orderstate數);
        }

        public JsonResult orderIdListAndTotalPrice(string str)
        {

            if (str == "order到達") {
                var table = from o in db.TOrders
                            join od in db.TOrderDetails on o.COrderId equals od.COrderId
                            where o.COrderStatusId ==4
                            group od by od.COrderId into g
                            orderby g.Key descending
                            select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };
                return Json(table.ToList());
            }
            else if (str=="orfer取消") {

                var table = from o in db.TOrders
                            join od in db.TOrderDetails on o.COrderId equals od.COrderId
                            where o.COrderStatusId==3
                            group od by od.COrderId into g
                            orderby g.Key descending
                            select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };

                return Json(table.ToList());
            }
            else if(str=="order完成")
            {
                var table = from o in db.TOrders
                            join od in db.TOrderDetails on o.COrderId equals od.COrderId
                            where o.COrderStatusId == 2
                            group od by od.COrderId into g
                            orderby g.Key descending
                            select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };
                return Json(table.ToList());
            }
            else if (str== "order運送")
            {
                var table = from o in db.TOrders
                            join od in db.TOrderDetails on o.COrderId equals od.COrderId
                            where o.COrderStatusId==1
                            group od by od.COrderId into g
                            orderby g.Key descending
                            select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };
                return Json(table.ToList());
            }
            else
            {
                var table = from o in db.TOrders
                            join od in db.TOrderDetails on o.COrderId equals od.COrderId
                            group od by od.COrderId into g
                            orderby g.Key descending
                            select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };
                return Json(table.ToList());
            }
        }
        //public JsonResult orderIdListAndTotalPrice()
        //{

        //    var table = from o in db.TOrders
        //                       join od in db.TOrderDetails on o.COrderId equals od.COrderId
        //                       group od by od.COrderId into g
        //                       orderby g.Key descending
        //                select new { COrderId = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };


        //    return Json(table.ToList());
        //}

        public JsonResult orderIdList(string str)
        {

            if(str== "order到達")
            {
                var table = from allorid in db.TOrders
                            join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
                            join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
                            join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
                            join orderCupon in db.TCupons on allorid.CCuponId equals orderCupon.CCuponId
                            join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                            where allorid.COrderStatusId == 4
                            orderby allorid.COrderId descending

                            select new
                            {
                                allorid.COrderId,
                                allorid.COrderDate,
                                allorid.CMemberId,
                                orderCuponPrice.CCutPrice,
                                cname.CName,
                                oderst.COrderStatusName
                            };
                //消除orderdetail重複的orderid
                var distinctID = table.Distinct().OrderByDescending(n => n.COrderId);
                //return Json(table.ToList());
                return Json(distinctID.ToList());
            }
            else if (str == "orfer取消")
            {
                var table = from allorid in db.TOrders
                            join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
                            join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
                            join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
                            join orderCupon in db.TCupons on allorid.CCuponId equals orderCupon.CCuponId
                            join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                            where allorid.COrderStatusId == 3
                            orderby allorid.COrderId descending

                            select new
                            {
                                allorid.COrderId,
                                allorid.COrderDate,
                                allorid.CMemberId,
                                cname.CName,
                                orderCuponPrice.CCutPrice,
                                oderst.COrderStatusName
                            };
                //消除orderdetail重複的orderid
                var distinctID = table.Distinct().OrderByDescending(n => n.COrderId);

                return Json(distinctID.ToList());
            }
            else if (str == "order完成")
            {
                var table = from allorid in db.TOrders
                            join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
                            join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
                            join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
                            join orderCupon in db.TCupons on allorid.CCuponId equals orderCupon.CCuponId
                            join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                            where allorid.COrderStatusId == 2
                            orderby allorid.COrderId descending

                            select new
                            {
                                allorid.COrderId,
                                allorid.COrderDate,
                                allorid.CMemberId,
                                cname.CName,
                                orderCuponPrice.CCutPrice,
                                oderst.COrderStatusName
                            };
                //消除orderdetail重複的orderid
                var distinctID = table.Distinct().OrderByDescending(n => n.COrderId);

                return Json(distinctID.ToList());
            }
            else if (str== "order運送")
            {
                var table = from allorid in db.TOrders
                            join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
                            join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
                            join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
                            join orderCupon in db.TCupons on allorid.CCuponId equals orderCupon.CCuponId
                            join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                            where allorid.COrderStatusId==1
                            orderby allorid.COrderId descending

                            select new
                            {
                                allorid.COrderId,
                                allorid.COrderDate,
                                allorid.CMemberId,
                                cname.CName,
                                orderCuponPrice.CCutPrice,
                                oderst.COrderStatusName
                            };
                //消除orderdetail重複的orderid
                var distinctID = table.Distinct().OrderByDescending(n => n.COrderId);

                return Json(distinctID.ToList());
            }
            else 
            {
                var table = from allorid in db.TOrders
                            join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
                            join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
                            join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
                            join orderCupon in db.TCupons on allorid.CCuponId equals orderCupon.CCuponId
                            join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                            orderby allorid.COrderId descending

                            select new
                            {
                                allorid.COrderId,
                                allorid.COrderDate,
                                allorid.CMemberId,
                                cname.CName,
                                orderCuponPrice.CCutPrice,
                                oderst.COrderStatusName
                            };
                //消除orderdetail重複的orderid
                var distinctID = table.Distinct().OrderByDescending(n => n.COrderId);

                return Json(distinctID.ToList());
            }


        }


        //public JsonResult orderIdList()
        //{
        //    var table = from allorid in db.TOrders
        //                join cname in db.TMembers on allorid.CMemberId equals cname.CMemberId
        //                join oderst in db.TOrderStatuses on allorid.COrderStatusId equals oderst.COrderStatusId
        //                join orderdetail in db.TOrderDetails on allorid.COrderId equals orderdetail.COrderId
        //                orderby allorid.COrderId descending

        //                select new
        //                {
        //                    allorid.COrderId,
        //                    allorid.COrderDate,
        //                    allorid.CMemberId,
        //                    cname.CName,
        //                    oderst.COrderStatusName
        //                };
        //    //消除orderdetail重複的orderid
        //    var distinctID = table.Distinct().OrderByDescending(n=>n.COrderId);

        //    return Json(distinctID.ToList());
        //}



        public JsonResult cOrderDetailsJson(int? id)
        {
            var table = from orderdetail in db.TOrderDetails
                        where orderdetail.COrderId==id
                        join order in db.TOrders on orderdetail.COrderId equals order.COrderId
                        join member in db.TMembers on order.CMemberId equals member.CMemberId
                        join prname in db.TProducts on orderdetail.CProductId equals prname.CProductId
                        join paytype in db.TPayTypes on order.CPayTypeId equals paytype.CPayTypeId
                        join orderstauts in db.TOrderStatuses on order.COrderStatusId equals orderstauts.COrderStatusId
                        join orderCupon in db.TCupons on order.CCuponId equals orderCupon.CCuponId
                        join orderCuponPrice in db.TCuponCategories on orderCupon.CCuponCategoryId equals orderCuponPrice.CCuponCategoryId
                        select new
                        {
                            orderdetail.COrderId,
                            orderdetail.COrderDetailId,
                            order.COrderDate,
                            member.CName,
                            prname.CProductName,
                            orderdetail.CPriceAtTheTime,
                            orderdetail.CQuantity,
                            orderstauts.COrderStatusId,
                            orderstauts.COrderStatusName,
                            orderCuponPrice.CCutPrice,
                            paytype.CPayTypeName
                        };

            return Json(table.ToList());
        }
        public void orderCancel(int? id)
        {

            if (id != null)
            {

                TOrder l_order被修改 = db.TOrders.FirstOrDefault(n => n.COrderId == id);
                TMember l_member被修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == l_order被修改.CMemberId);
                TCupon l_cupon被修改 = db.TCupons.FirstOrDefault(n => n.CCuponId == l_order被修改.CCuponId);
                TCuponCategory p折扣金額 = db.TCuponCategories.FirstOrDefault(n => n.CCuponCategoryId == l_cupon被修改.CCuponCategoryId);


                IEnumerable<TOrderDetail> table = null;
                table = db.TOrderDetails.Where(n => n.COrderId == id);

                List<TOrderDetail> list = new List<TOrderDetail>();
                foreach (var item in table)
                {
                    list.Add(item);
                }
                decimal p總價 = 0;
                decimal p價格 = 0;
                int p數量 = 0;
                int pID = 0;
                for (int i = 0; i < list.Count(); i++)
                {
                    //消費金額
                    p價格 = list[i].CPriceAtTheTime.Value;
                    p數量 = list[i].CQuantity.Value;
                    p總價 += (p價格 * p數量);

                    //產品庫存
                    pID = list[i].CProductId;
                    TProduct l_product被修改 = db.TProducts.FirstOrDefault(n => n.CProductId == pID);
                    l_product被修改.CQuantity += p數量;


                }

                if (l_order被修改.CPayTypeId.Value == 1)
                {
                    //無折扣
                    if (l_order被修改.CCuponId.Value == 1)
                    {
                        l_order被修改.COrderStatusId = 3;
                        l_member被修改.CDeposit += p總價;
                    }
                    //有折扣
                    else
                    {
                        l_order被修改.COrderStatusId = 3;
                        l_member被修改.CDeposit += (p總價 - p折扣金額.CCutPrice.Value);
                        l_cupon被修改.CBeUsed = 0;

                    }
                }
                else
                {
                    l_order被修改.COrderStatusId = 3;
                }
                db.SaveChanges();
            }


        }
        public IActionResult ordercomplete(int? id)
        {
            TOrder l_order被修改 = db.TOrders.FirstOrDefault(n => n.COrderId == id);
            if (id != null)
            {
                l_order被修改.COrderStatusId = 2;
                db.SaveChanges();
            }
            return View("list");
        }

    }
}

