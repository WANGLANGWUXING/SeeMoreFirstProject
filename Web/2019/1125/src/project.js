window.__require=function i(t,o,n){function e(a,r){if(!o[a]){if(!t[a]){var s=a.split("/");if(s=s[s.length-1],!t[s]){var m="function"==typeof __require&&__require;if(!r&&m)return m(s,!0);if(c)return c(s,!0);throw new Error("Cannot find module '"+a+"'")}}var l=o[a]={exports:{}};t[a][0].call(l.exports,function(i){return e(t[a][1][i]||i)},l,l.exports,i,t,o,n)}return o[a].exports}for(var c="function"==typeof __require&&__require,a=0;a<n.length;a++)e(n[a]);return e}({main:[function(i,t,o){"use strict";cc._RF.push(t,"a4318CdwuhPF4SYThKeN0F/","main"),Object.defineProperty(o,"__esModule",{value:!0});var n=cc._decorator,e=n.ccclass,c=(n.property,function(i){function t(){var t=null!==i&&i.apply(this,arguments)||this;return t.Room109=null,t.Room120=null,t.listRoom109=null,t.listRoom120=null,t.curIndex=0,t.curType=0,t.bar=null,t.btnBarBack=null,t.btnBarNext=null,t.listBarPersimmons=null,t}return __extends(t,i),t.prototype.changeBarSize=function(){var i=this.bar,t=1/cc.view.getScaleX();i.scaleX=t,i.scaleY=t,i.y=-1*cc.winSize.height/2+i.height*t/2},t.prototype.start=function(){var i=this;cc.find("Canvas/btnBackIndexTop");for(var t in this.btnBarBack=cc.find("Canvas/Bar/btnBack"),this.btnBarNext=cc.find("Canvas/Bar/btnNext"),this.Room109=cc.find("Canvas/scrollview/view/room/map109"),this.Room120=cc.find("Canvas/scrollview/view/room/map120"),this.bar=cc.find("Canvas/Bar"),this.changeBarSize(),cc.find("Canvas").on("size-changed",function(){i.changeBarSize()}),this.listRoom109=[cc.find("Canvas/scrollview/view/room/map109/109-img1"),cc.find("Canvas/scrollview/view/room/map109/109-bg1"),cc.find("Canvas/scrollview/view/room/map109/109-img2"),cc.find("Canvas/scrollview/view/room/map109/109-bg2"),cc.find("Canvas/scrollview/view/room/map109/109-img3"),cc.find("Canvas/scrollview/view/room/map109/109-bg3"),cc.find("Canvas/scrollview/view/room/map109/109-img4"),cc.find("Canvas/scrollview/view/room/map109/109-bg4")],this.listRoom120=[cc.find("Canvas/scrollview/view/room/map120/120-img1"),cc.find("Canvas/scrollview/view/room/map120/120-bg1"),cc.find("Canvas/scrollview/view/room/map120/120-img2"),cc.find("Canvas/scrollview/view/room/map120/120-bg2"),cc.find("Canvas/scrollview/view/room/map120/120-img3"),cc.find("Canvas/scrollview/view/room/map120/120-bg3"),cc.find("Canvas/scrollview/view/room/map120/120-img4"),cc.find("Canvas/scrollview/view/room/map120/120-bg4")],this.listBarPersimmons=[cc.find("Canvas/Bar/persimmon1").getComponent(cc.Toggle),cc.find("Canvas/Bar/persimmon2").getComponent(cc.Toggle),cc.find("Canvas/Bar/persimmon3").getComponent(cc.Toggle),cc.find("Canvas/Bar/persimmon4").getComponent(cc.Toggle)],this.reset(),this.listRoom109){(o=this.listRoom109[t].getChildByName("click"))&&o.on(cc.Node.EventType.TOUCH_END,function(){i.changeRoom()})}for(var t in this.listRoom120){var o;(o=this.listRoom120[t].getChildByName("click"))&&o.on(cc.Node.EventType.TOUCH_END,function(){i.changeRoom()})}this.btnBarBack.on(cc.Node.EventType.TOUCH_END,function(){window.xxxbtnbackIndex(),i.reset()}),this.btnBarNext.on(cc.Node.EventType.TOUCH_END,function(){i.changeRoom()}),window.xxxstartFind=this.startFind.bind(this)},t.prototype.reset=function(){for(var i in this.Room109.active=!1,this.Room120.active=!1,this.listRoom109)this.listRoom109[i].active=!1;for(var i in this.listRoom120)this.listRoom120[i].active=!1;for(var i in this.curIndex=0,this.curType=0,this.listBarPersimmons)this.listBarPersimmons[i].uncheck();this.btnBarBack.active=!0,this.btnBarNext.active=!1},t.prototype.startFind=function(i){this.reset(),this.curType=i,this.changeRoom()},t.prototype.changeRoom=function(){var i=109==this.curType?this.listRoom109:this.listRoom120;for(var t in this.Room109.active=109==this.curType,this.Room120.active=120==this.curType,this.listRoom109)this.listRoom109[t].active=!1;for(var t in this.listRoom120)this.listRoom120[t].active=!1;this.curIndex%2==0?(this.btnBarBack.active=!0,this.btnBarNext.active=!1):(this.btnBarBack.active=!1,this.btnBarNext.active=!0),i[this.curIndex++].active=!0;for(var o=Math.floor(this.curIndex/2),n=0;n<this.listBarPersimmons.length;n++)n<o?this.listBarPersimmons[n].check():this.listBarPersimmons[n].uncheck();this.curIndex>7&&(this.btnBarBack.active=!0,this.btnBarNext.active=!1,console.log("\u7ed3\u675f\u6e38\u620f"),window.xxxgetPresent())},t=__decorate([e],t)}(cc.Component));o.default=c,cc._RF.pop()},{}]},{},["main"]);
//# sourceMappingURL=project.js.map
