RadaCode.PortfolioKO = (function($) {
    "use strict";

    function PortfolioProjectTypeMenuItemViewModel(data, parent) {
        var self = this;

        self.menuItemText = data.ItemText;
        self.itemId = data.ItemId;

        self.menuItemId = ko.computed(function() {
            return self.itemId + '-selector';
        });
        
        self.selected = ko.observable(self.itemId == parent.selectedItemId());

        parent.selectedItemId.subscribe(function(newValue) {
            self.selected(self.itemId == newValue);
        });

        self.selectMenuItem = function() {
            if(!(self.itemId == parent.selectedItemId())) {
                parent.selectItem(self.itemId);
            }
        };
        
        self.menuItemClass = ko.computed(function () {
            if (self.selected()) {
                return 'type-select selected';
            } else {
                return 'type-select';
            }
        });
    }
    
    function ProjectViewModel(data, parent) {
        var self = this;

        self.id = data.Id;
        self.dateFinished = data.DateFinished;
        self.markup = data.Markup;
    }

    function PortfolioPageViewModel(data) {
        var self = this;

        self.currentProjects = ko.observableArray([]);

        var currentModelsArray = jQuery.map(data.InitialProjects, function (val, i) {
            self.currentProjects.push(new ProjectViewModel(val, self));
        });

        self.projectModels = [{ type: data.SelectedProjectTypeId, models: currentModelsArray }];
        
        self.selectedItemId = ko.observable(data.SelectedProjectTypeId);
        self.menuItems = ko.observableArray([]);

        self.selectItem = function(newId) {
            self.selectedItemId(newId);
        };

        var menuItemModelsArray = jQuery.map(data.MenuItems, function (val, i) {
            self.menuItems.push(new PortfolioProjectTypeMenuItemViewModel(val, self));
        });

        
        
        
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

    var ready = $(function() {
        RadaCode.PortfolioKO.init();
    });
}($));
