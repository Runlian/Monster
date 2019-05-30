layui.extend({
    dompurify: "lib/extend/simditor/scripts/dompurify",
    module: "lib/extend/simditor/scripts/module",
    hotkeys: "lib/extend/simditor/scripts/hotkeys",
    uploader: "lib/extend/simditor/scripts/uploader",
    simditor: "lib/extend/simditor/scripts/simditor"
}).define("simditor", function (exports) {

	layui.link("/Content/lib/extend/simditor/styles/simditor.css");
    var $ = layui.$,
        Simditor = layui.simditor;

    Simditor.locale = "zh-CN";
    var toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', 'code', 'table', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
    //mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
    //if (mobilecheck()) {
    // toolbar = mobileToolbar;
    //}

    var editor = function (options) {
        var elem = $(options.elem);
        if (!elem[0]) {
            return layui.hint.error("没有找到" + options.elem + "元素");
        }
        var config = {
	        textarea: elem,
	        placeholder:  "这里输入文字...",
	        toolbar:  toolbar,
	        defaultImage: '',
	        params:  {},
	        upload: {
                url: '/Upload/Image',
                fileKey: 'file',
                connectionCount: 5,
                leaveConfirm:'文件正在上传中，您确定要离开？'
	        },
	        tabIndent:  true,
	        toolbarFloat: true,
	        toolbarFloatOffset: 0,
	        toolbarHidden:  false,
	        pasteImage:  true,
            cleanPaste: false,
            imageButton: 'upload'
        }
        config = $.extend({}, config, options);

        var obj = new Simditor(config);

        return obj;
    }

    //layui.link("lib/extend/simditor/styles/mobile.css");

    exports("editor", editor);
})