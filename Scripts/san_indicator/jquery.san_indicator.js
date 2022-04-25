(function ($) {
    $.sanIndicator = function () {
        if ($('#sanIndicator_Overlay').length) {
            return false;
        }

        $('<div id="sanIndicator_Overlay"><div id="sanIndicator_Box"></div></div>').hide().appendTo('body').fadeIn();
        $('#sanIndicator_Box').activity({ length: 4, align: 'center', padding: 10 });

        return true;
    }

    $.sanIndicator.hide = function () {
        $('#sanIndicator_Overlay').fadeOut(function () {
            $(this).remove();
        });
    }
})(jQuery);