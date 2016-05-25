(function () {
    "use strict";

    // Variables
    //
    var onl = $(".over-navbar-left"),
              onlBtnToggle = $(".onl-btn-toggle"),
              onlBtnCollapse = $(".onl-btn-collapse"),
              contentWrap = $(".content-wrap"),
              contentWrapEffect = contentWrap.data("effect");

    // Functions
    //
    function onlShow() {
        onl.addClass("onl-show").removeClass("onl-hide");
        contentWrap.addClass(contentWrapEffect);
        onlBtnToggle.find("span").removeClass("fa-bars").addClass("fa-arrow-left");
    }

    function onlHide() {
        onl.removeClass("onl-show").addClass("onl-hide");
        contentWrap.removeClass(contentWrapEffect);
        onlBtnToggle.find("span").removeClass("fa-arrow-left").addClass("fa-bars");
    }

    // Toggle the edge navbar left
    //
    onlBtnToggle.click(function () {
        if (onl.hasClass("onl-hide")) {
            onlShow();
        } else {
            onlHide();
        }
    });

    // Collapse the over navbar left subnav
    //
    onlBtnCollapse.click(function (e) {
        e.preventDefault();
        if (onl.hasClass("onl-collapsed")) {
            onl.removeClass("onl-collapsed");
            contentWrap.removeClass("onl-collapsed");
            $(this).find(".onl-link-icon").removeClass("fa-arrow-right").addClass("fa-arrow-left");
        } else {
            onl.addClass("onl-collapsed");
            contentWrap.addClass("onl-collapsed");
            $(this).find(".onl-link-icon").removeClass("fa-arrow-left").addClass("fa-arrow-right");
        }
    });

})();

$(document).ready(function () {
    //$('#ddl-global-country').dropdown();
    $('.cc-number-val').payment('formatCardNumber');
    $('.data-numeric').payment('restrictNumeric');
    $('.cc-cvc').payment('formatCardCVC');


    var userName = $("#hid-User-Name").val();
    if (userName == undefined || userName === "") {
        userName = "Raza";
    }
    $('nav#menu').mmenu({
        extensions: ['effect-slide-menu', 'pageshadow'],
        counters: true,
        navbar: {
            title: userName
        },
        onclick: {
            blockUI: true,
            close: true
        },
        navbars: [
             {
                 position: 'top',
                 content: [
                     'prev',
                     'title',
                     'close'
                 ]
             }, {
                 position: 'bottom',
                 content: [

                 ]
             }
        ]
    });

    var dropHeader = $("#ddl-global-country");
    var countryByIp = $("#countryByIp").val();
    dropHeader.dropdown(
        'set selected', countryByIp // Preselect a value for the dropdown.
    );

    dropHeader.dropdown(
    {
        onChange: function (value, text, $selectedItem) {
            debugger;
            if (value == countryByIp)
                return;

            $("#countryByIp").val(value);
            //$("#changeHeaderCountryForm").submit();
            $.ajax({
                type: "POST",
                url: $("#countryByIpSubmitUrl").val(),
                data: {
                    countryByIp: value
                },
                success: function (res) {
                    window.location.href = res.url;
                }
            });
            //window.location.href = redirectTo;
        }
    });


    setSelectedIndex(document.getElementById("CallingFrom"), countryByIp);

    $(".search_drop").dropdown();

    //$('form').preventDoubleSubmission();

});

function loadingButtonStart(btn) {
    $(btn).addClass("has-spinner");
    $(btn).buttonLoader('start');
}

// jQuery plugin to prevent double submission of forms
jQuery.fn.preventDoubleSubmission = function () {
    $(this).on('submit', function (e) {
        var $form = $(this);
        if ($form.valid()) {
            var subby = $("input[type=submit][clicked=true]");
            loadingButtonStart(subby);
            $('input[type=submit],button[type=submit], input[type=button]', this).each(function () {
                loadingButtonStart(this);
                //$(this).attr("disabled", "disabled");
            });
        }
    });
    // Keep chainability
    return this;
};


function setSelectedIndex(s, valsearch) {
    // Loop through all the items in drop down list
    if (s == undefined)
        return;

    for (var i = 0; i < s.options.length; i++) {
        if (s.options[i].value == valsearch) {
            // Item is found. Set its property and exit
            s.options[i].selected = true;
            break;
        }
    }
    return;
}

function openChatWindow() {

    var url = "https://server.iad.liveperson.net/hc/69462672/?cmd=file&file=visitorWantsToChat&site=69462672&referrer=@Request.Url.ToString()";

    window.open(url,
        "mywindow", "location=1,status=1,scrollbars=1,width=400,height=400");

}

$(function () {

    $("#accordion_8").bwlAccordion({
        search: false,
        toggle: true
    });



});

$(document).ajaxStart(function () {
    $.LoadingOverlay("show");
});
$(document).ajaxStop(function () {
    $.LoadingOverlay("hide");
});


