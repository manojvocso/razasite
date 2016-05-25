/* 
 _____   _           _         _                        _                  
|_   _| | |         | |       | |                      | |                 
  | |   | |__   __ _| |_ ___  | |_ ___  _ __ ___   __ _| |_ ___   ___  ___ 
  | |   | '_ \ / _` | __/ _ \ | __/ _ \| '_ ` _ \ / _` | __/ _ \ / _ \/ __|
 _| |_  | | | | (_| | ||  __/ | || (_) | | | | | | (_| | || (_) |  __/\__ \
 \___/  |_| |_|\__,_|\__\___|  \__\___/|_| |_| |_|\__,_|\__\___/ \___||___/

Oh nice, welcome to the js file of dreams. 
Explore the code and see how I build the Apple shop navigation usign Greensock.
Enjoy responsibly!
@ihatetomatoes

*/

var $body = $('body'),
	productbrowser = $('#pb-mac');

$(document).ready(function() {

	var slide = $('.ul-slider'),
		navigationLinks = $('.pb-pageindicator a'),
		activeNavLink = $('.pb-pageindicator a.active'),
		navArrow = $('.caret');
	
	$(productbrowser).addClass('pb-dynamic'); 
	$(slide).not('.ul-slider:eq(0)').hide(); //hide all slides apart from 1st one
	$(slide).eq(0).addClass('active'); //add active class

	// Navigation
	$(navigationLinks).click(function() {
		animateSlides(this);
	});

	/* Slides Animation */
	function animateSlides(e){
		
		$(navigationLinks).unbind('click');
		
		var pageID = $(e).attr('page'),
			pageIDactive = navigationLinks.filter('.active').attr('page'),
			slideIn = $(".ul-slider[page='" + pageID + "']"),
			slideOut = $('.ul-slider.active'),
			products = $($(slideOut).find('li').get()),
			slideInProducts = $($(slideIn).find('li').get()),
			tl = new TimelineMax({onStart:updateNav, onStartParams: [pageID], onStartScope: e, onComplete:resetStage, onCompleteScope: e});
		
		$(slideIn).show();
		$(navigationLinks).removeClass('active');
		$(e).addClass('active'); //add active to active nav item
		
		if(pageID > pageIDactive){
			tl
			.staggerTo(products, 0.6, {css:{x:'-=960'}, ease:Power4.easeOut }, 0.02)
			
			// Final animation with fixed delay
			.staggerFromTo(slideInProducts, 0.5, {css:{x:'960'}, ease:Power4.easeIn }, {css:{x:'30'}, ease:Power4.easeIn }, 0.05) //appends to the end of the timeline
			.staggerTo(slideInProducts, 0.5, {css:{x:'0'}, ease:Elastic.easeOut.config(2) }, 0.05, "-=0.25");  
			
			
		} else {
			var products = $($(slideOut).find('li').get().reverse()),
				slideInProducts = $($(slideIn).find('li').get().reverse());
			tl
			.staggerTo(products, 0.6, {css:{x:'960'}, ease:Power4.easeOut }, 0.02)
			
			// Final animation with fixed delay
			.staggerFromTo(slideInProducts, 0.5, {css:{x:'-=960'}, ease:Power4.easeIn }, {css:{x:'30'}, ease:Power4.easeIn }, 0.05) //appends to the end of the timeline
			.staggerTo(slideInProducts, 0.5, {css:{x:'0'}, ease:Elastic.easeOut.config(2) }, 0.05, "-=0.25");  
		}
		
	}
	
	function updateNav(e, pageID){
		
		$(".ul-slider[page='" + pageID + "']").addClass('active'); //add active to active slide
		
		var centerPoint = parseInt(($(this).width())/2), //center of new active item
			oldOffset = parseInt($(this).offset().left - $(this).parent().offset().left), //old arrow offset
			newOffset = oldOffset + centerPoint + 1, //new arrow offset
			tl = new TimelineMax();
		
		tl.to(navArrow, 0.4, {css:{x:newOffset}, ease:Linear});
	}
	
	function resetStage(e){
		var pageID = $(this).attr('page'),
			currentSlide = $(".ul-slider[page='" + pageID + "']"),
			currentSlideItems = $(".ul-slider[page='" + pageID + "'] li");
		
		//remove active class and hide all slides apart from the current one
		slide.removeClass('active');
		slide.not(currentSlide).hide();
		
		//add class to the active nav item and current slide	
		$(this).add(currentSlide).addClass('active');
		
		$(navigationLinks).not($(this)).bind('click', function(){
			animateSlides(this);
		});
	}
	
	// Keyboard navigation
	$(document.documentElement).keyup(function (event) {
		var direction = null;
		
		// handle cursor keys
		if (event.keyCode == 37) {
			// go left
			direction = 'left';
		} else if (event.keyCode == 39) {
			// go right
			direction = 'right';
		}
		
		if (direction != null) {
		  if([direction] == 'left'){
				//animateRight();
				
				$(navigationLinks).filter('.active').prev('a').trigger('click', function(){
					animateSlides(this);
				});

		  } else if([direction] == 'right') {
				//animateLeft();
				
				$(navigationLinks).filter('.active').next('a').trigger('click', function(){
					animateSlides(this);
				});
		  }
		}
	});
	
});

function init() {
	
	// start up after 2sec no matter what
    window.setTimeout(function(){
        start();
    }, 2000);
}

// fade in experience
function start() {
	
	$('html').removeClass("loading");
	
	var products = $(".ul-slider.active li");
		tl = new TimelineMax();
	tl.staggerFrom(products, 0.5, {css:{y:'-=150',x:'-30',opacity:'0'}, ease:Bounce.easeOut }, 0.05)
	
}

$(window).load(function() {
	
	// fade in page
	init();
	
});

