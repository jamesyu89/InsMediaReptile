namespace InstagramPhotos.Utility.Data
{
    /// <summary>
    /// 各驱动ProviderName
    /// </summary>
    public class DbProviderName
    {
        public const string Aceess = "System.Data.OleDb";
        public const string Oracle = "System.Data.OracleClient";
        public const string MSSql = "System.Data.SqlClient";
        public const string MySql = "System.Data.Odbc";

        public const string ALL = "System.Data.Odbc";

    }

    /// <summary>
    /// 各个数据库DBKey
    /// </summary>
    public class DbKeyConst
    {
        public const string WMS_DBKey = "YGPS.WMS";
        public const string TMS_DBKey = "YGPS.TMS";
        public const string SCM_DBKey = "YGPS.SCM";
        public const string OTS_DBKey = "YGPS.OTS";
        public const string ERP_DBKey = "YGPS.EfruitERP_CN_SH";

        public const string TMP_WMS = "TMP_WMS";

        public const string WMS_TPP = "Exfresh.WMS.TPP";
    }
}
