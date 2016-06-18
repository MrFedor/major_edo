moment.locale('ru');

$('input[name="daterange"], input[name="date_in_edo"]').daterangepicker({
    format: 'DD.MM.YYYY',
    locale: {
        applyLabel: 'Готово',
        cancelLabel: 'Отмена',
        fromLabel: 'С',
        toLabel: 'Пo'
    }
});



//$(document).on('change', '[data-toggle="tooltip"]', function (e) {
//    e.preventDefault();
//    $(this).tooltip({ trigger: "hover" });
//});




$('#deport').on('change', function () {
    var d = $('form[name="form_list_deport"]');
    var dd = $(".client_fond_str").text("Перечень Файлов:");
    var ddd = $("#Filelist .panel-body");
    d.submit();
    ddd.empty();
});



$('#Filelist').on('click', 'button.file_in_butt', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();   
    $.ajax({
        async: false,
        dataType: "html",
        url: "/Home/FileList/",
        data: {
            id_rule: id_rule,
            file_in: true
        },
        cache: false,
        success: function (data) {
            $("div#Filelist").html(data);
        }
    });
});

$('#Filelist').on('click', 'button.file_out_butt', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();   
    $.ajax({
        async: false,
        dataType: "html",
        url: "/Home/FileList/",
        data: {
            id_rule: id_rule,
            file_in: false
        },
        cache: false,
        success: function (data) {
            $("div#Filelist").html(data);
        }
    });
});

$('#Filelist').on('change', '#date_folder', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val();
    var name_file = $('input[name="name_file"]').val();
    var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
    var date_folder = $("select#date_folder").val();

    $.ajax({
        async: false,
        url: "/Home/SearchFiles/",
        data: {
            id_rule: id_rule,
            file_in: file_in,
            name_file: name_file,
            id_checkbox: id_checkbox,
            form_date_in_edo: form_date_in_edo,
            date_folder: date_folder
        },
        cache: false,
        success: function (data) {
            $('div#file_list').html(data);
        }
    });
});

$('#Filelist').on('keyup', '#name_file', function (e) {
    e.preventDefault();
    var id_rule = $('input[name="id_rule"]').val();
    var file_in = $('input[name="file_in"]').val();
    var name_file = $('input[name="name_file"]').val();
    var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
    var date_folder = $("select#date_folder").val();
    
    $.ajax({
        async: false,
        url: "/Home/SearchFiles/",
        data: {
            id_rule: id_rule,
            file_in: file_in,
            name_file: name_file,
            id_checkbox: id_checkbox,
            form_date_in_edo: form_date_in_edo,
            date_folder: date_folder
        },
        cache: false,
        success: function (data) {
            $('div#file_list').html(data);
        }
    });
});

$('#Filelist').on('change', 'input[name="id_checkbox"]', function (e) {
    e.preventDefault();
    var id_checkbox = $('input[name="id_checkbox"]').prop("checked");
    if (id_checkbox == false) {
        var id_rule = $('input[name="id_rule"]').val();
        var file_in = $('input[name="file_in"]').val();
        var name_file = $('input[name="name_file"]').val();
        var form_date_in_edo = $('input[name="form_date_in_edo"]').val();
        var date_folder = $("select#date_folder").val();

        $.ajax({
            async: false,
            url: "/Home/SearchFiles/",
            data: {
                id_rule: id_rule,
                file_in: file_in,
                name_file: name_file,
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

$('#Filelist').on('apply.daterangepicker', '#form_date_in_edo', function (e) {
    e.preventDefault();
    $('form[name="form_search_list"]').submit();
});


$("#data_l_contragent").on('change', function () {
    var d = $('form[name="form_list_deport"]');
    var dd = $(".client_fond_str").text("Перечень Файлов:");
    var ddd = $("#Filelist .panel-body");
    d.submit();
    ddd.empty();
});

//Обработчик кнопки Сверка в ТаскЛисте и в ЛистЛисте
$('#file_task_list, #Filelist').on('click', '.butt_sverka', function (e) {
    var mod = $(this);
    var id_file = mod.data("id");
    e.preventDefault();
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
$('#file_task_list, #Filelist').on('click', 'button.cb-close', function (e) {
    var mod = $(this);
    var id_file = mod.data("id");
    e.preventDefault();
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
$('#file_task_list, #Filelist').on('click', 'button.cb-sign', function (e) {
    var mod = $(this);
    var id_file = mod.data("id");
    e.preventDefault();
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
$('#file_task_list, #Filelist').on('click', '.butt_sig_popover', function (e) {
    var pop = $(this);
    var id_file = pop.data("id");
    e.preventDefault();
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
            }).popover('toggle')
        },
        error: function (data) {
            pop.popover({
                html: true,
                placement: 'auto top',
                title: "Ошибка",
                viewport: "#Filelist",
                content: data,
                trigger: 'manual'
            }).popover('toggle')
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



