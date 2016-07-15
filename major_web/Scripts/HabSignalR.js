$(function () {

    var newFile = $.connection.newFile;

    //
    // Взять на себя
    //
    newFile.client.takeOnFile = function (id) {
        //Начальник
        $("div.cb-open[data-id=" + id + "]").toggle();
        $("div.cb-sign[data-id=" + id + "]").toggle();
        //Мэнеджер
        $("div.cb-to-sign[data-id=" + id + "]").toggle();
        $("div.cb-return[data-id=" + id + "]").toggle();
    };

    //
    // Закрыть
    //
    newFile.client.closeFile = function (id, msg) {
        $('#modDialogClose').modal('hide');
        var tr_close = $("tr[data-id=" + id + "]");
        tr_close.toggleClass("danger", true);
        var div_nachalnik = $("div.nachalnik[data-id=" + id + "]");
        div_nachalnik.empty();
        var div_manager = $("div.manager[data-id=" + id + "]");
        div_manager.empty();
        $("tr[data-id=" + id + "] .comment_cb").text(msg);
    };

    //
    // На подпись
    //
    newFile.client.toSignFile = function (id) {
        //Начальник
        $("div.cb-open[data-id=" + id + "]").toggle();
        $("div.cb-sign[data-id=" + id + "]").toggle();
        //Мэнеджер
        $("div.cb-to-sign[data-id=" + id + "]").toggle();
        $("div.cb-return[data-id=" + id + "]").toggle();

    };

    //
    // Подписать
    //
    newFile.client.signFile = function (id, out_id, date_creat) {
        var tr_close = $("tr[data-id=" + id + "]");
        //Начальник
        $("div.cb-open[data-id=" + id + "]").hide();
        $("div.cb-sign[data-id=" + id + "]").hide();
        //Мэнеджер
        $("div.cb-to-sign[data-id=" + id + "]").hide();
        $("div.cb-return[data-id=" + id + "]").hide();

        $('#modDialogSignFile').modal('hide');
        var msg = '<a href="/Home/Download/' + out_id + '?file_in=False">' + date_creat + '</a>';
        $("tr[data-id=" + id + "] .send_file_cb").append(msg);
        tr_close.toggleClass("success", true);

    };

    //
    // Если не указан сертификат (нету подписанта) с нашей стороны
    //
    newFile.client.notSignFile = function (id, str_comment_bad) {
        $("button.cb-sign[data-id=" + id + "]").removeClass("disabled");
        $("button.cb-close[data-id=" + id + "]").removeClass("disabled");
        $("button.cb-return[data-id=" + id + "]").removeClass("disabled");
    };

    //
    // Вернуть
    //
    newFile.client.reOpenFile = function (id) {
        //Начальник
        $("div.cb-open[data-id=" + id + "]").toggle();
        $("div.cb-sign[data-id=" + id + "]").toggle();
        //Мэнеджер
        $("div.cb-to-sign[data-id=" + id + "]").toggle();
        $("div.cb-return[data-id=" + id + "]").toggle();
    };




    $.connection.hub.start().done(function () {

        //Взять на себя
        $(document).on("click", "button.cb-open", function () {
            var id_file = $(this).data("id");

            newFile.server.fileOnTake(id_file);
        });

        //На подпись
        $(document).on("click", "button.cb-to-sign", function () {
            var id_user = $("div#user_name").data("user");
            var id_file = $(this).data("id");

            newFile.server.fileToSign(id_user, id_file);
        });

        //Закрыть
        $(document).on("click", "button.button_close_cb", function () {
            var id_file = $(this).attr('id');
            var msg = $('[name="comment_close_cb"]').val();
            msg = $.trim(msg);
            if (msg === "") {
                $('div.requeri_comment p').text("Укажите комментарий!");
            }
            else {
                newFile.server.fileClose(id_file, msg);
            }
        });

        //Подписать
        $(document).on("click", "button.button_sign_cb", function () {
            //var id_user = $("div#user_name").data("user");
            var id_file = $(this).attr('id');
            //var $btn = $(this).button("Подписываем");
            //btn.addClass("disabled");
            //$("button.cb-close[data-id=" + id_file + "]").addClass("disabled");
            //$("button.cb-return[data-id=" + id_file + "]").addClass("disabled");
            newFile.server.fileSign(id_file);
            //$btn.button('reset');
        });

        //Вернкть
        $(document).on("click", "button.cb-return", function () {
            var id_file = $(this).data("id");

            newFile.server.fileReOpen(id_file);
        });
    });

    newFile.client.stopClient = function () {
        $.connection.hub.stop();
    };
});