
$(document).ready(function() {

    

    // init and stop the inner slideshows

    var inners = $('.inner-slideshow').cycle().cycle('stop');

    

    var slideshow = $('#slideshow').cycle({

        fx: 'scrollHorz',

        speed: 300,

        timeout: 0,

        prev: '#prev',

        next: '#next',

        before: function() {

            // stop all inner slideshows

            inners.cycle('stop');

            // start the new slide's slideshow

            $(this).cycle({

                fx: 'fade',

                timeout: 2000,

                autostop: true,

                end: function() {

                    // when inner slideshow ends, advance the outer slideshow

                    slideshow.cycle('next');

                }

            });

        }

    });

	    var slideshow = $('#slideshow1').cycle({

        fx: 'scrollHorz',

        speed: 300,

        timeout: 0,

        prev: '#prev1',

        next: '#next1',

        before: function() {

            // stop all inner slideshows

            inners.cycle('stop');

            // start the new slide's slideshow

            $(this).cycle({

                fx: 'fade',

                timeout: 2000,

                autostop: true,

                end: function() {

                    // when inner slideshow ends, advance the outer slideshow

                    slideshow.cycle('next');

                }

            });

        }

    });

	    var slideshow = $('#slideshow2').cycle({

        fx: 'scrollHorz',

        speed: 300,

        timeout: 0,

        prev: '#prev2',

        next: '#next2',

        before: function() {

            // stop all inner slideshows

            inners.cycle('stop');

            // start the new slide's slideshow

            $(this).cycle({

                fx: 'fade',

                timeout: 2000,

                autostop: true,

                end: function() {

                    // when inner slideshow ends, advance the outer slideshow

                    slideshow.cycle('next');

                }

            });

        }

    });

    

});

