//(function fadeInDiv() {
//    var divs = $('.fadeIn');
//    var divsize = ((Math.random() * 100) + 50).toFixed();
//    var posx = (Math.random() * ($(document).width() - divsize)).toFixed();
//    var posy = (Math.random() * ($(document).height() - divsize)).toFixed();
//    var maxSize = 30;
//    var minSize = 8;
//    var size = (Math.random() * maxSize + minSize)

//    var elem = divs.eq(Math.floor(Math.random() * divs.length));

//    if (!elem.is(':visible')) {
//        elem.fadeIn(Math.floor(Math.random() * 1000), fadeInDiv);
//        elem.css({
//            'position': 'absolute',
//            'left': posx + 'px',
//            'top': posy + 'px',
//            'font-size': size + 'px'
//        });
//    } else {
//        elem.fadeOut(Math.floor(Math.random() * 1000), fadeInDiv);
//    }
//})();

$(document).ready(function () {
    //var elements = {};
    //var iframe = $("#tokenExIframe");
    //var hmac = $("#hmac");
    //var form = $("#iframeform");
    //var expMonth = $('#expMonth');
    //var expYear = $('#expYear');

    var iframeConfig = {
        origin: $('#authenOrigin').val(),
        timestamp: $('#authenTimestamp').val(),
        tokenExID: $('#tokenExID').val(),
        authentiationKey: $('#authenKey').val(),
        //tokenScheme: $('#tokenScheme').val(),
        debug: true,
        enablePrettyFormat: true,
        cvv: true
    };

    if ($('#tokenExDiv').is(":visible")) {
        var iframe = new TokenEx.Iframe("tokenExDiv", iframeConfig);

        iframe.on("load", function () { console.log("iFrame Loaded"); });

        iframe.load();
    }

    $('btnSubmit').on('click', function () {
        var isValidDate = validateExpiration();
        if (!isValidDate)
            return false;
    });


});

function validateExpiration() {
    var expiryMonth = $('#expYear').val();
    var expiryYear = $('#expYear').val();
    var expiryDate = new Date(expiryYear, expiryMonth);
    var validDate = false;

    //Set currentDate to beginning of current month
    var d = new Date();
    var currentMonth = d.getMonth();
    var currentYear = d.getFullYear();
    var currentDate = new Date(currentYear, currentMonth);

    if (expiryDate < currentDate) {
        if (!($('#expMonth').hasClass('input-validation-error')))
            $('#expMonth').addClass('input-validation-error');

        if (!($('#expYear').hasClass('input-validation-error')))
            $('#expYear').addClass('input-validation-error');
        
    } else {
        if ($('#expMonth').hasClass('input-validation-error'))
            $('#expMonth').removeClass('input-validation-error');

        if ($('#expYear').hasClass('input-validation-error'))
            $('#expYear').removeClass('input-validation-error');

        validDate = true;
    }

    return validDate;
}