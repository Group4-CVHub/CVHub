
window.fbAsyncInit = function () {
    FB.init({
        appId: '1011233316074653',
        autoLogAppEvents: true,
        xfbml: true,
        version: 'v10.0'
    });
    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


FB.login(function (response) {
    if (response.status === 'connected') {
        window.location.replace("http://www.w3schools.com");
    } else {
        window.location.replace("http://http://localhost:54894/Validation/FacebookSignIn"); 
    }
});


FB.logout(function (response) {
});