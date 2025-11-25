window.theme = {
    save(theme) {
        try {
            localStorage.setItem("rf-theme", theme);
        } catch (e) {
            console.warn("Theme save failed:", e);
        }
    },

    load() {
        try {
            return localStorage.getItem("rf-theme") || "light";
        } catch (e) {
            console.warn("Theme load failed:", e);
            return "light";
        }
    },

    apply() {
        const saved = this.load();
        document.documentElement.setAttribute("data-bs-theme", saved);
    },

    jsSwitchTheme() {
        const current = this.load();
        const next = current === "dark" ? "light" : "dark";

        this.save(next);

        // If we are on Identity pages (Login, Register, etc.)
        if (window.location.href.includes("/Account/")) {
            window.location.reload();
        }
        else {
            this.apply();
        }

        return next;
    }
};

// Apply theme immediately on page load
window.theme.apply();
