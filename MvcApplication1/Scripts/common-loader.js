$(document).ready(function () {

    $(document).ajaxSend(function (event, request, settings) {
        $('#load-blk').css('display', '');



    });

    $(document).ajaxComplete(function (event, request, settings) {
        $('#load-blk').css('display', 'none');

    });

});


