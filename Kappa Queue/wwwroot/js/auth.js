var tokenKey = "accessToken";

// отпавка запроса к контроллеру AccountController для получения токена
async function getTokenAsync() {

    // получаем данные формы и фомируем объект для отправки
    const formData = new FormData();
    formData.append("username", document.getElementById("usernameField").value);
    formData.append("password", document.getElementById("passwordField").value);

    //формируем json Для отправки
    var object = {};
    formData.forEach(function (value, key) {
        object[key] = value;
    });
    var json = JSON.stringify(object);

    // отправляет запрос и получаем ответ
    const response = await fetch("/api/auth", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-type": "application/json"
        },
        body: json
    });
    // получаем данные 
    const data = await response.json();

    // если запрос прошел нормально
    if (response.ok === true) {
        // сохраняем в хранилище sessionStorage токен доступа
        sessionStorage.setItem(tokenKey, data.access_token);
        window.location.replace("/users");
    }
    else {
        // если произошла ошибка, из errorText получаем текст ошибки
        console.log("Error: ", response.status, data.errorText);
    }
};

// получаем токен
document.getElementById("loginButton").addEventListener("click", e => {
    e.preventDefault();
    getTokenAsync();
});  