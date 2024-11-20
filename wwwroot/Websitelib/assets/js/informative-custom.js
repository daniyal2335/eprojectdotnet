/*--------------------- Copyright (c) 2021 -----------------------
[Master Javascript]

Template Name: Catering HTML Template 
Version: 1.0.0
Author: Kamleshyadav
Website: https://www.pixelnx.com/
Purchase: http://themeforest.net/user/kamleshyadav

-------------------------------------------------------------------*/
(function($) {
    "use strict";
    /*-----------------------------------------------------
    	Function  Start
    -----------------------------------------------------*/
    var control = {
        initialised: false,
        version: 1.0,
        init: function() {
            if (!this.initialised) {
                this.initialised = true;
            } else {
                return;
            }
            /*-----------------------------------------------------
            	Function Calling
            -----------------------------------------------------*/
            this.mainSlider();
            this.siteLoader();
            this.bookingForm();
        },



        /*-----------------------------------------------------
            Fix Slider
        -----------------------------------------------------*/

        mainSlider: function() {

            function resizeSwiper() {
                $('.main-slider').height($('.swiper-slide').height())
            }
            var menu = ['Slide1', 'Slide2', 'Slide3', 'Slide4', 'Slide5', 'Slide6', 'Slide7', 'Slide8', 'Slide9', 'Slide10', 'Slide11']
            var swiper = new Swiper('.main-slider', {
                parallax: true,
                stretch: 100,
                depth: 100,
                modifier: 1,
                touchThreshold: 100,
                draggable: true,
                slidesPerView: 1,
                spaceBetween: 0,
                loop: false,
                autoplay: false,
                direction: 'vertical',
                mousewheelControl: true,
                keyboardControl: true,
                // grabCursor: true,
                mousewheel: {
                    enable: true
                },
                keyboard: {
                    enabled: true
                },
                allowTouchMove: true,
                autoHeight: true,
                pagination: {
                    el: '.pagination-main-swiper',
                    clickable: true,
                    renderBullet: function(index, className) {
                        return '<span class="' + className + '"><b>' + (menu[index]) + '</b></span>';
                    },
                },
                breakpoints: {
                    0: {
                        slidesPerView: 'auto',
                    },
                    575: {
                        slidesPerView: 'auto',
                    },
                    767: {
                        init: false,
                        freeMode: true,
                        slidesPerView: 'auto',
                        onImagesReady: function(swiper) {
                            resizeSwiper();
                            swiper.onResize();
                        },
                    },
                    991: {
                        init: false,
                        freeMode: true,
                        slidesPerView: 'auto',
                        onImagesReady: function(swiper) {
                            resizeSwiper();
                            swiper.onResize();
                        },
                    },
                    992: {
                        slidesPerView: 1,
                        speed: 2500,
                        rotate: 90,
                    },
                    1200: {
                        slidesPerView: 1,
                        speed: 2500,
                        rotate: 90,
                    },
                },
            });

            var swiper1 = new Swiper('.dishes-swiper', {
                pagination: {
                    el: '.pagination-dishes-swiper',
                    clickable: true,
                },
                paginationClickable: false,
                direction: 'horizontal',
                spaceBetween: 30,
                nested: true,
                slidesPerView: 3,
                allowTouchMove: true,
                breakpoints: {
                    0: {
                        slidesPerView: 1,
                    },
                    575: {
                        slidesPerView: 1,
                    },
                    767: {
                        slidesPerView: 2,
                    },
                    992: {
                        slidesPerView: 2,
                    },
                    1200: {
                        slidesPerView: 3,
                    },
                },
            });

            var testimonials = new Swiper('.testimonials', {
                pagination: {
                    el: '.pagination-testimonials',
                    clickable: true,
                },
                paginationClickable: false,
                direction: 'horizontal',
                spaceBetween: 30,
                nested: true,
                slidesPerView: 1,
                allowTouchMove: true,
            });



            var serviceSlider = new Swiper('.service-swiper', {
                pagination: {
                    el: '.pagination-service-swiper',
                    clickable: true,
                },
                paginationClickable: false,
                direction: 'horizontal',
                spaceBetween: 30,
                nested: true,
                slidesPerView: 3,
                allowTouchMove: true,
                breakpoints: {
                    0: {
                        slidesPerView: 1,
                    },
                    575: {
                        slidesPerView: 1,
                    },
                    767: {
                        slidesPerView: 2,
                    },
                    992: {
                        slidesPerView: 2,
                    },
                    1200: {
                        slidesPerView: 3,
                    },
                },
            });


        },




        /*-----------------------------------------------------
                Site Loader Js
        -----------------------------------------------------*/
        siteLoader: function() {
            jQuery(window).on("load", function() {
                var preLoader = $('.cat-preloader');
                preLoader.fadeToggle(500).fadeOut("slow");
            });

            /** Nav Menu **/
            var count = 0;
            $('.right-menu').on("click", function() {
                if (count == '0') {
                    $('.cat-right-menu-wrapper').addClass('cat-rigth-menu-hide');
                    $('.menu-icon').addClass('clos-btn');
                    count++;
                } else {
                    $('.cat-right-menu-wrapper').removeClass('cat-rigth-menu-hide');
                    $('.menu-icon').removeClass('clos-btn');
                    count--;
                }
            });


        },



        /*-----------------------------------------------------
                Booking Form Submition
        -----------------------------------------------------*/

        bookingForm: function() {
            $('#bookingConfirm').on('click', function() {
                var cdate = $('#cus_date').val();
                var cevent = $('#cus_event').val();
                var cppl = $('#cus_ppl').val();
                var carea = $('#cus_area').val();
                var cname = $('#cus_name').val();
                var cnum = $('#cus_num').val();
                var cemail = $('#cus_email').val();
                var cename = $('#cus_event_name').val();
                var cnote = $('#cus_note').val();
                console.log(cdate + ' ' + cevent + ' ' + cppl + ' ' + carea + ' ' + cname + ' ' + cnum + ' ' + cemail + ' ' + cename + ' ' + cnote);
                $.ajax({
                    type: "POST",
                    url: "informative_booking.php",
                    data: {
                        'username': cname,
                        'userdate': cdate,
                        'usernum': cnum,
                        'uevent': cevent,
                        'uarea': carea,
                        'uemail': cemail,
                        'uppl': cppl,
                        'eventName': cename,
                        'note': cnote,
                    },
                    success: function(msg) {
                        console.log(msg);
                        var full_msg = msg.split("#");
                        if (full_msg[0] == '1') {
                            $('#cus_date').val("");
                            $('#cus_event').val("");
                            $('#cus_ppl').val("");
                            $('#cus_area').val("");
                            $('#cus_name').val("");
                            $('#cus_num').val("");
                            $('#cus_email').val("");
                            $('#cus_event_name').val("");
                            $('#cus_note').val("");
                            $('#err3').html(full_msg[1]);
                        } else {
                            $('#cus_date').val(cdate);
                            $('#cus_event').val(cevent);
                            $('#cus_ppl').val(cppl);
                            $('#cus_name').val(cname);
                            $('#cus_num').val(cnum);
                            $('#cus_email').val(cemail);
                            $('#cus_event_name').val(cename);
                            $('#cus_note').val(cnote);
                            $('#err3').html(full_msg[1]);
                        }
                    }
                });
            });
        },



    };

    control.init();


    $(document).ready(function() {

        //color picker start
        $(window).on("load", function() {

            var colorcode = document.cookie;
            if (colorcode != "") {
                var cname = "colorcssfile";
                var name = cname + "=";
                var cssname = '';
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(name) != -1) {
                        cssname = c.substring(name.length, c.length);
                    }
                }

                if (cssname != '') {
                    var new_style = '../assets/css/informative-color/' + cssname + '.css';
                    $('#color-change').attr('href', new_style);
                }
            }
        });
        //Color Change Script
        $('.colorchange').on("click", function() {
            var name = $(this).attr('id');
            var new_style = '../assets/css/informative-color/' + name + '.css';
            $('#color-change').attr('href', new_style);
        });

        $("#color-switcher .bottom a.settings").on("click", function(e) {
            e.preventDefault();
            var div = $("#color-switcher");
            if (div.css("left") === "-160px") {
                $("#color-switcher").animate({
                    left: "0px"
                });
            } else {
                $("#color-switcher").animate({
                    left: "-160px"
                });
            }
        });
        //color picker end

    });



    /*
    Mouse
    */
    function mousecursor() {
        if ($("body")) {
            const e = document.querySelector(".cat-cursor-inner"),
                t = document.querySelector(".cat-cursor-outer");
            let n, i = 0,
                o = !1;
            window.onmousemove = function(s) {
                o || (t.style.transform = "translate(" + s.clientX + "px, " + s.clientY + "px)"), e.style.transform = "translate(" + s.clientX + "px, " + s.clientY + "px)", n = s.clientY, i = s.clientX
            }, $("body").on("mouseenter", "a, .cursor-pointer", function() {
                e.classList.add("cat-cursor-hover"), t.classList.add("cat-cursor-hover")
            }), $("body").on("mouseleave", "a, .cursor-pointer", function() {
                $(this).is("a") && $(this).closest(".cursor-pointer").length || (e.classList.remove("cat-cursor-hover"), t.classList.remove("cat-cursor-hover"))
            }), e.style.visibility = "visible", t.style.visibility = "visible"
        }
    };

    $(function() {
        mousecursor();
    });




})(jQuery);