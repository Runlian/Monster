layui.define('table', function (exports) {
    var $ = layui.jquery;
    var mod = {
        render: function (tb) {
            var tableBox = $(tb.elem).next().children(".layui-table-box"),
                $main = $(tableBox.children(".layui-table-body").children("table").children("tbody").children("tr").toArray().reverse()),
                $fixLeft = $(tableBox.children(".layui-table-fixed-l").children(".layui-table-body").children("table").children("tbody").children("tr").toArray().reverse()),
                $fixRight = $(tableBox.children(".layui-table-fixed-r").children(".layui-table-body").children("table").children("tbody").children("tr").toArray().reverse()),
                cols = tb.cols[0],
                mergeRecord = {};

            for (var i = 0; i < cols.length; i++) {
                var item = cols[i], field = item.field;
                if (item.merge) {
                    var mergeField = [field];
                    if (item.merge !== true) {
                        if (typeof item.merge == "string") {
                            mergeField = [item.merge];
                        } else {
                            mergeField = item.merge;
                        }
                    }
                    mergeRecord[i] = { mergeField: mergeField, rowspan: 1 }
                }
            }

            $main.each(function (index) {
                for (var item in mergeRecord) {
                    if (index === $main.length - 1 || isMaster(index, item)) {
                        $(this).children("[data-key$='-" + item + "']").attr("rowspan", mergeRecord[item].rowspan).css("position", "static");
                        $fixLeft.eq(index).children("[data-key$='-" + item + "']").attr("rowspan", mergeRecord[item].rowspan).css("position", "static");
                        $fixRight.eq(index).children("[data-key$='-" + item + "']").attr("rowspan", mergeRecord[item].rowspan).css("position", "static");
                        mergeRecord[item].rowspan = 1;
                    } else {
                        $(this).children("[data-key$='-" + item + "']").remove();
                        $fixLeft.eq(index).children("[data-key$='-" + item + "']").remove();
                        $fixRight.eq(index).children("[data-key$='-" + item + "']").remove();
                        mergeRecord[item].rowspan += 1;
                    }
                }

            });
            function isMaster(index, item) {
                var mergeField = mergeRecord[item].mergeField;
                var dataLength = layui.table.cache[tb.id].length;
                for (var i = 0; i < mergeField.length; i++) {
                    if (layui.table.cache[tb.id][dataLength - 2 - index][mergeField[i]] !==
                        layui.table.cache[tb.id][dataLength - 1 - index][mergeField[i]]) {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    exports("mtable", mod);
})