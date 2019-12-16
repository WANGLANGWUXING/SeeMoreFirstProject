var winW = Math.max(document.documentElement.clientWidth, window.innerWidth || 0),
    winH = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
var neckOrg = 0;
var wxImgSrc = 'images/share/head.jpg';
var wxUserName = '张三丰';
var wxOpenId = '';
var user = '';
var tel = '';
$(function () {
    /*** 预加载 ***/
    getUserInfo();
    var loader = new createjs.LoadQueue(true);
    loader.on("progress", handleFileLoad);
    loader.on("complete", handleComplete);
    loader.loadManifest(manifest);

    function handleFileLoad(e) {
        var bnum = parseInt(loader.progress * 100);
        document.querySelector('.process span').innerText = bnum + '%';
        document.querySelector('.progress-color').style.width = bnum + '%';
    }

    function handleComplete() {
        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/

        // 初始化背景高度
        $('#bgChange').css('height', winH)
        neckOrg = Math.floor($('#neckChange').height());
        $('#neckChange').height(neckOrg)

        //输入文字设置为白色
        $("input").css("color", "white");
        // 关闭所有的弹窗事件
        $('.alert-close').on('click', alertCloseClick);
        // 显示规则事件
        $('#btnGameIntroductions').on('click', ruleShowClick);
        // 显示我的奖品
        $('#btnMyPrize').on('click', myPrizeShowClick);
        // 开始游戏按钮
        $('#btnGameStart').on('click', gameStartClick);
        // 开始游戏tip按钮
        $('#alertTipIknow').on('click', alertTipIknowClick);
        // 我的奖品：登记领奖按钮
        $('#alertMyPrizeUnloginToLogin,#alertLoginNow').on('click', alertMyPrizeUnloginToLoginClick);
        // 游戏结束：重新开始按钮
        $('#alertRestart').on('click', alertRestartClick);
        // 登记成功之后我的奖品需要分享
        $('#gameOverYourPrizeSubmit').on('click', gameOverYourPrizeSubmitFun);
        // 微信分享
        $('#alertMyPrizeHaveShare').on('click', shareWxImgFun);
        // 确认登记
        $('#alertSureLoginTrue').on('click', reg);
        // 取消登记
        $("#alertSureLoginFalse").on('click', noReg);

    }



    /*** 预加载 ***/
    var startPoint = 0;
    var lastX = 0;
    function arrow(pageObj) { // 手机屏幕的判断滑动
        pageObj.addEventListener("touchstart", function (e) {
            var e = e || window.event;
            startPoint = e.touches[0];
            lastX = pageObj.offsetTop;
        })
        pageObj.addEventListener("touchmove", function (e) {
            var e = e || window.event;
            //e.changedTouches能找到离开手机的手指，返回的是一个数组
            var endPoint = e.changedTouches[0];
            //计算终点与起点的差值
            var pointX = startPoint.clientX - endPoint.clientX;
            var pointY = endPoint.clientY - startPoint.clientY;
            var preX = lastX + pointX;
            if (preX > 0) {
                pageObj.style.left = 0 + 'px';
                return;
            }
            if (preX < (gameBg.clientWidth - window.innerWidth) * -1) {
                pageObj.style.left = (gameBg.clientWidth - window.innerWidth) * -1 + 'px';
                return;
            }
            pageObj.style.left = preX + 'px';
        })
    }


    function getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) { return pair[1]; }
        }
        return (false);
    }
    function isEmpty (input){
        return input + '' === 'null' || input + '' === 'undefined' || input.trim ? input.trim() === '' : input.replace(/^\s+|\s+$/gm, '') === '';
    };
    function getUserInfo() {
        //wxOpenId = getCookie("OpenId");
        //wxImgSrc = getCookie("ImgSrc");
        //wxUserName = getCookie("User");
        //alert(typeof (wxOpenId));
        //if (wxOpenId.length>0)
        ////if ((wxOpenId !== null && wxOpenId !== "") &&
        ////    (wxImgSrc !== null && wxImgSrc !== "") &&
        ////    (wxUserName !== null && wxUserName !== ""))
        //{
        //    console.log(wxOpenId);
        //    console.log(wxImgSrc);
        //    console.log(wxUserName);
            
        //    alert("Cookie: 这是第一次测试：  wxOpenId:" + wxOpenId + ",wxImgSrc:" + wxImgSrc + ",wxUserName:" + wxUserName)
        //    $.ajax({
        //        url: "http://weixin.seemoread.com/seemore/AddPV",
        //        dataType: 'json',
        //        data: { url: "http://wx.seemoread.com/2019/1212/", openId: wxOpenId },
        //        success: function (data) {

        //        }
        //    });
        //} else {
            var code = getQueryVariable('code');
            if (code) {
                //alert("已经转发"+ code)
                $.ajax({
                    url: "http://weixin.seemoread.com/seemore/MyGetUserInfoByCodeOrUrl",
                    dataType: 'json',
                    data: { code, url: "http://wx.seemoread.com/2019/1212" },
                    success: function (data) {
                        console.log(data);
                        //alert(JSON.stringify(data))
                        if (data !== null) {
                            setCookie("OpenId", data.Openid, 30);
                            setCookie("ImgSrc", data.Headimgurl, 30);
                            setCookie("User", data.Nickname, 30);
                            wxOpenId = data.Openid;
                            wxImgSrc = data.Headimgurl;
                            wxUserName = data.Nickname;
                            //alert("Test2:wxOpenId=" + wxOpenId);
                            if (wxOpenId === ""  || wxOpenId === null) {
                                window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1212';
                            }
                        }
                    }
                });
            } else {
                //alert("开始转发")
                window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1212';

            }
        //}

    }


    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        document.cookie = cname + "=" + cvalue + "; " + expires;
    }

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(name) === 0) { return c.substring(name.length, c.length); }
        }
        return "";
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

}) // $-over