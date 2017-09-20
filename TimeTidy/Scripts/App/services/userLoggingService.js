'use strict';

var UserLoggingService = function () {
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
        }).done(success).fail(fail);
    };

    return {
        userLogonToSite: userLogonToSite,
        userLogoffOfSite: userLogoffOfSite,
    };
}();