using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;

namespace Ruico.Infrastructure.Utility.Helper
{
    public class ExcelHelper
    {
        #region 从datatable中将数据导出到excel

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dt">源DataTable</param>
        /// <param name="sheetName">目标Excel工作表名</param>
        public static MemoryStream DataTableToStream(DataTable dt, string sheetName = "Sheet1")
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);

            ICellStyle HeadercellStyle = workbook.CreateCellStyle();
            HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //字体
            NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
            headerfont.Boldweight = (short) FontBoldWeight.Bold;
            HeadercellStyle.SetFont(headerfont);


            //用column name 作为列名
            int icolIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn item in dt.Columns)
            {
                ICell cell = headerRow.CreateCell(icolIndex);
                cell.SetCellValue(item.ColumnName);
                cell.CellStyle = HeadercellStyle;
                icolIndex++;
            }

            ICellStyle cellStyle = workbook.CreateCellStyle();

            //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


            NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
            cellfont.Boldweight = (short) FontBoldWeight.Normal;
            cellStyle.SetFont(cellfont);

            //建立内容行
            int iRowIndex = 1;
            int iCellIndex = 0;
            foreach (DataRow Rowitem in dt.Rows)
            {
                IRow DataRow = sheet.CreateRow(iRowIndex);
                foreach (DataColumn Colitem in dt.Columns)
                {

                    ICell cell = DataRow.CreateCell(iCellIndex);
                    cell.SetCellValue(Rowitem[Colitem].ToString());
                    cell.CellStyle = cellStyle;
                    iCellIndex++;
                }
                iCellIndex = 0;
                iRowIndex++;
            }

            //自适应列宽度
            for (int i = 0; i < icolIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            var ms = new MemoryStream();

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            return ms;
        }

        #endregion

        #region 从Excel文件中将数据转换为DataTable

        /// <summary>
        /// Excel文件导成DataTable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="tableName">Datatable表名称</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strFilePath, string tableName, string sheetName = "")
        {
            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(tableName))
            {
                dt.TableName = tableName;
            }

            if (!strExtName.IsNullOrBlank() && (strExtName.Equals(".xls") || strExtName.Equals(".xlsx")))
            {
                using (FileStream stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    return ExcelToDataTable(stream, tableName, sheetName);
                }
            }

            return dt;
        }

        /// <summary>
        /// Excel文件导成DataTable
        /// </summary>
        /// <param name="stream">Excel文件流</param>
        /// <param name="tableName">Datatable表名</param>
        /// <param name="sheetName">Excel sheet index</param>
        /// <returns></returns>
        public static
        DataTable ExcelToDataTable(Stream stream, string tableName, string sheetName = "")
        {

            DataTable dt = new DataTable();

            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = null;
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                sheet = workbook.GetSheetAt(0);
            }
            else
            {
                sheet = workbook.GetSheet(sheetName);
            }

            //列头
            foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
            {
                dt.Columns.Add(item.ToString(), typeof(string));
            }

            //写入内容
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                if (row.RowNum == sheet.FirstRowNum)
                {
                    continue;
                }

                DataRow dr = dt.NewRow();
                foreach (ICell item in row.Cells)
                {
                    switch (item.CellType)
                    {
                        case CellType.Boolean:
                            dr[item.ColumnIndex] = item.BooleanCellValue;
                            break;
                        case CellType.Error:
                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                            break;
                        case CellType.Formula:
                            switch (item.CachedFormulaResultType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string str = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        dr[item.ColumnIndex] = str.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                            break;
                        case CellType.Numeric:
                            if (DateUtil.IsCellDateFormatted(item))
                            {
                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                            }
                            else
                            {
                                dr[item.ColumnIndex] = item.NumericCellValue;
                            }
                            break;
                        case CellType.String:
                            string strValue = item.StringCellValue;
                            if (string.IsNullOrEmpty(strValue))
                            {
                                dr[item.ColumnIndex] = strValue.ToString();
                            }
                            else
                            {
                                dr[item.ColumnIndex] = null;
                            }
                            break;
                        case CellType.Unknown:
                        case CellType.Blank:
                        default:
                            dr[item.ColumnIndex] = string.Empty;
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion
    }
}
