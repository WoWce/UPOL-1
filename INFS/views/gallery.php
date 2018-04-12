
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="../js/gallery.js"></script>
<script src="../js/scroll.js"></script>
<link rel="stylesheet" type="text/css" href="../css/category.css">
<link rel="stylesheet" type="text/css" href="../css/gallery.css">
    <header>
        <div class="top-nav-bar">
            <div class="logo">
                <a href="../home">
                    <h1>
                        <span class="logo-express black">Express</span>
                        <span class="logo-blog kinda-blue">Blogs & Magazines</span>
                    </h1>
                </a>
            </div>
            <nav class="menu grey">
                <ul>
                    <li class="not-selected-button">
                        <a href="../home">Home</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li>
                                    <a href="#">Deep Menu 112312313</a>
                                </li>
                                <li>
                                    <a href="#">Deep Menu 2</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li class="not-selected-button">
                        <a href="./category">World</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li class="dir">
                                    <a href="#">Sub Menu 1</a>
                                </li>
                                <li class="dir">
                                    <a href="#">Sub Menu 2</a>
                                </li>
                                <li>
                                    <a href="#">Sub Menu 3</a>
                                </li>
                                <li>
                                    <a href="#">Sub Menu 4</a>
                                </li>
                                <li>
                                    <a href="#">Sub Menu 5</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li class="not-selected-button">
                        <a href="./category">Sport</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li>
                                    <a href="#">Deep Menu 1</a>
                                </li>
                                <li>
                                    <a href="#">Deep Menu 2</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li class="not-selected-button">
                        <a href="./category.html">Lifestyle</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li>
                                    <a href="#">Deep Menu 1</a>
                                </li>
                                <li>
                                    <a href="#">Deep Menu 2</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li class="not-selected-button">
                        <a href="./category">Health</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li>
                                    <a href="#">Deep Menu 1</a>
                                </li>
                                <li>
                                    <a href="#">Deep Menu 2</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li class="not-selected-button">
                        <a href="./category">Post & Pages</a>
                        <div>
                            <div class="submenu-space"></div>
                            <ul>
                                <li>
                                    <a href="#">Deep Menu 1</a>
                                </li>
                                <li>
                                    <a href="#">Deep Menu 2</a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li>
                        <button class="btn-search">
                            <img src="../img/search.png" alt="Search icon" />
                        </button>
                    </li>
                </ul>
            </nav>
        </div>
    </header>

    <main>
        <div id="overlay"></div>
        <button class="gallery-button left"><img src="../img/arrow-left-white.png"></button>
        <div id="overlayContent">
            <img id="imgBig" src="" alt=""  />
        </div>
        <button class="gallery-button right"><img src="../img/arrow-right-white.png"></button>
        <div class="row">
            <div class="column">
                <?php
                    $directory = "img/gallery";
                    $images = glob("$directory/*.{jpg,jpeg}", GLOB_BRACE);
                    usort($images, function($a, $b) { return filemtime($a) < filemtime($b); });
                    foreach($images as $image)
                    { ?>
                        <div class="container">
                            <img class="imgSmall" src="<?php echo $image ?>">
                        </div>
                    <?php }
                ?>
            </div>
        </div>
    </main>

    <footer>
    <div class="footer-container">
        <div class="footer-content">

            <section class="footer-info">
                <div class="logo">
                    <a href="../html%20temps/index.html">
                        <h2>
                            <span class="logo-express white">Express</span>
                            <span class="logo-blog kinda-blue">Blogs & Magazines</span>
                        </h2>
                    </a>
                </div>

                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam tincidunt tellus lacus. Duis quis mi ante.
                </p>

            </section>

            <section class="footer-useful-links">
                <div class="category-head">
                    <h2>Useful Links</h2>
                </div>
                <ul>
                    <li>
                        <a href="../html%20temps/index.html">Home 1</a>
                    </li>
                    <li>
                        <a href="../html%20temps/index.html">Home 2</a>
                    </li>
                    <li>
                        <a href="../html%20temps/index.html">Home 3</a>
                    </li>
                    <li>
                        <a href="../html%20temps/index.html">Home 4</a>
                    </li>
                    <li>
                        <a href="../html%20temps/index.html">Home 5</a>
                    </li>
                </ul>
            </section>

            <section class="footer-follow">
                <div class="category-head">
                    <h2>Follow Us</h2>
                </div>

                <ul class="twitter-list">
                    <li>
                        <a href="https://facebook.com">
                            <div>
                                <img src="../img/category/facebook-logo-white.png" alt="Facebook">
                            </div> facebook</a>
                    </li>
                    <li>
                        <a href="https://twitter.com">
                            <div>
                                <img src="../img/category/twitter-logo-white.png" alt="Twitter">
                            </div> Twitter</a>
                    </li>
                    <li>
                        <a href="https://youtube.com">
                            <div>
                                <img src="../img/category/youtube-logo-white.png" alt="Youtube">
                            </div> Youtube</a>
                    </li>
                    <li>
                        <a href="https://vimeo.com">
                            <div>
                                <img src="../img/category/vimeo-logo-white.png" alt="Vimeo">
                            </div> Vimeo</a>
                    </li>
                    <li>
                        <a href="https://pinterest.com">
                            <div>
                                <img src="../img/category/pinterest-logo-white.png" alt="Pinterest">
                            </div> Pinterest</a>
                    </li>
                </ul>
            </section>

            <section class="footer-newsletter">
                <div class="category-head">
                    <h2>Newsletter</h2>
                </div>

                <form>
                    <input type="email" name="mail" placeholder="Email">
                    <button type="submit">Subscribe</button>
                </form>
            </section>

        </div>
        <div class="copyright">&copy; Copyright Gomalthemes 2017, All Rights Reserved</div>
    </div>
    </footer>
    <button onclick="scrollToTop()" id="scroll-to-top" ><img src="../img/arrow-up.png"></button>
