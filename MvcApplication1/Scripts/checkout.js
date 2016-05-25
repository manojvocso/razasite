var drawer;
var api;
function loading() {
    $("div.loader").show();
}
function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#errorValidation").fadeIn(0500); // fadein popup div
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
        $("#errorValidation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}
// end for function for show popup div.


var newusercheckout;

$(document).ready(function () {

    var root = $("#wizard").scrollable();
    api = root.scrollable(), drawer = $("#drawer");

    $("#f_fixed").css({ "visibility": "visible" });
    $("#status li:first").addClass("active");

    // hide and show PassCode div
    $("#Submit").click(function () {

        $(".showdiv").hide();

    });

    $(".showraddiv").click(function () {

        $(".showdiv").show();

    });

    newusercheckout = new BillingInfoViewModel();

    //$("#issuenewpin-submit").click({ handler: newusercheckout.IssueNewPin });
    //$("#user-newsignup").click({ handler: newusercheckout.NewlogonValidation });
    $("#billinfo_edit").click({ handler: newusercheckout.editbillinfo });
    $("#billinfo_done").click({ handler: newusercheckout.donebillinfo });
    $("#close-val").click({ handler: newusercheckout.disablepopupval });
    $("#backgroundPopup").click({ handler: newusercheckout.disablepopupval });
    $("#process-withoutrefill").click({ handler: newusercheckout.WithoutAutorefillProceed });


    ko.applyBindings(newusercheckout, document.getElementById("wizard"));

    newusercheckout.GetCart();
    newusercheckout.FetchCountryListTo();
    //newusercheckout.CheckUserfrom();



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

            // fields for payment info.
            var card = $("#card").val();
            var strMonth = $('#cmbMonth').find(":selected").text();
            var strYear = $("#cmbExpYear").find(":selected").text();
            var checkedValue = $('.terms_chackbox:checked').val();
            var radio = $('.radio_bt:checked').val();
            var cvv = $("#txtCVV2 ").val();

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
                //slider start in this
                if (i == 1) {
                    var result = newusercheckout.Newlogon();
                    return result;
                }
                else if (i == 2) {
                    var paypalvalue = $('input[name=paymentType]:radio:checked').val();
                    $("#paypal-hidmsg").css("display", "none");
                    $("#paypal-hidmsg2").css("display", "none");
                    $("#val2-rech1").css("display", "none");
                    $("#val2-rech2").css("display", "none");
                    $("#val2-rech3").css("display", "none");
                    $("#val2-rech4").css("display", "none");

                    if (radio != undefined && radio == "PayPal") {
                        if (newusercheckout.PlanName() == "Talk Mobile" || newusercheckout.PlanName() == "1 Cent Plan") {
                            $("#paypal-hidmsg2").css("display", "");
                            loadPopup();
                            return false;
                        }
                        if (newusercheckout.AutoRefill() == "T") {
                            $("#paypal-hidmsg").css("display", "");
                            loadPopup();
                            return false;
                        }
                        if (newusercheckout.ValidateCouponCode() == false) {
                            return false;
                        }
                        $("#slider2").removeClass("active");
                        $("#slider3").addClass("active");
                        return true;
                    }
                    var myCardNo = $("#card").val();
                    var myCardType = $(".radio_bt:checked").val();
                    if (radio == undefined || card < 16 || strMonth == "Month" || strYear == "Year" || cvv < 3) {
                        //$("#val2-rech").css("display", "");
                        if (radio == undefined) {
                            $("#val2-rech1").css("display", "");
                            $("#billinfo-val1").html("Card type is required.");
                        }
                        if (card < 16) {
                            if (card == 0) {
                                $("#val2-rech2").css("display", "");
                                $("#billinfo-val2").html("Please enter a valid card number.");
                            } else {
                                $("#val2-rech2").css("display", "");
                                $("#billinfo-val2").html("Card number is required.");
                            }

                        }
                        if (strMonth == "Month" || strYear == "Year") {
                            $("#val2-rech3").css("display", "");
                            $("#billinfo-val3").html("Expiry Date is required.");
                        }

                        if (cvv != undefined && cvv.length < 3) {
                            if (cvv != undefined && cvv.length == 0) {
                                $("#val2-rech4").css("display", "");
                                $("#billinfo-val4").html("Cvv number is required.");
                            } else {
                                $("#val2-rech4").css("display", "");
                                $("#billinfo-val4").html("Cvv is invalid.");
                            }

                        }
                        $("#popup-head").text("Credit Card Validations.");
                        loadPopup();
                        return false;


                    } else if (!checkCreditCard(myCardNo, myCardType)) {

                        $("#val2-rech1").css("display", "");
                        $("#billinfo-val1").html("Please try with a valid card. ");
                        $("#popup-head").text("Credit Card Validations.");
                        loadPopup();
                        return false;
                    }
                    else if (checkedValue == undefined) {
                        return false;
                    }
                    else {
                        //validate coupon code for paypal.
                        if (newusercheckout.ValidateCouponCode() == false) {
                            return false;
                        }
                        $("#slider2").removeClass("active");
                        $("#slider3").addClass("active");
                        $("#billinfo-val1").html("");
                        $("#billinfo-val2").html("");
                        $("#billinfo-val3").html("");
                        $("#billinfo-val4").html("");
                        return true;
                    }

                } else if (i == 3) {
                    if (newusercheckout.CardId() != "9999" && newusercheckout.ValidateCardInfo() == false) {
                        return false;
                    }

                    // call web service Issue new pin method.
                    if (newusercheckout.IsCentinelAllowed()) {
                        return false;
                    } else {
                        if (newusercheckout.AcceptOrder()) {
                            return newusercheckout.IssueNewPin();
                        }
                        return true;
                    }
                }

            }
        }

        // update status bar
        $("#status li").removeClass("active").eq(i).addClass("active");
    });



    $("#payment_edit").click(function () {

        // seeks to prev tab by executing our validation routine
        api.prev();

    });

    // if tab is pressed on the next button seek to next page
    //root.find("button.next").keydown(function (e) {


    //    if (e.keyCode == 9) {

    //        // seeks to next tab by executing our validation routine

    //        //api_tab.next();

    //        e.preventDefault();

    //    }

    //});


});


function BillingInfoViewModel() {

    var self = this;

    self.AcceptOrder = ko.observable("");
    self.DoCcProcess = ko.observable("");
    self.CentinelBypass = ko.observable("");
    self.AvsByPass = ko.observable("");

    self.Password = ko.observable("");
    var firstname = $("#hid-firstname").val();
    self.FirstName = ko.observable(firstname);
    var lastname = $("#hid-lastname").val();
    self.LastName = ko.observable(lastname);

    var phone = $("#hid-aninumber").val();
    self.PhoneNumber = ko.observable(phone);
    var country = $("#hid-country").val();
    self.Country = ko.observable();
    var city = $("#hid-city").val();
    self.City = ko.observable(city);
    var zipcode = $("#hid-zipcode").val();
    self.ZipCode = ko.observable(zipcode);
    var state = $("#hid-state").val();
    self.State = ko.observable();
    self.Stateuk = ko.observable();
    var address = $("#hid-address").val();
    self.Address = ko.observable(address);
    self.Email = ko.observable("");
    self.CardNumber = ko.observable("");
    self.CvvNumber = ko.observable("");
    self.CardType = ko.observable("");
    self.ExpMonth = ko.observable("");
    self.ExpYear = ko.observable("");
    self.CoupanCode = ko.observable("");
    self.CurrencyCode = ko.observable("");
    self.UserType = ko.observable("");
    self.ConfPassword = ko.observable("");
    self.RefrerEmail = ko.observable("");

    self.IsPassCodeDial = ko.observable("F");
    self.PassCode = ko.observable("");
    self.ReconfPassCode = ko.observable("");

    self.StatusMessage = ko.observable("");
    self.PurchaseTime = ko.observable("");
    self.AutoRefillAmount = ko.observable("");
    self.AutoRefill = ko.observable("F");
    var isignuped = $("#hid-signup").val();
    self.IsSignuped = ko.observable(isignuped);

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
    self.BillingCountryList = ko.observableArray([]);
    self.BillingStateList = ko.observableArray([]);
    self.BillingcountryId = ko.observable("");
    self.LocalAccessNumberList = ko.observableArray([]);

    self.maskednumber = ko.computed(function () {
        return self.CardNumber().slice(0, 4) + "xxxxxxxx" + self.CardNumber().slice(12);
    }, this);

    self.UnmaskPhonenumber = ko.computed(function () {
        var number = self.PhoneNumber().replace("-", "");
        return number.replace("-", "");
    }, this);
    self.TotalAmount = ko.computed(function () {
        var amt = self.Amount() + self.ServiceFee();
        return Math.floor(amt * 100) / 100;
    }, this);
    self.ConfEmail = ko.observable("");
    self.validationmessages = ko.observableArray([]);

    self.Orderstatus = ko.observable("");
    self.StatusError = ko.observable("");
    self.UserStatus = ko.observable("");
    self.CouponCodeMsg = ko.observable("");
    self.IsPromotion = ko.observable("");

    self.GetCart = function () {
        $.ajax({
            url: "/Cart/GetCartData",
            type: "GET",
            cache: false,
            success: function (data) {
                //if (data.Userstatus == "QuickSignUp" || data.Userstatus == "quicksignup") {
                //    $("#from-quick").css("display", "none");
                //    $("#hd-text").text("Personal Info");
                //    self.GetBiillninfo();
                //}

                self.UserStatus(data.Userstatus);
                self.PlanName(data.PlanName);
                self.Amount(data.Price);
                self.ServiceFee(data.ServiceFee);
                self.CountryFrom(data.CountryFrom);
                self.CountryTo(data.CountryTo);
                self.CardId(data.FromToMapping);
                self.ServiceFee(data.ServiceFee);
                self.CurrencyCode(data.CurrencyCode);
                self.CoupanCode(data.CouponCode);
                self.IsPromotion(data.IsPromotionPlan);
                self.UserType(data.Userstatus);
                if (data.IsEnrollToExtraMinute) {
                    self.AutoRefill("E");
                }
               else if (data.IsAutoRefill) {
                    self.AutoRefill("T");
                    self.AutoRefillAmount(data.Price);
                } else {
                    self.AutoRefill("F");
                }
               
                //self.CheckUserfrom();

            }

        });
    };

    self.CheckUserfrom = function () {

        if (self.UserStatus == "Quicksignup") {

        }
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

    self.validateCreditCard = function () {
        self.validationmessages([]);


        if ((self.State() == "" && self.BillingcountryId() != 3) || self.ZipCode() == "" || self.PhoneNumber().length == 0 || self.Address() == ""
            || self.City() == "" || self.FirstName() == "" || self.LastName() == "") {

            if (self.PhoneNumber().length == 0) {
                self.validationmessages.push("Phone number is required.");
            } else if (self.PhoneNumber().length < 10) {
                self.validationmessages.push("Please enter a 10 digit phone number.");
            }
            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is required.");
            }
            if (self.Address() == "") {
                self.validationmessages.push("Address is required.");
            }
            if (self.City() == "") {
                self.validationmessages.push("City is required.");
            }
            if (self.State() == undefined) {
                self.validationmessages.push("State is required.");
            }
            if (self.FirstName() == "") {
                self.validationmessages.push("Firstname is required.");
            }
            if (self.LastName() == "") {
                self.validationmessages.push("Lastname is required.");
            }
            return false;
        }
        return true;
    };

    self.ValidateCouponCode = function () {
        var result = false;
        if (self.CoupanCode() == null || self.CoupanCode().length == 0) {
            result = true;
        } else {
            $.ajax({
                url: '/Cart/ValidateCouponCode',
                data: {
                    Amount: self.Amount,
                    CardId: self.CardId,
                    CouponCode: self.CoupanCode,
                    TransactionType: "Checkout",
                    Countryfrom: self.CountryFrom,
                    Countryto: self.CountryTo,
                },
                async: false,
                type: "POST",
                success: function (resp) {
                    if (resp.status == true) {
                        // Process Card LookUp
                        result = true;

                    } else {
                        self.CouponCodeMsg("(Invalid or expired coupon code)");
                        self.CoupanCode("");
                        result = false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    result = false;
                }
            });
        }
        return result;
    };

    self.ValidateCardInfo = function () {


        var result = false;

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
            result = true;
        } else {
            $.ajax({
                url: '/Cart/ValidateCardInfo',
                data: {
                    CardNumber: self.CardNumber,
                    Amount: self.Amount,
                    CardId: self.CardId,
                    CouponCode: self.CoupanCode,
                    TransactionType: "Checkout",
                    Countryfrom: self.CountryFrom,
                    Countryto: self.CountryTo,
                },
                async: false,
                type: "GET",
                success: function (resp) {
                    if (resp.status == true) {
                        // Process Card LookUp
                        result = true;

                    } else {
                        self.validationmessages([]);
                        self.validationmessages.push(resp.Message);
                        //self.loadpopupval();
                        //return false;

                        loadPopup();
                        result = false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    self.validationmessages.push(jqXHR.Message);
                    result = false;
                }
            });
        }
        return result;

    };


    self.IsCentinelAllowed = function () {
        var result = false;
        var ordertype = "NewPin";

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
        }
        else if (self.CardId() == 9999) {
            paymenttype = "F";
        }

        $.ajax({
            url: '/Cart/CcProcessValidation',
            data: {
                userType: "new",
                orderType: "NewPin",
                PaymentMethod: paymenttype,
                CardNumber: self.CardNumber,
                //Country: self.Country
                Country: self.BillingcountryId,
                TransAmount: self.Amount
            },
            async: false,
            type: "GET",
            success: function (resp) {
                if (resp.IsValidPlan != true) {
                    self.validationmessages.push(resp.StatusMsg);
                    loadPopup();
                    result = false;
                }
                if (resp.AcceptOrder != true) {
                    self.SaveNewPending();
                    return false;
                }
                if (resp.AcceptOrder == true && resp.DoCcProcess == true) {
                    self.AcceptOrder(resp.AcceptOrder);
                    self.DoCcProcess(resp.DoCcProcess);
                    self.CentinelBypass(resp.CentinelBypass);
                    self.AvsByPass(resp.AvsByPass);
                    // Process Card LookUp
                    result = true;
                    if (self.CentinelBypass() == true) {
                        return self.ProcessCreditCard();
                    }
                    else {
                        return self.ProcessCardLookUp();
                    }


                } else {
                    self.AcceptOrder(resp.AcceptOrder);
                    self.DoCcProcess(resp.DoCcProcess);
                    self.CentinelBypass(resp.CentinelBypass);
                    self.AvsByPass(resp.AvsByPass);
                    result = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(jqXHR.Message);
            }
        });
        return result;
    };

    self.ProcessCreditCard = function () {
        var result = false;

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
        }
        var state;
        if (self.BillingcountryId() == 3) {
            state = self.Stateuk();
        } else {
            state = self.State();
        }

        $.ajax({
            url: '/Cart/ProcessCreditCard',
            async: false,
            data: {
                UserName: self.FullName,
                autoRefillAmount: self.AutoRefillAmount,
                autoRefill: self.AutoRefill,
                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: state,
                Address: self.Address,
                cvv: self.CvvNumber,
                Email: self.Email,
                order_id: self.OrderId,
                CardNumber: self.CardNumber,
                CardType: self.CreditCardName,
                ExpDate: self.CardExpDate,
                Amount: self.Amount,
                PlanName: self.PlanName,
                ServiceFee: self.ServiceFee,
                ExpMonth: self.ExpMonth,
                ExpYear: self.ExpYear,
                CurrencyCode: self.CurrencyCode,
                TransactionType: "CheckOut",
                PaymentType: paymenttype,
                CardId: self.CardId,
                UserType: "new",
                FirstName: self.FirstName,
                LastName: self.LastName,
                Countryfrom: self.CountryFrom,
                Countryto: self.CountryTo,
                AniNumber: self.UnmaskPhonenumber,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                CouponCode: self.CoupanCode
            },
            type: "POST",
            success: function (resp) {
                if (resp.status == false && resp.cvv2match == "N" && resp.avsaddr == "N") {
                    self.validationmessages([]);
                    self.validationmessages.push("Credit Card’s CVV doesn’t match.");
                    self.validationmessages.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    loadPopup();
                    result = false;
                }
                else if (resp.status == false && resp.cvv2match == "N") {
                    //$("input.even_particular_input").val("");
                    self.CvvNumber("");
                    self.validationmessages([]);
                    self.validationmessages.push("Credit Card’s CVV doesn’t match. Please try again.");
                    loadPopup();
                    result = false;
                    //$("#payment_edit").trigger("click");

                } else if (resp.status == false && resp.avsaddr == "N") {
                    //$("#payment_edit").trigger("click");
                    self.validationmessages([]);
                    self.validationmessages.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    loadPopup();
                    result = false;

                } else if (resp.status == false && resp.code == "U") {
                    //$("#payment_edit").trigger("click");
                    self.validationmessages([]);
                    self.validationmessages.push("Invalid credit card information. please try again with valid card.");
                    loadPopup();
                    result = false;

                }
                else {
                    result = true;
                    window.location.href = resp.returl;
                    return false;

                    //self.validationmessages([]);
                    //self.validationmessages.push("There is issue with your credit card information.");
                    //self.validationmessages.push("Pleaes check the information and try again.");
                    //loadPopup();
                    //result = false;
                }

            },
            error: function (resp, textStatus, errorThrown) {
                result = false;
                self.validationmessages([]);

                self.validationmessages.push(resp.Message);
                loadPopup();
            }
        });
        return result;
    };

    self.ProcessCardLookUp = function () {
        var result = false;

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
        }

        var state;
        if (self.BillingcountryId() == 3) {
            state = self.Stateuk();
        } else {
            state = self.State();
        }

        $.ajax({
            url: '/Cart/ProcessCardLookUp',
            async: false,
            data: {
                UserName: self.FullName,
                autoRefillAmount: self.AutoRefillAmount,
                autoRefill: self.AutoRefill,
                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: state,
                Address: self.Address,
                cvv: self.CvvNumber,
                Email: self.Email,
                order_id: self.OrderId,
                CardNumber: self.CardNumber,
                CardType: self.CreditCardName,
                ExpDate: self.CardExpDate,
                Amount: self.Amount,
                PlanName: self.PlanName,
                ServiceFee: self.ServiceFee,
                ExpMonth: self.ExpMonth,
                ExpYear: self.ExpYear,
                CurrencyCode: self.CurrencyCode,
                TransactionType: "CheckOut",
                PaymentType: paymenttype,
                CardId: self.CardId,
                UserType: "new",
                FirstName: self.FirstName,
                LastName: self.LastName,
                Countryfrom: self.CountryFrom,
                Countryto: self.CountryTo,
                AniNumber: self.UnmaskPhonenumber,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass,
                CouponCode: self.CoupanCode
            },
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    result = true;
                    window.location.href = "/Cart/AuthenticateInfo";
                    return true;
                } else {
                    result = false;
                    self.validationmessages([]);
                    self.validationmessages.push(resp.Message);
                    loadPopup();
                    return false;
                }
            },
            error: function (resp, textStatus, errorThrown) {
                result = false;
                self.validationmessages([]);

                self.validationmessages.push(resp.Message);
                loadPopup();
            }
        });
        return result;
    };

    self.IssueNewPin = function () {
        var result = false;
        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
            result = true;
        }
        var state;
        if (self.BillingcountryId() == 3) {
            state = self.Stateuk();
        } else {
            state = self.State();
        }

        $.ajax({
            url: '/Cart/IssueNewPin',
            async: false,
            data: {
                UserName: self.FullName,
                FirstName: self.FirstName,
                LastName: self.LastName,
                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: state,
                Address: self.Address,
                Email: self.Email,
                CountryFrom: self.CountryFrom,
                CountryTo: self.CountryTo,
                CardNumber: self.CardNumber,
                CardType: self.CardType,
                ExpDate: self.CardExpDate,
                ExpMonth: self.ExpMonth,
                ExpYear: self.ExpYear,
                CoupanCode: self.CoupanCode,
                Amount: self.Amount,
                CvvNumber: self.CvvNumber,
                CardId: self.CardId,
                PaymentMethod: paymenttype,
                AniNumber: self.UnmaskPhonenumber,
                ServiceFee: self.ServiceFee,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                PaymentType: paymenttype,
                TransactionType: "CheckOut",
                CurrencyCode: self.CurrencyCode,
                UserType: "new",
            },
            type: "POST",
            success: function (resp) {
                if (resp.IssuenewpinStatus == true) {
                    $("#slider3").removeClass("active");
                    $("#slider4").addClass("active");
                    self.PurchaseTime(resp.OrderDateTime);
                    self.StatusMessage("Congratulations! Your Order is Approved.");
                    self.LocalAccessNumberList(resp.LocalAccessNumbers);
                    result = true;
                } else {
                    //$("#slider3").removeClass("active");
                    //$("#slider4").addClass("active");
                    //self.PurchaseTime(resp.ordertime);
                    //self.StatusMessage("Sorry! Your order is rejected.");
                    //self.StatusError(resp.errormsg);
                    //result = true;
                    window.location.href = "/Cart/OrderFailed";
                    return false;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                result = false;
                self.ErrorMsg(json.parse(jqXHR.responseText).Message);
            }
        });
        return result;
    };

    self.NewlogonValidation = function () {
        self.validationmessages([]);

        if (self.FirstName().length == 0 || self.LastName().length == 0 || self.ZipCode().length == 0 ||
            self.Address().length == 0 ||
            self.City().length == 0 ||
            (self.State() == undefined && self.BillingcountryId() != 3) ||
            self.Country().length == 0 ||
            self.Password().length == 0 ||
            self.Email().length == 0 || self.PhoneNumber().length < 10 ||
            (self.IsPassCodeDial() == "T" && self.PassCode().length < 4)
        ) {
            if (self.FirstName().length == 0) {
                self.validationmessages.push("First Name is a required field.");
            }
            if (self.LastName().length == 0) {
                self.validationmessages.push("Last Name is required field.");
            }
            if (self.PhoneNumber().length == 0) {
                self.validationmessages.push("Phone Number is a required field.");
            }
            else if (self.PhoneNumber().length < 10) {
                self.validationmessages.push("Please enter a 10 digit phone number.");
            }
            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is a required field.");
            }
            if (self.Address().length == 0) {
                self.validationmessages.push("Address is a required field.");
            }
            if (self.City().length == 0) {
                self.validationmessages.push("City is a required field.");
            }
            if (self.State() == undefined && self.BillingcountryId() != 3) {
                self.validationmessages.push("State is a required field.");
            }
            if (self.BillingcountryId().length == 0) {
                self.validationmessages.push("Country is a required field.");
            }
            if (self.Password().length == 0) {
                self.validationmessages.push("Password is a required field.");
            }

            if (self.Email().length == 0) {
                self.validationmessages.push("Email is a required field.");
            }
            if (self.IsPassCodeDial() == "T" && self.PassCode().length == 0) {
                self.validationmessages.push("Please enter a 4 digit Passcode.");
            }
            else if (self.IsPassCodeDial() == "T" && self.PassCode.length < 4) {
                self.validationmessages.push("Passcode must be atleast 4 digit.");
            }

            return false;
        }

        if (self.Email().length == 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(self.Email())) {
                self.validationmessages.push("Invalid Email.");
            }
            return false;
        }
        if (self.Email() != self.ConfEmail()) {
            self.validationmessages.push("Email and Confirm email doesn't match.");
            return false;
        }
        if (self.Password().length < 6 && self.Password().length > 0) {
            self.validationmessages.push("Password must be at least six digits.");
            return false;
        }

        if (self.Password() != self.ConfPassword()) {
            self.validationmessages.push("Password and Confirm password doesn't match.");
            return false;
        }
        if (self.PassCode() != self.ReconfPassCode()) {
            self.validationmessages.push("Passcode pin and confirm pin doesn't match.");
            return false;
        }

        return true;
    };

    self.personalinfovalidation = function () {

        self.validationmessages([]);

        if (self.FirstName().length == 0 || self.LastName().length == 0 || self.ZipCode().length == 0 ||
            self.Address().length == 0 ||
            self.City().length == 0 ||
            (self.State == undefined && self.BillingcountryId() != 3) ||
            self.Country().length == 0 || (self.IsPassCodeDial() == "T" && self.PassCode().length < 4)
        ) {
            if (self.FirstName().length == 0) {
                self.validationmessages.push("First Name is a required field.");
            }
            if (self.LastName().length == 0) {
                self.validationmessages.push("Last Name is required field.");
            }
            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is a required field.");
            }
            if (self.Address().length == 0) {
                self.validationmessages.push("Address is a required field.");
            }
            if (self.City().length == 0) {
                self.validationmessages.push("City is a required field.");
            }
            if (self.State() == undefined && self.BillingcountryId() != 3) {
                self.validationmessages.push("State is a required field.");
            }
            if (self.BillingcountryId().length == 0) {
                self.validationmessages.push("Country is a required field.");
            }
            if (self.IsPassCodeDial() == "T" && self.PassCode().length == 0) {
                self.validationmessages.push("Please enter a 4 digit passcode.");
            }
            else if (self.IsPassCodeDial() == "T" && self.PassCode.length < 4) {
                self.validationmessages.push("Passcode must be atleast 4 digit.");
            }

            return false;
        }
        else if (self.PassCode() != self.ReconfPassCode()) {
            self.validationmessages.push("Passcode pin and confirm pin does not match.");
        }

        return true;
    };

    self.Newlogon = function () {

        var result = false;
        if (self.IsSignuped()) {
            if (!self.personalinfovalidation()) {
                loadPopup();
                return false;
            }
        } else {
            if (!self.NewlogonValidation()) {
                loadPopup();
                return false;
            }
        }
        var state;
        if (self.BillingcountryId() == 3) {
            state = self.Stateuk();
        } else {
            state = self.State();
        }
        $.ajax({
            url: '/Account/ChechoutLogon',
            data: {
                FirstName: self.FirstName,
                LastName: self.LastName,
                Country: self.BillingcountryId,
                City: self.City,
                ZipCode: self.ZipCode,
                State: state,
                Address: self.Address,
                Email: self.Email,
                PhoneNumber: self.PhoneNumber,
                NewPwd: self.Password,
                RefererEmail: self.RefrerEmail

            },
            async: false,
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    $("#slider1").removeClass("active");
                    $("#slider2").addClass("active");
                    result = true;
                } else {
                    // $("#slider1").removeClass("active");
                    //$("#slider2").addClass("active");

                    self.validationmessages.push([]);
                        self.validationmessages.push(resp.message);
                        loadPopup();
                        result = false;
                    
                   // self.StatusError(resp.message);

                    /*self.StatusError(resp.message);
                    alert(self.StatusError());
                    self.FirstName("");
                    self.LastName("");
                    self.ZipCode("");
                    self.PhoneNumber("");
                    self.State("");
                    self.City("");
                    self.Country("");
                    self.Email("");
                    self.Password("");
                    self.ConfPassword("");
                    self.ConfEmail("");
                    self.Address("");
                    result = false;*/
                    return false;

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(json.parse(jqXHR.responseText).Message);
            }
        });

        return result;
    };

    self.BillingcountryId.subscribe(function () {
        self.Country($("#callingToId option:selected").text());

        if (self.BillingcountryId() == 3) {
            $("#firstname").attr("placeholder", "ForName");
            $("#lastname").attr("placeholder", "SurName");
            //$("#phonenumber").attr("placeholder", "ForName");
            $("#zipcode").attr("placeholder", "PostalCode");
            $("#address").attr("placeholder", "Street Address");
            $("#city").attr("placeholder", "Town");
            $("#billing-state").css("display", "none");
            $("#state-uk").css("display", "");

        } else {
            $("#firstname").attr("placeholder", "FirstName");
            $("#lastname").attr("placeholder", "LastName");
            //$("#phonenumber").attr("placeholder", "ForName");
            $("#zipcode").attr("placeholder", "ZipCode");
            $("#address").attr("placeholder", "Address");
            $("#city").attr("placeholder", "City");
            $("#billing-state").css("display", "");
            $("#state-uk").css("display", "none");

        }
        //ko.utils.arrayForEach(self.BillingCountryList(), function (item) {
        //    if (self.BillingcountryId() == item.Id) {
        //        self.Country(item.Name);
        //        if (self.BillingcountryId() == 3) {
        //            $("#firstname").attr("placeholder", "ForName");
        //            $("#lastname").attr("placeholder", "SurName");
        //            //$("#phonenumber").attr("placeholder", "ForName");
        //            $("#zipcode").attr("placeholder", "PostalCode");
        //            $("#address").attr("placeholder", "Street Address");
        //            $("#city").attr("placeholder", "Town");
        //            $("#billing-state").css("display", "none");
        //            $("#state-uk").css("display", "");

        //        } else {
        //            $("#firstname").attr("placeholder", "FirstName");
        //            $("#lastname").attr("placeholder", "LastName");
        //            //$("#phonenumber").attr("placeholder", "ForName");
        //            $("#zipcode").attr("placeholder", "ZipCode");
        //            $("#address").attr("placeholder", "Address");
        //            $("#city").attr("placeholder", "City");
        //            $("#billing-state").css("display", "");
        //            $("#state-uk").css("display", "none");

        //        }
        //    }
        //});
        self.FetchState();
    });

    self.FetchCountryListTo = function () {

        $.getJSON('/Account/GetThreeCountryFromList/', null, function (data) {
            self.BillingCountryList(data);
            var country = $("#hid-country").val();
            self.BillingcountryId(country);
            self.State("");
            self.Stateuk("");

        });
    };

    var co = 1;
    self.FetchState = function () {
        $.getJSON('/Account/GetStates/' + self.BillingcountryId(), null, function(data) {
            self.BillingStateList(data);

            var state = $("#hid-state").val();
            self.State(state);
            self.State(state);

        });
        if (co != 1) {
            self.State("");
            self.State("");
        }
        co = co + 1;
    };


    self.disablepopupval = function () {
        disablePopup();
    };

    self.editbillinfo = function () {
        $('#billinfo_edit').css('display', 'none');
        $('#billinfo_done').css('display', '');
        $('#fullname, #Addres, #Cities, #state, #ZipCode, #unmaskphone').css('display', 'none');
        $('#firstnameedit')
        //.val($('#fullname').text())
            .css('display', '')
            .focus();
        $('#lastnameedit,#addressedit, #cityedit, #stateedit, #zipcodeedit, #unmaskphoneedit')
        //.val($('#fullname').text())
            .css('display', '');

        if (self.BillingcountryId() == 3) {
            $("#firstnameedit").attr("placeholder", "ForName");
            $("#lastnameedit").attr("placeholder", "SurName");
            //$("#phonenumber").attr("placeholder", "ForName");
            $("#zipcodeedit").attr("placeholder", "PostalCode");
            $("#addressedit").attr("placeholder", "Street Address");
            $("#cityedit").attr("placeholder", "Town");
            $("#stateedit").css("display", "none");
            $("#state-edituk").css("display", "");

        } else {
            $("#firstnameedit").attr("placeholder", "FirstName");
            $("#lastnameedit").attr("placeholder", "LastName");
            //$("#phonenumber").attr("placeholder", "ForName");
            $("#zipcodeedit").attr("placeholder", "ZipCode");
            $("#addressedit").attr("placeholder", "Address");
            $("#cityedit").attr("placeholder", "City");
            $("#stateedit").css("display", "");
            $("#state-edituk").css("display", "none");

        }

    };
    self.donebillinfo = function () {
        //if (!self.validateCreditCard()) {
        //    loadPopup();
        //    return false;
        //}

        // var billstate = self.State();

        //if (self.Country() == "U.K.") {
        //    billstate = "";
        //}

        var state;
        var MyCountry = self.BillingcountryId();
        if (self.BillingcountryId() == 3) {
            state = self.Stateuk();
        } else {
            state = self.State();
        }
        self.validationmessages([]);
        $.ajax({
            url: '/Account/BillingInfo',
            data: {

                FirstName: self.FirstName,
                LastName: self.LastName,
                //Country: self.Country,
                Country: MyCountry,
                City: self.City,
                ZipCode: self.ZipCode,
                State: state,
                Address: self.Address,
                Email: self.Email,
                PhoneNumber: self.UnmaskPhonenumber

            },
            type: "POST",
            success: function (resp) {
                if (resp.status) {
                    self.GetBiillninfo();
                    $('#billinfo_edit').css('display', '');
                    $('#billinfo_done').css('display', 'none');
                    $('#firstnameedit,#lastnameedit, #addressedit, #cityedit, #stateedit, #state-edituk, #zipcodeedit, #unmaskphoneedit').css('display', 'none');
                    $('#fullname').text(self.FirstName() + " " + self.LastName()).css('display', '');
                    $('#Addres')
                    .text($('#addressedit').val())
                    .css('display', '');
                    $('#Cities')
                    .text($('#cityedit').val())
                    .css('display', '');

                    $('#ZipCode')
                    .text($('#zipcodeedit').val())
                    .css('display', '');
                    $('#unmaskphone')
                    .text($('#unmaskphoneedit').val())
                    .css('display', '');
                    if (self.BillingcountryId == 3) {
                        $('#state')
                    .text($('#state-edituk').val())
                    .css('display', '');
                    } else {
                        $('#state')
                    .text($('.optionselect').val())
                    .css('display', '');
                    }
                } else if (resp.message == "invalid state") {
                    self.validationmessages.push("Invalid state.");
                    loadPopup();
                } else {
                    self.validationmessages.push(resp.message);
                    loadPopup();
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.validationmessages.push(JSON.parse(jqXHR.responseText).Message);
                loadPopup();
            }
        });

        //$('#billinfo_edit').css('display', '');
        //$('#billinfo_done').css('display', 'none');
        //$('#firstnameedit,#lastnameedit, #addressedit, #cityedit, #stateedit, #zipcodeedit, #unmaskphoneedit').css('display', 'none');
        //$('#fullname').text(self.FirstName() + "" + self.LastName()).css('display', '');
        //$('#Addres')
        //.text($('#addressedit').val())
        //.css('display', '');
        //$('#Cities')
        //.text($('#cityedit').val())
        //.css('display', '');
        //$('#state')
        //.text($('.optionselect').val())
        //.css('display', '');
        //$('#ZipCode')
        //.text($('#zipcodeedit').val())
        //.css('display', '');
        //$('#unmaskphone')
        //.text($('#unmaskphoneedit').val())
        //.css('display', '');
    };

    self.ExistCustomerLogon = function () {
        window.location.href = "/Account/LogOn?ispromotion=" + self.IsPromotion();
    };

    self.SaveNewPending = function () {
        var result = false;

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (self.CardType() == "PayPal") {
            paymenttype = "P";
        }

        $.ajax({
            url: '/Cart/SavePendingOrder',
            async: false,
            data: {
                CardId: self.CardId,
                Amount: self.Amount,
                CountryFrom: self.CountryFrom,
                CountryTo: self.CountryTo,
                CoupanCode: self.CoupanCode,
                PaymentType: paymenttype,
                CardNumber: self.CardNumber,
                ExpiryDate: self.CardExpDate,
                CVV2: self.CvvNumber,
                FirstName: self.FirstName,
                LastName: self.LastName,
                Address1: self.Address,
                Address2: "",
                City: self.City,
                State: self.State,
                ZipCode: self.ZipCode,
                Country: self.Country,

            },
            type: "POST",
            success: function (resp) {
                window.location.href = "/Cart/Confirmation";
                result = false;
                return false;
            },
            error: function (resp, textStatus, errorThrown) {
                result = false;
                self.validationmessages([]);

                self.validationmessages.push(resp.Message);
                loadPopup();
            }
        });
        return result;
    };

    self.WithoutAutorefillProceed = function () {
        self.AutoRefill("F");
        self.disablepopupval();
        $("#payment-submit").trigger("click");
        return true;
    }
}


function DelMsg() {
    newusercheckout.CouponCodeMsg("");
}


