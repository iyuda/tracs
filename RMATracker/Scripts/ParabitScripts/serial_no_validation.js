

function validateSerialNo(e, scroll) {
    var divParent = $(e).closest('div');

    var status = $.trim($(e).val()) == '' || ($(e).val().length >= $(e).attr('minlength') && $(e).val().length <= $(e).attr('maxlength'));
    var errorLabels = $(divParent).find("label.select-part-info:visible, label.validate-numeric-error:visible, label.validate-unique-error:visible, label.validate-serial-error:visible");
    errorLabels.each(function () {
        $(this).remove();
    });


    if (!$.isNumeric($(e).val()) && $.trim($(e).val()) != '' && $(e).attr("index")>0) {
        $(divParent).find("label.validate-error:visible").each(function () {
            $(this).remove();
        });
        $(divParent).find("label.error:visible").each(function () {
            $(this).remove();
        });
        var $label = $("<label  class='validate-numeric-error' for='" + $(e).id + "'>");
        $label.attr('id', $(e).id + "-error");
        $label.text("Please enter a valid number."); $label.css("color", "red");
        $label.appendTo(divParent);
        if (scroll)
            $('html, body').animate({
                scrollTop: $(divParent).offset().top - $("#navigation").height()
            }, 2);
        status = false;
    }
    else {
        
        $(".serial:visible").each(function () {
            if ($(this).val() == $(e).val() && e.id != this.id && $.trim($(e).val()) != '') {
                
                var divOtherParent = this.closest('div');
                $(divOtherParent).find("label.validate-unique-error:visible").each(function () {
                    $(this).remove();
                });
                var $label = $("<label  class='validate-unique-error' for='" + $(e).id + "'>");
                $label.attr('id', $(e).id + "-error");
                $label.text("Serial Numbers must be unique."); $label.css("color", "red");
                $label.appendTo(divParent);
                if (scroll)
                    $('html, body').animate({
                        scrollTop: $(divParent).offset().top - $("#navigation").height()
                    }, 2);
                status = false;
            }
        });
    }
    if (status && $.trim($(e).val()) != '' ) {
        $(e).siblings("button").removeAttr("disabled");
        $(divParent).find("label:visible").each(function () {
            $(this).remove();
        });
        var $label = $("<label  class='select-part-info' for='" + $(e).id + "'>");
        $label.attr('id', $(e).id + "-info");
        $label.text("Please click the search button to get the part information."); $label.css("color", "green");
        $label.appendTo(divParent);
    }
    else
        $(e).siblings("button").attr("disabled", "disabled");

    return status;
}

function GetPartDetails(e) {
    var url = urlGetPartDetails.replace("dummy", $(e).val());
    $.ajax({
        type: "GET",
        url: url,
        async: false,
        success: function (result) {
            var status = result.status;
            var message = result.message;

            var divParent = $(e).closest('div');
            var errorLabels = $(divParent).find("label.select-part-info:visible, label.validate-serial-error:visible");
            errorLabels.each(function () {
                $(this).remove();
            });
            if (status == 0) {
                var $label = $("<label  class='validate-serial-error' for='" + $(e).attr('id') + "'>");
                $label.attr('id', $(e).attr('id') + "-error");
                message = "There is no part matching the serial #.  Please enter the part number manually.";
                $label.text(message); $label.css("color", "red");
                $label.appendTo(divParent);
                $(e).closest('div.container1').find(".part-number[role='combobox']").focus().select();

                // enable combo box
                var Selector = $(e).closest('div.part-details').find("[data-role='combobox']");
                Selector.each(function () {
                    var combobox = $(this).data("kendoComboBox");
                    combobox.enable(true);
                });
                
            }
            else if (status == 2) {
                changeFeedbackMessage("error", message)
                return false;
            }
            else {
                var data = result.data[0];
                var PartNumber = data.PartNumber;

                $(e).closest('div.container1').find(".part-number[data-role='combobox']").data("kendoComboBox").select(function (dataItem) {
                    return dataItem.Text === PartNumber;
                });
                // disable combo box
                var Selector = $(e).closest('div.part-details').find("[data-role='combobox']"); 
                Selector.each(function () {
                    var combobox = $(this).data("kendoComboBox");
                    combobox.enable(false);
                    $(this).trigger("change");
                });

            }
            return status;
        },
        error: function (request, status, error) {
            changeFeedbackMessage("error", error)
            return false;
        }
    });
}

$(document).ready(function () {

    var e = $(".serial");

    if (!e) {
        return;
    }
    $(".serial").each(function () {
        $(this).on('blur', function () {
            validateSerialNo(this);
        });
        $(this).on('focus', function () {
            validateSerialNo(this);
        });
        $(this).on('keyup', function () {
            validateSerialNo(this);
        });
    });
});
