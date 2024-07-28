//remove page loader
"use strict"; $(document).ready(function () {
    var el = document.getElementsByClassName('theme-loader');
    el[0].setAttribute("style", "opacity:0.6;");
    $('.theme-loader').animate({ 'opacity': '0', }, 1200);
    setTimeout(function () { $('.theme-loader').hide(); }, 1500);
});

//activate button loaders
$(function () {
    Common.blockerLink();
});

//disable autocomplete
$(document).ready(function () {
    $("input[type='text'],input[type='email'],input[type='password']").each(function () {
        $(this).attr("autocomplete", "off");
    });
});

function validEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}