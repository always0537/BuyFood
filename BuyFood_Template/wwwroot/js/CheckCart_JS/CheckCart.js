let member_NoCheck = $("#user_member").val();
let users_cart_NoCheck = "cart" + member_NoCheck;   //依登入的會員改變localstorage的Key值
let pdtcart = JSON.parse(localStorage.getItem(users_cart_NoCheck));  //從localStorage讀取購物車內的資料
let pdtcart_obj = JSON.parse(localStorage.getItem("cart_price"));    //取得購物車內的價格
let sum_total_time = 0;      //儲存訂單需要的製作時間及路程時間總和
let input_location = "";     //儲存輸入的地址位置
//console.log(pdtcart_obj.couponId);
//將金額顯示在html中
$(function () {
    //加入通知群組
    admAddToGroup('Order');
    $("#total_price").text('$' + pdtcart_obj.total);
    $("#discount_price").text('$' + pdtcart_obj.discount);
    $("#origin_price").text('$' + pdtcart_obj.origin);
});
//送出訂單
$("#confirm_order").click(function () {
    if (input_location == "") {
        window.alert("請輸入要指定的地點");
        $("#cover_page").css({ "display": "none" });
        return;
    }
    //加入遮罩效果
    addMaskPage();
    let radio_value = parseInt($("input[name=pay]:checked").val());
    //console.log(radio_value);
    checkPayType(radio_value);
})

//點擊"確認地址"的按鈕事件
$("#search_address").click(
    function () {
        var input_address = $("#input_address").val();
        console.log(input_address);
        //檢查是否有輸入地址
        if (input_address == "") {
            window.alert("請輸入要指定的地點");
            $("#estimate_time").html("");
            return;
        }
        //加入遮罩效果
        addMaskPage();

        var origin1 = '106台北市大安區復興南路一段390號2樓';
        var directionsService = new google.maps.DirectionsService();
        var directionsRenderer = new google.maps.DirectionsRenderer();
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 14,
            center: { lat: 25.0342831, lng: 121.5447801 }
        });

        directionsRenderer.setMap(map);
        var request = {
            origin: { 'query': origin1 },
            destination: { 'query': input_address },
            travelMode: 'DRIVING',
            avoidHighways: true,
            avoidTolls: true
        };
        //繪製兩個地點間的路線
        directionsService.route(request, function (result, status) {
            if (status == 'OK') {
                directionsRenderer.setDirections(result);
                //console.log(result);
            } else {
                window.alert("找不到該輸入的位置");
                return;
                //console.log(status);
            }
        });
        //設定路線的基本條件
        var service = new google.maps.DistanceMatrixService();
        service.getDistanceMatrix(
            {
                origins: [origin1],               //出發位置 (需用陣列型態)
                destinations: [input_address],    //目的地位置
                travelMode: 'DRIVING',
                unitSystem: google.maps.UnitSystem.METRIC,  //距離以公里為單位
                avoidHighways: true,
                avoidTolls: true,
            }, callback);
        //取得路線資訊後取出預估時間
        function callback(response, status) {
            if (status != 'OK') {
                window.alert("查不到該路線資訊");
                return;
            }
            geocoder = new google.maps.Geocoder();
            //geocoder.geocode()為取得該輸入地址的經緯度，再帶入computeDistanceBetween()方法
            geocoder.geocode({ 'address': input_address }, function (results, status) {
                if (status != 'OK') {
                    window.alert("輸入的位置有誤");
                    $("#cover_page").css({ "display": "none" });   //取消遮罩效果
                    return;
                    //console.log(results[0].geometry.location.lat());
                }
                //計算出發地與目的地的直線距離
                let distance_inline = Math.floor(google.maps.geometry.spherical.computeDistanceBetween(
                    new google.maps.LatLng({ lat: 25.033942297235797, lng: 121.54340761133165 }),
                    new google.maps.LatLng({ lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng() })
                ))
                //規劃路線後Circle會消失，故再生成一次
                const cityCircle = new google.maps.Circle({
                    strokeColor: "#00BBFF",
                    strokeOpacity: 0.3,
                    strokeWeight: 2,
                    fillColor: "#00BBFF",
                    fillOpacity: 0.2,
                    clickable: true,
                    map,
                    center: { lat: 25.033942297235797, lng: 121.54340761133165 },
                    radius: 3500,
                });
                //判斷輸入的距離是否超過Circle範圍
                if (distance_inline > cityCircle.radius) {
                    window.alert("輸入的位置超過可外送的距離，請重新再設定一次");
                    $("#cover_page").css({ "display": "none" });   //取消遮罩效果
                    $("#current_address").html("目前輸入的地點為:無");      //顯示目前輸入的地址
                    return;
                }
                //console.log(response);
                //console.log(response.rows[0].elements[0].duration.text);
                input_location = response.destinationAddresses[0];
                let duration_time = Math.round((response.rows[0].elements[0].duration.value) / 60);                
                sum_total_time = duration_time + pdtcart_obj.finishTime    //將路程與製作時間相加

                let datetime_now = new Date();    //存放目前的標準時間
                let datetime_last = new Date(datetime_now);  //儲存加總後的標準時間
                datetime_last.setMinutes(datetime_now.getMinutes() + sum_total_time);

                let text = `路程時間:${duration_time}鐘、製作時間:${pdtcart_obj.finishTime}鐘。 總計:${sum_total_time}鐘`;
                let time_text = "預估時間為:" + datetime_last.toLocaleString();
                let text_address = "目前輸入的地點為:" + input_location;
                $("#estimate_time").html(text);          //將預估時間顯示在畫面上
                $("#wu_display_time").html(time_text);   //顯示估計後的標準時間
                $("#confirm_order").prop('disabled', false);   //啟用'確認購買'的按鈕
                $("#current_address").html(text_address);      //顯示目前輸入的地址
                $("#confirm_order").css('background-color', '#7FAD39');
                $("#cover_page").css({ "display": "none" });   //取消遮罩效果
            });
        }
    }
)

//編譯成雜湊碼
async function sha256(message) {
    // encode as UTF-8
    const msgBuffer = new TextEncoder().encode(message);
    // hash the message
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer);
    // convert ArrayBuffer to Array
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    // convert bytes to hex string
    const hashHex = hashArray.map(b => ('00' + b.toString(16)).slice(-2)).join('');
    return hashHex;
}
//加入遮罩效果
function addMaskPage() {
    $("#cover_page").css({
        "height": $(document).height(),
        "width": $(document).width(),
        "display": "initial"
    });
}
//判斷使用的付款方式
function checkPayType(radio_value) {
    if (radio_value == 1) {
        //建立一個物件將資料轉為json傳回伺服器端
        let obj_cart_order = {
            cartOrder: pdtcart,
            couponSelected: pdtcart_obj.couponId,
            address: input_location,
            transportTime: sum_total_time,
            payType: radio_value
        };
        //console.log(obj_cart_order);
        $.ajax({
            url: "/CheckCart/InsertOrderToDB",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj_cart_order),
            success: function (data) {
                window.alert(data);
                if (data == "已收到訂單，請稍後") {
                    window.location.assign("/HomePage/Home");
                    localStorage.removeItem(users_cart_NoCheck);
                    localStorage.removeItem("cart_price");
                    SendOrderMessage('Order')//傳送訂單通知
                }
                //取消遮罩效果
                $("#cover_page").css({ "display": "none" });
            }
        })
    } else if (radio_value == 2) {
        let product_item = "";
        for (let i = 0; i < pdtcart.length; i++) {
            product_item += "#" + pdtcart[i].cProductName + " X " + pdtcart[i].QuantityInCart;
        }
        let web_host = window.location.host;
        console.log(web_host);
        $("#backUrl").val("https://" + web_host + "/HomePage/Home");
        var NowDate = new Date();
        //用日期時間當作商品編號
        let order_No = NowDate.getFullYear().toString() + NowDate.getMonth().toString() + NowDate.getDate().toString() + NowDate.getHours().toString()
            + NowDate.getMinutes().toString() + NowDate.getSeconds().toString() + NowDate.getMilliseconds().toString();
        //console.log(product_item);
        let ordertime = $("#order_date").val();
        //console.log(order_No);
        $("#product_name").val(product_item);
        //console.log($("#product_name").val());
        $("#total_amount").val(pdtcart_obj.total);
        //console.log($("#total_amount").val());
        $("#transaction_NO").val("DZ" + order_No);
        //console.log($("#transaction_NO").val());
        let CheckMacValue = `HashKey=5294y06JbISpM5x9&ChoosePayment=ALL&ChooseSubPayment=&ClientBackURL=${$("#backUrl").val()}&EncryptType=1&ItemName=${$("#product_name").val()}&MerchantID=2000132&MerchantTradeDate=${ordertime}&MerchantTradeNo=${$("#transaction_NO").val()}&PaymentType=aio&ReturnURL=${$("#returnUrl").val()}&StoreID=&TotalAmount=${pdtcart_obj.total}&TradeDesc=建立全金流測試訂單&HashIV=v77hoKGq4kWxNNIS`;
        //let CheckMacValue = `HashKey=5294y06JbISpM5x9&ChoosePayment=ALL&ChooseSubPayment=&ClientBackURL=https://localhost:44398/ViewProduct/ShowProduct&EncryptType=1&ItemName=${$("#product_name").val()}&MerchantID=2000132&MerchantTradeDate=${ordertime}&MerchantTradeNo=${$("#transaction_NO").val()}&PaymentType=aio&ReturnURL=https://developers.opay.tw/AioMock/MerchantReturnUrl&StoreID=&TotalAmount=${pdtcart_obj.total}&TradeDesc=建立全金流測試訂單&HashIV=v77hoKGq4kWxNNIS`;
        //console.log(CheckMacValue);
        CheckMacValue = encodeURIComponent(CheckMacValue).replaceAll("%20", '+');
        CheckMacValue = CheckMacValue.toLowerCase()
        //console.log(CheckMacValue);
        //console.log(sha256(CheckMacValue));
        const result = new Promise((resolve, reject) => {
            resolve(sha256(CheckMacValue));
        });
        result.then((res) => {
            let toUpper = res.toUpperCase();
            $("#check_mac_value").val(toUpper);
            console.log($("#check_mac_value").val()); // 執行成功
        });

        //顯示確認通知
        $("#dialog-confirm").dialog({
            resizable: false,
            height: "auto",
            width: 400,
            modal: true,
            buttons: {
                "確定": function () {
                    $(this).dialog("close");
                    let obj_cart_order = {
                        cartOrder: pdtcart,
                        couponSelected: pdtcart_obj.couponId,
                        address: input_location,
                        transportTime: sum_total_time,
                        payType: radio_value
                    };
                    //console.log(obj_cart_order);
                    $.ajax({
                        url: "/CheckCart/InsertOrderToDB",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(obj_cart_order),
                        success: function () {
                            $("#send_to_opay").click();
                            localStorage.removeItem(users_cart_NoCheck);
                            localStorage.removeItem("cart_price");
                            //取消遮罩效果
                            //$("#cover_page").css({ "display": "none" });
                            SendOrderMessage('Order')//傳送訂單通知
                        }
                    })
                },
                "取消": function () {
                    $(this).dialog("close");
                    //取消遮罩效果
                    $("#cover_page").css({ "display": "none" });
                }
            },
            //關閉小視窗的觸發事件
            //close: function () {
            //    $("#cover_page").css({ "display": "none" });
            //}
        });
    }
}

//加入群組
function admAddToGroup(ChannelID) {
    var groupNameOrder = ChannelID.toString();
    let userName = $("#adrLoginNameNow").val();

    connection.invoke("AddGroup", groupNameOrder, userName).catch(function (err) {
        return console.error(err.toString());
    })
}
//加入群組

//觸發發送通知事件
function SendOrderMessage(ChannelID) {
    var groupNameOrder = ChannelID.toString();
    let userName = $("#adrLoginNameNow").val();
    var messageOrder = '下了1筆訂單';
    var adrfotoOrder = $("#user_foto").val();
    var adrMemberIDForOrder = 101;
    $.ajax({
        url: "/Order/saveTheNewOrders/",
        type: "POST",
        data: { "userName": userName, "OrderMessage": messageOrder },
        success: function (data) {

        }

    });
    connection.invoke("SendMessageToOrder", groupNameOrder, userName, messageOrder, adrfotoOrder, adrMemberIDForOrder).catch(function (err) {
        return console.log(err.toString());
    });
}
//觸發發送通知事件