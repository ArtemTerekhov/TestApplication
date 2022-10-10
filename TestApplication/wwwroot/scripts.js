$(document).ready(function () {
    if (jQuery) {
        getClients();
    } else {
        alert("jQuery не загружен. Работа со страницей невозможна!");
    }
});

function getClients() {

    $.ajax({
        url: "/api/getclients",
        type: "GET",
        dataType: "json",
        success: function (data) {

            jQuery(data).each(function (i, item) {
                var clientRow = $("<tr  id='" + item.clientId + "' genderId='" + item.genderId +
                    "' " + "locationId = '" + item.locationId + "' clientName='" + item.name + "'>" +
                    "<td>" + (i + 1) + "&nbsp;<span class='getClient'>" + item.name +
                    "</span></td>" +
                    "<td><span class='editClient'>Редактировать</a></span></td>" +
                    "<td><span class='deleteClient'>Удалить</span></a></td>" +
                    "</tr>");

                $("#mainInfo").append(clientRow);
            });

            $(".getClient").on("mouseover", function () {
                getClient($(this).parent().parent().attr("id"));
            });
            $(".getClient").on("mouseout", function () {
                $("#clientInfo" + $(this).parent().parent().attr("id")).css("display", "none");
            });
            $(".editClient").on("click", function () {
                EditClient($(this).parent().parent().attr("id"));
            });
            $(".deleteClient").on("click", function () {
                DeleteClient($(this).parent().parent().attr("id"));
            });
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
}

$("#addClient").on("click", function () { AddClient(); });

function getClient(clientId) {

    if ($("#clientInfo" + clientId).html() === undefined) {

        $.ajax({
            url: "/api/getclient/" + clientId,
            type: "GET",
            dataType: "json",
            success: function (data) {

                $("tr[id='" + clientId + "']").after("<tr id='clientInfo" + clientId + "'>" +
                    "<td colspan='4'><div>Пол - " + data.gender + "</div>" +
                    "<div>Место проживания - " + data.location + "</div></td>" +
                    "</tr>");

                $("#clientInfo" + clientId).css("display", "block");
            },
            error: function (error) {
                alert(JSON.stringify(error));
            }
        });
    }
    else {
        $("#clientInfo" + clientId).css("display", "block");
    }
}

function AddClient() {
    AddForm(0);
}

function EditClient(clientId) {
    AddForm(clientId);
}

function DeleteClient(clientId) {
    $.ajax({
        url: "/api/deleteclient/" + clientId,
        type: "DELETE",
        success: function () {
            alert("Удалено!");
            location.reload();
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
}

function AddForm(clientId) {

    $(".editor").remove();

    var title = "";

    if (clientId != 0) {
        title = "Редактирование клиента";
    }
    else {
        title = "Добавление клиента";
    }

    $("#addClient").css("display", "none");
    $("h1").after("<div class='editor'>" +
        "<form id = 'clientForm' ><span>" + title + "</span><br /></form ></div>");
    $("div").css("width", "50%");
    $("div").css("margin-left", "auto");
    $("div").css("margin-right", "auto");

    $("#clientForm").append("<input type='hidden' name='clientId'  value='0' />");

    if (clientId != 0) {
        $("input[name='clientId']").val(clientId);
    }

    $("#clientForm").append("<input type='text' name='name' id='clientName'  value='' /><br />");

    $("input[name='name']").on('keyup blur', function () {

        $(this).val($(this).val().replace(/[^A-Za-z_\s]/, ""));
    });

    if (clientId != 0) {
        $("input[name='name']").val($("tr[id='" + clientId + "']").attr("clientName"));
    }

    $("#clientForm").append("<select name='genderId'></select><br />");

    $.ajax({
        url: "/api/getgenders",
        type: "GET",
        dataType: "json",
        success: function (data) {

            jQuery(data).each(function (i, item) {
                var clientRow = $("<option value='" + item.genderId + "'>" + item.name + "</option>");
                $("select[name='genderId']").append(clientRow);
            });

            if (clientId != 0) {
                $("select[name='genderId'] option[value=" + $("tr[id='" + clientId + "']")
                    .attr("genderId") +
                    "]").attr('selected', 'selected');
            }
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });

    $("#clientForm").append("<select name='locationId'></select>");

    $.ajax({
        url: "/api/getlocations",
        type: "GET",
        dataType: "json",
        success: function (data) {

            jQuery(data).each(function (i, item) {
                var clientRow = $("<option value='" + item.locationId + "'>" + item.name + "</option>");
                $("select[name='locationId']").append(clientRow);
            });

            if (clientId != 0) {
                $("select[name='locationId'] option[value=" + $("tr[id='" + clientId + "']")
                    .attr("locationId") +
                    "]").attr('selected', 'selected');
            }
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });

    $("#clientForm").append("<div><input type='button' name='clientToDb' class='button'" +
        " value='Сохранить' />" +
        "<input type='button' name='cancelEdit' class='button'" +
        " value='Отмена' /></div><br />"
    );

    if (clientId != 0) {
        $("input[name='clientToDb']").on("click", function () {
            SaveToDb(clientId);
        });
    }
    else {
        $("input[name='clientToDb']").on("click", function () {
            SaveToDb(0);
        });
    }

    $("input[name='cancelEdit']").on("click", function () {
        CancelEdit();
    });
}

function CancelEdit() {
    $(".editor").remove();
    $("#addClient").css("display", "block");
}

function SaveToDb(clientId) {

    var method = "", url = "", jsonData;

    if (clientId != 0) {
        url = "/api/editclient";
        method = "PUT";
    }
    else {
        url = "/api/addclient";
        method = "POST";
    }

    jsonData = convertFormToJSON($("#clientForm"));

    $.ajax({
        url: url,
        type: method,
        contentType: "application/json",
        data: JSON.stringify(jsonData),
        success: function () {
            alert("Сохранено!");
            location.reload();
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
}

function convertFormToJSON(form) {
    const array = $(form).serializeArray();
    const json = {};
    $.each(array, function () {
        json[this.name] = this.value || "";
    });
    return json;
}