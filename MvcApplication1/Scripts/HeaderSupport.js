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
        //        alert('Invalid Email Address');
        $(".form-err").text("Invalid Email Address").show();
        $(".form-err").delay(1000).fadeOut();

        return false;
    }

    return true;

}

// Luhn check
function CardCheck() {
    var cardnumber = $("#card").val();
    //alert(cardnumber);
    if ((cardnumber == null) || (cardnumber == "")) {
        //valid = false;
        
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
        //alert("valid");
        //document.getElementById("cardmsg").innerHTML = "";
    } else {
        //alert("invalid");
        document.getElementById("cardmsg").innerHTML = "Enter a valid card";
    }
}

