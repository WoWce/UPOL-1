<?php
 class PagesController{
     public function home(){
         require_once ('views/home.php');
     }

     public function category(){
         require_once ('views/category.php');
     }

     public function post_page(){
         require_once ('views/post_page.php');
     }
 }