

function cwc_addProducttoCart(product) {
    var CurrentTime = new Date().getHours();
    var tcIndex = CurrentTime >= 3 && CurrentTime < 10 ? 0 :
        CurrentTime >= 10 && CurrentTime < 17 ? 1 : 2;

    var cb = product.cIsBreakFast,
        cl = product.cIsLunch,
        cd = product.cIsDinner;

    var timeCheck = [cb, cl, cd];

    if (timeCheck[tcIndex] != 0) {
        if (product.cQuantity <= 0 || product.cIsOnSaleId != 1) {
            window.alert(`商品${product.cProductName}目前已售完`);
        }
        else {
            cwc_addCart(product);
            window.alert(`已加入購物車`);
        }

    }
    else window.alert(`此時段不供應${product.cProductName}，請耐心等待`);


}

function cwc_addCombotoCart(cwc_combo) {
 
    var CurrentTime = new Date().getHours();
    var tcIndex = CurrentTime >= 3 && CurrentTime < 10 ? 0 :
        CurrentTime >= 10 && CurrentTime < 17 ? 1 : 2;

    var cb = cwc_combo.comboB,
        cl = cwc_combo.comboL,
        cd = cwc_combo.comboD;

    var timeCheck = [cb, cl, cd];
    var doubleOrder = 0;

    var addItems = 0;
    if (timeCheck[tcIndex] != "0") {
        var txt_inCart = "";
        for (var i = 0; i < cwc_combo.comboDetails.length; i++) {
            var double = cwc_combo.comboDetails[i].cProduct.cProductId == doubleOrder ? "1" : "0";
            if (cwc_combo.comboDetails[i].cProduct.cQuantity <= 0 || cwc_combo.comboDetails[i].cProduct.cIsOnSaleId != 1) {
                if (double == "0") {
                    window.alert(`商品${cwc_combo.comboDetails[i].cProduct.cProductName}目前已售完`);
                }
            }
            else {
                cwc_addCart(cwc_combo.comboDetails[i].cProduct);
                addItems++;
                doubleOrder = cwc_combo.comboDetails[i].cProduct.cProductId;
                if (double == "0") {
                    txt_inCart += txt_inCart == "" ? `${cwc_combo.comboDetails[i].cProduct.cProductName}` :
                        `、${cwc_combo.comboDetails[i].cProduct.cProductName}`
                }
            }
        }
        if (addItems>0)
            window.alert(`${txt_inCart}已加入購物車`);
    }
    else window.alert("套餐內容包含此時段不供應餐點，請耐心等待");

}

var cwc_cart = [];
//判斷購物車內是否有商品
function cwc_isExist(id) {
    cwc_cart = JSON.parse(localStorage.getItem(users_cart_No));

    for (let i = 0; i < cwc_cart.length; i++) {
        if (cwc_cart[i].cProductId === id) {
            cwc_cart[i].QuantityInCart += 1;
            cwc_cart[i].ProductAmount += cwc_cart[i].cPrice;
            localStorage.setItem(users_cart_No, JSON.stringify(cwc_cart));
            return i;
        }
    }
    return -1;
}

var cwc_pdtItem = null;
//將商品加入到購物車內
function cwc_addCart(obj_product) {

    if (localStorage.getItem(users_cart_No)) {
        let cwc_pdtItem = cwc_isExist(obj_product.cProductId);
        if (cwc_pdtItem == -1) {
            cwc_addItemInCart(obj_product);
        }
    } else {
        cwc_addItemInCart(obj_product);
    }
}
//若購物車內無該商品則加入
function cwc_addItemInCart(obj_product) {
    let cartItem = {
        cProductId: obj_product.cProductId,
        cProductName: obj_product.cProductName,
        cPrice: obj_product.cPrice,
        cPicture: obj_product.cPicture,
        QuantityInCart: 1,
        ProductAmount: obj_product.cPrice * 1
    };
    cwc_cart.push(cartItem);
    localStorage.setItem(users_cart_No, JSON.stringify(cwc_cart));
}