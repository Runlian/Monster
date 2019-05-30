layui.extend({
    admin: "lib/admin", //核心模块
    pjax: "lib/pjax",
    nprogress: "lib/nprogress"
}).define(["element", "pjax", "nprogress", "admin", "table"], function (exports) {
    var $ = layui.jquery,
        element = layui.element,
        Pjax = layui.pjax,
        nprogress = layui.nprogress,
        admin = layui.admin,
        table = layui.table;

    var extend = [
        "echarts", //echarts 核心包
        "echartsTheme", //echarts 主题
        "mtable",
        "mselect",
        "tselect",
        "cascader",
        "editor",
        "swiper",
        "grid",
        "dtree"
    ];

    //初始
    if (admin.screen() < 2) admin.sideFlexible();

    $("#LAY-system-side-menu li a").each(function () {
        if (window.location.pathname === $(this).attr('href')) {
            $("#LAY-system-side-menu li").removeClass('layui-nav-itemed');
            $(this).parents('.layui-nav-item').addClass('layui-nav-itemed');
            $(this).parent().addClass('layui-this').siblings().removeClass('layui-this');
        }
    });

    // 局部刷新内容
    window.pjax = new Pjax({
        elements: ["a[data-pjax]"],
        selectors: ["#container", "title"],
        cacheBust: false
    });
    

    document.addEventListener('pjax:send', nprogress.start());
    document.addEventListener('pjax:complete', nprogress.done());

    //将模块根路径设置为 controller 目录
    layui.config({
        base: "/Content/"
    });

    //扩展 lib 目录下的其它模块
    layui.each(extend, function (index, item) {
        var mods = {};
        mods[item] = "/lib/extend/" + item;
        layui.extend(mods);
    });

    table.set({
        response: {
            statusCode: 200 //规定成功的状态码，默认：0
        },
        done: function (res, curr, count) {
            if (res.code === 403) {
                admin.logout();
            }
        }
    });

    if (!!window.localStorage) {
        for (var key in localStorage) {
            try {
                if ((key.split("_") || [""])[0] === "pjax") {
                    var item = localStorage.getItem(key);
                    if (item) {
                        item = JSON.parse(item);
                        if ((parseInt(item.time) + 600 * 1000) <= new Date * 1) {
                            localStorage.removeItem(key)
                        }
                    }
                }
            } catch (e) { }
        }
    }

    //对外输出
    exports("index", {});
})