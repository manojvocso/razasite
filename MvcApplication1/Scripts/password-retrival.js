$(document).ready(function () {

    var forgotPasswordViewModel = new ForgotPasswordViewModel();

    $("#password_retrieve").click({ handler: forgotPasswordViewModel.ForgotPassword });
    $("#close-new").click({ handler: forgotPasswordViewModel.statusclear });
    $("#mask-new").click({ handler: forgotPasswordViewModel.statusclear });

    ko.applyBindings(forgotPasswordViewModel, document.getElementById("paswword-ret"));



    //    $('#contact-new')
    //        .fadeIn('beforeShow', function () {
    //            alert("before show");
    //            forgotPasswordViewModel.status("");
    //        });

});

function ForgotPasswordViewModel() {

    var self = this;

    self.email = ko.observable("");
    self.ErrorMessage = ko.observable("");
    self.status = ko.observable("");

    self.ForgotPassword = function () {
        //self.status("");

        if (self.retrivalemailchk() == false) {
            self.email("");
            return;
        }
   
        $.ajax({
            url: '/Account/Forgotpassword',
            data: { email: self.email },
            type: "POST",
            success: function (resp) {
              
                self.status(resp.status);
                $("#emailtxt").val("");
                self.email("");
                //$("#retr_email_valmsg").text("Please enter a valid email address.").hide();

            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).ValidationMessage);
            }
        });
    };

    self.retrivalemailchk = function () {
       // self.email($('#emailtxt').val());

        if (self.email().length == 0) {
            self.status("Email id is a required field.");
            return false;
        } else {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(self.email())) {
                self.status("Invalid email address.");
                return false;
            }
        }
        return true;
    };

    self.statusclear = function () {
        self.status("").extend({ throttle: 2000 });
        self.email("");
        //$("#retr_email_valmsg").text("Please enter a valid email address.").css("display", "none");
    };
}

function password_ret_emailvalidate() {
    var x = $("#pass_ret_email").val();
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
        //alert(x);
        $("#retr_email_valmsg").text("Please enter a valid email address.").show("display", "");
        $("#email_status").text("").show("display", "none");
        $("#password_retrieve").focus();
        return false;
    }
    $("#retr_email_valmsg").text("Please enter a valid email address.").css("display", "none");
    $("#password_retrieve").focus();
    return true;

}

$("body").click(function (e) {
    $("#retr_email_valmsg").text("Please enter a valid email address.").css("display", "none");
    $("#email_status").text("").show("display", "none");

});

$('#contact-new').click(function (e) {
    // alert("prop");
    e.stopPropagation();
});

function blockerror() {
    $('#contact-new').stopPropagation();
}
