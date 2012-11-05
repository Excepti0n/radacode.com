function IndustryModel(data, parent) {
    var self = this;

    self.Id = data.Id;
    self.Name = ko.observable(data.Name);
    
    self.Remove = function () {
        var removeIndustryUrl = $('#RemoveIndustryUrl').val();
        $.ajax({
            type: 'POST',
            url: removeIndustryUrl,
            data: {
                id: self.Id
            },
            success: function (res) {
                if (res === "SPCD: OK") {
                    parent.RemoveUser(self);
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
                name: self.Name()
            },
            success: function(res) {
                if (res.status === "SPCD: OK") {

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