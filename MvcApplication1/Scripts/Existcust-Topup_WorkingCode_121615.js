

function loadPopupconfirm() { //popup for confirm the topup.
    if (popupStatus == 0) { // if value is 0, show popup
       // closeloading(); // fadeout loading
        $("#contact-imail").fadeIn(0500); // fadein popup div
        $("#mask-imail").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#mask-imail").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}

function disablepopupconfirm() {
    jQuery('div#contact-imail, div#mask-imail').stop().fadeOut('slow');
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

var topupmobile;
$(document).ready(function () {
    $("#status li:first").addClass("active");

    topupmobile = new TopupMobile();

    $("#add-newcard").click({ handler: topupmobile.AddNewCard });
    $("#close-val").click({ handler: topupmobile.DisableDivpopup });
    $("#backgroundPopup").click({ handler: topupmobile.DisableDivpopup });
    $("#close-imail").click({ handler: topupmobile.DisablePopUpConfirm });

    ko.applyBindings(topupmobile, document.getElementById("inner_body_container"));
    topupmobile.FetchCountryListTo();
    
    topupmobile.GetCountryOperatorData();
    topupmobile.Getexistingcards();
    topupmobile.Get();
    

    var root = $("#wizard").scrollable();
    // some variables that we need

    var api = root.scrollable(), drawer = $("#drawer");
    // validation logic is done inside the onBeforeSeek callback
    api.onBeforeSeek(function (event, i) {

        // we are going 1 step backwards so no need for validation
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
            if (empty.length) {
                // slide down the drawer
                drawer.slideDown(function () {

                    // colored flash effect
                    drawer.css("backgroundColor", "#229");
                    setTimeout(function () { drawer.css("backgroundColor", "#fff"); }, 1000);
                });

                // add a CSS class name "error" for empty & required fields
                empty.addClass("error");

                // cancel seeking of the scrollable by returning false
                return false;
                // everything is good

            } else {

                // hide the drawer
                //drawer.slideUp();
                if (i == 1) {
                    var res = topupmobile.ValidateMobileTopUpInfo();
                    if (res != false) {
                        $("#slider1").removeClass("active");
                        $("#slider2").addClass("active");
                    }
                    return res;
                }
                else if (i == 2) {
                   
                    if (topupmobile.ValidationforTopup() == false) {
                        return false;
                    }
                    if (topupmobile.ValidateCouponCode() == false) {
                        return false;
                    }
                    if (topupmobile.ValidateCardInfo() == false) {
                        return false;
                    }
                    if (topupmobile.IsCentinelAllowed() == true) {
                        if (topupmobile.CentinelBypass() == true)
                        {
                            topupmobile.ProcessCreditCard();
                        }
                        else
                        {
                            if (topupmobile.ProcessCardLookUp() == true) {
                                window.location.href = "/Cart/AuthenticateInfo";
                            }
                        }
                        return false;
                    }
                    var res1 = topupmobile.MobileTopUpRecharge();
                    
                    return res1;
                }
                else if (i == 3) {
                    
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

function TopupMobile() {
    var self = this;

    var phonenumber = $("#hidden-phonenumber").val();
    
    self.AcceptOrder = ko.observable("");
    self.DoCcProcess = ko.observable("");
    self.CentinelBypass = ko.observable("");
    self.AvsByPass = ko.observable("");

    self.TopupMobileNumber = ko.observable(phonenumber);
    self.TopupCountryId = ko.observable("");
  
    self.OperatorName = ko.observable("");
    self.OperatorImageName = ko.observable("");
    self.Opearatordatalist = ko.observableArray([]);
    self.AllCountryOpearatorlist = ko.observableArray([]);
    self.DestinationAmount = ko.observable("");
    self.OperatorCode = ko.observable("");
    self.SourceAmount = ko.observable("");
    self.SourceAmountWithSign = ko.observable("");
    self.OperatorName = ko.observable("");
    self.SelectedOperatorName = ko.observable("");
    self.CountryCode = ko.observable("");
    self.DestCurrency = ko.observable("");

    self.CountryListTo = ko.observableArray([]);
    self.Validationarray = ko.observableArray([]);
    self.NewCardType = ko.observable("");
    self.NewCardNumber = ko.observable("");
    self.NewExpMonth = ko.observable("");
    self.NewExpYear = ko.observable("");
    self.NewCvvNumber = ko.observable("");
    self.NewNameonCard = ko.observable("");

    self.CardNumber = ko.observable("");
    self.CreditCardName = ko.observable("");
    self.SelectedCardCvv = ko.observable("");
    self.SelectedCardType = ko.observable("");
    self.Exp_date = ko.observable("");
    self.PaymentMethod = ko.observable("Credit card");
    self.CardList = ko.observableArray([]);
    self.chosenItems = ko.observableArray([]);
    self.CouponCode = ko.observable("");

    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Country = ko.observable("");
    self.City = ko.observable("");
    self.ZipCode = ko.observable("");
    self.State = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");
    self.CouponCodeMsg = ko.observable("");
    self.CreditcardMsg = ko.observable("");
    self.CarierStatus = ko.observable("");
    self.IsAutoOperatorFind = ko.observable("");
    self.NewOrderId = ko.observable("");
    self.TopUpStatus = ko.observable("");

    self.SelectedMaskCard = ko.computed(function () {
        if (self.CardNumber().length > 0) {
            return (self.CardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CardNumber()).slice(12);
        } else {
            return null;
        }
        
    }, this);


    self.MobileNumberWithCode = ko.computed(function () {
        return self.CountryCode() + self.TopupMobileNumber();
    }, this);

    self.TextAreaAmount =  ko.computed(function () {
        return "Amount Sent: " + self.DestinationAmount() +" "+ self.DestCurrency();
    }, this);

    self.DisplayCountryCode = ko.computed(function () {
        return "+" + self.CountryCode();
    }, this);

    
    self.SelectedOperatorName.subscribe(function () {
        if (self.SelectedOperatorName() != undefined) {
            self.OperatorName(self.SelectedOperatorName());
            self.GetSelectedOperatorAmount();
        }
    });

    // if an change happen in mobile number operator data will update..
    self.TopupMobileNumber.subscribe(function () {
        if (self.TopupMobileNumber().length > 5) {
            self.GetCountryOperatorData();
        }
    });
    
    // if any change happen in country droapdown..
    var tp_count = 1;
    self.TopupCountryId.subscribe(function () {
        if (self.TopupCountryId() != undefined) {
            if (tp_count == 1) {
                self.GetCountryOperatorData();
                tp_count = tp_count + 1;
            }
            else {
                self.TopupMobileNumber("");
                document.getElementById("topup-mobilenum").focus();
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

    self.SelectedExpMonth = ko.computed(function () {
        return self.Exp_date().split('/')[0];
    }, this);

    self.SelectedExpYear = ko.computed(function () {
        return self.Exp_date().split('/')[1];
    }, this);

    self.SelectedExpDate = ko.computed(function () {
        return self.SelectedExpMonth() + self.SelectedExpYear();
    }, this);

    self.GetCountryCode = function () {
        ko.utils.arrayForEach(self.CountryListTo(), function (item) {
            if (item.id == self.TopupCountryId()) {
                self.CountryCode(item.CountCode);
              
            }

        });
    };

    self.TopupMobilewithCode = ko.computed(function () {
        return "+"+ self.CountryCode() + " " + self.TopupMobileNumber();
    }, this);


    self.Get = function () {
        $.ajax({
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
            }
        });
    };


    self.ValidateCouponCode = function () {
        self.CouponCodeMsg("");
        var result = false;
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
       
     
        var paypalvalue = $('input[name=paymentType]:radio:checked').val();
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

    //for updating the credit card 

 
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
        self.NewCardNumber(self.ExpirySelectedCard());
        self.NewNameonCard(self.ExpCreditCardName());
        //  alert(self.ExpiryCreditCardName());
        if (self.ExpiryCreditCardName("Visa")) {

            self.NewCardType("Visa");
        }
        else if (self.ExpiryCreditCardName("Master")) {
            self.NewCardType("Master");
        }
        else if (self.ExpiryCreditCardName() == "Amex") {
            self.NewCardType("Amex");
        }
        else if (self.ExpiryCreditCardName("Discover")) {

            self.NewCardType("Discover");
        }


    }













    self.IsCentinelAllowed = function () {
        var result = false;
        var paymenttype = "C";

        $.ajax({
            url: '/Cart/CcProcessValidation',
            data: {
                userType: "old",
                orderType: "Topup",
                PaymentMethod: paymenttype,
                CardNumber: self.CardNumber,
                Country: self.Country,
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

                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.SelectedExpDate,
                Amount: self.SourceAmount,
                //PlanName: $("#PlanName").val(),
                //ServiceFee: $("#ServiceFee").val(),
                CurrencyCode: "USD",
                TransactionType: "TopUp",
                PaymentType: paymenttype,
                // CardId: self.CardId,
                CouponCode: self.CouponCode,
                FirstName: self.FirstName,
                LastName: self.LastName,
                ExpMonth: self.SelectedExpMonth,
                ExpYear: self.SelectedExpYear,
                TopupMobileNumber: self.TopupMobileNumber,
                TopupCountry: self.TopupCountryId,
                TopupOperatorCode: self.OperatorCode,
                TopupOperatorName: self.OperatorName,
                TopupSourceAmount: self.SourceAmount,
                TopupDestAmount: self.DestinationAmount,
                TopUpCountryCode:self.CountryCode,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass
            },
            type: "POST",
            success: function (resp) {
                   //window.location.href = resp.returl;
                //result = false;




                //********************** START OF CODE ADDED BY SABAL ON 02/04/2015 *******************************
                if (resp.status == false && resp.cvv2match == "N" && resp.avsaddr == "N") {
                    self.Validationarray([]);
                    self.Validationarray.push("Credit Card’s CVV doesn’t match.");
                    self.Validationarray.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    loadPopupforVal();
                    result = false;
                } else if (resp.status == false && resp.cvv2match == "N") {
                    //$("input.even_particular_input").val("");
                    self.Validationarray([]);
                    self.Validationarray.push("Credit Card’s CVV doesn’t match. Please try again.");
                    loadPopupforVal();
                    result = false;

                } else if (resp.status == false && resp.avsaddr == "N") {
                    self.Validationarray([]);
                    self.Validationarray.push("Your address or zip code/postal code do not match with your credit card billing address.");
                    loadPopupforVal();
                    result = false;

                }
                else if (resp.status == false && resp.code == "U") {
                    self.Validationarray([]);
                    self.Validationarray.push("Invalid credit card information. Please try again with valid card.");
                    loadPopupforVal();
                    result = false;
                } else {
                    window.location.href = resp.returl;
                }
                //********************** END OF CODE ADDED BY SABAL ON 02/04/2015 *******************************




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

                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                cvv: self.SelectedCardCvv,
                Email: self.Email,
                CardNumber: self.CardNumber,
                CardType: self.SelectedCardType,
                ExpDate: self.SelectedExpDate,
                Amount: self.SourceAmount,
                //PlanName: $("#PlanName").val(),
                //ServiceFee: $("#ServiceFee").val(),
                CurrencyCode: "USD",
                TransactionType: "TopUp",
                PaymentType: paymenttype,
                // CardId: self.CardId,
                CouponCode: self.CouponCode,
                FirstName: self.FirstName,
                LastName: self.LastName,
                ExpMonth: self.SelectedExpMonth,
                ExpYear: self.SelectedExpYear,
                TopupMobileNumber: self.TopupMobileNumber,
                TopupCountry: self.TopupCountryId,
                TopupOperatorCode: self.OperatorCode,
                TopupOperatorName: self.OperatorName,
                TopupSourceAmount: self.SourceAmount,
                TopupDestAmount: self.DestinationAmount,
                AcceptOrder: self.AcceptOrder,
                DoCcProcess: self.DoCcProcess,
                CentinelByPass: self.CentinelBypass,
                AvsByPass: self.AvsByPass,
                TopUpCountryCode: self.CountryCode,
            },
            type: "POST",
            success: function (resp) {
                if (resp.status == true) {
                    result = true;

                } else {

                    self.Validationarray([]);
                    self.Validationarray.push(resp.Message);
                    loadPopupforVal();
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

    
    self.ValidationforTopup = function () {
        self.Validationarray([]);
        var cardchk = $('.radio:checked').val();
        var terms = $('.condition_input:checked').val();
        var cvv_value;
        $(".debit-card tr input[type='radio']:checked").each(function () {
            cvv_value = $(this).parents("tr").find("input.even_particular_input_tp").val();

        });
        
            if (cardchk == undefined && cardchk != "Paypal") {
                self.Validationarray.push("Please select a credit card.");
                loadPopupforVal();
                return false;
            } else if (cvv_value == undefined || cvv_value.length < 3 && cardchk != "Paypal") {
                self.Validationarray.push("Please enter a valid cvv number.");
                loadPopupforVal();
                return false;
            }
            
         else {
            self.SelectConfig(cardchk);
        }
        if (terms == undefined) {
            self.Validationarray.push("Please check the Terms & Conditions.");
            loadPopupforVal();
            return false;
        }

        return true;
    };

   // var count = 1;
    //todo: recharge mobile top up..
    self.MobileTopUpRecharge = function () {
     
        var res = false;
        
        
        $.ajax({
            url: "/TopUpMobile/MobileTopUpRechargeExistinguser",
            async: false,
            data: {
                
                OperatorCode: self.OperatorCode,
                Operator: self.OperatorName,
                CountryId: self.TopupCountryId,
                SourceAmount: self.SourceAmount,
                DestinationAmt: self.DestinationAmount,
                DestinationPhoneNumber: self.TopupMobileNumber,
                SmsTo: self.TopupMobileNumber,
                PaymentMethod: self.PaymentMethod,
                PurchaseAmount: self.SourceAmount,
                CardNumber: self.CardNumber,
                ExpDate: self.SelectedExpDate,
                Cvv2: self.SelectedCardCvv,
                CouponCode: self.CouponCode,
                TopUpCountryCode:self.CountryCode
            },
            type: "POST",
            //success: function (resp) {
            //    if (resp.status) {
            //        self.TopUpStatus(true);
            //        self.NewOrderId(resp.orderid);
            //    } else {
            //        self.TopUpStatus(false);
            //    }
            //    $("#slider2").removeClass("active");
            //    $("#slider3").addClass("active");
            //    res = true;
            //},




            success: function (resp) {
                if (resp.status) {
                    self.TopUpStatus(true);
                    self.NewOrderId(resp.orderid);

                    $("#slider2").removeClass("active");
                    $("#slider3").addClass("active");
                    res = true;

                } else {
                    //self.TopUpStatus(false);
                    window.location.href = "TopUpFailed";
                    return false;
                }
            },





        });
        return res;
    };

   

    self.ValidateAddNewCard = function () {
        self.Validationarray([]);
      
        //alert(self.NewCardNumber() + "," + self.NewExpMonth() + "," + self.NewExpYear() + "," + self.NewCvvNumber() + "," + self.NewNameonCard() + "," + self.NewCardType());

  if (self.NewCardNumber().length < 15 || self.NewExpMonth().length == 0 || self.NewExpYear().length == 0
            || self.NewCvvNumber().length < 3 || self.NewNameonCard().length == 0 || self.NewCardType().length == 0) {

            if (self.NewCardType().length == 0) {
                 self.Validationarray.push("Please select your card type.");
            }
            if (self.NewNameonCard().length == 0) {
                self.Validationarray.push("Name on card is a required field.");
            }

            if (self.NewCardNumber().length == 0) {
                self.Validationarray.push("Please enter your card number.");
           }
           else if (self.NewCardNumber().length < 16) {
               self.Validationarray.push("Please enter a valid card number.");
           }

            if (self.NewExpMonth().length == 0 || self.NewExpYear().length == 0) {
                self.Validationarray.push("Please select expiry date.");
            }
            if (self.NewCvvNumber().length == 0) {
                self.Validationarray.push("Please enter a cvv number.");
            }
            else if (self.NewCvvNumber().length < 3) {
                self.Validationarray.push("Please enter a valid cvv number.");
            }
            return false;

        }
      
        else {
            var myCardNo = self.NewCardNumber();
            var myCardType = self.NewCardType();

            if (checkCreditCard(myCardNo, myCardType)) {
                // CARD ACCEPTED.   
            } else {
                self.Validationarray.push("Please enter a valid credit card.");

                return false;
            }
        }
        return true;
    };
    self.creditcardFulllength = ko.observable("");
    // for new credit card..
    self.AddNewCard = function () {

        //Added by sabal on 01/28/205
        self.ExpirySelectedCard = ko.computed(function () {
            return (self.CompleteCardNumber()).slice(0, 4) + "xxxxxxxx" + (self.CompleteCardNumber()).slice(12);
        }, this);
        //************************



        self.creditcardFulllength(self.NewCardNumber());
        //alert("Hello World., " + self.NewCardNumber() + "," + self.ExpirySelectedCard());
     if (self.NewCardNumber() == self.ExpirySelectedCard()) {

        //self.CreditCard(self.CompleteCardNumber());
            self.creditcardFulllength(self.CompleteCardNumber());
        }
       if (!self.ValidateAddNewCard()) {
            //todo: diplay a popup div here.
            loadPopupforVal();
            return false;
        }
    
       //alert(self.creditcardFulllength());
        $.ajax({
            url: '/TopUpMobile/AddCreditCard/',
            data: {
                creditCard: self.creditcardFulllength,
                exp_Month: self.NewExpMonth,
                exp_Year: self.NewExpYear,
                nameOnCard: self.NewNameonCard,
                cVV: self.NewCvvNumber
            },
            type: "Post",
            success: function (resp) {
                if (resp.status == true) {
                    self.Getexistingcards();
                }
                self.NewCvvNumber("");
                self.NewNameonCard("");
                self.NewCardNumber("");
                self.NewCardType("");
                self.NewExpYear(null);
                self.NewExpMonth(null);
                self.CreditcardMsg(resp.message);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.statusMessage(json.parse(jqXHR.responseText).message);
            }
        });
    };


    // get users existing top 3 cards..
    self.Getexistingcards = function() {
        $.ajax({
            url: '/TopUpMobile/GetCreditCard/',
            type: "GET",
            success: function (data) {
                self.CardList(data.CardList);

                if (data.CardList[0].CardStatus == "Update") {

                    self.ExpCard(data.CardList[0].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[0].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[0].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[0].CreditCardType)


                }
                if (data.CardList[1].CardStatus == "Update") {
                    self.ExpCard(data.CardList[1].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[1].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[1].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[1].CreditCardType)


                }
                if (data.CardList[2].CardStatus == "Update") {
                    self.ExpCard(data.CardList[2].CreditCardNo);
                    self.CompleteCardNumber(data.CardList[2].CreditCardNumber);
                    self.ExpCreditCardName(data.CardList[2].CreditCardName);
                    self.ExpiryCreditCardName(data.CardList[2].CreditCardType)

                }
                //self.Year(data.Years);
                //self.Selected(self.CardList());
                //self.Exp_date(self.CardList());
                //self.CardNumber(self.CardList());
            }
        });
    };


    //get selected card data on radio select..
    self.SelectConfig = function(card) {

        if (card == "Paypal") {
            self.CardNumber("");
            self.CardNumber("");
            self.SelectedCardCvv("");
            self.SelectedCardType("Paypal");
            self.CreditCardName("Paypal");
            self.Exp_date("");
        } else {
            ko.utils.arrayForEach(self.CardList(), function(item) {
                if (item.CreditCardNumber == card) {
                    self.CardNumber(item.CreditCardNumber);
                    self.SelectedCardType(item.CreditCardType);
                    self.CreditCardName(item.CreditCardName);
                    self.Exp_date(item.ViewExpDate);
                }
            });
            var cvv_value;
            $(".debit-card tr input[type='radio']:checked").each(function () {
                cvv_value = $(this).parents("tr").find("input.even_particular_input_tp").val();

            });
            
            self.SelectedCardCvv(cvv_value);
        };

    }
    // call when we change the amount..
    self.SourceAmount.subscribe(function () {
        self.GetSelectAmountData();
    });

    // update data when amount change the amount or select..
    self.GetSelectAmountData = function() {
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

    //Get selected country and mobile number operators data...
    self.GetCountryOperatorData = function () {
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
              //  alert(jqXHR);
            }
        });
        return true;
    };

    // fetch country list..
    self.FetchCountryListTo = function () {
        $.getJSON('/Account/GetCountryToList/', null, function (data) {
            self.CountryListTo(data);
            var countryid = $("#hidden-countryid").val();
            self.TopupCountryId(countryid);
        });
    };

    
    var count = 1;
    // validation on 1st tab..
    self.ValidateMobileTopUpInfo = function () {
        var val_response = true;
        
        self.Validationarray([]);
        if (self.TopupMobileNumber().length == 0 || self.SourceAmount() == undefined || self.TopupCountryId() == undefined) {
            if (self.TopupMobileNumber().length == 0) {
                self.Validationarray.push("Mobile number is a required field.");
                val_response = false;
            }
            else if (self.Opearatordatalist().length == 0) {
                self.Validationarray.push("Invalid mobile number or country.");
                val_response = false;
            }
            else if (self.SourceAmount() == undefined) {
                self.Validationarray.push("Please select purchase amount. ");
                val_response = false;
            }
            else if (self.TopupCountryId() == undefined) {
                self.Validationarray.push("Country is a required field. ");
                val_response = false;
            }
            //todo: show popup div for validation.
            loadPopupforVal();
        } else {
            if (count == 1) {
                loadPopupconfirm();
                count = count + 1;
                val_response = false;
            }
        }
        return val_response;

    };

    self.DisablePopUpConfirm=function() {
        disablePopupconfirm();
    }

    self.DisableDivpopup = function() {
        disablePopupforval();
    };

};

function ClearCouponMsg() {
    topupmobile.CouponCodeMsg("");
}