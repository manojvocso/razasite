
function numberOnly() {
    // 48 = 0
    // 57 = 9
    if ((event.keyCode < 48) || (event.keyCode > 57)) {
        return false;
    }
    return true;
}




$(document).ready(function () {

    $('input[id$=NickName]').bind('keyup blur', function () {
        if (this.value.match(/[^a-zA-Z0-9 ]/g)) {
            this.value = this.value.replace(/[^a-zA-Z0-9 ]/g, '');
//            alert("You are entering the non alpha numeric value");
        }
    });
    $('input[id$=SpeedDialNumber]').bind('keyup blur', function () {
        if (this.value.match(/[^a-zA-Z0-9 ]/g)) {
            this.value = this.value.replace(/[^a-zA-Z0-9 ]/g, '');
//            alert("You can not enter the non alpha numeric value in the password textbox");
        }
    });
});
// Used for disable the input fields until dropdownlist is not selected

$(document).ready(function () {

    $("#ddlToCountry").change(function () {
        var fields = $("input");
        fields.removeAttr("disabled");
    });

});


                    