/*
 * Version: 1.0.1
 * Author: Lp
 * Create: 2016-04-06
 */


;(function($){
    $.fn.lazyloder = function(callback) {
    	var curNum = 0;
    	var imgs = this.find('.preload');
    	var preLoadNum = imgs.length;
		imgs.each(function() {
			var self = $(this);
			if(self.attr('data-src')){
	            var img = new Image();
	            img.src = self.attr('data-src');
	            img.onload = function(){
	            	curNum += 1;
		            var percent = curNum/preLoadNum;
		            self.attr('src', self.attr('data-src'));
		            // 预加载的图片都自动设置高宽
		            if(!self.hasClass('noResize')){
		            	self.css({'width':this.width/100+'rem'});
		            }
		            // 是否设置center
		            if(self.hasClass('_sCenter')){
		            	self.css({'left':'50%'});
		            	self.css({'margin-left':-this.width/100/2 +'rem'});
		            }
		            if(callback) {
		            	callback(percent);
		            }
	            };
	            img.onerror = function(){
	            	var index = img.src.lastIndexOf('/');
            		var imgname = img.src.substr(index+1);
	            	console.log("图片加载错误："+imgname);
	            }
	            
	        }
		})
		return this;
	}
})(jQuery);