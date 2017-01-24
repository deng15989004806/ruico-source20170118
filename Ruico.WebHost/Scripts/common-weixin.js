function ajaxForm(form) {
    $.ajax({
        url: form.action,
        type: form.method,
        data: $(form).serialize(),
        dataType: "json",
        success: ajaxRequestSuccess,
        error: function (result) {
            alert(JSON.stringify(result));
        }
    });
}

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
                    msg_modal('提示', msg, function () {
                        window.location.href = url;
                    });
                } else {
                    window.location.href = url;
                }
            }else if (!!msg) {
                msg_modal('提示', msg);
            }
        } else {
            msg_modal('提示', resp.ErrorMessage);
        }
    }
}

function msg_modal(title, content) {
    $('#confirm_modal').modal();
    $('#confirm_title').html(title);
    $('#confirm_content').html(content);
    $('#confirm_confirm').html('确定');
    $('#confirm_cancel').hide();
}

function confirm_modal_by_url(url, title, content, requestType, okText, cancelText) {
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

    $('#confirm_confirm').html('确定');
    $('#confirm_cancel').html('取消');
    if (okText != null && okText != '') {
        $('#confirm_confirm').html(okText);
    }
    if (cancelText != null && cancelText != '') {
        $('#confirm_cancel').html(cancelText);
    }

    $('#confirm_cancel').one('click', function () {
        $('#confirm_confirm').unbind('click');
    });
    $('#confirm_cancel').show();
}

$(function () {
    /* 页面载入时的js提示 */
    if (typeof (window.jsMessage) != "undefined") {
        if (window.jsMessage !== "") {
            msg_modal('提示', window.jsMessage);
            window.setTimeout(function () {
                $('#confirm_modal').modal("hide");
            }, 3000);
        }
    }

    $('.ajax-cancel-apply').click(function () {
        var url = $(this).attr('href');
        var name = $(this).attr('data-name');
        confirm_modal_by_url(url, '取消确认', '确认取消【' + name + '】申请吗？', 'POST', '取消申请', '不，点错了');
        return false;
    });
});

String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.LTrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.RTrim = function () {
    return this.replace(/(\s*$)/g, "");
}
