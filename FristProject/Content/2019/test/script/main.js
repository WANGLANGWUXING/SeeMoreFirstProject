$(function () {
    var startPoint = null;
    var g_PrizeNum;
    /*** 预加载 ***/
    var loader = new createjs.LoadQueue(true);
    loader.on("progress", handleFileLoad);
    loader.on("complete", handleComplete);
    loader.loadManifest(manifest);
    authorize();
    function handleFileLoad(e) {
        var bnum = parseInt(loader.progress * 100);
        document.querySelector('.process span').innerText = bnum + '%';
    }

    function handleComplete() {

        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/

        var _redux = $('#redux');
        var PrizeNum = 0;
        var hasLogin = false, hasPrize = false;
        var Scroll1;

        // 最开始就请求中什么奖
        PrizeNum = 0;
        document.getElementById('robot').src = '../Content/2019/test/images/prize/' + PrizeNum + '.png';
        //switch (PrizeNum) {
        //    case 1:
        //        document.getElementById('showPrizeInfo').innerHTML = '恭喜获得<br/>碗具套装1套';
        //        jQuery('#show2wm').qrcode({ width: 120, height: 120, text: "http://wx.seemoread.com/2019/0906/index2.html" });
        //        // Prize2wm = $('#show2wm').qrcode({width: 120,height: 120,text: "http://wx.seemoread.com/2019/0906/index2.html"});
        //        // var _prize2wm = Prize2wm.find('canvas').get(0);
        //        // console.log(_prize2wm.toDataURL('image/webp'))
        //        break;
        //    case 2:
        //        document.getElementById('showPrizeInfo').innerHTML = '恭喜获得<br/>精致玩偶1个';
        //        jQuery('#show2wm').qrcode({ width: 120, height: 120, text: "http://wx.seemoread.com/2019/0906/index2.html" });
        //        break;
        //    case 3:
        //        document.getElementById('showPrizeInfo').innerHTML = '恭喜获得<br/>环保购物袋1个';
        //        jQuery('#show2wm').qrcode({ width: 120, height: 120, text: "http://wx.seemoread.com/2019/0906/index2.html" });
        //        break;
        //    case 4:
        //        document.getElementById('showPrizeInfo').innerHTML = '恭喜获得<br/>定制公交卡1张';
        //        jQuery('#show2wm').qrcode({ width: 120, height: 120, text: "http://wx.seemoread.com/2019/0906/index2.html" });
        //        break;
        //    case 0:
        //        document.getElementById('showPrizeInfo').innerHTML = '活动已结束<br/>更多活动敬请期待！';
        //        jQuery('#show2wm').qrcode({ width: 120, height: 120, text: "活动已结束，更多活动敬请期待！" });
        //        break;
        //    default:
        //        break;
        //}

        _redux.eraser({
            size: 10, //设置橡皮擦大小
            completeRatio: .5, //设置擦除面积比例
            progressFunction: function (p) {
                if (Math.round(p * 100) > 30) {
                    $('#redux').eraser('clear');
                    hasPrize = true;

                    if (g_PrizeNum === "3") {
                        fhb();

                    }

                    setTimeout(function () {
                        //_winLogin.show();

                    }, 1000)
                }
            }
        });
        // $redux.eraser('disable');
        // $redux.eraser('enable');
        getPrice();
    } // 初始化完毕-over

    ///***  抽奖效果  ***/
    function startRoll(event) {
        num = event.data.prize - 1
        var dom = document.getElementById('rollUp')
        var _prize = [0, 35, 70, 106, 142, 177, 210, 247, 290, 327]
        TweenMax.to(dom, 3, {
            rotation: 1080 + _prize[num], onComplete: function () {
                $('#login').show()
                document.getElementById('showPrizeInfo').src = './images/prize/' + event.data.prize + '.png'
                document.getElementById('showPrize').src = './images/2wm-' + event.data.prize + '.png'
            }
        });
    }

    ///***  文字滚动动效  ***/
    function roll(t) {
        var ul1 = document.getElementById("comment1");
        var ul2 = document.getElementById("comment2");
        var ulbox = document.getElementById("review_box");
        ul2.innerHTML = ul1.innerHTML;
        ulbox.scrollTop = 0; // 开始无滚动时设为0
        setInterval(function () {
            // 正常滚动不断给scrollTop的值+1,当滚动高度大于列表内容高度时恢复为0
            if (ulbox.scrollTop >= ul1.scrollHeight) {
                ulbox.scrollTop = 0;
            } else {
                ulbox.scrollTop++;
            }
        }, t);
    }

    ///**  提示信息弹出  **/
    function toast(txt, time) {
        var dom = $('.alerts');
        if (dom.length < 1) { return; }
        dom.empty();
        dom.text(txt);
        dom.fadeIn('500').delay(time).fadeOut('500');
    }

    ///**  弹出选择框  **/
    function confirm(title, txts, time, trueFun, falseFun) {
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


    function getPrice() {
        $.ajax({
            //以post方式发送代码
            type: "post",
            //目标路径
            url: "/SeeMore/GetPrize2",
            //表名数据格式是json
            dataType: "json",
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);
                g_PrizeNum = data.id;
                //alert(data);
                if (data.id === "0") {
                    document.getElementById('robot').src = '../Content/2019/test/images/prize/1.png';
                } else {
                    document.getElementById('robot').src = '../Content/2019/test/images/prize/' + data.id + '.png';

                }


            }
        });
    }


    function fhb() {
        $.ajax({
            //以post方式发送代码
            type: "post",
            //目标路径
            url: "/SeeMore/FHB",
            //表名数据格式是json
            dataType: "html",
            //是否异步
            async: true,
            //数据成功返回后的操作，就是局部改变动态页面
            success: function (data) {
                console.log(data);
                //alert(data);
                //return data;

            }
        });
    }

    /**  musicButton  **/
    $('.music').on('click', function () {
        $(this).toggleClass('music-ani');
        if ($(this).hasClass('music-ani')) {
            document.getElementById('music').play()
        } else {
            document.getElementById('music').pause()
        }
    })

    /**  musicButton for weiXin **/
    document.addEventListener("WeixinJSBridgeReady", function () {
        document.getElementById('music').play();
    }, false);



}); // $-over

