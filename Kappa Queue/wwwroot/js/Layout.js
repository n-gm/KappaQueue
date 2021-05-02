var tokenKey = "accessToken";

window.onload = function () {
    //получаем идентификатор элемента
    var users = document.getElementById('usersHref');

    //вешаем на него событие
    users.onclick = function () {
        const token = sessionStorage.getItem(tokenKey);

        if (!token) {
            alert("Вы не авторизованы\n");
        }

        var req = new XMLHttpRequest();
        req.open('GET', '/forms/users', true); //true means request will be async
        req.onreadystatechange = function (aEvt) {
            if (req.readyState == 4) {
                if (req.status == 200)
                    document.write(req.responseText)
                else if (req.status == 401) {
                    alert("Вы не авторизованы\n");
                }
                else if (req.status == 403) {
                    alert("У вас нет права на работу с пользователями\n");
                } else
                    alert("Error loading page\n");
            }
        };
        req.setRequestHeader('Authorization', 'Bearer' + token);
        req.send();
    }
}