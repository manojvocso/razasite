// JavaScript Document
$(function(){
	$('.ftr_tabs-section').hide();
	$('#ftr_tabs a').bind('click', function(e){
		$('#ftr_tabs a.current').removeClass('current');
		$('.ftr_tabs-section:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});


$(function(){
	$('.newtab-section-under').hide();
	$('#retail-tabs-under a').bind('click', function(e){
		$('#retail-tabs-under a.current').removeClass('current');
		$('.newtab-section-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});


$(function(){
	$('.global_call-under').hide();
	$('#global_call a').bind('click', function(e){
		$('#global_call a.current').removeClass('current');
		$('.global_call-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});




$(function(){
	$('.retail_fill-under').hide();
	$('#retail_fill a').bind('click', function(e){
		$('#retail_fill a.current').removeClass('current');
		$('.retail_fill-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});





$(function(){
	$('.web-affiliate-under').hide();
	$('#web-affiliate a').bind('click', function(e){
		$('#web-affiliate a.current').removeClass('current');
		$('.web-affiliate-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});




$(function(){
	$('.one-touch-under').hide();
	$('#one-touch a').bind('click', function(e){
		$('#one-touch a.current').removeClass('current');
		$('.one-touch-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});




$(function(){
	$('.raza_rewards-under').hide();
	$('#raza_rewards a').bind('click', function(e){
		$('#raza_rewards a.current').removeClass('current');
		$('.raza_rewards-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});




$(function(){
	$('.auto_refill-under').hide();
	$('#auto_refill a').bind('click', function(e){
		$('#auto_refill a.current').removeClass('current');
		$('.auto_refill-under:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});

$(function(){
	$('.newtab-section').hide();
	$('#retail-tabs a').bind('click', function(e){
		$('#retail-tabs a.current').removeClass('current');
		$('.newtab-section:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		if ($(this).attr('href') == '#newtabs-1') {
		    $('#image_plan').attr('src', '/images/plan_global.jpg');
		}
		 if($(this).attr('href')=='#newtabs-2')
		{
			$('#image_plan').attr('src','/images/plan_mobiledirect.jpg');
		}
		else if($(this).attr('href')=='#newtabs-3')
		{
			$('#image_plan').attr('src','/images/plan_landline_page.jpg');
		}
		else if($(this).attr('href')=='#newtabs-4')
		{
			$('#image_plan').attr('src','/images/compare_plan_page.jpg');
		}
		else if($(this).attr('href')=='#newtabs-5')
		{
			$('#image_plan').attr('src','/images/howitworks_plan_page.jpg');
		}
	
		e.preventDefault();
	})
});







$(function(){
	$('.tab-section').hide();
	$('#map-tabs a').bind('click', function(e){
		$('#map-tabs a.current').removeClass('current');
		$('.tab-section:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter(':first').click();
});

$(function(){
	$('.tab-section-new').hide();
	$('#map-tabs-new a').bind('click', function(e){
		$('#map-tabs-new a.current').removeClass('current');
		$('.tab-section-new:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter('.second-tab').click();
});

$(function(){
	$('.tabs-section-europe').hide();
	$('#map-tabs-europe a').bind('click', function(e){
		$('#map-tabs-europe a.current').removeClass('current');
		$('.tabs-section-europe:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter('.second-tab').click();
});
$(function(){
	$('.tabs-section-north').hide();
	$('#map-tabs-north a').bind('click', function(e){
		$('#map-tabs-north a.current').removeClass('current');
		$('.tabs-section-north:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter('.second-tab').click();
});
$(function(){
	$('.tabs-section-south').hide();
	$('#map-tabs-south a').bind('click', function(e){
		$('#map-tabs-south a.current').removeClass('current');
		$('.tabs-section-south:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter('.second-tab').click();
});
$(function(){
	$('.tabs-section-middle').hide();
	$('#map-tabs-middle a').bind('click', function(e){
		$('#map-tabs-middle a.current').removeClass('current');
		$('.tabs-section-middle:visible').hide();
		$(this.hash).show();
		$(this).addClass('current');
		e.preventDefault();
	}).filter('.second-tab').click();
});


