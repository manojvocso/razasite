$(document).ready(function () {

    $("#email-quickkeys").click(function () {

        var order_id = $("#hidden-oid").val();

        $.ajax({
            url: "/Account/MailQuickKeys",
            data: {
                orderId: order_id
            },
            type: "POST",
            cache: false,
            success: function (resp) {
                if (resp.status) {
                    $("#mail-status").text("Your Quickkeys(s) sent successfully.").show();
                } else {
                    $("#mail-status").text("").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });

    });

   
});


