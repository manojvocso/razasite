
function loading() {
   // $("div.loader").show();
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
   // $("div.loader").fadeOut('normal');
}

var popupStatus = 0; // set value

function disablePopup() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#errorValidation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}

var rechargeViewModel;
var api;
var tabCounter;
$(document).ready(function () {
    tabCounter = 1;
    $("input.even_particular_input").val("");
    var root = $("#wizard").scrollable();

    // some variables that we need
    api = root.scrollable(), drawer = $("#drawer");

    $("#f_fixed").css({ "visibility": "visible" });
    $("#status li:first").addClass("active");

    rechargeViewModel = new RechargeViewModel();
    $("#close-val").click({ handler: rechargeViewModel.disablepopupval });
    $("#backgroundPopup").click({ handler: rechargeViewModel.disablepopupval });
    $("#Updatebtnclick").click({ handler: rechargeViewModel.Update });
    // $("#Recharge_submitbtn").click({ handler: rechargeViewModel.Recharge });
    $("#billinginfo_edit").click({ handler: rechargeViewModel.editbillinfo });
    $("#billinginfo_done").click({ handler: rechargeViewModel.donebillinfo });
    $("#process-withoutrefill").click({ handler: rechargeViewModel.UnselectAutorefillProced });
    // $("#Update-btn-recharge").click({ handler: rechargeViewModel.SubmitDetails });
    //    $("#Update-btn").click({ handler: rechargeViewModel.SubmitDetails });

    ko.applyBindings(rechargeViewModel, document.getElementById("edit-Recharge"));
    rechargeViewModel.GetCard();
    rechargeViewModel.Get();
    rechargeViewModel.GetCart();

    $("#payment_edit").click(function () {

        // seeks to prev tab by executing our validation routine
        tabCounter = tabCounter - 1;
        api.prev();


        //   e.preventDefault();
    });


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

            //  var e = document.getElementById("recharge-amount");
            // var strUser = e.options[e.selectedIndex].value;
            var checkedValue = $('.terms_chackbox:checked').val();
            var radio = $('.radio:checked').val();
            var paypalvalue = $('input[name=paymentType]:radio:checked').val();
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
                return false;
            }
                // everything is good
            else {

                if (tabCounter == 1) {
                    var selcvv_value;
                    $(".debit-card tr input[type='radio']:checked").each(function () {
                        selcvv_value = $(this).parents("tr").find("input.even_particular_input").val();

                    });
                    $("#paypal-hidmsg").css("display", "none");
                    rechargeViewModel.validationmessages([]);
                    if (checkedValue == undefined || radio == undefined || ((selcvv_value == undefined || selcvv_value.length < 3) && radio != "Paypal") ||
                        (rechargeViewModel.AutoRefill() == "T" && (rechargeViewModel.AutoRefillAmount() == "select" || rechargeViewModel.AutoRefillAmount() == undefined)) ||
                        (rechargeViewModel.IsPassCodeDial() == "T" && rechargeViewModel.PassCode().length < 4)) {
                        if (checkedValue == undefined) {
                            $("#val2-rech1").css("display", "");
                            $("#status-user").html("Please check the Terms & Conditions.");
                        } else {
                            $("#val2-rech1").css("display", "none");
                            $("#status-user").html("");
                        }
                        if (radio == undefined) {
                            $("#val2-rech3").css("display", "");
                            $("#status-user2").html("Please select your credit card");
                        } else {
                            $("#val2-rech3").css("display", "none");
                            $("#status-user2").html("");
                        }
                        if ((selcvv_value == undefined || selcvv_value.length < 3) && radio != "Paypal" && radio != undefined) {
                            $("#val2-cvv").css("display", "");
                            $("#status-cvv").html("Please enter a valid cvv number.");
                        } else {
                            $("#val2-cvv").css("display", "none");
                            $("#status-cvv").html("");
                        }
                        if (rechargeViewModel.AutoRefill() == "T" && (rechargeViewModel.AutoRefillAmount() == "select" || rechargeViewModel.AutoRefillAmount() == undefined)) {
                            $("#val2-rech4").css("display", "");
                            $("#status-user3").html("Please select autorefill amount.");
                        } else {
                            $("#val2-rech4").css("display", "none");
                            $("#status-user3").html("");
                        }
                        if (rechargeViewModel.IsPassCodeDial() == "T" && rechargeViewModel.PassCode().length == 0) {
                            $("#val2-rech5").css("display", "");
                            $("#status-user4").html("Please enter a 4 digit passcode.");
                        }
                        else if (rechargeViewModel.IsPassCodeDial() == "T" && rechargeViewModel.PassCode().length < 4) {
                            $("#val2-rech5").css("display", "");
                            $("#status-user4").html("Please enter a 4 digit passcode.");
                        } else {
                            $("#val2-rech5").css("display", "none");
                            $("#status-user4").html("");
                        }
                        $("#val2-rech").css("display", "");
                        loadPopup();
                        return false;
                    }
                    else if (rechargeViewModel.PassCode() != rechargeViewModel.ReconfPassCode()) {
                        $("#val2-rech1").css("display", "");
                        $("#status-user").html("Passcode pin and confirm pin does not match.");

                        $("#val2-rech2").css("display", "none");
                        $("#val2-rech3").css("display", "none");
                        $("#val2-rech4").css("display", "none");
                        $("#val2-rech5").css("display", "none");
                        $("#val2-cvv").css("display", "none");
                        $("#val2-rech").css("display", "");
                        loadPopup();
                        return false;
                    }

                    else if (rechargeViewModel.AutoRefill() == "T" && paypalvalue == "Paypal" && !rechargeViewModel.IsAutorefillMandatory()) {
                        $("#paypal-hidmsg").css("display", "");
                        loadPopup();
                        return false;
                    } else if (rechargeViewModel.ValidateCouponCode() == false) {
                        return false;
                    }
                    else if ($('.radiobtn:checked').val() == "T") {
                        $("#val2-rech").css("display", "none");
                        $("#classresult").css("display", "");
                        $("#val2-rech6").css("display", "");
                        if (!rechargeViewModel.IsAutorefillMandatory()) {
                            $("#status-user5").html("You have agreed to recharge this plan automatically when the balance falls below $2 and receive 10% free minutes on every auto-refill. If you wish not to enroll the plan to this feature please uncheck the auto-refill option and proceed.Note:  You will not be eligible to get 10% extra minutes without enrolling to auto-refill feature.");

                            loadPopup();
                            $('.radiobtn:checked').val("N");
                            return false;
                        }
                    }
                    rechargeViewModel.SelectConfig(radio);
                    tabCounter = tabCounter + 1;
                    drawer.slideUp();

                }
                else if (tabCounter == 2) {
                    // call web service
                    $("#val2-rech").css("display", "none");
                    $("#classresult").css("display", "none");
                    $("#paypal-hidmsg").css("display", "none");
                    if (rechargeViewModel.ValidateCardInfo() == false) {
                        return false;
                    }

                    if (rechargeViewModel.IsCentinelAllowed()) {

                        return false;
                    }
                    if (rechargeViewModel.AcceptOrder()) {
                        return rechargeViewModel.IssuenewPin();
                        return true;
                    }
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


function RechargeViewModel() {
    var self = this;

    self.AcceptOrder = ko.observable("");
    self.DoCcProcess = ko.observable("");
    self.CentinelBypass = ko.observable("");
    self.AvsByPass = ko.observable("");

    self.statusMessage = ko.observable("");
    self.StatusError = ko.observable("");
    // personal info
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Country = ko.observable("");
    self.City = ko.observable("");
    self.ZipCode = ko.observable("");
    self.State = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");
    self.PhoneNumber = ko.observable("");

    //payment info
    self.NameOnCard = ko.observable("");
    self.CreditCard = ko.observable("");
    self.CVV = ko.observable("");
    self.ExpiryDate = ko.observable("");
    self.Exp_Month = ko.observable("");
    self.Exp_Year = ko.observable("");

    self.Name = ko.observable("");
    self.CardTypeNo = ko.observable("");
    self.Selected = ko.observable("");
    self.Exp_date = ko.observable("");
    self.ExpDate = ko.observable("");
    //var oid = $("#order_id").val();

    self.OrderId = ko.observable("");
    self.ErrorMsg = ko.observable("");
    self.CreditCardName = ko.observable("");

    // Cart Data
    self.PurchaseAmount = ko.observable("");
    self.PurchaseAmounts = ko.observableArray([10, 20, 50, 90]);
    self.PlanId = ko.observable("");
    self.PlanName = ko.observable("");
    self.CountryFrom = ko.observable("");
    self.CountryTo = ko.observable("");
    self.CardId = ko.observable("");
    self.ServiceFee = ko.observable("");
    self.CurrencyCode = ko.observable("");
    self.UserType = ko.observable("");

    self.PassCode = ko.observable("");
    self.ReconfPassCode = ko.observable("");
    self.IsPassCodeDial = ko.observable("F");



    self.MonthArray = ko.observableArray([
    { name: "Jan.", value: "01" },
    { name: "Feb.", value: "02" },
        { name: "March.", value: "03" },
        { name: "April.", value: "04" },
        { name: "May.", value: "05" },
        { name: "June.", value: "06" },
        { name: "July.", value: "07" },
        { name: "Aug.", value: "08" },
        { name: "Sept.", value: "09" },
        { name: "Oct.", value: "10" },
        { name: "Nov.", value: "11" },
        { name: "Dec", value: "12" }
    ]);
    self.CardList = ko.observableArray([]);
    self.chosenItems = ko.observableArray([]);
    self.CardNumber = ko.observable("");
    self.RequireField = ko.observableArray([]);
    self.AutoRefill = ko.observable("F");
    self.AutoRefillAmount = ko.observable();
    self.SelectedCardCvv = ko.observable("");
    self.SelectedCardType = ko.observable("");
    self.validationmessages = ko.observableArray([]);
    self.CardTypeChk = ko.observable("");
    self.NewOrderId = ko.observable("");
    self.RequiredFields = ko.observableArray([]);
    self.Year = ko.observableArray([]);
    self.CurrentDate = ko.observable("");
    self.Isregphone = ko.observable(false);
    self.NewIsuuedPin = ko.observable("");
    self.Couponcode = ko.observable("");
    self.SelectedAmount = ko.observable("");
    self.CouponCodeMsg = ko.observable("");
    self.CreditcardMsg = ko.observable("");
    self.IsAutorefillMandatory = ko.observable(false);

    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, this);

    self.Selected = ko.computed(function () {
        return (self.CardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CardNumber()).slice(12);
    }, this);

    self.PaymentMethod = ko.computed(function () {
        if (self.CreditCardName() == "PayPal")
            return self.CardType();
        else {
            return "Credit Card";
        }
    }, this);

    self.SelectedExpMonth = ko.computed(function () {
        return self.Exp_date().split('/')[0];
    }, this);

    self.SelectedExpYear = ko.computed(function () {
        return self.Exp_date().split('/')[1];
    }, this);

    self.SelectedExpDate = ko.computed(function () {
        return self.SelectedExpMonth() + self.SelectedExpYear();
    }, this);


    //update expiry card information

    self.ExpirySelectedCard = ko.computed(function () {
        return (self.CardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CardNumber()).slice(12);
    }, this);
    self.ExpCard = ko.observable("");
    self.CompleteCardNumber = ko.observable("");
    self.ExpCreditCardName = ko.observable("");
    self.ExpiryCreditCardName = ko.observable("");

    self.UpdateExpCard = function () {

        self.ExpirySelectedCard = ko.computed(function () {
            return (self.CompleteCardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CompleteCardNumber()).slice(12);
        }, this);
        //  var radio = $('.radio:checked').val();

        // if (radio == self.CompleteCardNumber()) {
        show_form();
        self.CreditCard(self.ExpirySelectedCard());
        self.NameOnCard(self.ExpCreditCardName());
        //  alert(self.ExpiryCreditCardName());
        if (self.ExpiryCreditCardName("Visa")) {

            self.CardTypeChk("Visa");
        }
        else if (self.ExpiryCreditCardName("Master")) {
            self.CardTypeChk("Master");
        }
        else if (self.ExpiryCreditCardName() == "Amex") {
            self.CardTypeChk("Amex");
        }
        else if (self.ExpiryCreditCardName("Discover")) {

            self.CardTypeChk("Discover");
        }


    }






    self.UnmaskPhonenumber = ko.computed(function () {
        var number = self.PhoneNumber().replace("-", "");
        return number.replace("-", "");
    }, this);

    self.TotalAmount = ko.computed(function () {
        var amt = self.PurchaseAmount() + self.ServiceFee();
        return Math.floor(amt * 100) / 100;
    }, this);

    ////selected card
    //self.SelectConfig = function (card) {

    //    self.CardNumber(card.CreditCardNumber);
    //    self.CreditCardName(card.CreditCardName);
    //    self.SelectedCardCvv(card.CVV);
    //    self.SelectedCardType(card.CreditCardType);
    //    self.Exp_date(card.ViewExpDate);
    //};

    self.SelectConfig = function (number) {
        if (number == "Paypal") {
            self.CardNumber("");
            self.CardNumber("");
            self.SelectedCardCvv("");
            self.SelectedCardType("Paypal");
            self.CreditCardName("Paypal");
            self.Exp_date("");
        }
        var cvv_value;
        ko.utils.arrayForEach(self.CardList(), function (item) {
            if (item.CreditCardNumber == number) {
                self.CardNumber(item.CreditCardNumber);
                // self.SelectedCardCvv(item.CVV);
                self.SelectedCardType(item.CreditCardType);
                self.CreditCardName(item.CreditCardName);
                self.Exp_date(item.ViewExpDate);
            }
        });
        $(".debit-card tr input[type='radio']:checked").each(function () {
            cvv_value = $(this).parents("tr").find("input.even_particular_input").val();

        });
        self.SelectedCardCvv(cvv_value);
    };

    

    self.ValidateCardInfo = function () {
        var result = false;
        self.validationmessages([]);
        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (paypalvalue == "Paypal") {
            paymenttype = "P";
            result = true;
        } 
        $.ajax({
            url: '/Cart/ValidateCardInfo',
            data: {
                CardNumber: self.CardNumber,
                Amount: self.SelectedAmount,
                CardId: self.CardId,
                CouponCode: self.Couponcode,
                TransactionType: "PurchaseNewPlan",
                Countryfrom: self.CountryFrom,
                Countryto: self.CountryTo,
                PaymentType: paymenttype
            },
            async: false,
            type: "GET",
            success: function (resp) {
                if (resp.status == true) {
                    // Process Card LookUp
                    result = true;

                } else {
                        
                    self.validationmessages.push(resp.Message);
                    if (resp.Type == "B") {
                        $("#billinginfo_edit").trigger("click");
                    }
                    self.loadpopupval();
                    result = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.validationmessages.push(jqXHR.Message);
                result = false;
            }
        });
        return result;
    };


    //self.DateCalculation = ko.computed(function () {

    //    ko.utils.arrayForEach(self.CardList(), function (item) {
    //        //            var expDate = item.ExpiryDate;
    //        //            var curDate = item.CurrentDate;
    //        var res = item.CurrentDateYear;
    //        self.CurrentDate(res);

    //        //            if (expDate > curDate) {
    //        //                item.CardStatus = "Active";
    //        //            } else {
    //        //                item.CardStatus = "Update";
    //        //            }
    //    });
    //});

    //personal information
    self.Get = function () {
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

    self.SetNumber = function () {
        if (self.PassCode() != self.ReconfPassCode()) {
            return false;
        }
        $.ajax({
            url: '/Recharge/SetPassCode',
            type: 'POST',
            data: {
                order_id: $("#order_id").val(),
                PassCode: self.PassCode
            },
            success: function (resp) {
                self.statusMessage(resp.status);
                self.PassCode("");
                self.ReconfPassCode("");

            }
        });
    };


    self.GetCard = function () {

        $.ajax({
            url: '/Recharge/GetCreditCard/',
            data:
            {
                order_id: "xyz"
            },
            type: "Get",
            success: function (data) {
                self.CardList(data.CardList);
                self.CurrentDate(data.Currentdate);
                self.Year(data.Years);
                self.Selected(self.CardList());
                self.Exp_date(self.CardList());
                self.CardNumber(self.CardList());
                if (data.CardList[0].CardStatus == "Update") {

                    self.ExpCard(data.CardList[0].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[0].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[0].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[0].CreditCardType);


                }
                else if (data.CardList[1].CardStatus == "Update") {
                    self.ExpCard(data.CardList[1].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[1].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[1].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[1].CreditCardType);


                }
                else if (data.CardList[2].CardStatus == "Update") {
                    self.ExpCard(data.CardList[2].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[2].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[2].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[2].CreditCardType);

                }


            }
        });
    };

    self.GetCart = function () {
        $.ajax({
            url: "/Cart/GetCartData",
            type: "GET",
            cache: false,
            success: function (data) {
                self.PlanName(data.PlanName);
                self.PurchaseAmount(data.Price);
                self.ServiceFee(data.ServiceFee);
                self.CountryFrom(data.CountryFrom);
                self.CountryTo(data.CountryTo);
                self.CardId(data.FromToMapping);
                self.ServiceFee(data.ServiceFee);
                self.CurrencyCode(data.CurrencyCode);
                self.UserType(data.Userstatus);
                self.Couponcode(data.CouponCode);
                self.SelectedAmount(data.Price);
                if (data.IsAutoRefill) {
                    self.AutoRefill("T");
                    self.AutoRefillAmount(data.Price);
                } else {
                    self.AutoRefill("F");
                }
                if (data.IsMandatoryAutorefill == "am" || data.IsMandatoryAutorefill == "mp") {
                    $("#DisableBtn").attr('disabled', true);
                    $(".get-text").css("display", "none");
                    $('input[name="paymentType"]').attr('disabled', 'disabled');
                    self.IsAutorefillMandatory(true);
                }

            }

        });
    };

    self.ValidateCouponCode = function () {
        var result = false;
        if (self.Couponcode() == undefined || self.Couponcode().length == 0 || self.Couponcode() == "null") {
            result = true;
        } else {
            $.ajax({
                url: '/Cart/ValidateCouponCode',
                data: {
                    Amount: self.SelectedAmount,
                    CardId: self.CardId,
                    CouponCode: self.Couponcode,
                    TransactionType: "PurchaseNewPlan",
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
                        self.Couponcode("");
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

    


    self.IsCentinelAllowed = function () {
        var result = false;
        var paypalvalue = $('input[name=paymentType]:radio:checked').val();
        var paymenttype = "C";

        if (paypalvalue == "Paypal") {
            paymenttype = "P";
        }
        $.ajax({
            url: '/Cart/CcProcessValidation',
            data: {
                userType: "old",
                orderType: "NewPin",
                PaymentMethod: paymenttype,
                CardNumber: self.CardNumber,
                Country: self.CountryFrom,
                TransAmount: self.SelectedAmount
            },
            async: false,
            type: "GET",
            success: function (resp) {
                if (resp.IsValidPlan != true) {
                    self.validationmessages.push(resp.StatusMsg);
                    self.loadpopupval();
                    return false;
                }
                if (resp.AcceptOrder == true && resp.DoCcProcess == true) {
                    // Process Card LookUp
                    self.AcceptOrder(resp.AcceptOrder);
                    self.DoCcProcess(resp.DoCcProcess);
                    self.CentinelBypass(resp.CentinelBypass);
                    self.AvsByPass(resp.AvsByPass);
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
                    return false;
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

        if (paypalvalue == "Paypal") {
            paymenttype = "P";
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
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                order_id: self.OrderId,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.Exp_date().replace('/', ""),
                Amount: self.SelectedAmount,
                PlanName: $("#PlanName").val(),
                ServiceFee: self.ServiceFee,
                ExpMonth: self.SelectedExpMonth,
                ExpYear: self.SelectedExpYear,
                CurrencyCode: self.CurrencyCode,
                TransactionType: "PurchaseNewPlan",
                PaymentType: paymenttype,
                FirstName: self.FirstName,
                LastName: self.LastName,
                CardId: self.CardId,
                UserType: "Old",
                Countryfrom: self.CountryFrom,
                Countryto: self.CountryTo,
                AniNumber: self.UnmaskPhonenumber,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                CouponCode: self.Couponcode,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass
            },
            type: "POST",
            success: function (resp) {
                if (resp.status == false && resp.cvv2match == "N" && resp.avsaddr == "N") {
                    self.validationmessages([]);
                    self.validationmessages.push("Credit Card’s CVV doesn’t match.");
                    self.validationmessages.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    self.loadpopupval();
                    result = false;
                }
                else if (resp.status == false && resp.cvv2match == "N") {
                    $("input.even_particular_input").val("");
                    // $("#payment_edit").trigger("click");
                    self.validationmessages([]);
                    self.validationmessages.push("Please enter a valid CVV number.");
                    self.loadpopupval();

                } else if (resp.status == false && resp.avsaddr == "N") {
                    //$("#payment_edit").trigger("click");
                    self.validationmessages([]);
                    self.validationmessages.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    self.loadpopupval();
                    result = false;

                } else if (resp.status == false && resp.code == "U") {
                    //$("#payment_edit").trigger("click");
                    self.validationmessages([]);
                    self.validationmessages.push("Invalid Credit card information. Please try with a valid card.");
                    self.loadpopupval();
                    result = false;

                } else {
                    result = true;
                    window.location.href = resp.returl;
                }

            },
            error: function (resp, textStatus, errorThrown) {
                result = false;
                self.validationmessages([]);

                self.validationmessages.push(resp.Message);
                self.loadpopupval();
            }
        });
        return result;
    };

    self.ProcessCardLookUp = function () {
        var result = false;

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";

        if (paypalvalue == "Paypal") {
            paymenttype = "P";
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
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                order_id: self.OrderId,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.Exp_date().replace('/', ""),
                Amount: self.SelectedAmount,
                PlanName: $("#PlanName").val(),
                ServiceFee: self.ServiceFee,
                ExpMonth: self.SelectedExpMonth,
                ExpYear: self.SelectedExpYear,
                CurrencyCode: self.CurrencyCode,
                TransactionType: "PurchaseNewPlan",
                PaymentType: paymenttype,
                FirstName: self.FirstName,
                LastName: self.LastName,
                CardId: self.CardId,
                UserType: "Old",
                Countryfrom: self.CountryFrom,
                Countryto: self.CountryTo,
                AniNumber: self.UnmaskPhonenumber,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                CouponCode: self.Couponcode,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass
            },
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    result = true;
                    window.location.href = "/Cart/AuthenticateInfo";

                } else {
                    result = false;
                    self.validationmessages([]);
                    self.validationmessages.push(resp.Message);
                    self.loadpopupval();
                    //self.ErrorMsg(resp.Message);
                }
            },
            error: function (resp, textStatus, errorThrown) {
                result = false;
                self.validationmessages([]);

                self.validationmessages.push(resp.Message);
                self.loadpopupval();
            }
        });
        return result;
    };

    self.IssuenewPin = function () {

        var paypalvalue = $('input[name=paymentType]:radio:checked').val();

        var paymenttype = "C";
        var paymentmethod = "CreditCard";

        if (paypalvalue == "Paypal") {
            paymenttype = "P";
            paymentmethod = "Paypal";

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
                State: self.State,
                Address: self.Address,
                Email: self.Email,
                CountryFrom: self.CountryFrom,
                CountryTo: self.CountryTo,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.Exp_date().replace('/', ""),
                CoupanCode: self.Couponcode,
                Amount: self.SelectedAmount,
                CvvNumber: self.SelectedCardCvv,
                CardId: self.CardId,
                PaymentMethod: paymentmethod,
                AniNumber: self.UnmaskPhonenumber,
                TransactionType: "PurchaseNewPlan",
                ExpMonth: self.SelectedExpMonth,
                ExpYear: self.SelectedExpMonth,
                ServiceFee: self.ServiceFee,
                IsPasscodeDial: self.IsPassCodeDial,
                PassCodePin: self.PassCode,
                PaymentType: paymenttype,
                autoRefillAmount: self.AutoRefillAmount,
                autoRefill: self.AutoRefill,
                CurrencyCode: self.CurrencyCode,
                UserType: "Old",
                PlanName: $("#PlanName").val(),

            },
            type: "POST",
            success: function (resp) {
                if (resp.IssuenewpinStatus == true) {
                    //  self.PurchaseTime(resp.ordertime);
                    self.StatusError("Congratulations! Your Order is Approved.");
                    self.NewIsuuedPin(resp.NewPin);
                    self.NewOrderId(resp.NewOrderId);
                    tabCounter = tabCounter + 1;
                    api.next();

                } else {
                    // self.PurchaseTime(resp.ordertime);
                    //self.StatusError("Sorry! Your order is rejected.");
                    //self.StatusError(resp.errormsg);
                    //tabCounter = tabCounter + 1;
                    //api.next();
                    window.location.href = "/Cart/OrderFailed";
                    return false;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.StatusError("Sorry! Your order is rejected.");
                self.StatusError(json.parse(jqXHR.responseText).Message);
            }
        });

    };


    self.loadpopupval = function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup
        }, 500);
    };
    self.disablepopupval = function () {
        disablePopup();
    };
    self.validateCreditCard = function () {

        self.validationmessages([]);
        self.creditcardFulllength(self.CreditCard());

        if (self.CreditCard() == self.ExpirySelectedCard()) {

            //self.CreditCard(self.CompleteCardNumber());
            self.creditcardFulllength(self.CompleteCardNumber());

        }
        else if (self.CreditCard().length < 15 || self.Exp_Month() == undefined || self.Exp_Year() == undefined || self.NameOnCard().length == 0 || self.CVV().length < 3 || self.CardTypeChk() == undefined || (self.CurrentDate().split('/')[0] > self.Exp_Year() && self.CurrentDate().split('/')[1] > self.Exp_Month())) {
            if (self.CreditCard.length < 15) {
                self.validationmessages.push("Please insert valid credit card.");
            }
            //            if (self.Exp_Month() ==undefined) {
            //                self.validationmessages.push("Please select expiry month.");
            //            }
            if (self.Year() == undefined || self.Exp_Month() == undefined) {
                self.validationmessages.push("Please select expiry date.");
            }
            if (self.NameOnCard().length == 0) {
                self.validationmessages.push("Please enter required fields.");
            }

            if (self.CVV().length < 3) {
                self.validationmessages.push("Please enter valid cvv number.");
            }
            //if (self.CurrentDate() > (self.Exp_Year() + "/" + self.Exp_Month())) {
            //    self.validationmessages.push("Your credit card has been expired.");
            //}
            if (self.CurrentDate().split('/')[0] > self.Exp_Year() && self.CurrentDate().split('/')[1] > self.Exp_Month()) {
                self.validationmessages.push("Your credit card has been expired.");
            }
            return false;
        } else {
            var myCardNo = $("#CreditCard").val();
            var myCardType = self.CardTypeChk();

            if (checkCreditCard(myCardNo, myCardType)) {

            } else {
                self.validationmessages.push("Please enter valid credit card.");

                return false;
            }
        }
        //    self.searchIsVisible(false);
        return true;
    };
    self.creditcardFulllength = ko.observable("");
    self.Update = function () {



        self.validationmessages([]);
        if (!self.validateCreditCard()) {
            $("#val2-rech").css("display", "none");
            self.loadpopupval();
            //self.NameOnCard("");
            //self.CreditCard("");
            //self.CardTypeChk("");
            //self.CVV("");
            //self.Exp_Year(null);
            //self.Exp_Month(null);

            return false;
        }

        $.ajax({
            url: '/Recharge/AddCreditCard/',
            data: {
                creditCard: self.creditcardFulllength,
                exp_Month: self.Exp_Month,
                exp_Year: self.Exp_Year,
                nameOnCard: self.NameOnCard,
                cVV: self.CVV
            },
            type: "Post",
            success: function (resp) {
                if (resp.status == true) {
                    self.GetCard();
                }
                self.CVV("");
                self.NameOnCard("");
                self.CreditCard("");
                self.CardTypeChk("");
                self.Exp_Year(null);
                self.Exp_Month(null);
                self.CreditcardMsg(resp.message);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.statusMessage(json.parse(jqXHR.responseText).message);
            }
        });
    };

    self.ValidateBillingInfo = function () {
        self.validationmessages([]);


        if (self.State() == "" || self.ZipCode() == "" || self.Address() == "" || self.City() == "" || self.FirstName() == "" || self.LastName() == "") {

            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is a required field.");
            }
            if (self.Address() == "") {
                self.validationmessages.push("Address is a required field.");
            }
            if (self.City() == "") {
                self.validationmessages.push("City is a required field.");
            }
            if (self.State() == "") {
                self.validationmessages.push("State is a required field.");
            }
            if (self.FirstName() == "") {
                self.validationmessages.push("First Name is required field.");
            }
            if (self.LastName() == "") {
                self.validationmessages.push("Last Name is required field.");
            }

            return false;
        }
        return true;
    }

    self.editbillinfo = function () {
        $('#billinginfo_edit').css('display', 'none');
        $('#billinginfo_done').css('display', '');
        $('#fullname, #address, #city, #state, #ZipCode').css('display', 'none');
        $('#firstnameedit')
        //.val($('#fullname').text())
            .css('display', '')
            .focus();
        $('#lastnameedit,#addressedit, #cityedit, #stateedit, #zipcodeedit')
        //.val($('#fullname').text())
            .css('display', '');
    };

    self.donebillinfo = function () {
        if (!self.ValidateBillingInfo()) {
            loadPopup();
            return false;
        }

        var state = self.State();

        if (self.Country() == "U.K.") {
            state = "";
        }
        self.validationmessages([]);
        $.ajax({
            url: '/Account/BillingInfo',
            data: {

                FirstName: self.FirstName,
                LastName: self.LastName,
                Country: self.Country,
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
                    self.Get();
                    $('#billinginfo_edit').css('display', '');
                    $('#billinginfo_done').css('display', 'none');
                    $('#firstnameedit,#lastnameedit, #addressedit, #cityedit, #stateedit, #zipcodeedit').css('display', 'none');
                    $('#fullname').text(self.FirstName() + " " + self.LastName()).css('display', '');
                    /* $('#firstname')
                         .text($('#firstnameedit').val())
                         .css('display', '');
                     $('#lastname')
                         .text($('#lastnameedit').val())
                         .css('display', '');*/
                    $('#address')
                        .text($('#addressedit').val())
                        .css('display', '');
                    $('#city')
                        .text($('#cityedit').val())
                        .css('display', '');
                    $('#state')
                        .text($('.optionselect').val())
                        .css('display', '');
                    $('#ZipCode')
                        .text($('#zipcodeedit').val())
                        .css('display', '');
                } else if (resp.message == "invalid state") {
                    self.validationmessages.push("Invalid state.");
                    self.loadpopupval();
                } else {
                    self.validationmessages.push(resp.message);
                    self.loadpopupval();
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.validationmessages.push(JSON.parse(jqXHR.responseText).Message);
                self.loadpopupval();
            }
        });

    };

    self.UnselectAutorefillProced = function () {
        self.AutoRefill("F");
        disablePopup();
        $("#Update-btn").trigger("click");
    }

}

function DelMsg() {
    rechargeViewModel.CouponCodeMsg("");
};

