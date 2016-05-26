
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

function show_form() {
    $("#credit").slideDown("slow");
}
function close_form() {
    $("#credit").slideUp("slow");

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

var rechargeViewModel;
var api;
var tabCounter;
$(document).ready(function () {
    reviewmodel = new ReviewModel();

    $("#close-val").click({ handler: reviewmodel.DisablePopupVal });
    $("#backgroundPopup").click({ handler: reviewmodel.DisablePopupVal });
    $("#edit-billinfo").click({ handler: reviewmodel.Editbillinfo });
    $("#done-billinfo").click({ handler: reviewmodel.UpdateBillingInfo });

    ko.applyBindings(reviewmodel, document.getElementById("review-bind"));
    reviewmodel.GetBillingInfo();
    //ReviewModel.GetCard();

    // rechargeViewModel.GetCart();


});

function ReviewModel() {
    var self = this;

    self.CVV = ko.observable("");

    var nameoncard = $("#hid-cardname").val();
    self.NameOnCard = ko.observable(nameoncard);
    var cardnumber = $("#hid-cardnumber").val();
    self.CreditCard = ko.observable(cardnumber);
    var cardtype = $("#hid-cardtype").val();

    self.CardTypeChk = ko.observable(cardtype);
    var expyear = $("#hid-expyear").val();
    self.Exp_Year = ko.observable(expyear);
    var expmonth = $("#hid-expmonth").val();
    self.Exp_Month = ko.observable(expmonth);

    self.CreditcardMsg = ko.observable("");
    self.validationmessages = ko.observableArray([]);

    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Address = ko.observable("");
    self.State = ko.observable();
    self.City = ko.observable();
    self.Country = ko.observable();
    self.ZipCode = ko.observable();
    self.Email = ko.observable();
    self.PhoneNumber = ko.observable();
    self.ExpirySelectedYear = ko.computed(function () {
        return (self.Exp_Year()).slice(2);
    }, this);

    self.Editbillinfo = function () {
        $("#billingspan-edit").css("display", "");
        $("#billing-span").css("display", "none");
        $("#edit-billinfo").css("display", "none");
        $("#done-billinfo").css("display", "");

    };

    self.DoneBillingInfo = function () {
        self.UpdateBillingInfo();
        $("#billingspan-edit").css("display", "none");
        $("#billing-span").css("display", "");
        $("#edit-billinfo").css("display", "");
        $("#done-billinfo").css("display", "none");
    };

    self.GetBillingInfo = function () {
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

    self.UpdateBillingInfo = function () {
        self.validationmessages([]);
        if (self.ValidateBillingInfo() == false) {
            loadPopup();
            return false;
        }
        $.ajax({
            url: '/Account/BillingInfo',
            data: {

                FirstName: self.FirstName,
                LastName: self.LastName,
                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,
                Email: self.Email,
                PhoneNumber: self.PhoneNumber

            },
            type: "POST",
            success: function (resp) {
                if (resp.status) {
                    $("#billingspan-edit").css("display", "none");
                    $("#billing-span").css("display", "");
                    $("#edit-billinfo").css("display", "");
                    $("#done-billinfo").css("display", "none");
                    return true;
                }
                else {
                    self.validationmessages.push(resp.message);
                    loadPopup();
                    return false;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
        });

    };

    self.UpdateCard = function () {
        var result = false;
        //if (!self.validateCreditCard()) {

        //    return false;
        //}
        $.ajax({
            url: '/Recharge/AddCreditCard/',
            async: false,
            data: {
                creditCard: self.CreditCard,
                exp_Month: self.Exp_Month,
                exp_Year: self.ExpirySelectedYear,
                nameOnCard: self.NameOnCard,
                cVV: self.CVV
            },
            type: "Post",
            success: function (resp) {

                if (resp.status == true) {
                    result = true;
                    return true;
                } else {
                    self.validationmessages([]);
                    self.validationmessages.push(resp.message);
                    loadPopup();
                    result = false;
                    return false;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.statusMessage(json.parse(jqXHR.responseText).message);
            }
        });
        return result;
    };

    self.ReSubmitOrder = function () {
        if (!self.ValidateEditCreditCard()) {
            loadPopup();
            return false;
        }
        if (self.UpdateCard()) {
            return true;
        } else {
            return false;
        }
    };

    self.ValidateBillingInfo = function () {
        self.validationmessages([]);
        if ((self.State().length == 0 && self.Country() != "U.K.") || self.ZipCode().length == 0 || self.Address().length == 0 ||
            self.City().length == 0 || self.FirstName().length == 0 || self.LastName().length == 0) {
            if (self.FirstName().length == 0) {
                self.validationmessages.push("First Name is a required field.");
            }
            if (self.LastName().length == 0) {
                self.validationmessages.push("Last Name is required field.");
            }
            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is required field.");
            }
            if (self.Address().length == 0) {
                self.validationmessages.push("Address is a required field.");
            }
            if (self.City().length == 0) {
                self.validationmessages.push("City is a required field.");
            }
            if (self.State().length == 0 && self.Country() != "U.K.") {
                self.validationmessages.push("State is a required field.");
            }

            return false;
        }
        return true;
    };

    self.ValidateEditCreditCard = function () {
        self.validationmessages([]);
        if ((self.CardTypeChk() == undefined || self.CardTypeChk().length == 0) || (self.NameOnCard() == undefined || self.NameOnCard().length == 0)
            || (self.CreditCard() == undefined || self.CreditCard().length == 0) ||
            (self.Exp_Month() == undefined || self.Exp_Month().length == 0 || self.Exp_Year() == undefined || self.Exp_Year().length == 0) ||
             (self.CVV() == undefined || self.CVV().length == 0)
            ) {
            if (self.CardTypeChk() == undefined || self.CardTypeChk().length == 0) {
                self.validationmessages.push("Please select your card type.");
            }
            if (self.NameOnCard() == undefined || self.NameOnCard().length == 0) {
                self.validationmessages.push("Please enter your Cardholder Name.");
            }
            if (self.CreditCard() == undefined || self.CreditCard().length == 0) {
                self.validationmessages.push("Please enter a  valid Credit Card.");
            }
            if (self.Exp_Month() == undefined || self.Exp_Month().length == 0 || self.Exp_Year() == undefined || self.Exp_Year().length == 0) {
                self.validationmessages.push("Please select expiry date.");
            }
            if (self.CVV() == undefined || self.CVV().length == 0) {
                self.validationmessages.push("Please enter a  valid Cvv Number.");
            }
            return false;
        }
        else if (!checkCreditCard(self.CreditCard(), self.CardTypeChk())) {
            self.validationmessages.push("Please enter a valid credit card.");
            return false;
        }
        return true;
    };

    self.DisablePopupVal = function () {
        disablePopup();
    };

};

