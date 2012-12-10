RadaCode.HeaderLogic = (function ($) {
    "use strict";

    var ready = $(function () {

        var baseImgUrl = $('header #logo img').attr("src");
        baseImgUrl = baseImgUrl.substring(0, baseImgUrl.length - 4);

        $('header #logo img').hover(function() {
            var $element = $(this);
            $element.attr("src", baseImgUrl + "-selected.png");
        }, function() {
            var $element = $(this);
            $element.attr("src", baseImgUrl + ".png");
        });

        var $nav = $('menu');

        $('#mobile_menu_link').click(function () {
            $nav.toggleClass('open');

            if ($nav.hasClass('open')) {
                $(this).html('<span class="ss-icon">&#x2421;</span>' + $('header > #close-string').val());
            } else {
                $(this).html('<span class="ss-icon">&#x002B;</span>' + $('header > #menu-string').val());
            }

            return false;
        });
    });
}($));