var Raza = { };
var basepathUrl;

jQuery(document).ready(function () {
    // for tabs
    $("#f_fixed").css({ "visibility": "visible" });
    $(".container_tabbing").css({ "visibility": "visible" });

    $('.nav-tabs > li > a').click(function (event) {
        event.preventDefault(); //stop browser to take action for clicked anchor
        //get displaying tab content jQuery selector
        var active_tab_selector = $('.nav-tabs > li.active > a').attr('href'); //find actived navigation and remove 'active' css
        var actived_nav = $('.nav-tabs > li.active');
        actived_nav.removeClass('active');
        //add 'active' css into clicked navigation
        $(this).parents('li').addClass('active');
        //hide displaying tab content
        $(active_tab_selector).removeClass('active');
        $(active_tab_selector).addClass('hide');
        //show target tab content
        var target_tab_selector = $(this).attr('href');
        $(target_tab_selector).removeClass('hide');
        $(target_tab_selector).addClass('active');
    });

   
    $(".recharge-now").click(function () {
        $('.image-bar').hide();
        $('.sel-logo').show();
    });

    $("#countries, #countries_signup_popup, #countries_searchrates_footer, #countries_tryusfree_footer, #countries_plans").msDropdown();

    $(".big_box_btn").click(function () {

        $('.image-bar').hide();

        $('.sel-logo').show();
    });


    $("#tabs_support").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
    $("#tabs_support li").removeClass("ui-corner-top").addClass("ui-corner-left");


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

    });

    $('#right').mouseleave(function () {

        var $target = $($('input.panel').attr('id'));

        if ($target.hasClass('active')) {

            $target.removeClass('active').finish().animate({

                left: +$target.width() + 200

            }, 1000);

        }

    });

    $('.submit_login').click(function () {

        var $target = $($('input.panel').attr('id'));

        if ($target.hasClass('active')) {

            $target.removeClass('active').finish().animate({

                left: +$target.width() + 200

            }, 1000);

        }

    });

    // start ticker
    $("#webticker").webTicker();


    activeItem = $("#accordion-explore li:first");

    $(activeItem).addClass('active');



    $("#accordion-explore li").click(function () {

        $(activeItem).animate({ width: "38px" }, { duration: 500, queue: false });

        $(this).animate({ width: "510px" }, { duration: 500, queue: false });

        activeItem = this;

    });
});


// functions

function getIDofAccLi(id) {

    var numitems1 = $("ul.toggle1 li.shown").length;

    for (i = 1; i <= numitems1; i++) {

        if (i == id) {

            $('ul.toggle1 #1_' + i).slideToggle(300);
            if ($('#hid_2_' + id).hasClass('addopen-new')) {

                $('#hid_2_' + id).removeClass('addopen-new');

            }
            else {

                $('#hid_2_' + id).addClass('addopen-new');

            }

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

function addclassli(clas5sname) {
    for (var i = 1; i <= 5; i++) {
        $('#li_' + i).addClass('open_1');
    }

}



function openwindow() {

    window.open("https://server.iad.liveperson.net/hc/69462672/?cmd=file&amp;file=visitorWantsToChat&amp;site=69462672&amp;referrer=http://www.Raza.com",

"mywindow", "location=1,status=1,scrollbars=1,width=400,height=400");

}


function getBaseURL() {
    var url = location.href;  // entire url including querystring - also: window.location.href;
    var baseURL = url.substring(0, url.indexOf('/', 14));


    if (baseURL.indexOf('http://localhost') != -1) {
        // Base Url for localhost
        var url = location.href;  // window.location.href;
        var pathname = location.pathname;  // window.location.pathname;
        var index1 = url.indexOf(pathname);
        var index2 = url.indexOf("/", index1 + 1);
        var baseLocalUrl = url.substr(0, index2);

        return baseLocalUrl + "/";
    }
    else {
        // Root Url for domain name
        return baseURL + "/";
    }

}


function number() {
    // 48 = 0
    // 57 = 9
    if ((event.keyCode < 48) || (event.keyCode > 57)) {
        return false;
    }
    return true;
}


function checknumber() {
    var filled = 0;
    var x = $("#MobileNumber").val();
    x = x.replace(/^\s+/, ""); // strip leading spaces
    if (x.length == 10) { filled++; }

    if (filled == 1) {
        document.getElementById("mobno_submit").disabled = false;
    } else {
        document.getElementById("mobno_submit").disabled = true;
    } // in case a field is filled then erased

}




 