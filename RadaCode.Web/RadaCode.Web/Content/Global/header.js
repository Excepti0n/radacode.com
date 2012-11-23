RadaCode.HeaderLogic = (function ($) {
    "use strict";

    var ready = $(function () {
        $('header #logo img').hover(function() {
            var $element = $(this);
            $element.attr("src", "/Content/global/img/logo-ru-selected.png");
        }, function() {
            var $element = $(this);
            $element.attr("src", "/Content/global/img/logo-ru.png");
        });
    });
}($));