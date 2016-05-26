var retailagentweb = new RetailAgent3();
$(document).ready(function () {
 
    $(".webexpandableapply").hide();
    $("#Webaffliates-apply").click({ handler: retailagentweb.WebSignUp });

   
    ko.applyBindings(retailagentweb, document.getElementById("Websignup-apply"));
    retailagentweb.FetchCountryList();

});

function RetailAgent3() {
    var self = this;
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.Message = ko.observable("");
    self.Country = ko.observable("");
    self.StatusMessage = ko.observable("");
    self.validationIssue = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);


    self.FetchCountryList = function () {

        $.getJSON('/Account/GetCountryToCountryFromList/', null, function (data) {
            self.CountryListTo(data);

        });
    };
   
    self.ValidateInfo = function () {
        self.validationIssue([]);
      
        if (self.FirstName() == "" || self.LastName() == "" || self.Email() == "" || self.Country() == undefined || self.Address() == "" || self.PhoneNumber() =="") {

            if (self.FirstName() == "") {
                self.validationIssue.push("First name is required.");
            }
            if (self.LastName() == "") {
                self.validationIssue.push("Last name is required.");
            }
            if (self.Country() == undefined) {
                self.validationIssue.push("select the country.");
            }
            if (self.Address() == "") {
                self.validationIssue.push("Address is required.");
            }
            if (self.Email() == "") {
              
                self.validationIssue.push("Email is required.");
            }
            if (self.PhoneNumber() == "") {

                self.validationIssue.push("Phone number is required.");
            }
            return false;
        }
        if (self.Email() != 0) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(self.Email())) {
                self.validationIssue.push("Invalid Email.");
                return false;
            }
        }

        return true;

    }
  

    self.WebSignUp = function () {
       
        if (!self.ValidateInfo()) {
           
           
            $(".webexpandableapply").show();
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
                Address: self.Address,
                PhoneNumber: self.PhoneNumber,
                Country: self.Country,
                Message: self.Message,
                CallApiFor:'W'
            },
            success: function (resp) {

                self.StatusMessage(resp.message);

            }
        });

    }
}


