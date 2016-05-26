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

