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

                                if (data.giftLogs[i].GiftName == "红包0.88") {
                                    giftNameStr = "0.88元红包一个";
                                } else if (data.giftLogs[i].GiftName == "红包1.88") {
                                    giftNameStr = "1.88元红包一个";
                                } else if (data.giftLogs[i].GiftName == "红包8.88") {
                                    giftNameStr = "8.88元红包一个";
                                } else if (data.giftLogs[i].GiftName == "红包88.88") {
                                    giftNameStr = "88.88元红包一个";
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
                if (g_Tel.length > 5) {
                    $("#alertLogin").hide();
                    $("#alertLoginTips").show();
                }
                
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


        

        //$("#alertTips1").on('hide');

        $('.center').on('touchstart', function () {
            $('.center1,.center3,.center4,.center5').addClass('startAni')
            setTimeout(() => {
                $('.bottom2').removeClass('bottom2').addClass('bottom');
            }, 1500)
            //$('.bottom2')

        })
        $("#alertShare").on('click', function () {
            $(this).fadeOut();
        })

        $('.center').on('touchend', function () {
            //var num = Math.ceil(Math.random() * 8);
            //$("#resultImg").attr("src", "../Content/2020/0109/images/share/" + num + ".png");
            //$('#result').show();
            shareWxImgFun();

            $.ajax({
                url: "http://weixin.seemoread.com/ZGTJJYC/PrizeDraw",
                dataType: 'json',
                data: { openId },
                success: function (data) {
                    console.log(data);
                    if (data.id == 2 || data.id == 5) {
                        if (data.gift.GiftDesc == "奖金") {
                            if (data.gift.GiftName == "红包0.88") {
                                $(".alert-title-content").html("0.88元红包一个");
                            } if (data.gift.GiftName == "红包1.88") {
                                $(".alert-title-content").html("1.88元红包一个");
                            } if (data.gift.GiftName == "红包8.88") {
                                $(".alert-title-content").html("8.88元红包一个");
                            } if (data.gift.GiftName == "红包88.88") {
                                $(".alert-title-content").html("88.88元红包一个");
                            } 
                        } else {
                            $(".alert-title-content").html(data.gift.GiftName + "一" + data.gift.Unit);
                        }
                        $("#alertTips1").show();
                        if (data.id == 5) {
                            setTimeout(() => {
                                $("#alertShare").show();
                            }, 6000)
                        }
                    }

                    if (data.id == 4) {
                        setTimeout(() => {
                            $("#alertShare").show();
                        }, 1500)
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
    function shareWxImgFun() {

        //wxImgSrc = 'http://thirdwx.qlogo.cn/mmopen/vi_32/poo31O8GibesWht9uNZ6kuhia3riaXfOQ52DXb1czp3WpT2ARWAjEo9aSIOnzFsFQKpNIKBuU6r22Q5VsIT0Ogu8Q/132'
      


        var img = new Image();
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');
        document.getElementById('PageCanvasImg').appendChild(img);
        document.getElementById('PageCanvasImg').appendChild(canvas);

        //直接读成blob文件对象
        var xhr = new XMLHttpRequest();
        xhr.open('get', wxImgSrc, true);
        xhr.responseType = 'blob';
        xhr.onload = function () {
            if (this.status == 200) {
                //这里面可以直接通过URL的api将其转换，然后赋值给img.src
                img.src = URL.createObjectURL(this.response);
                img.setAttribute('crossOrigin', 'anonymous');
                img.onload = function () {
                    canvas.width = img.width;
                    canvas.height = img.height;
                    canvas.style.display = 'none'
                    ctx.drawImage(img, 0, 0);
                    wxImgSrc = canvas.toDataURL("image/jpeg", 1);
                    img.remove();
                    canvas.remove();
                    // 创建Pixi-Canvas
                    PixiCanvas();
                }
            }
        };
        xhr.send();

        $('.alert').hide();
        //$('#wxShareImg').show();
        $('#result').show();

    }

    function PixiCanvas() {
        var app = new PIXI.Application({
            width: 750,
            height: 1206,
            antialias: true,
            transparent: true,
            resolution: 1,
            preserveDrawingBuffer: true
        });
        const style = new PIXI.TextStyle({
            fontFamily: 'Microsoft YaHei',
            fontSize: 28,
            fontWeight: 'bold',
            fill: ['#000', '#555'],
            wordWrap: true,
        });

        var message = new PIXI.Text(wxUserName, style);
        message.x = 387;
        message.y = 958;

        var num = Math.ceil(Math.random() * 8);

        PIXI.loader
            .add("../Content/2020/0109/images/share/share-" + num + ".png")
            .add("../Content/2020/0109/images/share/wx2wm.png")
            .add(wxImgSrc)
            .load(function () {
                //Create the dom sprite
                let bgDom = new PIXI.Sprite(PIXI.loader.resources["../Content/2020/0109/images/share/share-" + num + ".png"].texture);
                let headDom = new PIXI.Sprite(PIXI.loader.resources[wxImgSrc].texture);
                let wx2wmDom = new PIXI.Sprite(PIXI.loader.resources["../Content/2020/0109/images/share/wx2wm.png"].texture);
                //Add the dom to the stage
                bgDom.width = 750;
                bgDom.height = 1206;
                headDom.width = 40;
                headDom.height = 40;
                wx2wmDom.width = 108;
                wx2wmDom.height = 108;
                headDom.x = 332;
                headDom.y = 955;
                wx2wmDom.x = 200;
                wx2wmDom.y = 955;

                app.stage.addChild(bgDom);
                app.stage.addChild(headDom);
                app.stage.addChild(wx2wmDom);
                app.stage.addChild(message);

                app.render(app.stage);

                document.getElementById('PageCanvasImg').src = app.renderer.plugins.extract.image().src;
            });
        app.view.style.width = 375 + 'px';
        app.view.style.height = 603 + 'px';
        document.getElementById('PageCanvasImg').appendChild(app.view);
    }
}) // $-over