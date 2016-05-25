$(document).ready(function () {

    var model = new Shopmodel();

    $("#checkout-cart").click({ handler: model.checkoutcart });

    ko.applyBindings(model, document.getElementById("edit-profile"));
    model.Get();
    
});

function formatCurrency(value) {
    return "$" + value;
}

function Shopmodel() {

    var self = this;

    self.PlanName = ko.observable("");
    self.Price = ko.observable("");
    //self.AutoRefill = ko.observable("No");
    self.CallingFrom = ko.observable("");
    self.CallingTo = ko.observable("");
    self.CurrencyCode = ko.observable("");
    self.IsfromSerchrate = ko.observable("");
    self.SelectedAmount = self.Price;
   // self.Recharges = ko.observableArray(['10', '20', '50', '90' ]);
    self.Recharges = ko.observableArray([]);
    self.formattedPrice = ko.computed(function () {
        var price = self.Price();
        var currencycode = self.CurrencyCode();
        return price ? currencycode + " " + price : 0;
    });    
    self.collect = ko.observableArray([]);

    self.Get = function () {
        $.ajax({
            url: "/Cart/GetCartData",
            datatype: "json",
            type: "GET",
            cache: false,
            success: function (data) {
                self.PlanName(data.PlanName);
                
                //self.AutoRefill(data.AutoRefill);
                self.CallingFrom(data.CallingFrom);
                self.CallingTo(data.CallingTo);
                self.CurrencyCode(data.CurrencyCode);
                
                self.IsfromSerchrate(data.IsfromSerchrate);
                if (data.IsfromSerchrate == true) {
                    self.Recharges(data.AmountList);
                    setSelectedIndex(document.getElementById("amount-dd"), data.Price);
                }
                self.Price(data.Price);
            }

        });
    };

    self.SetAmount = function() {
       
    };

    var redirect_url;
    self.checkoutcart = function () {
        $.ajax({
            url: "/Cart/CheckOutCart",
            data: {
                planAmount: self.Price  
            },
            type:"POST",
            success: function (resp) {
                var url = resp.newurl;
                if (resp.user == "newact") {
                    // new customer new plan redirect to checkout.
                    window.location.href = url; //"/Cart/CheckOut";
                } else if (resp.user == "exist-newact") {
                    //new customer new plan redirect to checkout.
                    window.location.href = url; //"/Cart/CheckOut";
                }
                else if (resp.user == "existplan") {
                    // existing plan existing customer redirect to recharge page or we can show popup message for user here.
                    loadPopup();
                    redirect_url = resp.newurl;
                    // window.location.href = resp.newurl;
                } else if (resp.user == "newplan") {
                    // redirect to regphone new plan existing customer.
                    window.location.href = url; //resp.newurl;
                }
            } 
        });
    };


};

function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#valpopup").fadeIn(0500); // fadein popup div
        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}

$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopup(); // function close pop up
    }
});

function closeloading() {
    $("div.loader").fadeOut('normal');
}

var popupStatus = 0; // set value

function disablePopup() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#valpopup").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}


function setSelectedIndex(s, valsearch) {
    // Loop through all the items in drop down list
    for (var i = 0; i < s.options.length; i++) {
        if (s.options[i].value == valsearch) {
            // Item is found. Set its property and exit
            s.options[i].selected = true;
            break;
        }
    }
    return;
}





