using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http;

namespace BuyFood_Template.Controllers
{

    public class ViewModelForOPay
    {
        public string MerchantTradeNo { get; set; }
        public string StoreID { get; set; }
        public int RtnCode { get; set; } 
        public int TradeAmt { get; set; }
    }
    public class DepositsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void saveDepositResult(ViewModelForOPay returnData)
        {
            DateTime now = DateTime.Now;
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            TMember changeTarget = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(returnData.StoreID));
            
            if (returnData.RtnCode == 1)
            {
                TDeposit result = new TDeposit
                {
                    CMemberId = int.Parse(returnData.StoreID),
                    CDepositTime = now,
                    CDepositAmount = returnData.TradeAmt,
                    CDepositRecordNo = returnData.MerchantTradeNo
                };
                dbcontext.TDeposits.Add(result);
                changeTarget.CDeposit += returnData.TradeAmt;

                int couponCategory=0;
                switch (returnData.TradeAmt)
                {
                    case 1000:
                        couponCategory = 2;
                        break;
                    case 2000:
                        couponCategory = 4;
                        break;
                    case 5000:
                        couponCategory = 7;
                        break;
                    default:
                        break;
                }
                if (couponCategory != 0)
                {
                    string dsCode = "";
                    while (dsCode == "")
                    {
                        bool check = false;
                        string newCode = (new ShareFunction()).產生亂數(6);
                        var data = dbcontext.TCupons;
                        foreach(var item in data)
                        {
                            if (item.CDiscountCode == newCode)
                            {
                                check = true;
                                break;
                            }
                        }
                        if (!check) dsCode = newCode;
                    }
                    TCupon newCoupon = new TCupon
                    {
                        CCuponCategoryId = couponCategory,
                        CMenberId = int.Parse(returnData.StoreID),
                        CDiscountCode = dsCode,
                        CValidDate = now.AddDays(60),
                        CReceivedTime = now
                    };
                    dbcontext.TCupons.Add(newCoupon);
                }
                dbcontext.SaveChanges();
            }
            string EmailContent = returnData.RtnCode == 1 ?
                $"已成功於{now.ToString("yyyy/MM/dd")}加值共{returnData.TradeAmt}擺腹幣" :
                $"加值失敗，請重新加值並確認付款內容。";
            (new ShareFunction()).sendGrid(changeTarget.CEmail, changeTarget.CName, "通知-加值結果", EmailContent);
        }

        public JsonResult buildOrderDeposit(string id, string set)
        {
            int depositAmount= 0;
            switch (set)
            {
                case "1":
                    depositAmount = 500;
                    break;
                case "2":
                    depositAmount = 1000;
                    break;
                case "3":
                    depositAmount = 2000;
                    break;
                case "4":
                    depositAmount = 5000;
                    break;
                default:
                    depositAmount = 0;
                    break;                    
            }

            string MerchantTradeNo = "D"+DateTime.Now.ToString("yyyyMMddHHmmss");
            string MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string ItemName = "儲值"+depositAmount;
            string ReturnURL = CDictionary.LOCAL_WEBSITES+"/Deposits/saveDepositResult";
            string ClientBackURL = CDictionary.LOCAL_WEBSITES;

            var buildOrder = deposit(id, MerchantTradeNo, MerchantTradeDate,depositAmount, ItemName, ReturnURL, ClientBackURL);
            return buildOrder;
        }
        public JsonResult deposit(
            string memberID,
            string MerchantTradeNo,
            string MerchantTradeDate,
            int TotalAmount,
            string ItemName,
            string ReturnURL,
            string ClientBackURL)
        {
            string rawString = $"HashKey=5294y06JbISpM5x9" +
    $"&ChoosePayment=ALL&ClientBackURL={ClientBackURL}" +
    $"&CreditInstallment=" +
    $"&EncryptType=1" +
    $"&InstallmentAmount=" +
    $"&ItemName={ItemName}" +
    $"&MerchantID=2000132" +
    $"&MerchantTradeDate={MerchantTradeDate}" +
    $"&MerchantTradeNo={MerchantTradeNo}" +
    $"&PaymentType=aio" +
    $"&Redeem=" +
    $"&ReturnURL={ReturnURL}" +
    $"&StoreID={memberID}" +
    $"&TotalAmount={TotalAmount}" +
    $"&TradeDesc=建立信用卡測試訂單" +
    $"&HashIV=v77hoKGq4kWxNNIS";

            string urlString = WebUtility.UrlEncode(rawString).ToLower();
            SHA256 sha256 = SHA256.Create();
            string sha256String = (new ShareFunction()).GetHash(sha256, urlString).ToUpper();

            var bulidOrder = new
            {
                MerchantID = "2000132",
                MerchantTradeNo = MerchantTradeNo,
                MerchantTradeDate = MerchantTradeDate ,
                PaymentType = "aio",
                TotalAmount = TotalAmount,
                TradeDesc = "建立信用卡測試訂單",
                ItemName = ItemName,
                ReturnURL = ReturnURL,
                ChoosePayment = "Credit",
                StoreID = memberID,
                ClientBackURL = ClientBackURL,
                CreditInstallment = "",
                InstallmentAmount = "",
                Redeem = "",
                EncryptType = "1",
                CheckMacValue = sha256String,
            };
                return Json(bulidOrder);
        }



        public JsonResult getDeposits(string id)
        {
            var data = (new 擺腹BuyFoodContext()).TDeposits.Where(n => n.CMemberId == int.Parse(id))
                .Select(n => new
                {
                    depositTime = n.CDepositTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    depositAmount = n.CDepositAmount
                }) ;
            return Json(data);
        }
    }
}
