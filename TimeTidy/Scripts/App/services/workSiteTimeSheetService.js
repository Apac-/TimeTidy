"use strict";

var WorkSiteTimeSheetService = function () {

    var getWorkSiteTimeSheets = function (siteId, done, fail) {
        return $.getJSON("/api/workSiteTimeSheets/" + siteId)
            .done(done)
            .fail(fail);
    };

    return {
        getWorkSiteTimeSheets: getWorkSiteTimeSheets, 
    };
}();