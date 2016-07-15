moment.locale('ru');

$('body').on('focus', '.form_daterangepicker', function (e) {
    e.preventDefault();
    $(this).daterangepicker({
        autoApply: false,
        autoUpdateInput: false,
        opens: "left",
        linkedCalendars: false,
        locale: {
            format: "DD.MM.YYYY",
            separator: " - ",
            applyLabel: "ОК",
            cancelLabel: "Отмена"
        }
    });
});

$('body').on('apply.daterangepicker', '.form_daterangepicker', function (e, picker) {
    e.preventDefault();
    $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
});

$('body').on('cancel.daterangepicker', '.form_daterangepicker', function (e, picker) {
    e.preventDefault();
    $(this).val('');
});

$('body').on('change', '#Department', function (e) {
    e.preventDefault();
    var id = $(this).val();
    $.ajax({
        async: true,
        beforeSend: function () {
            $('div#list_deport').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
        },
        url: "/Home/GetNavClient/",
        data: {
            Department: id
        },
        cache: false,
        success: function (data) {
            $('div#list_deport').html(data);
        }
    });
});

$('body').on('click', 'button.file_in_butt', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val(true);
    var name_filelist = $('input[name="name_filelist"]').val('');
    var id_checkbox = $('input[name="id_checkbox"]').prop({ checked: false });
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val('');
    //var date_folder = $("#date_folder").val('');

    $('button.file_out_butt').removeClass('btn-primary');
    $('button.file_out_butt').addClass('btn-default');
    $(this).removeClass('btn-default');
    $(this).addClass('btn-primary');

    $.ajax({
        async: true,
        beforeSend: function () {
            $('div#file_list').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
        },
        dataType: "html",
        url: "/Home/SearchFiles/",
        data: {
            id_rule: id_rule,
            file_in: true,
            name_filelist: "",
            id_checkbox: false,
            form_date_in_edo: "",
            date_folder: ""
        },
        cache: false,
        success: function (data) {
            myCombo3.clearAll();
            myCombo3.setComboValue(null);
            myCombo3.setComboText("");
            $.ajax({
                type: 'get',
                async: false,
                url: '/Home/GetDateList',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: {
                    id_rule: id_rule,
                    file_in: true
                },
                cache: false,
                success: function (response) {
                    myCombo3.addOption(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //$('#DuNumDate').val("В Базе не найден Номер и Дата ДУ");
                }
            });
            $('div#file_list').html(data);
        }
    });

    var chek = $('input[name="id_checkbox"]');
    if (chek.is(":checked")) {
        //$('#date_folder').hide();
        myCombo3.hide();
        $('#form_date_in_edo').show();
    }
    else {
        //$('#date_folder').show();
        myCombo3.show();
        $('#form_date_in_edo').hide();
    }

    chek.on('change', function () {
        if (chek.is(":checked")) {
            //$('#date_folder').hide();
            myCombo3.hide();
            $('#form_date_in_edo').show();
        }
        else {
            //$('#date_folder').show();
            myCombo3.show();
            $('#form_date_in_edo').hide();
        }

    });

});

$('body').on('click', 'button.file_out_butt', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val(false);
    var name_filelist = $('input[name="name_filelist"]').val('');
    var id_checkbox = $('input[name="id_checkbox"]').prop({ checked: false });
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val('');
    //var date_folder = $("#date_folder").val('');

    $('button.file_in_butt').removeClass('btn-primary');
    $('button.file_in_butt').addClass('btn-default');
    $(this).removeClass('btn-default');
    $(this).addClass('btn-primary');

    $.ajax({
        async: true,
        beforeSend: function () {
            $('div#file_list').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
        },
        dataType: "html",
        url: "/Home/SearchFiles/",
        data: {
            id_rule: id_rule,
            file_in: false,
            name_filelist: "",
            id_checkbox: false,
            form_date_in_edo: "",
            date_folder: ""
        },
        cache: false,
        success: function (data) {
            myCombo3.clearAll();
            myCombo3.setComboValue(null);
            myCombo3.setComboText("");
            $.ajax({
                type: 'get',
                async: false,
                url: '/Home/GetDateList',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: {
                    id_rule: id_rule,
                    file_in: false
                },
                cache: false,
                success: function (response) {
                    myCombo3.addOption(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //$('#DuNumDate').val("В Базе не найден Номер и Дата ДУ");
                }
            });
            $('div#file_list').html(data);
        }
    });

    var chek = $('input[name="id_checkbox"]');
    if (chek.is(":checked")) {
        //$('#date_folder').hide();
        myCombo3.hide();
        $('#form_date_in_edo').show();
    }
    else {
        //$('#date_folder').show();
        myCombo3.show();
        $('#form_date_in_edo').hide();
    }

    chek.on('change', function () {
        if (chek.is(":checked")) {
            //$('#date_folder').hide();
            myCombo3.hide();
            $('#form_date_in_edo').show();
        }
        else {
            //$('#date_folder').show();
            myCombo3.show();
            $('#form_date_in_edo').hide();
        }

    });
});

$('body').on('keyup', '#name_filelist', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val();
    var name_filelist = $('input[name="name_filelist"]').val();
    var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
    var date_folder = myCombo3.getSelectedValue();

    delay(function () {
        $.ajax({
            async: true,
            beforeSend: function () {
                $('div#file_list').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
            },
            url: "/Home/SearchFiles/",
            data: {
                id_rule: id_rule,
                file_in: file_in,
                name_filelist: name_filelist,
                id_checkbox: id_checkbox,
                form_date_in_edo: form_date_in_edo,
                date_folder: date_folder
            },
            cache: false,
            success: function (data) {
                $('div#file_list').html(data);
            }
        });
    }, 1000);

});

$('body').on('change', '#id_checkbox', function (e) {
    e.preventDefault();
    var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    if (id_checkbox === false) {
        var id_rule = $('input[name="id_rule"]').val();
        var file_in = $('input[name="file_in"]').val();
        var name_filelist = $('input[name="name_filelist"]').val();
        var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
        var date_folder = myCombo3.getSelectedValue();
        $.ajax({
            async: true,
            beforeSend: function () {
                $('div#file_list').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
            },
            url: "/Home/SearchFiles/",
            data: {
                id_rule: id_rule,
                file_in: file_in,
                name_filelist: name_filelist,
                id_checkbox: id_checkbox,
                form_date_in_edo: form_date_in_edo,
                date_folder: date_folder
            },
            cache: false,
            success: function (data) {
                $('div#file_list').html(data);
            }
        });
    }

});

$('body').on('apply.daterangepicker', '#form_date_in_edo', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val();
    var name_filelist = $('input[name="name_filelist"]').val();
    //var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
    var date_folder = myCombo3.getSelectedValue();

    $.ajax({
        async: true,
        beforeSend: function () {
            $('div#file_list').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
        },
        url: "/Home/SearchFiles/",
        data: {
            id_rule: id_rule,
            file_in: file_in,
            name_filelist: name_filelist,
            id_checkbox: true,
            form_date_in_edo: form_date_in_edo,
            date_folder: date_folder
        },
        cache: false,
        success: function (data) {
            $('div#file_list').html(data);
        }
    });
});

var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();


//Модальное окно - просмотр XML Согласия/Договора
$('body').on('click', '.butt_xml_sogl', function (e) {
    e.preventDefault();
    var mod = $(this);
    var id_file = mod.data("id");
    var file_in = $('input[name="file_in"]').val();
    $('#modDialogXmlSogl').modal('show');
    $.ajax({
        async: false,
        beforeSend: function () {
            $('div#dialogContentXmlSogl').html("<img class='center-block' src='/Content/imgs/ring-alt.gif' />");
        },
        url: "/Home/XMLSogl/",
        data: {
            id_file: id_file,
            file_in: file_in
        },
        cache: false,
        success: function (data) {
            $('#dialogContentXmlSogl').html(data);
            //$('#modDialogXmlSogl').modal('show');
        }
    });
});


//Обработчик кнопки Сверка в ТаскЛисте и в ЛистЛисте
$('body').on('click', '.butt_sverka', function (e) {
    e.preventDefault();
    var mod = $(this);
    var id_file = mod.data("id");
    $.ajax({
        async: false,
        url: "/Home/Sverka/" + id_file,
        cache: false,
        success: function (data) {
            $('#dialogContentSverka').html(data);
            $('#modDialogSverka').modal('show');
        }
    });
});

//Модальное для закрытия файлов
$('body').on('click', 'button.cb-close', function (e) {
    e.preventDefault();
    var mod = $(this);
    var id_file = mod.data("id");
    $.ajax({
        async: false,
        url: "/Home/CloseFile/" + id_file,
        cache: false,
        success: function (data) {
            $('#dialogContentClose').html(data);
            $('#modDialogClose').modal('show');
        }
    });
});

//Модальное для подписания файлов
$('body').on('click', 'button.cb-sign', function (e) {
    e.preventDefault();
    var mod = $(this);
    var id_file = mod.data("id");
    $.ajax({
        async: false,
        url: "/Home/SignFile/" + id_file,
        cache: false,
        success: function (data) {
            $('#dialogContentSignFile').html(data);
            $('#modDialogSignFile').modal('show');
        }
    });
});

//Обработчик для кнопки ИнфаПодпись в ТаскЛисте и в ЛистЛисте
$('body').on('click', '.butt_sig_popover', function (e) {
    e.preventDefault();
    var pop = $(this);
    var id_file = pop.data("id");
    $.ajax({
        async: false,
        url: "/Home/InfoSig/" + id_file,
        dataType: "html",
        cache: false,
        success: function (data) {
            pop.popover({
                html: true,
                placement: 'auto top',
                title: "Информация",
                viewport: "#Filelist",
                content: data,
                trigger: 'manual'
            }).popover('toggle');
        },
        error: function (data) {
            pop.popover({
                html: true,
                placement: 'auto top',
                title: "Ошибка",
                viewport: "#Filelist",
                content: data,
                trigger: 'manual'
            }).popover('toggle');
        }
    });
});

$('body').on('click', function (e) {
    $('[data-toggle="popover"]').each(function () {
        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
            $(this).popover('hide');
        }
    });
});



