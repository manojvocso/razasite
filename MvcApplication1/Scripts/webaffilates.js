function loading() {
    $("div.loader").show();
}
function loadPopup1() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#errorValidation1").fadeIn(0500); // fadein popup div
        $("#backgroundPopup1").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup1").fadeIn(0001);
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

function disablePopup1() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#errorValidation1").fadeOut("normal");
        $("#backgroundPopup1").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}



$(document).ready(function () {
   
    var retailagent = new RetailAgent2();
    $("#Webaffiliates-signup").click({ handler: retailagent.WebSignUp });

    $("#close-val2").click({ handler: retailagent.disablepopupval });
    ko.applyBindings(retailagent, document.getElementById("Webaffiliatesbind"));
   

});

function RetailAgent2() {
    var self = this;
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.Email = ko.observable("");
    self.Status = ko.observable("");
    self.validationmessage = ko.observableArray([]);


    self.disablepopupval = function () {
        disablePopup();
    };
    self.ValidationInfo = function () {
        self.validationmessage([]);
  
        if (self.FirstName() == "" || self.FirstName() == undefined || self.LastName() == "" || self.LastName == undefined || self.PhoneNumber == undefined || self.PhoneNumber().length < 10 || self.Email() == "" || self.Email() == undefined) {

            if (self.FirstName() == "" || self.FirstName() ==undefined) {
                self.validationmessage.push("First name is required.");
            }
            if (self.LastName() == "" || self.LastName== undefined) {
                self.validationmessage.push("Last name is required.");
            }

            if (self.PhoneNumber().length < 10 || self.PhoneNumber == undefined) {
                self.validationmessage.push("Phone number is required.");
            }
            if (self.Email() == "" ||self.Email() == undefined) {
                self.validationmessage.push("Email is required.");
            }
        

            return false;
        }
        if (self.Email() != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(self.Email())) {
                self.validationmessage.push("Invalid Email.");
                return false;
            }
        }

        return true;

    }
    self.disablepopupval = function () {
        disablePopup1();
    };

    self.WebSignUp = function () {

        if (!self.ValidationInfo()) {

            loadPopup1();
            return false;
        }

        $.ajax({
            url: '/Account/Agent_SignUp',
            type: 'POST',
            data: {
                Firstname: self.FirstName,
                Lastname: self.LastName,
                Email: self.Email,
                Phonenumber: self.PhoneNumber,
                CallApiFor: 'W'
            },
            success: function (resp) {

                self.Status(resp.message);
                alert(self.Status());
            }
        });

    }
}
function numberOnly() {
    // 48 = 0
    // 57 = 9
    if ((event.keyCode < 48) || (event.keyCode > 57)) {
        return false;
    }
    return true;
}