        
        
$(document).ready(function () {
            var radioValue = $("input[tag='Yes']:checked").val();
            if (radioValue == "true") {
                $('#divCalledDate').show();
            }
            else {
                $('#divCalledDate').hide();
            }
            ToggeleCreditRequestReason($("input[id='rmaType']:checked"));
            GetReturnAddressId();            
            
            $(document).on('change', ".dropdown-return-type", function (e) {
                $("input[name$='ReturnTypeDescription']").val($("#" + $(this).attr("id") + " option:selected").text());
            });
            $(document).on('change', ".dropdown-credit-reason", function (e) {
                $("input[name$='CreditReasonDescription']").val($("#" + $(this).attr("id") + " option:selected").text());
            });

    
    function GetProblemDescription(e) {
        //alert($("#" + e.id + " option:selected").text());
        $("input[name$='ClientComplaint']").prop('value', $("#" + e.id + " option:selected").text());
    }
    function GetBankStateName(e) {
        $("input[name$='BankStateFullName']").prop('value', $("#" + e.id + " option:selected").text());
    }
    function GetReturnAddressStateName(e) {
        $("input[name$='ReturnAddressStateFullName']").prop('value', $("#" + e.id + " option:selected").text());
    }
    function GetSurveyStateName(e) {
        $("input[name$='SurveyStateFullName']").prop('value', $("#" + e.id + " option:selected").text());
    }
    function GetCableOriginName(e) {
        $("input[name$='CableOriginFullName']").prop('value', $("#" + e.id + " option:selected").text());
    }
    function LoadImageOnForm(e) {
        var index = $("#" + e.id).attr("index");
        var fileName
        if (e.files.length == 0) {
            fileName = $("[id$=" + index + "__image_location]").val();
            if (fileName != '' && fileName != null) {
                $("#image-" + index).attr("src", fileName);
                $("#" + e.id).removeAttr("aria-required");
                $("#" + e.id).removeAttr("required");
            }

            return;
        }

        fileName = e.files[0].name;
        fileName = fileName;
        if ($("[id$=" + index + "__image_location]").length>0)
            $("[id$=" + index + "__image_location]").val(fileName);
        else if ($("[tag=image-path-" + index+"]").length > 0)
            $("[tag$=image-path-" + index + "]").val(fileName);
        

        var reader = new FileReader();
        reader.onload = function (e) {
            $("#image-" + index).attr("src", e.target.result);
        }        
        reader.readAsDataURL(e.files[0]);
        $("image-"+ index).fadeIn("fast").attr('src', URL.createObjectURL(e.files[0]));
}

       
    


   
//function LoadReturnAddressData() {
//        $.ajax({
//            cache: false,
//            url: '@Html.Raw(Url.Action(AddAddressPartialView","RMAForm"))',
//            data: { obj1: value1, obj2: value2 },
//            dataType: "json",
//            type: 'post',
//            success: function (result) {
//                if (result.success) {
//                    $('#frmAddNewAddress').html(result.data);
//                }
//                else {
//                    alert("Error Loading data!");
//                }
//            }
//        });
//    }
//    function CancelNewAddress(e) {

//    }
        //@*function UpdateStatus() {
        //    var key = $("#CaseList").val();
        //    $.ajax({
        //        url: '@Url.Action("UpdateStatus", "Shared")',
        //        async: false,
        //        type: "GET",
        //        contentType: 'application/json',
        //        data: { 'CaseNo': key },
        //        dataType: 'html',
        //        success: function (result) {
        //            $("#rma_header").html('<h2>RMA Details for Case #' + key + '</h2>');
        //            $("#div_rma_header").show();;
        //            result = result.replace('"div_category"', '"div_category" style="display:none"');
        //            result = result.replace('"btnSend"', '"btnSend" style="display:none"');
        //            $("#div_rma_form").html(result);

        //        },
        //        error: function (request, status, error) {
        //            alert(error);
        //        }
        //    });
        //}*@

    
        //$(function () {
        //    $("input[name^='upload-file-']").change(function (event) {
            
        //        //var tmppath = URL.createObjectURL(event.target.files[0]);
        //        alert(event.target.files[0]);
        //        $(event.target.id.replace("upload-file-", "image-")).fadeIn("fast").attr('src', URL.createObjectURL(event.target.files[0]));

        //    });
        //});

