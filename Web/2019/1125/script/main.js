
var startPoint = null;
var OpenId;
var type;
getUserInfo();
/*** 预加载 ***/
var loader = new createjs.LoadQueue(true);
loader.on("progress", handleFileLoad);
loader.on("complete", handleComplete);
loader.loadManifest(manifest);

function handleFileLoad(e) {
    var bnum = parseInt(loader.progress * 100);
    document.getElementById('processNum').innerText = bnum + '%';
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
        $('#PageGame').hide()
        $("#PageChoice").show();
    })

    $('.btn-rule').on('click', function () {
        $('#ruleAlert').show();
    })

    $('.btn-my-prize').on('click', function () {
        getMyPriceInfo();
    })


    $('.choice-1,.choice-2').on('click', function () {
        //console.log(this, "choice");
        var cls = $(this).attr('class');
        if (cls === 'choice-1') {
            type = '109';
            window["xxxstartFind"](109)
        } else {
            type = '120';
            window["xxxstartFind"](120)
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
    });


    $('.game-btn-back').on('click', function () {
        $('#PageGame').hide();
        $('#PageBegin').show();

    });

    $('.game-tips').on('click', function () {
        $('.game-tips').hide();
    });


    $('.btn-back-index').on('click', function () {
        $('#PageBegin').show();
        $('#PageGame').hide();
    })

    $('.present-out').on('click', function () {
        // 获取礼物
        $('#givePresentAlert').hide()
        getPrice();
    })

    $('.alert-btn-check-my-prize').on('click', function () {
        $('.alert').hide();
        $('#PageGame').hide()
        $("#PageChoice").show();
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
                data: { openId: OpenId, name: user, tel, type },
                success: function (data) {
                    console.log(data);
                    if (data.id == 1) {
                        //$("#user-txt").removeAttr("disabled");
                        //$("#tel-txt").removeAttr("disabled");

                        $('#PageGame').hide();
                        $('#loginAlert').hide();
                        $('#PageChoice').show();

                        $("#user-txt").val("");
                        $("#tel-txt").val("");
                    } else {
                        alert(data.msg);
                    }
                }
            });
        }


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

function getUserInfo() {
    OpenId = getCookie("OpenId");
    if (OpenId !== "") {
        console.log(OpenId);
        // 添加访问量
        $.ajax({
            url: "http://weixin.seemoread.com/seemore/AddPV",
            dataType: 'json',
            data: { url: "http://wx.seemoread.com/2019/1123/", openId: OpenId },
            success: function (data) {

            }
        });


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
        data: { openId: OpenId, type },
        success: function (data) {
            console.log(data);
            // 0 没有奖品
            if (data.id === 0) {
                $("#hadEndPresentAlert").show();
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

            // 2 用户抽中过同类型的奖品
            else if (data.id == 2) {
                if (!!data.giftLog.Name && !!data.giftLog.Telphone) {
                    $("#hadPresentAlert").show();

                } else {
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
            }
            // 3 抽过了并且两种都抽到了
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
                $('#showPresentAlert').show()
                //$("#hadPresentAlert").show();
            } else if (data.id == 4) {
                $("#hadPresentAlert").show();
            }


        }
    });
}

// 我的奖品
function getMyPriceInfo() {
    $.ajax({
        url: "http://weixin.seemoread.com/HXC/GetPriceInfo",
        dataType: 'json',
        data: { openId: OpenId },
        success: function (data) {
            console.log(data);
            if (data.length == 0) {
                $('#myPrizeEmptyAlert').show();
            }
            else if (data.length == 1) {
                if (!!data[0].Name && !!data[0].Telphone) {
                    var tempGiftId = data[0].GiftId;
                    if (tempGiftId == 32) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize2.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize2-txt.png");
                    } else if (tempGiftId == 33) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize1.png");
                        $(".alert-my-prize-txt-hast").attr("src", "./images/alert-prize1-txt.png");
                    } else if (tempGiftId == 34) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize3.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize3-txt.png");
                    } else if (tempGiftId == 35) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize4.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize4-txt.png");
                    }

                    $('#myPrizeHasAlert').show();
                } else {
                    $('#myPrizeEmptyAlert').show();
                }
            }
            else if (data.length == 2) {
                var allReg = 0;//0 都没有注册 1 都注册了  其他数字 注册的礼物id


                if (!!data[0].Name
                    && !!data[0].Telphone
                    && !!data[1].Name
                    && !!data[1].Telphone
                ) {
                    allReg = 1;
                }
                else if (data[0].Name == null
                    && data[0].Telphone == null
                    && data[1].Name == null
                    && data[1].Telphone == null
                ) {
                    allReg = 0;
                }

                else if (!!data[0].Name && !!data[0].Telphone) {
                    allReg = data[0].GiftId;
                }

                else if (!!data[1].Name && !!data[1].Telphone) {
                    allReg = data[1].GiftId;
                }

                if (allReg == 0) {
                    $('#myPrizeEmptyAlert').show();
                } else if (allReg == 1) {
                    if (data[0].GiftId == 32) {
                        $("#myPriceFirst").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data[0].GiftId == 33) {
                        $("#myPriceFirst").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data[0].GiftId == 34) {
                        $("#myPriceFirst").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data[0].GiftId == 35) {
                        $("#myPriceFirst").attr("src", "./images/alert-prize4-txt.png");
                    }

                    if (data[1].GiftId == 32) {
                        $("#myPriceSecond").attr("src", "./images/alert-prize2-txt.png");
                    } else if (data[1].GiftId == 33) {
                        $("#myPriceSecond").attr("src", "./images/alert-prize1-txt.png");
                    } else if (data[1].GiftId == 34) {
                        $("#myPriceSecond").attr("src", "./images/alert-prize3-txt.png");
                    } else if (data[1].GiftId == 35) {
                        $("#myPriceSecond").attr("src", "./images/alert-prize4-txt.png");
                    }
                    $('#myPrizeHasAlertOf2').show();
                } else {
                    if (allReg == 32) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize2.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize2-txt.png");
                    } else if (allReg == 33) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize1.png");
                        $(".alert-my-prize-txt-hast").attr("src", "./images/alert-prize1-txt.png");
                    } else if (allReg == 34) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize3.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize3-txt.png");
                    } else if (allReg == 35) {
                        $(".alert-my-prize-has").attr("src", "./images/alert-prize4.png");
                        $(".alert-my-prize-txt-has").attr("src", "./images/alert-prize4-txt.png");
                    }

                    $('#myPrizeHasAlert').show()
                }

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



window["xxxgetPresent"] = function () {
    $('#givePresentAlert').show();
}
window["xxxbtnbackIndex"] = function () {
    $('#PageChoice').show();
}