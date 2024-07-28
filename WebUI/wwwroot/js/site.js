
(function (window) {
    'use strict';
    var config;
    var isDirty = false;
    window.Common = (function () {
        var swalShow = function (a, b, c) {
            Swal.fire({
                icon: a,
                title: b,
                text: c,
            });
        }

        var swalDelete = function (url, recid) {
            Swal.fire({
                title: "Confirm Delete",
                text: "Are you sure you want to delete this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                allowOutsideClick: false,
                showLoaderOnConfirm: false
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: url,
                        data: { id: recid },
                        beforeSend: function () { blocker() },
                        complete: function () { $.unblockUI() },
                        success: function (data) {
                            if (data.success === true) {
                                window.location.href = data.url;
                            }
                            eval(data.message);
                        }
                    });
                }
            });
        };

        var swalDeleteLine = function (url, recid, form) {
            Swal.fire({
                title: "Confirm Delete",
                text: "Are you sure you want to delete line?",
                icon: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                allowOutsideClick: false,
                showLoaderOnConfirm: true,
                preConfirm: () => {

                }
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: $(form).serialize() + '&lineId=' + recid,
                        beforeSend: function () { blocker() },
                        complete: function () { $.unblockUI() },
                        success: function (data) {
                            if (data.success) {
                                reloadLine(data.dto, data.url);
                            }
                            eval(data.message);
                        }
                    });
                }
            });
        };

        var swalConfirmSubmit = function (form, msg, linkaction, close) {
            Swal.fire({
                title: "Confirm",
                text: msg,
                icon: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                allowOutsideClick: false,
                showLoaderOnConfirm: false
            }).then((result) => {
                if (result.value) {
                    submitModal(form, linkaction, close);
                }
            });
        };

        var swalConfirmAction = function (form, msg, url, report, close, approval) {
            Swal.fire({
                title: "Confirm",
                text: msg,
                icon: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                allowOutsideClick: false,
                showLoaderOnConfirm: false
            }).then((result) => {
                if (result.value) {
                    var targetMod = $(form).closest(".modal");
                    var redirectWindow;
                    blocker();
                    if (report === 'True') {
                        redirectWindow = window.open('', '_blank');
                    }
                    $.ajax({
                        url: url,
                        type: 'GET',
                        complete: function () { $.unblockUI() },
                        success: function (data) {
                            if (data.success) {
                                if (report === 'True' && data.reportUrl !== '') {
                                    redirectWindow.location.href = data.reportUrl;
                                }
                                if (approval === true) {
                                    window.setTimeout(function () {
                                        location.reload();
                                    }, 2000);
                                   
                                }
                                if (close === 'True') {
                                    targetMod.toggle();
                                } else {
                                    reloadModal(form, data.url)
                                }
                            } else if (report === 'True') {
                                redirectWindow.close();
                            }
                            eval(data.message);
                        }
                    });
                }
            });
        };

        var toastrShow = function (a, b) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            toastr[a](b);
        }

        var blockerLink = function () {
            $.ajaxSetup({ cache: false });

            $('*[data-load]').click(function () {
                blocker();
            });

            $('*[data-submit]').click(function () {
                var form = $(this).parents('form:first');
                $.validator.unobtrusive.parse(form);
                if (form.valid()) {
                    blocker();
                } else {
                    toastr['error']('Ensure all fields are filled correctly.');
                    $.unblockUI();
                }
            });
        }

        var blocker = function () {
            $.blockUI({
                message: '<div class="theme-loader"><div class="ball-scale"><div class= "contain">'
                    + '<div class="ring"><div class="frame"></div></div> '
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '<div class="ring"><div class="frame"></div></div>'
                    + '</div></div></div>',
                css: { backgroundColor: 'transparent', height: '50px', border: '0', cursor: 'default' },
                overlayCSS: { cursor: 'default' },
                baseZ: 2000
            });
        }

        var displayModal = function (url) {
            var dialog = $('<div class="modal fade modalIndex" role="dialog"><div class="modal-dialog modal-lg" role="document"><div class="modal-content"></div></div></div>');
            $(dialog).appendTo('#modal-holder');
            $(dialog).on('hidden.bs.modal', function (e) {
                $(this).fadeOut().remove();
            });

            var divTarget = dialog.children(".modal-dialog").first().children(".modal-content").first();
            divTarget.html('');
            divTarget.load(url, function () {
                dialog.modal({
                    keyboard: false,
                    backdrop: 'static'
                })
                var form = dialog.find('form').first();
                if (form.data("error") !== 'none') { eval(form.data("error")); }
                ("")
                //Controls.activate(siteOptions);    
                $.unblockUI();
            });
        }

        var submitModal = function (form, linkAction, close) {
            $.validator.unobtrusive.parse(form);
            if (form.valid()) {
                var targetMod = $(form).closest(".modal");
                var options = {
                    type: 'POST',
                    cache: false,
                    dataType: 'json',
                    data: { linkAction: linkAction },
                    success: function (data) {
                        if (data.success) {
                            isDirty = false;
                            if (close) {
                                targetMod.modal('toggle');
                                $.unblockUI();
                            } else {

                                reloadModal(form, data.url, data.message);
                            }
                        } else {
                            isDirty = false;
                            $.unblockUI();
                            eval(data.message);
                        }
                    },
                    beforeSubmit: function (formData, jqForm, options) {

                        Common.blocker();
                        $.unblockUI();
                    },
                    error: function (e, r, a) {
                        Common.swalShow('error', 'Error Occurred! Unknown Error. Please contact your administrator', 'Alert');
                        $.unblockUI();
                    }
                };

                form.ajaxSubmit(options);
                return false;
            } else {
                Common.toastrShow('error', 'Ensure all fields are filled correctly.')
                $.unblockUI();
                return false;
            }
            $.unblockUI();
        }

        var submitReport = function (form) {
            var redirectWindow;
            var targetMod = $(form).closest(".modal");
            redirectWindow = window.open('', '_blank');
            var options = {
                type: 'POST',
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.success) {
                        redirectWindow.location.href = data.reportUrl;
                    } else {
                        redirectWindow.close();
                        eval(data.message);
                    }
                    $.unblockUI();
                },
                beforeSubmit: function (formData, jqForm, options) {
                    Common.blocker();
                },
                error: function (e, r, a) {
                    redirectWindow.close();
                    Common.swalShow('error', 'Error Occurred! Unknown Error. Please contact your administrator', 'Alert');
                    $.unblockUI();
                }
            };

            form.ajaxSubmit(options);
            return false;
        }

        var reloadModal = function (form, url, msg) {
            var targetDiv = $(form).closest(".modal-content");
            var targetMod = $(form).closest(".modal");
            targetMod.removeData();
            targetDiv.load(url, function () {
                //Controls.activate(siteOptions);
                $.unblockUI();
                if (msg !== null && msg !== "") {
                    eval(msg);
                }
                $.unblockUI();
            })
        }

        var reloadLine = function (formdata, url) {
            $.ajax({
                async: true,
                data: formdata,
                type: "POST",
                url: url,
                success: function (partialView) {
                    $('#divtbline').html(partialView);
                    //Controls.activate(config);
                }
            });
            $.unblockUI();
        }

        var runReport = function (url) {
            var redirectWindow;
            redirectWindow = window.open('', '_blank');
            $.ajax({
                url: url,
                type: 'GET',
                complete: function () { $.unblockUI() },
                success: function (data) {
                    if (data.success) {
                        redirectWindow.location.href = data.reportUrl;
                    } else {
                        redirectWindow.close();
                        eval(data.message);
                    }
                    $.unblockUI();
                }
            });
        }


        return {
            swalShow: swalShow,
            swalDelete: swalDelete,
            swalDeleteLine: swalDeleteLine,
            swalConfirmSubmit: swalConfirmSubmit,
            swalConfirmAction: swalConfirmAction,
            toastrShow: toastrShow,
            blockerLink: blockerLink,
            blocker: blocker,
            displayModal: displayModal,
            submitModal: submitModal,
            submitReport: submitReport,
            reloadModal: reloadModal,
            reloadLine: reloadLine,
            runReport: runReport
        };
    })();

    window.Controls = (function () {

        var activate = function (options) {
            config = options;
            Common.blockerLink();
            bootTable();
            dateTime();
            dataselect();
            cardToggle();
            modalEvents();
            inputPicker();
            inputMask();
            selectTable();
            multiLine();
            multiselect();
            attachDropzone();
        }

        var bootTable = function () {
            var $btable = $('.refresh-index, .modal-index');
            $btable.bootstrapTable({
                classes: 'table table-sm table-hover',
                theadClasses: 'thead-light',
                toggle: 'table',
                sortable: true,
                silentSort: false,
                //serverSort: false,
                search: true,
                visibleSearch: true,
                pagination: true,
                pageList: '[5,10, 25, 50, 100, 200, All]',
                pageSize: '10',
                //sidePagination: 'server',
                showExtendedPagination: true,
                //totalNotFilteredField: 'TotalNotFiltered',
                singleSelect: true,
                //clickToSelect: true,
                loadingFontSize: '14px',
                //queryParams: function (p) {
                //    return queryParams(p);
                //}
            });

            function queryParams(params) {

                var cols = $btable.bootstrapTable('getVisibleColumns').map(function (e) {
                    return e
                });

                var searchcol = '';
                for (var c of cols) {
                    if (!c.checkbox && c.searchable/* && ((this.options.visibleSearch && column.visible) || !this.options.visibleSearch)*/) {
                        searchcol = searchcol + c.field + ',';
                    }
                }
                params.searchable = searchcol;
                return {
                    request: JSON.stringify(params)
                }
            }
        }
        var dateTime = function () {
            $('[data-ctrl="jquerydate"]').datepicker({
                showOn: "button",
                dateFormat: 'dd mmm yyyy',
                buttonText: "<i class='fa icon-calendar'></i>"
            });

            $('[data-ctrl="datepicker"]').datetimepicker({
                format: 'DD MMM YYYY',
                widgetParent: config.parentControl,
                toolbarPlacement: 'bottom',
                showToday: true,
                showClear: true,
                showClose: true
            });
        }

        var dataselect = function () {
            $('[data-ctrl="select2"]').select2({
                dropdownParent: config.parentControl,
                theme: 'bootstrap4',
                allowClear: true
            });

            var firstEmptySelect = true;
            var hdr = "";

            var ajaxdef = {
                ajax: {
                    dataType: 'json',
                    delay: 500,
                    data: function (params) {
                        return {
                            search: params.term, // search term
                            page: params.page
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        hdr = data.header;
                        return {
                            results: data.items,
                            pagination: {
                                more: (params.page * 20) < data.count
                            }
                        };
                    },
                    cache: true
                },
                templateResult: formatResult,
                templateSelection: formatSelect,
                theme: 'bootstrap4',
                allowClear: true
            };

            $('[data-ctrl="ajaxcascade"],[data-ctrl="ajaxselect"]').select2(ajaxdef);

            $('[data-ctrl="ajaxcascade"],[data-ctrl="ajaxselect"],[data-ctrl="select2"]').on("change", function (e) {
                var ctrlName = this.name;
                if ($('[data-target-ctrl ="' + ctrlName + '"]').length) {
                    var val = $(this).val();
                    var options = ajaxdef;
                    $('[data-target-ctrl ="' + ctrlName + '"]').each(function () {
                        var targeturl = $(this).data("target-url");
                        var $ctrl = $(this);
                        $ctrl.val('').trigger('change');
                        $.get(targeturl, { value: val }, function (data) {
                            var ajaxopt = $.extend(true, ajaxdef.ajax, { url: data.url });
                            options = $.extend({}, ajaxdef, { ajax: ajaxopt });
                            $ctrl.select2('destroy');
                            $ctrl.removeAttr("data-ajax--url");
                            $ctrl.select2(options);
                            if (data.readstatus) {
                                $ctrl.attr("readonly", "readonly");
                            } else {
                                $ctrl.removeAttr("readonly");
                            }
                        });
                    });
                }
            });

            function formatResult(result) {

                var $container = "";
                if (result.loading) {
                    firstEmptySelect = true;
                    return result.text;
                }
                if (firstEmptySelect) {
                    firstEmptySelect = false;
                    $container = $(hdr + result.display);
                    return $container;
                }

                $container = $(result.display);
                return $container;
            }

            function formatSelect(repo) {
                return repo.text;
            }
        }

        var cardToggle = function () {
            $(".card-header-right .minimize-card").on('click', function (e) {
                e.stopImmediatePropagation();
                var $this = $(this);
                var port = $($this.parents('.card'));
                $(port).children('.card-block').slideToggle();
                $(this).toggleClass("icon-minus").fadeIn('slow');
                $(this).toggleClass("icon-plus").fadeIn('slow');
            });
            $(".card-header-right .full-card").on('click', function () {
                var $this = $(this);
                var port = $($this.parents('.card'));
                port.toggleClass("full-card");
                $(this).toggleClass("icon-maximize");
                $(this).toggleClass("icon-minimize");
            });
        }

        var selectTable = function () {
            $('[data-ctrl="selecttable"]').selecttable();
        }

        var multiLine = function () {
            $('[data-ctrl="multiline"]').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['para', ['ul', 'ol', 'paragraph']]
                ],
                placeholder: 'Enter Text...',
                height: 100
            });
            $('[data-ctrl="multiline"]').each(function () {
                var readOnly = $(this).data("readonly");
                if (readOnly) {
                    $(this).summernote('disable');
                }
            });
        }

        var inputPicker = function () {
            $('[data-ctrl="inputpicker"]').inputpicker({
                data: [
                    { value: "1", text: "Text 1", description: "This is the description of the text 1." },
                    { value: "2", text: "Text 2", description: "This is the description of the text 2." },
                    { value: "3", text: "Text 3", description: "This is the description of the text 3." }
                ],
                fields: ['text', 'description'],
                fieldValue: 'value',
                fieldText: 'text',
                headShow: true,
                filterOpen: true,
                autoOpen: true
            });
        }

        var multiselect = function () {
            $('[data-ctrl="multiselect"]').multiSelect({
                selectableHeader: "<input type='text' class='search-input form-control' autocomplete='off' placeholder='Search available options...'>",
                selectionHeader: "<input type='text' class='search-input form-control' autocomplete='off' placeholder='Search selected options...'>",
                afterInit: function (ms) {
                    var that = this,
                        $selectableSearch = that.$selectableUl.prev(),
                        $selectionSearch = that.$selectionUl.prev(),
                        selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
                        selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

                    that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
                        .on('keydown', function (e) {
                            if (e.which === 40) {
                                that.$selectableUl.focus();
                                return false;
                            }
                        });

                    that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
                        .on('keydown', function (e) {
                            if (e.which === 40) {
                                that.$selectionUl.focus();
                                return false;
                            }
                        });
                },
                afterSelect: function () {
                    this.qs1.cache();
                    this.qs2.cache();
                },
                afterDeselect: function () {
                    this.qs1.cache();
                    this.qs2.cache();
                }
            });
        }

        var modalEvents = function () {
            $('.show-modal').on('click', function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var url = $(this).attr('href');
                Common.displayModal(url);
            });

            $('[data-form-submit]').on('change', function () {
                isDirty = true;
            });

            $('[data-form-submit ="addEdit"]').submit(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this);
                Common.submitModal(form, 'save', false);
            });

            $('[data-form-submit ="addClose"]').submit(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this);
                Common.submitModal(form, 'save', true);
            });

            $('[data-form-submit ="report"]').submit(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this);
                Common.submitReport(form);
            });

            $('*[data-form-action]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var action = $(this).data('form-action');
                var close = $(this).data('form-close');
                if ($(this).data('form-confirm')) {
                    var msg = $(this).data('form-confirm');
                    Common.swalConfirmSubmit(form, msg, action, close);
                } else {
                    Common.submitModal(form, action, close ?? false);
                }
            });

            $('[data-action =" "]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var action = "saveNew";
                Common.submitModal(form, action, false);
            });

            $('[data-action ="reload"]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var url = $(this).attr('href');
                Common.reloadModal(form, url);
            });

            $('[data-action ="delete"]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var recId = $(this).data('id');
                var url = $(this).attr('href');
                Common.swalDelete(url, recId);
            });

            $('[data-action ="addline"]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var fdata = $(form).serialize()
                var url = $(this).attr('href');
                Common.reloadLine(fdata, url);
            });

            $("[data-parent]").on('hide.bs.modal', function (e) {
                e.stopImmediatePropagation();
                var modalId = $(this).data('parent');
                var form = $(modalId + ' form');
                if (form.length) {
                    var url = $(form)[0].action;
                    Common.reloadModal(form, url);
                }
            });

            $('[data-action ="deleteline"]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var recId = $(this).data('id');
                var url = $(this).attr('href');
                Common.swalDeleteLine(url, recId, form);
            });

            $('[data-action ="pageaction"]').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var close = $(this).data('form-close');
                var report = $(this).data('report');
                var msg = $(this).data('form-confirm');
                var url = $(this).attr('href');
                Common.swalConfirmAction(form, msg, url, report, close, false);
            });
            $('[data-action ="approvalaction"]').click(function (e) {

                e.preventDefault();
                e.stopImmediatePropagation();
                var form = $(this).closest('form');
                var close = $(this).data('form-close');
                var report = $(this).data('report');
                var msg = $(this).data('form-confirm');
                var url = $(this).attr('href');
                Common.swalConfirmAction(form, msg, url, report, close, true);
            });

            $('.report').click(function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var url = $(this).attr('href');
                Common.runReport(url);
            });

            $(function () {
                var $table = $('.refresh-index:first').first();
                $('.modalIndex').on('hide.bs.modal', function (e) {
                    e.stopImmediatePropagation();
                    if ($('.refresh-index').length) {
                        var dataUrl = $table.data("url");
                        $table.bootstrapTable('refreshOptions', {
                            //showColumns: true,
                            search: true,
                            //showRefresh: true,
                            url: dataUrl
                        })
                    }
                });
                $table.on("post-body.bs.table", function () {
                    Common.blockerLink()
                })

                var $tbpartial = $('.refresh-partial:first').first();
                $('.modalChild').on('hide.bs.modal', function (e) {
                    e.stopImmediatePropagation();
                    if ($('.refresh-partial').length) {
                        var dataUrl = $tbpartial.data("url");
                        $tbpartial.bootstrapTable('refreshOptions', {
                            //showColumns: true,
                            search: true,
                            //showRefresh: true,
                            url: dataUrl
                        })
                    }
                });
            });

            $('.modalIndex').on('hide.bs.modal', function (e) {
                if (isDirty) {
                    isDirty = false; // remove this line to force save and prevent close until save
                    $('#promptText').collapse('show');
                    e.preventDefault();
                }
            });

            //$('[data-form-submit]').on('submit', function () {
            //    isDirty = false;
            //    $('.modalIndex').modal('close');
            //});
        }

        var inputMask = function () {
            $('[data-mask ="decimal"]').inputmask({
                'alias': 'decimal',
                digits: 2,
                digitsOptional: false,
                rightAlign: true,
                'groupSeparator': '.',
                'autoGroup': true,
                placeholder: '0'
            });
        }

        var attachDropzone = function () {
            $(".attach-dropzone").dropzone({
                url: config.dropzoneUploadUrl,
                //prevents Dropzone from uploading dropped files immediately
                autoProcessQueue: true,
                addRemoveLinks: true,
                createImageThumbnails: true,
                previewsContainer: "#dz-preview",
                init: function () {
                    var self = this;
                    var recId = this.element.getAttribute('data-id');
                    $.get(config.getAttachmentsUrl, { tempId: $('input[name="tempFileId"]').val(), id: recId }, function (data) {
                        $.each(data, function (index, file) {

                            var existingFile = { name: file.name, size: file.size, isAttached: file.isAttached, };

                            self.emit("addedfile", existingFile);
                            ////TODO: Not sure about all this thumbnail business, need a custom template instead, and proably just use a stock image.
                            if (!file.type.match(/image.*/)) {
                                // This is not an image, so Dropzone doesn't create a thumbnail.
                                // Set a default thumbnail:
                                self.emit("thumbnail", existingFile, config.defaultThumbnailUrl);
                            } else {
                                self.emit("thumbnail", existingFile, file.url);
                            }
                        });
                        //wait to register the custom event here, otherwise the above emit ends up calling this and things go sideways
                        self.on("addedfile", function (file) {
                            if (!file.type.match(/image.*/)) {
                                self.emit("thumbnail", file, config.defaultThumbnailUrl);
                            }
                            else {
                                self.emit("thumbnail", file, file.url);
                            }
                        });
                    });

                },
                sending: function (file, xhr, formData) {
                    var tmpId = $('input[name="tempFileId"]').val();
                    formData.append("tempId", tmpId);
                },
                removedfile: function (file) {
                    if (file.isAttached) {
                        //file is attached to a saved ticket, collect files to delete in hidden input instead of deleting from server
                        var elem = $('input[name="deletedFiles"]');
                        var val = elem.val() ? elem.val().split(',') : [];
                        val.push(file.name);
                        elem.val(val.join(','));
                        killPreview();
                    } else {
                        //file is pending for either a new or existing ticket, delete from server immediately
                        $.ajax({
                            type: 'POST',
                            url: config.deleteFileUrl,
                            data: {
                                "id": $('input[name="tempFileId"]').val(),
                                "fileName": file.name
                            },
                            success: killPreview,
                            dataType: 'json'
                        });
                    }
                    function killPreview() {
                        var ref;
                        return (ref = file.previewElement) !== null ? ref.parentNode.removeChild(file.previewElement) : void 0;
                    }
                }
            });
        }

        return {
            activate: activate,
            bootTable: bootTable,
            dateTime: dateTime,
            dataselect: dataselect,
            cardToggle: cardToggle,
            inputPicker: inputPicker,
            modalEvents: modalEvents,
            inputMask: inputMask,
            selectTable: selectTable,
            multiLine: multiLine,
            multiselect: multiselect,
            attachDropzone: attachDropzone
        };
    })();

})(window);


