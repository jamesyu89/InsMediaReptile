using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InstagramPhotos.CodeGender.Coder;
using InstagramPhotos.CodeGender.Helper;

namespace InstagramPhotos.CodeGender.DB
{
    public static class DataAccess
    {
        public static string conn;

        public static string configPatch = System.Windows.Forms.Application.StartupPath + "\\StrConns.txt";


        private static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(conn);
        }

        public static string GetDatabase()
        {
            string dbname;
            using (SqlConnection conn = GetSqlConnection())
            {
                dbname = conn.Database;
            }
            return dbname;
        }


        public static List<string> GetTables()
        {
            List<string> tables = new List<string>();
            using (SqlConnection conn = GetSqlConnection())
            {
                using (SqlDataReader dr = SqlHelper.ExecuteReader(conn,
                    "sp_tables",
                    new SqlParameter("@table_type", "'TABLE'")))
                {
                    while (dr.Read())
                    {
                        tables.Add(Convert.ToString(dr["TABLE_NAME"]));
                    }
                }
            }
            return tables;
        }

        public static Dictionary<string, string> GetTablesWithCommaRemark()
        {
            string sql = @"
select A.[name] AS TABLE_NAME,B.VALUE as REMARK
from ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table',NULL, NULL, default) B
FULL outer JOIN sysobjects A 
ON A.NAME = B.objname COLLATE DATABASE_DEFAULT
where 
	A.xtype = 'U' 
and A.[name]<>'dtproperties' 
and A.[name]<>'syssegments' 
and A.[name]<>'sysconstraints' 
AND A.[NAME]<>'sysdiagrams'
AND B.NAME IS NULL OR B.NAME = 'MS_Description'
order by A.xtype,A.[name]
";
            Dictionary<string, string> tables = new Dictionary<string, string>();
            using (SqlConnection conn = GetSqlConnection())
            {
                using (SqlDataReader dr = SqlHelper.QueryReader(conn, sql))
                {
                    while (dr.Read())
                    {
                        tables.Add(Convert.ToString(dr["TABLE_NAME"]), dr["REMARK"] == null ? string.Empty : Convert.ToString(dr["REMARK"]));
                        //tables.Add(Convert.ToString(dr["TABLE_NAME"]));
                    }
                }
            }
            return tables;
        }

        public static List<Column> GetColumns(string tablename)
        {
            List<Column> columns = new List<Column>();
            using (SqlConnection conn = GetSqlConnection())
            {
                string sql = @"
 SELECT     
          TableDesc=ISNULL(CASE   WHEN   C.column_id=1   THEN   PTB.[value]   END,N''),   
          Column_id=C.column_id,   
          ColumnName=C.name,   
          PrimaryKey=CASE   WHEN   IDX.PrimaryKey=N'√'   THEN   1 ELSE   0   END,   
          [IDENTITY]=C.is_identity,   
          Computed=C.is_computed,   
          Type=T.name,   
          Length=C.max_length,   
          Precision=C.precision,   
          Scale=C.scale,   
          NullAble=C.is_nullable,   
          [Default]=ISNULL(D.definition,N''),   
          ColumnDesc=ISNULL(PFD.[value],N''),   
          IndexName=ISNULL(IDX.IndexName,N''),   
          IndexSort=ISNULL(IDX.Sort,N''),   
          Create_Date=O.Create_Date,   
          Modify_Date=O.Modify_date   
  FROM   sys.columns   C   
          INNER   JOIN   sys.objects   O   
                  ON   C.[object_id]=O.[object_id]   
                          AND   O.type='U'   
                          AND   O.is_ms_shipped=0   
          INNER   JOIN   sys.types   T   
                  ON   C.user_type_id=T.user_type_id   
          LEFT   JOIN   sys.default_constraints   D   
                  ON   C.[object_id]=D.parent_object_id   
                          AND   C.column_id=D.parent_column_id   
                          AND   C.default_object_id=D.[object_id]   
          LEFT   JOIN   sys.extended_properties   PFD   
                  ON   PFD.class=1     
                          AND   C.[object_id]=PFD.major_id     
                          AND   C.column_id=PFD.minor_id   
  --                           AND   PFD.name='Caption'     --   字段说明对应的描述名称(一个字段可以添加多个不同name的描述)   
          LEFT   JOIN   sys.extended_properties   PTB   
                  ON   PTB.class=1     
                          AND   PTB.minor_id=0     
                          AND   C.[object_id]=PTB.major_id   
  --                           AND   PFD.name='Caption'     --   表说明对应的描述名称(一个表可以添加多个不同name的描述)     
    
          LEFT   JOIN                                               --   索引及主键信息   
          (   
                  SELECT     
                          IDXC.[object_id],   
                          IDXC.column_id,   
                          Sort=CASE   INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending')   
                                  WHEN   1   THEN   'DESC'   WHEN   0   THEN   'ASC'   ELSE   ''   END,   
                          PrimaryKey=CASE   WHEN   IDX.is_primary_key=1   THEN   N'√'ELSE   N''   END,   
                          IndexName=IDX.Name   
                  FROM   sys.indexes   IDX   
                  INNER   JOIN   sys.index_columns   IDXC   
                          ON   IDX.[object_id]=IDXC.[object_id]   
                                  AND   IDX.index_id=IDXC.index_id   
                  LEFT   JOIN   sys.key_constraints   KC   
                          ON   IDX.[object_id]=KC.[parent_object_id]   
                                  AND   IDX.index_id=KC.unique_index_id   
                  INNER   JOIN     --   对于一个列包含多个索引的情况,只显示第1个索引信息   
                  (   
                          SELECT   [object_id],   Column_id,   index_id=MIN(index_id)   
                          FROM   sys.index_columns   
                          GROUP   BY   [object_id],   Column_id   
                  )   IDXCUQ   
                          ON   IDXC.[object_id]=IDXCUQ.[object_id]   
                                  AND   IDXC.Column_id=IDXCUQ.Column_id   
                                  AND   IDXC.index_id=IDXCUQ.index_id   
          )   IDX   
                  ON   C.[object_id]=IDX.[object_id]   
                          AND   C.column_id=IDX.column_id     
    
   WHERE   O.name=@tablename AND T.name<>'timestamp'             --   如果只查询指定表,加上此条件   
  ORDER   BY   O.name,C.column_id     

";
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@tablename", tablename);
                    conn.Open();
                    using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            columns.Add(new Column(dr));
                        }
                    }
                    conn.Close();

                }

            }
            return columns;
        }

        public static List<CodeMark> GetCodeMarks(string tablename)
        {
            List<CodeMark> codes = new List<CodeMark>();
            using (SqlConnection conn = GetSqlConnection())
            {
                using (SqlDataReader dr = SqlHelper.QueryReader(conn,
                    "select * from " + tablename))
                {
                    while (dr.Read())
                    {
                        codes.Add(new CodeMark(dr));
                    }
                }
            }
            return codes;
        }
    }


    /// <summary>
    /// CodeMark
    /// </summary>
    public class CodeMark
    {

        /// <summary>
        /// DrawCashState 构造函数
        /// </summary>
        public CodeMark()
        { }


        /// <summary>
        /// DrawCashState OR映射构造函数
        /// </summary>
        public CodeMark(System.Data.IDataReader dr)
        {
            this.CodeNo = Convert.ToInt32(dr[0]);
            this.CodeName = Convert.ToString(dr[1]);
            this.Description = Convert.ToString(dr[2]);
        }


        /// <summary>
        /// 状态编号
        /// </summary>
        public int CodeNo { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 说明，备注
        /// </summary>
        public string Description { get; set; }
    }

}
