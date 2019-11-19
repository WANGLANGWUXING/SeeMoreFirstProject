(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";
/* 点击穿透，js引入方式
if ('addEventListener' in document) {
  document.addEventListener('DOMContentLoaded', function() {
      FastClick.attach(document.body);
  }, false);
}
*/
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var Common = /** @class */ (function () {
    function Common() {
    }
    Common.delay = function (timeout) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                return [2 /*return*/, new Promise(function (resolve, reject) {
                        setTimeout(function () {
                            resolve();
                        }, timeout);
                    })];
            });
        });
    };
    Common.jqReady = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                return [2 /*return*/, new Promise(function (resolve, reject) {
                        $(function () {
                            resolve();
                        });
                    })];
            });
        });
    };
    Common.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)
            return unescape(r[2]);
        return null;
    };
    Common.toast = function (txt, time) {
        if (time === void 0) { time = 1000; }
        var dom = $('.alerts');
        if (dom.length < 1) {
            return;
        }
        dom.empty();
        dom.text(txt);
        dom.fadeIn('500').delay(time).fadeOut('500');
    };
    Common.confirm = function (title, txts, time, trueFun, falseFun) {
        var appendDom = '<div class="alert-wrap">\
                <h6 id="alertTitile" class="alert-title"></h6>\
                <div id="alertContent" class="alert-content"></div>\
                <div class="alert-btn-flex">\
                  <input id="alertFalse" type="button" value="取消" />\
                  <input id="alertTrue" type="button" value="确定" />\
                </div>\
              </div>';
        var dom = $('#alertConfirm');
        dom.append(appendDom);
        if (title.length < 1) {
            title = '';
        }
        ;
        if (txts.length < 1) {
            txts = '是否取消？';
        }
        ;
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
    };
    return Common;
}());
exports.default = Common;
window['Common'] = Common;
},{}],2:[function(require,module,exports){
"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var index = /** @class */ (function () {
    function index() {
        this.loader = null;
    }
    index.prototype.init = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                this.loader = new createjs.LoadQueue(true);
                FastClick.attach(document.body);
                /* 预加载 */
                this.loader.on("progress", this.handleFileLoad.bind(this));
                this.loader.on("complete", this.handleComplete.bind(this));
                this.loader.loadManifest(manifest);
                return [2 /*return*/];
            });
        });
    };
    index.prototype.handleFileLoad = function () {
        return __awaiter(this, void 0, void 0, function () {
            var bnum;
            return __generator(this, function (_a) {
                bnum = parseInt(this.loader.progress * 100 + "");
                $('.process span').text(bnum + '%');
                return [2 /*return*/];
            });
        });
    };
    index.prototype.handleComplete = function () {
        return __awaiter(this, void 0, void 0, function () {
            var LoadPage, aniPage;
            return __generator(this, function (_a) {
                LoadPage = $('#loading');
                aniPage = $('#aniPage');
                LoadPage.fadeOut(500).delay(500).remove();
                aniPage.fadeIn(1500);
                return [2 /*return*/];
            });
        });
    };
    return index;
}());
exports.default = index;
},{}],3:[function(require,module,exports){
"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var common_1 = require("./common");
var index_1 = require("./index");
function main() {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, common_1.default.jqReady()];
                case 1:
                    _a.sent();
                    new index_1.default().init();
                    return [2 /*return*/];
            }
        });
    });
}
main();
},{"./common":1,"./index":2}]},{},[3])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9fYnJvd3Nlci1wYWNrQDYuMS4wQGJyb3dzZXItcGFjay9fcHJlbHVkZS5qcyIsInNyYy90eXBlU2NyaXB0L2NvbW1vbi50cyIsInNyYy90eXBlU2NyaXB0L2luZGV4LnRzIiwic3JjL3R5cGVTY3JpcHQvbWFpbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7QUNDQTs7Ozs7O0VBTUU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7O0FBRUY7SUFBQTtJQTREQSxDQUFDO0lBMURjLFlBQUssR0FBbEIsVUFBbUIsT0FBTzs7O2dCQUN4QixzQkFBTyxJQUFJLE9BQU8sQ0FBQyxVQUFTLE9BQU8sRUFBQyxNQUFNO3dCQUN0QyxVQUFVLENBQUM7NEJBQ1QsT0FBTyxFQUFFLENBQUE7d0JBQ1gsQ0FBQyxFQUFFLE9BQU8sQ0FBQyxDQUFDO29CQUNoQixDQUFDLENBQUMsRUFBQzs7O0tBQ0o7SUFFWSxjQUFPLEdBQXBCOzs7Z0JBQ0Usc0JBQU8sSUFBSSxPQUFPLENBQUMsVUFBUyxPQUFPLEVBQUMsTUFBTTt3QkFDeEMsQ0FBQyxDQUFDOzRCQUNBLE9BQU8sRUFBRSxDQUFDO3dCQUNaLENBQUMsQ0FBQyxDQUFDO29CQUNMLENBQUMsQ0FBQyxFQUFDOzs7S0FFSjtJQUVNLHFCQUFjLEdBQXJCLFVBQXNCLElBQVk7UUFDaEMsSUFBSSxHQUFHLEdBQUcsSUFBSSxNQUFNLENBQUMsT0FBTyxHQUFHLElBQUksR0FBRyxlQUFlLEVBQUUsR0FBRyxDQUFDLENBQUE7UUFDM0QsSUFBSSxDQUFDLEdBQUcsTUFBTSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQTtRQUNuRCxJQUFJLENBQUMsSUFBSSxJQUFJO1lBQUUsT0FBTyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7UUFDcEMsT0FBTyxJQUFJLENBQUE7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QyxDQUFDLGVBQWUsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLEdBQUcsR0FBRyxDQUFDLENBQUM7Ozs7S0FDckM7SUFFSyw4QkFBYyxHQUFwQjs7OztnQkFFUSxRQUFRLEdBQVUsQ0FBQyxDQUFDLFVBQVUsQ0FBQyxDQUFDO2dCQUVoQyxPQUFPLEdBQVUsQ0FBQyxDQUFDLFVBQVUsQ0FBQyxDQUFDO2dCQUdyQyxRQUFRLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxNQUFNLEVBQUUsQ0FBQztnQkFDMUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsQ0FBQzs7OztLQUd0QjtJQUVILFlBQUM7QUFBRCxDQS9CQSxBQStCQyxJQUFBOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztBQ2pDRCxtQ0FBOEI7QUFDOUIsaUNBQTRCO0FBRTVCLFNBQWUsSUFBSTs7Ozt3QkFDaEIscUJBQU0sZ0JBQU0sQ0FBQyxPQUFPLEVBQUUsRUFBQTs7b0JBQXRCLFNBQXNCLENBQUM7b0JBQ3ZCLElBQUksZUFBSyxFQUFFLENBQUMsSUFBSSxFQUFFLENBQUM7Ozs7O0NBQ3JCO0FBRUQsSUFBSSxFQUFFLENBQUMiLCJmaWxlIjoiZ2VuZXJhdGVkLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXNDb250ZW50IjpbIihmdW5jdGlvbigpe2Z1bmN0aW9uIHIoZSxuLHQpe2Z1bmN0aW9uIG8oaSxmKXtpZighbltpXSl7aWYoIWVbaV0pe3ZhciBjPVwiZnVuY3Rpb25cIj09dHlwZW9mIHJlcXVpcmUmJnJlcXVpcmU7aWYoIWYmJmMpcmV0dXJuIGMoaSwhMCk7aWYodSlyZXR1cm4gdShpLCEwKTt2YXIgYT1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK2krXCInXCIpO3Rocm93IGEuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixhfXZhciBwPW5baV09e2V4cG9ydHM6e319O2VbaV1bMF0uY2FsbChwLmV4cG9ydHMsZnVuY3Rpb24ocil7dmFyIG49ZVtpXVsxXVtyXTtyZXR1cm4gbyhufHxyKX0scCxwLmV4cG9ydHMscixlLG4sdCl9cmV0dXJuIG5baV0uZXhwb3J0c31mb3IodmFyIHU9XCJmdW5jdGlvblwiPT10eXBlb2YgcmVxdWlyZSYmcmVxdWlyZSxpPTA7aTx0Lmxlbmd0aDtpKyspbyh0W2ldKTtyZXR1cm4gb31yZXR1cm4gcn0pKCkiLCJcclxuLyog54K55Ye756m/6YCP77yManPlvJXlhaXmlrnlvI9cclxuaWYgKCdhZGRFdmVudExpc3RlbmVyJyBpbiBkb2N1bWVudCkge1xyXG4gIGRvY3VtZW50LmFkZEV2ZW50TGlzdGVuZXIoJ0RPTUNvbnRlbnRMb2FkZWQnLCBmdW5jdGlvbigpIHtcclxuICAgICAgRmFzdENsaWNrLmF0dGFjaChkb2N1bWVudC5ib2R5KTtcclxuICB9LCBmYWxzZSk7XHJcbn1cclxuKi9cclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNsYXNzIENvbW1vbiB7XHJcblxyXG4gIHN0YXRpYyBhc3luYyBkZWxheSh0aW1lb3V0KXtcclxuICAgIHJldHVybiBuZXcgUHJvbWlzZShmdW5jdGlvbihyZXNvbHZlLHJlamVjdCl7XHJcbiAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgICByZXNvbHZlKClcclxuICAgICAgICB9LCB0aW1lb3V0KTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgc3RhdGljIGFzeW5jIGpxUmVhZHkoKXtcclxuICAgIHJldHVybiBuZXcgUHJvbWlzZShmdW5jdGlvbihyZXNvbHZlLHJlamVjdCl7XHJcbiAgICAgICQoZnVuY3Rpb24gKCkge1xyXG4gICAgICAgIHJlc29sdmUoKTtcclxuICAgICAgfSk7XHJcbiAgICB9KTtcclxuICAgXHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0UXVlcnlTdHJpbmcobmFtZTogc3RyaW5nKSB7XHJcbiAgICBsZXQgcmVnID0gbmV3IFJlZ0V4cChcIihefCYpXCIgKyBuYW1lICsgXCI9KFteJl0qKSgmfCQpXCIsIFwiaVwiKVxyXG4gICAgbGV0IHIgPSB3aW5kb3cubG9jYXRpb24uc2VhcmNoLnN1YnN0cigxKS5tYXRjaChyZWcpXHJcbiAgICBpZiAociAhPSBudWxsKSByZXR1cm4gdW5lc2NhcGUoclsyXSlcclxuICAgIHJldHVybiBudWxsXHJcbiAgfVxyXG5cclxuICBzdGF0aWMgdG9hc3QodHh0OiBzdHJpbmcgLCB0aW1lOm51bWJlciA9IDEwMDApIHtcclxuICAgIGxldCBkb20gPSAkKCcuYWxlcnRzJyk7XHJcbiAgICBpZihkb20ubGVuZ3RoIDwgMSl7cmV0dXJuIDt9XHJcbiAgICBkb20uZW1wdHkoKTtcclxuICAgIGRvbS50ZXh0KHR4dCk7XHJcbiAgICBkb20uZmFkZUluKCc1MDAnKS5kZWxheSh0aW1lKS5mYWRlT3V0KCc1MDAnKTtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBjb25maXJtKHRpdGxlOiBzdHJpbmcsIHR4dHM6IHN0cmluZywgdGltZTogbnVtYmVyLCB0cnVlRnVuOiBGdW5jdGlvbiwgZmFsc2VGdW46IEZ1bmN0aW9uKXtcclxuICAgIGNvbnN0IGFwcGVuZERvbSA9ICc8ZGl2IGNsYXNzPVwiYWxlcnQtd3JhcFwiPlxcXHJcbiAgICAgICAgICAgICAgICA8aDYgaWQ9XCJhbGVydFRpdGlsZVwiIGNsYXNzPVwiYWxlcnQtdGl0bGVcIj48L2g2PlxcXHJcbiAgICAgICAgICAgICAgICA8ZGl2IGlkPVwiYWxlcnRDb250ZW50XCIgY2xhc3M9XCJhbGVydC1jb250ZW50XCI+PC9kaXY+XFxcclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJhbGVydC1idG4tZmxleFwiPlxcXHJcbiAgICAgICAgICAgICAgICAgIDxpbnB1dCBpZD1cImFsZXJ0RmFsc2VcIiB0eXBlPVwiYnV0dG9uXCIgdmFsdWU9XCLlj5bmtohcIiAvPlxcXHJcbiAgICAgICAgICAgICAgICAgIDxpbnB1dCBpZD1cImFsZXJ0VHJ1ZVwiIHR5cGU9XCJidXR0b25cIiB2YWx1ZT1cIuehruWumlwiIC8+XFxcclxuICAgICAgICAgICAgICAgIDwvZGl2PlxcXHJcbiAgICAgICAgICAgICAgPC9kaXY+JztcclxuICAgIGxldCBkb20gPSAkKCcjYWxlcnRDb25maXJtJylcclxuICAgIGRvbS5hcHBlbmQoYXBwZW5kRG9tKTtcclxuICAgIGlmKHRpdGxlLmxlbmd0aCA8IDEpeyB0aXRsZSA9ICcnfTtcclxuICAgIGlmKHR4dHMubGVuZ3RoIDwgMSl7IHR4dHMgPSAn5piv5ZCm5Y+W5raI77yfJ307XHJcbiAgICAkKCcjYWxlcnRUaXRpbGUnKS50ZXh0KHRpdGxlKTtcclxuICAgICQoJyNhbGVydENvbnRlbnQnKS50ZXh0KHR4dHMpO1xyXG4gICAgZG9tLmZhZGVJbih0aW1lKTtcclxuICAgICQoJyNhbGVydFRydWUnKS5vbmUoJ2NsaWNrJyxmdW5jdGlvbigpe1xyXG4gICAgICBkb20uZmFkZU91dCh0aW1lKTtcclxuICAgICAgdHJ1ZUZ1bigpO1xyXG4gICAgfSk7XHJcbiAgICAkKCcjYWxlcnRGYWxzZScpLm9uZSgnY2xpY2snLGZ1bmN0aW9uKCl7XHJcbiAgICAgIGRvbS5mYWRlT3V0KHRpbWUpO1xyXG4gICAgICBmYWxzZUZ1bigpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxufVxyXG5cclxud2luZG93WydDb21tb24nXSA9IENvbW1vbjsiLCJpbXBvcnQgQ29tbW9uIGZyb20gXCIuL2NvbW1vblwiO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgaW5kZXgge1xyXG5cclxuICBsb2FkZXI6IGFueSA9IG51bGw7XHJcblxyXG4gIGFzeW5jIGluaXQoKSB7XHJcbiAgICB0aGlzLmxvYWRlciA9IG5ldyBjcmVhdGVqcy5Mb2FkUXVldWUodHJ1ZSk7XHJcbiAgICBGYXN0Q2xpY2suYXR0YWNoKGRvY3VtZW50LmJvZHkpO1xyXG4gICAgLyog6aKE5Yqg6L29ICovXHJcbiAgICB0aGlzLmxvYWRlci5vbihcInByb2dyZXNzXCIsIHRoaXMuaGFuZGxlRmlsZUxvYWQuYmluZCh0aGlzKSk7XHJcbiAgICB0aGlzLmxvYWRlci5vbihcImNvbXBsZXRlXCIsIHRoaXMuaGFuZGxlQ29tcGxldGUuYmluZCh0aGlzKSk7XHJcbiAgICB0aGlzLmxvYWRlci5sb2FkTWFuaWZlc3QobWFuaWZlc3QpO1xyXG4gIH1cclxuXHJcbiAgYXN5bmMgaGFuZGxlRmlsZUxvYWQoKSB7XHJcbiAgICBsZXQgYm51bSA9IHBhcnNlSW50KHRoaXMubG9hZGVyLnByb2dyZXNzICogMTAwICsgXCJcIik7XHJcbiAgICAkKCcucHJvY2VzcyBzcGFuJykudGV4dChibnVtICsgJyUnKTtcclxuICB9XHJcblxyXG4gIGFzeW5jIGhhbmRsZUNvbXBsZXRlKCkge1xyXG4gICAgLy8g5Yqg6L295a6M5oiQ5ZCO77yM5Li76YC76L6R5Yy6XHJcbiAgICBjb25zdCBMb2FkUGFnZTpKUXVlcnkgPSAkKCcjbG9hZGluZycpO1xyXG5cclxuICAgIGNvbnN0IGFuaVBhZ2U6SlF1ZXJ5ID0gJCgnI2FuaVBhZ2UnKTtcclxuXHJcblxyXG4gICAgTG9hZFBhZ2UuZmFkZU91dCg1MDApLmRlbGF5KDUwMCkucmVtb3ZlKCk7XHJcbiAgICBhbmlQYWdlLmZhZGVJbigxNTAwKTtcclxuXHJcblxyXG4gIH1cclxuXHJcbn0iLCJpbXBvcnQgQ29tbW9uIGZyb20gXCIuL2NvbW1vblwiO1xyXG5pbXBvcnQgSW5kZXggZnJvbSBcIi4vaW5kZXhcIjtcclxuXHJcbmFzeW5jIGZ1bmN0aW9uIG1haW4oKSB7IFxyXG4gICBhd2FpdCBDb21tb24uanFSZWFkeSgpO1xyXG4gICBuZXcgSW5kZXgoKS5pbml0KCk7XHJcbn1cclxuXHJcbm1haW4oKTsiXX0=
