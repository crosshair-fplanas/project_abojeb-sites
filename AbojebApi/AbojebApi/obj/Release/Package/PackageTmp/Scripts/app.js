$(document).ready(function () {
    $('.sidebar-toggler').on('click', function (e) {
        $('.sidebar-fixed').toggleClass($(this).data('toggle'));
        e.preventDefault();
    });

    //--------------------------------------
    //form functions
    //--------------------------------------

    function CallMethod(url, parameters, submitBtn, targetElement, successCallback) {
        BtnLoadingText(submitBtn);

        $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(parameters),
            dataType: "html",
            contentType: 'application/json;',
            success: function (data) {
                successCallback(data);

                submitBtn.html(submitBtn.data('original-text'));
                submitBtn.prop('disabled', false);
            },
            error: function (xhr, textStatus, errorThrown) {
                alert(errorThrown);
                submitBtn.html(submitBtn.data('original-text'));
                submitBtn.prop('disabled', false);
            }
        });
    }

    function ShowMessage(title, message) {
        $('#messageTitle').html(title);
        $('#messageText').html(message);
        $('#popReturnMessage').modal('show');
    }

    function OnTransactionSuccess(data) {
        if (data != null)
        {
            data = $.parseJSON(data);
            ShowMessage(data.Title, data.Message);
        }
    }

    $('.btn-submit').on('click', function (e) {
        e.preventDefault();
        var form = $(this.form);

        var action = form.attr('action');
        if ($(this).attr('data-form-action')) {
            form.attr('action', $(this).data('form-action'));
        }

        var parameters = $(form).serializeObject();
        var targetElement = $(this).data('target-element');
        var returnType = $(this).data('return-type');

        var callback;
        switch (returnType) {
            case 'report':
                callback = function (data) {
                    $(targetElement).html(data);
                    $('.data-table').dataTable();
                    $('.onsubmit-show').removeClass('d-none');
                };
                break;
            default:
                callback = OnTransactionSuccess;
        }

        CallMethod(action, parameters, $(this), targetElement, callback);
    });

    $('.btn-submit-redirect').on('click', function (e) {
        e.preventDefault();
        var form = $(this.form);

        var action = form.attr('action');
        if ($(this).attr('data-form-action')) {
            form.attr('action', $(this).data('form-action'));
        }

        form.submit();
    });

    function BtnLoadingText(button) {
        button.prop('disabled', true);
        var loadingText = '<i class="fas fa-spinner fa-spin"></i> loading...';
        if (button.html() !== loadingText) {
            button.data('original-text', button.html());
            button.html(loadingText);
        }
    }


    //--------------------------------------
    //plugins initialization
    //--------------------------------------

    //multiple select with tags
    $('.multiselect-tag').fastselect();

    //date range picker
    $('.daterange').daterangepicker();

    //data table
    $('.data-table').dataTable();

    //--------------------------------------
    //helpers
    //--------------------------------------
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
})