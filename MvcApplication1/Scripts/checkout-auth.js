function loading() {
    $("div.loader").show();
}
function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#toPopup").fadeIn(0500); // fadein popup div
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
        $("#toPopup").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}
// end for function for show popup div.

var api;
var tabCounter;
$(document).ready(function () {

    tabCounter = 1;
    var root = $("#wizard").scrollable();

    // some variables that we need
    api = root.scrollable(), drawer = $("#drawer");

    $("#f_fixed").css({ "visibility": "visible" });
    $("#status li:first").addClass("active");

    var checkoutmodel = new CheckOutAuthModel();

    $("#issuenewpin-submit").click({ handler: checkoutmodel.IssueNewPin });
    //  $("#user-newsignup").click({ handler: checkoutmodel.Newlogon });
    $("#billinfo_edit").click({ handler: checkoutmodel.editbillinfo });
    $("#billinfo_done").click({ handler: checkoutmodel.donebillinfo });

    ko.applyBindings(checkoutmodel, document.getElementById("wizard"));

    checkoutmodel.GetCart();
    checkoutmodel.GetBiillninfo();
    api.onBeforeSeek(function (event, i) {
        // we are going 1 step backwards so no need for validation
        //  drawer = $("#drawer");

        if (api.getIndex() < i) {

            // 1. get current page

            var page = root.find(".form_field").eq(api.getIndex()),
            // 2. .. and all required fields inside the page

                inputs = page.find(".required :input").removeClass("error"),
            // 3. .. which are empty

                empty = inputs.filter(function () {

                    return $(this).val().replace(/\s*/g, '') == '';

                });
            // if there are empty fields, then
            var card = $("#card").val().length;
            var strMonth = $('#cmbMonth').find(":selected").text();
            var strYear = $("#cmbYear").find(":selected").text();
            var checkedValue = $('.terms_chackbox:checked').val();
            var radio = $('.radio_bt:checked').val();
            var cvv = $("#txtCVV2 ").val().length;

            if (empty.length) {

                // slide down the drawer
                drawer.slideDown(function () {
                    // colored flash effect
                    drawer.css("backgroundColor", "#229");
                    setTimeout(function () { drawer.css("backgroundColor", "#fff"); }, 1000);
                }
                );

                // add a CSS class name "error" for empty & required fields
                empty.addClass("error");


                // cancel seeking of the scrollable by returning false
                //                return false;
            }
            // everything is good
            else {

                if (tabCounter == 1) //
                {
                    var cardnumber = $("#card").val();
                    var cardtype = $('.radio_bt:checked').val();
                    if (radio == undefined || card < 16 || strMonth == "Month" || strYear == "Year" || cvv < 3 ) {
                       if (radio == undefined) {
                           $("#val2-rech1").css("display", "");
                           $("#billinfo-val1").html("Card type is required.");
                       } else {
                           $("#val2-rech1").css("display", "none");
                           $("#billinfo-val1").html("");
                       }
                       if (card == 0) {
                           $("#val2-rech2").css("display", "");
                           $("#billinfo-val2").html("Card number is required.");
                       } else {
                           $("#val2-rech2").css("display", "none");
                           $("#billinfo-val2").html("Card number is required.");
                       }
                       if (card < 16 && card != 0) {
                           $("#val2-rech3").css("display", "");
                           $("#billinfo-val3").html("Please insert a valid card.");
                       } else {
                           $("#val2-rech3").css("display", "none");
                           $("#billinfo-val3").html("Please insert a valid card.");
                       }
                       if (strMonth == "Month" || strYear == "Year") {
                           $("#val2-rech4").css("display", "");
                           $("#billinfo-val4").html("Expiry Date is required.");
                       } else {
                           $("#val2-rech4").css("display", "none");
                           $("#billinfo-val4").html("Expiry Date is required.");
                       }
                        if (cvv < 3) {
                            if (cvv == 0) {
                                $("#val2-rech5").css("display", "");
                                $("#billinfo-val5").html("Cvv number is required.");
                            } else {
                                $("#val2-rech5").css("display", "");
                                $("#billinfo-val5").html("Cvv is invalid.");
                            }
                        } else {
                            $("#val2-rech5").css("display", "none");
                            $("#billinfo-val5").html("Cvv is invalid.");
                        }
                       loadPopup();
                        return false;
                    }
                    else if (!checkCreditCard(cardnumber, cardtype)) {
                        $("#val2-rech1").css("display", "");
                        $("#billinfo-val1").html("Please try with a valid card. ");
                        $("#val2-rech1").css("display", "none");
                        $("#val2-rech2").css("display", "none");
                        $("#val2-rech3").css("display", "none");
                        $("#val2-rech4").css("display", "none");
                        $("#val2-rech5").css("display", "none");
                        loadPopup();
                        return false;
                    }
                    else if (checkedValue == undefined) {
                        return false;
                    }
                    tabCounter = tabCounter + 1;
                    drawer.slideUp();
                } else if (tabCounter == 2) {

                    // call web service
                    checkoutmodel.IssueNewPin();

                    drawer.slideUp();

                    return false;
                }

            }

        }
        // update status bar
        $("#status li").removeClass("active").eq(i).addClass("active");
    });


    // if tab is pressed on the next button seek to next page
    root.find("button.next").keydown(function (e) {


        if (e.keyCode == 9) {

            // seeks to next tab by executing our validation routine

            //api.next();

            e.preventDefault();

        }

    });


});



function CheckOutAuthModel() {

    var self = this;
    // some extra varible use in checkout with out authentication.
    self.Password = ko.observable("");

    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.Country = ko.observable("");
    self.City = ko.observable("");
    self.ZipCode = ko.observable("");
    self.State = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");
    self.CardNumber = ko.observable("");
    self.CvvNumber = ko.observable("");
    self.CardType = ko.observable("");
    self.ExpMonth = ko.observable("");
    self.ExpYear = ko.observable("");
    self.CoupanCode = ko.observable("");
    self.CurrencyCode = ko.observable("");
    self.UserType = ko.observable("");
    self.StatusMessage = ko.observable("");
    self.PurchaseTime = ko.observable("");
    
    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, this);
    self.ExpDate = ko.computed(function () {
        return self.ExpMonth() + "/" + self.ExpYear();
    }, this);
    self.CardExpDate = ko.computed(function () {
        return self.ExpMonth() + self.ExpYear();
    }, this);
    self.PaymentMethod = ko.computed(function () {
        if (self.CardType() == "PayPal")
            return self.CardType();
        else {
            return "Credit Card";
        }
    }, this);

    self.PlanName = ko.observable("");
    self.Amount = ko.observable("");
    self.ServiceFee = ko.observable("");
    self.CardId = ko.observable("");
    self.collect = ko.observableArray([]);
    self.CountryFrom = ko.observable("");
    self.CountryTo = ko.observable("");
    self.maskednumber = ko.computed(function () {
        return self.CardNumber().slice(0, 4) + "xxxxxxxx" + self.CardNumber().slice(12);
    }, this);

    self.UnmaskPhonenumber = ko.computed(function () {
        var number = self.PhoneNumber().replace("-", "");
        return number.replace("-", "");
    }, this);

    self.Orderstatus = ko.observable("");

    self.GetCart = function () {
        $.ajax({
            url: "/Cart/GetCartData",
            type: "GET",
            cache: false,
            success: function (data) {
                self.PlanName(data.PlanName);
                self.Amount(data.Price);
                self.ServiceFee(data.ServiceFee);
                self.CountryFrom(data.CountryFrom);
                self.CountryTo(data.CountryTo);
                self.CardId(data.FromToMapping);
                self.ServiceFee(data.ServiceFee);
                self.CurrencyCode(data.CurrencyCode);
                self.UserType(data.Userstatus);
            }

        });
    };

    self.GetBiillninfo = function () {
      
        $.ajax({
            //url: window.location + 'GetBillingInfo',
            url: "/Account/GetBillingInfo",
            type: "GET",
            cache: false,
            success: function (data) {
                self.FirstName(data.FirstName);
                self.LastName(data.LastName);
                self.Country(data.Country);
                self.City(data.City);
                self.ZipCode(data.ZipCode);
                self.State(data.State);
                self.Address(data.Address);
                self.Email(data.Email);
                self.PhoneNumber(data.PhoneNumber);
                
            }

        });
    };


    self.IssueNewPin = function () {
        $.ajax({
            url: '/Cart/IssueNewPin',
            data: {
                FirstName: self.FirstName,
                LastName: self.LastName,
                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                Email: self.Email,
                CountryFrom: self.CountryFrom,
                CountryTo: self.CountryTo,
                CardNumber: self.CardNumber,
                CardType: self.CardType,
                ExpDate: self.CardExpDate,
                CoupanCode: self.CoupanCode,
                Amount: self.Amount,
                CvvNumber: self.CvvNumber,
                CardId: self.CardId,
                PaymentMethod: self.PaymentMethod,
                AniNumber: self.UnmaskPhonenumber,
                UserType: self.UserType
            },
            type: "POST",
            success: function (resp) {
                if (resp.status) {
                    self.PurchaseTime(resp.ordertime);
                    self.StatusMessage("Congratulations! Your Order is Approved.");
                } else {
                    self.PurchaseTime(resp.ordertime);
                    self.StatusMessage("Sorry! Your order is rejected.");
                }
                tabCounter = tabCounter + 1;
                api.next();

            },
            error: function(jqXHR, textStatus, errorThrown) {
                self.StatusMessage(jqXHR.Message);
            }
        });
    };

    self.editbillinfo = function () {
        $('#billinfo_edit').css('display', 'none');
        $('#billinfo_done').css('display', '');
        $('#fullname, #address, #city, #state, #zipcode, #unmaskphone').css('display', 'none');
        $('#fullnameedit')
        //.val($('#fullname').text())
            .css('display', '')
            .focus();
        $('#addressedit, #cityedit, #stateedit, #zipcodeedit, #unmaskphoneedit')
        //.val($('#fullname').text())
            .css('display', '');
    };
    self.donebillinfo = function () {
        $('#billinfo_edit').css('display', '');
        $('#billinfo_done').css('display', 'none');
        $('#fullnameedit, #addressedit, #cityedit, #stateedit, #zipcodeedit, #unmaskphoneedit').css('display', 'none');
        $('#fullname')
        .text($('#fullnameedit').val())
        .css('display', '');
        $('#address')
        .text($('#addressedit').val())
        .css('display', '');
        $('#city')
        .text($('#cityedit').val())
        .css('display', '');
        $('#state')
        .text($('.optionselect').val())
        .css('display', '');
        $('#zipcode')
        .text($('#zipcodeedit').val())
        .css('display', '');
        $('#unmaskphone')
        .text($('#unmaskphoneedit').val())
        .css('display', '');
    };


}







