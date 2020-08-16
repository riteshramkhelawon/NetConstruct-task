//setup before functions
var typingTimer;                //timer identifier
var doneTypingInterval = 1000;  //time in ms (5 seconds)

//on keyup, start the countdown
$('#emailAddress').keyup(function () {
    clearTimeout(typingTimer);
    if ($('#emailAddress').val()) {
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    }
});

//user is "finished typing," do something
function doneTyping() {
    console.log($('#emailAddress').val())
    $.ajax({
        url: '/checkEmail',
        type: 'POST',
        data: { emailAddress: $('#emailAddress').val() },
        success: function (validEmailAddress) {
            console.log(validEmailAddress);
            if (validEmailAddress == "False") {
                $('#emailMessage').css('display', 'block');
                $('#submitCommentBtn').addClass('btn-secondary');
                $('#submitCommentBtn').attr('disabled', 'true');
            } else {
                $('#emailMessage').css('display', 'none');
                $('#submitCommentBtn').removeClass('btn-secondary');
                $('#submitCommentBtn').removeAttr('disabled');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}