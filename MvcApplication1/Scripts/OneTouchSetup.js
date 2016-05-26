var objVM = new CascadingDDLViewModel();

function loading() {
    $("div.loader").show();
}
function loadPopupdelconfirm() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#Deleteconfirmation").fadeIn(0500); // fadein popup div
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

function disablePopupdelconfirm() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#Deleteconfirmation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}



$(document).ready(function () {

    $("#btnAddDestination").click({ handler: objVM.AddOnetouchNumber });
    $("#YesDelete").click({ handler: objVM.DeleteOnetouch });
    $("#NoDelete").click({ handler: objVM.NoDelete });
    $("#close-val").click({ handler: objVM.NoDelete });
    $("#mail-setups").click({ handler: objVM.MailAllOnetouchNumber });

    ko.applyBindings(objVM, document.getElementById("inner_body_container"));

    objVM.GetOneTouchNumber();
});

function CascadingDDLViewModel() {
    var self = this;

    self.states = ko.observableArray([]);
    self.areas = ko.observableArray([]);
    self.availableNumbers = ko.observableArray([]);
    self.CountryListFrom = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);

    self.OneTouchList = ko.observableArray([]);
    self.ValidationArray = ko.observableArray([]);

    self.SelectedCountryfrom = ko.observable("");
    self.SelectedState = ko.observable("");
    self.SelectedArea = ko.observable("");
    self.SelectedAvailNumber = ko.observable("");
    self.SelectedCountryTo = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.CountryCode = ko.observable("");
    self.OnetouchName = ko.observable("");
    self.Addmessage = ko.observable("");

    self.DeleteOnetouchNumber = ko.observable("");

    self.ErrorMsg = ko.observable("");

    self.AddOnetouchNumber = function () {
        if (!self.ValidateAddNumber()) {
            self.Addmessage("Please check the information you provided.");
            return false;
        }
        if (self.OneTouchList().length == 20) {
            self.Addmessage("You can add only twenty Onetouch dialing numbers.");
            return false;
        }
        $.ajax({
            url: '/Account/OnetouchSetup',
            data: {
                ddlCountry: self.SelectedCountryfrom,
                CountryCode: self.CountryCode,
                Destination: self.PhoneNumber,
                OneTouch_Name: self.OnetouchName,
                OrderId: $("#hidden-oid").val(),
                AvailableNumber: self.SelectedAvailNumber,

            },
            type: "POST",
            success: function (resp) {
                self.GetOneTouchNumber();
                $("#ddlCountry").val("");
                $("#ddlStates").val("");
                $("#ddlAreaCode").val("");
                $("#AvailableNumber").val("");
                self.OnetouchName("");
                self.PhoneNumber("");
                self.CountryCode("");
                self.Addmessage("Your One Touch number successfully added !!");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    };

    self.NoDelete = function() {
        disablePopupdelconfirm();
    };

    self.LoadDelConfirm = function (data) {
        self.DeleteOnetouchNumber(data.OneTouch_Number);
        loadPopupdelconfirm();
    };

    self.ValidateAddNumber = function () {
        var dddlcountry = $("#ddlCountry").val();
        var ddlstate = $("#ddlStates").val();
        var ddlarecode = $("#ddlAreaCode").val();

        var availnumber = $("#AvailableNumber").val();

        var ddltocountry = $("#ddlToCountry").val();

        var destinationnumber = $("#Destination").val();

        var onetouchname = $("#OneTouch_Name").val();

        if (dddlcountry.length == 0 || ddlstate.length == 0 || ddlarecode.length == 0 || self.SelectedAvailNumber().length == 0
            || self.CountryCode().length == 0 || self.PhoneNumber().length == 0) {
            
            return false;
        }
        return true;

    };

    self.DeleteOnetouch = function () {
        $.ajax({
            url: '/Account/DeleteOneTouchSetup',
            data: {
                num: self.DeleteOnetouchNumber,
                pin: $("#pin").val()

            },
            type: "POST",
            success: function (resp) {
                if (resp) {
                    self.GetOneTouchNumber();
                    self.NoDelete();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    };

    self.GetOneTouchNumber = function () {
        $.ajax({
            url: '/Account/GetOneTouchNumber',
            data: {
                orderid: $("#hidden-oid").val(),
            },
            type: "GET",
            async: false,
            success: function (data) {
                self.OneTouchList(data);
              

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    };

    
    self.MailAllOnetouchNumber = function() {
        self.Addmessage("");
        $.ajax({
            url: '/Account/MailAllOnetouchNumber',
            data: {
                orderId: $("#hidden-oid").val(),
            },
            type: "POST",
            success: function (resp) {
                if (resp.status) {
                    self.Addmessage("Your Onetouch setup(s) email sent successfully.");
                } else {
                    self.Addmessage("Your Onetouch setup(s) do not email sent successfully.");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    };

}

$(document).ready(function () {
    var url = '/Account/GetCountryToCountryFromList';
    $.getJSON(url, function (data) {
        objVM.CountryListFrom(data);
   
    });
});


function FetchStates() {
    var countryCode = $("#ddlCountry").val();

    $.getJSON('/Account/GetStates/' + countryCode, null, function (data) {
        objVM.states(data);
       

    });
}

function FetchAreaCode() {
    var stateCode = $("#ddlStates").val();
    $.getJSON('/Account/GetAreas/' + stateCode, null, function (data) {
        objVM.areas(data);
    });
}


function FetchAvailableNumbers() {

    $.ajax({
        url: "/Account/GetAvailableNumbers/",
        type: 'GET',
        data: { pin: $("#pin").val(), countryId: $("#ddlCountry").val(), state: $("#ddlStates").val(), areaCode: $("#ddlAreaCode").val() },
        success: function (data) {
            //        
            objVM.availableNumbers(data);
        }
    });

   
}

function FetchContact(data) {
    

   // document.getElementById("CountryCode").value = data.value;
}



$(document).ready(function() {

    $('input[id$=OneTouch_Name]').bind('keyup blur', function () {
        if (this.value.match(/[^a-zA-Z0-9 ]/g)) {
            this.value = this.value.replace(/[^a-zA-Z0-9 ]/g, '');
         
        }
    });
});

// Used for disable the button until necessary fields doesnot fill up.

function checkFilled() {
    var filled = 0;
    var x = $("#Destination").val();
    x = x.replace(/^\s+/, ""); // strip leading spaces
    if (x.length > 0) { filled++; }

    var y = $("#OneTouch_Name").val();
    y = y.replace(/^s+/, ""); // strip leading spaces
    if (y.length > 0) {
        filled++;
    }
    
    if (filled == 2) {
     //  document.getElementById("btnAddDestination").disabled = false;
    } else {
       // document.getElementById("btnAddDestination").disabled = true;
    }

}

function onetouch_validate() {
    var dddlcountry = $("#ddlCountry").val();
    var ddlstate = $("#ddlStates").val();
    var ddlarecode = $("#ddlAreaCode").val();
    var availnumber = $("#AvailableNumber").val();
    var ddltocountry = $("#ddlToCountry").val();
    var destinationnumber = $("#Destination").val();
    var onetouchname = $("#OneTouch_Name").val();
    if (dddlcountry.length == 0 || ddlstate.length == 0 || ddlarecode.length == 0 || availnumber.length == 0 || ddltocountry.length == 0 || destinationnumber.length == 0 || onetouchname.length == 0) {
        
        return false;
    }
    return true;

}

