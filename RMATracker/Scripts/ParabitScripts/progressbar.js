/**
 * Progressbar that will load forever until you destroy the html object 
 */


function generate_progressbar() {
    var progressionbar = document.getElementById('ldBar');
    $("#blackout").removeClass("below hidden");
    if (progressionbar === null || progressionbar === undefined) {
        // @TODO: Create a new 
        var progressbar_container = document.getElementById('loading-container');        
    }
    
    var loading_banner = document.getElementById('loading-banner');
    var parabit_loading_bar = new ldBar(progressionbar);
    var loading_count = 0;
    var incrementer = -5;
    var percent = 0;
    var interval = setInterval(function () {
        if (percent === -5 || percent === 105)
            incrementer *= -1

        percent += incrementer;
        parabit_loading_bar.set(percent);

        //parabit_loading_bar.set(Math.round(Math.random() * 100));
        if ((loading_count - 1) % 4 === 0)
            loading_banner.innerText = 'Loading';
        else
            loading_banner.innerText += '.';
        loading_count++;
    }, 350);
    
    document.getElementById("ld-bar-container").classList.remove('hidden');
    return interval;
}

function destroy_progressbar(interval) {
    if(interval)
        clearInterval(interval);
    $("#blackout").addClass("below hidden");
    $("#ld-bar-container").addClass("hidden")
    
    
}

function test_progressbar() {
    var x = generate_progressbar();
    // destroy_progressbar(x);
}
