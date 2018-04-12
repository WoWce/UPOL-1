$(document).ready(function(){

    var numImgs = $('.imgSmall').length;
    var prevImages = $('.imgSmall').prevAll().length;
    var imageClicked;
    var thisImage;
    var nextImage;
    var image;
    $(".imgSmall").click(function(){
        image = $(this).attr("src");
        $("#imgBig").attr("src",image);
        thisImage = $(this);
        $("#overlay").show();
        $("#overlayContent").show();
        if(thisImage.closest('img').parent().next().find('img').attr("src") != undefined){
            $(".right").show();
        }
        if(thisImage.closest('img').parent().prev().find('img').attr("src") != undefined){
            $(".left").show();
        }

    });

    $("#imgBig").click(function(){

        $("#imgBig").attr("src", "");
        $("#overlay").hide();
        $("#overlayContent").hide();
        $(".left").hide();
        $(".right").hide();
    });



    $(".right").click(function () {
        nextImage = thisImage.closest('img').parent().next().find('img');
        $("#imgBig").attr("src",nextImage.attr("src"));
        thisImage = nextImage;
        if(thisImage.closest('img').parent().next().find('img').attr("src") == undefined){
            $(".right").hide();
        }
        if(thisImage.closest('img').parent().prev().find('img').attr("src") != undefined){
            $(".left").show();
        }
    });

    $(".left").click(function () {
        nextImage = thisImage.closest('img').parent().prev().find('img');
        $("#imgBig").attr("src",nextImage.attr("src"));
        thisImage = nextImage;
        if(thisImage.closest('img').parent().prev().find('img').attr("src") == undefined){
            $(".left").hide();
        }
        if(thisImage.closest('img').parent().next().find('img').attr("src") != undefined){
            $(".right").show();
        }
    });

});