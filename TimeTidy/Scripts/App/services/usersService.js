"use strict";

var UsersService = function () {

    var getUsers = function (done, fail) {
        return $.getJSON(
            "/api/users")
            .done(done(data))
            .fail(fail);
    };

    var deleteUser = function (userId, success, fail) {
        $.ajax({
            url: "/api/users/" + userId,
            method: "DELETE",
            success: success,
            error: fail
        });
    };

    return {
        getUsers: getUsers,
        deleteUser: deleteUser,
    };
}();