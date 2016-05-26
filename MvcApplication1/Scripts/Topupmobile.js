$(document).ready(function () {

    var topupmobile = new TopupMobile();

    $("#topup-newsignup").click({ handler: topupmobile.NewSignup });

    $("#topuplogin").click({ handler: topupmobile.Topuplogin });

    $("#status li:first").addClass("active");
    
    ko.applyBindings(topupmobile, document.getElementById("topup-bind"));
    topupmobile.Get();

});

function TopupMobile() {

    var self = this;
    var mob = $("#hidden-mob-number").val();
    self.MobileNumber = ko.observable(mob);
    self.MobileOperator = $("#operator-hidden").val();
    self.TopupAmount = $("#topup-amount-hidden").val();
    self.LoginEmail = ko.observable("");
    self.LoginPassword = ko.observable("");
    self.NewEmail = ko.observable("");
    self.NewPassword = ko.observable("");
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Phonenumber = ko.observable("");
    self.ZipCode = ko.observable("");
    self.Address = ko.observable("");
    self.City = ko.observable("");
    self.State = ko.observable("");
    self.Country = ko.observable("");
    self.CardType = ko.observable("");
    self.CardNumber = ko.observable("");
    self.ExpMonth = ko.observable("");
    self.ExpYear = ko.observable("");
    self.ExpDate = ko.observable("");
    self.CvvNumber = ko.observable("");
    self.CoupanCode = ko.observable("");
    self.Email = ko.observable("");
    self.validationmessages = ko.observableArray([]);


    self.RechargeMobile = function() {

    };

    self.NewSignup = function () {
        alert(self.MobileNumber());
        alert("click");
        $.ajax({
            url: '/Account/Topupsignup',
            data: {

                Email: self.NewEmail,
                Password: self.NewPassword,
                Phone_Number: self.Phonenumber
            },
            type: "POST",
            success: function (resp) {
                alert("success");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
        });

    };

    self.Topuplogin = function() {
        $.ajax({
            url: '/Account/Topuplogin',
            data: {

                EmailAddress: self.LoginEmail,
                Password: self.LoginPassword
            },
            type: "POST",
            success: function (resp) {
                if (resp.Islogin == true) {
                    alert("LOGIN");
                    self.Get();    
                } else {
                    alert("login again");
                    stopslide();

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

                var root = $("#wizard").scrollable();
                var api = root.scrollable();
                api.prev();
            }
        });

    };

    self.Get = function () {
        $.ajax({
            //url: window.location + 'GetBillingInfo',
            url: "/Account/GetBillingInfo",
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

            }

        });
    };


};


    $(document).ready(function () {

        $("#status li:first").addClass("active");

    });

    $(function () {
            var root = $("#wizard").scrollable();
        // some variables that we need
            var api = root.scrollable(), drawer = $("#drawer");
        // validation logic is done inside the onBeforeSeek callback
            api.onBeforeSeek(function (event, i) {
            // we are going 1 step backwards so no need for validation
            if (api.getIndex() < i) {
            // 1. get current page
                var page = root.find(".form_field").eq(api.getIndex()),

                // 2. .. and all required fields inside the page
			 inputs = page.find(".required :input").removeClass("error"),

                // 3. .. which are empty
			 empty = inputs.filter(function () {
			     return $(this).val().replace(/\s*/g, '') == '';
			 });
                // if there are empty fields, then
                if (empty.length) {
                // slide down the drawer
                drawer.slideDown(function () {
                // colored flash effect
                drawer.css("backgroundColor", "#229");
                setTimeout(function () { drawer.css("backgroundColor", "#fff"); }, 1000);
                });
                    // add a CSS class name "error" for empty & required fields
                    empty.addClass("error");
                    // cancel seeking of the scrollable by returning false
                    return false;
                    // everything is good
                    } else {
                    // hide the drawer
                    drawer.slideUp();
                    }

            }
            $("#status li").removeClass("active").eq(i).addClass("active");
            });

            // if tab is pressed on the next button seek to next page

        root.find("button.next").keydown(function (e) {

            if (e.keyCode == 9) {
            api.next();
            e.preventDefault();
            }
            
        });

});
 



