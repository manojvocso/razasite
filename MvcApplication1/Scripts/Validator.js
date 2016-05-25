
// Function used for enable and disable the click button

function checkFilled() {
    var filled = 0;
    
    var x = $("#txtAreaCodeFrom1").val();
    x = x.replace(/^\s+/, ""); // strip leading spaces
    if (x.length > 2) { filled++; }

    var y = $("#txtPhoneFrom1").val();
    y = y.replace(/^s+/, ""); // strip leading spaces
    if (y.length > 2) {
        filled++;
    }

    var z = $("#txtPhoneFrom2").val();
    z = z.replace(/^s+/, ""); // strip leading spaces
    if (z.length > 3) {
        filled++;
    }

    if (filled == 3) {
        document.getElementById("btnSubmit").disabled = false;
    } else {
        document.getElementById("btnSubmit").disabled = true;
    } // in case a field is filled then erased

}


    