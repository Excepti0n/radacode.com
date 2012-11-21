RadaCode.PortfolioKO = (function($) {
    "use strict";

    function PortfolioProjectTypeMenuItemViewModel(data, parent) {
        
    }

    function PortfolioPageViewModel(data) {
        var self = this;

        self.MenuItems = ko.observableArray([]);
        
    }

    var init = function() {
        var initialPortfolioViewModelData = $.parseJSON($('#initial-portfolio-view-model-data').val());
        var vm = new PortfolioPageViewModel(initialPortfolioViewModelData);

        ko.applyBindings(vm, document.getElementById("portfolio-control"));
    };

    return { init: init };
}($));

RadaCode.PortfolioLogic = (function ($) {
    "use strict";

    ready = $(function() {
        RadaCode.PortfolioKO.init();
    });
}($));
