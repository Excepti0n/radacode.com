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

        /* PROJECT ITEMS INITIALIZATION */

        self.currentProjects = ko.observableArray([]);
        self.page = ko.observable(1);
        self.totalProjects = ko.observable(0);

        var currentModelsArray = jQuery.map(data.InitialProjects, function (val, i) {
            self.currentProjects.push(new ProjectViewModel(val, self));
        });

        self.totalProjects = new Array();
        
        jQuery.each(data.TotalProjectsPerType, function(i, val) {
            self.totalProjects.push({ type: i, count: val });
        });

        self.projectModels = [{ type: data.SelectedProjectTypeId, models: currentModelsArray }];
        
        /* MENU INITIALIZATION */

        self.selectedItemId = ko.observable(data.SelectedProjectTypeId);
        self.menuItems = ko.observableArray([]);

        self.selectItem = function(newId) {
            self.selectedItemId(newId);
        };

        var menuItemModelsArray = jQuery.map(data.MenuItems, function (val, i) {
            self.menuItems.push(new PortfolioProjectTypeMenuItemViewModel(val, self));
        });

        self.LoadMoreProjects = function() {
            var getProjectsUrl = $("#GetNextProjectsUrl").val();
            $.ajax({
                url: getProjectsUrl,
                data: {
                    page: parseInt(self.page(), "10"),
                    type: self.selectedItemId()
                },
                beforeSend: function () {
                    //$("#ajaxload").show();
                },
                success: function (result) {
                    var newlyAddedModelsArray = jQuery.map(result, function (i, val) {
                        self.currentProjects.push(new ProjectViewModel(val, self));
                    });
                    self.page(parseInt(self.page(), "10") + 1);
                    jQuery.each(self.projectModels, function (i, val) {
                        if(val.type == self.selectedItemId()) {
                            val.models = val.models.concat(newlyAddedModelsArray);
                        }
                    });
                },
                error: function () {
                    $("#error").show();
                }
            });
        };
        
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                var projectsPerPage = parseInt($('#ProjectsPerPage').val(), "10");
                var currentTypeTotalProjects;
                jQuery.each(self.totalProjects, function(i, val) {
                    if (val.type == self.selectedItemId()) currentTypeTotalProjects = parseInt(val.count, "10");
                });
                if (parseInt(self.page(), "10") * projectsPerPage < currentTypeTotalProjects) {
                    self.LoadMoreProjects();
                }
            }
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
