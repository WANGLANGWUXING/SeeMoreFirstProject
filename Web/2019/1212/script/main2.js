var beforeGameCountDownTime = 0;
var GameCountDownTime = 0;
var beforeGameCountDownTimeInt = null;
var GameCountDownTimeInt = null;
var GameOverBoolean = true;

var heighter = 20,  // 点击增速
    prizeName = '礼品名称', // $('.my-prize-show-name')
    prizeID = '',   // $('.my-prize-id')
    prizeSrc = '';  // $('.my-prize-pic-src')
scoreNum = 0;   // $('.my-score-show')
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

function arrowAni(dom, arrow) { // 向上滑动事件的动画Fun
    dom.animate({ 'height': arrow }, 100)
}
function myPrizeShowClick(eve) {
    if (eve.data.key === 0) { // 没有玩过，空的
        $('#alertMyPrizeEmpty').show();
    } else if (eve.data.key === 1) { // 玩过，没有登记
        $('#alertMyPrizeUnlogin').show();
    } else if (eve.data.key === 2) { // 玩过，登记过，有奖品
        getUserGiftInfo(2);
        $('#alertMyPrizeHave').show();
    }
}
function alertTipIknowClick() {
    $('#alertTip').hide();
    $('#PageIndex').hide();
    $('#PageGame').show();
    GameStartFun();
}
function GameStartFun() {
    beforeGameCountDownTime = 1;
    GameCountDownTime = 1;// 30改为1  用于测试
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
        if (GameCountDownTime < 1) {
            clearInterval(GameCountDownTimeInt)
            clearInterval(neckHeighChangeInt)
            GameCountDownTime = 30;
            lastStamp = 0;
            scoreNum = 501;// 用于测试几种分数下的接口调用是否正常
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
            scoreNum = data.gameScore.Score;
            prizeName = data.giftLog.GiftName;
            prizeID = data.giftLog.GiftCustomNum;
            if (prizeName == "围脖手套礼盒") {
                prizeSrc = "images/prize/img1.png";
            } else if (prizeName == "娃娃公仔+暖心抱枕") {
                prizeSrc = "images/prize/img2.png";
            }
            //alertCloseClick();
            $('.my-prize-id').html(prizeID);
            $('.my-score-show').html(scoreNum);
            $('.my-prize-show-name').html(prizeName+"一份");
            $('.my-prize-pic-src').attr('src', prizeSrc);

            if (flag == 2) {
                $('#alertMyPrizeHave').show();
            } else {
                $('#alertGameOverPrize').show();
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
        $('#alertGameOverEmptyPrize').show();
    } else if (key === 1) {
        alert("分数没有达到领奖标准");
        alertCloseClick();
        //$('#alertGameOverPrize').show();
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
function shareWxImgFun() {
    console.log(wxImgSrc);
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
    var scoreMessage = new PIXI.Text(scoreNum + 'm', style2);
    var prizeMessage = new PIXI.Text(prizeName, style3);
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