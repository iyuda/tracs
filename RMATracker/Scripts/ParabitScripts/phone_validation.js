
    /*Start of phone number formating */
var n;
var p;
var p1;
function format_phone(id) {
    p = p1.value;
    if (p.length >= 3) {
    pp = p;
    d4 = p.indexOf('(')
    d5 = p.indexOf(')')
            if (d4 == -1) {
        pp = "(" + pp;
    }
            if (d5 == -1) {
        pp = pp + ")";
    }
    $(id).val("");
    $(id).val(pp);
}
if (p.length > 3) {
    d1 = p.indexOf('(')
    d2 = p.indexOf(')')
    if (d2 == -1) {
        l30 = p.length;
        p30 = p.substring(0, 4);
        p30 = p30 + ")"
        p31 = p.substring(4, l30);
        pp = p30 + p31;
        $(id).val("");
        $(id).val(pp);
    }
}
    if (p.length > 5) {
    p11 = p.substring(d1 + 1, d2);
    if (p11.length > 3) {
        p12 = p11;
        l12 = p12.length;
        l15 = p.length
        p13 = p11.substring(0, 3);
        p14 = p11.substring(3, l12);
        p15 = p.substring(d2 + 1, l15);
        $(id).val("");
        pp = "(" + p13 + ")" + p14 + p15;
        $(id).val(pp);
    }
    l16 = p.length;
    p16 = p.substring(d2 + 1, l16);
    l17 = p16.length;
    if (l17 > 3 && p16.indexOf('-') == -1) {
        p17 = p.substring(d2 + 1, d2 + 4);
        p18 = p.substring(d2 + 4, l16);
        p19 = p.substring(0, d2 + 1);
        pp = p19 + p17 + "-" + p18;
        $(id).val("");
        $(id).val(pp);
    }
}

//setTimeout(format_phone, 100, id);
}

function ValidatePhoneNumber(e) {
    var elementValue = $(e).val();
    var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
    var divParent = $(e).closest('div');
    
    var status=true;
    if (!phoneNumberPattern.test(elementValue) && $.trim(elementValue)!='') {
        $(divParent).find("label.validate-error:visible").each(function () {
            $(this).remove();
        });
        var $label = $("<label  class='validate-error' for='" + this.id + "'>");
        $label.attr('id', this.id + "-error");
        $label.text("Expected format: (xxx) xxx-xxxx (enter only digits)"); $label.css("color", "red");
        $label.appendTo(divParent);
        
        status= false;
    }
    return status;
}
    function getPhoneFormat(m, event) {
        n = m.name;
        p1 = m;
        var divParent = $(m).closest('div');
        $(divParent).find("label.validate-error:visible").each(function () {
            $(this).remove();
        });
        var keyDown = event.which || event.keyCode;
        if (!(keyDown > 47 && keyDown < 58)) {
            event.preventDefault();
            return;
        }
        if (keyDown !== 8) {
            format_phone("#" + $(m).attr('id'));
        }
    }
function phoneKeyPress(m, event) {

    var keyPress = event.which || event.keyCode;
    if (!(keyPress > 47 && keyPress < 58) && keyPress !== 8) {
        event.preventDefault();
        return;
    }
    
}
$(document).ready(function () {
    var e = $("input[id$='phone-txt'");
    if (!e) return;

    $(e).on('blur', function () {
        ValidatePhoneNumber(this);
    });
    $(e).on('focus', function () {
        ValidatePhoneNumber(this);
    });
});
