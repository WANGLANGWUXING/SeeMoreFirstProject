
//var wxOpenId = "";
//var wxImgSrc = "";
//var wxUserName = "";
var initFlag = false;
// 当前用户助力编号
var shareId = "";
// 被助力人编号
//var beShareId = "";



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



$(function () {
    var Scroll1;
    $("#PageIndex").css("background-image", "url(/Content/2019/1212C/images/index-top.png?v=" + + Math.floor(Math.random() * 1000) + "), url(/Content/2019/1212C/images/bg.jpg?v=" + + Math.floor(Math.random() * 1000) +")");
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

        // 设置分享链接可以在这里设置，设置完再执行下面的代码
        document.getElementById('music').play();
        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/
        getUserInfo();
        $('#btnRank').on('click', alertRankShow);
        $('#btnRule').on('click', alertRuleShow);

        $('#btnJZRQ').on('click', btnJZRQFun);

        $('#alertRank .alert-close').on('click', alertRankClose);
        $('#alertRule .alert-close').on('click', alertRuleClose);
        $('#alertTip .alert-close').on('click', alertTipClose);

    }
    function alertRankShow() {
        Scroll1 = new BScroll('#pageScroll', {
            scrollY: true,
            click: true
        })
        if (initFlag === true) {
            $('#alertRank').show();
            return;
        }

        $.ajax({
            url: "http://weixin.seemoread.com/HXC/SelHelpRank",
            dataType: 'json',
            success: function (data) {
                console.log(data);
                $("#pageScroll ul").html("");
                var htmlContent = "";
                for (var i = 0; i < data.length; i++) {
                    htmlContent += "<li><div class=\"rank-content-left\"> <span>" + (i + 1)
                        + "</span> <img src=\"../Content/Images/WeiXinHeadimgurl/" + data[i].UserImg
                        + "\" /> <strong>" + data[i].NickName
                        + "</strong> </div > <small>" + (data[i].HelpCount+1)
                        + "</small>  </li >";
                    if (data[i].OpenId === wxOpenId) {
                        $(".rank-user img").attr("src", "../Content/Images/WeiXinHeadimgurl/" + data[i].UserImg);
                        $(".rank-user-name strong").html(data[i].NickName);
                        $(".rank-user-name small").html("第" + (i + 1) + "名");
                        $(".rank-user-score").html((data[i].HelpCount + 1));

                    }

                }
                initFlag = true;
                $("#pageScroll ul").append(htmlContent)
                $('#alertRank').show();
               
            }
        });







    }
    function alertRankClose() {
        $('#alertRank').hide();
        Scroll1.destroy();
    }
    function alertRuleShow() {
        // 为了处理微信
        $(".alert .rule-img").css("background-image", "url(/Content/2019/1212C/images/rule.png?v=" + + Math.floor(Math.random() * 1000)  + ")");
        
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
            if (pair[0] === variable) { return pair[1]; }
        }
        return (false);
    }


    function init() {
        //alert("wxOpenId:" + wxOpenId + ";wxImgSrc:" + wxImgSrc + ";wxUserName:" + wxUserName);

        //alert("window.location.search:" + window.location.search);

        // 开始进入
        // 进入就默认参加
        // 添加助力记录
        $.ajax({
            url: "http://weixin.seemoread.com/HXC/AddShareUser",
            dataType: 'json',
            data: { openId: wxOpenId },
            success: function (data) {
                console.log(data);
                //alert(JSON.stringify(data))
                if (data.id === 1 || data.id === 0) {
                    shareId = data.shareId;

                    // 设置分享链接

                    SHARE.shareOption({
                        link: "http://weixin.seemoread.com/2019/1212C?shareId=" + shareId,
                        pic: "http://weixin.seemoread.com/Content/2019/1212C/share.jpg",
                        title: "圣诞人气王 欢乐开宝箱",
                        desc: "iPad Air、MacBook Pro、HUAWEI Mate 30 Pro等惊喜豪礼等你来拿~",
                        success: function () { }
                    });

                    //alert("分享链接的人：" + shareId)
                    // 还要判断是否是从本人的链接进来的

                    //alert(typeof beShareId);

                    //alert("http://weixin.seemoread.com/2019/1212C?shareId="
                        //+ shareId + ";\n"
                        //+ "beShareId="
                    //+ beShareId + ";\nshareId =" + shareId);
                    $("#PageIndex").css("background-image", "url(/Content/2019/1212C/images/index-top.png), url(/Content/2019/1212C/images/bg.jpg)");

                    if (beShareId == "" || shareId == beShareId ) {
                        // alert("没有从其他人的分享链接进来");
                        // 界面正常显示
                        $.ajax({
                            url: "http://weixin.seemoread.com/HXC/GetHelpCount",
                            dataType: 'json',
                            data: { shareId: shareId},
                            success: function (data) {
                                console.log("GetHelpCount:" + data);
                                $(".index-txt span").html(data+1);
                            }
                        });


                    } else {
                        $.ajax({
                            url: "http://weixin.seemoread.com/HXC/GetHelpCount",
                            dataType: 'json',
                            data: { shareId: beShareId },
                            success: function (data) {
                                console.log("GetHelpCount:" + data);
                                $(".index-txt span").html(data + 1);
                            }
                        });
                        // 如果是从其他人的链接进来的
                       // alert("从其他人的链接进来的,链接来源：" + beShareId);
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
            }
        });
        //alert(beShareId);


    }

    // 为TA开箱
    function helpOther() {
        $.ajax({
            url: "http://weixin.seemoread.com/HXC/AddHelpUser",
            dataType: 'json',
            data: { openId: wxOpenId, shareId: beShareId, url: window.location.href },
            success: function (data) {
                console.log(data);
                //alert(JSON.stringify(data))
                if (data.id === 1) {
                    // 助力成功
                    toast("助力成功")
                    //confirm("温馨提示", "助力成功",500);
                   // alert("助力成功")
                } else if (data.id === 4) {
                    // 助力过了
                    toast("助力过了")
                    //alert("助力过了")
                    //confirm("温馨提示", "助力过了", 500);
                } else if(data.id===99){
                    toast("活动已结束")	
	}else {
                    //alert("助力失败")
                   // alert(JSON.stringify(data))
                    toast("助力失败\n" + JSON.stringify(data))
                    //confirm("温馨提示", "助力失败\n" + JSON.stringify(data), 500);
                }
            }
        });
    }
    // 参与游戏
    function addNewPage() {
        initFlag = false;
        wxOpenId = "";
        wxImgSrc = "";
        wxUserName = "";
        beShareId = "";
        shareId = "";
        window.location.replace('http://weixin.seemoread.com/2019/1212C');
    }




    function getUserInfo() {
        //wxOpenId = getQueryVariable("openId");
        beShareId = getQueryVariable("shareId");
        init();
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