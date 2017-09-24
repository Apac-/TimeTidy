"use strict";
var WorksitesIndexView = function () {
    var populateSitesTable = function (jsonData) {
        $('#sites').DataTable({
            data: jsonData,
            columns: [
                {
                    data: "name",
                    render: function (data, type, workSite) {
                        return "<button class='btn-link js-select' data-lat=" + workSite.lat + " data-lng=" + workSite.lng + " data-siteId=" + workSite.id + ">" + data + "</button>";
                    }
                },
                {
                    data: "streetAddress"
                }
            ]
        });
    };

    var reportError = function (message) {
        alert(message);
    };

    var setLogButton = function (isLoggedOn) {
        if (!isLoggedOn) {
            $('#logbtn').prop({
                'value': 'Logon',
                'class': 'btn btn-success js-logon'
            }).show();
        } else {
            $('#logbtn').prop({
                'value': 'Logoff',
                'class': 'btn btn-warning js-logoff'
            }).show();
        };
    };

    var setSite = function (siteName) {
        $('#nearestSite').text(`Selected: ${siteName}`);
        $('html,body').animate({ scrollTop: 0 }, 'fast');
    };

    var loggingButtonClicked = function (button) {
        $(button).hide();
        $('#overlay').css('width', '100%');
    };

    return {
        populateSitesTable: populateSitesTable,
        reportError: reportError,
        setLogButton: setLogButton,
        setSite: setSite,
        loggingButtonClicked: loggingButtonClicked
    };
}();