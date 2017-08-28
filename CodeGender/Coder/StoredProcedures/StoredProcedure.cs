using System;
using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    /// <summary>
    /// 存储过程
    /// </summary>
    public abstract class StoredProcedure
    {
        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 存储过程描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 存储过程参数
        /// </summary>
        public List<StoredProcedureParameter> Parameters { get; set; }

        public StoredProcedure(string name, string author)
        {
            this.Name = name;
            this.Author = author;
        }

        public StoredProcedure()
        {
            
        }

        public bool HasParameter
        {
            get
            {
                return Parameters != null && Parameters.Count > 0;
            }
        }

        /// <summary>
        /// 需要为副键生成的集合
        /// </summary>
        public List<Column> AssistantColumn;

        /// <summary>
        /// 所需生成的副键列存储过程名集合
        /// </summary>
        public List<string> AssistantColumnProName;

        /// <summary>
        /// 存储过程内容
        /// </summary>
        public abstract string Body { get; }

        public string Sql
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("-- =============================================");
                sql.AppendLineFormat("-- Author:            {0}", Author);
                sql.AppendLineFormat("-- Create date:       {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                sql.AppendLineFormat("-- Description:       {0}", Description);
                sql.AppendLine("-- =============================================");
                sql.AppendLineFormat("CREATE PROCEDURE {0}", Name);
                if (HasParameter)
                {
                    sql.AppendLine("(");
                    bool first = true;
                    int order = 0;
                    foreach (StoredProcedureParameter param in Parameters)
                    {
                        param.Order = order++;
                        sql.Append("\t");
                        if (!first)
                            sql.Append(",");
                        else
                        {
                            sql.Append(" ");
                            first = false;
                        }
                        sql.AppendLine(param.Sql);
                    }
                    sql.AppendLine(")");
                }
                sql.AppendLine("AS");
                sql.AppendLine("BEGIN");

                sql.AppendLine();
                sql.AppendLine(Body.IncreaseIndent(1));
                sql.AppendLine();

                sql.AppendLine("END");
                sql.AppendLine("GO");
                return sql.ToString();
            }
        }

        /// <summary>
        /// 根据副键集合生成SQL(查询副键IDS)
        /// </summary>
        public string Sqls
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                int i = 0;
                
                foreach (Column column in AssistantColumn)
                {
                    i++;
                    sql.AppendLine("-- =============================================");
                    sql.AppendLineFormat("-- Author:            {0}", Author);
                    sql.AppendLineFormat("-- Create date:       {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sql.AppendLineFormat("-- Description:       {0}", Description);
                    sql.AppendLine("-- =============================================");
                    sql.AppendLineFormat("CREATE PROCEDURE {0}", Name.Split('_')[0] + "_" + Name.Split('_')[1] + "_" + Name.Split('_')[2] + "_Get" + AssistantColumn[i - 1].Name + "s");
                    if (HasParameter)
                    {
                        sql.AppendLine("(");
                        bool first = true;
                        int order = 0;
                        foreach (StoredProcedureParameter param in Parameters)
                        {
                            param.Order = order++;
                            sql.Append("\t");
                            if (!first)
                                sql.Append(",");
                            else
                            {
                                sql.Append(" ");
                                first = false;
                            }
                            sql.AppendLine(param.Sql);
                        }
                        sql.AppendLine(")");
                    }
                    sql.AppendLine("AS");
                    sql.AppendLine("BEGIN");
                    sql.AppendLine();
                    sql.AppendLine(GetAssistantSP.SqlStr(AssistantColumn[i - 1]).IncreaseIndent(1));
                    sql.AppendLine();
                    sql.AppendLine("END");
                    sql.AppendLine("GO");
                    sql.AppendLine("\r\n");
                }
                return sql.ToString();
            }
        }

        /// <summary>
        /// 根据副键ids集合查询实体生成SQL
        /// </summary>
        public string SqlEntitys
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                int i = 0;
                foreach (Column column in AssistantColumn)
                {
                    i++;
                    sql.AppendLine("-- =============================================");
                    sql.AppendLineFormat("-- Author:            {0}", Author);
                    sql.AppendLineFormat("-- Create date:       {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sql.AppendLineFormat("-- Description:       {0}", Description);
                    sql.AppendLine("-- =============================================");
                    sql.AppendLineFormat("CREATE PROCEDURE {0}", AssistantColumnProName[i - 1]);
                    if (HasParameter)
                    {
                        sql.AppendLine("(");
                        bool first = true;
                        int order = 0;
                        foreach (StoredProcedureParameter param in Parameters)
                        {
                            param.Order = order++;
                            sql.Append("\t");
                            if (!first)
                                sql.Append(",");
                            else
                            {
                                sql.Append(" ");
                                first = false;
                            }
                            sql.AppendLine(param.Sql);
                        }
                        sql.AppendLine(")");
                    }
                    sql.AppendLine("AS");
                    sql.AppendLine("BEGIN");
                    sql.AppendLine();
                    sql.AppendLine(GetAssistantEntitiesSP.SqlStr(AssistantColumn[i - 1]).IncreaseIndent(1));
                    sql.AppendLine();
                    sql.AppendLine("END");
                    sql.AppendLine("GO");
                    sql.AppendLine("\r\n");
                }
                return sql.ToString();
            }
        }
    }
}
