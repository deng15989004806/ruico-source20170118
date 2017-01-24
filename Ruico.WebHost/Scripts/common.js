function confirm_modal_by_url(url, title, content) {
    confirm_modal_by_url(url, title, content, "GET");
}

function confirm_modal_by_url(url, title, content, requestType) {
    $('#confirm_modal').modal();
    $('#confirm_title').html(title);
    $('#confirm_content').html(content);
    $('#confirm_confirm').one('click', function () {
        $.ajax({
            type: (requestType != null && requestType != '') ? requestType : 'GET',
            url: url,
            dataType: "json",
            success: ajaxRequestSuccess
        });
        $('#confirm_modal').modal('hide');
        return false;
    });
    $('#confirm_cancel').one('click', function () {
        $('#confirm_confirm').unbind('click');
    });
}

function popup_modal(url, title, width, height) {
    if ($("#hide-modal-body").html() == "") {
        // 用于每次都从显示空白页开始
        $("#hide-modal-body").html($('#popup_modal .modal-body').html());
    }
    $("#popup_modal .modal-body").html($('#hide-modal-body').html());
    $('#popup_modal').css("width", width + "px");
    $('#popup_modal .modal-body').css("height", height + "px");
    $('#popup_title').html(title);
    $("#popup_frame").html("");
    $("#popup_frame").attr("src", url);
    $('#popup_modal').modal();
}

$(document).ready(function () {
    $('.ajax-remove').click(function () {
        var url = $(this).attr('href');
        var name = $(this).attr('data-name');
        confirm_modal_by_url(url, '删除确认', '确认删除【' + name + '】吗？');
        return false;
    });
    
    $('.ajax-popup').click(function () {
        var url = $(this).data('url');
        var title = $(this).data('title');
        var width = $(this).data('width');
        var height = $(this).data('height');
        popup_modal(url, title, width, height);
        return false;
    });

    var url = location.pathname; //location.href;
    $(".submenu a").each(function () {
        var link = $(this).attr("href");
        var menu = $(this).data("menu");
        var smenu = $(this).data("smenu");
        if (link != '' && ((url + '/').match(link + '/')
        || (url + '/').match(link))) {
            $("#menu-" + menu).addClass("open");
            if (smenu != '') {
                var submenu = $('#smenu-' + smenu).children('dd');
                var icon = $('#smenu-' + smenu).children('dt').find("i");

                icon.removeClass("icon-plus");
                icon.addClass("icon-minus");
                $('#smenu-' + smenu).addClass("open");
                $(submenu).addClass("open");
                submenu.show();
            }
        }
    });
    
    $("#sidebar .open li a, #sidebar dl.open dd a").each(function () {
        var link = $(this).attr("href");
        if (link != '' && ((url + '/').match(link + '/')
        || (url + '/').match(link))) {
            $(this).parent().addClass("active");
        }
    });

    $(".chkAll").click(function () {
        var checked = this.checked;
        var depth = $(this).attr("depth");
        var node = $(this);
        while (depth>0) {
            node = node.parent();
            depth--;
        }
        $("input[type=checkbox]:enabled", node).each(function () {
            $(this).attr("checked", checked);
        });
    });
    
    /* 支持两级子菜单展开和收缩 */
    $('#sidebar ul > li > ul > li > dl > dd').each(function (e) {
        if ($(this).hasClass('open')) {
        } else {
            $(this).hide();
        }
    });
    
    $('#sidebar ul > li > ul > li > dl > dt > a').click(function (e) {
        e.preventDefault();
        var submenu = $(this).parent().siblings('dd');
        var dl = $(this).parents('dl');
        var submenus = $('#sidebar ul > li > ul > li > dl > dd');
        var submenus_parents = $('#sidebar ul > li > ul > li > dl');
        var icon = $(this).find("i");
        var icons = $('#sidebar ul > li > ul > li > dl > dt > a > i');
        if (dl.hasClass('open')) {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenu.slideUp();
            } else {
                submenu.fadeOut(250);
            }
            dl.removeClass('open');
            
            icon.removeClass("icon-minus");
            icon.addClass("icon-plus");
        } else {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenus.slideUp();
                submenu.slideDown();
            } else {
                submenus.fadeOut(250);
                submenu.fadeIn(250);
            }
            submenus_parents.removeClass('open');
            dl.addClass('open');
            
            icons.removeClass("icon-minus");
            icons.addClass("icon-plus");
            icon.removeClass("icon-plus");
            icon.addClass("icon-minus");
        }
    });
    
    /* 页面载入时的js提示 */
    if (typeof (jsMessage) != "undefined") {
        if (jsMessage != "") {
            $.gritter.add({
                title: '提示',
                text: jsMessage,
                sticky: false
            });
        }
    }
});

function ajaxRequestSuccess(resp) {
    if (!!resp) {
        if (resp.Succeeded) {
            var url = null;
            var msg = "Succeed!!!";
            if (!!resp.RedirectUrl && resp.RedirectUrl.Trim().length > 0) {
                url = resp.RedirectUrl;
            }
            if (resp.ShowMessage) {
                if (!/^\s*$/.test(resp.Message) && resp.Message != null) {
                    msg = resp.Message;
                }
            } else {
                msg = null;
            }
            if (!!url) {
                if (!!msg) {
                    setTimeout(function () {
                        window.location.href = url;
                    }, 1500);
                } else {
                    window.location.href = url;
                }
            } else if (!!msg) {
                $.gritter.add({
                    title: '提示',
                    text: msg,
                    sticky: false
                });
            }
        } else {
            $.gritter.add({
                title: '提示',
                text: resp.ErrorMessage,
                sticky: false
            });
        }
    }
}

String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.LTrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.RTrim = function () {
    return this.replace(/(\s*$)/g, "");
}
