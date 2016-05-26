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

var autorefillViewModel;
$(document).ready(function () {
    autorefillViewModel = new AutoRefillViewModel();

    $("#Submitbtn").click({ handler: autorefillViewModel.AutoRefill });
    $("#AddCard-click").click({ handler: autorefillViewModel.AddCard });
    $("#DeActivate-btn").click({ handler: autorefillViewModel.ConfirmRemove });
    $("#close-val1").click({ handler: autorefillViewModel.disablepopupval });
    $("#close-val2").click({ handler: autorefillViewModel.DisableConfirm });
    $("#YesDelete").click({ handler: autorefillViewModel.RemoveAutoRefill });
    $("#NoDelete").click({ handler: autorefillViewModel.DisableConfirm });
    $("#edit-cr").click({ handler: autorefillViewModel.EditCreditCard });
    ko.applyBindings(autorefillViewModel, document.getElementById("edit-refill"));

    autorefillViewModel.Get();

});

function AutoRefillViewModel() {
    var self = this;
    self.PlanName = ko.observable();
    self.OrderId = $("#order_id").val();
    self.CreditCardList = ko.observableArray([]);
    self.NameOnCard = ko.observable();
    self.CreditCard = ko.observable();
    self.Month = ko.observable();
    self.Year = ko.observable();
    self.CVV = ko.observable();
    self.ExpiryYear = ko.observableArray([]);
    self.ExpiryMonth = ko.observableArray([]);
    self.YearArray = ko.observableArray([]);
    self.AmountArray = ko.observableArray([10, 20, 30, 40, 50, 90]);
    self.RefillAmount = ko.observable();
    self.checkbox = ko.observable(true);
    self.CreditNumber = ko.observable();
    self.Status = ko.observable();
    self.CardTypeChk = ko.observable("");
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
    self.validationmessages = ko.observableArray([]);

    self.IsAutorefillEnroll = ko.observable("");

    self.Get = function () {
        $.ajax({
            url: '/Account/GetAutoRefilldata',
            type: 'GET',
            data: {
                order_id: self.OrderId
            },
            success: function (resp) {
                self.PlanName(resp.Planname);
                resp.CardList.push({ CreditCardId: "2", MaskCardNumber: "Add New Credit Card" });
                self.YearArray(resp.Years);
                self.CreditCardList(resp.CardList);
                self.IsAutorefillEnroll(resp.IsAutorefillEnroll);
                self.ExpiryYear(resp.Years);
                self.CheckrefillEnroll();
            }
        });
    };

    self.CheckrefillEnroll = function () {
        if (self.IsAutorefillEnroll() == "A") {
            $("#DeActivate-btn").css("display", "");
            $("#DeActivate-btn-gray").css("display", "none");
            $("#Submitbtn").css("display", "none");
            $("#Submitbtn-gray").css("display", "");

            $("#refill-amountdd").prop("disabled", true);
            $("#refill-creditdd").prop("disabled", true);
            $("#refill-chkbox").prop("disabled", true);
        } else if (self.IsAutorefillEnroll() == "M") {
            $("#DeActivate-btn").css("display", "none");
            $("#DeActivate-btn-gray").css("display", "");
            $("#Submitbtn").css("display", "none");
            $("#Submitbtn-gray").css("display", "");

            $("#refill-amountdd").prop("disabled", true);
            $("#refill-creditdd").prop("disabled", true);
            $("#refill-chkbox").prop("disabled", true);
        } else {
            $("#DeActivate-btn").css("display", "none");
            $("#DeActivate-btn-gray").css("display", "");
            $("#Submitbtn").css("display", "");
            $("#Submitbtn-gray").css("display", "none");

            $("#refill-amountdd").prop("disabled", false);
            $("#refill-creditdd").prop("disabled", false);
            $("#refill-chkbox").prop("disabled", false);
        }
    };

    self.loadpopupval = function () {
        loading(); // loading popup
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup
        }, 500);
    };
    self.disablepopupval = function () {
        disablePopup();
    };

    self.validateCreditCard = function () {
        self.validationmessages([]);

        if (self.NameOnCard() == undefined || self.CreditCard() == undefined || self.CVV() == undefined || self.Year() == undefined || self.Month() == "Select") {
            if (self.CreditCard() == undefined) {
                self.validationmessages.push("Please enter valid credit card.");
            }

            //            if (self.Exp_Month() ==undefined) {
            //                self.validationmessages.push("Please select expiry month.");
            //            }
            if (self.Year() == undefined || self.Month() == undefined) {
                self.validationmessages.push("Please select expiry date.");
            }
            if (self.NameOnCard() == undefined) {
                self.validationmessages.push("Please enter name on card.");
            }

            if (self.CVV() == undefined) {
                self.validationmessages.push("Please enter cvv number.");
            }


            return false;
        }
        else if (self.CVV().length < 3) {
            self.validationmessages.push("Please enter valid cvv number.");
            return false;
        }
        else if (self.CreditCard().length < 15) {
            self.validationmessages.push("Please enter valid credit card.");
            return false;
        }


        else {
            var myCardNo = $("#Card_Number").val();
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


    self.AddCard = function () {
        if (!self.validateCreditCard()) {
            self.loadpopupval();
            self.CardTypeChk("");
            self.CVV("");
            self.CreditCard("");
            self.NameOnCard("");
            self.Year(null);
            self.Month(null);
            return false;
        }
        $.ajax({
            url: '/Account/AddCreditCard',
            type: 'POST',
            data: {
                NameOnCard: self.NameOnCard,
                CreditCardNo: self.CreditCard,
                Exp_Month: self.Month,
                Exp_Year: self.Year,
                CVV2: self.CVV
            },
            success: function (resp) {
                self.Status(resp.message);
                if (resp.status) {
                    self.Get();
                    self.CardTypeChk("");
                    self.CVV("");
                    self.CreditCard("");
                    self.NameOnCard("");
                    self.Year(null);
                    self.Month(null);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
    self.AutoRefill = function () {
        self.validationmessages([]);

        if (self.CreditNumber() == undefined || self.RefillAmount() == undefined || self.CreditNumber() == "2") {
            if (self.CreditNumber() == undefined) {
                self.validationmessages.push("Please select your credit or debit card");
            }
            if (self.RefillAmount() == undefined) {
                self.validationmessages.push("Please select autorefill amount");
            }
            if (self.CreditNumber() == "2") {
                self.validationmessages.push("Please select valid credit card");
            }
            self.loadpopupval();
            return false;
        }
        if (self.checkbox() == false) {
            return false;
        }
        $.ajax({
            url: '/Account/AutoRefill',
            type: 'POST',
            data: {
                order_id: self.OrderId,
                ReFill_Amount: self.RefillAmount,

                CreditCardNo: self.CreditNumber
            },
            success: function (resp) {
                self.Status(resp.status);
                self.Get();
                self.CreditNumber(null);
                self.RefillAmount(null);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Status(JSON.parse(jqXHR.responseText).ValidationMessage);
            }
        });
    };

    self.ConfirmRemove = function () {
        //if (!self.IsAutorefillEnroll()) {
        if (self.IsAutorefillEnroll() != "A") {
            return false;
        }
        loadPopup1();
    };

    self.DisableConfirm = function () {
        disablePopup1();
    };

    self.RemoveAutoRefill = function () {

        $.ajax({
            url: '/Account/RemoveAutoRefill',
            type: 'POST',
            data: {
                order_id: self.OrderId
            },
            success: function (resp) {
                self.Status(resp.status);
                self.Get();
                self.DisableConfirm();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Status(JSON.parse(jqXHR.responseText).ValidationMessage);
            }
        });
    };


    self.EditCreditCard = function () {
        if (self.CreditNumber() == undefined) {
            self.validationmessages.push("Please select your credit card");
            self.loadpopupval();
            return false;
        }
        $.ajax({
            url: '/Account/EditCreditCard',
            type: 'Get',
            cache: false,
            data: {
                creditCardId: self.CreditNumber
            },
            success: function (resp) {
                $("#edit-creditcard").html(resp);
                $("#edit-creditcard").css('display', 'block');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

}



function loading() {
    $("div.loader").show();
}
function loadPopup1() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#Deleteconfirmation").fadeIn(0500); // fadein popup div
        $("#backgroundValidation").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundValidation").fadeIn(0001);
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

function disablePopup1() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#Deleteconfirmation").fadeOut("normal");
        $("#backgroundValidation").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}

