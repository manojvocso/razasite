// JavaScript Document

// Nested Accordion
$("html").addClass("js");
$.fn.accordion.defaults.container = false; 
$(function() {
  $("#acc3").accordion({
					   // initShow: "#current"
					   //initShow : "li > ul:eq(1)",
					   initShow : "li > ul > li:eq(1)",
					
					   
					   });

  $("html").removeClass("js");
});

$(function () {
    $("#acc-cll").accordion({
        // initShow: "#current"
        //initShow : "li > ul:eq(1)",
        initShow: "li > ul > li:eq(1)",

    });

    $("html").removeClass("js");
});

$("html").addClass("js");
$.fn.accordion.defaults.container = false; 
$(function() {
  $("#acc4").accordion({initShow: "#current"});

  $("html").removeClass("js");
});

$("html").addClass("js");
$.fn.accordion.defaults.container = false; 
$(function() {
  $("#acc5").accordion({initShow: "#current"});

  $("html").removeClass("js");
});

$("html").addClass("js");
$.fn.accordion.defaults.container = false; 
$(function() {
  $("#acc6").accordion({initShow: "#current"});

  $("html").removeClass("js");
});
$(function() {
  $("#acc7").accordion({initShow: "#current"});

  $("html").removeClass("js");
});
$(function() {
  $("#acc8").accordion({initShow: "#current"});

  $("html").removeClass("js");
});
$(function() {
  $("#acc9").accordion({initShow: "#current"});

  $("html").removeClass("js");
});

$(function() {
  $("#acc10").accordion({initShow: "#current"});

  $("html").removeClass("js");
});

$(function() {
  $("#acc11").accordion({initShow: "#current"});

  $("html").removeClass("js");
});
$(function() {
  $("#acc12").accordion({initShow: "#current"});

  $("html").removeClass("js");
});
$(function() {
  $("#acc13").accordion({initShow: "#current"});

  $("html").removeClass("js");
});