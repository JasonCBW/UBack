$(function () {
    initUserInfoFromCoookie();
});

var userinfo;

function getUserInfo() {
    return userinfo;
}

function initUserInfoFromCoookie() {
    debugger;
    var u = $.cookie('userinfo');
    var obj = $.parseJSON(u);
    console.log(obj);
    userinfo = obj;
}