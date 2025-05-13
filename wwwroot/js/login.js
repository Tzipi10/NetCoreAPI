
document.addEventListener("DOMContentLoaded", () => {
const form = document.getElementById("loginForm");
    form.onsubmit = async (e) => {
        e.preventDefault();
        console.log("Form submitted");

        const Name = document.getElementById("username").value;
        const Password = document.getElementById("password").value;

        const response = await fetch("/Manage/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Name, Password })
        });

        if (response.status === 401) {
            document.getElementById("error").innerText = "המשתמש אינו קיים במערכת";
            return;
        } else if (response.status !== 200) {
            document.getElementById("error").innerText = "שגיאה";
            return;
        }
        const data = await response.text();
        console.log("------"+data);
        localStorage.setItem("token", data);
        window.location.href = "./index.html";
    };
});

