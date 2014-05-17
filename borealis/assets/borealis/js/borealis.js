//Boreales component resizer
//Author: GerardSoleCa (http://twitter.com/GerardSoleCa)

$( document ).ready(function() {
	// Get the real window height
	var realHeight = $(window).height()-$("nav").height(); 

	// Click listener over the signup button (navbar)
	$( "#signup-btn" ).click(function() {
		$("#signin-row").fadeOut( 400,"linear", function() {

			realHeight = $(window).height()-$("nav").height();
			
			// Get the height of the hidden panel using jquery.actual.js and recalculate position
			if(realHeight>$("#signup-panel").actual('outerHeight'))			
				$("#signup-panel").css("margin-top", ($(document).height()-$("nav").height()-$("#signup-panel").actual('outerHeight'))/2);
			$("#signup-row").fadeIn(400,"linear");
		});
	});

	$( "#back-to-login-btn" ).click(function() {
		$("#signup-row").fadeOut( 400,"linear", function() { 
			realHeight = $(window).height()-$("nav").height();

			// Get the height of the hidden panel using jquery.actual.js and recalculate position
			if(realHeight>$("#signin-panel").actual('outerHeight'))	
				$("#signin-panel").css("margin-top", ($(document).height()-$("nav").height()-$("#signin-panel").actual('outerHeight'))/2);
			$("#signin-row").fadeIn(400,"linear");
		});
	});

	// Get the height of the hidden panel using jquery.actual.js and recalculate position
	// And init the boxes
	if(realHeight>$("#signin-panel").actual('outerHeight'))
		$("#signin-panel").css("margin-top", ($(document).height()-$("nav").height()-$("#signin-panel").actual('outerHeight'))/2);
	$("#signin-row").fadeIn(400,"linear");
	$("#footer-nav").fadeIn(400,"linear");
});

// Windows resize listener to recalculate module positions
$(window).resize(function () { 
	realHeight = $(window).height()-$("nav").height();

	$("#signin-panel").css("margin-top",0);
	$("#signup-panel").css("margin-top",0);

	if(realHeight>$("#signup-panel").actual('outerHeight'))			
		$("#signup-panel").css("margin-top", ($(document).height()-$("nav").height()-$("#signup-panel").actual('outerHeight'))/2);
	if(realHeight>$("#signin-panel").actual('outerHeight'))	
		$("#signin-panel").css("margin-top", ($(document).height()-$("nav").height()-$("#signin-panel").actual('outerHeight'))/2);
});

// public variable to know if background is static or dynamic
var isDynamic = true;
$( document ).ready(function() {
	$( "#background-btn" ).click(function() {
		if(isDynamic){
			$('body').fadeTo('slow', 0, function()
			{
				$("#canvas").remove().fadeIn(400);
				$('body').prepend( "<canvas id=\"canvas\"></canvas>" ).fadeIn(400);
				$('body').css("background-image","url('assets/borealis/img/background.jpg')").fadeIn(400);
				$('#background-btn-txt').html("Go Dynamic").fadeIn(400);
			}).fadeTo('slow', 1);
		}
		else{
			$('body').fadeTo('slow', 0, function()
			{
				$('body').css("background-image","").fadeIn(400);
				bokeh();
				$('#background-btn-txt').html("Go Static").fadeIn(400);
			}).fadeTo('slow', 1);
		}
		isDynamic = !isDynamic;
	});
});