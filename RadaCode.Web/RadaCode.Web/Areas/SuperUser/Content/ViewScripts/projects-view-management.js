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

function ClientModel(data, parent) {
    var self = this;
    
    self.Id = data.Id;
    self.Name = ko.observable(data.Name);
    self.NameChanged = ko.observable(false);
    self.Name.subscribe(function () {
        self.NameChanged(true);
    });

    self.ParentIndustries = ko.computed(function () {
        return parent.Industries();
    });

    self.IndustryId = ko.observable(data.IndustryId);
    self.IndustryIdChanged = ko.observable(false);
    self.IndustryId.subscribe(function () {
        self.IndustryIdChanged(true);
    });
    
    self.Size = ko.observable(data.CustomerCompanySize);
    self.SizeChanged = ko.observable(false);
    self.Size.subscribe(function () {
        self.SizeChanged(true);
    });
    
    self.Ravenue = ko.observable(data.NetRevenue);
    self.RavenueChanged = ko.observable(false);
    self.Ravenue.subscribe(function () {
        self.RavenueChanged(true);
    });
    
    self.WebUrl = ko.observable(data.WebSiteUrl);
    self.WebUrlChanged = ko.observable(false);
    self.WebUrl.subscribe(function () {
        self.WebUrlChanged(true);
    });
    
    self.SaveUpdatedClient = function () {
        var updateClientUrl = $('#UpdateClientUrl').val();
        $.ajax({
            type: 'POST',
            url: updateClientUrl,
            data: {
                id: self.Id,
                name: self.Name(),
                industryId: self.IndustryId(),
                size: self.Size(),
                ravenue: self.Ravenue(),
                webUrl: self.WebUrl()
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    self.NameChanged(false);
                    self.IndustryIdChanged(false);
                    self.SizeChanged(false);
                    self.RavenueChanged(false);
                    self.WebUrlChanged(false);
                } else {
                    alert("There was an error updating a client record: " + res.status);
                }
            }
        });
    };
    
    self.Remove = function () {
        var removeClientUrl = $('#RemoveClientUrl').val();
        $.ajax({
            type: 'POST',
            url: removeClientUrl,
            data: {
                id: self.Id
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    parent.RemoveClient(self);
                } else {
                    alert("There was an error removing a client - " + res);
                }
            }
        });
    };
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
    self.NewIndustry.Name = ko.observable('').extend({ required: true });
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
    
    self.Clients = ko.observableArray([]);
    var clientsModelsArray = jQuery.map(data.Clients, function (val, i) {
        self.Clients.push(new ClientModel(val, self));
    });

    self.RemoveClient = function (clientVM) {
        self.Clients.remove(clientVM);
    };

    self.isAddClientDialogVisible = ko.observable(false);
    self.NewClient = {};
    self.NewClient.Name = ko.observable('').extend({ required: true });
    self.NewClient.IndustryId = ko.observable('').extend({ required: true });
    self.NewClient.Size = ko.observable('').extend({ required: true });
    self.NewClient.Ravenue = ko.observable('').extend({ required: true });
    self.NewClient.WebUrl = ko.observable('').extend({ required: true });

    self.NewClientErrors = ko.validation.group(self.NewClient);

    self.addClientDialogOptions = {
        autoOpen: false,
        modal: true,
        height: 400,
        width: 800,
        title: 'Добавление нового клиента',
        open: function () {
            $('.chzn-select').chosen();
        },
        buttons: {
            'Добавить клиента': function (e) {
                if (!self.NewClientErrors().length == 0) {
                    self.NewClient.errors.showAllMessages();
                    return;
                }
                var addClientUrl = $('#AddClientUrl').val();
                $.ajax({
                    type: 'POST',
                    url: addClientUrl,
                    data: {
                        name: self.NewClient.Name(),
                        industryId: self.NewClient.IndustryId(),
                        size: self.NewClient.Size(),
                        ravenue: self.NewClient.Ravenue(),
                        webUrl: self.NewClient.WebUrl()
                    },
                    dataType: "json",
                    success: function (res) {
                        if (res.status === "SPCD: OK") {
                            self.Clients.push(new ClientModel(res.customer, self));
                            self.isAddClientDialogVisible(false);
                            self.NewClient.Name('');
                            self.NewClient.IndustryId('');
                            self.NewClient.Size('');
                            self.NewClient.Ravenue('');
                            self.NewClient.WebUrl('');
                        } else {
                            alert("There was an error adding the client: " + res.status);
                        }
                    }
                });
            },
            'Отмена': function () { self.isAddClientDialogVisible(false); }
        }
    };

    self.ShowAddClientDialog = function () {
        self.isAddClientDialogVisible(true);
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