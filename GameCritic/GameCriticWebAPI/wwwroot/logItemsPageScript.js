$(document).ready(function () {
    const jwtToken = localStorage.getItem("JWT");

    function fetchLogs() {
        $.ajax({
            type: "GET",
            url: "http://localhost:5123/api/LogItems/GetAllLogs",
            headers: {
                Authorization: `Bearer ${jwtToken}`,
            },
            dataType: "json",
            success: function (data) {
                console.log("Data received:", data);
                displayLogs(data);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching data:", error);
            }
        });
    }

    function displayLogs(logs) {
        const $logList = $("#logList");
        $logList.empty(); // Clear previous logs if any
        logs.forEach(log => {
            $logList.append(`
                <li>ID: ${log.idlog}  ||  Text: ${log.txt}  ||  Lvl: ${log.lvl}  ||  Time: ${log.tmstamp}</li>
            `);
        });
    }

    $("#showLogs").on("click", fetchLogs);

    function jwtLogout() {
        localStorage.removeItem("JWT");
        window.location.href = "loginPageAdmin.html";
    }

    $("#logout").on("click", jwtLogout);
});