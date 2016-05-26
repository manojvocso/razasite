// JavaScript Document

//Dialog Bog//

jQuery(function() {

	// load the modal window
	jQuery('a.modal').click(function(){
	
		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		jQuery('#comment').val('Enter your comment or query...');

	    //reset all the fileds for quicksignup
		document.getElementById("signup-hed-error1").innerHTML = "";
		document.getElementById("signup-hed-error2").innerHTML = "";
		document.getElementById("signup-hed-error3").innerHTML = "";
		document.getElementById("signup-hed-error4").innerHTML = "";
		document.getElementById("signup-hed-error5").innerHTML = "";
	
		document.getElementById("countries_signup_popup").value = '';
		document.getElementById("Email").value = 'Email address';
		document.getElementById("TypePassword").value = 'Password';
		document.getElementById("TypePassword2").value = 'Confirm Password';
		document.getElementById("Phone_Number").value = 'Phone Number';
	    $("#errorexpandable").css("display", "none");

		//show the mask and contact divs
		jQuery('#mask').show().fadeTo('', 0.7);
		jQuery('div#contact').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close, div#mask').click(function() {
		jQuery('div#contact, div#mask').stop().fadeOut('slow');

	});


	// popu2
	jQuery('a.modal-new').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		jQuery('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask-new').show().fadeTo('', 0.7);
		jQuery('div#contact-new').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-new, div#mask-new').click(function() {
		jQuery('div#contact-new, div#mask-new').stop().fadeOut('slow');

	});
	
	
	// popu3
	jQuery('a.modal-ie').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		jQuery('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask-ie').show().fadeTo('', 0.7);
		jQuery('div#contact-ie').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-ie, div#mask-ie').click(function() {
		jQuery('div#contact-ie, div#mask-ie').stop().fadeOut('slow');

	});

	
	// popu4
	jQuery('a.modal-cvv').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask-cvv').show().fadeTo('', 0.7);
		jQuery('div#contact-cvv').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-cvv, div#mask-cvv').click(function() {
		jQuery('div#contact-cvv, div#mask-cvv').stop().fadeOut('slow');

	});
	
	// popu5
	jQuery('a.modal-learn').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		jQuery('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask-learn').show().fadeTo('', 0.7);
		jQuery('div#contact-learn').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-learn, div#mask-learn').click(function() {
		jQuery('div#contact-learn, div#mask-learn').stop().fadeOut('slow');

	});
	
	
	// popup8
	jQuery('a.modal-imail').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		jQuery('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask-imail').show().fadeTo('', 0.7);
		jQuery('div#contact-imail').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-imail, div#mask-imail').click(function () {
		jQuery('div#contact-imail, div#mask-imail').stop().fadeOut('slow');

	});
	

	jQuery('#contactForm input').focus(function() {
		jQuery(this).val(' ');
	});
	
	jQuery('#contactForm textarea').focus(function() {
        jQuery(this).val('');
    });

	// when the Submit button is clicked...
	jQuery('input#submit').click(function() {
	jQuery('.error').hide().remove();
		//Inputed Strings
		var username = jQuery('#name').val(),
			email = jQuery('#email').val(),
			comment = jQuery('#comment').val();
		
	
		//Error Count
		var error_count;
		
		//Regex Strings
		var username_regex = /^[a-z0-9_-]{3,16}$/,
			email_regex = /^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/;
		
			//Test Username
			if(!username_regex.test(name)) {
				jQuery('#contact_header').after('<p class=error>Invalid username entered!</p>');
				error_count += 1;
			}
			
			//Test Email
			if(!email_regex.test(email)) {
				jQuery('#contact_header').after('<p class=error>Invalid email entered!</p>');
				error_count += 1;
			}
			
			//Blank Comment?
			if(comment == '') {
				jQuery('#contact_header').after('<p class=error>No Comment was entered!</p>');
				error_count += 1;
			}
			
			//No Errors?
			if(error_count === 0) {
				jQuery.ajax({
					type: "post",
					url: "send.php",
					data: "name=" + name + "&email=" + email + "&comment=" + comment,
					error: function() {
						jQuery('.error').hide();
						jQuery('#sendError').slideDown('slow');
					},
					success: function () {
						jQuery('.error').hide();
						jQuery('.success').slideDown('slow');
						jQuery('form#contactForm').fadeOut('slow');
					}				
				});	
			}
			
			else {
                jQuery('.error').show();
            }
			
		return false;
	});
	
});


jQuery(function() {

	// load the modal window
	jQuery('a.modal_map').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_map').show().fadeTo('', 0.7);
		jQuery('div#contact_map').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#map-close, div#mask_map').click(function() {
		jQuery('div#contact_map, div#mask_map').stop().fadeOut('slow');

	});
});



jQuery(function() {

	// load the modal window
	jQuery('a.modal_africa').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_africa').show().fadeTo('', 0.7);
		jQuery('div#contact_africa').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#africa-close, div#mask_africa').click(function() {
		jQuery('div#contact_africa, div#mask_africa').stop().fadeOut('slow');

	});
});

jQuery(function() {

	// load the modal window
	jQuery('a.modal_europe').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_europe').show().fadeTo('', 0.7);
		jQuery('div#contact_europe').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-europe, div#mask_europe').click(function() {
		jQuery('div#contact_europe, div#mask_europe').stop().fadeOut('slow');

	});
});


jQuery(function() {

	// load the modal window
	jQuery('a.modal_north').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_north').show().fadeTo('', 0.7);
		jQuery('div#contact_north').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-north, div#mask_north').click(function() {
		jQuery('div#contact_north, div#mask_north').stop().fadeOut('slow');

	});
});


jQuery(function() {

	// load the modal window
	jQuery('a.modal_south').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_south').show().fadeTo('', 0.7);
		jQuery('div#contact_south').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-south, div#mask_south').click(function() {
		jQuery('div#contact_south, div#mask_south').stop().fadeOut('slow');

	});
});
jQuery(function() {

	// load the modal window
	jQuery('a.modal_middle').click(function(){

		// scroll to top
		jQuery('html, body').animate({scrollTop:0}, 'fast');

		// before showing the modal window, reset the form incase of previous use.
		jQuery('.success, .error').hide();
		jQuery('form#contactForm').show();
		
		// Reset all the default values in the form fields
		jQuery('#name').val('Your name');
		jQuery('#email').val('Your email address');
		$('#comment').val('Enter your comment or query...');

		//show the mask and contact divs
		jQuery('#mask_middle').show().fadeTo('', 0.7);
		jQuery('div#contact_middle').fadeIn();

		// stop the modal link from doing its default action
		return false;
	});

	// close the modal window is close div or mask div are clicked.
	jQuery('div#close-middle, div#mask_middle').click(function() {
		jQuery('div#contact_middle, div#mask_middle').stop().fadeOut('slow');

	});
});






