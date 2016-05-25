//start of pop up loading

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
        $("#errorValidation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}
//End for popup loading


var api;
var tabCounter;
$(document).ready(function () {

    $("#status li:first").addClass("active");

    var model = new RedeemNow();

    ko.applyBindings(model, document.getElementById("RedeemPointNow"));
    $("#billinginfo_edit").click({ handler: model.editbillinfo });
    $("#billinginfo_done").click({ handler: model.donebillinfo });
    $("#backgroundPopup").click({ handler: model.disablepopupval });
    $("#close-val").click({ handler: model.disablepopupval });
    $("#issueNewpinBtn").click({ handler: model.IssueNewpin });
   
    model.Get();
    model.GetSelectRate();
    tabCounter = 1;

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

                    if (tabCounter == 1) //
                    {

                        // hide the drawer
                        tabCounter++;

                    }

                

                    drawer.slideUp();

                }


            }

            // update status bar

            $("#status li").removeClass("active").eq(i).addClass("active");

        });


        // if tab is pressed on the next button seek to next page

        root.find("button.next").keydown(function (e) {


            if (e.keyCode == 9) {


                // seeks to next tab by executing our validation routine

                // api.next();

                // e.preventDefault();


            }


        });


    });
});

function RedeemNow() {
    var self = this;
    self.CallingPlan = ko.observable("");
    self.Amount = ko.observable("");
    self.ServiceFee = ko.observable("");
    self.AccountNumber = ko.observable("");
    self.OrderId = ko.observable("");
    var Point = $("#redeemPoints").val();
   
    self.RedeemPoint = ko.observable(Point);
    self.Planname = ko.observable("Global plan");
    self.CurrencyCode = ko.observable("");
    self.PlanList = ko.observableArray([]);
    self.validationmessages = ko.observableArray([]);
    self.SelectListArray = ko.observableArray([]);
    self.Rechamount = ko.observable("");
    self.MonthArray = ko.observableArray([]);
    self.ServiceCharge = ko.observable("");
    //{ name: "10", value: "10" }

    //Billing info members

    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.City = ko.observable("");
    self.State = ko.observable("");
    self.Country = ko.observable("");
    self.ZipCode = ko.observable("");
    self.Address = ko.observable("");
    self.Email = ko.observable("");

    //for finding out the total amount
   

    //redeem now variable for recharge
    self.ErrorMsg = ko.observable("");
    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, this);
    //End of billing info members
    self.Get = function () {
        $.ajax({
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

    self.GetSelectRate = function () {
     
        if (self.RedeemPoint() < 2000) {

            self.MonthArray.push(
  { name: "$10 (1000 Points)", value: "10" }
            );
        }
        else if (self.RedeemPoint() >= 2000 && self.RedeemPoint() < 3000) {
            self.MonthArray.push(
     { name: "$10 (1000 Points)", value: "10" },
     { name: "$20 (1000 Points)", value: "20" }

               );
        }
        else if (self.RedeemPoint() >= 3000 && self.RedeemPoint() < 4000) {
            self.MonthArray.push(
     { name: "$10 (1000 Points)", value: "10" },
     { name: "$20 (1000 Points)", value: "20" },
      { name: "$30 (1000 Points)", value: "30" }

               );
        }
        else if (self.RedeemPoint() >= 4000 && self.RedeemPoint() < 5000) {
            self.MonthArray.push(
     { name: "$10 (1000 Points)", value: "10" },
     { name: "$20 (1000 Points)", value: "20" },
      { name: "$30 (1000 Points)", value: "30" }

               );
        }
        else if (self.RedeemPoint() >= 5000) {
            self.MonthArray.push(
     { name: "$10 (1000 Points)", value: "10" },
     { name: "$20 (2000 Points)", value: "20" },
      { name: "$30 (3000 Points)", value: "30" },
       { name: "$50 (5000 Points)", value: "50" }

               );
        }
    }

    self.editbillinfo = function () {
        $('#billinginfo_edit').css('display', 'none');
        $('#billinginfo_done').css('display', '');
        $('#fullname, #address, #city, #state, #ZipCode').css('display', 'none');
        $('#firstnameedit')
        //.val($('#fullname').text())
            .css('display', '')
            .focus();
        $('#lastnameedit,#addressedit, #cityedit, #stateedit, #zipcodeedit')
        //.val($('#fullname').text())
            .css('display', '');
    };

    self.donebillinfo = function () {
        if (!self.ValidateBillingInfo()) {
            loadPopup();
            return false;
        }

        $('#billinginfo_edit').css('display', '');
        $('#billinginfo_done').css('display', 'none');
        $('#firstnameedit,#lastnameedit, #addressedit, #cityedit, #stateedit, #zipcodeedit').css('display', 'none');
        $('#fullname').text(self.FirstName() + "" + self.LastName()).css('display', '');
        /* $('#firstname')
             .text($('#firstnameedit').val())
             .css('display', '');
         $('#lastname')
             .text($('#lastnameedit').val())
             .css('display', '');*/
        $('#address')
            .text($('#addressedit').val())
            .css('display', '');
        $('#city')
            .text($('#cityedit').val())
            .css('display', '');
        $('#state')
            .text($('.optionselect').val())
            .css('display', '');
        $('#ZipCode')
            .text($('#zipcodeedit').val())
            .css('display', '');
    };
    self.disablepopupval = function () {
        disablePopup();
    };
    self.ValidateBillingInfo = function () {
        self.validationmessages([]);


        if (self.State() == "" || self.ZipCode() == "" || self.Address() == "" || self.City() == "" || self.FirstName() == "" || self.LastName() == "") {

            if (self.ZipCode().length == 0) {
                //self.validationmessages.push("Zipcode is required.");
            }
            if (self.Address() == "") {
                self.validationmessages.push("Address is required.");
            }
            if (self.City() == "") {
                self.validationmessages.push("City is required.");
            }
            if (self.State() == "") {
                self.validationmessages.push("State is required.");
            }
            if (self.FirstName() == "") {
                self.validationmessages.push("Firstname is required.");
            }
            if (self.LastName() == "") {
                self.validationmessages.push("Lastname is required.");
            }
            return false;
        }
        return true;
    };

    self.IssueNewpin = function () {
        alert("hi");
        var result = false;
        $.ajax({
            url: '/Cart/GlobalPlanIssue',
            data: {
                UserName: self.FullName,


                Country: self.Country,
                City: self.City,
                ZipCode: self.ZipCode,
                State: self.State,
                Address: self.Address,

                order_id: self.OrderId,

                Amount: self.Rechamount,
                PlanName: self.Planname,
                ServiceFee: self.ServiceCharge,
                Pin: self.AccountNumber,
                FirstName: self.FirstName,
                LastName: self.LastName,
                TransactionType: "Recharge",

            },
            async: false,
            type: "POST",
            success: function (resp) {
                if (resp.status) {

                    result = true;
                    //self.NewOrderId(resp.Orderid);
                    //self.ErrorMsg("“Thank you for purchasing !”");

                } else {
                    self.ErrorMsg("Sorry!! Transaction is not successfull.");
                

                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(jqXHR.Message);
            }
        });
    };
}