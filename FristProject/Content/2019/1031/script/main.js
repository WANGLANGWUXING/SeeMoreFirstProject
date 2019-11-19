$(function () {
    var startPoint = null;
    // 是否已经领取礼物
    var isReceiveGift = 0;
    /*** 预加载 ***/
    var loader = new createjs.LoadQueue(true);
    loader.on("progress", handleFileLoad);
    loader.on("complete", handleComplete);
    loader.loadManifest(manifest);
    // 用户验证及用户信息记录
    authorize();


    function handleFileLoad(e) {
        var bnum = parseInt(loader.progress * 100);
        document.querySelector('.process span').innerText = bnum + '%';
    }

    function handleComplete() {
        $('.flip-over').css('opacity', 0);
        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/

        // 初始化Swiper
        var mySwiper = new Swiper('#mySwiper', {
            direction: 'vertical',
            effect: 'slide',
            speed: 500,
            noSwiping: true,
            preloadImages: true,
            on: {
                init: function () {
                    swiperAnimateCache(this); //隐藏动画元素 
                    swiperAnimate(this); //初始化完成开始动画
                },
                slideChangeTransitionEnd: function () {
                    swiperAnimate(this); //每个slide切换结束时也运行当前slide动画
                }
            }
        });
        getGiftLog();
        // mySwiper.slideNext();
        // mySwiper.slideTo(0);
        $('.btn-rule').on('click', function () {
            $('#alertRule').show();
        });
        $('.alert-btn-i-know').on('click', function () {
            $('#alertRule').hide();
        });
        $('.input-back-index').on('click', function () {
            mySwiper.slideTo(0);
        });
        $('.btn-to-1').on('click', function () {
            // 食用油
            mySwiper.slideTo(1);
        });
        $('.btn-to-2').on('click', function () {
            // 有机果蔬
            mySwiper.slideTo(2);
        });
        $('.btn-to-3').on('click', function () {
            // 星巴克
            mySwiper.slideTo(3);
        });
        $('.btn-to-4').on('click', function () {
            // 鲜花
            mySwiper.slideTo(4);
        });
        $('.alert-tel').on('click', function () {
            window.location.href = 'tel:02786859999';
        });

        // 提交表格
        $('.input-submit').on('click', function () {
            // 判断是否已经领取
            if (isReceiveGift === 0) {
                var thisInput = $(this).prev().children().val();// 电话号码
                //var prizeImg = '../Content/2019/1031/images/page' + $(this).attr('num') + '/alert.png';
                console.log(thisInput);

                if (thisInput) {
                    receiveGift(thisInput, $(this).attr('num'));
                }
            } else {
                $('#alertResult').show();
            }
           
            //// 提交用户信息之后，弹出奖品
            //$('#prizeSrc').attr('src', prizeImg);
            //$('#alertGift').show();
        });
        $('.alert-closed').on('click', function () {
            $('#alertGift').hide();
        });
        $('.alert-closed-result').on('click', function () {
            $('#alertResult').hide();
        });
        $(".alert-btn-my").on('click', function () {
            $('#alertResult').hide();
            $('#alertGift').show();
        });
    }

    // 手机屏幕的判断滑动
    function phoneTouch(pageObj, callback, arrow) {
        pageObj.addEventListener("touchstart", function (e) {
            e = e || window.event;
            startPoint = e.touches[0];
        });
        pageObj.addEventListener("touchend", function (e) {
            e = e || window.event;
            //e.changedTouches能找到离开手机的手指，返回的是一个数组
            var endPoint = e.changedTouches[0];
            //计算终点与起点的差值
            var x = endPoint.clientX - startPoint.clientX;
            var y = endPoint.clientY - startPoint.clientY;
            //设置滑动距离的参考值
            var d = 10;
            if (Math.abs(x) > d) {
                if (x > 0) {
                    console.log("向右滑动");
                } else {
                    console.log("向左滑动");
                }
            }
            if (Math.abs(y) > d) {
                if (y > 0) {
                    console.log("向下滑动");
                } else {
                    console.log("向上滑动");
                }
            }
        });
    }

    function toast(txt, time) { // 提示信息弹出
        var dom = $('.alerts');
        if (dom.length < 1) { return; }
        dom.empty();
        dom.text(txt);
        dom.fadeIn('500').delay(time).fadeOut('500');
    }

    function confirm(title, txts, time, trueFun, falseFun) { // 弹出选择框
        var appendDom = '<div class="alert-wrap">\
                <h6 id="alertTitile" class="alert-title"></h6>\
                <div id="alertContent" class="alert-content"></div>\
                <div class="alert-btn-flex">\
                  <input id="alertFalse" type="button" value="取消" />\
                  <input id="alertTrue" type="button" value="确定" />\
                </div>\
              </div>';
        var dom = $('#alertConfirm')
        dom.append(appendDom);
        if (title.length < 1) { title = '' };
        if (txts.length < 1) { txts = '是否取消？' };
        $('#alertTitile').text(title);
        $('#alertContent').text(txts);
        dom.fadeIn(time);
        $('#alertTrue').one('click', function () {
            dom.fadeOut(time);
            trueFun();
        });
        $('#alertFalse').one('click', function () {
            dom.fadeOut(time);
            falseFun();
        });
    }


    function authorize() {
        $.ajax({
            type: "post",
            url: "/SeeMore/GetUserInfo",
            dataType: "json",
            async: true,
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);

                if (data.id === "1") {
                    addUser(data.user.Openid, data.user.Nickname);
                }
                //alert(data);

            }
        });
    }

    function addUser(openid, nickname) {
        $.ajax({
            //以post方式发送代码
            type: "post",
            //目标路径
            url: "/SeeMore/AddUser",
            //表名数据格式是json
            dataType: "json",
            data: {
                //已经申明了数据格式，就用那种数据格式
                openid: openid,
                nickname: nickname
            },
            //是否异步
            async: true,
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);
                //alert(data);
                return data;

            }
        });
    }
    // 获取礼物
    function receiveGift(tel,giftCode) {
        $.ajax({
            //以post方式发送代码
            type: "post",
            //目标路径
            url: "/SeeMore/ReceiveGift",
            //表名数据格式是json
            dataType: "json",
            data: {
                telephone: tel,
                giftCode: giftCode
            },
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);
                var prizeImg;
                //alert(data);
                // 礼物没有领取过
                if (data.id === "1") {
                    prizeImg = '//weixin.seemoread.com/Content/2019/1031/images/page' + giftCode + '/alert.png';
                    $('#prizeSrc').attr('src', prizeImg);
                    $('.alert-num').html("ID: "+data.alertCode);
                    $('#alertGift').show();
                    isReceiveGift = 1;
                    //礼物领取过
                } else if (data.id === "2") {
                    prizeImg = '//weixin.seemoread.com/Content/2019/1031/images/page' + data.giftCode + '/alert.png';
                    $('#prizeSrc').attr('src', prizeImg);
                    $('.alert-num').html("ID: " + data.alertCode);
                    $('#alertGift').show();
                    isReceiveGift = 1;
                }


            }
        });
    }

    // 获取当前用户获取礼物的记录
    function getGiftLog() {
        $.ajax({
            //以post方式发送代码
            type: "post",
            //目标路径
            url: "/SeeMore/GetGiftLog",
            //表名数据格式是json
            dataType: "json",
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);
                var prizeImg;
                //alert(data);
                if (data.id === "1") {
                    //给电话号码输入框输入内容
                    $(".input>input[type=tel]").val(data.tel);
                    prizeImg = '//weixin.seemoread.com/Content/2019/1031/images/page' + data.msg + '/alert.png';
                    $('#prizeSrc').attr('src', prizeImg);
                    $('.alert-num').html("ID: " + data.alertCode);
                    $('#alertGift').show();
                    isReceiveGift = 1;
                } 
            }
        });
    }


    /**  musicButton  **/
    $('.music').on('click', function () {
        $(this).toggleClass('music-ani');
        if ($(this).hasClass('music-ani')) {
            document.getElementById('music').play();
        } else {
            document.getElementById('music').pause();
        }
    });

    /**  musicButton for weiXin **/
    document.addEventListener("WeixinJSBridgeReady", function () {
        document.getElementById('music').play();
    }, false);

}); // $-over