(function(){
 "use strict";
 //nav

// $('ul.navbar-nav li a').click(function(e) {
	// e.preventDefault();
	// $("#sidebar-wrapper").toggleClass("active");
// });
$('ul.navbar-nav').onePageNav({
scrollSpeed: 1000,
scrollThreshold: 0.25
});
 $('a.about-more').click(function() {
          if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') 
            || location.hostname == this.hostname) {

            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
            if (target.length) {
              $('html,body').animate({
                scrollTop: target.offset().top
              }, 1000);
              return false;
            }
          }
        });
 //=========== jquery invew ====================
$('.hidding').bind('inview', function(event, visible) {
var effect = $(this).attr('data-animated');
$(this).addClass(effect);
$(this).addClass('visible');
});

$("body").backstretch("img/.jpg");
 //function
  HeadHeight();
folioHOver();
navClick();
valid();
})();

$(window).load(function(){

	  navClick();
});
$(document).delegate('*[data-toggle="lightbox"]', 'click', function(event) {
    event.preventDefault();
    $(this).ekkoLightbox();
    }); 
$(window).resize(function(){
HeadHeight();
navClick();
});
function HeadHeight(){
var wHeight = $(window).height();
$('header').css('height', wHeight);
}
function folioHOver(){
var animIn = $('.onHover').attr('data-animatedin');
var animOut = $('.onHover').attr('data-animatedout');
 $('.caption').fadeOut();
$('.hcaption').hover(function(){

	$(this).find('.myToggle').fadeIn();
	$(this).find('.attr-icon').fadeIn();
    $(this).find('.caption').removeClass(animOut).addClass(animIn);
    //$(this).find('.right').removeClass('bounceOutRight').addClass('bounceInRight');
    $(this).find('.caption').fadeIn();
    

}, function(){
$(this).find('.myToggle').fadeOut();
//$(this).find('.attr-icon').fadeOut();
    $(this).find('.caption').removeClass(animIn).addClass(animOut);
 //$(this).find('.right').removeClass('bounceInRight').addClass('bounceOutRight');
    $(this).find('.caption').fadeOut();
});
}
 function navClick(){
 
    $('.navbar-default .navbar-nav>li>a').click(function(){
    $('.navbar-collapse').toggleClass('in');
      navMain.on("click", "a:not([data-toggle])", null, function () {
          navMain.collapse('hide');
      });
	});
	
 }
 function valid(){
 $("input,select,textarea").not("[type=submit]").jqBootstrapValidation();
}