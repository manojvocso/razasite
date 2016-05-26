$(document).ready(function() {
    
    var topupmodel = new TopupMobileModel();

    $("#Submit-login").click({ handler: topupmodel.LoginUser });
    
    ko.applyBindings(topupmodel, document.getElementById("wizard"));

    

});



function TopupMobileModel() {

    self.LoginEmail = ko.observable("");
    self.LoginPassword = ko.observable("");


    self.LoginUser = function () {
        alert("hii");
        $.ajax({
            url: "TopUpMobile/Topuplogin",
            data: {
                emailAddress: self.LoginEmail,
                password: self.LoginPassword
            },
            type: "POST",
            success: function(resp) {
                if (resp.Islogin) {
                    alert("login success");
                    return true;
                } else {
                    alert("login failed");
                    return false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("error");
                return false;
            }
        });

    };


}