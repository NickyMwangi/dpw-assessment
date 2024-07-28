
(function ($) {

    $.fn.selecttable = function (method) {

        var methods = {

            init: function (options) {
                this.selecttable.settings = $.extend({}, this.selecttable.defaults, options);
                return this.each(function () {
                    var $element = $(this), // reference to the jQuery version of the current DOM element
                        element = this;      // reference to the actual DOM element
                    var hgt = '200px';//element.selecttable.settings.height;
                    var wdh = '30%';//element.selecttable.settings.width;
                    var url = $element.data("url");
                    
                    tableActions.loadData($element,url);
                    tableActions.containerSize($element,hgt, wdh);
                });

            },

            // a public method. for demonstration purposes only - remove it!
            loadTable: function (url) {
                
            }

        }

        var tableActions = {
            loadData: function (elem, url) {
                var a = this;
                var tspop = elem.find('.table-select-pop')[0];
                $(tspop).load(url, function () {
                    $('#bt-list').bootstrapTable({
                        classes: 'table table-sm table-hover',
                        theadClasses: 'thead-light',
                        search: true,
                        visibleSearch: true,
                        sortable: true,
                        singleSelect: true,
                        clickToSelect: true,
                    });
                }); 
            },
            containerSize: function (elem, h, w) {
                var ddmenu = elem.find('.dropdown-menu')[0];
                $(ddmenu).attr('max-height', h);
                $(ddmenu).attr('width', w);
            }
        }

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method "' + method + '" does not exist in pluginName plugin!');
        }

    }

    $.fn.selecttable.defaults = {
        width: '30%',
        height: '200px',
        url:""
    }

    $.fn.selecttable.settings = {}

})(jQuery);