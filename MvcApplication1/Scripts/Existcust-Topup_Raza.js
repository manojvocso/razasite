$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopup(); // function close pop up
    }
});

var popupStatus = 0; // set value


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

    $("#Submit").click({ handler: opupmobile.ValidateMobileTopUpInfo });
    $("#close-val").click({ handler: topupmobile.DisableDivpopup });
    $("#backgroundPopup").click({ handler: topupmobile.DisableDivpopup });

    ko.applyBindings(topupmobile, document.getElementById("inner_body_container"));
    topupmobile.FetchCountryListTo();
    topupmobile.GetCountryOperatorData();
});




function TopupMobile() {
    var self = this;

    var phonenumber = $("#hidden-phonenumber").val();

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


    self.CarierStatus = ko.observable("");
    self.IsAutoOperatorFind = ko.observable("");
    self.NewOrderId = ko.observable("");
    self.TopUpStatus = ko.observable("");


    self.MobileNumberWithCode = ko.computed(function () {
        return self.CountryCode() + self.TopupMobileNumber();
    }, this);

    self.TextAreaAmount = ko.computed(function () {
        return "Amount Sent: " + self.DestinationAmount() + " " + self.DestCurrency();
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



    self.GetCountryCode = function () {
        ko.utils.arrayForEach(self.CountryListTo(), function (item) {
            if (item.id == self.TopupCountryId()) {
                self.CountryCode(item.CountCode);

            }

        });
    };

    self.TopupMobilewithCode = ko.computed(function () {
        return "+" + self.CountryCode() + " " + self.TopupMobileNumber();
    }, this);


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
        }
        return val_response;

    };


    self.DisableDivpopup = function () {
        disablePopupforval();
    };
};