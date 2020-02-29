function fixBootstrapModalZoomBug() {
    
    if (/ipad|iphone|ipod/.test(navigator.userAgent.toLowerCase())) {
        var m = $('meta[name=viewport]');
        var originalContent = m.attr('content');

        $('body').delegate('*', 'show.bs.modal', function (ev) {
            m.attr('content', 'width=device-width, minimum-scale=1.0, maximum-scale=1.0, initial-scale=1.0');
        }).delegate('*', 'hidden.bs.modal', function (ev) {
            m.attr('content', originalContent);
        });
    }
}