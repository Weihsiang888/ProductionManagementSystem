// Culture cookie management functions
window.CultureHelper = {
    setCookie: function(name, value, days) {
        const maxAge = days * 24 * 60 * 60;
        document.cookie = name + "=" + value + "; path=/; max-age=" + maxAge + "; SameSite=Lax";
    }
};
