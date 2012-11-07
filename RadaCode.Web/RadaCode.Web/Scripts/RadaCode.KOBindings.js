$(function() {

    ko.bindingHandlers.timeago = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var $this = $(element);

            // Set the title attribute to the new value = timestamp
            $this.attr('title', value);

            // If timeago has already been applied to this node, don't reapply it -
            // since timeago isn't really flexible (it doesn't provide a public
            // remove() or refresh() method) we need to do everything by ourselves.
            if ($this.data('timeago')) {
                var datetime = $.timeago.datetime($this);
                var distance = (new Date().getTime() - datetime.getTime());
                var inWords = $.timeago.inWords(distance);

                // Update cache and displayed text..
                $this.data('timeago', { 'datetime': datetime });
                $this.text(inWords);
            } else {
                // timeago hasn't been applied to this node -> we do that now!
                $this.timeago();
            }
        }
    };

    ko.bindingHandlers.chosen = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var options = ko.utils.unwrapObservable(valueAccessor()) || {};
            var allBindings = allBindingsAccessor();
            var attrList = { placeholderText: options.PlaceholderText };
            $.extend(attrList, allBindings.chosen);
            if (options.AddPlaceholder) {
                $(element).attr('data-placeholder', attrList.placeholderText).addClass('chzn-select');
            } else {
                $(element).addClass('chzn-select');
            }
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var options = ko.utils.unwrapObservable(valueAccessor()) || {};
            $(element).chosen(options.ChosenOptions);
            $(element).trigger("liszt:updated");
        }
    };
    
    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $(element).datepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "change", function () {
                var observable = valueAccessor();
                observable($(element).datepicker("getDate"));
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).datepicker("destroy");
            });

        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                current = $(element).datepicker("getDate");

            if (value - current !== 0) {
                $(element).datepicker("setDate", value);
            }
        }
    };
    
    ko.bindingHandlers.jQRangePicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor()) || {};
            //do in a setTimeout, so the applyBindings doesn't bind twice from element being copied and moved to bottom
            setTimeout(function () {

                $(element).rangeSlider(options);
                $(element).bind("valuesChanged", function (event, data) {
                    var value = allBindingsAccessor().value;
                    value(data.values.min + '::' + data.values.max);
                });
                
                if(options.prohibitLeftChange) {
                    $(element).bind("valuesChanging", function (event, data) {
                        if (options.leftFix != data.values.min) {
                            $(element).rangeSlider("min", options.leftFix);
                        }
                    });
                }
            }, 0);
        }
    };
    
    ko.bindingHandlers.tagsinput = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor()) || {};
            //do in a setTimeout, so the applyBindings doesn't bind twice from element being copied and moved to bottom
            setTimeout(function () {
                options.onChange = function (obj, tagslist) {
                    var value = allBindingsAccessor().value;
                    var arrayOfTags = obj.val().split(',');
                    value(arrayOfTags);
                };

                $(element).tagsInput(options);
            }, 0);

        }
    };

    ko.bindingHandlers.dialog = {
        init: function(element, valueAccessor, allBindingsAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor()) || { };
            //do in a setTimeout, so the applyBindings doesn't bind twice from element being copied and moved to bottom
            setTimeout(function() {
                options.close = function() {
                    allBindingsAccessor().dialogVisible(false);
                };

                $(element).dialog(options);
            }, 0);

            //handle disposal (not strictly necessary in this scenario)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
                $(element).dialog("destroy");
            });
        },
        update: function(element, valueAccessor, allBindingsAccessor) {
            var shouldBeOpen = ko.utils.unwrapObservable(allBindingsAccessor().dialogVisible);
            $(element).dialog(shouldBeOpen ? "open" : "close");
        }
    };
    
    ko.bindingHandlers.ckeditor = {
        init: function (element, valueAccessor, allBindingsAccessor, context) {
            var options = allBindingsAccessor().ckeditorOptions || {};
            var modelValue = valueAccessor();
            var value = ko.utils.unwrapObservable(valueAccessor());

            $(element).html(value);
            $(element).ckeditor();

            var editor = $(element).ckeditorGet();

            editor.on('blur', function (e) {
                var self = this;
                if (ko.isWriteableObservable(self)) {
                    self($(e.listenerData).val());
                }
            }, modelValue, element);

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                var existingEditor = CKEDITOR.instances[element.name];
                existingEditor.destroy(true);
            });
        }
    };
    
    ko.bindingHandlers.executeOnEnter = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var allBindings = allBindingsAccessor();
            $(element).keypress(function (event) {
                var keyCode = (event.which ? event.which : event.keyCode);
                if (keyCode === 13) {
                    allBindings.executeOnEnter.call(viewModel);
                    return false;
                }
                return true;
            });
        }
    };
    
    ko.bindingHandlers.fadeVisible = {
        init: function (element, valueAccessor) {
            // Initially set the element to be instantly visible/hidden depending on the value
            var value = valueAccessor();
            $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
        },
        update: function (element, valueAccessor) {
            // Whenever the value subsequently changes, slowly fade the element in or out
            var value = valueAccessor();
            ko.utils.unwrapObservable(value) ? $(element).fadeIn() : $(element).fadeOut();
        }
    };
    
    ko.bindingHandlers.stopBinding = {
        init: function () {
            return { controlsDescendantBindings: true };
        }
    };
});