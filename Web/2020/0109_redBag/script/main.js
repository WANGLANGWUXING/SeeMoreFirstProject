$(function () {
  var wxUserName = 'xxx'
  
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
    shareWxImgFun();
    /*** 初始化完毕-   正式代码开始： ***/
    $('.btn-start').on('click',function(){
      $('#index1').hide();
      $('#index2').show()
    })
    $('.btn-rule').on('click',function(){
      $('#alertRule').show()
    })
    $('.btn-my-prize').on('click',function(){
      $('#alertMyPrizeEmpty').show()
    })

    $('.alert-close').on('click',function(){
      $('.alert').hide()
    })

    $('.btn-start-wrap').on('touchstart',function(){
      $('.center1,.center3,.center4,.center5').addClass('startAni')
      $('.bottom2').removeClass('bottom2')
      $('.bottom2').addClass('bottom')
    })
    $('.center').on('touchend',function(){
      $('#result').show();
    })
    
  }

  function toast(txt , time) { // 提示信息弹出
    var dom = $('.alerts');
    if(dom.length < 1){return ;}
    dom.empty();
    dom.text(txt);
    dom.fadeIn('500').delay(time).fadeOut('500');
  }

  function confirm(title,txts,time,trueFun,falseFun){ // 弹出选择框
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
    if(title.length < 1){ title = ''};
    if(txts.length < 1){ txts = '是否取消？'};
    $('#alertTitile').text(title);
    $('#alertContent').text(txts);
    dom.fadeIn(time);
    $('#alertTrue').one('click',function(){
      dom.fadeOut(time);
      trueFun();
    });
    $('#alertFalse').one('click',function(){
      dom.fadeOut(time);
      falseFun();
    });
  }

  /**  musicButton  **/
  $('.music').on('click',function(){
    $(this).toggleClass('music-ani');
    if( $(this).hasClass('music-ani') ){
      document.getElementById('music').play()
    }else{
      document.getElementById('music').pause()
    }
  })

  /**  musicButton for weiXin **/
  document.addEventListener("WeixinJSBridgeReady", function () { 
    document.getElementById('music').play();
  }, false); 

  function shareWxImgFun() {

    wxImgSrc = 'http://thirdwx.qlogo.cn/mmopen/vi_32/poo31O8GibesWht9uNZ6kuhia3riaXfOQ52DXb1czp3WpT2ARWAjEo9aSIOnzFsFQKpNIKBuU6r22Q5VsIT0Ogu8Q/132'
  
    var img = new Image();
    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext('2d');
    document.getElementById('PageCanvas').appendChild(img);
    document.getElementById('PageCanvas').appendChild(canvas);
  
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
    $('#wxShareImg').show();
  
  }
  
  function PixiCanvas (){
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
      fontSize: 40,
      fontWeight: 'bold',
      fill: ['#000', '#555'],
      wordWrap: true,
    });
  
    var message = new PIXI.Text(wxUserName, style);
    message.x = 387;
    message.y = 947;
  
    PIXI.loader
      .add("images/share/1.png")
      .add("images/share/wx2wm.png")
      .add(wxImgSrc)
      .load(function(){
          //Create the dom sprite
          let bgDom = new PIXI.Sprite(PIXI.loader.resources["images/share/1.png"].texture);
          let headDom = new PIXI.Sprite(PIXI.loader.resources[wxImgSrc].texture);
          let wx2wmDom = new PIXI.Sprite(PIXI.loader.resources["images/share/wx2wm.png"].texture);
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
    app.view.style.width = 375 +'px';
    app.view.style.height = 603 +'px';
    document.getElementById('PageCanvas').appendChild(app.view);
  }

}) // $-over