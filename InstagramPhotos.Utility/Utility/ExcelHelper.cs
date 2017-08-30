using System.Data;
using System.Data.OleDb;

namespace InstagramPhotos.Utility.Utility
{
    public static class ExcelHelper
    {
        public static DataSet ReadDataForExcel(string filePath)
        {
            var conStr = string.Format("Provider=Microsoft.ACE.OleDb.12.0;data Source={0};Extended Properties='Excel 12.0;HDR=yes;IMEX=1';", filePath);
            var conn = new OleDbConnection(conStr);
            var oada = new OleDbDataAdapter("select * from [Sheet1$]", conn);
            DataSet result = new DataSet();
            oada.Fill(result);
            return result;
        }
    }
}
