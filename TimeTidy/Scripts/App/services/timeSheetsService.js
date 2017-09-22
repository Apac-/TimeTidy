"use strict";

var TimeSheetsService = function () {
    var getTimeSheet = function (sheetId, done, fail) {
        return $.getJSON(
            "/api/timesheets/" + siteId)
            .done(done(data))
            .fail(fail);
    };

    var deleteTimeSheet = function (sheetId, done, fail) {
        return $.ajax({
            url: "/api/usertimesheets/" + button.attr("data-sheet-id"),
            method: "DELETE",
            success: done,
            error: fail
        });
    };

    return {
        getTimeSheet: getTimeSheet,
        deleteTimeSheet: deleteTimeSheet,
    };
}();