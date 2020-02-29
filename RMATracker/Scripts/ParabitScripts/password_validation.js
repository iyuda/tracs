function ValidatePasswordMain(Popup) {
    Popup = Popup || true;
    var pw_id = 'password-txt';
    var pw_regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@@#\$%\^&\*%?])(?=.{8,16}).*/g;
    var pw = $('#' + pw_id);
    if (!$('#password-details').hasClass('self'))
        $(document).find(".password-rules, .pass-rule-row").each(function () {
            $(this).remove();
        });
    else
        $('#password-details').addClass("hidden");

    var divParent = pw.closest('div.pwd-field');
    if (divParent.length===0) divParent = pw.closest('div');
    $(divParent).find("label.error").each(function () {
        $(this).remove();
    });
    if (pw.val() === '' || !pw_regex.test(pw.val())) {
        //pw.addClass('input-error');
        $('#password-details').addClass('error-text');
        
        
        if (pw.val().length >= 8) {
            
                $label = $("<label  class='error pass-rule' for='" + pw_id + "'>");
                $label.attr('id', pw_id + "-error");
                if (pw.parent().is('td')) {
                    var newCell = $('<td> </td>');
                    var newRow = $('<tr class="pass-rule-row"> </tr>');
                    newCell.appendTo(newRow);
                    newCell = $('<td align="right"> </td>')
                    $label.appendTo(newCell);
                    newCell.appendTo(newRow);
                    newRow.insertAfter(pw.closest('tr'));
                }
                else {
                    $label.appendTo(divParent);
                }
                $label.css("color", "red");

            var regex1 = /^(?=.*[a-z]).*/g;
            var regex2 = /^(?=.*[A-Z]).*/g;
            var regex3 = /^(?=.*[0-9]).*/g;
            var regex4 = /^(?=.* [!@@#\$ %\^&\*%?]).*/g;
            if (!regex1.test(pw.val()))
                $label.text("Required: 1 lowercase letter, i.e. a");
            else if (!regex2.test(pw.val()))
                $label.text("Required: 1 uppercase letter, i.e. A");
            else if (!regex3.test(pw.val()))
                $label.text("Required: 1 digit, i.e. 1");
            else if (!regex4.test(pw.val()))
                $label.text("Required: 1 special character (#?!@$%^&*_)");
        }
        if (!$('#password-details').hasClass('self')) {
            var $passwordMsg = $('#password-details').clone();
            $passwordMsg.removeClass("hidden");
            $passwordMsg.addClass("password-rules");
            $passwordMsg.insertAfter(divParent);
        }
        else {
            $('#password-details').removeClass("hidden");
        }
            
        return false;
    }
    else {
        $('label[for=' + pw_id + ']').removeClass('error-text');
        $('#password-details').removeClass('error-text');
        //pw.removeClass('input-error');
        return true;
    }

}

function ValidatePasswordConfirm() {
    var conf_id = 'confirm-password-txt'
    var pw_id = 'password-txt'
    var conf = $('#' + conf_id);
    var pw = $('#' + pw_id);

    var divParent = conf.closest('div.pwd-field');
    if (divParent.length === 0) divParent = conf.closest('div');
    var errorLabels = $(divParent).find("label.error:visible");
    errorLabels.each(function () {
        $(this).remove();
    });
    if (pw.val() !== '' && conf.val() !== '' &&
        conf.val() !== pw.val()) {

        //conf.addClass('input-error');
        //pw.addClass('input-error')
        var $label = $("<label  class='error' for='" + this.id + "'>");
        $label.attr('id', this.id + "-error");

        $label.text('Password and Confirm Passwords don\'t match'); $label.css("color", "red");
        
        $(divParent).find("label.error").each(function () {
            $(this).remove();
        });
        $label.appendTo(divParent);

        return false;
    } else if (conf.val() === pw.val()) {
        $('label[for=' + conf_id + ']').removeClass('error-text');
        $('label[for=' + pw_id + ']').removeClass('error-text');
        return true;
    }
    return false;

}
function ValidatePassword(e) {
    return ValidatePasswordMain() && ValidatePasswordConfirm();
}
$(document).ready(function () {
    $('#password-txt').on('blur', function () {
        ValidatePasswordMain();
    });
    $('#password-txt').on('focus', function () {
        ValidatePasswordMain(false);
    });
    $('#password-txt').on('keyup', function () {
        $(".pass-rule").remove();
    });
    $('#confirm-password-txt').on('blur', function () {
        ValidatePasswordConfirm();
    });
});
