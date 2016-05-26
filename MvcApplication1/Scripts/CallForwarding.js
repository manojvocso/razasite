
function loading() {
    $("div.loader").show();
}
function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#Deleteconfirmation").fadeIn(0500); // fadein popup div
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
        $("#Deleteconfirmation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}




$(document).ready(function () {
    var callforwardingViewModel = new CallForwardingViewModel();
    $("#close-val").click({ handler: callforwardingViewModel.disablepopupval });
    $("#close-val1").click({ handler: callforwardingViewModel.disablepopupval1 });
    $("#YesDelete").click({ handler: callforwardingViewModel.Delete });
    $("#NoDelete").click({ handler: callforwardingViewModel.NoDelete });
    $("#CallFrowrdbtn").click({ handler: callforwardingViewModel.AddForwardNum }); 
    $("#backgroundValidation").click({ handler: callforwardingViewModel.DisablePopVal });

    ko.applyBindings(callforwardingViewModel, document.getElementById("Call-Forward"));

    callforwardingViewModel.Getdata();

});

function CallForwardingViewModel() {
    var self = this;
    self.OrderId = $("#order_id").val();
    self.CountryTo = ko.observableArray([]);
    self.Number = ko.observableArray([]);
    self.CountryCode = ko.observable();
    self.ActivationDate = ko.observable();
    self.Expiry_Date = ko.observable();
    self.PhoneNumber = ko.observable();
    self.Activation_Date = ko.observable();
    self.DestinationNum = ko.observable();
    self.CountryCode = ko.observable();
    self.number_800 = ko.observable();
    self.Forwarding_Name = ko.observable();
    self.CallForwardList = ko.observableArray([]);
    self.Status = ko.observable();
    self.Sno = ko.observable();
    self.Validationarray = ko.observableArray([]);
    /*self.DestinationNumber = ko.computed(function() {
        return self.CountryCode() + "" + self.DestinationNum();
    }, this);
  */

    self.Getdata = function () {

        $.ajax({
            url: '/Account/GetCallForward',
            type: 'GET',
            data: {
                order_id: self.OrderId
            },
            success: function (resp) {
                self.Number(resp.ListNumber_800);
                self.CountryTo(resp.ToCountryList);
                self.CallForwardList(resp.ForwardNumberList);
            }

        });

    };

    self.loadpopupval1 = function () {
                loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup1(); // function show popup
        }, 500);
    };
    self.disablepopupval1 = function () {
        disablePopup1();
    };
    self.ValidateCallForward = function() {
        self.Validationarray([]);
        self.Expiry_Date($("#Expiry_Date").val());
        self.Activation_Date($("#Activation_Date").val());
     
        if (self.CountryCode() == undefined || self.DestinationNum() ==undefined || self.number_800() == undefined ||
           self.Activation_Date().length == 0 || self.Expiry_Date().length == 0 ) {
            if (self.CountryCode() == undefined) {
                self.Validationarray.push("Select call forwarding to country ");
            }
            if (self.DestinationNum() == undefined) {
                self.Validationarray.push("Please insert destination number.");
            }
         
            if (self.number_800() == undefined) {
                self.Validationarray.push("Please select your 1-800 number.");
            }
            if (self.Activation_Date().length == 0) {
                self.Validationarray.push("Please enter valid activation date.");
            }
            if (self.Expiry_Date().length == 0) {
                self.Validationarray.push("Please enter valid expiry date");
            }
            //if (self.Forwarding_Name() == undefined) {
            //    self.Validationarray.push("Please insert your name.");
            //}

            return false;
        }
        //} else if (self.DestinationNum().length < 10) {
        //    self.Validationarray.push("Please enter 10 digit destination number.");
        //    return false;
        //}
        return true;
    };

    self.AddForwardNum = function() {
        if (!self.ValidateCallForward()) {
            self.loadpopupval1();
            self.Forwarding_Name("");
            self.CountryCode(null);
            self.DestinationNum("");
            $("#Expiry_Date").val("");
            $("#Activation_Date").val("");
            self.number_800(null);
            return false;
        }
        
        $.ajax({
            url: '/Account/AddCallForward',
            type: 'POST',
            data: {
                CountryCode: self.CountryCode,
                Destination_Number: self.DestinationNum,
                Number_800: self.number_800,
                order_id: self.OrderId,
                Forwarding_Name: self.Forwarding_Name,
                Activation_Date: self.Activation_Date,
                Expiry_Date: self.Expiry_Date

            },
            success: function(resp) {
                self.Status(resp.status);
                self.Getdata();
                self.Forwarding_Name("");
                self.CountryCode(null);
                self.DestinationNum("");
                $("#Expiry_Date").val("");
                $("#Activation_Date").val("");
                self.number_800(null);

            }

        });

    };

        self.loadpopupval = function (item) {
            
            // result = item.Sno;
            self.Sno(item.Sno);
            
       loading(); // loading
            setTimeout(function () { // then show popup, deley in .5 second
                loadPopup(); // function show popup
            }, 500);
        };

        self.disablepopupval = function () {
            disablePopup();
        };

        self.Delete = function () {
          
            self.DeleteNumber();
            disablePopup();
        };
    self.NoDelete = function() {
     
        disablePopup();
    };

    self.DisablePopVal = function() {
        disablePopup1();
    };
        self.DeleteNumber = function () {
         
            
            $.ajax({
                url: '/Account/DeleteCallForward',
                type: 'POST',
                data: {
                    SNumber: self.Sno,
                    order_id: self.OrderId,
                },
                success: function (resp) {

                    self.Status(resp.Message);
                    self.Getdata();
                }
            });

        };

    }

    function numberOnly() {
        // 48 = 0
        // 57 = 9
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            return false;
        }
        return true;
    }

    function myFunction() {
     $("#LoginPwd").text("Please insert Valid 10 Digit Number.").hide();

 }


/*   function validate() {
       var Destinationnumber = document.getElementById('Destination_Number').value;
       var ExpiryDate = document.getElementById('Expiry_Date').value;
       var ActivationDate = document.getElementById('Expiry_Date').value;
        var Number800 = document.getElementById('Number_800').value;
       var CountryTo=document.getElementById('CountryTo').value;
       if (Destinationnumber.length < 10   || ExpiryDate.length == "" || ActivationDate.length == ""|| Number800 == "Select Your 1-800 Number" || CountryTo == "Select Country" ) {
            if (Destinationnumber.length < 10 ) {
             
               $("#LoginPwd").text("*Please insert valid ten digit number.").show();
           }
           if (ExpiryDate.length == "") {
               $("#LoginPwd1").text("*Please insert expiryDate.").show();
            }
           if (ActivationDate == "") {
                $("#LoginPwd2").text("*Please insert activation Date.").show();
           }
            if (Number800 == "Select Your 1-800 Number") {
             $("#LoginPwd3").text("*Please select the 1-800 number.").show();
          }
           if (CountryTo == "Select Country") {
               $("#LoginPwd5").text("*Please select country.").show();
           }
            return false;
        }
        return true;
  }*/


  /* function CalculateAge(birthday) {

   var re=/^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/;

    if (birthday.value != '') {

   if(re.test(birthday.value ))
    {
    birthdayDate = new Date(birthday.value);
    dateNow = new Date();

    var years = dateNow.getFullYear() - birthdayDate.getFullYear();
    var months=dateNow.getMonth()-birthdayDate.getMonth();
    var days=dateNow.getDate()-birthdayDate.getDate();
    if (isNaN(years)) {

    document.getElementById('LoginPwd4').innerHTML = '';
   document.getElementById('LoginPwd4').innerHTML = 'Input date is incorrect!';
   return false;

    }

    else {
    document.getElementById('LoginPwd4').innerHTML = '';
    }
    }
    else
    {
    document.getElementById('LoginPwd4').innerHTML = 'Date must be mm/dd/yyyy format';
    return false;
   }
 
    }
       
   }
*/

    //function CalculateExp_Age(birthday) {

    //var re=/^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/;

    //if (birthday.value != '') {

    //if(re.test(birthday.value ))
    //{
    //birthdayDate = new Date(birthday.value);
    //dateNow = new Date();

    //var years = dateNow.getFullYear() - birthdayDate.getFullYear();
    //var months=dateNow.getMonth()-birthdayDate.getMonth();
    //var days=dateNow.getDate()-birthdayDate.getDate();
    //if (isNaN(years)) {

    //document.getElementById('LoginPwd6').innerHTML = '';
    //document.getElementById('LoginPwd6').innerHTML = 'Input date is incorrect!';
    //return false;

    //}

    //else {
    //document.getElementById('LoginPwd6').innerHTML = '';

    //}
    //}
    //else
    //{
    //document.getElementById('LoginPwd6').innerHTML = 'Date must be mm/dd/yyyy format';
    //return false;
    //}
    //}
    //}
  

    function loading() {
        $("div.loader").show();
    }
   function loadPopup1() {
       if (popupStatus1 == 0) { // if value is 0, show popup
           closeloading(); // fadeout loading          
           $("#errorValidation").fadeIn(0500); // fadein popup div
           $("#backgroundValidation").css("opacity", "0.7"); // css opacity, supports IE7, IE8
           $("#backgroundValidation").fadeIn(0001);
           popupStatus1 = 1; // and set value to 1
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
   var popupStatus1 = 0; // set value

   function disablePopup1() {
       if (popupStatus1 == 1) { // if value is 1, close popup
           $("#errorValidation").fadeOut("normal");
           $("#backgroundValidation").fadeOut("normal");
           popupStatus1 = 0; // and set value to 0
       }
   }