using System.Collections.Generic;
using System.Data;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{

    public class CreateEntityDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        Column idColumn;
        List<Column> columns;
        bool enableParamCache;
        bool needAutoGuid;

        bool withTrans = false;//是否带SQL事务

        public CreateEntityDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache, bool needAutoGuid)
            : base(
                string.Format("Create{0}", entityClass)
            , "" + entityClass, null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.columns = columns;
            this.idColumn = idColumn;
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(entityClass.ToFirstLower(), entityClass));
            this.enableParamCache = enableParamCache;
            this.needAutoGuid = needAutoGuid;
        }

        public CreateEntityDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache, bool needAutoGuid, bool withTrans)
            : this(entityClass, storedProcedureName, columns, idColumn, enableParamCache, needAutoGuid)
        {
            this.withTrans = withTrans;
            if (withTrans)
                this.Parameters.Add(new FunctionParameter("tran","SqlTransaction"));
        }

        public override string Body
        {
            get
            {
                string paramEntity = entityClass.ToFirstLower();
                StringBuilder code = new StringBuilder();

                code.AppendLine("using (SqlConnection conn = GetSqlConnection())");
                code.AppendLine("{");

                if (enableParamCache)
                {
                    #region With paramcache

                    string autoId = "retId";

                    if (idColumn.CSTypeName == "int" || (idColumn.CSTypeName == "Guid" && needAutoGuid))
                    {
                        code.AppendLineFormatWithTabs("{0} {1};", 1, idColumn.CSTypeName, autoId);
                        code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery<{1}>(conn, {2} \"{0}\" ", 1, storedProcedureName, idColumn.CSTypeName, this.withTrans ? "tran, " : string.Empty);
                    }
                    else
                    {
                        code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery(conn, {1}\"{0}\"", 1, storedProcedureName, this.withTrans ? "tran, " : string.Empty);
                    }
                    bool first = true;
                    int order = 0;
                    foreach (Column column in columns)
                    {
                        code.Append("\t\t");
                        if (needAutoGuid)
                            if (!first)
                                code.Append(",");
                            else
                                first = false;
                        else
                        {
                            code.Append(",");
                        }
                        if (column.Name == idColumn.Name && needAutoGuid)
                        {
                            if (idColumn.CSTypeName == "int" || (idColumn.CSTypeName == "Guid" && needAutoGuid))
                                code.AppendLineFormat("\t\t ,out {0}// {1}.{2}【输出】", autoId.PadRight(26 + paramEntity.Length), order++, column.Remarks);
                            continue;
                        }
                        code.AppendLineFormat("{0}.{1}// {2}.{3}", paramEntity,column.Name==entityClass?("_"+column.Name).PadRight(29): column.Name.PadRight(29), order++, column.Remarks);
                    }

                    code.AppendLineWithTabs(");", 1);
                    if (idColumn.CSTypeName == "int" || (idColumn.CSTypeName == "Guid" && needAutoGuid))
                    {
                        code.AppendLineFormatWithTabs("{0}.{1} = {2};", 1, paramEntity, idColumn.Name, autoId);
                    }

                    #endregion
                }
                else
                {
                    #region Without paramcache

                    if (idColumn.CSTypeName == "int" || (idColumn.CSTypeName == "Guid" && needAutoGuid))
                    {
                        var type = SqlDbType.Int;
                        if (idColumn.CSTypeName == "Guid")
                            type = SqlDbType.UniqueIdentifier;
                        code.AppendLineFormatWithTabs("SqlParameter sp{0} = new SqlParameter(\"@{0}\", SqlDbType.{1});", 1, idColumn.Name, type);
                        code.AppendLineFormatWithTabs("sp{0}.Direction = ParameterDirection.Output;", 1, idColumn.Name);
                        code.AppendLineFormatWithTabs("sp{0}.Value = {1}.{0};", 1, idColumn.Name, paramEntity);
                        code.AppendLine();
                    }
                    code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery(conn, {1}\"{0}\",", 1, storedProcedureName, this.withTrans ? "tran, " : string.Empty);
                    bool first = true;
                    foreach (Column column in columns)
                    {
                        code.Append("\t\t");
                        if (needAutoGuid)
                            if (!first)
                                code.Append(",");
                            else
                                first = false;
                        else
                        {
                            code.Append(",");
                        }
                        if ((column.Name == idColumn.Name && needAutoGuid) && (idColumn.CSTypeName == "int" || idColumn.CSTypeName == "Guid"))
                        {
                            code.AppendLineFormat("sp{0}", column.Name);
                        }
                        else
                        {
                            code.AppendLineFormat("new SqlParameter(\"@{0}\", {1}.{0})", column.Name, paramEntity);
                        }
                    }
                    code.AppendLineWithTabs(");", 1);
                    if (idColumn.CSTypeName == "int" || idColumn.CSTypeName == "Guid")
                    {
                        code.AppendLineFormatWithTabs("{0}.{1} = ({2})sp{1}.Value;", 1, paramEntity, idColumn.Name, idColumn.CSTypeName);
                    }

                    #endregion
                }
                code.AppendLineFormatWithTabs("return {0};", 1, paramEntity);
                code.AppendLine("}");

                return code.ToString();
            }
        }
    }

}
