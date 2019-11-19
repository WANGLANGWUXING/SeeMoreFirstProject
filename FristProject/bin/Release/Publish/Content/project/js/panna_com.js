/*
 * Version: 0.0.3
 * Author: Fun
 * Create: 2015-09-06
 * Update: 2015-10-23
 */


pannaUtil = {};

pannaUtil.preload = function(callback, isResize){
	var preLoadNum = $(".preload").length;
    var curNum = 0;
	$(".preload").each(function(){
        var self = $(this);
        if(self.attr('data-src')){
            var img = new Image();
            img.src = self.attr('data-src');
            img.onload = function(){
            	curNum += 1;
	            var percent = curNum/preLoadNum;
	            self.attr('src', self.attr('data-src'));
	            // 预加载的图片都自动设置高宽
	            if(isResize && !self.hasClass('noResize')){
	            	self.css({'width':this.width/100+'rem'});
	            }
	            // 是否设置center
	            if(self.hasClass('_sCenter')){
	            	self.css({'left':'50%'});
	            	self.css({'margin-left':-this.width/100/2 +'rem'});
	            }
	            callback(percent);
            };
            img.onerror = function(data){
            	var index = img.src.lastIndexOf('/');
            	var imgname = img.src.substr(index+1);
            	pWidget.alert("图片加载错误："+imgname);
            }
        }
    });
}
pannaUtil.loadJs = function(url){
	var script = document.createElement("script");
	script.type = "text/javascript";
	script.src = url;
	document.head.appendChild(script);
}
pannaUtil.loadCss = function(url){
	var link = document.createElement("link");
	link.rel = "stylesheet";
	link.type = "text/css";
	link.href = url;
	link.src = url;
	document.head.appendChild(link);
}

 // 参数获取
pannaUtil.getQueryString = function(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

// vertification...
pannaUtil.vertify = {
	isName : function(str){
		var reg = /^[\u4E00-\u9FA5]{2,6}$/;
    	return reg.test(str);
	},

	isTel : function(str){
	    var reg = /^1[34578][0-9]\d{8}$/;
	    return reg.test(str);
	},

	isIDCard : function(str){
	    var reg = new RegExp("/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/");
	    return reg.test(str);
	}
};

/* panna global widget*/
pWidget = {
	params:{},
	config : function(params){
		this.params = params;
		if(params.music){
			this.addMusic();
		};
		if(params.bottomLogo){
			this.addBottomLogo();
		};
		if(params.arrow){
			this.addArrow();
		}
	},

	addMusic: function(){
		var music = null;
		var openImgUrl = 'http://lib.chinapanna.com/images/global/music-open.png';
		var closeImgUrl = 'http://lib.chinapanna.com/images/global/music-close.png';
		if(typeof this.params.musicImgUrl == 'object'){
			openImgUrl = this.params.musicImgUrl[0];
			closeImgUrl = this.params.musicImgUrl[1];
		}
		if($("#musicBtn").length>0){
			music = $("#music")[0];
		}else{ 
			$("body").prepend("<a href='javascript:;' class='musicBtn music_open' id='musicBtn'></a>");
			$("body").prepend("<audio src='mp3/music.mp3' id='music' autoplay loop></audio>");
			$("#musicBtn").css('background-image', "url(" + openImgUrl +")");
			music = $("#music")[0];
		}
		$("#musicBtn").click(function () {
            if (music.paused) {
                music.play();
                $("#musicBtn").addClass("music_open");
                $("#musicBtn").css('background-image', "url(" + openImgUrl +")");
            } else {
                music.pause();
                $("#musicBtn").removeClass("music_open");
                $("#musicBtn").css('background-image', "url(" + closeImgUrl +")");
            }
        });
	},

	addBottomLogo: function(){
		if($("#bottom_logo").length>0){
			$("#bottom_logo").show();
		}else{
			if(typeof this.params.bottomLogoImgUrl == "string"){
				$("body").prepend("<img src='"+ this.params.bottomLogoImgUrl + "' class='bootom_logo'/>");	
			}else{
				$("body").prepend("<img src='http://lib.chinapanna.com/images/global/panna_logo.png' class='bootom_logo'/>");
			}
			
		}
	},

	addArrow: function(){
		if($("#arrow").length>0){
			$("#arrow").show();
		}else{
			if(typeof this.params.arrowImgUrl == "string"){
				$("body").append("<img src='" + this.params.arrowImgUrl + "' class='arrow animate_int fadeInUp' id='arrow'>");
			}else{
				$("body").append("<img src='http://lib.chinapanna.com/images/global/arrow.png' class='arrow animate_int fadeInUp' id='arrow'>");
			}
		}
	},

	showShare: function(){
		if($("#fxImg").length>0){
			$("#fxImg").show();
		}else{
			if(typeof this.params.shareImage == "string"){
				$("body").prepend("<img id='fxImg' src='" + this.params.shareImage +"' alt='' style='width:100%; height:100%; position:fixed; z-index:1000;  top: 0px; '>");
			}else{
				$("body").prepend("<img id='fxImg' src='http://lib.chinapanna.com/images/global/fxzy.png' alt='' style='width:100%; height:100%; position:fixed; z-index:1000;  top: 0px; '>");
			}
			$("#fxImg").click(function(){
				$("#fxImg").hide();
			});
		}
	},

	showLoading: function(){
		if($(".spinner").length>0){
			$(".spinner").show();
		}else{
			$("body").prepend("<div class='spinner'><div class='spinner-mask'></div><div class='spinner-circle'></div></div>");
		}
	},
	
	hideLoading: function(){
		$(".spinner").hide();
	},

	alert: function(res, callback, okres){
		if($('.myalert').length>0){
			$('.myalert').remove();
		}
		var ok = "好";
		if(typeof okres == "string"){
			ok = okres;
		}
		$("body").append("<div class='myalert'><div class='alert-mask'></div><div class='_alert'><div class='alert-title'>" + res + "</div><div class='alert-ok'>" + ok + "</div></div></div>")
		$(".alert-ok").click(function(){
			$(".myalert").remove();
			if(typeof callback == "function"){
				callback();
			}
		});
	}	
}