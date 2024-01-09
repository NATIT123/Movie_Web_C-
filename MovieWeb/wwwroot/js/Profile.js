document.getElementById("button-addon2").addEventListener("click", function () {
    if (document.getElementById("User_Name").readOnly)
        document.getElementById("User_Name").readOnly = false
    else
        document.getElementById("User_Name").readOnly = true
});

document.getElementById("button-addon3").addEventListener("click", function () {
    if (document.getElementById("User_Password").readOnly)
        document.getElementById("User_Password").readOnly = false
    else
        document.getElementById("User_Password").readOnly = true
});
