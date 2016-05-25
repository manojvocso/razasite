// JavaScript Document

 jQuery(function ($) {

jQuery('input.panel').hover(function () {

var $target = $($(this).attr('id')),

$other = $target.siblings('.active'),

animIn = function () {

$target.addClass('active').show().css({

left: +($target.width())+200

}).finish().animate({

left: 0

}, 1000);

};

if (!$target.hasClass('active') && $other.length > 0){

$other.each(function (index, self) {

var $this = $(this);

$this.removeClass('active').animate({

left: +$this.width()+200

}, 1000, animIn);

});

} else if (!$target.hasClass('active')) {

animIn();

}

});

jQuery(document).click(function(e){

var $target = $(e.target), $active = $('div.panel.active');

if($active.length && $target.closest('a.panel').length == 0 && $target.closest($active).length == 0){

$active.removeClass('active').finish().animate({

left: +$active.width()+200

}, 1000);

}

})

jQuery('#right').mouseleave(function() {

//alert("akash akash ");

var $target =$($('input.panel').attr('id'));

if ($target.hasClass('active')) {

$target.removeClass('active').finish().animate({

left: +$target.width()+200

}, 1000);

}

});

$('.submit_login').click(function() {

//alert("akash akash ");

var $target =$($('input.panel').attr('id'));

if ($target.hasClass('active')) {

$target.removeClass('active').finish().animate({

left: +$target.width()+200

}, 1000);

}

});

});
 
 
jQuery(document).ready(function() {
jQuery("#countries, #countries_signup_popup, #countries_searchrates_footer, #countries_tryusfree_footer, #countries_plans").msDropdown();


  jQuery(".close_fixed").click(function(){



    jQuery(".footer_fixed").hide();



  });


})


function getIDofAccLi(id){

	var numitems1 =  jQuery("ul.toggle1 li.shown").length;

	for(i=1;i<=numitems1;i++){

		if(i==id){

			jQuery('ul.toggle1 #1_'+i).slideToggle(300);
			if(jQuery('#hid_2_'+id).hasClass('addopen-new')){



jQuery('#hid_2_'+id).removeClass('addopen-new');

}

else{

jQuery('#hid_2_'+id).addClass('addopen-new');



}

		}

	}





	var numitems2 =  jQuery("ul.toggle2 li.shown").length;

	for(i=1;i<=numitems2;i++){

		if(i==id){

			jQuery('ul.toggle2 #2_'+i).slideToggle(300);

		}

	}

	var numitems3 =  jQuery("ul.toggle3 li.shown").length;

	for(i=1;i<=numitems3;i++){

		if(i==id){

			jQuery('ul.toggle3 #3_'+i).slideToggle(300);

		}

	}

	var numitems4 =  jQuery("ul.toggle4 li.shown").length;

	for(i=1;i<=numitems4;i++){

		if(i==id){

			jQuery('ul.toggle4 #4_'+i).slideToggle(300);

		}

	}



	var numitems5 =  jQuery("ul.toggle5 li.shown").length;

	for(i=1;i<=numitems5;i++){

		if(i==id){

			jQuery('ul.toggle5 #5_'+i).slideToggle(300);

		}

	}

	

	var numitems6 =  jQuery("ul.toggle6 li.shown").length;

	for(i=1;i<=numitems6;i++){

		if(i==id){

			jQuery('ul.toggle6 #6_'+i).slideToggle(300);

		}

	}

	

	var numitems7 =  jQuery("ul.toggle7 li.shown").length;

	for(i=1;i<=numitems7;i++){

		if(i==id){

			jQuery('ul.toggle7 #7_'+i).slideToggle(300);

		}

	}

	

	var numitems8 = jQuery("ul.toggle8 li.shown").length;

	for(i=1;i<=numitems8;i++){

		if(i==id){

			jQuery('ul.toggle8 #8_'+i).slideToggle(300);

		}

	}

	

	var numitems9 =  jQuery("ul.toggle9 li.shown").length;

	for(i=1;i<=numitems9;i++){

		if(i==id){

			jQuery('ul.toggle9 #9_'+i).slideToggle(300);

		}

	}

	

	var numitems10 =  jQuery("ul.toggle10 li.shown").length;

	for(i=1;i<=numitems10;i++){

		if(i==id){

			jQuery('ul.toggle10 #10_'+i).slideToggle(300);

		}

	}

	

	var numitems11 = jQuery("ul.toggle11 li.shown").length;

	for(i=1;i<=numitems11;i++){

		if(i==id){

			jQuery('ul.toggle11 #11_'+i).slideToggle(300);

		}

	}

	

}


 