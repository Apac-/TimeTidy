"use strict";

var TimeSheetsService = function () {
    var getTimeSheet = function (sheetId, done, fail) {
        let deferredObj = $.getJSON("/api/timesheets/" + siteId)
                                .done(done(data)).fail(fail);
        return deferredObj;
    };

    return {
        getTimeSheet: getTimeSheet,
    };
}();