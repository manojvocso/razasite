/*$(document).ready(function () {

    var masterViewModel = new MasterViewModel();

    ko.applyBindings(masterViewModel, document.getElementById("SearchthroughKnock"));
    
//    ko.applyBindings(masterViewModel);

    // Always fill From list
    masterViewModel.GetCountryListFrom();
    
});

function MasterViewModel() {
    var self = this;
    self.states = ko.observableArray([]);
    self.areas = ko.observableArray([]);
    self.availableNumbers = ko.observableArray([]);
    self.CountryListFrom = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);

    self.Countryto = ko.observable("");
    self.Countryfrom = ko.observable("");
    self.Phonenumber=ko.observable("");
    self.Emailid = ko.observable("");
    self.Password = ko.observable("");
    
    self.Update = function () {
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
            },
            type: "POST",
            success: function (resp) {
                self.ErrorMsg(resp.message);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
        });

    };

    self.GetCountryListFrom = function() {
        var url = '/Account/GetCountryToCountryFromList';
        $.getJSON(url, function(data) {
            self.CountryListFrom(data);
        });
    };

    // if any change happen on countrylistfrom
    self.CountryListFrom.subscribe(function () {
        self.CountryListTo(undefined);
        self.FetchCountryListTo();
    });
    
    self.FetchCountryListTo = function() {
        $.getJSON('/Account/GetCountryToList/', null, function (data) {
            self.CountryListTo(data);
        });
    };
}*/

function signup_validate() {
 
    $("#errorexpand-validation").css("display", "block");
    $("#QuickSignupError").css("display", "none");
    var countryfrom = $("#stamp_flag").val();
    var countryto = $("#countryto").val();
  
    var emailid = $("#Emailsignup").val();
    var newpassword = $("#NewPassword").val();
   
    var confirmpwd = $("#ConfirmPassword").val();
    var phonenumber = $("#Phone_Number-signup").val();
    if (countryfrom == "" || countryto == "" || emailid == "" || emailid == "Email address" || phonenumber == "Phone Number" || newpassword == "Password" || confirmpwd =="New Password" || emailid == null || newpassword == "" || newpassword == null || phonenumber.length < 10) {
     
      if (countryfrom == "" || countryfrom == null ) {
       
          document.getElementById("form-errors").innerHTML = "*Country from is required.";
          return false;
      } else {
           document.getElementById("form-errors").innerHTML = "";
      }
      if (countryto == "" || countryto == null) {
          document.getElementById("form-errors1").innerHTML = "* 'Calling to' country is required.";
          return false;
        } else {
          document.getElementById("form-errors1").innerHTML = "";
      }
      if (emailid == "" || emailid == null || emailid == "Email address") {
          document.getElementById("form-errors2").innerHTML = "*Email id is a required field.";
          return false;
      } 
     /* else if (emailid != "") {
          alert(emailid);
      var at = x.indexOf("@");
      var dot = x.lastIndexOf(".");
      if (at < 1 || dot < at + 2 || dot + 2 >= x.length) {
          alert(emailid);
          document.getElementById("form-errors2").innerHTML = "*Please enter a valid email address.";
          return false;
      } else {
         document.getElementById("form-errors2").innerHTML = "";
     }
      return false;
      }*/
      else if (emailid != "" || emailid != null) {

          var x = emailid;
          var atpos = x.indexOf("@");
          var dotpos = x.lastIndexOf(".");
          if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
              document.getElementById("form-errors2").innerHTML = "*Please enter a valid email address.";
              document.getElementById("NewPassword").value = '';
              document.getElementById("ConfirmPassword").value = '';
              $("#Emailsignup").focus();
              document.getElementById("Emailsignup").value = '';
              return false;
          } else {
              document.getElementById("form-errors2").innerHTML = "";
          }
      }
      else {
          document.getElementById("form-errors2").innerHTML = "";
      }
      if (phonenumber == "" || phonenumber == null || phonenumber == "Phone Number") {
          document.getElementById("form-errors3").innerHTML = "*Phone number is a required field.";
          return false;
      }
      else if (phonenumber != "" && phonenumber.length < 10) {
          document.getElementById("form-errors3").innerHTML = "*Please enter a 10 digit phone number.";
          return false;
      } else {
          document.getElementById("form-errors3").innerHTML = "";
      }  
      if (newpassword == "" || newpassword == null || newpassword =="Password" ) {
          document.getElementById("form-errors4").innerHTML = "*Password is a required field.";
          return false;
        }
     else if ( newpassword != "" && newpassword != null && newpassword.length < 6 ) {
         document.getElementById("form-errors4").innerHTML = "*Password must be at least 6 digits.";
         return false;
      } else {
          document.getElementById("form-errors4").innerHTML = "";
      }
        document.getElementById("NewPassword").value='';
        document.getElementById("ConfirmPassword").value='';
        return false;
    }
   
  
    if (phonenumber != "" && phonenumber.length < 10) {
       document.getElementById("form-errors3").innerHTML = "*Please enter a 10 digit phone number."; 
    } else {
        document.getElementById("form-errors3").innerHTML = ""; 
    }
    if (newpassword != "" && newpassword != null ) {
        if (newpassword.length < 6) {
            document.getElementById("form-errors4").innerHTML = "*Password must be at least 6 digits.";
            document.getElementById("NewPassword").value='';
            document.getElementById("ConfirmPassword").value='';
            $("#NewPassword").focus();
            return false;
        }
       else if (newpassword != confirmpwd) {
            document.getElementById("form-errors4").innerHTML = "*Password and confirm password do not match.";
            document.getElementById("NewPassword").value='';
            document.getElementById("ConfirmPassword").value='';
            $("#NewPassword").focus();
             return false;
         } else {
             document.getElementById("form-errors4").innerHTML = "";
         }
    }
    $("#errorexpand-validation").css("display", "none");
     return true;
}

function loginvalidate() {
    
    document.getElementById("login-servererror").innerHTML = "";
    var login_email = $("#login-EmailAddress").val();
    var login_password = $("#login-Password").val();
    if (login_email.length == 0 || login_password == 0) {
         document.getElementById("login-errors1").innerHTML = "Please enter a email and password.";
         return false;
    }
    if (login_email.length > 0) {
        document.getElementById("login-errors1").innerHTML = "";
        var x = login_email;
      var atpos = x.indexOf("@");
      var dotpos = x.lastIndexOf(".");
      if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
         document.getElementById("login-errors1").innerHTML = "Please enter a valid email and password.";
          $("#login-Password").val='';
          $("#login-EmailAddress").focus();
          document.getElementById("login-EmailAddress").value='';
         return false;
      } 
     }
    if (login_password.length < 6) {
       document.getElementById("login-errors1").innerHTML = "";
       document.getElementById("login-errors1").innerHTML = "Please enter a valid email and password.";
        $("#login-Password").focus(); 
        document.getElementById("login-Password").value = '';
        
        
        return false;
    }
    return true;
}
  