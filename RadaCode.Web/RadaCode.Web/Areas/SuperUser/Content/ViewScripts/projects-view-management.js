function IndustryModel(data, parent) {
    var self = this;

    self.Id = data.Id;
    self.Name = ko.observable(data.Name);
    self.NameChanged = ko.observable(false);
    self.Name.subscribe(function () {
        self.NameChanged(true);
    });
    
    self.Name_En = ko.observable(data.Name_En);
    self.Name_EnChanged = ko.observable(false);
    self.Name_En.subscribe(function () {
        self.Name_EnChanged(true);
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
                newName: self.Name(),
                newName_en: self.Name_En()
            },
            success: function(res) {
                if (res.status === "SPCD: OK") {
                    self.NameChanged(false);
                    self.Name_EnChanged(false);
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
    
    self.Name_En = ko.observable(data.Name_En);
    self.Name_EnChanged = ko.observable(false);
    self.Name_En.subscribe(function () {
        self.Name_EnChanged(true);
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
                name_en: self.Name_En(),
                industryId: self.IndustryId(),
                size: self.Size(),
                ravenue: self.Ravenue(),
                webUrl: self.WebUrl()
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    self.NameChanged(false);
                    self.Name_EnChanged(false);
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
    
    self.LabelFormatter = function (value) {
        return parent.LabelFormatter(value);
    };

    self.WeeksInterpreter = function (values) {
        return parent.WeeksInterpreter(values);
    };

    self.Id = data.Id;
    self.Name = ko.observable(data.Name).extend({ required: true });;
    self.NameChanged = ko.observable(false);
    self.Name.subscribe(function () {
        self.NameChanged(true);
    });
    
    self.Name_En = ko.observable(data.Name_En);
    self.Name_EnChanged = ko.observable(false);
    self.Name_En.subscribe(function () {
        self.Name_EnChanged(true);
    });

    self.ParentClients = ko.computed(function () {
        return parent.Clients();
    });
    
    self.ParentProjectTypes = ko.computed(function () {
        return parent.ProjectTypes();
    });

    self.ClientId = ko.observable(data.ClientId).extend({ required: true });;
    self.ClientIdChanged = ko.observable(false);
    self.ClientId.subscribe(function () {
        self.ClientIdChanged(true);
    });
    
    self.CurrentUsersCount = ko.observable(data.CurrentUsersCount);
    self.CurrentUsersCountChanged = ko.observable(false);
    self.CurrentUsersCount.subscribe(function () {
        self.CurrentUsersCountChanged(true);
    });
    
    self.DateStarted = ko.observable(data.DateStarted);
    self.DateStartedChanged = ko.observable(false);
    self.DateStarted.subscribe(function () {
        self.DateStartedChanged(true);
    });

    self.DateFinished = ko.observable(data.DateFinished);
    self.DateFinishedChanged = ko.observable(false);
    self.DateFinished.subscribe(function () {
        self.DateFinishedChanged(true);
    });
    
    self.Description = ko.observable(data.Description).extend({ required: true });;
    self.DescriptionChanged = ko.observable(false);
    self.Description.subscribe(function () {
        self.DescriptionChanged(true);
    });
    
    self.Description_En = ko.observable(data.Description_En).extend({ required: true });;
    self.Description_EnChanged = ko.observable(false);
    self.Description_En.subscribe(function () {
        self.Description_EnChanged(true);
    });
    
    self.IsCloudConnected = ko.observable(data.IsCloudConnected).extend({ required: true });;
    self.IsCloudConnectedChanged = ko.observable(false);
    self.IsCloudConnected.subscribe(function () {
        self.IsCloudConnectedChanged(true);
    });
    
    self.ProjectActualCompletionSpan = ko.observable(data.ProjectActualCompletionSpan);
    self.ProjectActualCompletionSpanChanged = ko.observable(false);
    self.ProjectActualCompletionSpan.subscribe(function () {
        self.ProjectActualCompletionSpanChanged(true);
    });
    
    self.ProjectEstimate = ko.observable(data.ProjectEstimate);
    self.ProjectEstimateChanged = ko.observable(false);
    self.ProjectEstimate.subscribe(function () {
        self.ProjectEstimateChanged(true);
    });
    
    self.ProjectDescriptionMarkup = ko.observable(data.ProjectDescriptionMarkup).extend({ required: true });;
    self.ProjectDescriptionMarkupChanged = ko.observable(false);
    self.ProjectDescriptionMarkup.subscribe(function () {
        self.ProjectDescriptionMarkupChanged(true);
    });
    
    self.ProjectDescriptionMarkup_En = ko.observable(data.ProjectDescriptionMarkup_En).extend({ required: true });;
    self.ProjectDescriptionMarkup_EnChanged = ko.observable(false);
    self.ProjectDescriptionMarkup_En.subscribe(function () {
        self.ProjectDescriptionMarkup_EnChanged(true);
    });
    
    self.ROIpercentage = ko.observable(data.ROIpercentage);
    self.ROIpercentageChanged = ko.observable(false);
    self.ROIpercentage.subscribe(function () {
        self.ROIpercentageChanged(true);
    });
    
    self.SpecialFeatures = ko.observableArray(data.SpecialFeatures);
    self.SpecialFeaturesChanged = ko.observable(false);
    self.SpecialFeatures.subscribe(function () {
        self.SpecialFeaturesChanged(true);
    });
    
    self.SpecialFeatures_En = ko.observableArray(data.SpecialFeatures_En);
    self.SpecialFeatures_EnChanged = ko.observable(false);
    self.SpecialFeatures_En.subscribe(function () {
        self.SpecialFeatures_EnChanged(true);
    });
    
    self.TechnologiesUsed = ko.observableArray(data.TechnologiesUsed);
    self.TechnologiesUsedChanged = ko.observable(false);
    self.TechnologiesUsed.subscribe(function () {
        self.TechnologiesUsedChanged(true);
    });
    
    self.WebSiteUrl = ko.observable(data.WebSiteUrl);
    self.WebSiteUrlChanged = ko.observable(false);
    self.WebSiteUrl.subscribe(function () {
        self.WebSiteUrlChanged(true);
    });
    
    self.PlatformsSupported = ko.observableArray(data.PlatformsSupported);
    self.PlatformsSupportedChanged = ko.observable(false);
    self.PlatformsSupported.subscribe(function () {
        self.PlatformsSupportedChanged(true);
    });
    
    self.Type = ko.observable(data.Type);
    self.TypeChanged = ko.observable(false);
    self.Type.subscribe(function () {
        self.TypeChanged(true);
    });
    
    self.ProjectModelErrors = ko.validation.group(self);

    self.isEditProjectDialogVisible = ko.observable(false);

    self.markupEditorId = ko.computed(function() {
        return 'inputMarkupEditor-' + self.Id;
    });
    
    self.markupEditorId_En = ko.computed(function () {
        return 'inputMarkupEditor-En-' + self.Id;
    });

    self.editProjectDialogOptions = {
        autoOpen: false,
        modal: true,
        height: 1400,
        width: 1100,
        title: 'Редактирование проекта: ' + self.Name(),
        open: function () {
            //$('.chzn-select').chosen();
            $('.jqSlider').rangeSlider("resize");
        },
        buttons: {
            'Сохранить изменения в проекте': function (e) {
                if (!self.ProjectModelErrors().length == 0) {
                    self.errors.showAllMessages();
                    return;
                }
                self.SaveUpdatedProject();
            },
            'Отмена': function () { self.isEditProjectDialogVisible(false); }
        }
    };

    self.OpenDetailsDialog = function () {
        self.isEditProjectDialogVisible(true);
    };
    
    self.SaveUpdatedProject = function () {
        var updateProjectUrl = $('#UpdateProjectUrl').val();
        $.ajax({
            type: 'POST',
            url: updateProjectUrl,
            data: {
                id: self.Id,
                type: self.Type(),
                name: self.Name(),
                name_en: self.Name_En(),
                description: self.Description(),
                description_en: self.Description_En(),
                customerId: self.ClientId(),
                technologiesUsed: JSON.stringify(self.TechnologiesUsed()),
                dateStarted: self.DateStarted(),
                dateFinished: self.DateFinished(),
                estimate: self.ProjectEstimate(),
                usersCount: self.CurrentUsersCount(),
                roi: self.ROIpercentage(),
                specialFeatures: JSON.stringify(self.SpecialFeatures()),
                specialFeatures_en: JSON.stringify(self.SpecialFeatures_En()),
                isCloudConnected: self.IsCloudConnected(),
                markup: self.ProjectDescriptionMarkup(),
                markup_en: self.ProjectDescriptionMarkup_En(),
                webUrl: self.WebSiteUrl(),
                platformsSupported: JSON.stringify(self.PlatformsSupported())
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    self.isEditProjectDialogVisible(false);
                    self.DescriptionChanged(false);
                } else {
                    alert("There was an error updating a project record: " + res.status);
                }
            }
        });
    };

    self.Remove = function () {
        var removeProjectUrl = $('#RemoveProjectUrl').val();
        $.ajax({
            type: 'POST',
            url: removeProjectUrl,
            data: {
                id: self.Id
            },
            success: function (res) {
                if (res.status === "SPCD: OK") {
                    parent.RemoveProject(self);
                } else {
                    alert("There was an error removing a project - " + res);
                }
            }
        });
    };
    
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
    self.NewIndustry.Name_En = ko.observable('').extend({ required: true });
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
                        name: self.NewIndustry.Name(),
                        name_en: self.NewIndustry.Name_En()
                    },
                    dataType: "json",
                    success: function (res) {
                        if (res.status === "SPCD: OK") {
                            self.Industries.push(new IndustryModel(res.industry, self));
                            self.isAddIndustryDialogVisible(false);
                            self.NewIndustry.Name('');
                            self.NewIndustry.Name_En('');
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
    self.NewClient.Name_En = ko.observable('').extend({ required: true });
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
                        name_en: self.NewClient.Name_En(),
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
                            self.NewClient.Name_En('');
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
    
    self.ProjectTypes = ko.observableArray([]);
    var projectTypeModelsArray = jQuery.map(data.Types, function (val, i) {
        self.ProjectTypes.push(val);
    });

    self.Projects = ko.observableArray([]);
    var projectModelsArray = jQuery.map(data.Projects, function (val, i) {
        self.Projects.push(new ProjectModel(val, self));
    });

    self.RemoveProject = function (projectVM) {
        self.Projects.remove(projectVM);
    };
    

    self.isAddProjectDialogVisible = ko.observable(false);
    self.NewProject = {};
    self.NewProject.Type = ko.observable('').extend({ required: true });
    self.NewProject.Name = ko.observable('').extend({ required: true });
    self.NewProject.Name_En = ko.observable('').extend({ required: true });
    self.NewProject.Description = ko.observable('').extend({ required: true });
    self.NewProject.Description_En = ko.observable('').extend({ required: true });
    self.NewProject.CustomerId = ko.observable('').extend({ required: true });
    self.NewProject.TechnologiesUsed = ko.observableArray([]);
    self.NewProject.DateStarted = ko.observable('');
    self.NewProject.DateFinished = ko.observable('');
    self.NewProject.Estimate = ko.observable('');
    self.NewProject.UsersCount = ko.observable('');
    self.NewProject.Roi = ko.observable('');
    self.NewProject.SpecialFeatures = ko.observableArray([]);
    self.NewProject.SpecialFeatures_En = ko.observableArray([]);
    self.NewProject.IsCloudConnected = ko.observable('').extend({ required: true });
    self.NewProject.Markup = ko.observable('').extend({ required: true });
    self.NewProject.Markup_En = ko.observable('').extend({ required: true });
    self.NewProject.WebUrl = ko.observable('');
    self.NewProject.PlatformsSupported = ko.observableArray([]);

    self.LabelFormatter = function(value) {
        if (value == 0) return '';
        return parseInt(value) + ' (недель)';
    };

    self.NewProjectErrors = ko.validation.group(self.NewProject);
    self.WeeksInterpreter = function(values) {
        return parseInt(values.max - values.min);
    };

    self.addProjectDialogOptions = {
        autoOpen: false,
        modal: true,
        height: 1400,
        width: 1100,
        title: 'Добавление нового проекта',
        open: function () {
            //$('.chzn-select').chosen();
            $('.jqSlider').rangeSlider("resize");
        },
        buttons: {
            'Добавить проект': function (e) {
                if (!self.NewProjectErrors().length == 0) {
                    self.NewProject.errors.showAllMessages();
                    return;
                }
                var addProjectUrl = $('#AddProjectUrl').val();
                $.ajax({
                    type: 'POST',
                    url: addProjectUrl,
                    data: {
                        type: self.NewProject.Type(),
                        name: self.NewProject.Name(),
                        name_en: self.NewProject.Name_En(),
                        description: self.NewProject.Description(),
                        description_en: self.NewProject.Description_En(),
                        customerId: self.NewProject.CustomerId(),
                        technologiesUsed: JSON.stringify(self.NewProject.TechnologiesUsed()),
                        dateStarted: self.NewProject.DateStarted(),
                        dateFinished: self.NewProject.DateFinished(),
                        estimate: self.NewProject.Estimate(),
                        usersCount: self.NewProject.UsersCount(),
                        roi: self.NewProject.Roi(),
                        specialFeatures: JSON.stringify(self.NewProject.SpecialFeatures()),
                        specialFeatures_en: JSON.stringify(self.NewProject.SpecialFeatures_En()),
                        isCloudConnected: self.NewProject.IsCloudConnected(),
                        markup: self.NewProject.Markup(),
                        markup_en: self.NewProject.Markup_En(),
                        webUrl: self.NewProject.WebUrl(),
                        platformsSupported: JSON.stringify(self.NewProject.PlatformsSupported())
                    },
                    dataType: "json",
                    success: function (res) {
                        if (res.status === "SPCD: OK") {
                            self.Projects.push(new ProjectModel(res.project, self));
                            self.isAddProjectDialogVisible(false);
                            self.NewProject.Type('');
                            self.NewProject.Name('');
                            self.NewProject.Name_En('');
                            self.NewProject.Description('');
                            self.NewProject.Description_En('');
                            self.NewProject.CustomerId('');
                            self.NewProject.TechnologiesUsed([]);
                            self.NewProject.DateStarted('');
                            self.NewProject.DateFinished('');
                            self.NewProject.Estimate('');
                            self.NewProject.UsersCount('');
                            self.NewProject.Roi('');
                            self.NewProject.SpecialFeatures([]);
                            self.NewProject.SpecialFeatures_En([]);
                            self.NewProject.IsCloudConnected('');
                            self.NewProject.Markup('');
                            self.NewProject.Markup_En('');
                            self.NewProject.WebUrl('');
                            self.NewProject.PlatformsSupported([]);
                        } else {
                            alert("There was an error adding a project: " + res.status);
                        }
                    }
                });
            },
            'Отмена': function () { self.isAddProjectDialogVisible(false); }
        }
    };

    self.ShowAddProjectDialog = function () {
        self.isAddProjectDialogVisible(true);
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