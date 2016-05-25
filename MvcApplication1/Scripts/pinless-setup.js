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

    var model = new Pinlessmodel();
    
    $("#btnSubmit").click({ handler: model.AddNumber });
    $("#close-val").click({ handler: model.disablepopupval });
    $("#YesDelete").click({ handler: model.Delete });
    $("#NoDelete").click({ handler: model.NoDelete });
    ko.applyBindings(model, document.getElementById("pinless-edit-bind"));
    model.GetNumbers();
    model.GetCountryListFrom();
    
});

function Pinlessmodel() {

    var self = this;

    self.PlanName = ko.observable("");
    self.PinlessNumberList = ko.observableArray([]);
    self.OrderId = ko.observable("");
    self.collect = ko.observableArray([]);
    var plan_id = $('#Planid').val();
    self.PlanId = ko.observable(plan_id);
    self.OrderId = $('#getorderid').val();
    self.CountryListFrom = ko.observableArray([]);
    self.Country = ko.observable("");
    self.Number1 = ko.observable("");
    self.Number2 = ko.observable("");
    self.Number3 = ko.observable("");
    self.StatusMessage = ko.observable("");
    self.ReqiredFields = ko.observableArray([]);
    self.CountryCode = ko.observable();
    self.PinlessNumber = ko.observable();
   

    self.AniNumber = ko.computed(function () {
        return self.Number1() + self.Number2() + self.Number3();
    }, this);

    self.validateform = function () {
        
        if (self.AniNumber().length < 10) {
            return false;
        }
        return true;
    };
    self.AddNumber = function () {
      
        if (self.AniNumber() < 10) {
            
            self.StatusMessage("Invalid 'Calling From' Number.");
            return false;
        }
        if (self.validateform() == false) {
            self.StatusMessage("");
            return false;
        }
        if (self.PlanId() == 143 || self.PlanId() == 144 || self.PlanId() == 149 || self.PlanId() == 150 || self.PlanId() == 151 || self.PlanId() == 152 || self.PlanId() == 153 || self.PlanId() == 154 || self.PlanId() == 155 || self.PlanId() == 156 ||
            self.PlanId()==157 || self.PlanId()==158||self.PlanId() ==159 ||self.PlanId()==160|| self.PlanId()==170 ||self.PlanId() ==171 || self.PlanId()==175|| self.PlanId()==176 || self.PlanId() ==177 ||self.PlanId() ==178 || self.PlanId() ==179 || self.PlanId()==180) {
            self.StatusMessage("You can add only one pinless number under this plan.");
            return false;
        }
        if (self.PinlessNumberList().length == 5) {
            self.StatusMessage("You can add only five pinless numbers.");
            return false;
        }
      
        $.ajax({
            url: '/Account/PinLessSetupEdit',
            data: {
                PinNumber: self.AniNumber,
                OrderID: self.OrderId,
                CoutryCode: self.Country
            },
            type: "POST",
            success: function (resp) {
                self.StatusMessage(resp.message);
                self.GetNumbers();
                self.Number1("");
                self.Number2("");
                self.Number3("");
                self.Country("");

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Statusmsg(JSON.parse(jqXHR.responseText).Message);
            }
        });

    };
    self.loadpopupval = function (item) {
        self.PinlessNumber(item.PinlessNumber);
        self.CountryCode(item.CountryCode);
      // result = item.Sno;
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
    self.NoDelete = function () {

        disablePopup();
    };

    self.DeleteNumber = function () {
        $.ajax({
            url: '/Account/DeletePinLessSetup',
            data: {
               pn: self.PinlessNumber,
               cd: self.CountryCode,
               oid: self.OrderId
            },
            type: "POST",
            cache: false,
            success: function (resp) {

                if (resp.status == true) {
                    self.GetNumbers();
                    self.StatusMessage(resp.message);
                } else {
                    self.GetNumbers();
                    self.StatusMessage(resp.message);
                }
            }

        });
    };
    
    self.GetNumbers = function () {
        $.ajax({
            url: '/Account/GetPinlessResult',
            data: {
                order_id: self.OrderId
            },
            type: "GET",
            cache: false,
            success: function (data) {
                self.PinlessNumberList(data.PinlessNumberList);
                
                //self.OrderId = data.OrderID;
            }

        });
    };

    self.GetCountryListFrom = function () {
        var url = '/Account/GetThreeCountryFromList';
        $.getJSON(url, function (data) {
            self.CountryListFrom(data);
        });
    };

};


        function pinlessvalidate() {
            var x = ('#txtAreaCodeFrom1').val();
            var y = ('#txtPhoneFrom1').val();
            var z = ('#txtPhoneFrom2').val();
            
            if (x.length < 3 || y.length < 3 || z.length < 4) {
                
            }
        }

          