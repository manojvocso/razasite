$(document).ready(function () {

    //    $("tr").hover({ background-color: lightyellow; });

    $("tr").click({ handler: SelectRow });

    $('#tabletest').bind('click', function (e) {

    });
});

function SelectRow() {

    var emailId = $(this).find("td:first").html();

    $.get("/Account/RazaMailer", { EmailId: emailId });


};