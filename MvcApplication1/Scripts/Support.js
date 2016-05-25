function validatemail(emailField) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (reg.test(emailField.value) == false) {
        $(".form-errors").text("Invalid Email Address").show();
        $(".form-errors").delay(1000).fadeOut();
        return false;
    }

    return true;

}

function numberOnly() {
    // 48 = 0
    // 57 = 9
    if ((event.keyCode < 48) || (event.keyCode > 57)) {
        return false;
    }
    return true;
}

function characterOnly() {
    
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[a-zA-Z]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
    }
}
function validateEmail(emailField) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (reg.test(emailField.value) == false) {
        $(".form-err").text("Invalid Email Address").show();
        $(".form-err").delay(1000).fadeOut();

        return false;
    }

    return true;

}
 function validateForm() {
     var x = $("#Email").val();
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
      //alert(x);
        $(".errormsg").text("Invalid Email Address").show();
        return false;
    }
    $(".errormsg").text("Invalid Email Address").hide();
     return true;
 }


function confirmemail() {
    var e1 = $("#Email").val();
    e1 = e1.replace(/^\s+/g, ""); // strip leading spaces
    var e2 = $("#ConfirmEmail").val();
    
    var x = $("#Email").val();
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
        //alert(x);
        $(".errormsg").text("Invalid Email Address").show();
        return true;
    }
        //$(".errormsg").text("Invalid Email Address").hide();
    else {
        e2 = e2.replace(/^\s+/g, ""); // strip leading spaces
        if (e1 != e2) {
            $(".errormsg").text("Email doesn't match.").show();
            e1 = "";
            document.myform.Confirm_Email.focus();
            return false;
        }
        if (e1 == e2) {
            $(".errormsg").hide();
        }
    }
}

// Luhn check
function CardCheck() {
    var cardnumber = $("#card").val();
    //alert(cardnumber);
    if ((cardnumber == null) || (cardnumber == "")) {
        //valid = false;
        //alert("Invalid Number!\nThere was no input.");
    } 
//    else if (cardnumber.length != 16) {
//        //valid = false;
//        alert("Invalid Number!\nThe number is the wrong length.");
//        alert(cardnumber.length);
//    }
    
    var numSum = 0;
    var value;
    for (var i = 0; i < 16; ++i) {
        if (i % 2 == 0) {
            value = 2 * cardnumber[i];
            if (value >= 10) {
                value = (Math.floor(value / 10) + value % 10);
            }
        } else {
            value = +cardnumber[i];
        }
        numSum += value;
    }
    if (numSum % 10 == 0) {
        document.getElementById("cardmsg").innerHTML = "";
        } else {
        document.getElementById("cardmsg").innerHTML = "Enter a valid card";
        
    }
}

function ValidateQuickSignup() {
    
    if (pwdCompare2() == false)
        return false;

    if (email_check() == false)
        return false;

    $("#clickonbtn").disabled = false;
    
    return true;
}

function pwdCompare2() {
    var p1 = $("#TypePassword").val();
    p1 = p1.replace(/^\s+/g, ""); // strip leading spaces
    var p2 = $("#TypePassword2").val();
    p2 = p2.replace(/^\s+/g, ""); // strip leading spaces
    if (p1 != p2) {
        $("#Form2error").text("Password do not match").show();
        $("#clickonbtn").prop("disabled", true);
        return false;
    }

    if (p1 = p2) {
        $("#Form2error").text("Password do not match").hide();
    }
    
    $("#clickonbtn").prop("disabled", false);
    
    return true;
}

function email_check() {
    var x = $("#Email").val();
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
        $("#Form2error").text("Email Address is not valid").show();
        $("#clickonbtn").prop("disabled", true);
        return false;
    } else {
        $("#Form2error").text("Email Address is not valid").hide();
    }
    $("#clickonbtn").prop("disabled", false);
    return true;
}



// validations for Quick signup (header)---
function quicksignup_header() {
   
    $("#errorexpandable").css("display", "block");
    /*document.getElementById("signup-hed-error1").innerHTML = "";
    document.getElementById("signup-hed-error2").innerHTML = "";
    document.getElementById("signup-hed-error3").innerHTML = "";
    document.getElementById("signup-hed-error4").innerHTML = "";
    document.getElementById("signup-hed-error5").innerHTML = "";*/
    
    var countryfrom = $("#countries_signup_popup").val();
    var e = document.getElementById("standard");
    var countryto = e.options[e.selectedIndex].value;
   
    var emailid = $("#Email").val();

    var newpassword = $("#TypePassword").val();
    var confirmpwd = $("#TypePassword2").val();
    var phonenumber = $("#Phone_Number").val();
    
    if ( countryto == "" || emailid == ""  || emailid =="Email address" || newpassword == "" || phonenumber == "" || phonenumber == "Phone Number") {
 
        if (countryto == "") {
            document.getElementById("signup-hed-error2").innerHTML = "* 'Calling to' country is required.";
        } else {
  
            document.getElementById("signup-hed-error2").innerHTML = "";
        }
        if (emailid == "" || emailid == "Email address") {
            document.getElementById("signup-hed-error3").innerHTML = "*Email id is a required field.";
        }
        else if (emailid.length > 0) {
            var x = emailid;
            var at = x.indexOf("@");
            var dot = x.lastIndexOf(".");
            if (at < 1 || dot < at + 2 || dot + 2 >= x.length) {
                document.getElementById("signup-hed-error3").innerHTML = "*Please enter a valid email address.";
            } else {
                document.getElementById("signup-hed-error3").innerHTML = "";
            }

        } else {
            document.getElementById("signup-hed-error3").innerHTML = "";
        }
        if (phonenumber == "" || phonenumber == "Phone Number") {
            document.getElementById("signup-hed-error4").innerHTML = "*Phone number is a required field.";
        }
        else if (phonenumber.length < 10 ) {
            document.getElementById("signup-hed-error4").innerHTML = "Please enter a ten digit phone number.";
        }
        else {
            document.getElementById("signup-hed-error4").innerHTML = "";
        }
        if (newpassword == "") {
            document.getElementById("signup-hed-error5").innerHTML = "*Password is a  required field.";
        }else if (newpassword.length < 6) {
            document.getElementById("signup-hed-error5").innerHTML = "Password must be atleast 6 digits.";
        }
        else {
            document.getElementById("signup-hed-error5").innerHTML = "";
        }
        document.getElementById("TypePassword").value = '';
        document.getElementById("TypePassword2").value = '';
        return false;
    }
    if (emailid.length > 0) {
        var x = emailid;
        var posat = x.indexOf("@");
        var posdot = x.lastIndexOf(".");
        if (posat < 1 || posdot < posat + 2 || posdot + 2 >= x.length) {
            document.getElementById("signup-hed-error3").innerHTML = "Please enter a valid email address.";
            document.getElementById("Email").value = '';
            document.getElementById("TypePassword").value = '';
            document.getElementById("TypePassword2").value = '';
            $("#Email").focus();
            return false;
        } else {
            document.getElementById("signup-hed-error3").innerHTML = "";
        }
    }
    if (phonenumber.length < 10) {
        document.getElementById("signup-hed-error4").innerHTML = "Please enter a 10 digit phone number.";
        document.getElementById("TypePassword").value = '';
        document.getElementById("TypePassword2").value = '';
        return false;
    } else {
        document.getElementById("signup-hed-error4").innerHTML = "";
    }

    if (newpassword != "" && newpassword != null) {
        if (newpassword.length < 6) {
            document.getElementById("signup-hed-error5").innerHTML = "Password must be at least 6 digits.";
            document.getElementById("TypePassword").value = '';
            document.getElementById("TypePassword2").value = '';
            $("#TypePassword").focus();
            return false;
        }
        else if (newpassword != confirmpwd) {
            document.getElementById("signup-hed-error5").innerHTML = "Password and confirm password do not match.";
            document.getElementById("TypePassword").value = '';
            document.getElementById("TypePassword2").value = '';
            $("#TypePassword").focus();
            return false;
        } else {
            document.getElementById("signup-hed-error5").innerHTML = "";
        }
    }
    $("#errorexpandable").css("display", "none");
    return true;
}
$(function () {
    $('#close').click(function () {
       // $("#errorexpandable").hide();
    });
});