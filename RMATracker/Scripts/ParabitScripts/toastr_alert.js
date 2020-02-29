$(document).ready(function () {
    changeToStandardToastr();
});

function changeToTapToDismiss() {
    toastr.options = {


         "tapToDismiss": false,
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-center-center",
        "preventDuplicates": true,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": 0,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

/**
 * */
function changeToStandardToastr() {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-center-center",
        "preventDuplicates": true,
        "showDuration": 300,
        "hideDuration": 0,
        "timeOut": 0,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}
/**
 * Change to temporary toastr
 *  good for cases where you want the toastr alert
 *  to timeout
 * @param {int} timeout -> in milliseconds
 * @param {int} extended_timeout -> in milliseconds
 */
function changeToTempToastr(timeout, extended_timeout) {
    timeout = timeout || 3000;
    extended_timeout = extended_timeout || 8000;
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-center-center",
        "preventDuplicates": true,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": timeout,
        "extendedTimeOut": extended_timeout,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}
function changeToTapToDismissToastr(timeout, extended_timeout) {
    timeout = timeout || 3000;
    extended_timeout = extended_timeout || 8000;
    toastr.options = {
        "tapToDismiss": false,
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-center-center",
        "preventDuplicates": true,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": 0,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}
/**
 * changeFeedbackMessage is the only function you should be calling.
 * 
 * @param {string} alertType the type of alert you want:
 *  info -> blue
 *  error -> red
 *  warning -> yellow
 *  success -> green
 * @param {string} message message to be shown in alert
 * @param {string} title, if any
 * @param {string} additionalInfo, @TODO: Add modal for more information?
 */
function changeFeedbackMessage(alertType, message, title, additionalInfo, strictDialog, showCloseButton) {
    title = title || '';
    strictDialog = strictDialog || false;
    showCloseButton = showCloseButton || true;

    if (message.toLowerCase().indexOf('login again') > -1)
        additionalInfo = '<br /><br /><br /><button onclick = "href" type="button" class="para-btn clear">Login</button>'.replace('href', 'window.location.href = \'' + location + '\'')

    if (additionalInfo) {
        message += additionalInfo;
    }
    
    toastr[alertType.toLowerCase()](message, title);
    $('div[class="toast-message"]').css("text-align", "center");
    if (strictDialog)
        $('#toast-container').dialog({
            autoOpen: true,
            position: { my: "center", at: "top+" + $("#navigation").height, of: window },
            width: 1000,
            resizable: false,
            modal: true,
            title: formName == "formCompanyBranchSave" ? "Add New Branch" : (formName == "formCompanyDetailsSave" ? "Add New Company" : ""),
        });

}
