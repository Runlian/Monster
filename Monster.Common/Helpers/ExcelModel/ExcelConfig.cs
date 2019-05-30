using System.Collections.Generic;
using System.Drawing;

namespace Monster.Common
{
    public class ExcelConfig
    {/// <summary>
     /// 文件名
     /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public Color Background { get; set; }
        private short _titlepoint;
        /// <summary>
        /// 标题字号
        /// </summary>
        public short TitlePoint
        {
            get { return _titlepoint == 0 ? (short) 20 : _titlepoint; }
            set { _titlepoint = value; }
        }

        public string SubTitle { get; set; }

        private short _subtitlepoint;
        /// <summary>
        /// 标题字号
        /// </summary>
        public short SubTitlePoint
        {
            get { return _subtitlepoint == 0 ? (short)10 : _subtitlepoint; }
            set { _subtitlepoint = value; }
        }

        private short _headpoint;
        /// <summary>
        /// 列头字号
        /// </summary>
        public short HeadPoint
        {
            get { return _headpoint == 0 ? (short) 10 : _headpoint; }
            set { _headpoint = value; }
        }
        /// <summary>
        /// 标题高度
        /// </summary>
        public short TitleHeight { get; set; }
        /// <summary>
        /// 列标题高度
        /// </summary>
        public short HeadHeight { get; set; }
        private string _titlefont;
        /// <summary>
        /// 标题字体
        /// </summary>
        public string TitleFont
        {
            get { return _titlefont ?? "微软雅黑"; }
            set { _titlefont = value; }
        }
        private string _headfont;
        /// <summary>
        /// 列头字体
        /// </summary>
        public string HeadFont
        {
            get { return _headfont ?? "微软雅黑"; }
            set { _headfont = value; }
        }
        /// <summary>
        /// 是否按内容长度来适应表格宽度
        /// </summary>
        public bool IsAllSizeColumn { get; set; }
        /// <summary>
        /// 列设置
        /// </summary>
        public List<ColumnEntity> ColumnEntity { get; set; }
    }
}