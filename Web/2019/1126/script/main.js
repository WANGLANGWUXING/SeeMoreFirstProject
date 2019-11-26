$(function () {
    var OpenId;
    getUserInfo();
    /*** 预加载 ***/
    var loader = new createjs.LoadQueue(true);
    loader.on("progress", handleFileLoad);
    loader.on("complete", handleComplete);
    loader.loadManifest(manifest);

    function handleFileLoad(e) {
        var bnum = parseInt(loader.progress * 100);
        document.querySelector('.process span').innerText = bnum + '%';
    }

    function handleComplete() {
        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/

        $('.alert-btn-ok').on('click', function () {
            $('.alert').hide();
        });

        // 提交表格
        $('#btnSubmit').on('click', function () {
            var user = $("#user-txt").val();
            var tel = $("#tel-txt").val();
            var idCard = $("#idCard-txt").val();
            var area = $("#area-txt").val();
            var referrer = $("#referrer-txt").val();

            if (!!user && !!tel && !!idCard && !!area && !!referrer) {
                $.ajax({
                    type: "post",
                    url: "http://weixin.seemoread.com/ZGTJZYC/AddVIP",
                    dataType: "json",
                    data: {
                        openId: OpenId,
                        name: user,
                        tel,
                        idCard,
                        area,
                        referrer
                    },
                    success: function (data) {
                        console.log(data);

                        if (data.id == 1) {
                            $("#alert-ok").show();
                        } else {
                            $("#alert-err").show();
                        }
                    }
                });
            }
            
        });

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

    function getUserInfo() {
        OpenId = getCookie("OpenId");
        if (OpenId !== "") {
            console.log(OpenId);
        } else {
            var code = getQueryVariable('code');
            if (code) {
                $.ajax({
                    url: "http://weixin.seemoread.com/seemore/MyGetUserInfoByCodeOrUrl",
                    dataType: 'json',
                    data: { code, url: "http://wx.seemoread.com/2019/1125" },
                    success: function (data) {
                        console.log(data);
                        if (data !== null) {
                            setCookie("OpenId", data.Openid, 30);
                            OpenId = data.Openid;
                        }
                    }
                });
            } else {
                window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1125/';

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