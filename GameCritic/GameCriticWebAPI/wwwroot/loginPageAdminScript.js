let loginUrl = "http://localhost:5123/api/UserGamers/Login";

function jwtLogin() {
    $("#loginButton").prop("disabled", true);

    let loginData = {
        "username": $("#username").val(),
        "password": $("#password").val()
    }
    $.ajax({
        method: "POST",
        url: loginUrl,
        data: JSON.stringify(loginData),
        contentType: 'application/json'
    }).done(function (tokenData) {
        //console.log(tokenData);
        localStorage.setItem("JWT", tokenData);

        $("#loginButton").prop("disabled", false);

        // redirect
        window.location.href = "logItemsPage.html";
    }).fail(function (err) {
        alert(err.responseText);

        localStorage.removeItem("JWT");
        $("#loginButton").prop("disabled", false);
    });
}