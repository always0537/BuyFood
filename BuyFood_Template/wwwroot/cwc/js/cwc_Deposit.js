function cwc_showWallet(memberID) {
    var txt =
        `<ul class="nav nav-pills mb-3 justify-content-center"
                        id="pills-tab"
                        role="tablist">
                        <li class="nav-item"
                                role="presentation">
                                <button class="nav-link active"
                                                id="pills-wallet-tab"
                                                data-bs-toggle="pill"
                                                data-bs-target="#pills-wallet"
                                                type="button"
                                                role="tab"
                                                aria-controls="pills-wallet"
                                                aria-selected="true">
                                儲值
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link"
                                                id="pills-DepositRecord-tab"
                                                data-bs-toggle="pill"
                                                data-bs-target="#pills-DepositRecord"
                                                type="button"
                                                role="tab"
                                                aria-controls="pills-DepositRecord"
                                                aria-selected="false"
                                                onclick="cwc_showDeposits(${memberID})">
                                儲值紀錄
                                </button>
                            </li>
                    </ul>`;
    var txtBottom = `<div class="tab-content" id="pills-tabContent">
            <div style="height:700px" class="tab-pane fade show active" id="pills-wallet" role="tabpanel" aria-labelledby="pills-wallet-tab">
                <div>
                    <div  style="display: inline-block; width:40%; height:250px; margin:10px">
                        <P style="font-size: 30px;height:100px; line-height:100px">儲值擺腹幣500</P>
                        <button class="btn btn-success" onclick="cwc_deposit(${memberID},1)">加值
                     </div>
                     <div  style="display: inline-block; width:40%; height:250px; margin:10px">
                        <P style="font-size: 30px; height:50px; line-height:50px">儲值擺腹幣1000</P>
                        <P style="font-size: 30px; height:50px; line-height:50px">送50折價券</P>
                        <button class="btn btn-success" onclick="cwc_deposit(${memberID},2)">加值
                    </div>
                </div>
                <div>
                    <div style="display: inline-block; width:40%; height:250px; margin:10px">
                        <P style="font-size: 30px; height:50px; line-height:50px">儲值擺腹幣2000</P>
                        <P style="font-size: 30px; height:50px; line-height:50px">送200折價券</P>
                         <button class="btn btn-success" onclick="cwc_deposit(${memberID},3)">加值
                    </div>
                    <div  style="display: inline-block; width:40%; height:250px; margin:10px">
                        <P style="font-size: 30px; height:50px; line-height:50px">儲值擺腹幣5000</P>
                        <P style="font-size: 30px; height:50px; line-height:50px">送500折價券</P>
                        <button class="btn btn-success" onclick="cwc_deposit(${memberID},4)">加值
                    </div>
                </div>
            </div>
            <div class="tab-pane fade" id="pills-DepositRecord" role="tabpanel" aria-labelledby="pills-DepositRecord-tab"></div>
        </div>`;
    $("#head_cwc").html(txt);
    $("#content_cwc").html(txtBottom);
}


function cwc_showDeposits(memberID) {
    $.ajax({
        url: "/Deposits/getDeposits?id=" + memberID,
        type: "GET",
        success: function (data) {
            var start =
                `<table class="table"><thead><tr><td>儲值時間</td><td>儲值金額</td></tr></thead><tbody>`;
            var end =
                `</tbody></table>`;
            var content = "";
            for (var i = 0; i < data.length; i++) {
                content += `<tr><td>${data[i].depositTime}<td>${data[i].depositAmount}`;
            };
            var txt = start + content + end;

            $("#pills-DepositRecord").html(txt);

        }
    })
}
function cwc_deposit(memberID, set) {
    $.ajax({
        url: `/Deposits/buildOrderDeposit?id=${memberID}&&set=${set}`,
        success: function (data) {
            if (data.totalAmount != 0) {
                var txt = `<form style="display:none" id="formCreditCard" method="post" accept-charset="UTF-8"
                      action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5">

                    <input type="text" name="MerchantID" value="${data.merchantID}" /><br />
                    <input type="text" name="MerchantTradeNo" value="${data.merchantTradeNo}" /><br />
                    <input type="text" name="MerchantTradeDate" value="${data.merchantTradeDate}" /><br />
                    <input type="text" name="PaymentType" value="aio" /><br />
                    <input type="text" name="TotalAmount" value="${data.totalAmount}" /><br />
                    <input type="text" name="TradeDesc" value="建立信用卡測試訂單" /><br />
                    <input type="text" name="ItemName" value="${data.itemName}" /><br />
                    <input type="text" name="ReturnURL" value="${data.returnURL}" /><br />
                    <input type="text" name="ChoosePayment" value="ALL" /><br />
                    <input type="text" name="StoreID" value="${memberID}" /><br />
                    <input type="text" name="ClientBackURL" value="${data.clientBackURL}" /><br />
                    <input type="text" name="CreditInstallment" value="" /><br />
                    <input type="text" name="InstallmentAmount" value="" /><br />
                    <input type="text" name="Redeem" value="" /><br />
                    <input type="text" name="EncryptType" value="1" /><br />
                    <input type="text" name="CheckMacValue" value="${data.checkMacValue}" /><br />
                    <input type="submit" id="submit"  value="送出訂單" />
                </form>`
                $("#content_cwc").append(txt);
                $("#submit").click();
            }
            else console.log("fail");
        }
    });
}
