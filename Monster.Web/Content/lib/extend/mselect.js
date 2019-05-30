layui.define("form", function (exports) {
    var MOD_NAME = "mselect",
        $ = layui.jquery,
        form = layui.form,
        selecteds = [],
        Mselect = function () { };

    Mselect.prototype.init = function () {
        form.render("select");
        var that = this;
        $("select[multiple]").each(function (index, item) {
            var othis = $(this),
                vals = [];
            othis.find("option:selected").each(function () {
                vals.push($(this).val());
            });
            othis.next(".layui-form-select").addClass("multiple").find(".layui-select-title").click(function () {
                console.log(selecteds[index].join(","))
                selecteds[index] && $(this).find("input").val(selecteds[index].join(","));
            }).next().find("dd").each(function () {
                var dt = $(this),
                    checked = (dt.hasClass("layui-this") || $.inArray(dt.attr("lay-value"), vals) > -1) ? "checked" : "",
                    title = dt.text(),
                    disabled = dt.attr("lay-value") === "" ? "disabled" : "";
                dt.html("<input type='checkbox' lay-skin='primary' title='" + title + "' " + checked + " " + disabled + " >");
                that.selected(index, othis, dt);
            }).click(function (e) {
                var dt = $(this);
                if (e.target.localName === "dd" && dt.attr("lay-value") !== "") {
                    var status = dt.find(".layui-form-checkbox").toggleClass("layui-form-checked").hasClass("layui-form-checked");
                    dt.find("input").prop("checked", status);
                }
                dt.parents(".layui-form-select").addClass("layui-form-selected");
                that.selected(index, othis, dt);
            });
        });
         form.render("checkbox");
    }

    Mselect.prototype.selected = function (index, othis, dt) {
        selecteds[index] = [];
        othis.find("option").prop("selected", false);
        dt.parents("dl").find("[type=checkbox]:checked").each(function () {
            var val = $(this).parent().attr("lay-value");
            othis.find("option[value=" + val + "]").prop("selected", true);
            selecteds[index].push($(this).attr("title"));
        });
        dt.parents("dl").prev().find("input").val(selecteds[index].join(","));
    }
    Mselect.prototype.render = function (type, filter) {
        form.render(type, filter);
        this.init();
    }

    var obj = new Mselect();
    //obj.init();

    exports(MOD_NAME, obj);
})