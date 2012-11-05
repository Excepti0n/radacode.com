function IndustryModel(data, parent) {
    var self = this;

    self.Id = data.Id;
    self.Name = ko.observable(data.Name);
    self.NameChanged = ko.observable(false);
    self.Name.subscribe(function () {
        self.NameChanged(true);
    });
    
    self.Remove = function () {
        var removeIndustryUrl = $('#RemoveIndustryUrl').val();
        $.ajax({
            type: 'POST',
            url: removeIndustryUrl,
            data: {
                id: self.Id
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    parent.RemoveIndustry(self);
                } else {
                    alert("There was an error removing an idustry - " + res);
                }
            }
        });
    };

    self.SaveUpdatedIndustry = function() {
        var updateIndustryUrl = $('#UpdateIndustryUrl').val();
        $.ajax({
            type: 'POST',
            url: updateIndustryUrl,
            data: {
                id: self.Id,
                newName: self.Name()
            },
            success: function(res) {
                if (res.status === "SPCD: OK") {
                    self.NameChanged(false);
                } else {
                    alert("There was an error updating an industry");
                }
            }
        });
    };
}

function CustomerModel(data, parent) {
    var self = this;
}

function ProjectModel(data, parent) {
    var self = this;
}

function ProjectsViewModel(data) {
    var self = this;

    self.Industries = ko.observableArray([]);
    var industryModelsArray = jQuery.map(data.Industries, function (val, i) {
        self.Industries.push(new IndustryModel(val, self));
    });
    
    self.RemoveIndustry = function (industryVM) {
        self.Industries.remove(industryVM);
    };

    self.isAddIndustryDialogVisible = ko.observable(false);
    self.NewIndustry = { };
    self.NewIndustry.Name = ko.observable('');
    self.NewIndustryErrors = ko.validation.group(self.NewIndustry);

    self.addIndustryDialogOptions = {
        autoOpen: false,
        modal: true,
        height: 200,
        width: 800,
        title: 'Добавление новой индустрии',
        open: function () {
            $('.chzn-select').chosen();
        },
        buttons: {
            'Добавить индустрию': function (e) {
                if (!self.NewIndustryErrors().length == 0) {
                    self.NewIndustry.errors.showAllMessages();
                    return;
                }
                var addIndustryUrl = $('#AddIndustryUrl').val();
                $.ajax({
                    type: 'POST',
                    url: addIndustryUrl,
                    data: {
                        name: self.NewIndustry.Name()
                    },
                    dataType: "json",
                    success: function (res) {
                        if (res.status === "SPCD: OK") {
                            self.Industries.push(new IndustryModel(res.industry, self));
                            self.isAddIndustryDialogVisible(false);
                            self.NewIndustry.Name('');
                        } else {
                            alert("There was an error adding the user: " + res.status);
                        }
                    }
                });
            },
            'Отмена': function () { self.isAddIndustryDialogVisible(false); }
        }
    };

    self.ShowAddIndustryDialog = function () {
        self.isAddIndustryDialogVisible(true);
    };
}

var ProjectsView = {
    Init: function () {
        ko.validation.configure({
            registerExtenders: true,
            messagesOnModified: true,
            insertMessages: true,
            parseInputAttributes: true,
            messageTemplate: null
        });

        var initialProjectsDataObject = $.parseJSON($('#initial-projects-data').val());
        var vm = new ProjectsViewModel(initialProjectsDataObject);

        ko.applyBindings(vm, document.getElementById("projects-management-view"));
    }
};