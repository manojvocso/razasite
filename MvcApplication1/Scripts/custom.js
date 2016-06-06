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





//footer js start



                    
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
                        setSelectedIndex(document.getElementById("countries_tryusfree_footer"), countryid);
                 
					


//


                function tryusloadPopup() {
                    if (popupStatus == 0) { // if value is 0, show popup
                        closeloading(); // fadeout loading
                        $("#tryus-valpopup").fadeIn(0500); // fadein popup div
                        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
                        $("#backgroundPopup").fadeIn(0001);
                        popupStatus = 1; // and set value to 1
                    }
                }

                $(this).keyup(function (event) {
                    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
                        disablePopup(); // function close pop up
                    }
                });

                function closeloading() {
                    $("div.loader").fadeOut('normal');
                }

                var popupStatus = 0; // set value

                function tryusdisablePopup() {
                    if (popupStatus == 1) { // if value is 1, close popup
                        $("#tryus-valpopup").fadeOut("normal");
                        $("#backgroundPopup").fadeOut("normal");
                        popupStatus = 0; // and set value to 0
                    }
                }


                function navigate() {
                  
                    var trialcountryfrom = $('#countries_tryusfree_footer').val();
                    var istryusvalid = $("#Istryusvalid").val();
                   
                    var trialcountryto = $('#trialcountryto').val();
                    
                    if (trialcountryto == 0) {

                        return false;
                    }
                    if (istryusvalid != "valid" && istryusvalid != "") {
                        // alert("Note: you can't buy this plan.");
                        tryusloadPopup();
                        return false;
                    }
               
                    var url = "/Cart/GetFreeTrialPlan?trialcountryfrom=" + trialcountryfrom + "&trialcountryto=" + trialcountryto;
                    window.location.href = url;
                }
				
				
				
//

                                $("#subscribe").click(function (e) {
                                         
                                    var res = $(".email_inputbox_promotions").val();
                                    var reg = /^([A-Za-z0-9_\-\.])+\@@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

                                    if (reg.test(res) == false) {
                                        $("#div1").text("Invalid Email Address").show();
                                        $('.email_inputbox_promotions').val("");
                                        return false;
                                    }
                                    $.ajax({
                                        url: '/Account/SubscribeMail/',
                                        dataType: 'json',
                                        data: { Email: res },
                                        contentType: 'application/json; charset=UTF-8',
                                        success: function (result) {

                                            $("#div1").html(result);
                                            $('.email_inputbox_promotions').val("");
                                        }
                                    });
                                           
                                });
                                       
                                       
									   
									   
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
                setSelectedIndex(document.getElementById("countries_searchrates_footer"), countryid);
				
				
				
				
				
//


    $(document).ready(function () {

        $(".close_fixed").click(function () {

            $(".footer_fixed").hide();

        });

    });


//


    $(document).ready(function () {

        $("#countries_signup_popup").msDropdown();
        $("#countries_searchrates_footer").msDropdown();
        $("#countries_tryusfree_footer").msDropdown();
        $("#countries_plans").msDropdown();
        $("#countries").msDropdown();
        $("#rate-countries").msDropdown();
        $("#rate-countries-1").msDropdown();
        $("#landline_special").msDropdown();
        $("#compare_plans").msDropdown();
        $("#northamerica").msDropdown();
        $("#southamerica").msDropdown();
        $("#international").msDropdown();
        $("#retail-global-countries").msDropdown();
        $("#NewCust_countries").msDropdown();
        $("#stamp_flag").msDropdown();
        $("#stamp_flag1").msDropdown();
        $("#countries-rate").msDropdown();
        $("#countries-rates").msDropdown();
        $("#from-asia").msDropdown();
        $("#from-africa").msDropdown();
        $("#from-Europe").msDropdown();
        $("#from-middleeast").msDropdown();
        $("#from-southamerica").msDropdown();
        $("#from-North").msDropdown();
        $("#global-countries").msDropdown();
        $("#global-countries_15").msDropdown();
        $("#global-countries_17").msDropdown();
        $("#countries_searchrates_newCustomer").msDropdown();
        $("#countries_searchrates_existCustomer").msDropdown();
        $("#countryfrom-promotion").msDropdown();
      
        
    });


//



    $(document).ready(function () {

        $(".close_fixed").click(function () {

            $(".footer_fixed").hide();

        });

    });

//



    function openwindow() {

        var url = "https://server.iad.liveperson.net/hc/69462672/?cmd=file&file=visitorWantsToChat&site=69462672&referrer=@Request.Url.ToString()";

        window.open(url,
            "mywindow", "location=1,status=1,scrollbars=1,width=400,height=400");

    }


//



    window.lpTag=window.lpTag||{};if(typeof
    window.lpTag._tagCount==='undefined'){window.lpTag={site:'69462672',section:lpTag.section||'',autoStart:lpTag.autoStart===false?false:true,ovr:lpTag.ovr||{},_v:'1.5.1',_tagCount:1,protocol:location.protocol,events:{bind:function(app,ev,fn){lpTag.defer(function(){lpTag.events.bind(app,ev,fn)},0)},trigger:function(app,ev,json){lpTag.defer(function(){lpTag.events.trigger(app,ev,json)},1)}},defer:function(fn,fnType){if(fnType==0){this._defB=this._defB||[];this._defB.push(fn)}else
        if(fnType==1){this._defT=this._defT||[];this._defT.push(fn)}else{this._defL=this._defL||[];this._defL.push(fn)}},load:function(src,chr,id){var
        t=this;setTimeout(function(){t._load(src,chr,id)},0)},_load:function(src,chr,id){var
        url = src; if (!src) { url = this.protocol + '//' + ((this.ovr && this.ovr.domain) ? this.ovr.domain : 'lptag.liveperson.net[2]') + '/tag/tag.js?site=' + this.site }
        
       var s=document.createElement('script');s.setAttribute('charset',chr?chr:'UTF-8');if(id){s.setAttribute('id',id)}s.setAttribute('src',url);document.getElementsByTagName('head').item(0).appendChild(s)},init:function(){this._timing=this._timing||{};this._timing.start=(new
        Date()).getTime();var
        that=this;if(window.attachEvent){window.attachEvent('onload',function(){that._domReady('domReady')})}else{window.addEventListener('DOMContentLoaded',function(){that._domReady('contReady')},false);window.addEventListener('load',function(){that._domReady('domReady')},false)}if(typeof(window._lptStop)=='undefined'){this.load()}},start:function(){this.autoStart=true},_domReady:function(n){if(!this.isDom){this.isDom=true;this.events.trigger('LPT','DOM_READY',{t:n})}this._timing[n]=(new
        Date()).getTime()},vars:lpTag.vars||[],dbs:lpTag.dbs||[],ctn:lpTag.ctn||[],sdes:lpTag.sdes||[],ev:lpTag.ev||[]};lpTag.init()}else{window.lpTag._tagCount+=1}





            									   