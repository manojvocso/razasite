function loading() {
 //   $("div.loader").show();
}
function loadPopup1() {

    if (popupStatus == 0) { // if value is 0, show popup
        
        $("#errorValidation").fadeIn(0500); // fadein popup div
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



var popupStatus = 0; // set value

function disablePopup1() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#errorValidation").fadeOut("normal");
        $("#backgroundValidation").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}


$(document).ready(function () {

    $("#Submit").click(function () {
        $(".showdiv").hide();
    });

    $("#show").click(function () {
        $(".showdiv").show();
    });

    var billInfoViewModel = new BillingInfoViewModel();

    $("#btnUpdate").click({ handler: billInfoViewModel.Update });
    $("#close-val1").click({ handler: billInfoViewModel.disablepopupval1 });

    ko.applyBindings(billInfoViewModel, document.getElementById("auto-refill"));

    //billInfoViewModel.FetchCountryList();
    billInfoViewModel.Get();


});

function BillingInfoViewModel() {

    var self = this;
    self.FirstName = ko.observable("").extend({ required: true });
    self.LastName = ko.observable("");
    self.Country = ko.observable("");
    self.City = ko.observable("");
    self.ZipCode = ko.observable("");
    self.State = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");
    self.ErrorMsg = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.collect = ko.observableArray([]);
    self.OldPwd = ko.observable("");
    self.NewPwd = ko.observable("");
    self.ConfPwd = ko.observable("");
    self.validationmessages = ko.observableArray([]);
    self.BillingCountryList = ko.observableArray([]);
    self.BillingStateList = ko.observableArray([]);
    self.BillingcountryId = ko.observable("");
    self.PreState = ko.observable("");

    self.valdation = function () {
        return BillingInfoViewModel();
    };
    self.Get = function () {
        $.ajax({
            //url: window.location + 'GetBillingInfo',
            url: $("#fetchdatafromurl").val(),
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
    self.loadpopupval1 = function () {
        loading();// loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup1(); // function show popup
        }, 500);
    };
    self.disablepopupval1 = function () {
        disablePopup1();
    };
    self.validate = function () {

        self.validationmessages([]);
        if (self.FirstName().length == 0 || self.LastName().length == 0 || self.ZipCode().length == 0 || self.Address().length == 0 || self.City().length == 0 ||
             ((self.State() == undefined || self.State().length == 0) && self.Country() != "U.K.") ||
             self.Country() == undefined) {
            if (self.FirstName().length == 0) {
                self.validationmessages.push("First name is required.");

            }
            if (self.LastName().length == 0) {
                self.validationmessages.push("Last name is required.");
            }
            if (self.ZipCode().length == 0) {
                self.validationmessages.push("Zipcode is required.");
            }
            if (self.Address().length == 0) {
                self.validationmessages.push("Address is required.");
            }
            if (self.City().length == 0) {
                self.validationmessages.push("City is required.");
            }
            if ((self.State() == undefined || self.State().length == 0) && self.Country() != "U.K.") {
                self.validationmessages.push("State is required.");
            }
            if (self.Country() == undefined) {
                self.validationmessages.push("Country is required.");
            }
            self.loadpopupval1();
            return false;
        }
        if (self.OldPwd().length > 0 && self.NewPwd().length == 0) {
            self.validationmessages.push("New Password is required.");
            self.loadpopupval1();
            return false;
        }
        if (self.NewPwd().length > 0 && self.NewPwd().length < 6) {
            self.validationmessages.push("Password must be at least six digits.");
            self.loadpopupval1();
            return false;
        }
        if (self.NewPwd().length > 0 && self.OldPwd().length == 0) {
            self.validationmessages.push("Old Password is required.");
            self.loadpopupval1();
            return false;
        }
        if (self.NewPwd() != self.ConfPwd()) {
            self.validationmessages.push("New password and Confirm password doesn't match.");
            self.loadpopupval1();
            return false;
        }


        return true;
    };

    self.Update = function () {
        var state = self.State();
        if (self.validate() == false)
        { return; }
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
                OldPwd: self.OldPwd,
                NewPwd: self.NewPwd,
                PhoneNumber: self.UnmaskPhonenumber

            },
            type: "POST",
            success: function (resp) {

                self.ErrorMsg(resp.message);

                self.OldPwd("");
                self.NewPwd("");
                self.ConfPwd("");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
        });

    };
    self.UnmaskPhonenumber = ko.computed(function () {
        var number = self.PhoneNumber().replace("-", "");
        return number.replace("-", "");
    }, this);
    self.Country.subscribe(function () {

        if (self.Country() == "U.K.") {
            self.State("");
            $("#State").prop("disabled", true);
        } else {
            $("#State").prop("disabled", false);
        }

    });

    //self.FetchCountryList = function () {
    //    $.getJSON('/Account/GetCountryToCountryFromList/', null, function (data) {
    //        self.BillingCountryList(data);

    //    });
    //};

    //self.FetchState = function () {
    //    $.getJSON('/Account/GetStates/' + self.BillingcountryId(), null, function(data) {
    //        self.BillingStateList(data);

    //        self.GetData();

    //    });
    //};

    //self.State = ko.computed(function () {
    //    return self.PreState();
    //}, this);

    //self.GetData = function() {
    //    if (self.FirstName().length == 0 || self.LastName().length == 0 || self.Address().length == 0
    //       || self.City().length == 0) {
    //        self.Get();
    //    }
    //};


};




