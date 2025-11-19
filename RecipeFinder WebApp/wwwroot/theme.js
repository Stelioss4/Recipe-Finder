window.theme = {
    save: function (theme) {
        localStorage.setItem("rf-theme", theme);
    },
    load: function () {
        return localStorage.getItem("rf-theme");
    }
};
