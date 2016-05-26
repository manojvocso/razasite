$(document).ready(function () {
    var model = new FeaturesReward();

 
    $("#Reward-btn1").click({ handler: model.Updatereward });
    ko.applyBindings(model, document.getElementById("RazaRewardSignUp1"));
  // for raza rewards


});

function FeaturesReward() {

    var self = this;
    var email = $("#rewards-email").val();
    self.Email = ko.observable(email);
    self.Password = ko.observable("");
    self.month = ko.observable("");
    self.date = ko.observable("");
    self.year = ko.observable("");
    self.error = ko.observable("");
  
    self.Updatereward = function () {

        if (self.Email() == "" || self.Password() == "") {
          
                self.error("Please fill required fields.");
                return false;
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

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Email("");
                self.Password("");
                alert(errorThrown);
                //                self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);
            }
        });


    };
}







