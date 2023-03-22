$(function () {
    $("#mainnav a[href='" + window.location.pathname.split('?')[0] + "']").each(function () {
        $(this).addClass("active");
        $(this).parent().parent().show();
        $(this).parent().parent().prev().addClass("expanded");
    });
    $("#mainnav a[href='" + window.location.pathname.split('?')[0] + "/Index']").each(function () {
        $(this).addClass("active");
        $(this).parent().parent().show();
        $(this).parent().parent().prev().addClass("expanded");
    });
    function tooglemenu() {
        var expandmenu = $.cookie("expandmenu");
        if (expandmenu == undefined) {
            expandmenu = "expanded";
        }
        if (expandmenu === "collapsed") {
            $(".leftpanel").width(45);
            $(".hidebutton i").removeClass();
            $(".hidebutton i").addClass("fa fa-chevron-right");
            $(".leftpanel .children").hide();
        } else {
            $(".leftpanel").width(250);
            $(".hidebutton i").removeClass();
            $(".hidebutton i").addClass("fa fa-chevron-left");
        }
        $("#mainnav").removeClass();
        $("#mainnav").addClass(expandmenu);
    }
    tooglemenu();
    $(".hidebutton a").click(function () {
        $.cookie("expandmenu", $.cookie("expandmenu") === "expanded" ? "collapsed" : "expanded");
        tooglemenu();
    });
    $("#mainnav > li").click(function () {
        $.cookie("expandmenu", "expanded");
        tooglemenu();
    });
    $(".login-content").height($(window).height() - $(".header").height());
    $("#mainnav > li > a").click(function () {
        $("#mainnav > li > ul").hide("fast");
        $(this).toggleClass("expanded");
        $(this).next().toggle("slow");
    });
});

function showmodal(e) {
    return showmodalurl($(e).attr('href'));
}

function showmodalurl(url) {
    $("#myModal").modal({
        backdrop: "static",
        keyboard: false
    }).modal("show");
    $("#myModal .close").click(function () {
        closemodal();
    });
    $(".modal-body").html("<iframe id='modalForm' style='width:100%;height:100%;border:none' src='" + url + "'></iframe>");
    return false;
}

function showcalendarmodal(url) {
    $("#myModal").modal({
        backdrop: "static",
        keyboard: false
    }).modal("show");
    $("#myModal .close").click(function () {
        $("#myModal").modal("hide");
        $('#calendar').fullCalendar('refetchEvents');
    });
    $(".modal-body").html("<iframe id='modalForm' style='width:100%;height:100%;border:none' src='" + url + "'></iframe>");
    return false;
}

function closemodal() {
    $("#myModal").modal("hide");
    window.location.href = window.location.href;
}

function closemodal(reload) {
    $("#myModal").modal("hide");
    if (reload) {
        window.location.href = window.location.href;
    }
}

function setmodalsize(width, height) {
    $(".modal-dialog").css("width", width);
    $(".modal-dialog").css("height", height);
    $(".modal-dialog").css("max-width", "none");
}

function closeloading() {
    $("#poploading").remove();
}