var beforeGameCountDownTime = 0;
var GameCountDownTime = 0;
var beforeGameCountDownTimeInt = null;
var GameCountDownTimeInt = null;
var GameOverBoolean = true;

var heighter = 20,  // 点击增速
    prizeName = '礼品名称', // $('.my-prize-show-name')
    prizeID = '',   // $('.my-prize-id')
    prizeSrc = '';  // $('.my-prize-pic-src')
var scoreNum = 0;   // $('.my-score-show')
var scoreMissFlag = false;
var scoreLastNum = 0;
var neckHeighChangeInt = null;

function alertCloseClick() {
    $('.alert').hide();
    $('#PageGame').hide();
    $('#PageIndex').show();
    $('#clickEve').off();
}
function ruleShowClick() {
    $('#alertRule').show();
}
function gameStartClick() {
    scoreNum = 0;
    $('#alertTip').show();
    $('.my-score-show').text(0);
}
function alertMyPrizeUnloginToLoginClick() {
    $('#alertMyPrizeUnlogin,#alertGameOverPrize').hide();
    $('#alertGameOverLogin').show();
}
function alertRestartClick() {
    scoreNum = 0;
    $('.my-score-show').text(0);
    $('#alertGameOverPrize').hide();
    GameStartFun();
}
// 弹出电话登记框
function gameOverYourPrizeSubmitFun() {

    user = $("#user-txt").val();
    tel = $("#phone-txt").val();

    // 确认弹窗
    //#alertSureLogin
    if (!!user && !!tel) {
        $("#alertSureLogin").show();


    }
}

// 电话登记
function reg() {
    $.ajax({
        type: "post",
        url: "http://weixin.seemoread.com/HXC/AddRegInfo2019Christmas",
        dataType: "json",
        data: {
            openId: wxOpenId,
            name: user,
            tel: tel
        },
        success: function (data) {
            console.log(data)
            if (data.id === 1) {
                $("#alertSureLogin").hide();
                $('#alertGameOverLogin').hide();
                $('#alertMyPrizeHave').show();
            }

        }
    });
}



function noReg() {
    $('.alert').hide();
    $("#alertMyPrizeUnlogin").show();
}

function arrowAni(dom, arrow) { // 向上滑动事件的动画Fun
    dom.animate({ 'height': arrow }, 100)
}
function myPrizeShowClick() {
    //if (eve.data.key === 0) { // 没有玩过，空的
    //    $('#alertMyPrizeEmpty').show();
    //} else if (eve.data.key === 1) { // 玩过，没有登记
    //    $('#alertMyPrizeUnlogin').show();
    //} else if (eve.data.key === 2) { // 玩过，登记过，有奖品
    getUserGiftInfo(10);
    //$('#alertMyPrizeHave').show();
    //}
}
function alertTipIknowClick() {
    $('#alertTip').hide();
    $('#PageIndex').hide();
    $('#PageGame').show();
    GameStartFun();
}
function GameStartFun() {
    beforeGameCountDownTime = 3;// 倒计时
    GameCountDownTime = 30;// 30改为1  用于测试
    $('#neckChange').height(neckOrg);
    $('#bgChange').height(winH);
    $('#countDownNum').text(beforeGameCountDownTime);
    $('#countDownTime').text(GameCountDownTime);
    $('.my-score-show').text(scoreNum);
    $('#alertCountDown').show();
    // 游戏点击事件
    $('#clickEve').on('click', snowClickEvent);
    beforeGameCountDownTimeInt = setInterval(function () {
        if (beforeGameCountDownTime < 2) {
            clearInterval(beforeGameCountDownTimeInt)
            $('#alertCountDown').hide();
            beforeGameCountDownTime = 5;
            GameCountDownTimeFun();
        } else {
            beforeGameCountDownTime--
            $('#countDownNum').text(beforeGameCountDownTime);
        }
    }, 1000)

}
var lastStamp = 0, intervalTime = 0;
function snowClickEvent(e) { // 游戏点击事件Fun
    var t = new Date().getTime();
    if (lastStamp == 0) {
        neckHeighChangeInt = setInterval(function () {
            var c = new Date().getTime()
            if (c > lastStamp) {
                scoreNum = Math.floor(($('#neckChange').height() - neckOrg) / heighter);
                $('.my-score-show').text(scoreNum);
                if ($('#neckChange').height() <= neckOrg) { return; }
                $('#neckChange').animate({ height: neckOrg }, 100);
                $('#bgChange').animate({ height: winH }, 100);
            }
        }, 100)
    }
    $('#neckChange').stop();
    $('#bgChange').stop();

    var speed = ($('#neckChange').height() - neckOrg) / 600;
    lastStamp = t + 200 - (200 * speed > 1 ? 1 : speed);
    arrowAni($('#bgChange'), $('#bgChange').height() + heighter);
    arrowAni($('#neckChange'), $('#neckChange').height() + heighter);
    scoreNum = Math.floor(($('#neckChange').height() - neckOrg) / heighter);
    $('.my-score-show').text(scoreNum);
}
function GameCountDownTimeFun() {
    GameCountDownTimeInt = setInterval(function () {
        // 时间为0时停留太久 ，所有从1改为2
        if (GameCountDownTime < 2) {
            //alert("时间为0")
            $('#clickEve').off('click');
            clearInterval(GameCountDownTimeInt)
            clearInterval(neckHeighChangeInt)
            GameCountDownTime = 30;
            lastStamp = 0;
            //scoreNum = 1001;// 用于测试几种分数下的接口调用是否正常
            //alert("时间为0后：wxOpenId:" + wxOpenId + ",wxUserName:" + wxUserName + ",wxImgSrc:" + wxImgSrc + ",scoreNum:" + scoreNum)
            $.ajax({
                type: "post",
                url: "http://weixin.seemoread.com/HXC/GetPrice2019Christmas",
                dataType: "json",
                data: {
                    openId: wxOpenId,
                    nickName: wxUserName,
                    img: wxImgSrc,
                    score: scoreNum
                },
                success: function (data) {
                    console.log(data);
                    //alert(JSON.stringify(data))
                    GameOverShowPrize(data.id);
                }
            });
            
            // GameOverShowPrize(1);
        } else {
            GameCountDownTime--
            $('#countDownTime').text(GameCountDownTime);
        }
    }, 1000);
}
// 获取当前用户的礼品
function getUserGiftInfo(flag) {
    $.ajax({
        type: "post",
        url: "http://weixin.seemoread.com/HXC/GetWeiXinInfo2019Christmas",
        dataType: "json",
        data: {
            openId: wxOpenId,
        },
        success: function (data) {
            console.log(data);

            if (scoreMissFlag) {
                // 显示文字提示
                $(".game-over-your-prize").show();
                // 设置按钮位置
                $("#alertRestart").removeClass("alertRestart2");
                // 隐藏登记按钮
                $("#alertLoginNow").show();

                scoreMissFlag = false;
            }

            if (data.giftLog == null && flag == 10) {
                $("#alertMyPrizeEmpty").show();
            } else {
                //alert(scoreNum);
                //scoreNum = data.gameScore.Score;
                wxImgSrc = data.gameScore.WeiXinImg;
                
                scoreLastNum = data.gameScore.Score;
                $('.my-score-show').html(scoreNum);
                if (data.giftLog != null) {
                    prizeID = data.giftLog.GiftCustomNum;
                    prizeName = data.giftLog.GiftName;
                    //alertCloseClick();
                    $('.my-prize-id').html(prizeID);
                    if (prizeName == "围脖手套礼盒") {
                        prizeSrc = "images/prize/img1.png";
                    } else if (prizeName == "娃娃公仔+暖心抱枕") {
                        prizeSrc = "images/prize/img2.png";
                    }

                    $('.my-prize-show-name').html(prizeName + "一份");
                    $('.my-prize-pic-src').attr('src', prizeSrc);
                    if (flag == 2) {
                        $('#alertMyPrizeHave').show();
                    }
                    else if (flag >= 3 && flag <= 5) {

                        $('#alertGameOverPrize').show();
                    } else if (flag == 10) {
                        if (data.giftLog.Name != null) {
                            $('#alertMyPrizeHave').show();
                        } else {
                            $('.my-score-show').html(scoreLastNum);
                            $("#alertMyPrizeUnlogin").show();
                        }
                    }

                    
                } else {

                    if (scoreNum > 200) {
                        prizeName = "围脖手套礼盒";
                    } else if (scoreNum > 100) {
                       prizeName = "娃娃公仔+暖心抱枕"
                    } else {
                        prizeName = "";
                    }

                    if (prizeName == "围脖手套礼盒") {
                        prizeSrc = "images/prize/img1.png";
                    } else if (prizeName == "娃娃公仔+暖心抱枕") {
                        prizeSrc = "images/prize/img2.png";
                    }
                    $('.my-prize-show-name').html(prizeName + "一份");
                    $('.my-prize-pic-src').attr('src', prizeSrc);

                    if (prizeName == "") {
                        scoreMiss();
                    } else {
                        if (flag >= 3 && flag <= 5) {

                            $('#alertGameOverPrize').show();
                        }
                        else if (flag == 10) {
                            $("#alertMyPrizeUnlogin").show();
                        }
                        else {
                            // 设置分数
                            $('.my-score-show').html(scoreNum);

                            $('#alertGameOverEmptyPrize').show();
                        }
                    }

                    
                    
                }
                scoreNum = data.gameScore.Score;
            }



        }
    });
}


function GameOverShowPrize(key) {
    //  0 礼物领完了
    //  1 分数没有达到领奖标准
    //  2 已登记
    //  3 没有超过上一次的分数
    //  4 和上次一样的分数 礼物一样的
    //  5 添加记录成功
    // 3,4,5 类似的情况
    if (key === 0) { // 奖品已领完
        // 设置分数
        $('.my-score-show').html(scoreNum);
        $('#alertGameOverEmptyPrize').show();
    } else if (key === 1) {
        scoreMiss();

    } else if (key === 2) {
        getUserGiftInfo(2);
    } else if (key === 3) {
        getUserGiftInfo(3);
    } else if (key === 4) {
        getUserGiftInfo(4);
    } else if (key === 5) {
        getUserGiftInfo(5);

    }
}

function scoreMiss() {
    $(".my-prize-pic-src").attr('src', 'images/alert/score-misss.png');
    // 设置分数
    $('.my-score-show').html(scoreNum);

    // 隐藏文字提示
    $(".game-over-your-prize").hide();
    // 设置按钮位置
    $("#alertRestart").addClass("alertRestart2");
    // 隐藏登记按钮
    $("#alertLoginNow").hide();
    $('#alertGameOverPrize').show();
    scoreMissFlag = true;
}

function shareWxImgFun() {
    console.log(wxImgSrc);
    wxImgSrc = "WxImgs/" + wxImgSrc;

   // alert(wxImgSrc)
    $('.alert').hide();
    $('#wxShareImg').show();

    var app = new PIXI.Application({
        width: 375,
        height: 603,
        antialias: true,
        transparent: true,
        resolution: 1,
        preserveDrawingBuffer: true
    });

    const style = new PIXI.TextStyle({
        fontFamily: 'Microsoft YaHei',
        fontSize: 27,
        fontWeight: 'bold',
        fill: ['#cfa972', '#cfa972'],
        wordWrap: true,
    });
    const style2 = new PIXI.TextStyle({
        fontFamily: 'Microsoft YaHei',
        fontSize: 76,
        fontWeight: 'bold',
        fill: ['#fff', '#fff'],
        wordWrap: true,
    });
    const style3 = new PIXI.TextStyle({
        fontFamily: 'Microsoft YaHei',
        fontSize: 18,
        fontWeight: 'bold',
        fill: ['#a40000', '#a40000'],
        wordWrap: true,
    });

    var message = new PIXI.Text(wxUserName, style);
    var scoreMessage = new PIXI.Text(scoreLastNum + 'm', style2);
    var prizeMessage = new PIXI.Text("获得"+prizeName+"一份", style3);
    message.x = 150;
    message.y = 130;
    prizeMessage.x = 70;
    prizeMessage.y = 300;
    scoreMessage.x = 60;
    scoreMessage.y = 190;

    PIXI.loader
        .add("images/share/bg.jpg")
        .add("images/share/txt.png")
        .add(wxImgSrc)
        .load(setup);

    function setup() {
        //Create the dom sprite
        let bgDom = new PIXI.Sprite(PIXI.loader.resources["images/share/bg.jpg"].texture);
        let headDom = new PIXI.Sprite(PIXI.loader.resources[wxImgSrc].texture);
        let txtDom = new PIXI.Sprite(PIXI.loader.resources["images/share/txt.png"].texture);
        //Add the dom to the stage
        bgDom.width = 375;
        bgDom.height = 603;
        headDom.width = 70;
        headDom.height = 70;
        txtDom.width = 69;
        txtDom.height = 9;
        headDom.x = 70;
        headDom.y = 105;
        txtDom.x = 150;
        txtDom.y = 165;

        app.stage.addChild(bgDom);
        app.stage.addChild(headDom);
        app.stage.addChild(txtDom);
        app.stage.addChild(message);
        app.stage.addChild(scoreMessage);
        app.stage.addChild(prizeMessage);
        app.render(app.stage)
        document.getElementById('PageCanvasImg').src = app.renderer.plugins.extract.image().src
    }

    document.getElementById('PageCanvas').appendChild(app.view);
}