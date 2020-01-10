var g_GiftId = 0;
var g_GiftStr = "";
var g_Name = "";
var g_Tel = "";
function Reg(giftid,giftStr) {
    
    console.log(giftid);
    g_GiftId = giftid;
    g_GiftStr = giftStr;
    $(".alert-title-content").html(g_GiftStr);
    $("#alertMyPrize").hide();
    $("#alertLogin").show();
}
$(function () {

    /*** 预加载 ***/
    var loader = new createjs.LoadQueue(true);
    loader.on("progress", handleFileLoad);
    loader.on("complete", handleComplete);
    loader.loadManifest(manifest);

    function handleFileLoad(e) {
        var bnum = parseInt(loader.progress * 100);
        document.querySelector('.process span').innerText = bnum + '%';
        document.querySelector('.progress-front').style.width = bnum + '%';
    }

    function handleComplete() {
        $('#loading').fadeOut(200).delay(500).remove();
        /*** 初始化完毕-   正式代码开始： ***/
        $('.btn-start').on('click', function () {
            $('#index1').hide();
            $('#index2').show()
        })
        $('.btn-rule').on('click', function () {
            $(".rule-img").css("background-image", "url(../Content/2020/0109/images/alert/rule.png?v=" + + Math.floor(Math.random() * 1000) + ")");
            $('#alertRule').show();
        })

        $(".tips-btn-back").on('click', function () {
            $('.alert').hide();
            $('#index2').hide();
            $('#result').hide();
            $('#index1').show();
        })


        $('.btn-my-prize').on('click', function () {
            
            $.ajax({
                url: "http://weixin.seemoread.com/ZGTJJYC/GetMyGift",
                dataType: 'json',
                data: { openId },
                success: function (data) {
                    console.log(data);
                    // 已登记
                    if (data.flag == 1) {
                        //alert("已登记");
                        if (data.giftLogs[0].GiftName == "红包1") {
                            g_GiftStr = "1元红包一个";
                        } else if (data.giftLogs[0].GiftName == "红包8") {
                            g_GiftStr = "8元红包一个";
                        } else {
                            g_GiftStr = data.giftLogs[0].GiftName + "一" + data.giftLogs[0].Unit;
                        }
                        $(".alert-title-content").html(g_GiftStr);
                        if (g_GiftStr.indexOf("红包") != -1) {
                            $("#alertRedBag").show();

                        } else {
                            $("#alertGift").show();

                        }

                    } else {
                        if (data.giftLogs.length > 0) {
                            var htmlStr = "";
                            for (var i = 0; i < data.giftLogs.length; i++) {
                                var giftNameStr = "";

                                if (data.giftLogs[i].GiftName == "红包1") {
                                    giftNameStr = "1元红包一个";
                                } else if (data.giftLogs[i].GiftName == "红包8") {
                                    giftNameStr = "8元红包一个";
                                } else {
                                    giftNameStr = data.giftLogs[i].GiftName + "一" + data.giftLogs[i].Unit;
                                }
                                htmlStr += "<div class=\"alert-prize-item\">" +
                                    "<div class=\"left-item\">" +
                                    giftNameStr + "</div ><div class=\"right-item\"  onclick='Reg(" + data.giftLogs[i].GiftId + ",\"" + giftNameStr+"\")'></div></div >";
                            }
                            
                            $(".alert-prize-has").html(htmlStr);
                            $("#alertMyPrize").show();
                        }
                        else {
                            $('#alertMyPrizeEmpty').show()
                        }
                    }

                }
            });

        })

        $(".alert-login-submit").on('click', function () {
            g_Name = $(".input-name").val();
            g_Tel = $(".input-tel").val();
            if (g_Name || g_Tel) {
                $("#alertLogin").hide();
                $("#alertLoginTips").show();
            }
        })

        $(".login-tips-true").on('click', function () {
            // 否
            $("#alertLoginTips").hide();
            $("#alertLogin").show();
        })

        $(".login-tips-false").on('click', function () {
            // 是
            if (g_Name || g_Tel) {

                $.ajax({
                    url: "http://weixin.seemoread.com/ZGTJJYC/JYC20200109Reg",
                    dataType: 'json',
                    data: {
                        openId,
                        giftId: g_GiftId,
                        name: g_Name,
                        telphone: g_Tel
                    },
                    success: function (data) {
                        console.log(data);
                        if (data.id == 1) {
                            // 登记
                            $("#alertLoginTips").hide();
                            if (g_GiftStr.indexOf("红包") != -1) {
                                $("#alertRedBag").show();

                            } else {
                                $("#alertGift").show();
                            }
                        } else {
                            alert("礼物没有了");
                        }

                    }
                });


            }
            
        })

        $('.alert-close').on('click', function () {
            $('.alert').hide();
            //console.log($(this).parents("#alertTips1"));
            //if ($(this).parents("#alertTips1").length > 0) {
            //    $('#alertShare').show();
            //}

        })


        $('#result').on('touchend', function () {

            $('#alertShare').show();
        })

        //$("#alertTips1").on('hide');

        $('.center').on('touchstart', function () {
            $('.center1,.center3,.center4,.center5').addClass('startAni')
            $('.bottom').removeClass('hide')
        })


        $('.center').on('touchend', function () {
            var num = Math.ceil(Math.random() * 8);
            $("#resultImg").attr("src", "../Content/2020/0109/images/share/" + num + ".png");
            $('#result').show();
            $.ajax({
                url: "http://weixin.seemoread.com/ZGTJJYC/PrizeDraw",
                dataType: 'json',
                data: { openId },
                success: function (data) {
                    console.log(data);
                    if (data.id == 2 || data.id == 5) {
                        if (data.gift.GiftDesc == "奖金") {
                            if (data.gift.GiftName == "红包1") {
                                $(".alert-title-content").html("1元红包一个");
                            } else {
                                $(".alert-title-content").html("8元红包一个");
                            }
                        } else {
                            $(".alert-title-content").html(data.gift.GiftName + "一" + data.gift.Unit);
                        }
                        $("#alertTips1").show();
                    }

                }
            });
        })

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