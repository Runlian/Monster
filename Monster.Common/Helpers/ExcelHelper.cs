using Monster.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;

namespace Monster.Common
{
    public class ExcelHelper
    {
        #region Excel导出方法 ExcelDownload
        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelDownload(DataTable dtSource, ExcelConfig excelConfig)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(excelConfig.FileName, Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(ExportMemoryStream(dtSource, excelConfig).GetBuffer());
            curContext.Response.End();
        }
      
        public static void ExcelDownload(byte[] bytes, string fileName)
        {

            HttpContext curContext = HttpContext.Current;
            curContext.Response.Buffer = true;
            curContext.Response.Clear();
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(bytes);
            curContext.Response.End();
        }
        #endregion

        #region DataTable导出到Excel文件excelConfig中FileName设置为全路径
        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelExport(DataTable dtSource, ExcelConfig excelConfig)
        {
            using (MemoryStream ms = ExportMemoryStream(dtSource, excelConfig))
            {
                using (FileStream fs = new FileStream(excelConfig.FileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        #endregion

        #region DataTable导出到Excel的MemoryStream
        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static MemoryStream ExportMemoryStream(DataTable dtSource, ExcelConfig excelConfig)
        {
            for (int i = 0; i < dtSource.Columns.Count;)
            {
                bool isExists = false;
                DataColumn column = dtSource.Columns[i];
                for (int j = 0; j < excelConfig.ColumnEntity.Count; j++)
                {
                    if (excelConfig.ColumnEntity[j].Column == column.ColumnName)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    dtSource.Columns.Remove(column);
                }
                else
                {
                    i++;
                }
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = ""; //填加xls文件作者信息
                si.ApplicationName = ""; //填加xls文件创建程序信息
                si.LastAuthor = ""; //填加xls文件最后保存者信息
                si.Comments = ""; //填加xls文件作者信息
                si.Title = excelConfig.Title; //填加xls文件标题信息
                si.Subject = excelConfig.Title;//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            #region 设置标题样式
            ICellStyle headStyle = workbook.CreateCellStyle();
            int[] arrColWidth = new int[dtSource.Columns.Count];
            string[] arrColName = new string[dtSource.Columns.Count];//列名
            ICellStyle[] arryColumStyle = new ICellStyle[dtSource.Columns.Count];//样式表
            headStyle.Alignment = HorizontalAlignment.Center; // ------------------
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            if (excelConfig.Background != new Color())
            {
                if (excelConfig.Background != new Color())
                {
                    headStyle.FillPattern = FillPattern.SolidForeground;
                    headStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                }
            }
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = excelConfig.TitlePoint;
            if (excelConfig.ForeColor != new Color())
            {
                font.Color = GetXLColour(workbook, excelConfig.ForeColor);
            }
            //font.Boldweight = (short)FontBoldWeight.Bold;
            font.FontName = excelConfig.TitleFont;
            headStyle.SetFont(font);
            #endregion

            #region 设置副标题样式
            ICellStyle subTitleStyle = workbook.CreateCellStyle();
            subTitleStyle.Alignment = HorizontalAlignment.Left; // ------------------
            subTitleStyle.VerticalAlignment = VerticalAlignment.Center;
            if (excelConfig.Background != new Color())
            {
                if (excelConfig.Background != new Color())
                {
                    subTitleStyle.FillPattern = FillPattern.SolidForeground;
                    subTitleStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                }
            }
            IFont subfont = workbook.CreateFont();
            subfont.FontHeightInPoints = excelConfig.SubTitlePoint;
            if (excelConfig.ForeColor != new Color())
            {
                subfont.Color = GetXLColour(workbook, excelConfig.ForeColor);
            }

            //subfont.Boldweight = (short)FontBoldWeight.Bold;;
            subfont.FontName = excelConfig.HeadFont;
            subTitleStyle.SetFont(subfont);
            #endregion

            #region 列头及样式
            ICellStyle cHeadStyle = workbook.CreateCellStyle();
            cHeadStyle.Alignment = HorizontalAlignment.Center; // ------------------
            cHeadStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont cfont = workbook.CreateFont();
            cfont.FontHeightInPoints = excelConfig.HeadPoint;
            cfont.Boldweight = (short)FontBoldWeight.Bold;
            cfont.FontName = excelConfig.HeadFont ?? "宋体";
            cHeadStyle.SetFont(cfont);
            #endregion

            #region 设置内容单元格样式
            foreach (DataColumn item in dtSource.Columns)
            {
                ICellStyle columnStyle = workbook.CreateCellStyle();
                columnStyle.Alignment = HorizontalAlignment.Center;
                columnStyle.VerticalAlignment = VerticalAlignment.Center;
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                arrColName[item.Ordinal] = item.ColumnName.ToString();
                if (excelConfig.ColumnEntity != null)
                {
                    ColumnEntity columnentity = excelConfig.ColumnEntity.Find(t => t.Column == item.ColumnName);
                    if (columnentity != null)
                    {
                        arrColName[item.Ordinal] = columnentity.ExcelColumn;
                        if (columnentity.Width != 0)
                        {
                            arrColWidth[item.Ordinal] = columnentity.Width;
                        }
                        if (columnentity.Background != new Color())
                        {
                            if (columnentity.Background != new Color())
                            {
                                columnStyle.FillPattern = FillPattern.SolidForeground;
                                columnStyle.FillForegroundColor = GetXLColour(workbook, columnentity.Background);
                            }
                        }
                        IFont columnFont = workbook.CreateFont();
                        columnFont.FontHeightInPoints = 12;
                        columnFont.FontName = columnentity.Font ?? "宋体";
                        if (columnentity.Point != 0)
                        {
                            columnFont.FontHeightInPoints = columnentity.Point;
                        }
                        if (columnentity.ForeColor != new Color())
                        {
                            columnFont.Color = GetXLColour(workbook, columnentity.ForeColor);
                        }
                        columnStyle.SetFont(columnFont);
                        columnStyle.Alignment = GetAlignment(columnentity.Alignment);
                    }
                }
                arryColumStyle[item.Ordinal] = columnStyle;
            }
            if (excelConfig.IsAllSizeColumn)
            {
                #region 根据列中最长列的长度取得列宽
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        if (arrColWidth[j] != 0)
                        {
                            int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                            if (intTemp > arrColWidth[j])
                            {
                                arrColWidth[j] = intTemp;
                            }
                        }

                    }
                }
                #endregion
            }
            #endregion

            #region 填充数据

            #endregion
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
            dateStyle.Alignment = HorizontalAlignment.Center;
            dateStyle.VerticalAlignment = VerticalAlignment.Center;
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        if (excelConfig.Title != null)
                        {
                            IRow headerRow = sheet.CreateRow(0);
                            if (excelConfig.TitleHeight != 0)
                            {
                                headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                            }
                            headerRow.HeightInPoints = 35;
                            headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                        }

                    }
                    #endregion

                    #region 副表头及样式
                    {
                        if (excelConfig.SubTitle != null)
                        {
                            IRow headerRow = sheet.CreateRow(1);
                            if (excelConfig.TitleHeight != 0)
                            {
                                headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                            }
                            headerRow.HeightInPoints = 35;
                            headerRow.CreateCell(0).SetCellValue(excelConfig.SubTitle);
                            headerRow.GetCell(0).CellStyle = subTitleStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                        }

                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(2);
                        headerRow.HeightInPoints = 30;
                        #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                            if (arrColWidth[column.Ordinal] > 255)
                            {
                                arrColWidth[column.Ordinal] = 254;
                            }
                            else
                            {
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    rowIndex = 3;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.HeightInPoints = 30;
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = arryColumStyle[column.Ordinal];
                    string drValue = row[column].ToString();
                    SetCell(newCell, dateStyle, column.DataType, drValue);
                }
                #endregion
                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion


        #region 设置表格内容
        private static void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ExcelImport(string strFileName)
        {
            DataTable dt = new DataTable("Excel");

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static DataTable ExcelImport(Stream stream)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            XSSFWorkbook xssfworkbook = new XSSFWorkbook(stream);
            sheet = xssfworkbook.GetSheetAt(0);

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static DataTable ExcelImport(Stream stream, string fileName)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) == -1)//2003
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
                sheet = hssfworkbook.GetSheetAt(0);
            }
            else//2007
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(stream);
                sheet = xssfworkbook.GetSheetAt(0);
            }

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                bool emptyRow = true;
                object[] itemArray = null;
                if (row != null)
                {
                    itemArray = new object[row.LastCellNum];
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell.CellType == CellType.Numeric)
                            {
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    itemArray[j] = cell.DateCellValue;
                                }
                                else
                                {
                                    itemArray[j] = cell.NumericCellValue;
                                }
                            }
                            else if (cell.CellType == CellType.Blank)
                            {
                                itemArray[j] = "";
                            }
                            else
                            {
                                itemArray[j] = cell.StringCellValue;
                            }
                            if (itemArray[j] != null && !string.IsNullOrEmpty(itemArray[j].ToString().Trim()))
                            {
                                emptyRow = false;
                            }
                        }

                    }

                }

                //非空数据行数据添加到DataTable
                if (!emptyRow)
                {
                    dt.Rows.Add(itemArray);
                }
            }
            return dt;
        }
        
        #endregion

        #region RGB颜色转NPOI颜色
        private static short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    s = XlColour.Indexed;
                }

            }
            else
                s = XlColour.Indexed;
            return s;
        }
        #endregion

        #region 设置列的对齐方式
        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static HorizontalAlignment GetAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return HorizontalAlignment.Center;
                case "left":
                    return HorizontalAlignment.Left;
                case "right":
                    return HorizontalAlignment.Right;
                case "fill":
                    return HorizontalAlignment.Fill;
                case "justify":
                    return HorizontalAlignment.Justify;
                case "centerselection":
                    return HorizontalAlignment.CenterSelection;
                case "distributed":
                    return HorizontalAlignment.Distributed;
            }
            return HorizontalAlignment.General;


        }

        #endregion

        
    }
}
