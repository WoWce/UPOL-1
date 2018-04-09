
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

