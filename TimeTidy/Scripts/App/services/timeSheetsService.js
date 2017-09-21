"use strict";

var TimeSheetsService = function () {
    var getTimeSheet = function (sheetId, done, fail) {
        $.getJSON("/api/timesheets/" + siteId)
            .done(done(data)).fail(fail);
    };

    return {
        getTimeSheet: getTimeSheet,
    };
}();