"use strict";

var UserTimeSheetsService = function () {
    var getUserTimeSheets = function (userId, done, fail) {
        return $.getJSON(
            "/api/userTimeSheets/" + userId)
            .done(done(data))
            .fail(fail);
    };

    var deleteUserTimeSheet = function (sheetId, success, fail) {
        return $.ajax({
            url: "/api/usertimesheets/" + sheetId,
            method: "DELETE",
            success: success,
            error: fail,
        });
    };


    return {
        getUserTimeSheets: getUserTimeSheets,
        deleteUserTimeSheet: deleteUserTimeSheet,
    };
}();