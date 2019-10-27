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

//Setup DOM elements and Event Listeners
elements = {};
ready(function () {

    elements = {
        iframe: document.getElementById('tokenExIframe'),
        iframeData: document.getElementById("IframePost"),
        form: document.getElementById("paymentForm"),
        btnSubmit: document.getElementById('btnSubmit'),
        expMonth: document.getElementById('expMonth'),
        expYear: document.getElementById('expYear')
    };

    elements.form.addEventListener("submit", submitForm);

    window.addEventListener("message", util.listener);

    //util.addEventListener(window, 'message', util.listener);
});


var util = {
    addEventListener:
        function (el, eventName, handler) {
            if (el.addEventListener) {
                el.addEventListener(eventName, handler, false);
            } else {
                el.attachEvent('on' + eventName, function () {
                    handler.call(el);
                });
            }
        },
    preventDefault: function (evt) {
        var ev = evt !== undefined ? evt : window.event;
        if (ev.preventDefault) {

            ev.preventDefault();
        } else {
            ev.returnValue = false;
        }
        return ev;
    },
    log:
        function (el) {
            console.log(el);
        },
    addClass:
        function (el, className) {
            if (el) {
                if (el.classList) {
                    el.classList.add(className);
                } else {
                    el.className += ' ' + className;
                }

            }
        },
    removeClass:
        function (el, className) {
            if (el) {
                if (el.classList) {
                    el.classList.remove(className);
                } else {
                    el.className = el.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
                }
            }
        },
    validateExpiration:
        function (el) {
            var expiryMonth = $('#expMonth').val();
            var expiryYear = $('#expYear').val();
            var expiryDate = new Date(expiryYear, expiryMonth);

            //Set currentDate to beginning of current month
            var d = new Date();
            var currentMonth = d.getMonth();
            var currentYear = d.getFullYear();
            var currentDate = new Date(currentYear, currentMonth);

            if (expiryDate < currentDate) {
                util.addClass(elements.expMonth, 'input-validation-error');
                return false;
            } else {
                util.removeClass(elements.expMonth, 'input-validation-error');
                return true;
            }
        },
    submit:
        function (el) {
            util.log(el);
            if (util.validateExpiration()) {
                util.postMessage('tokenize', 'https://test-htp.tokenex.com');
            }
        },
    postMessage:
        function (msg, URL) {
            util.log($('#tokenExIframe'));
            elements.iframe.contentWindow.postMessage(msg, URL);
        },
    listener:
        function (event) {
            util.log("event origin: " + event.origin);
            var message = JSON.parse(event.data);
            util.log("event captured: " + message.event);
            for (var i in message) {
                util.log(message[i]);
                //if (message.event == "validation")
                //    continue;
                //if (message.event == "post") {
                //    elements.iframeData.value = JSON.stringify(message.data);
                //    //elements.form.submit();
                //}
            }

            util.log("iframe message:" + event.data)
        }
};

function submitForm(e) {
    var validDate = util.validateExpiration();
    util.preventDefault(e);

    if (!validDate) {
        return false;
    }
    else {
        elements.iframe.contentWindow.postMessage('tokenize', 'https://test-htp.tokenex.com');
        return true;
    }

}

//Cross Browser Compatible Document Ready Check
function ready(fn) {
    if (document.addEventListener) {
        document.addEventListener('DOMContentLoaded', fn);
    } else {
        document.attachEvent('onreadystatechange', function () {
            if (document.readyState === 'complete') {
                fn();
            }
        });
    }
}