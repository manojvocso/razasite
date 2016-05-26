function loading() {
    $("div.loader").show();
}

$(document).ready(function () {
    var whyRaza = new WhyRaza();

    $("#WhyRazabtn").click({ handler: whyRaza.Update });
    $("#close-val1").click({ handler: whyRaza.disablepopupval1 });
    ko.applyBindings(whyRaza, document.getElementById("WhyRazaRef"));


});

function WhyRaza() {

    var self = this;
    self.Mail1 = ko.observable();
    self.Mail2 = ko.observable();
    self.Mail3 = ko.observable();
   
    self.Mail4 = ko.observable();
    self.Mail5 = ko.observable();
    self.Mail = ko.observableArray([]);
    self.Thank = ko.observable();
    self.No_Mail = ko.observableArray([]);

    self.loadpopupval1 = function () {
        loading();// loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup1(); // function show popup
        }, 500);
    };
    self.disablepopupval1 = function () {
        disablePopup1();
    };

    self.Update = function () {
        if (self.Mail1() == undefined && self.Mail2() == undefined && self.Mail3() == undefined && self.Mail4() == undefined && self.Mail5() == undefined) {
            self.Thank("Please refer atleast one friend.");
            return false;
        }
       self.No_Mail([]);
        
        self.Thank("");
        self.Mail([]);
        $.ajax({
            url: '/WhyRaza/ReferFreind',
            data: {
                mail1: self.Mail1,
                mail2: self.Mail2,
                mail3: self.Mail3,
                mail4:self.Mail4,
                mail5:self.Mail5

            },
            type: "POST",
            success: function (resp) {
                               
                if (resp.Mail[0] == "These are Already Our Customer(s):" + resp.ReferFriendName[0]) {
                  
                    self.Mail.push(resp.Mail[0]);
                }
                else if (resp.Mail[0] == "Your Mail Successfully Sent to:" + resp.ReferFriendName[0]) {
                    
                    
                    self.No_Mail.push(resp.Mail[0]);
                }

                if (resp.Mail[1] == "These are Already Our Customer(s):" + resp.ReferFriendName[1]) {
                  
                    self.Mail.push(resp.Mail[1]);
                }
                else if (resp.Mail[1] == "Your Mail Successfully Sent to:" + resp.ReferFriendName[1]) {


                    self.No_Mail.push(resp.Mail[1]);
                }

                if (resp.Mail[2] == "These are Already Our Customer(s):" + resp.ReferFriendName[2]) {
                   
                    self.Mail.push(resp.Mail[2]);
                }
                else if (resp.Mail[2] == "Your Mail Successfully Sent to:" + resp.ReferFriendName[2]) {


                    self.No_Mail.push(resp.Mail[2]);
                }

                if (resp.Mail[3] == "These are Already Our Customer(s):" + resp.ReferFriendName[3]) {
                 
                    self.Mail.push(resp.Mail[3]);
                }
                else if (resp.Mail[3] == "Your Mail Successfully Sent to:" + resp.ReferFriendName[3]) {


                    self.No_Mail.push(resp.Mail[3]);
                }

                if (resp.Mail[4] == "These are Already Our Customer(s):" + resp.ReferFriendName[4]) {
                  
                    self.Mail.push(resp.Mail[4]);
                }
                else if (resp.Mail[4] == "Your Mail Successfully Sent to:" + resp.ReferFriendName[4]) {


                    self.No_Mail.push(resp.Mail[4]);
                }
                //self.Mail(resp.Mail);
                self.Thank(resp.Msg);
                self.loadpopupval1();
               
                self.Mail1("");
                self.Mail2("");
                self.Mail3("");
                self.Mail4("");
                self.Mail5("");

            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
//                           self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
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
function closeloading() {
    $("div.loader").fadeOut('normal');
}






