!function ($) {
    $(function(){

        var $root = $('html, body');

        $('a').click(function() {
            var href = $.attr(this, 'href');
            $root.animate({
                scrollTop: $(href).offset().top
            }, 700, function () {
                window.location.hash = href;
            });
            return false;
        });
    });       
}(window.jQuery)

$( document ).ready(function() {
//    $( "#submit" ).click(function( event ) {
//            $(this).button('loading')
//            event.preventDefault();
//            var url = "alternative/send.php";
//
//            $.ajax({
//               type: "POST",
//               url: url,
//               data: $("#contact-form").serialize(), // serializes the form's elements.
//               success: function(data)
//               {
//                   if(data!='ko')
//                        $('#contact-div').html(data);
//                    else{
//                        $('#contact-error-div').show();
//                        $('#submit').button('reset');
//                    }
//               }
//            });
//            return false;
//        });
});