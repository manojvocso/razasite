$(function () {
    $("a.ajaxAction").click(function (e) {

        e.preventDefault();  //prevent default click behaviour
        var item = $(this);
        $.post(item.attr("href"), function (data) {
            if (data.Status == "Deleted") {
                //successsfully delete from db. Lets remove the Item from UI
                item.closest("tr").remove();
            }
            else {
                alert("Could not delete");
            }

        });

    });

});


