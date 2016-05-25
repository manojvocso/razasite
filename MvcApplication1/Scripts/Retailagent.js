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



$(document).ready(function () {
    var retailagent = new RetailAgent();
    $("#Retailagentbtn").click({ handler: retailagent.AgentSignUp });
    $("#newagentsignup").click({ handler: retailagent.NewAgentSignUp });
    $(".expandableapply").hide();
    $("#close-val1").click({ handler: retailagent.disablepopupval });
    ko.applyBindings(retailagent, document.getElementById("bind-agent"));
    retailagent.FetchCountryListTo();

});

function RetailAgent() {
    var self = this;
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.Email = ko.observable("");
    self.Address = ko.observable("");
    self.City = ko.observable("");
    self.ZipCode = ko.observable("");
    self.StateList = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);
    self.Message = ko.observable("");
    self.Country = ko.observable("");
    self.BillingcountryId = ko.observable("");
    self.State = ko.observable("");
    self.Status = ko.observable("");
    self.validationmessages = ko.observableArray([]);
    self.Messagestatus = ko.observable("");
    self.firstName = ko.observable("");
    self.lastName = ko.observable("");
    self.email = ko.observable("");
    self.phoneNumber = ko.observable("");


    self.FetchCountryListTo = function () {
        
        $.getJSON('/Account/GetCountryToCountryFromList/', null, function (data) {
            self.CountryListTo(data);

        });
    };
    self.BillingcountryId.subscribe(function () {
     
        ko.utils.arrayForEach(self.CountryListTo(), function (item) {
            if (self.BillingcountryId() == item.Id) {
                self.Country(item.Name);
            }
        });
        self.FetchState();
    });
    self.FetchState = function () {
        $.getJSON('/Account/GetStates/' + self.BillingcountryId(), null, function (data) {
            self.StateList(data);

        });

    }
    self.disablepopupval = function () {
        disablePopup();
    };
    self.ValidationInfo = function () {
        self.validationmessages([]);
 
        if (self.FirstName() == "" || self.LastName() == "" || self.PhoneNumber().length < 10 || self.Email() == "" || self.Country() == "" ||
            self.State() == undefined || self.City == "" || self.Address == "") {
       
            if (self.FirstName() == "") {
                self.validationmessages.push("First name is required.");
            }
            if (self.LastName() == "") {
                self.validationmessages.push("Last name is required.");
            }

            if (self.PhoneNumber().length < 10) {
                self.validationmessages.push("Phone number is required.");
            }
            if (self.Email() == "") {
                self.validationmessages.push("Email is required.");
            }
            if (self.Country() == "") {
                self.validationmessages.push("Select country.");
            }
            if (self.State() == undefined) {
                self.validationmessages.push("Select state.");
            }
            if (self.City() == "") {
                self.validationmessages.push("City is required.");
            }
            if (self.Address() == "") {
                self.validationmessages.push("Address is required.");
            }
            if (self.ZipCode() == "") {
                self.validationmessages.push("Zipcode is required.");
            }
        
            return false;
        }
        if (self.Email() != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(self.Email())) {
                self.validationmessages.push("Invalid Email.");
                return false;
            }            
        }               
      
        return true;
       
        }
    self.ValidationInfomessage = function () {
        self.validationmessages([]);

        if (self.firstName() == "" || self.lastName() == "" || self.phoneNumber().length < 10 || self.email() == ""  ) {

            if (self.firstName() == "") {
                self.validationmessages.push("First name is required.");
            }
            if (self.lastName() == "") {
                self.validationmessages.push("Last name is required.");
            }

            if (self.phoneNumber().length < 10) {
                self.validationmessages.push("Phone number is required.");
            }
            if (self.email() == "") {
                self.validationmessages.push("Email is required.");
            }
         

            return false;
        }
        if (self.email() != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(self.email())) {
                self.validationmessages.push("Invalid Email.");
                return false;
            }
        }

        return true;

    }
    self.NewAgentSignUp = function () {

        if (!self.ValidationInfomessage()) {
            
            loadPopup();
            return false;
        }

        $.ajax({
            url: '/Account/Agent_SignUp',
            type: 'POST',
            data: {
                Firstname: self.firstName,
                Lastname: self.lastName,
                Email: self.email,
                Phonenumber: self.phoneNumber,
                CallApiFor:'A'
            },
            success: function (resp) {

                self.Messagestatus(resp.message);

            }
        });

    }
    self.AgentSignUp = function () {

            if (!self.ValidationInfo()) { 
                
                $(".expandableapply").show();
                return false;
            }
            $(".expandableapply").hide();
            $.ajax({
                url: '/Account/Agent_SignUp',
                type: 'POST',
                data: {
                    Firstname: self.FirstName,
                    Lastname: self.LastName,
                    Email: self.Email,
                    Phonenumber: self.PhoneNumber,
                    address: self.Address,
                    City: self.City,
                    Zipcode: self.ZipCode,
                    Country: self.Country,
                    State: self.State,
                    Message: self.Message,
                    CallApiFor:'A'
                },
                success: function (resp) {
              
                    self.Status(resp.message);
               
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