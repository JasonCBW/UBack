$(function () {
    initUserInfoFromCoookie();
});

var userinfo;

function getUserInfo() {
    return userinfo;
}

function initUserInfoFromCoookie() {
    var u = $.cookie('userinfo');
    if (u != null && u != "") {
        var obj = $.parseJSON(u);
        console.log(obj);
        userinfo = obj;
    }
    else {
        window.location.href = "/WebAdmin/Login/login";
    }
}