$(function () {
    initUserInfoFromCoookie();
});

var userinfo;

function getUserInfo() {
    return userinfo;
}

function initUserInfoFromCoookie() {
    var u = $.getCookie("userinfo");
    var de_u = decodeURIComponent(u);
    de_u = decodeURIComponent(de_u);
    var obj = $.parseJSON(de_u);
    console.log(obj);
    userinfo = obj;
}