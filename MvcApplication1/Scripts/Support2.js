$(document).ready(function () {

   // $("html, body").animate({ scrollTop: 0 }, "slow");
    // Accordion
    
    $("#tabs_support").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
    $("#tabs_support li").removeClass("ui-corner-top").addClass("ui-corner-left");

    $('.nav-tabs > li > a').click(function (event) {

        event.preventDefault(); //stop browser to take action for clicked anchor
        //get displaying tab content jQuery selector
        var active_tab_selector = $('.nav-tabs > li.active > a').attr('href');

        var active_class = $(active_tab_selector).attr('class');
        var class_list = active_class.split(" ");
        $("." + class_list[0]).removeClass('active');
        $("." + class_list[0]).addClass('hide');

        //find actived navigation and remove 'active' css
        var actived_nav = $('.nav-tabs > li.active');
        actived_nav.removeClass('active');

        //add 'active' css into clicked navigation

        $(this).parents('li').addClass('active');
        //hide displaying tab content
        $(active_tab_selector).addClass('hide');
        $(active_tab_selector).removeClass('active');

        //show target tab content
        var target_tab_selector = $(this).attr('href');
        $(target_tab_selector).removeClass('hide');
        $(target_tab_selector).addClass('active');
    });

    /**************************/
    $('.ui-tabs-nav > li > a').click(function () {
        var div_li = $(this).attr('href');
        var active_div = $(div_li + "> div.inner>div>ul>li:first>a").attr('href');
        $(active_div).addClass('active');
        $(active_div).removeClass('hide');
        $(div_li + "> div.inner>div>ul>li:first").addClass('active');

        if (div_li == "#tabs-5") {

            if (userauthenticated == false) {

                window.location = "/Account/LogOn?returnUrl=Support_tabs-5";
            }
        }
    });
    $("#webticker").webTicker();

    $('input.panel').hover(function () {

        var $target = $($(this).attr('id')),

        $other = $target.siblings('.active'),

        animIn = function () {

            $target.addClass('active').show().css({

                left: +($target.width()) + 200

            }).finish().animate({

                left: 0

            }, 1000);

        };



        if (!$target.hasClass('active') && $other.length > 0) {

            $other.each(function (index, self) {

                var $this = $(this);

                $this.removeClass('active').animate({

                    left: +$this.width() + 200

                }, 1000, animIn);

            });

        } else if (!$target.hasClass('active')) {

            animIn();

        }

    });
    $(document).click(function (e) {

        var $target = $(e.target), $active = $('div.panel.active');



        if ($active.length && $target.closest('a.panel').length == 0 && $target.closest($active).length == 0) {

            $active.removeClass('active').finish().animate({

                left: +$active.width() + 200

            }, 1000);

        }

    })
    $('#right').mouseleave(function () {

        var $target = $($('input.panel').attr('id'));

        if ($target.hasClass('active')) {

            $target.removeClass('active').finish().animate({

                left: +$target.width() + 200

            }, 1000);

        }

    });
    $('.submit_login').click(function () {

        //alert("akash akash ");

        var $target = $($('input.panel').attr('id'));

        if ($target.hasClass('active')) {

            $target.removeClass('active').finish().animate({

                left: +$target.width() + 200

            }, 1000);

        }

    });

    $("#tabs_support").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
    $("#tabs_support li").removeClass("ui-corner-top").addClass("ui-corner-left");

});


function getIDofAccLi(id) {

    if ($("#hid_1_" + id).hasClass("open")) {

        $("#hid_1_" + id).removeClass("open");
    }
    else {
        $("#hid_1_" + id).addClass("open");
    }

    if ($("#hid_2_" + id).hasClass("open")) {

        $("#hid_2_" + id).removeClass("open");
    }
    else {
        $("#hid_2_" + id).addClass("open");
    }

    if ($("#hid_4_" + id).hasClass("open")) {

        $("#hid_4_" + id).removeClass("open");
    }
    else {
        $("#hid_4_" + id).addClass("open");
    }

    if ($("#hid_6_" + id).hasClass("open")) {

        $("#hid_6_" + id).removeClass("open");
    }
    else {
        $("#hid_6_" + id).addClass("open");
    }

    if ($("#hid_7_" + id).hasClass("open")) {

        $("#hid_7_" + id).removeClass("open");
    }
    else {
        $("#hid_7_" + id).addClass("open");
    }

    if ($("#hid_8_" + id).hasClass("open")) {

        $("#hid_8_" + id).removeClass("open");
    }
    else {
        $("#hid_8_" + id).addClass("open");
    }

    if ($("#hid_9_" + id).hasClass("open")) {

        $("#hid_9_" + id).removeClass("open");
    }
    else {
        $("#hid_9_" + id).addClass("open");
    }

    if ($("#hid_10_" + id).hasClass("open")) {

        $("#hid_10_" + id).removeClass("open");
    }
    else {
        $("#hid_10_" + id).addClass("open");
    }

    if ($("#hid_5_" + id).hasClass("open")) {

        $("#hid_5_" + id).removeClass("open");
    }
    else {
        $("#hid_5_" + id).addClass("open");
    }

    var numitems1 = $("ul.toggle1 li.shown").length;
    for (i = 1; i <= numitems1; i++) {
        if (i == id) {

            $('ul.toggle1 #1_' + i).slideToggle(300);
        }
    }

    var numitems2 = $("ul.toggle2 li.shown").length;
    for (i = 1; i <= numitems2; i++) {
        if (i == id) {
            $('ul.toggle2 #2_' + i).slideToggle(300);

        }
    }

    var numitems3 = $("ul.toggle3 li.shown").length;
    for (i = 1; i <= numitems3; i++) {
        if (i == id) {
            $('ul.toggle3 #3_' + i).slideToggle(300);

        }
    }

    var numitems4 = $("ul.toggle4 li.shown").length;
    for (i = 1; i <= numitems4; i++) {
        if (i == id) {
            $('ul.toggle4 #4_' + i).slideToggle(300);
        }
    }

    var numitems5 = $("ul.toggle5 li.shown").length;
    for (i = 1; i <= numitems5; i++) {
        if (i == id) {
            $('ul.toggle5 #5_' + i).slideToggle(300);
        }
    }


    var numitems6 = $("ul.toggle6 li.shown").length;
    for (i = 1; i <= numitems6; i++) {
        if (i == id) {
            $('ul.toggle6 #6_' + i).slideToggle(300);
        }
    }


    var numitems7 = $("ul.toggle7 li.shown").length;
    for (i = 1; i <= numitems7; i++) {
        if (i == id) {
            $('ul.toggle7 #7_' + i).slideToggle(300);

        }

    }


    var numitems8 = $("ul.toggle8 li.shown").length;
    for (i = 1; i <= numitems8; i++) {
        if (i == id) {
            $('ul.toggle8 #8_' + i).slideToggle(300);
        }
    }


    var numitems9 = $("ul.toggle9 li.shown").length;
    for (i = 1; i <= numitems9; i++) {
        if (i == id) {
            $('ul.toggle9 #9_' + i).slideToggle(300);
        }
    }


    var numitems10 = $("ul.toggle10 li.shown").length;
    for (i = 1; i <= numitems10; i++) {
        if (i == id) {
            $('ul.toggle10 #10_' + i).slideToggle(300);
        }
    }
}
function faq(ov_id, faq_id) {
    $("#ovr_" + ov_id).removeClass("active");
    $("#tab" + ov_id).removeClass("active");
    $("#tab" + ov_id).addClass("hide");

    $("#faq_" + faq_id).addClass("active");
    $("#tab" + faq_id).removeClass("hide");
    $("#tab" + faq_id).addClass("active");

}
function act(act_id) {
    $("#ovr_" + act_id).addClass("active");
    $("#tab" + act_id).addClass("active");
}