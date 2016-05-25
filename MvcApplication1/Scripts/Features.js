$(document).ready(function () {
    var features = new Features();

    $("#Reward-signupbtn").click({ handler: features.Update });
  
    ko.applyBindings(features, document.getElementById("RazaRewardSignUp"));
   
  // for raza rewards


});

function Features() {

    var self = this;
    self.Email = ko.observable("");
    self.Password = ko.observable("");
    self.month = ko.observable("");
    self.date = ko.observable("");
    self.year = ko.observable("");
    self.error = ko.observable("");

    self.Update = function () {
        self.Email($("#RazaRewardSignUp_Email").val());
       
        if (!userauthenticated) {

            if (self.Email() == "" || self.Password() == "") {
                self.error("Please enter required fields.");
                return false;

            }
        } else {
            if (self.Email() == "") {
                self.error("Please enter Email.");
                return false;
            }
        }

        $.ajax({
            url: '/Features/RazaRewardSignup/',
            data: {
                Email: self.Email,
                Password: self.Password,
                Month: self.month,
                Date: self.date,
                Year: self.year

            },
            type: "POST",
            success: function (resp) {
                self.Email("");
                self.Password("");
                self.error(resp.result);
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Email("");
                self.Password("");
                alert(errorThrown);
                //                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
                return false;
            }
        });


    };

}







