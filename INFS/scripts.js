/*Scroll up*/
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

/*Slider*/

var slideIndex = 0;
slideShow();

function slideShow() {
    var i;
    var articleArr = document.getElementsByClassName("slider-article");

    var tmp;
    for(i = 0; i<articleArr.length; i++){
        if(i == 0){
            tmp = {innerHTML: articleArr[0].innerHTML};
        }
        if(i == articleArr.length - 1){
            articleArr[i].innerHTML = tmp.innerHTML;
        } else{
            articleArr[i].innerHTML = articleArr[i+1].innerHTML;
        }
    }

    slideIndex++;
    if (slideIndex > articleArr.length) {slideIndex = 1}
    setTimeout(slideShow, 9000);
}