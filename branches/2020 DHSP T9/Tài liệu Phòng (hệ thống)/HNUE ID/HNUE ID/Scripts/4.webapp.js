$(function () {
    $("input[code=number], input.number").keypress(function (e) {
        if (e.keyCode === 8 | e.keyCode === 13 | e.keyCode === 9 | e.keyCode === 116) return true;
        if (isNaN(this.value + "" + String.fromCharCode(e.charCode))) return false;
    }).on("cut copy paste", function (e) {
        e.preventDefault();
    });
    $(".datepicker").datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        formatDate: 'd-m-Y'
    });
    $(".timepicker").datetimepicker({
        datepicker: false,
        format: 'H:i',
        step: 5
    });
    $("select.checkoption option").each(function () {
        if ($(this).attr("value") === "-1") $(this).attr("disabled", "disabled");
    });
    $("table.table:not('.no-hover')").on('click', 'tr', function () {
        $("table.table tr.active").removeClass("active");
        $(this).addClass("active");
    });
    $("input[code=number], input.number").keypress(function (e) {
        if (e.keyCode === 8 | e.keyCode === 13 | e.keyCode === 9 | e.keyCode === 116) return true;
        if (isNaN(this.value + "" + String.fromCharCode(e.charCode))) return false;
    }).on("cut copy paste", function (e) {
        e.preventDefault();
    });
    //$('a[data-toggle="tab"]').on('click', function (e) {
    //    $.cookie("activeTab", $(e.target).attr('href'));
    //});
    //if ($.cookie("activeTab")) {
    //    $('a[href="' + $.cookie("activeTab") + '"]').tab('show');
    //}
    $(".css-random-color").each(function() {
        var colorR = Math.floor((Math.random() * 256));
        var colorG = Math.floor((Math.random() * 130));
        var colorB = Math.floor((Math.random() * 256));
        $(this).css("color", "rgb(" + colorR + "," + colorG + "," + colorB + ")");
    });
});
$.fn.digits = function (fix) {
    this.each(function () {
        if (fix < 0) fix = 0;
        $(this).html(parseFloat($(this).html()).toFixed(fix).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    });
}
function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}
function acronym(str) {
    var arr = str.split(" ");
    var r = "";
    $.each(arr, function(index, i) {
        r += i[0].toUpperCase();
    });
    return r;
}