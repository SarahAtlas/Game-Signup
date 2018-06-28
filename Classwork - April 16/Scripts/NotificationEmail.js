$(function () {

    $("#firstName, #lastName, #email").on('keyup', function() {
        $("#submit-button").prop('disabled', !isFormValid());
    });
    
    function isFormValid() {
        var firstName = $('#firstName').val();
        var lastName = $('#lastName').val();
        var email = $('#email').val();

        if (!firstName || !lastName || !email) {
            return false;
        }

        return isValidEmail(email);
    }

    function isValidEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
});