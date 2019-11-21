<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>--%>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>静谧时光机——寻找另一个空间的你</title>

    <!--http://www.html5rocks.com/en/mobile/mobifying/-->
    <meta name="viewport"
        content="width=device-width,user-scalable=no,initial-scale=1, minimum-scale=1,maximum-scale=1" />

    <!--https://developer.apple.com/library/safari/documentation/AppleApplications/Reference/SafariHTMLRef/Articles/MetaTags.html-->
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent">
    <meta name="format-detection" content="telephone=no">

    <!-- force webkit on 360 -->
    <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />
    <!-- force edge on IE -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="msapplication-tap-highlight" content="no">

    <!-- force full screen on some browser -->
    <meta name="full-screen" content="yes" />
    <meta name="x5-fullscreen" content="true" />
    <meta name="360-fullscreen" content="true" />

    <!-- force screen orientation on some browser -->
    <meta name="screen-orientation" content="<%=orientation%>" />
    <meta name="x5-orientation" content="<%=orientation%>">

    <!--fix fireball/issues/3568 -->
    <!--<meta name="browsermode" content="application">-->
    <meta name="x5-page-mode" content="app">

    <!--<link rel="apple-touch-icon" href=".png" />-->
    <!--<link rel="apple-touch-icon-precomposed" href=".png" />-->

    <link rel="stylesheet" type="text/css" href="style-mobile.da1e2.css" />
    <link rel="stylesheet" type="text/css" href="fixed.60640.css" />


</head>
<body>
    <canvas id="GameCanvas" oncontextmenu="event.preventDefault()" tabindex="0"></canvas>
    <div id="splash">
        <div class="progress-bar stripes">
            <span style="width: 0%"></span>
        </div>
    </div>
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="src/settings.2d2af.js" charset="utf-8"></script>
    <script src="plugins.75290.js" charset="utf-8"></script>
    <script>
        window.lodash = require('lodash')
    </script>
    <%=webDebugger%>
    <script src="main.c0d83.js" charset="utf-8"></script>

    <script type="text/javascript">
        function getQueryString(name) {
            let reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i")
            let r = window.location.search.substr(1).match(reg)
            if (r != null) return unescape(r[2])
            return null
        }
        var code = getQueryString('code');

        if (!code) {
            window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/wl/2019/moon/';
        } else {
            window["wxcode"] = code;
            document.addEventListener("WeixinJSBridgeReady", () => {
                WeixinJSBridge.call('hideToolbar');
                // open web debugger console
                if (typeof VConsole !== 'undefined') {
                    window.vConsole = new VConsole();
                }

                var splash = document.getElementById('splash');
                splash.style.display = 'block';

                var cocos2d = document.createElement('script');
                cocos2d.async = true;
                cocos2d.src = window._CCSettings.debug ? 'cocos2d-js.js' : 'cocos2d-js-min.33343.js';

                var engineLoaded = function () {
                    document.body.removeChild(cocos2d);
                    cocos2d.removeEventListener('load', engineLoaded, false);
                    window.boot();
                };
                cocos2d.addEventListener('load', engineLoaded, false);
                document.body.appendChild(cocos2d);
            });
        }
    </script>
    <script src="share.56167.js"></script>
    <script>
        SHARE.shareOption({
            link: "http://wx.seemoread.com/wl/2019/moon",
            pic: "http://wx.seemoread.com/wl/2019/moon/share.jpg",
            title: "静谧时光机——寻找另一个空间的你",
            desc: "参与互动，百元红包和惊喜好礼等你来拿！",
            success: function () { }
        });
    </script>
</body>
</html>

