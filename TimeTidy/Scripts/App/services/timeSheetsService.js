"use strict";

var TimeSheetsService = function () {
    var getTimeSheet = function (sheetId, done, fail) {
        return $.getJSON(
            "/api/timesheets/" + siteId)
            .done(done(data))
            .fail(fail);
    };

    var userLogonToSite = function (site, userLat, userLng, success, fail) {
        let logonDto = {
            workSiteId: site.id,
            siteName: site.name,
            siteLat: site.lat,
            siteLng: site.lng,
            siteAddress: site.streetAddress,
            userLat: userLat,
            userLng: userLng
        };

        $.ajax({
            url: "/api/timeSheets",
            method: "post",
            data: logonDto
        }).done(success).fail(fail);
    };

    var userLogoffOfSite = function (sheetId, userLat, userLng, success, fail) {
        let logoffDto = {
            userLat: userLat,
            userLng: userLng
        };

        $.ajax({
            url: "/api/timeSheets/" + sheetId,
            method: "put",
            data: logoffDto
        }).done(success).fail(fail(sheetId));
    };

    return {
        getTimeSheet: getTimeSheet,
        userLogonToSite: userLogonToSite,
        userLogoffOfSite: userLogoffOfSite,
    };
}();