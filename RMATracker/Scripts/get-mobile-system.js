function getMobileOperatingSystem() {
  var userAgent = navigator.userAgent || navigator.vendor || window.opera;

      // Windows Phone must come first because its UA also contains "Android"
    if (/windows phone/i.test(userAgent)) {
        return "Windows Phone";
    }

    if (/android/i.test(userAgent)) {
        return "android";
    }

    
    if (/iPad|iPhone|iPod|Apple /.test(userAgent) ) {
        return "ios";
    }

    return "unknown";
}