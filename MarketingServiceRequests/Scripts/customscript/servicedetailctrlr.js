$(document).ready(function () {

    /*
    $('.chkbox').on('change', function () {
        $('.chkbox').not(this).prop('checked', false);
    }); */

  



   


});


$("#btnserviceeventsubmit").on("click", function (event) {
    
    var checked = $(".eventchkbox:checked").length > 0;
    if (!checked) {
        alert("Please check at least one checkbox");
        event.preventDefault();
    }
});
$("#agreementsubmit").on("click", function (event) {
    var checked = $("#agreementconfirm:checked").length > 0;
    if (!checked) {
        alert("Please Confirm Agreement to Submit");
        event.preventDefault();
    }
});


function serviceeventssaveonclick(e) {



    var checked = $(".eventchkbox:checked").length > 0;
    if (!checked) {
        alert("Please check at least one checkbox");
        e.preventDefault();
    }
}