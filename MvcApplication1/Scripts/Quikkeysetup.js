function loading() {
    $("div.loader").show();
}
function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading();  // fadeout loading
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
  
    $("#rechrge-plan").click(function () {
      
        var orderid = $('#OrderId').val();
        
        window.location.href = "/Recharge?orderid=" + orderid ;

    });

    var model = new QickkeySetup();

    $("#btnAddDestination").click({ handler: model.AddNumber });
    $("#close-val").click({ handler: model.disablepopupval });
    $("#close-val1").click({ handler: model.disablepopupval1 });
    $("#YesDelete").click({ handler: model.Delete });
    $("#NoDelete").click({ handler: model.NoDelete });
    $("#mail-allquickkeys").click({ handler: model.MailAllquickkeys });
    ko.applyBindings(model, document.getElementById("quickeysetup"));
    /*model.GetNumbers();*/
    model.GetQuickData();
 
});

function QickkeySetup() {
    var self = this;
    self.CountryListTo = ko.observableArray([]);
    self.OrderId = $("#OrderId").val();
    self.PlanName = ko.observable();
    self.CurrencyCode = ko.observable();
    self.PinNumber = ko.observable();
    self.QuickeyEditList = ko.observableArray([]);
    self.CountryCode = ko.observable();
    self.Nickname = ko.observable();
    self.SplDialnumber = ko.observable();
    self.DestNumber = ko.observable();
    self.Pinnumber = ko.observable();
    self.StatusMessage = ko.observable();
    self.Destination = ko.observable();
    self.spcl = ko.observable();
    self.Validationarray = ko.observableArray([]);
    self.ExceedLength = ko.observable("");
    self.ErrorMsg = ko.observable("");
    self.GetQuickData = function () {

        $.ajax({
            url: '/Account/GetQuickData',
            type: 'GET',
            data: {
                order_id: self.OrderId
            },
            success: function (resp) {
                self.CountryListTo(resp.Tocountry);
                
                self.CurrencyCode(resp.CurrencyCode);
                
                self.PinNumber(resp.PinNumber);
                self.Pinnumber(resp.Pin);
               
                self.PlanName(resp.planname);
                self.QuickeyEditList(resp.QuickeyEdit);
                

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
    self.validateQuickKey = function () {
        self.ExceedLength("");
        self.Validationarray([]);
        if (self.CountryCode() == undefined || self.SplDialnumber()==undefined || self.DestNumber()==undefined || self.Nickname() == undefined) {
            if (self.CountryCode() == undefined) {
                self.Validationarray.push("Please select country");
            }
            if (self.SplDialnumber() == undefined) {
              
                self.Validationarray.push("Please enter Quickeys");
            }
            else if (self.SplDialnumber().length < 3) {
            
                self.Validationarray.push("Please enter valid Quickeys");
            }
                 
            if (self.DestNumber() == undefined  ) {
                self.Validationarray.push("Please enter destination number");
            }
            else if (self.DestNumber().length < 10) {
                self.Validationarray.push("Please enter 10 digit destination number");
            }
            if (self.Nickname()==undefined) {
                self.Validationarray.push("Please enter contact name");
            }
            return false;
        }
        return true;

    };
    self.AddNumber = function () {
    
        if (!self.validateQuickKey()) {
            self.loadpopupval1();
            return false;
        }
        if (self.QuickeyEditList().length == 10) {
            self.ExceedLength("You cant add more than 10 numbers.");
            self.loadpopupval1();
            self.CountryCode(null);
            self.DestNumber("");
            self.SplDialnumber("");
            self.Nickname("");
            return false;
        }
     
        $.ajax({
            url: '/Account/QuickkeysSetupEdit/',
            data: {
                Pin: self.Pinnumber,
                CountryCode: self.CountryCode,
                SpeedDialNumber: self.SplDialnumber,
                DestinationNumber: self.DestNumber,
                NickName:self.Nickname,
            },
            type: "POST",
            success: function(resp) {
                self.StatusMessage(resp.message);
                self.GetQuickData();
                self.CountryCode(null);
                self.DestNumber("");
                self.SplDialnumber("");
                self.Nickname("");

            },
        });
    };

    self.MailAllquickkeys = function () {
        $.ajax({
            url: '/Account/MailQuickKeys',
            data: {
                orderid: $("#OrderId").val()
            },
            type: "POST",
            success: function (resp) {
                if (resp.status) {
                    self.ErrorMsg("Your Quickey(s) email sent successfully.");
                } else {
                    self.ErrorMsg("Your Quickey(s) do not email sent successfully.");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    };


  
    self.loadpopupval = function (item) {
      
        self.Destination(item.Destination);
        self.spcl = (item.SpeedDialNumber);
      
        // result = item.Sno;
        loading();// loading
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
    self.NoDelete = function () {

        disablePopup();
    };

    self.DeleteNumber = function () {
        $.ajax({
            url: '/Account/DeleteQuickeys',
            data: {
                destNum: self.Destination,
                spdial: self.spcl,
                pin:self.Pinnumber
            },
            type: "POST",
            cache: false,
            success: function (resp) {
                self.StatusMessage(resp.status); 
                    self.GetQuickData();
                },
          

        });
    };

}

function loadPopup1() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading();// fadeout loading
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