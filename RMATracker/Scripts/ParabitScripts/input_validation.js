$(document).ready(function () {
    
    $('input').on("blur", function () {
        var id = this.id;

        if (this.value === '' && $("#" + id + "-error")[0] !== undefined) {
            var $x = $("#" + id + "-error");
            changeToTempToastr();
            changeFeedbackMessage('error', 'There are items that require your attention on the form');
            changeToStandardToastr();
            $('#' + id).addClass('input-error');
            $('label[for=' + id + ']').addClass('error-text');
            return;
        }

        if (this.value === '') {
            $('#' + id).addClass('input-error');
            $('label[for=' + id + ']').addClass('error-text');
        }
        else {
            $('#' + id).removeClass('input-error');
            $('label[for=' + id + ']').removeClass('error-text');
        }
    });
});