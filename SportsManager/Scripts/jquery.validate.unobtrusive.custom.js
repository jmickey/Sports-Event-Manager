(function ($) {
    var defaultOptions = {
        validClass: 'has-success',
        errorClass: 'has-error',
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".form-group")
                .addClass('has-error');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).closest(".form-group")
                .removeClass('has-error');
        }
    };

    $.validator.setDefaults(defaultOptions);
})(jQuery);
