/**

 @Name：layuiAdmin iframe版核心模块
 @Author：贤心
 @Site：http://www.layui.com/admin/
 @License：LPPL
    
 */

layui.define('layer', function (exports) {
    var $ = layui.jquery
        , layer = layui.layer
        , element = layui.element
        , device = layui.device()

        , $win = $(window), $body = $('body')
        , container = $('#LAY_app')

        , SHOW = 'layui-show', HIDE = 'layui-hide', THIS = 'layui-this', DISABLED = 'layui-disabled', TEMP = 'template'
        , APP_BODY = '#LAY_app_body', APP_FLEXIBLE = 'LAY_app_flexible'
        , FILTER_TAB_TBAS = 'layadmin-layout-tabs'
        , APP_SPREAD_SM = 'layadmin-side-spread-sm', TABS_BODY = 'layadmin-tabsbody-item'
        , ICON_SHRINK = 'layui-icon-shrink-right', ICON_SPREAD = 'layui-icon-spread-left'
        , SIDE_SHRINK = 'layadmin-side-shrink', SIDE_MENU = 'LAY-system-side-menu'
        , MOD_NAME = 'admin'
        //通用方法
        , admin = {
            v: '1.2.1 std'

            //数据的异步请求
            , req: function (options) {
                var that = this
                    , success = options.success
                    , error = options.error;



                options.data = options.data || {};
                options.headers = options.headers || {};



                delete options.success;
                delete options.error;

                return $.ajax($.extend({
                    type: 'GET'
                    , dataType: 'json'
                    , success: function (res) {

                        //只有 response 的 code 一切正常才执行 done
                        if (res.code === 200) {
                            typeof options.done === 'function' && options.done(res);
                        }
                        //其它异常
                        else {
                            layer.alert(res.msg, { icon: 7, title: '错误' });
                            //var error = [
                            //    '<cite>Error：</cite> ' + (res.msg || '返回状态码异常')
                            //    , ''
                            //].join('');
                            //admin.error(error);
                        }

                        //只要 http 状态码正常，无论 response 的 code 是否正常都执行 success
                        typeof success === 'function' && success(res);
                    }
                    , error: function (e, code) {
                        var err = [
                            '请求异常，请重试<br><cite>错误信息：</cite>' + code
                            , ''
                        ].join('');
                        admin.error(err);

                        typeof error === 'function' && error(e);
                    }
                }, options));
            }

            , logout: function () {
                var storage = layui.sessionData('login');
                var area = storage.area;
                var url = area !== null && area !== '' ? '/' + area + '/Account/OutLogin' : '/Home/OutLogin';
                var href = area !== null && area !== '' ? '/' + area + '/Account/Index' : '/Home/Index';
                admin.req({
                    url: url
                    , type: 'get'
                    , data: {}
                    , done: function (res) { //这里要说明一下：done 是只有 response 的 code 正常才会执行。而 succese 则是只要 http 为 200 就会执行
                         location.href = href;
                    }
                });
            }
            , back: function () {
                history.back();
            }
            , reload: function() {
                location.reload();
            }
            //xss 转义
            , escape: function (html) {
                return String(html || '').replace(/&(?!#?[a-zA-Z0-9]+;)/g, '&amp;')
                    .replace(/</g, '&lt;').replace(/>/g, '&gt;')
                    .replace(/'/g, '&#39;').replace(/"/g, '&quot;');
            }

            //事件监听
            , on: function (events, callback) {
                return layui.onevent.call(this, MOD_NAME, events, callback);
            }
            , select: function (options) {
                var elem = $(options.elem);
                if (!elem[0]) {
                    return layui.hint.error("没有找到" + options.elem + "元素");
                }
                $.ajax({
                    type: options.method || "get",
                    url: options.url,
                    data: options.data || {},
                    contentType: options.contentType,
                    dataType: "json",
                    success: function (res) {
                        var html = "";
                        $(res.data).each(function (index, item) {
                            if (options.value && options.value === item[options.valueName]) {
                                html += "<option value=" + item[options.valueName] + " selected >" + item[options.displayName] + "</option>";
                            } else {
                                html += "<option value=" + item[options.valueName] + " >" + item[options.displayName] + "</option>";
                            }
                        });
                        elem.append(html);
                        layui.form.render("select");
                    },
                    error: function (res) {
                        layer.msg("出错了！", { icon: 2 });
                        return false;
                    }
                });
            }

            //发送验证码
            , sendAuthCode: function (options) {
                options = $.extend({
                    seconds: 60
                    , elemPhone: '#LAY_phone'
                    , elemVercode: '#LAY_vercode'
                }, options);

                var seconds = options.seconds
                    , btn = $(options.elem)
                    , token = null
                    , timer, countDown = function (loop) {
                        seconds--;
                        if (seconds < 0) {
                            btn.removeClass(DISABLED).html('获取验证码');
                            seconds = options.seconds;
                            clearInterval(timer);
                        } else {
                            btn.addClass(DISABLED).html(seconds + '秒后重获');
                        }

                        if (!loop) {
                            timer = setInterval(function () {
                                countDown(true);
                            }, 1000);
                        }
                    };

                options.elemPhone = $(options.elemPhone);
                options.elemVercode = $(options.elemVercode);

                btn.on('click', function () {
                    var elemPhone = options.elemPhone
                        , value = elemPhone.val();

                    if (seconds !== options.seconds || $(this).hasClass(DISABLED)) return;

                    if (!/^1\d{10}$/.test(value)) {
                        elemPhone.focus();
                        return layer.msg('请输入正确的手机号')
                    };

                    if (typeof options.ajax === 'object') {
                        var success = options.ajax.success;
                        delete options.ajax.success;
                    }

                    admin.req($.extend(true, {
                        url: '/auth/code'
                        , type: 'get'
                        , data: {
                            phone: value
                        }
                        , success: function (res) {
                            layer.msg('验证码已发送至你的手机，请注意查收', {
                                icon: 1
                                , shade: 0
                            });
                            options.elemVercode.focus();
                            countDown();
                            success && success(res);
                        }
                    }, options.ajax));
                });
            }

            //屏幕类型
            , screen: function () {
                var width = $win.width()
                if (width > 1200) {
                    return 3; //大屏幕
                } else if (width > 992) {
                    return 2; //中屏幕
                } else if (width > 768) {
                    return 1; //小屏幕
                } else {
                    return 0; //超小屏幕
                }
            }

            //侧边伸缩
            , sideFlexible: function (status) {
                var app = container
                    , iconElem = $('#' + APP_FLEXIBLE)
                    , screen = admin.screen();

                //设置状态，PC：默认展开、移动：默认收缩
                if (status === 'spread') {
                    //切换到展开状态的 icon，箭头：←
                    iconElem.removeClass(ICON_SPREAD).addClass(ICON_SHRINK);

                    //移动：从左到右位移；PC：清除多余选择器恢复默认
                    if (screen < 2) {
                        app.addClass(APP_SPREAD_SM);
                    } else {
                        app.removeClass(APP_SPREAD_SM);
                    }

                    app.removeClass(SIDE_SHRINK)
                } else {
                    //切换到搜索状态的 icon，箭头：→
                    iconElem.removeClass(ICON_SHRINK).addClass(ICON_SPREAD);

                    //移动：清除多余选择器恢复默认；PC：从右往左收缩
                    if (screen < 2) {
                        app.removeClass(SIDE_SHRINK);
                    } else {
                        app.addClass(SIDE_SHRINK);
                    }

                    app.removeClass(APP_SPREAD_SM)
                }

                layui.event.call(this, MOD_NAME, 'side({*})', {
                    status: status
                });
            }


            //弹出面板
            , modal: function (options) {
                var success = options.success
                    , skin = options.skin;

                delete options.success;
                delete options.skin;

                return layer.open($.extend({
                    type: 1,
                    title: '提示',
                    content: '',
                    id: 'LAY-system-view-popup',
                    skin: 'layui-layer-admin' + (skin ? ' ' + skin : ''),
                    shade: .65,
                    closeBtn: false,
                    success: function (layero, index) {
                        var elemClose = $('<i class="layui-icon" close>&#x1006;</i>');
                        layero.append(elemClose);
                        elemClose.on('click',
                            function () {
                                layer.close(index);
                            });
                        typeof success === 'function' && success.apply(this, arguments);
                    }
                },
                    options));
            }

            , error: function (content, options) {
                return admin.modal($.extend({
                    content: content,
                    maxWidth: 300
                    //,shade: 0.01
                    ,
                    offset: 't',
                    anim: 6,
                    id: 'LAY_adminError'
                },
                    options));
            }

            //右侧面板
            , modalRight: function (options) {
                //layer.close(admin.modal.index);
                return admin.modal.index = layer.open($.extend({
                    type: 1
                    , id: 'LAY_adminPopupR'
                    , anim: -1
                    , title: false
                    , closeBtn: false
                    , offset: 'r'
                    , shade: 0.1
                    , shadeClose: true
                    , skin: 'layui-anim layui-anim-rl layui-layer-adminRight'
                    , area: '300px'
                }, options));
            }

            //resize事件管理
            , resize: function (fn) {
                var router = layui.router()
                    , key = router.path.join('-');

                if (admin.resizeFn[key]) {
                    $win.off('resize', admin.resizeFn[key]);
                    delete admin.resizeFn[key];
                }

                if (fn === 'off') return; //如果是清除 resize 事件，则终止往下执行

                fn(), admin.resizeFn[key] = fn;
                $win.on('resize', admin.resizeFn[key]);
            }
            , resizeFn: {}
            , runResize: function () {
                var router = layui.router()
                    , key = router.path.join('-');
                admin.resizeFn[key] && admin.resizeFn[key]();
            }
            , delResize: function () {
                this.resize('off');
            }
            , toUpper: function (n) {
                n = n + "".replace(',', '');
                if (!/^(0|[1-9]\d*)(\.\d+)?$/.test(n)) return "数据非法";
                var unit = "京亿万仟佰拾兆万仟佰拾亿仟佰拾万仟佰拾元角分", str = "";
                n += "00";
                var p = n.indexOf('.');
                if (p >= 0)
                    n = n.substring(0, p) + n.substr(p + 1, 2);
                unit = unit.substr(unit.length - n.length);
                for (var i = 0; i < n.length; i++) str += '零壹贰叁肆伍陆柒捌玖'.charAt(n.charAt(i)) + unit.charAt(i);
                return str.replace(/零(仟|佰|拾|角)/g, "零").replace(/(零)+/g, "零").replace(/零(兆|万|亿|元)/g, "$1").replace(/(兆|亿)万/g, "$1").replace(/(京|兆)亿/g, "$1").replace(/(京)兆/g, "$1").replace(/(京|兆|亿|仟|佰|拾)(万?)(.)仟/g, "$1$2零$3仟").replace(/^元零?|零分/g, "").replace(/(元|角)$/g, "$1整");
            }
        };

    //事件
    var events = admin.events = {
        //伸缩
        flexible: function (othis) {
            var iconElem = othis.find('#' + APP_FLEXIBLE)
                , isSpread = iconElem.hasClass(ICON_SPREAD);
            admin.sideFlexible(isSpread ? 'spread' : '');
        }

        //点击消息
        , message: function (othis) {
            othis.find('.layui-badge-dot').remove();
        }

        //返回上一页
        , back: function () {
            history.back();
        }

        //遮罩
        , shade: function () {
            admin.sideFlexible();
        }
        , logout: function () {
            admin.logout();
        }
    };

    //监听侧边导航点击事件
    element.on('nav(layadmin-system-side-menu)', function (elem) {
        if (elem.siblings('.layui-nav-child')[0] && container.hasClass(SIDE_SHRINK)) {
            admin.sideFlexible('spread');
            layer.close(elem.data('index'));
        };
    });

    //点击事件
    $body.on('click', '*[layadmin-event]', function () {
        var othis = $(this)
            , attrEvent = othis.attr('layadmin-event');
        events[attrEvent] && events[attrEvent].call(this, othis);
    });

    //tips
    $body.on('mouseenter', '*[lay-tips]', function () {
        var othis = $(this);

        if (othis.parent().hasClass('layui-nav-item') && !container.hasClass(SIDE_SHRINK)) return;

        var tips = othis.attr('lay-tips')
            , offset = othis.attr('lay-offset')
            , direction = othis.attr('lay-direction')
            , index = layer.tips(tips, this, {
                tips: direction || 1
                , time: -1
                , success: function (layero, index) {
                    if (offset) {
                        layero.css('margin-left', offset + 'px');
                    }
                }
            });
        othis.data('index', index);
    }).on('mouseleave', '*[lay-tips]', function () {
        layer.close($(this).data('index'));
    });

    //窗口resize事件
    var resizeSystem = layui.data.resizeSystem = function () {
        //layer.close(events.note.index);
        layer.closeAll('tips');

        if (!resizeSystem.lock) {
            setTimeout(function () {
                admin.sideFlexible(admin.screen() < 2 ? '' : 'spread');
                delete resizeSystem.lock;
            }, 100);
        }

        resizeSystem.lock = true;
    }
    $win.on('resize', layui.data.resizeSystem);

    //接口输出
    exports('admin', admin);
});