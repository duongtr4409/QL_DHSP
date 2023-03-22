/*----------------------------------------------
--Author: nxduong
--Phone: 0983029603
--Description: Thư viện các hàm dùng chung cho các ứng dụng
--Date of created: 13/09/2016
--Input:
--Output:
--Note: 
--Updated by:
--Date of updated: 
----------------------------------------------*/
function app() { };
app.prototype = {
    userId: null,
    userName: null,
    fullName: null,
    langId: null,
    appCode: null,
    root: '',
    storageFolder: '',
    strungdung_id: '',
    pageIndex_default: 1,
    pageSize_default: 10,
    DataRow: 0,
    keyName: {
        codeName: 'cmdCode',
        langId: 'strNgonNguId',
        userId: 'strUserId'
    },
    EnumImageType: {
        NEWS: '1',
        PRODUCT: '2',
        ACCOUNT: 'Avatar/',
        SUPPORT: '4',
        PROJECT: '5',
        PARTNER: '6',
        INTRODUCE: '7',
        MEMBER: '8',
        SERVICE: '9',
        DOCUMENT: 'Files/',
        STAFF: '11',
        USER: '12',
        TEMPLATE: '13',
        COMMON: '14',
        TEMP: '15',
        SETTING: '16',
        CROWD: '17',
        STAR: '18',
        VIDEO: '19',
        ADV: '20'
    },
    DanhMuc: {
        TINHTRANGHONNHAN: 'QLCB.TTHN',
        COCAUTOCHUC: 'QLCB.CCTC',
        LOAICANBO: 'QLCB.LOCB',
        LOAIHOCVI: 'QLCB.DMHV',
        LOAICHUCDANH: 'QLCB.CHDA',
        NHOMMAU: 'QLCB.NHMA',
        GIOITINH: 'CHUN.GITI',
        TONGIAO: 'QLCB.DMTG',
        DANTOC: 'QLCB.DMDT'
    },
    isDebug: false,
    apiUrlCommom: null,
    apiUrl: null,
    apiUrlTemp: null,
    urlService: '',
    isValidate: false,

    startApp: function () {
        var me = this;
        me.apiUrl = $('#txtUrlApi').val();
        me.apiUrlTemp = $('#txtUrlApi').val();
        me.apiUrlCommom = $('#txtUrlApiCommon').val();
        me.root = $("#txtRoot").val();
        me.userId = $("#txtNguoiDungId").val();
        me.storageFolder = $('#txtStorageFolder').val();
        me.strungdung_id = $("#txtUngDungId").val();
        me.langId = $('#txtNgonNgu_Id').val();
        /*Clear input search functions*/
        $("#navbar-search-input").val("");
        /* Ẩn toàn bộ tooltip khi ẩn popup */
        $('.modal').on('hide.bs.modal', function () {
            me.isValidate == false;
            $('[data-toggle="tooltip"], .tooltip').tooltip("hide");
        })

        $('.modal').on('show.bs.modal', function () {
            me.isValidate == false;
            $('[data-toggle="tooltip"], .tooltip').tooltip("hide");
        })

        $('.modal').on('hidden.bs.modal', function () {
            me.isValidate == false;
            $('[data-toggle="tooltip"], .tooltip').tooltip("hide");
        })

        $("#btnChageMyPass").click(function (e) {
            $("#notif-pass").hide();
            $("#txt_oldpass").val('');
            $("#txt_newpass1").val('');
            $("#txt_newpass2").val('');
            $('#myModal_change_pass').modal('show');
            document.getElementById('txt_oldpass').focus();
        });

        $("#btnSave_Change_Pass").click(function (e) {
            me.changePassword();
        });
        $("#btnQuayLai").click(function (e) {
            me.goBack();
        });
        $("#btnProfile").click(function (e) {
            me.alert("Thông báo", "Chức năng chưa cập nhật!");
        });
        /* Cấu hình scroll menu */
        // Highlight the top nav as scrolling occurs
        $('body').scrollspy({
            target: '.navbar-fixed-top',
            offset: 51
        })

        // Closes the Responsive Menu on Menu Item Click
        $('.navbar-collapse ul li a').click(function () {
            $('.navbar-toggle:visible').click();
        });

        // Offset for Main Navigation
        $('#mainNav').affix({
            offset: {
                top: 100
            }
        })

        // Closes the sidebar menu
        $("#menu-close").click(function (e) {
            e.preventDefault();
            $("#sidebar-wrapper").toggleClass("active");
        });

        // Opens the sidebar menu
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#sidebar-wrapper").toggleClass("active");
        });

        /* Cấu hình combobox */
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
        /* End Cấu hình combobox */

        /* Cấu hình validate */
        //set schema manually - using only jQuery
        var form = $("#formMain");
        //define the data entry
        var dataentry = new $.Forms.DataEntry({
            $el: form,
            //defines the validation, formatting and constraints schema
            /*Cách dùng:YEAR, only-letters, required là tên đặt tùy ý, validation:["required"] là thiết lập thuộc tính không được để rỗng trường input...*/
            schema: {
                required: {
                    validation: ["required"],
                    format: ["cleanSpaces"]
                },
                yearRequired: {
                    validation: ["required", { name: "integer", params: [{ min: 1900, max: 2900 }] }]
                },
                monthRequired: {
                    validation: ["required", { name: "integer", params: [{ min: 1, max: 12 }] }]
                },
                year: {
                    validation: ["digits", { name: "integer", params: [{ min: 1900, max: 2900 }] }]
                },
                day: {
                    validation: ["digits", { name: "integer", params: [{ min: 1, max: 31 }] }]
                },
                "only-letters": {
                    validation: ["letters"]
                },
                "policy-read": {
                    validation: ["mustCheck"]
                },
                "integer": {
                    validation: ["digits"]
                },
                "integerequired": {
                    validation: ["required", { name: "integer" }]
                },
                "phone": {
                    validation: ["digits"]
                }
            }
        });
        //trigger focus on the first input field
        form.find(":input:first").trigger("focus");

        form.find(".submit").on("click", function (e) {
            //validate
            dataentry.validate()
            .done(function (data) {
                //do something with data :)
                me.isValidate = true;
            })
            .fail(function (data) {
                //the form contains error; the errors are displayed internally by the dataentry
                me.isValidate = false;
            });
        });
        /* End Cấu hình validate */
        /*Start Cấu hình curency*/
        $('[data-ax5formatter]').ax5formatter();
        /*End Cấu hình curency*/
        /*Start xử lý hộp tìm kiếm chức năng*/
        $("#searchBox").mouseleave(function () {
            me.hideSeachBox();
        });
        /*End xử lý hộp tìm kiếm chức năng*/
        $(".select-opt").select2();
        //Khởi tạo giao diện phân trang
        me.pagInfoRender();
        me.pagInfoRender_basic();

        me.common_page_setup_datepicker();
    },
    makeRequest: function (params, inc_user, inc_code, inc_lang, container, urlService) {
        var op = params;
        var me = this;
        var onSuccess = function () { };

        if (op.hasOwnProperty('success')) {
            if (typeof (op.success) == 'function') {
                onSuccess = op.success;
            }
        }

        var onError = function () { };
        if (op.hasOwnProperty('error')) {
            if (typeof (op.error) == 'function') {
                onError = op.error;
            }
        }
        var is_inc_code = false;
        var is_inc_lang = false;
        var is_inc_user = false;

        if (inc_code != null) is_inc_code = inc_code;
        if (is_inc_lang != null) is_inc_lang = inc_lang;
        if (is_inc_user != null) is_inc_user = inc_user;
        if (container != null) {
            $('[id$=' + container + ']').html('<div class="IMSLoadingWrapper"><img src="' + me.root + '/App_Themes/CMS/images/twitter_loading.gif" alt="loading..." /></div>');
        }
        if (urlService != null) {
            me.apiUrl = urlService;
        }
        else {
            me.apiUrl = me.apiUrlTemp;
        }
        me.apiUrl = me.apiUrl + op.action;

        var dataPost = op.data;
        if (me.isDebug) {
            console.log(dataPost);
        }
        if (me.isDebug) {
            if (typeof (op.fakedb) != 'undefined')
                onSuccess(op.fakedb);
        } else {
            var request = $.ajax({
                type: op.type,
                url: me.apiUrl,
                data: dataPost,
                cache: false,
                contentType: Portal.ModuleSetting.method.CONTENT_TYPE,
                dataType: Portal.ModuleSetting.method.DATA_TYPE,
                success: function (d, s, x) {
                    var result = d;
                    if (result != null) {
                        onSuccess(result);
                    }
                },
                error: function (x, t, m) {
                    onError(x);
                },
                async: op.async,
                timeout: op.timeout != undefined ? op.timeout : 3000000
            });
        }
    },
    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    stopLoading: function (container) {
        if (container != null) {
            $('[id$=' + container + ']').html('');
        }
    },
    randomString: function (len, charSet) {
        charSet = charSet || 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var randomString = '';
        for (var i = 0; i < len; i++) {
            var randomPoz = Math.floor(Math.random() * charSet.length);
            randomString += charSet.substring(randomPoz, randomPoz + 1);
        }
        return randomString;
    },
    changePassword: function () {
        var me = this;
        var OldPass = $('#txt_oldpass').val();
        var NewPass1 = $('#txt_newpass1').val();
        var NewPass2 = $('#txt_newpass2').val();

        if (OldPass.length <= 0) {
            $("#txt_oldpass").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-info' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập vào mật khẩu cũ!</p></div>");
            document.getElementById('txt_oldpass').focus();
            return false;
        }
        else {
            $("#txt_oldpass").removeAttr("data-toggle");
            $("#txt_oldpass").removeAttr("data-original-title");
            $("#txt_oldpass").removeAttr("title");
        }

        if (NewPass1.length <= 0) {
            $("#txt_newpass1").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-warning' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập lại mật khẩu mới!</p></div>");
            document.getElementById('txt_newpass1').focus();
            return false;
        }
        else {
            $("#txt_newpass1").removeAttr("data-toggle");
            $("#txt_newpass1").removeAttr("data-original-title");
            $("#txt_newpass1").removeAttr("title");
        }

        if (NewPass2.length <= 0) {
            $("#txt_newpass2").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-warning' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập lại mật khẩu mới!</p></div>");
            document.getElementById('txt_newpass2').focus();
            return false;
        }
        else {
            $("#txt_newpass2").removeAttr("data-toggle");
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").removeAttr("title");
        }
        if (NewPass1 != NewPass2) {
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").attr("data-toggle", "tooltip");
            $("#txt_newpass2").attr("data-original-title", "");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-info' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Mật khẩu mới không trùng nhau!</p></div>");
            document.getElementById('txt_newpass2').focus();
            return false;
        }
        else {
            $("#txt_newpass2").removeAttr("data-toggle");
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").removeAttr("title");
        }
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    $("#notif-pass").show();
                    $("#notif-pass").html("<div class='alert alert-success' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Thay đổi mật khẩu thành công!</p></div>");
                    $("#txt_oldpass").attr("title", data.Message);
                    document.getElementById('txt_oldpass').focus();
                }
                else {
                    $("#notif-pass").show();
                    $("#notif-pass").html("<div class='alert alert-danger' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Mật khẩu cũ không đúng, vui lòng nhập lại!</p></div>");
                    $("#txt_oldpass").attr("title", data.Message);
                    document.getElementById('txt_oldpass').focus();
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'HeThong/chageMyPassword',
            data: {
                'old': OldPass,
                'pass1': NewPass1,
                'pass2': NewPass2,
                'userId': me.userId
            },
            fakedb: [
            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    getServerTime: function ()//get datetime on server
    {
        try {
            //FF, Opera, Safari, Chrome
            xmlHttp = new XMLHttpRequest();
        }
        catch (err1) {
            //IE
            try {
                xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
            }
            catch (err2) {
                try {
                    xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                }
                catch (eerr3) {
                    //AJAX not supported, use CPU time.
                    alert("AJAX not supported");
                }
            }
        }
        xmlHttp.open('HEAD', window.location.href.toString(), false);
        xmlHttp.setRequestHeader("Content-Type", "text/html");
        xmlHttp.send('');
        return xmlHttp.getResponseHeader("Date");
    },
    dateValid: function (dmy) {
        /*------------------------------
        --Input: format date: dd/mm/yyyy 
        --Output: true/false
        -------------------------------*/
        var me = this;
        var pattern = /(\d+)(-|\/)(\d+)(?:-|\/)(?:(\d+)\s+(\d+):(\d+)(?::(\d+))?(?:\.(\d+))?)?/;//[dd/mm/yyyy || d/m/yyyy|| dd/m/yyyy||d/mm/yyyy] is ok
        if (pattern.test(dmy)) {//check format date
            var date = "";
            var str_mm_dd_yyyy = "";
            var arrdate = [];
            arrdate = dmy.split("/");
            str_mm_dd_yyyy = me.dateConvertDMYtoMDY(dmy);
            date = new Date(str_mm_dd_yyyy);
            if (date.getFullYear() == arrdate[2] && (date.getMonth() + 1) == arrdate[1] && date.getDate() == Number(arrdate[0])) {//check valid date
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    },
    dateConvertDMYtoMDY: function (dmy) {
        /*------------------------------
        --Input: format date: dd/mm/yyyy
        --Output: mm/dd/yyyy
        -------------------------------*/
        var str_mm_dd_yyyy = "";
        arr_date = dmy.split("/");
        str_mm_dd_yyyy = arr_date[1] + "/" + arr_date[0] + "/" + arr_date[2];
        return str_mm_dd_yyyy;

    },
    checkTime: function (i) {
        if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
        return i;
    },
    dateInRange: function (dmy, dmy_start, dmy_end) {
        var me = this;
        if (dmy == "")
        {
            var today = new Date(me.getServerTime());
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            if (mm == 0) {
                mm = 1;
            }
            dd = me.checkTime(dd);
            mm = me.checkTime(mm);
            var yyyy = today.getFullYear();
            dmy = dd.toString() + "/" + mm.toString() + "/" + yyyy.toString();
        }
        
        if (me.dateValid(dmy_start) && me.dateValid(dmy_end) && me.dateValid(dmy)) {
            var arrDate = dmy.split("/");
            var arrDate_start = dmy_start.split("/");
            var arrDate_end = dmy_end.split("/");

            var valDate = new Date(arrDate[2], arrDate[1] - 1, arrDate[0]);
            var valDate_start = new Date(arrDate_start[2], arrDate_start[1] - 1, arrDate_start[0]);
            var valDate_end = new Date(arrDate_end[2], arrDate_end[1] - 1, arrDate_end[0]);

            if (valDate >= valDate_start && valDate <= valDate_end) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    },
    beginLoading: function () {
        document.getElementById('overlay').style.display = "";
    },
    endLoading: function () {
        document.getElementById('overlay').style.display = "none";
    },
    alert: function (title, content) {
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog"><div class="modal-dialog">';
        render += '<div class="modal-content"><div class="modal-header">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += ' <p>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    confirm: function (title, content) {
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog"><div class="modal-dialog">';
        render += '<div class="modal-content"><div class="modal-header">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += '<p id=tbldataKemTheo>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" id="btnNo" data-dismiss="modal">Đóng</button>';
        render += '<button type="button" class="btn btn-primary" id="btnYes">Đồng ý</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    alertOnModal: function (title, content, colorText, prePos) {
        $("#btnNotifyModal").remove();
        var htm = "<div id='btnNotifyModal' class='alert " + colorText + " alert-dismissible' style='text-align:center;'>" +
                    "<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                    "<strong><span style='font-size:15px;'>" + title + ": " + content + "</span></strong>" +
                "</div>";
        $(prePos).append(htm);
        $("#notify").slideDown("slow");
        /*colorCode: - 'alert-danger' - 'alert-dismissible' - 'alert-info' - 'alert-link' - 'alert-success'- 'alert-warning' */
    },
    loadDataToComboBox: function (strMaBangDanhMuc, zone_Id, strTitle) {
        var me = this;
        eduroot.app.beginLoading();
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    var tbCombo = $('[id$=' + zone_Id + ']');
                    tbCombo.html('');
                    if (mlen > 0) {
                        var i;
                        var getList = "";
                        if (strTitle == "" || strTitle == null || strTitle == undefined) {
                            strTitle = "-- Chọn dữ liệu --";
                        }
                        else {
                            strTitle = "-- " + strTitle + " --";
                        }
                        getList += "<option value=''>" + strTitle + "</option>";
                        for (i = 0; i < mlen; i++) {
                            getList += "<option value='" + json[i].ID + "'>" + json[i].TEN + "</option>";
                        }
                        tbCombo.html(getList);
                    }
                    tbCombo.val('').trigger("change");
                }
                else {
                    console.log(data.Message);
                }
                eduroot.app.endLoading();
            },
            error: function (er) { eduroot.app.endLoading(); },
            type: 'GET',
            action: 'DuLieuDanhMuc/LoadDataToComboBox',
            data: {
                'strMaBangDanhMuc': strMaBangDanhMuc
            },
            fakedb: [

            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    getFileNameUpload: function (strGiaTri, strKyTu) {
        var ketqua = "";
        if (strKyTu == ".") {
            ketqua = strGiaTri.substr(strGiaTri.lastIndexOf(strKyTu) - 36, strGiaTri.length - 1);
        }
        else {
            ketqua = strGiaTri.substr(strGiaTri.lastIndexOf(strKyTu) + 1, strGiaTri.length - 1);
        }

        return ketqua;
    },
    getImageUrl: function (ImageName, imageType) {
        var me = this;
        if (ImageName == "" || ImageName == null || ImageName == undefined) {
            return me.root + "/Upload/Avatar/no-avatar.png";
        }
        else {
            switch (imageType) {
                case me.EnumImageType.ACCOUNT:
                    return me.root + "/Upload/Avatar/" + ImageName;
                    break;
                case me.EnumImageType.DOCUMENT:
                    return me.root + "/Upload/Files/" + ImageName;
                    break;
                default:
                    return me.root + "/Upload/Avatar/no-avatar.png";
            }
        }
    },
    convertStringToNumber: function (strGiaTri, strDau) {
        var ketqua = "";
        if (strDau == "," || strDau == null || strDau == undefined) {
            ketqua = strGiaTri.replace(/,/g, "");
        }
        else {
            ketqua = strGiaTri.replace(/./g, "");
        }
        return ketqua
    },
    htmlEscape: function (strGiaTri) {
        return strGiaTri.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "").replace(/</g, "").replace(/>/g, "").replace(/&/g, "");
    },
    validate_email: function (field) {
        var testresults;
        var str = field;
        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
        if (filter.test(str)) {
            testresults = true
        }
        else {
            testresults = false
        }
        return testresults
    },
    //Thương mới thêm hàm này - 14/05/2017
    validate_string: function (name_id) {
        var value = $(name_id).val().trim();
        if (value) return true;
        else return false;
    },
    //Thương mới thêm hàm này - 14/05/2017
    validate_integer: function (name_id) {
        var value = $(name_id).val().trim();
        if (value.isInteger()) return true;
        else return false;
    },
    convertEmptyToNumber: function (strGiaTri) {
        if (strGiaTri == "" || strGiaTri == null || strGiaTri == undefined) {
            return 0
        }
        else {
            var ketqua = strGiaTri.replace(',', ".");
            return ketqua;
        }

    },
    formatCurrency: function (number) {
        return number.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    },
    getYear: function (year, DropControlName, lblName) {//year: nam bat dau can lay.vd 1930, 1990... den nam hien tai
        var me = this;
        var today = new Date(me.getServerTime);
        var nowYear = today.getFullYear();
        var dropControl = "#" + DropControlName;
        if (lblName == "" || lblName == null || lblName == undefined) {
            lblName = "-- Chọn dữ liệu --";
        }
        else {
            lblName = "-- " + lblName + " --";
        }
        $(dropControl).append($('<option value=""></option>').html(lblName));
        for (i = new Date().getFullYear() ; i > year; i--) {
            $(dropControl).append($('<option />').val(i).html(i));
        }
        $(".chosen-select").select();
        $(dropControl).trigger("change");
    },
    goBack: function () {
        window.history.back();
    },
    /*Start xử lý hộp tìm kiếm chức năng*/
    functionSearch: function (value) {
        var me = this;
        var strTuKhoa = "";
        strTuKhoa = $("#navbar-search-input").val();
        /*Display a Div search*/
        if (strTuKhoa == "" || strTuKhoa == null || strTuKhoa == undefined) {
            $("#search-result").hide();
            return false;
        }
        else {
            $("#search-result").show();
        }
        /*Display result, response data to Div search from database*/

        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    $('#tblSearch').dataTable({
                        "aaData": json,
                        "destroy": true,
                        "bPaginate": false,
                        "bLengthChange": false,
                        "bFilter": false,
                        "processing": false,
                        "bSort": false,
                        "bInfo": false,
                        "bAutoWidth": false,
                        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "Tất cả"]],
                        "language": {
                            "search": "Tìm kiếm theo từ khóa:",
                            "lengthMenu": "Hiển thị _MENU_ dữ liệu",
                            "zeroRecords": "Không có dữ liệu nào được tìm thấy!",
                            "info": "Hiển thị _START_ đến _END_ trong _TOTAL_ dữ liệu",
                            "infoEmpty": "Hiển thị 0 đến 0 của 0 dữ liệu ",
                            "infoFiltered": "(Dữ liệu tìm kiếm trong _MAX_ dữ liệu)",
                            "processing": "Đang tải dữ liệu...",
                            "emptyTable": "Không có dữ liệu nào được tìm thấy!",
                            "paginate": {
                                "first": "Đầu",
                                "last": "Cuối",
                                "next": "Tiếp theo",
                                "previous": "Quay lại"
                            }
                        },
                        "aoColumnDefs": [
                        ],
                        "order": [[1, "asc"]],
                        "aoColumns": [{
                            "mDataProp": "ID",
                            "bVisible": false
                        }
                        , {
                            "mDataProp": "TENCHUCNANG"
                        }
                        ],
                        "fnRowCallback": function (nRow, aData) {
                            var $nRow = $(nRow); // cache the row wrapped up in jQuery
                            if (aData.TENANH == "" || aData.TENANH == null || aData.TENANH == undefined) {
                                $('td:eq(0)', nRow).html('<a href="' + me.root + "/Index.aspx" + '">' + '<i class="fa fa-bullseye"></i> ' + aData.TENCHUCNANG + '</a>');
                            }
                            else {
                                $('td:eq(0)', nRow).html('<a href="' + me.root + "/" + aData.DUONGDANHIENTHI + '">' + '<i class="' + aData.TENANH + '"></i> ' + aData.TENCHUCNANG + '</a>');
                            }
                            return nRow
                        },
                    });
                }
                else {
                    console.log(data.Message);
                }
                eduroot.app.endLoading();
            },
            error: function (er) { eduroot.app.endLoading(); },
            type: 'GET',
            action: 'ChucNangUngDung/LayDanhSachChucNang',
            data: {
                'strTuKhoa': strTuKhoa,
                'strUngDung_Id': me.strungdung_id
            },
            fakedb: [

            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    hideSeachBox: function () {
        var me = this;
        $("#navbar-search-input").val("");
        me.functionSearch("");
    },
    /*End xử lý hộp tìm kiếm chức năng*/
    returnempty: function (strGiaTri) {
        if (strGiaTri == "" || strGiaTri == null || strGiaTri == undefined)
            return ""
        else
            return strGiaTri
    },
    //Ký tự hoa đầu tiên
    capitalize: function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1).toLowerCase();
    },
    GuiEmailXacNhan: function (ids) {
        var me = this;
        var strNoiDung = "";

        var strTieuDe = "";

        var strChuKy = "";

        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    location.href = me.root + "/Logout.aspx";
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'ThongTinLyLich/GuiEmailKichHoat',
            data: {
                'strIds': ids,
                'strNoiDung': strNoiDung,
                'strTieuDe': strTieuDe,
                'strChuKy': strChuKy
            },
            fakedb: [
            ]
        }, false, false, false, null);
    },
    LoiHinhDaiDien: function (id) {
        var me = this;
        id = "#" + id;
        var url = me.getImageUrl("", me.EnumImageType.ACCOUNT);
        $(id).attr("src", url);
    },
    XetMacDinhTheoMaDanhMuc: function (strMaBangDanhMuc, zone_Id, strMaDuLieu) {
        var me = this;
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    var tbCombo = $('[id$=' + zone_Id + ']');
                    var id = json[0].ID;
                    tbCombo.val(id).trigger("change");
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'DuLieuDanhMuc/XetMacDinhTheoMaDanhMuc',
            data: {
                'strMaBangDanhMuc': strMaBangDanhMuc,
                'strMaDuLieu': strMaDuLieu
            },
            fakedb: [

            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    LoadEditor: function (editorName) {
        var me = this;
        if (typeof (CKEDITOR) != "undefined") {
            var configCK = {
                customConfig: '',
                height: '320px',
                width: '100%',
                language: 'vi',
                entities: false,
                fullPage: false,
                toolbarCanCollapse: false,
                resize_enabled: false,
                startupOutlineBlocks: true,
                colorButton_enableMore: false,
                toolbar:
                [
                    { name: 'document', items: ['Source'] },
                    { name: 'tools', items: ['Maximize'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'TextColor', 'BGColor'] },
                    { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'] },
                    { name: 'insert', items: ['HorizontalRule', 'SpecialChar'] },
                    { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                ],
                filebrowserImageUploadUrl: me.root + '/App_Themes/Plugins/Upload.ashx',
                filebrowserBrowseUrl: me.root + '/App_Themes/Plugins/ckfinder/ckfinder.html'
            };
            var editor = CKEDITOR.replace(editorName, configCK);
            CKEDITOR.on('instanceReady', function (e) {
                CKFinder.setupCKEditor(editor, me.root + '/App_Themes/Plugins/ckfinder/');
            });
        }
    },
    /*
    Xữ lý phân trang V=1.0.0.0.0
    1. Hàm render html số trang tùy chọn và thông tin dữ liệu hiển thị
    2. Hàm setup thông tin phân trang và xữ lý sự kiện next page (strFuntionName: Tên khai báo hàm trong file js,
    strTable_Id: Id bảng dữ liệu)
    Cách gọi như sau:
    1. Tại hàm init gọi hàm pagInfoRender để render html (hoặc gọi trong app)
    2. Tại hàm load dữ liệu sau khi trả về kết quả json thì gọi hàm pagButtonRender
    */
    pagInfoRender: function (strClassName) {
        var me = this;
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "tbl-pagination";

        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strTable_Id = x[i].id;
            var strPageSize_Id = "Change" + strTable_Id;
            var class_pull_left = '';
            class_pull_left += '<div style="margin-top:10px" class="pull-left" id="' + strPageSize_Id + '" style="padding-left:0px; font-style:itali"></div>';
            $("#" + strTable_Id).parent().prev().append(class_pull_left);

            var row_change = "";
            row_change += '<div style="padding-left:0 !important; margin-top:6px; float:left; font-style:italic">';
            row_change += '<label>Hiển thị</label>';
            row_change += '</div>';
            row_change += '<div style="width: 70px; padding-left:3px !important; float:left">';
            row_change += '<select id="DropPageSize' + strPageSize_Id + '" class="select-opt">';
            row_change += '<option value="5"> 5 </option>';
            row_change += '<option value="15"> 15 </option>';
            row_change += '<option value="25"> 25 </option>';
            row_change += '<option value="50"> 50 </option>';
            row_change += '<option value="-1"> Tất cả </option>';
            row_change += '</select>';
            row_change += '</div>';
            row_change += '<div style="padding-left:3px !important; margin-top:6px; float:left; font-style:italic">';
            row_change += '<label>dữ liệu</label>';
            row_change += '</div>';
            $("#" + strPageSize_Id).html(row_change);
            $(".select-opt").select2();

            var row_info = '';
            row_info += '<div style="width:100%; float:right">';
            row_info += '<div id="tbldata_info' + strTable_Id + '" style="width: 40%; float:left; margin-top:5px"></div>';
            row_info += '<input type="hidden" id="hr_total_rows' + strTable_Id + '" value="0" />';
            row_info += '<div id="light-pagination' + strTable_Id + '" style="float:left; width: 60%"></div>';
            row_info += '</div>';
            $("#" + strTable_Id).after(row_info);

            row_inputhiden = '';
            row_inputhiden += '<input type="hidden" value="10" id="' + strTable_Id + '_DataRow" />';
            $("#" + strTable_Id).after(row_inputhiden);
        }
    },
    pagInfoRender_basic: function (strClassName) {
        var me = this;
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "tbl-paginationbasic";

        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strTable_Id = x[i].id;

            var strPageSize_Id = "Change" + strTable_Id;
            var class_pull_left = '';
            class_pull_left += '<div class="pull-left col-sm-6" id="' + strPageSize_Id + '" style="padding-left:0px; font-style:itali"></div>';
            $("#" + strTable_Id).before(class_pull_left);

            var strFilter_Id = strTable_Id + '_filter';
            var class_pull_right = '';
            class_pull_right += '<div id="' + strFilter_Id + '" class="col-sm-6 dataTables_filter"></div>';
            $("#" + strTable_Id).before(class_pull_right);

            var row_change = "";
            row_change += '<div style="padding-left:0 !important; margin-top:6px; float:left; font-style:italic">';
            row_change += '<label>Hiển thị</label>';
            row_change += '</div>';
            row_change += '<div style="width: 70px; padding-left:3px !important; float:left">';
            row_change += '<select id="DropPageSize' + strPageSize_Id + '" class="select-opt">';
            row_change += '<option value="10"> 10 </option>';
            row_change += '<option value="15"> 15 </option>';
            row_change += '<option value="25"> 25 </option>';
            row_change += '<option value="50"> 50 </option>';
            row_change += '<option value="-1"> Tất cả </option>';
            row_change += '</select>';
            row_change += '</div>';
            row_change += '<div style="padding-left:3px !important; margin-top:6px; float:left; font-style:italic">';
            row_change += '<label>dữ liệu</label>';
            row_change += '</div>';
            $("#" + strPageSize_Id).html(row_change);
            $(".select-opt").select2();

            var row_filter = '';
            row_filter += '<div class="pull-right">';
            row_filter += '<label>';
            row_filter += 'Tìm kiếm theo từ khóa:';
            row_filter += '<input id="' + strTable_Id + '_input" class="form-control ui-touched" style="width: 250px">';
            row_filter += '</label>';
            row_filter += '</div>';
            $("#" + strFilter_Id).html(row_filter);

            var row_info = '';
            row_info += '<div style="width:100%; float:right">';
            row_info += '<div id="tbldata_info' + strTable_Id + '" style="width: 40%; float:left; margin-top: 5px"></div>';
            row_info += '<input type="hidden" id="hr_total_rows' + strTable_Id + '" value="0" />';
            row_info += '<div id="light-pagination' + strTable_Id + '" style="float:left; width: 60%"></div>';
            row_info += '</div>';
            $("#" + strTable_Id).after(row_info);

            row_inputhiden = '';
            row_inputhiden += '<input type="hidden" value="10" id="' + strTable_Id + '_DataRow" />';
            $("#" + strTable_Id).after(row_inputhiden);
        }
    },
    pagButtonRender: function (strFuntionName, strTable_Id, iDataRow) {
        var me = this;
        var pageIndex = me.pageIndex_default;
        var pageSize = me.pageSize_default;
        $("#hr_total_rows" + strTable_Id).val(iDataRow);
        var eDataRow = $("#" + strTable_Id + "_DataRow").val();
        eDataRow = parseInt(eDataRow);
        if (eDataRow != parseInt(iDataRow) || pageIndex == 1) {
            pageIndex = 1;
            me.pagInit(strFuntionName, strTable_Id, pageSize);
        }
        $("#" + strTable_Id + "_DataRow").val(iDataRow);
        if (parseInt(iDataRow) > 0) {
            var first_item = 1;
            if (pageIndex != 1) {
                first_item = (pageSize * pageIndex) - pageSize + 1;
            }
            if (pageSize == 1000000) {
                first_item = 1;
            }
            var items_in = "";
            if (parseInt(iDataRow) < parseInt(pageSize)) {
                items_in = iDataRow.toString();
            }
            else {
                items_in = (pageSize * pageIndex).toString();
            }
            if (parseInt(iDataRow) < parseInt(items_in)) {
                items_in = iDataRow.toString();
            }
            $("#tbldata_info" + strTable_Id).html("<span style='font-style:italic'>Hiển thị " + first_item + " đến " + items_in + " trong " + iDataRow + " dữ liệu<span>");
        }
        else {
            me.pagInit(strFuntionName, strTable_Id, pageSize);
            $("#tbldata_info" + strTable_Id).html("<span style='font-style:italic'>Hiển thị 0 đến 0 trong 0 dữ liệu</span>");
        }
    },
    pagInit: function (strFuntionName, strTable_Id, pageSize_default) {
        var me = this;
        var totalRows = $('[id$="hr_total_rows' + strTable_Id + '"]').attr('value');
        $('#light-pagination' + strTable_Id).pagination({
            items: totalRows,
            itemsOnPage: pageSize_default,
            cssStyle: 'compact-theme',
            onPageClick: function (pageNumber, event) {
                me.pageIndex_default = pageNumber;
                eval(strFuntionName);
            }
        });

        $('#DropPageSizeChange' + strTable_Id).on('select2:select', function (e) {
            e.stopImmediatePropagation();
            me.pageIndex_default = 1;
            me.pageSize_default = $("#DropPageSizeChange" + strTable_Id).val();
            if (me.pageSize_default == "-1" || me.pageSize_default == -1) {
                me.pageSize_default = 1000000;
            }
            eval(strFuntionName);
            return false;
        });
        if ($("#" + strTable_Id).hasClass("tbl-paginationbasic")) {
            $("#" + strTable_Id + "_input").keypress(function (e) {
                e.stopImmediatePropagation();
                if (e.which == 13) {
                    eval(strFuntionName);
                }
            });
        }
    },
    /*
    Dữ liệu nhà khoa học
    */
    loadToCombo_DanhSachNhaKhoaHoc: function (zone_Id, strTitle) {
        var me = this;
        var strTuKhoa = "";
        var pageIndex = 1;
        var pageSize = 10000;
        var iTrangThai = 1;
        var iTinhTrang = 1;
        var loaitimkiem = "";
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    if (mlen > 0) {
                        var i;
                        var getList = "";
                        if (strTitle == "" || strTitle == null || strTitle == undefined) {
                            strTitle = "-- Chọn nhà khoa học --";
                        }
                        else {
                            strTitle = "-- " + strTitle + " --";
                        }
                        getList += "<option value=''>" + strTitle + "</option>";
                        for (i = 0; i < mlen; i++) {
                            getList += "<option title='" + json[i].COQUANCONGTAC + "' value='" + json[i].ID + "'>" + json[i].LOAICHUCDANH + json[i].HOVATEN + " - " + json[i].COQUANCONGTAC + "</option>";
                        }
                        for (var j = 0; j < zone_Id.length; j++) {
                            var tbCombo = $('[id$=' + zone_Id[j] + ']');
                            tbCombo.html('');
                            tbCombo.html(getList);
                            tbCombo.val('').trigger("change");
                        }
                    }

                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'LyLichNhaKhoaHoc/LayDanhSachLLKH_RutGon',
            data: {
                'strTuKhoa': strTuKhoa,
                'pageIndex': pageIndex,
                'pageSize': pageSize,
                'iTrangThai': iTrangThai,
                'iTinhTrang': iTinhTrang,
                'loaitimkiem': loaitimkiem,
                'iChiLayDSCoSua': -1
            },
            fakedb: [

            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    loadTable_DanhSachNhaKhoaHoc: function () {
        var me = this;
        var strTuKhoa = "";
        var pageIndex = 1;
        var pageSize = 10000;
        var iTrangThai = 1;
        var iTinhTrang = 1;
        var loaitimkiem = "";
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    if (mlen > 0) {
                        //01
                        $('#tbldataTimKiemNguoiHD').dataTable({
                            "aaData": json,
                            "destroy": true,
                            "bPaginate": true,
                            "bLengthChange": false,
                            "bFilter": true,
                            "processing": true,
                            "bSort": false,
                            "bInfo": true,
                            "bAutoWidth": false,
                            "lengthMenu": [[8, 25, 50, 100, -1], [8, 25, 50, 100, "Tất cả"]],
                            "language": {
                                "search": "Tìm kiếm theo từ khóa:",
                                "lengthMenu": "Hiển thị _MENU_ dữ liệu",
                                "zeroRecords": "Không có dữ liệu nào được tìm thấy!",
                                "info": "Hiển thị _START_ đến _END_ trong _TOTAL_ dữ liệu",
                                "infoEmpty": "Hiển thị 0 đến 0 của 0 dữ liệu ",
                                "infoFiltered": "(Dữ liệu tìm kiếm trong _MAX_ dữ liệu)",
                                "processing": "Đang tải dữ liệu...",
                                "emptyTable": "Không có dữ liệu nào được tìm thấy!",
                                "paginate": {
                                    "first": "Đầu",
                                    "last": "Cuối",
                                    "next": "Tiếp theo",
                                    "previous": "Quay lại"
                                }
                            },
                            "aoColumnDefs": [
                                {
                                    'bSortable': false,
                                    'aTargets': [6]
                                }
                            ],
                            "order": [[1, "asc"]],
                            "aoColumns": [{
                                "mDataProp": "ID",
                                "bVisible": false
                            }
                            ,
                            {
                                "mDataProp": "THUTU"
                            }
                            ,
                            {
                                "mDataProp": "HOVATEN"
                            }
                            ,
                            {
                                "mDataProp": "SODIENTHOAIDIDONG"
                            }
                            ,
                            {
                                "mDataProp": "DIACHIEMAIL"
                            }
                            ,
                            {
                                "mDataProp": "COQUANCONGTAC"
                            }
                            ,
                            {
                                "mData": "ID",
                                "mRender": function (data) {
                                    return '<div class="form-group" style="width:200px; text-align:left">' +
                                                '<select id="DropVaiTroThamGia' + data + '" data-placeholder="Chọn vai trò..." class="select-opt" style="width:100%">' +
                                                    '<option value="1">Người hướng dẫn 1</option>' +
                                                    '<option value="2">Người hướng dẫn 2</option>' +
                                                    '<option value="0">Người hướng dẫn độc lập</option>' +
                                                '</select>' +
                                            '</div>'
                                },
                            }
                            , {
                                "mData": "ID",
                                "mRender": function (data) {
                                    return '<input type=\"checkbox\" id="chkSelectNguoiHD' + data + '" name="chkSelectNguoiHD' + data + '">';
                                },
                            }
                            ],
                            "fnRowCallback": function (nRow, aData) {
                                var $nRow = $(nRow);
                                var strChucDanh = aData.MACHUCDANH;
                                var strHocVi = aData.MAHOCVI;
                                if (strChucDanh == null || strChucDanh == "null")
                                {
                                    strChucDanh = "";
                                }
                                if (strHocVi == null || strHocVi == "null") {
                                    strHocVi = "";
                                }
                                var strHoTen = strChucDanh + strHocVi + " " + aData.HOVATEN;
                                $('td:eq(1)', nRow).html(strHoTen);
                                return nRow
                            }
                        });
                        $(".select-opt").select2();
                    }

                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'LyLichNhaKhoaHoc/LayDanhSachLLKH_RutGon',
            data: {
                'strTuKhoa': strTuKhoa,
                'pageIndex': pageIndex,
                'pageSize': pageSize,
                'iTrangThai': iTrangThai,
                'iTinhTrang': iTinhTrang,
                'loaitimkiem': loaitimkiem,
                'iChiLayDSCoSua': -1
            },
            fakedb: [

            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    common_page_setup_datepicker: function (strClassName) {
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "input-datepicker";
        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strInput_Id = x[i].id;
            var cleave = new Cleave('#' + strInput_Id, {
                date: true,
                datePattern: ['d', 'm', 'Y']
            });
            $('#' + strInput_Id).datepicker({
                autoclose: true,
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true
            });
        }
    },
    CapNhatThongTinDuLieu: function (strTenBang, strTenCot, strDuLieu, strId) {
        var me = this;
        strTenCot = "h1_camnhancuancskhiratruong";
        eduroot.app.makeRequest({
            success: function (data) {
                if (data.Success) {
                    console.log("Success");
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'POST',
            action: 'HeThong/CapNhatTheoTungTruongThongTinV2',
            data: JSON.stringify({
                'strTenBang': strTenBang,
                'strDuLieu': strDuLieu,
                'strTenCot': strTenCot,
                'strId': strId
            }),
            fakedb: [
            ]
        }, false, false, false, null, me.apiUrlCommom);
    },
    getvalue_null: function (strGiaTri) {
        if (strGiaTri == null || strGiaTri == "null" || strGiaTri == undefined) {
            strGiaTri = "-";
        }
        return strGiaTri;
    }
};