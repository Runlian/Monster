layui.define('layer', function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,
        cascader = {
            config: {},
            data: [],
            temp: [],
            //设置全局项
            set: function (options) {
                var that = this;
                that.config = $.extend({}, that.config, options);
                return that;
            },
            //事件监听

            on: function (events, callback) {
                return layui.onevent.call(this, MOD_NAME, events, callback);
            }
        },

        //构造器

        Class = function (options) {
            var that = this;
            that.config = $.extend({}, that.config, cascader.config, options);
            that.data = options.url ? that.getData(options.url) : options.data;
            that.render();
        },

        MOD_NAME = "cascader", MODAL = "cascader-modal", CHILDREN = "layui-children", DISABLED = "layui-disabled";

    // 默认配置
    Class.prototype.config = {
        data: [],
        url: '',
        placeholder: '请选择',
        disabledTips: '',
        success: function () { }
    };

    Class.prototype.render = function () {
        var that = this,
            options = that.config;

        options.elem = $(options.elem);
        var box = '<div class="cascader-box"></div>';
        var modal = '<div class="cascader-modal"></div>';
        options.elem.wrap(box);
        options.elem.after(modal);

        that.popup(that.data);

        if (options.elem.val()) {
            var names = options.elem.val().split("/");
            var data;
            layui.each(names, function (i, item) {
                var query = that.getChildrenByName(data || that.data, item);
                data = query.children;
                if (data) {
                    that.popup(data);
                }
                var uls = options.elem.parent().find("." + MODAL + " ul");
                uls.eq(i).find("li[data-id='" + query.id + "']").addClass("active");
            });
        }

        options.elem.parent().find("." + MODAL).on("click", "ul li", function (e) {
            if ($(this).hasClass(DISABLED)) {
                layer.msg(options.disabledTips);
                return false;
            }
            $(this).addClass('active').siblings().removeClass('active');
            $(this).parent().nextAll('ul').remove();

            var index = [];
            options.elem.parent().find("." + MODAL + " li.active").each(function () {
                index.push($(this).data('index'));
            });

            var childrens = that.getChildren(that.data, index);
            if (childrens)
                that.popup(childrens);
            if (!$(this).hasClass(CHILDREN)) {
                options.elem.parent().find("." + MODAL).hide();
                var activeLi = options.elem.parent().find("." + MODAL + " li.active"),
                    checkValue = [], ids = [];
                for (var i = 0; i < activeLi.length; i++) {
                    checkValue.push(activeLi.eq(i).html());
                    ids.push(activeLi.eq(i).data('id'));
                }
                var value = checkValue.join("/");
                options.elem.val(value);

                if (typeof options.success === "function") {
                    var callback = {
                        value: value,
                        data: checkValue,
                        ids: ids
                    };
                    options.success(callback);
                }
            }
            layui.stope(e);
        });

        options.elem.on("click", function (e) {
            $(this).parent().find("." + MODAL).css('display','flex');;
            that.scroll();
            layui.stope(e);
        });
        options.elem.on("focus", function (e) {
            $(this).parent().find("." + MODAL).css('display','flex');;
            that.scroll();
            layui.stope(e);
        });
        $(document).on("click", function () {
            options.elem.parent().find("." + MODAL).hide();
        })
    };

    Class.prototype.getData = function (url) {
        var data;
        $.ajax({
            url: url,
            type: "get",
            dataType: 'json',
            async: false,
            success: function (res) {
                data = res.data || layer.msg('数据接口错误，请正确配置');
            },
            error: function () {
                layer.msg('数据接口错误，请正确配置');
                data = '';
            }
        });
        return data;
    }

    Class.prototype.getChildren = function (data, index) {
        var item = data;
        for (var i = 0; i < index.length; i++) {
            if (typeof item[index[i]] === "undefined") {
                item = null;
                break;
            } else if (typeof item[index[i]]["children"] === "undefined") {
                item = null;
                break;
            } else {
                item = item[index[i]]["children"];
            }
        }
        return item;
    };

    Class.prototype.getChildrenByName = function (data, name) {
        var res = data.filter(function (item) { return item.name === name });
        return res[0];
    }

    Class.prototype.popup = function (data) {
        var that = this,
            options = that.config;


        options.elem = $(options.elem);

        if (data.length !== 0) {
            var ul = "<ul>";
            layui.each(data, function (index, item) {
                var hasChild = item.children && item.children.length > 0;
                var children = hasChild ? CHILDREN : "",
                    disabled = item.disabled ? DISABLED : "";
                ul += "<li class='" + children + " " + disabled + "' data-id='" + item.id + "' data-index='" + index + "'>" + item.name + "</li>";
            });
            ul += "</ul>";
            options.elem.parent().find("." + MODAL).append(ul);
        }
    };

    Class.prototype.scroll = function () {
        var that = this,
            options = that.config;

        options.elem = $(options.elem);

        var uls = options.elem.parent().find("." + MODAL + " ul");
        for (var i = 0; i < uls.length; i++) {
            var li = uls.eq(i).children("li.active");
            if (li[0]) {
                var top = li.position().top, ha = uls.eq(i).height(), hl = li.height();
                top > ha && uls.eq(i).scrollTop(top + uls.eq(i).scrollTop() - ha + hl + 3);
                top < 0 && uls.eq(i).scrollTop(top + uls.eq(i).scrollTop() - 5);
            }
        }
    };

    //核心入口
    cascader.render = function (options) {
        var inst = new Class(options);
        return inst;
    };

    exports(MOD_NAME, cascader);
});