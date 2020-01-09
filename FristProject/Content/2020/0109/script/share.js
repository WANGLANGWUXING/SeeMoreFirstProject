(function () {
    function ajax(options) {
        options = options || {};
        options.type = (options.type || "GET").toUpperCase();
        options.dataType = options.dataType || "json";
        var params = formatParams(options.data);

        //创建 - 非IE6 - 第一步
        if (window.XMLHttpRequest) {
            var xhr = new XMLHttpRequest();
        } else { //IE6及其以下版本浏览器
            var xhr = new ActiveXObject('Microsoft.XMLHTTP');
        }

        //接收 - 第三步
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                var status = xhr.status;
                if (status >= 200 && status < 300) {
                    options.success && options.success(xhr.responseText, xhr.responseXML);
                } else {
                    options.fail && options.fail(status);
                }
            }
        }

        //连接 和 发送 - 第二步
        if (options.type == "GET") {
            xhr.open("GET", options.url + "?" + params, true);
            xhr.send(null);
        } else if (options.type == "POST") {
            xhr.open("POST", options.url, true);
            //设置表单提交时的内容类型
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xhr.send(params);
        }
    }
    //格式化参数
    function formatParams(data) {
        var arr = [];
        for (var name in data) {
            arr.push(encodeURIComponent(name) + "=" + encodeURIComponent(data[name]));
        }
        arr.push(("v=" + Math.random()).replace(".", ""));
        return arr.join("&");
    }
    //微信配置
    ajax({
        url: '/SeeMore/GetSignature',
        // 服务器数字签名服务地址   
        method: 'post',
        data: {
            url: location.href.split('#')[0] // 将当前URL地址上传至服务器用于产生数字签名
        },
        success: function (data) {
            var data = eval('(' + data + ')');
            // console.log(data);
            JSSDK(data);
        }
    });
    // 返回了数字签名对象开始配置微信JS-SDK
    function JSSDK(data) {
        wx.config({
            debug: false,
            appId: data.appid,
            timestamp: data.timestamp,
            nonceStr: data.noncestr,
            signature: data.sign,
            jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo', 'onMenuShareQZone', 'startRecord', 'stopRecord', 'onVoiceRecordEnd', 'playVoice', 'pauseVoice', 'stopVoice', 'onVoicePlayEnd', 'uploadVoice', 'downloadVoice', 'chooseImage', 'previewImage', 'uploadImage', 'downloadImage', 'translateVoice', 'getNetworkType', 'openLocation', 'getLocation', 'hideOptionMenu', 'showOptionMenu', 'hideMenuItems', 'showMenuItems', 'hideAllNonBaseMenuItem', 'showAllNonBaseMenuItem', 'closeWindow', 'scanQRCode', 'chooseWXPay', 'openProductSpecificView', 'addCard', 'chooseCard', 'openCard']
        });
    }

    var SHARE = {};
    //自定义分享配置项
    SHARE.shareOption = function (share) {
        var title = share.title || '',
        link = share.link || '',
        desc = share.desc || '',
        pic = share.pic || '',
        success = share.success || '';
        wx.ready(function () {
            wx.onMenuShareTimeline({
                title: title,
                link: link,
                imgUrl: pic,
                success: success
            });

            wx.onMenuShareAppMessage({
                title: title,
                desc: desc,
                link: link,
                imgUrl: pic,
                success: success
            });
        });
    };

    wx.ready(function () {
        wx.hideMenuItems({
            menuList: ['menuItem:share:qq', 'menuItem:share:QZone', 'menuItem:share:email', 'menuItem:openWithSafari', 'menuItem:readMode', 'menuItem:copyUrl', 'menuItem:openWithQQBrowser']
        });
    });

    //隐藏所有非基础按钮
    SHARE.hideAll = function () {
        wx.ready(function () {
            wx.hideAllNonBaseMenuItem();
        });
    };

    //隐藏复制链接按钮
    SHARE.hideCopyUrl = function () {
        wx.ready(function () {
            wx.hideMenuItems({
                menuList: ['menuItem:copyUrl']
            });
        });
    };

    //隐藏分享到朋友圈按钮
    SHARE.hideTimeLine = function () {
        wx.ready(function () {
            wx.hideMenuItems({
                menuList: ['menuItem:share:timeline']
            });
        });
    };

    //隐藏分享给好友按钮
    SHARE.hideAppMessage = function () {
        wx.ready(function () {
            wx.hideMenuItems({
                menuList: ['menuItem:share:appMessage']
            });
        });
    };

    window.SHARE = SHARE;
})(window);