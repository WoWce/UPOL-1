window.onscroll = function() {scrollFunction()};

function scrollFunction(){
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
        document.getElementById("scroll-to-top").style.display = "block";
    } else {
        document.getElementById("scroll-to-top").style.display = "none";
    }
}

function scrollToTop() {
    var scrollStep = -window.scrollY / (500 /*scroll duration*/ / 15),
        scrollInterval = setInterval(function(){
            if ( window.scrollY != 0 ) {
                window.scrollBy( 0, scrollStep );
            }
            else clearInterval(scrollInterval);
        },15);
}