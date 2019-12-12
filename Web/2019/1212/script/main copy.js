$(function () {
    var startPoint = null;
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
        document.querySelector('.progress-p').style.height = bnum + '%';
    }

    function handleComplete() {
        $('#loading').fadeOut(200).delay(500).remove();
        var gameBg = document.getElementById('gameBgImg')
        /*** 初始化完毕-   正式代码开始： ***/

        $('.hourse-persimmon').on('click', function () {
            $('#PageMain').hide();
            $('#PageChoice').show();
        })

        $('.hourse-persimmon').on('click', function () {
            $('#PageMain').hide();
            $('#PageChoice').show();
        })

        $('.alert-btn-back').on('click', function () {
            $('.alert').hide();
        })

        $('.btn-rule').on('click', function () {
            $('#ruleAlert').show();
        })

        $('.btn-my-prize').on('click', function () {

            getMyPriceInfo();



        })
        var type;
        var isChoice = 0;
        var index = 0;
        $('.choice-1,.choice-2').on('click', function () {
            //console.log(this, "choice");
            var cls = $(this).attr('class');
            if (cls === 'choice-1') {
                type = '109';
            } else {
                type = '120';
            }

            if (index === 0) {
                $('#gameBgImg').attr('src', './images/room/' + type + '-img1.jpg');
                isChoice = 0;
            }else if (index === 1) {
                $('#gameBgImg').attr('src', './images/room/' + type + '-img2.jpg');
                isChoice = 0;
            } else if (index === 2) {
                $('#gameBgImg').attr('src', './images/room/' + type + '-img3.jpg');
                isChoice = 0;
            } else if (index === 3) {
                $('#gameBgImg').attr('src', './images/room/' + type + '-img4.jpg');
                isChoice = 0;
            }

            $(".begin-bg").attr('src', './images/page3-img-' + type + '.png');
            $(".begin-bg").css('background-image', 'url(./images/page3-img-' + type + '.png)');
            console.log(type)
            $('#PageChoice').hide();
            $('#PageBegin').show();
        });

        $('.btn-begin').on('click', function () {
            $('#PageBegin').hide();
            $('#PageGame').show();
            document.getElementById('gameBg').style.width = gameBg.clientWidth + 'px';
            document.getElementById('gameBg').style.left = (gameBg.clientWidth / 2 - window.innerWidth / 2) * -1 + 'px';
            $('#gameChoice').addClass('pos-'+type+'-'+index);
        });

        $('.game-btn-back').on('click', function () {
            $('#PageGame').hide();
            $('#PageBegin').show();
            
        });

        $('.game-tips').on('click', function () {
            $('.game-tips').hide();
        });
        
        $('.btn-next').on('click', function () {
            // 去下一个房间
            console.log(index);
            $('#gameBgImg').attr('src', './images/room/' + type + '-img'+ (index+1) +'.jpg');
            $('#gameChoice').addClass('pos-'+type+'-'+index);
            isChoice = 0;
        });
        $('.btn-back-index').on('click', function () {
            $('#PageBegin').show();
            $('#PageGame').hide();
        })

        $('#gameChoice').on('click', function () {
            if (isChoice === 0) {
                isChoice = 1;
                $(".game-bottom").children(".persimmon").eq(index).removeClass("empty");
                index++;
            }
            if (index == 4) {
                $('#givePresentAlert').show();
            }
        })
        $('.present-out').on('click', function () {
            // 获取礼物
            $('#givePresentAlert').hide()
            getPrice();
        })
        $('.alert-btn-login-present').on('click', function () {
            $('#showPresentAlert').hide()
            $('#loginAlert').show()
        })
        $('.alert-btn-submit').on('click', function () {
            var user = $("#user-txt").val();
            var tel = $("#tel-txt").val();
            if (!!user && !!tel) {
                $.ajax({
                    url: "http://weixin.seemoread.com/HXC/AddRegInfo",
                    dataType: 'json',
                    data: { openId: OpenId, name: user, tel },
                    success: function (data) {
                        console.log(data);
                        if (data.id == 1) {
                            $("#user-txt").removeAttr("disabled");
                            $("#tel-txt").removeAttr("disabled");

                            $('#PageGame').hide()
                            $('#loginAlert').hide()
                            $('#PageChoice').show()
                        } else {
                            alert(data.msg);
                        }
                    }
                });
            }


        })

        arrow(document.getElementById('gameBg'));
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
                    data: { code, url: "http://wx.seemoread.com/2019/1123/" },
                    success: function (data) {
                        console.log(data);
                        if (data !== null) {
                            setCookie("OpenId", data.Openid, 30);
                            OpenId = data.Openid;
                        }
                    }
                });
            } else {
                window.location.href = 'http://weixin.seemoread.com/seemore/MyAuthorization?url=http://wx.seemoread.com/2019/1123/';
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


    function getPrice() {
        $.ajax({
            url: "http://weixin.seemoread.com/HXC/GetPrice1125",
            dataType: 'json',
            data: { openId: OpenId },
            success: function (data) {
                console.log(data);
                // 0 没有奖品
                if (data.id == 0) {
                    // 没有奖品，显示一个默认奖品
                    $('#showPresentAlert').show();
                    //$("#hadEndPresentAlert").show();
                }
                // 1 这次抽中了
                else if (data.id == 1) {
                    if (data.gitId == 32) {
                        $(".show-present-img").attr("src", "./images/alert-prize2.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data.gitId == 33) {
                        $(".show-present-img").attr("src", "./images/alert-prize1.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data.gitId == 34) {
                        $(".show-present-img").attr("src", "./images/alert-prize3.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data.gitId == 35) {
                        $(".show-present-img").attr("src", "./images/alert-prize4.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize4-txt.png");
                    }
                    $('#showPresentAlert').show()
                }

                // 2 抽过了但是没有登记
                else if (data.id == 2) {
                    if (data.gitId == 32) {
                        $(".show-present-img").attr("src", "./images/alert-prize2.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data.gitId == 33) {
                        $(".show-present-img").attr("src", "./images/alert-prize1.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data.gitId == 34) {
                        $(".show-present-img").attr("src", "./images/alert-prize3.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data.gitId == 35) {
                        $(".show-present-img").attr("src", "./images/alert-prize4.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize4-txt.png");
                    }
                    $('#showPresentAlert').show()
                }
                // 3 抽过了并且登记了
                else if (data.id == 3) {
                    if (data.gitId == 32) {
                        $(".show-present-img").attr("src", "./images/alert-prize2.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data.gitId == 33) {
                        $(".show-present-img").attr("src", "./images/alert-prize1.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data.gitId == 34) {
                        $(".show-present-img").attr("src", "./images/alert-prize3.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data.gitId == 35) {
                        $(".show-present-img").attr("src", "./images/alert-prize4.png");
                        $(".show-present-txt").attr("src", "./images/alert-prize4-txt.png");
                    }
                    $("#user-txt").attr("disabled", "disabled");
                    $("#tel-txt").attr("disabled", "disabled");
                    $("#user-txt").val(data.giftLog.Name);
                    $("#tel-txt").val(data.giftLog.Telphone);
                    $('#showPresentAlert').show()
                }


            }
        });
    }

    // 我的奖品
    function getMyPriceInfo() {
        $.ajax({
            url: "http://weixin.seemoread.com/HXC/GetPrice1125",
            dataType: 'json',
            data: { openId: OpenId },
            success: function (data) {
                console.log(data);
                // 0 没有奖品
                if (data.id == 0) {
                    $('#myPrizeEmptyAlert').show();
                }
                // 1 这次抽中了
                else if (data.id == 1) {
                    $('#myPrizeEmptyAlert').show();
                }
                // 2 抽过了但是没有登记
                else if (data.id == 2) {
                    $('#myPrizeEmptyAlert').show();
                }
                // 3 抽过了并且登记了
                else if (data.id == 3) {
                    if (data.gitId == 32) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize2.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data.gitId == 33) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize1.png");
                        $(".alert-my-prize-txt-hast").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data.gitId == 34) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize3.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data.gitId == 35) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize4.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize4-txt.png");
                    }

                    $('#myPrizeHasAlert').show()
                }


            }
        });
    }
    var startPoint = 0;
    var lastX = 0;
    function arrow(pageObj) { // 手机屏幕的判断滑动
        var dom = $('#gameBg');
        pageObj.addEventListener("touchstart", function (e) {
            var e = e || window.event;
            startPoint = e.touches[0];
            lastX = dom.position().left;
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

    });

    /**  musicButton for weiXin **/
    document.addEventListener("WeixinJSBridgeReady", function () {
        document.getElementById('music').play();
    }, false);

}); // $-over