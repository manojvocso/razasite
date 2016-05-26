var isfocus = 0;
function loadPopupconfirm() { //popup for confirm the topup.
    if (popupStatus == 0) { // if value is 0, show popup
        // closeloading(); // fadeout loading
        $("#contact-imail").fadeIn(0500); // fadein popup div
        $("#mask-imail").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#mask-imail").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}

$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopup(); // function close pop up
    }
});

var popupStatus = 0; // set value

function disablePopupconfirm() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#contact-imail").fadeOut("normal");
        $("#mask-imail").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}


function loadPopupforVal() {
    if (popup == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#toPopup1").fadeIn(0500); // fadein popup div
        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup").fadeIn(0001);
        popup = 1; // and set value to 1
    }
}


$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopupforval(); // function close pop up
    }
});

function closeloading() {
    $("div.loader").fadeOut('normal');
}

var popup = 0; // set value

function disablePopupforval() {
    if (popup == 1) { // if value is 1, close popup
        $("#toPopup1").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popup = 0; // and set value to 0
    }
}

function loadPopupforVal() {
    if (popup == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#toPopup1").fadeIn(0500); // fadein popup div
        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup").fadeIn(0001);
        popup = 1; // and set value to 1
    }
}


$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopupforval(); // function close pop up
    }
});

function closeloading() {
    $("div.loader").fadeOut('normal');
}

var popup = 0; // set value

function disablePopupforval() {
    if (popup == 1) { // if value is 1, close popup
        $("#toPopup1").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popup = 0; // and set value to 0
    }
}

var topupmobile;
$(document).ready(function () {
    $("#status li:first").addClass("active");
    $(".logonexpandable").hide();
    topupmobile = new TopupMobile();

    var root = $("#wizard").scrollable();
    // some variables that we need

    $("#topuplogin").click({ handler: topupmobile.ExistCustLogin });
    $("#close-val").click({ handler: topupmobile.DisableDivpopup });
    $("#backgroundPopup").click({ handler: topupmobile.DisableDivpopup });

    ko.applyBindings(topupmobile, document.getElementById("inner_body_container"));
    topupmobile.FetchCountryListTo();
    topupmobile.FetchCountryList();
    topupmobile.GetCountryOperators();

    var api = root.scrollable(), drawer = $("#drawer");
    // validation logic is done inside the onBeforeSeek callback

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


            if (empty.length) {
                // slide down the drawer
                drawer.slideDown(function () {
                    // colored flash effect
                    drawer.css("backgroundColor", "#229");
                    setTimeout(function () { drawer.css("backgroundColor", "#fff"); }, 1000);
                }
                );

                empty.addClass("error");

                // cancel seeking of the scrollable by returning false
                //                return false;
            }
                // everything is good
            else {

                if (i == 1) {
                    var result = topupmobile.NewCustSignup();
                    isfocus = 1;
                    return result;
                } else if (i == 2) {
                    var st = topupmobile.ValidateMobileTopupInfo();
                    return st;

                } else if (i == 3) {
                    if (topupmobile.ValidateCouponCode() == false) {
                        return false;
                    }
                    if (topupmobile.ValidateCardInfo() == false) {
                        return false;
                    }
                    if (topupmobile.IsCentinelAllowed() == true) {
                        if (topupmobile.CentinelBypass() == true) {
                            topupmobile.ProcessCreditCard();
                        } else {
                            if (topupmobile.ProcessCardLookUp() == true) {
                                window.location.href = "/Cart/AuthenticateInfo";
                            }
                        }
                        return false;
                    }
                    var status = topupmobile.MobileTopUpRecharge();
                    return status;
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
            // api.next();

            e.preventDefault();

        }

    });

});

function TopupMobile() {
    var self = this;

    self.AcceptOrder = ko.observable("");
    self.DoCcProcess = ko.observable("");
    self.CentinelBypass = ko.observable("");
    self.AvsByPass = ko.observable("");

    //Existing customer parameter.
    self.LoginEmail = ko.observable("");
    self.LoginPassword = ko.observable("");

    //Newcustomer parameter
    self.Email = ko.observable("");
    self.ConfEmail = ko.observable("");
    self.Password = ko.observable("");
    self.ConfPassword = ko.observable("");
    self.OperatorImageName = ko.observable("");

    var phonenumber = $("#hidden-phonenumber").val();
    self.TopupMobileNumber = ko.observable(phonenumber);
    //self.TopupMobileNumber = ko.observable(phonenumber);

    self.TopupCountryId = ko.observable("");

    //Mobile top info Para...
    self.CountryListTo = ko.observableArray([]);
    self.OperatorName = ko.observable("");
    self.SelectedOperatorName = ko.observable("");
    self.CountryCode = ko.observable("");
    self.OperatorCode = ko.observable("");
    self.SourceAmount = ko.observable("");
    self.SourceAmountWithSign = ko.observable("");
    self.DestinationAmount = ko.observable("");
    self.Pin = ko.observable("");
    self.PurchaseAmount = ko.observable("");
    self.BillingCountryList = ko.observableArray([]);
    self.Opearatordatalist = ko.observableArray([]);
    self.AllCountryOpearatorlist = ko.observableArray([]);
    self.Country = ko.observable("");
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.BillngPhoneNumber = ko.observable("");
    self.ZipCode = ko.observable("");
    self.Address = ko.observable("");
    self.City = ko.observable("");
    self.State = ko.observable("");
    self.BillngCountry = ko.observable("");
    self.StateList = ko.observableArray([]);
    self.DestCurrency = ko.observable("");
    self.BillingStateList = ko.observableArray([]);
    //Payment info Param...
    self.CardType = ko.observable("");
    self.CardNumber = ko.observable("");
    self.ExpMonth = ko.observable("");
    self.ExpYear = ko.observable("");
    self.CvvNumber = ko.observable("");
    self.PaymentMethod = ko.observable("Credit Card");
    self.CoupanCode = ko.observable("");
    self.CouponCode = ko.observable("");
    self.CouponCodeMsg = ko.observable("");
    self.IsAutoOperatorFind = ko.observable("");

    self.ExpDate = ko.computed(function () {
        return self.ExpMonth() + self.ExpYear();
    }, this);

    self.TextAreaAmount = ko.computed(function () {
        return "Amount Sent: " + self.DestinationAmount() + " " + self.DestCurrency();
    }, this);

    self.MobileNumberWithCode = ko.computed(function () {
        return "+" + self.CountryCode() + self.TopupMobileNumber();
    }, this);

    self.DisplayCountryCode = ko.computed(function () {
        return "+" + self.CountryCode();
    }, this);



    self.SelectedMaskCard = ko.computed(function () {
        return (self.CardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CardNumber()).slice(12);
    }, this);

    self.LoginFailMessage = ko.observable("");
    self.Validationarray = ko.observableArray([]);

    // if any change happen on billngcountrylist statelist update.
    /*self.BillngCountry.subscribe(function () {
        alert("state update");
        alert(self.BillngCountry());
        if (self.BillngCountry() != undefined) {
            self.GetStateList();
        }
    });*/


    //Fetch state basis on Country
    /*self.GetStateList = function() {
        $.ajax({
            url: "/Account/GetStates",
            data: {
                id: self.BillngCountry //self.BillngCountry
            },
            datatype: "json",
            type: "GET",
            cache: false,
            success: function(data) {
                self.StateList(data);
            }
        });
    };*/


    self.SelectedOperatorName.subscribe(function () {
        if (self.SelectedOperatorName() != undefined) {
            self.OperatorName(self.SelectedOperatorName());
            self.GetSelectedOperatorAmount();
        }
    });

    self.TopupMobileNumber.subscribe(function () {
        if (self.TopupMobileNumber().length > 5 || self.TopupCountryId() != undefined) {
            self.GetCountryOperators();
        }
    });

    var tp_count = 1;
    self.TopupCountryId.subscribe(function () {
        if (self.TopupCountryId() != undefined) {
            if (tp_count == 1) {
                self.GetCountryOperators();
                tp_count = tp_count + 1;
            }
            else {
                self.TopupMobileNumber("");
                if (isfocus == 1) {
                     document.getElementById("topup-mobilenum").focus();
                }
                self.Opearatordatalist([]);
                self.OperatorImageName("");
                self.OperatorName("");
                // self.CarierStatus("please enter your mobile number.")
            }
        }

        ko.utils.arrayForEach(self.CountryListTo(), function (item) {
            if (item.Id == self.TopupCountryId()) {
                self.CountryCode(item.CountCode);
            }
        });

    });



    // call when we change the amount..
    self.SourceAmount.subscribe(function () {
        self.GetSelectAmountData();
    });

    // update data when amount change the amount or select..
    self.GetSelectAmountData = function () {
        ko.utils.arrayForEach(self.Opearatordatalist(), function (item) {
            if (item.SourceAmount == self.SourceAmount()) {
                self.OperatorCode(item.OperatorCode);
                self.DestinationAmount(item.DestinationAmount);
                self.OperatorName(item.OperatorName);
                self.DestCurrency(item.Currency);
                self.SourceAmountWithSign(item.SourceAmountwithSign);
            }
        });
    };

    self.ValidateCouponCode = function () {
        self.CouponCodeMsg("");
        var result = false;
        if (!self.validationforMobileTopup()) {
            loadPopupforVal();
            return result;
        }
        if (self.CouponCode().length == 0) {
            result = true;
        } else {
            $.ajax({
                url: '/Cart/ValidateCouponCode',
                data: {
                    Amount: self.SourceAmount,
                    CardId: "",
                    CouponCode: self.CouponCode,
                    TransactionType: "TopUp"
                },
                async: false,
                type: "POST",
                success: function (resp) {
                    if (resp.status == true) {
                        // Process Card LookUp
                        result = true;

                    } else {
                        self.CouponCodeMsg("(Invalid or expired coupon code)");
                        self.CouponCode("");
                        result = false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    self.Validationarray.push(jqXHR.Message);
                    result = false;
                }
            });
        }
        return result;
    };

    self.ValidateCardInfo = function () {
        var result = false;

        $.ajax({
            url: '/Cart/ValidateCardInfo',
            data: {
                CardNumber: self.CardNumber,
                Amount: self.SourceAmount,
                CouponCode: self.CouponCode,
                TransactionType: "Recharge"
            },
            async: false,
            type: "GET",
            success: function (resp) {

                if (resp.status == true) {
                    // Process Card LookUp
                    result = true;

                } else {
                    self.Validationarray.push(resp.Message);
                    result = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Validationarray.push(jqXHR.Message);
                result = false;
            }
        });

        return result;

    };

    self.IsCentinelAllowed = function () {
        var result = false;
        var paymenttype = "C";
        $.ajax({
            url: '/Cart/CcProcessValidation',
            data: {
                userType: "new",
                orderType: "Topup",
                PaymentMethod: paymenttype,
                CardNumber: self.CardNumber,
                Country: self.BillngCountry
            },
            async: false,
            type: "GET",
            success: function (resp) {
                if (resp.AcceptOrder == true && resp.DoCcProcess == true) {
                    // Process Card LookUp
                    result = true;

                } else {

                    result = false;
                }
                self.AcceptOrder(resp.AcceptOrder);
                self.DoCcProcess(resp.DoCcProcess);
                self.CentinelBypass(resp.CentinelBypass);
                self.AvsByPass(resp.AvsByPass);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Validationarray.push(jqXHR.Message);
                result = false;
            }
        });


        return result;

    };

    self.ProcessCreditCard = function () {
        var result = false;

        var paymenttype = "C";

        $.ajax({
            url: '/Cart/ProcessCreditCard',
            async: false,
            data: {

                Country: self.BillngCountry,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.ExpDate,
                Amount: self.SourceAmount,
                //PlanName: $("#PlanName").val(),
                //ServiceFee: $("#ServiceFee").val(),
                CurrencyCode: "USD",
                TransactionType: "Recharge",
                PaymentType: paymenttype,
                // CardId: self.CardId,
                //CouponCode: self.CoupanCode,
                FirstName: self.FirstName,
                LastName: self.LastName,
                ExpMonth: self.ExpMonth,
                ExpYear: self.ExpYear,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass

            },
            type: "POST",
            success: function (resp) {
                result = true;
                window.location.href = resp.returl;

            },
            error: function (resp, textStatus, errorThrown) {

                self.Validationarray([]);

                self.Validationarray.push(resp.Message);
                loadPopupforVal();
                result = false;
            }
        });

        return result;
    };

    self.ProcessCardLookUp = function () {
        var result = false;

        var paymenttype = "C";

        $.ajax({
            url: '/Cart/ProcessCardLookUp',
            async: false,
            data: {

                Country: self.BillngCountry,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.ExpDate,
                Amount: self.SourceAmount,
                //PlanName: $("#PlanName").val(),
                //ServiceFee: $("#ServiceFee").val(),
                CurrencyCode: "USD",
                TransactionType: "Recharge",
                PaymentType: paymenttype,
                // CardId: self.CardId,
                //CouponCode: self.CoupanCode,
                FirstName: self.FirstName,
                LastName: self.LastName,
                ExpMonth: self.ExpMonth,
                ExpYear: self.ExpYear,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass

            },
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    result = true;

                } else {

                    self.Validationarray([]);
                    self.Validationarray.push(resp.Message);
                    self.loadpopupval();
                    result = false;

                }
            },
            error: function (resp, textStatus, errorThrown) {

                self.Validationarray([]);

                self.Validationarray.push(resp.Message);
                loadPopupforVal();
                result = false;
            }
        });

        return result;
    };

    self.GetSelectedOperatorAmount = function () {
        $.ajax({
            url: "/TopUpMobile/GetSeletedOpeartorAmountList",
            data: {
                countryid: self.TopupCountryId,
                operatorname: self.SelectedOperatorName
            },
            datatype: "json",
            type: "GET",
            cache: false,
            success: function (data) {
                self.Opearatordatalist(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //  alert(jqXHR);
            }
        });
        return true;
    };

    self.GetCountryOperators = function () {
        if (self.TopupCountryId() == undefined || self.TopupMobileNumber().length < 10) {
            return false;
        }
        $.ajax({
            url: "/TopUpMobile/GetCountryOperators",
            data: {
                mobileNumber: self.TopupMobileNumber,
                countryid: self.TopupCountryId
            },
            datatype: "json",
            type: "GET",
            cache: false,
            success: function (data) {
                self.OperatorName(data.Operator);
                self.CountryCode(data.TopUpCountryCode);
                self.Opearatordatalist(data.Operatordata);
                self.OperatorImageName(data.OperatorImage);
                self.AllCountryOpearatorlist(data.AllCountryOperatorList);
                self.IsAutoOperatorFind(data.IsAutoOperatorfind);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //  alert("error");
            }
        });
    };

    // var confstatus = 0;
    self.MobileTopUpRecharge = function () {
        var res = false;
        if (!self.validationforMobileTopup()) {
            loadPopupforVal();
            return res;
        }
        var terms = $('#terms-new-topup:checked').val();
        if (terms == undefined) {
            self.Validationarray.push("Please check the terms and condiions.");
            loadPopupforVal();
            return res;
        }

        $.ajax({
            url: "/TopUpMobile/MobileTopUpRecharge",
            async: false,
            data: {
                FirstName: self.FirstName,
                LastName: self.LastName,
                OperatorCode: self.OperatorCode,
                Operator: self.OperatorName,
                CountryId: self.TopupCountryId,
                SourceAmount: self.SourceAmount,
                DestinationAmt: self.DestinationAmount,
                DestinationPhoneNumber: self.TopupMobileNumber,
                SmsTo: self.TopupMobileNumber,
                PaymentMethod: self.PaymentMethod,
                Pin: self.Pin,
                CardNumber: self.CardNumber,
                ExpDate: self.ExpDate,
                Cvv2: self.CvvNumber,
                Address1: self.Address,
                City: self.City,
                State: self.State,
                ZipCode: self.ZipCode,
                Country: self.BillngCountry,
                BillngPhoneNumber: self.BillngPhoneNumber
            },
            type: "POST",
            success: function (resp) {
                alert("success");
                res = true;
                $("#slider3").removeClass("active");
                $("#slider4").addClass("active");
            },


        });
        return res;
    };

    self.validationforMobileTopup = function () {
        var checkedValue = $('#terms-new-topup:checked').val();

        self.Validationarray([]);
        if (self.FirstName().length == 0 || self.LastName().length == 0 || self.BillngPhoneNumber().length < 10 || self.ZipCode().length == 0 ||
            self.Address().length == 0 || self.City().length == 0 || self.State() == undefined || self.BillngCountry() == undefined ||
            self.CardType().length == 0 || self.CardNumber().length == 0 || self.ExpMonth().length == 0 || self.ExpYear().length == 0 || self.CvvNumber().length < 3 || checkedValue == undefined) {
            if (self.FirstName().length == 0) {
                self.Validationarray.push("First name is required.");
            }
            if (self.LastName().length == 0) {
                self.Validationarray.push("Last name is required.");
            }
            if (self.BillngPhoneNumber().length == 0) {
                self.Validationarray.push("Phone number is required.");
            }
            else if (self.BillngPhoneNumber().length < 10) {
                self.Validationarray.push("Please enter a ten digit valid phone number.");
            }
            if (self.ZipCode().length == 0) {
                self.Validationarray.push("Zip code is required.");
            }
            if (self.Address().length == 0) {
                self.Validationarray.push("Address is required.");
            }
            if (self.City().length == 0) {
                self.Validationarray.push("City is required.");
            }
            if (self.State() == undefined) {
                self.Validationarray.push("State is required.");
            }
            if (self.BillngCountry() == undefined) {
                self.Validationarray.push("Country is required.");
            }

            //todo: validation for payment info
            if (self.CardType().length == 0) {
                self.Validationarray.push("Card Type is required.");
            }
            if (self.CardNumber().length == 0) {
                self.Validationarray.push("Card number is required.");
            }
            if (self.ExpMonth().length == 0 || self.ExpYear().length == 0) {
                self.Validationarray.push("Expiry date is required..");
            }
            if (self.CvvNumber().length == 0) {
                self.Validationarray.push("Cvv is required.");
            }
            else if (self.CvvNumber().length < 3) {
                self.Validationarray.push("Please enter a valid Cvv number.");
            }
            return false;
        } else {
            var myCardNo = self.CardNumber();
            var myCardType = self.CardType();

            if (checkCreditCard(myCardNo, myCardType)) {
                // CARD ACCEPTED.   
            } else {
                self.Validationarray.push("Please insert valid credit card.");
                return false;
            }
        }
        return true;
    };


    // validation for second tab mobile top up info..
    var confstatus = 0;
    self.ValidateMobileTopupInfo = function () {
        var valres = false;
        self.Validationarray([]);
        if (self.TopupCountryId() == undefined || self.TopupMobileNumber().length < 10 || self.SourceAmount() == undefined) {
            if (self.TopupCountryId() == undefined) {
                self.Validationarray.push("Country is required.");
            }
            if (self.TopupMobileNumber().length == 0) {
                self.Validationarray.push("Mobile number is required.");
            }
            else if (self.TopupMobileNumber().length < 10) {
                self.Validationarray.push("Please enter a ten digit valid mobile number.");
            }
            if (self.SourceAmount() == undefined) {
                self.Validationarray.push("Amount is required.");
            }
            loadPopupforVal();
            return valres;
        } else {
            if (confstatus == 0) {
                loadPopupconfirm();
                confstatus = confstatus + 1;
                return valres;
            }
        }
        valres = true;
        $("#slider2").removeClass("active");
        $("#slider3").addClass("active");
        return valres;
    };
    self.BillngCountry.subscribe(function () {

        ko.utils.arrayForEach(self.CountryListTo(), function (item) {
            if (self.BillngCountry() == item.Id) {
                self.Country(item.Name);
            }
        });
        self.FetchState();
    });

    //Get Country List..
    self.FetchCountryListTo = function () {
        $.getJSON('/Account/GetThreeCountryFromList/', null, function (data) {
            self.BillingCountryList(data);

            var countryid = $("#hidden-countryid").val();
            self.TopupCountryId(countryid);

        });
    };

    self.FetchCountryList = function () {
        $.getJSON('/Account/GetCountryToList/', null, function (data) {
            self.CountryListTo(data);
            var countryid = $("#hidden-countryid").val();
            self.TopupCountryId(countryid);

        });
    }
    self.FetchState = function () {

        $.getJSON('/Account/GetStates/' + self.BillngCountry(), null, function (data) {
            self.BillingStateList(data);


        });

    };



    //todo: validation for login and sign up later..
    self.ValidateLogon = function () {
        self.Validationarray([]);
        if (self.Email().length == 0 || self.Password().length == 0) {
            if (self.Email().length == 0) {
                self.Validationarray.push("Email is required.");
            }
            if (self.Password().length == 0) {
                self.Validationarray.push("Password is required.");
            }
            return false;
        }
        if (self.Email().length != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(self.Email())) {
                self.Validationarray.push("Invalid Email.");
                return false;
            }
        }
        if (self.Email() != self.ConfEmail()) {
            self.Validationarray.push("Email and Confirm email doesn't match.");
            return false;
        }
        if (self.Password().length < 6) {
            self.Validationarray.push("Password must be at least six digits.");
            return false;
        }
        if (self.Password() != self.ConfPassword()) {
            self.Validationarray.push("Password and Confirm password doesn't match.");
            return false;
        }
        return true;

    };

    self.ValidateLogin = function () {
        self.Validationarray([]);
        if (self.LoginEmail().length == 0 || self.LoginPassword().length == 0) {
            if (self.LoginEmail().length == 0) {
                self.Validationarray.push("Email is required.");
            }
            if (self.LoginPassword().length == 0) {
                self.Validationarray.push("Password is required.");
            }
            return false;
        }
        if (self.Email().length != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(self.Email())) {
                self.Validationarray.push("Invalid Email.");
                return false;
            }
        }
        return true;
    };

    //Existing Customer login.
    self.ExistCustLogin = function () {
        if (!self.ValidateLogin()) {
            $(".validationerorr").hide();
            $(".logonexpandable").show();
            return false;
        }
        $(".logonexpandable").hide();
        $.ajax({
            url: '/TopUpMobile/Topuplogin',
            async: false,
            data: {
                emailAddress: self.LoginEmail,
                password: self.LoginPassword
            },
            type: "POST",
            success: function (resp) {
                if (resp.Islogin == true) {
                    var abc = $("#hidden-phonenumber").val();
                    //window.location.href = "/TopUpMobile/ExistingCustomerTopup?topupmobilenumber=" + self.TopupMobileNumber() + "&topupcountry=" + self.TopupCountryId() + "";
                    window.location.href = "/TopUpMobile/ExistingCustomerTopup?topupmobilenumber=" + abc + "&topupcountry=" + self.TopupCountryId() + "";
                } else {
                    $(".logonexpandable").show();
                    $(".validationerorr").show();

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    };

    //Signup for new customer.
    self.NewCustSignup = function () {
        var result = false;
        if (!self.ValidateLogon()) {
            loadPopupforVal();
            return result;
        }
        $.ajax({
            url: '/TopUpMobile/TopUpSignup',
            async: false,
            data: {
                Email: self.Email,
                password: self.Password,
                Phone_Number: self.TopupMobileNumber

            },
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    result = true;
                    $("#slider1").removeClass("active");
                    $("#slider2").addClass("active");
                } else {
                    self.Validationarray.push(resp.message);
                    loadPopupforVal();
                    result = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                result = false;
            }

        });
        return result;

    };

    self.DisableDivpopup = function () {
        disablePopupforval();
    };


}
