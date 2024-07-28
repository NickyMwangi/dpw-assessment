$(function () {
    Controls.activate(siteOptions);
});
'use strict';
$(document).ready(function () {
    $(document).on('show.bs.modal', '.modal', function (event) {
        var dialogs = $('.modal:visible');
        $(this).css('z-index', 1050);
        $($(this).find(">:first-child")).css('max-width', '90%');
        var dialogcnt = 1;
        if (dialogs.length) {
            dialogcnt += dialogs.length;
            var zIndex = 1050 + (10 * $('.modal:visible').length);
            var highestzindex = 0;
            var resultelement = null;
            dialogs.each(function () {
                var currentzindex = parseInt($(this).css("zIndex"), 10);
                if (currentzindex > highestzindex) {
                    highestzindex = currentzindex;
                    resultelement = $(this);
                }
            });
            var currWidth = (resultelement.find('>:first-child').css("max-width"))
            var newWidth = (currWidth.match(/\d+/) - 5) + '%';
            $(this).css('z-index', zIndex);
            $($(this).find(">:first-child")).css('max-width', newWidth);
            $(this).data("parent", $(resultelement).attr("id"));
            $(this).removeClass('modalIndex');
            $(this).addClass('modalChild');
        }
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);

        $(this).attr("id", "modal" + dialogcnt);
    });
    $(document).on('shown.bs.modal', '.modal', function (event) {
        var parentModal = $(this);
        var controlOptions = {
            dropzoneUploadUrl: '@Url.Action("Upload", "File", new { area = "" })',
            getAttachmentsUrl: '@Url.Action("GetAttachmentsInfo", "File", new { area = "" })',
            deleteFileUrl: '@Url.Action("Delete", "File", new { area = "" })',
            defaultThumbnailUrl: '@Url.Content("~/images/general-file.png")',
            parentControl: parentModal
        };
        Controls.activate(controlOptions);
        $.unblockUI();
    });
    $('.page-body').show();

})





