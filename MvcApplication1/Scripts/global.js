

//Right Red Fixed Floating Div JS Start

jQuery(document).ready(function(){
		
	
		jQuery('#post-1,#account-1,#account-2, #post-2').mouseover(function(){
		jQuery(this).stop(true,false);
		$(this).animate({
		right:0
		});
		})
		jQuery('#post-1,#account-1,#account-2, #post-2').mouseout(function(){
		jQuery(this).stop(true,false);
		jQuery(this).animate({
		right: -92
		});
		})
		
		
	});


	
	
	
	