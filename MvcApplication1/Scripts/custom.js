		$(document).ready(function(){	
			$("#slider").easySlider({
				auto: true,
				continuous: true,
				nextId: "slider1next",
				prevId: "slider1prev"
			});

		});	





//New Fade In Fade Out JS Start

 	var fadeDuration=2000;
	var slideDuration=4000;
	var currentIndex=1;
	var nextIndex=1;
	$(document).ready(function()
	{
		$('.slideshow div img').corner();
		$('.slideshow div').css({opacity: 0.0});
		$("'.slideshow div:nth-child("+nextIndex+")'").addClass('show').animate({opacity: 1.0}, fadeDuration);
		var timer = setInterval('nextSlide()',slideDuration);
	})
	function nextSlide(){
			nextIndex =currentIndex+1;
			if(nextIndex > $('ul.slideshow li').length)
			{
				nextIndex =1;
			}
			$("'.slideshow div:nth-child("+nextIndex+")'").addClass('show').animate({opacity: 1.0}, fadeDuration);
			$("'.slideshow div:nth-child("+currentIndex+")'").animate({opacity: 0.0}, fadeDuration).removeClass('show');
			currentIndex = nextIndex;
	}



//

    $(function () {
        $("#standard").customselect();
    });
	
	
	
//




    jQuery(function ($) {



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


        $(document).click(function(e) {
            var $target = $(e.target), $active = $('div.panel.active');
            if ($active.length && $target.closest('a.panel').length == 0 && $target.closest($active).length == 0) {
                var inp_50_1 = $.trim(jQuery('#EmailAddress').val());
                var inp_10_1 = $.trim(jQuery('#Password').val());
                if (inp_50_1.length == 0 && inp_10_1.length == 0) {
                    $active.removeClass('active').finish().animate({
                        left: +$active.width() + 200
                    }, 1000);
                }


            }

        });

        $('#right').mouseleave(function () {

            //alert("akash akash ");
            var inp_50_1 = $.trim(jQuery('#EmailAddress').val());
            var inp_10_1 = $.trim(jQuery('#Password').val());


            var $target = $($('input.panel').attr('id'));
            if (jQuery('#EmailAddress').is(':focus') || jQuery('#Password').is(':focus')) {
                return false;
            }
            if (inp_50_1.length == 0 && inp_10_1.length == 0) {
                if ($target.hasClass('active')) {

                    $target.removeClass('active').finish().animate({

                        left: +$target.width() + 200

                    }, 1000);

                }
            }

            //if (inp_50_1.length == 0 && inp_10_1.length == 0) {
            //    if ($target.hasClass('active')) {

            //        $target.removeClass('active').finish().animate({

            //            left: +$target.width() + 200

            //        }, 1000);

            //    }
            //}
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



    });



//



    function closedrop_down() {
        /* $("#dropvalue").html('');
        $("#dropvalue").fadeOut(); */
        if ($("#dropvalue_a").html() == '') {
            $("#dropvalue_a").html('Enter the Country name you wish to call');
        }
        else if ($("#dropvalue_a").html() == 'Enter the Country name you wish to call') {
            $("#dropvalue").html('');
            $("#dropvalue").fadeOut();
        }

    }


//




    function openwindow() {

        var url = "https://server.iad.liveperson.net/hc/69462672/?cmd=file&file=visitorWantsToChat&site=69462672&referrer=@Request.Url.ToString()";

        window.open(url,
            "mywindow", "location=1,status=1,scrollbars=1,width=400,height=400");

    }

//




     
                var countryid = $("#country-byip").val();
                function setSelectedIndex(s, valsearch) {
                    // Loop through all the items in drop down list
                    for (var i = 0; i < s.options.length; i++) {
                        if (s.options[i].value == valsearch) {
                            // Item is found. Set its property and exit
                            s.options[i].selected = true;
                            break;
                        }
                    }
                    return;
                }
                setSelectedIndex(document.getElementById("countries"), countryid);

                function updatenumber() {

                    $("#country-change").submit();

                }


           
			
			
//



    var countryid = $("#country-byip").val();
    function setSelectedIndex(s, valsearch) {
        // Loop through all the items in drop down list
        for (var i = 0; i < s.options.length; i++) {
            if (s.options[i].value == valsearch) {
                // Item is found. Set its property and exit
                s.options[i].selected = true;
                break;
            }
        }
        return;
    }
    setSelectedIndex(document.getElementById("countries_signup_popup"), countryid);




//


    function getBlank(id, value) {
        if (jQuery('#' + id).val() == value) {
            document.getElementById(id).value = '';
        }
        //document.getElementById(div).value='';
    }
    function getVal(div, str) {
        if (document.getElementById(div).value == "")
            document.getElementById(div).value = str;
    }



//


    var lbs = "Email-address";
    var txtWeight = document.getElementById("Email");
    txtWeight.value = lbs;


//



    $(document).ready(function () {
        changeAttrTxt('NewPassword');
        changeAttrTxt('ConfirmPassword');
        changeAttrTxt('TypePassword');
        changeAttrTxt('TypePassword2');
        changeAttrTxt('Password');
        changeAttrTxt('EmailAddress');
    });

    function changeAttrPwd(element) {
        if ($("#" + element).val() != '') {
            $("#" + element).attr('type', 'password');
        }
        else {
            $("#" + element).attr('type', 'text');
        }
    }
    function changeAttrPwd2(element) {
        //if ($("#" + element).val() != '') {


        //Commented on 111414 so that confirm password does not show characters
        //$("#" + element).attr('type', 'confirm password');
        $("#" + element).attr('type', 'password');




        //}
        //else {
        //    $("#" + element).attr('type', 'text');
        //}
    }


    function changeAttrTxt(element) {
        $("#" + element).attr('type', 'text');
    }


//
