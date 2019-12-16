
var wxOpenId = "";
var wxImgSrc = "";
var wxUserName = "";
var initFlag = false;
var shareId = "";
$(function () {
    var Scroll1;
    getUserInfo();
    /*** 预加载 ***/
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
        // 添加完分享人再执行下面的代码
        //if (initFlag == false) {
        //    handleComplete();
        //}
        // 设置分享链接可以在这里设置，设置完再执行下面的代码

        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/

        $('#btnRank').on('click', alertRankShow);
        $('#btnRule').on('click', alertRuleShow);

        $('#btnJZRQ').on('click', btnJZRQFun);

        $('#alertRank .mask').on('click', alertRankClose);
        $('#alertRule .alert-close').on('click', alertRuleClose);
        $('#alertTip .alert-close').on('click', alertTipClose);

    }
    function alertRankShow() {
        $('#alertRank').show();
        Scroll1 = new BScroll('#pageScroll', {
            scrollY: true,
            click: true
        })
    }
    function alertRankClose() {
        $('#alertRank').hide();
        Scroll1.destroy();
    }
    function alertRuleShow() {
        $('#alertRule').show();
    }
    function alertRuleClose() {
        $('#alertRule').hide();
    }
    function btnJZRQFun() {
        $('#alertTip').show();
    }
    function alertTipClose() {
        $('#alertTip').hide();
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


    function init() {
        //alert("wxOpenId:" + wxOpenId + ";wxImgSrc:" + wxImgSrc + ";wxUserName:" + wxUserName);
        // 如果信息没有获取到就再次调用这个函数
        //if (wxOpenId === "" || wxOpenId === null) {
        //    init();
        //}

        alert("window.location.search:" + window.location.search);

        // 开始进入
        // 进入就默认参加
        // 添加助力记录
        $.ajax({
            url: "http://weixin.seemoread.com/HXC/AddShareUser",
            dataType: 'json',
            data: { openId: wxOpenId, img: wxImgSrc, nickName: wxUserName },
            success: function (data) {
                console.log(data);
                //alert(JSON.stringify(data))
                if (data.id == 1 || data.id == 0) {
                    shareId = data.shareId;

                    // 设置分享链接

                    SHARE.shareOption({
                        link: "http://wx.seemoread.com/2019/1212c?shareId=" + shareId,
                        pic: "http://wx.seemoread.com/2019/1212c/share.jpg",
                        title: "圣诞助力",
                        desc: "圣诞助力1",
                        success: function () { }
                    });

                    alert("分享链接的人：" + shareId)

                    initFlag = true;
                }
            }
        });

        var beShareId = getQueryVariable("shareId");

       
        if (!beShareId) {
            alert("没有从任何人的分享链接进来");
            // 界面正常显示
        } else {
            // 如果是从其他人的链接进来的
            alert("从其他人的链接进来的,链接来源：" + beShareId);
            $("#btnRule").addClass("hide");
            $("#btnJZRQ").addClass("hide");
            //为下面两个按钮绑定事件
            $("#btnWTKX").on('click', helpOther);
            // 参与游戏
            $("#btnCYYX").on('click', addNewPage);
            $("#btnWTKX").removeClass("hide");
            $("#btnCYYX").removeClass("hide");
        }
    }

    // 为TA开箱
    function helpOther() {

    }
    // 参与游戏
    function addNewPage() {
        window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1212c';
    }


    function getUserInfo() {
        wxOpenId = getCookie("OpenId");
        wxImgSrc = getCookie("ImgSrc");
        wxUserName = getCookie("User");
        if (wxOpenId != "" && wxOpenId != null) return;
        var code = getQueryVariable('code');
        if (code) {
            //alert("已经转发"+ code)
            $.ajax({
                url: "http://weixin.seemoread.com/seemore/MyGetUserInfoByCodeOrUrl",
                dataType: 'json',
                data: { code, url: "http://wx.seemoread.com/2019/1212c" },
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
                        if (wxOpenId == "" || wxOpenId == null) {
                            window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1212c';
                        }

                        init();
                    }
                }
            });
           
        }
        // 必须要用else 要不然会陷入一个多次调用下面代码的情况
        else {


            if (wxOpenId == "" || wxOpenId == null) {
                window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1212c';

            } else {
                init();
            }

        }


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