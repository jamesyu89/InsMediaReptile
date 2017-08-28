using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InstagramPhotos.CodeGender.Coder;
using InstagramPhotos.CodeGender.Coder.Classes;
using InstagramPhotos.CodeGender.Coder.Funtions;
using InstagramPhotos.CodeGender.Coder.Funtions.DataAccess;
using InstagramPhotos.CodeGender.Coder.StoredProcedures;
using InstagramPhotos.CodeGender.DB;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender
{
    public partial class frmMain : Form
    {
        string currentTable = null;
        readonly string[] dbTypeName = new string[] { "uniqueidentifier", "int", "datetime", "bigint", "money","bit","tinyint","decimal" };
        public frmMain()
        {
            InitializeComponent();
            //LoadData();
        }

        private void lvColumns_Resize(object sender, EventArgs e)
        {
            chRemarks.Width = lvColumns.Width - chName.Width - chIsPrimaryKey.Width - chTypeName.Width - 5;
        }

        private void btnGenerateEntity_Click(object sender, EventArgs e)
        {
            List<Column> columns = CodeOption.Columns;
            Column idColumn = CodeOption.IdColumn;
            if (!string.IsNullOrEmpty(currentTable) && columns.Count > 0)
            {
                Class c = new EntityClass(CodeOption);
                editorEntity.Text = c.Code;
                Class c_dto = new EntityDtoClass(CodeOption);
                editorEntityDto.Text = c_dto.Code;
                Class q = new QueryEntityClass(CodeOption);
                editorQueryEntity.Text = q.Code;
                GenerateMapperExtension(CodeOption);
                GenerateSqlCode(CodeOption);
                GenerateDataAccessCode(CodeOption);
                //GenerateManagerCode(idColumn);

              //  GenerateRepositoryCode(CodeOption);
                GenerateRepositoryCodeForBatchAndTrans(CodeOption);
                GenerateBusinessCode(CodeOption);
                GenerateIServiceCode(CodeOption);
            }
        }

        private void GenerateDataAccessCode(CodeOption option)
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine();

            code.AppendFormat("#region {0}", option.ClassName);
            code.AppendLine();
            code.AppendLine();

            Function funCreateEntity = new CreateEntityDAFunc(option.ClassName, option.CreateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache, option.GetNewAutoGUID);
            code.Append(funCreateEntity.Code);
            code.AppendLine();

            if (option.CreateBatOperation)
            {
                Function batfunCreateEntity = new BatCreateDAFunc(option.ClassName, option.CreateEntityStoredProcedureName + "_Bat", option.Columns, option.IdColumn, option.EnableParamCache, option.GetNewAutoGUID);
                code.Append(batfunCreateEntity.Code);
                code.AppendLine();
            }

            Function funUpdateEntity = new UpdateEntityDAFunc(option.ClassName, option.UpdateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache);
            code.Append(funUpdateEntity.Code);
            code.AppendLine();

            Function funDeleteEntity = new DeleteEntityDAFunc(option.ClassName, option.DeleteEntityStoredProcedureName, option.IdColumn);
            code.Append(funDeleteEntity.Code);
            code.AppendLine();

            if (option.WithTrans)
            {
                Function funCreateEntityT = new CreateEntityDAFunc(option.ClassName, option.CreateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache, option.GetNewAutoGUID, true);
                code.Append(funCreateEntityT.Code);
                code.AppendLine();

                Function batfunCreateEntityT = new BatCreateDAFunc(option.ClassName, option.CreateEntityStoredProcedureName + "_Bat", option.Columns, option.IdColumn, option.EnableParamCache, option.GetNewAutoGUID, true);
                code.Append(batfunCreateEntityT.Code);
                code.AppendLine();

                Function funUpdateEntityT = new UpdateEntityDAFunc(option.ClassName, option.UpdateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache, true);
                code.Append(funUpdateEntityT.Code);
                code.AppendLine();

                Function funDeleteEntityT = new DeleteEntityDAFunc(option.ClassName, option.DeleteEntityStoredProcedureName, option.IdColumn, true);
                code.Append(funDeleteEntityT.Code);
                code.AppendLine();
            }

            #region lb新增根据副键生成DataAccess
            int i = 0;
            foreach (Column item in option.AssistantColumn)
            {
                i++;
                Function fun3 = new GetEntitiesDAFunc(option.ClassName, option.AssistantColumnProName[i - 1], item, "EntitysBy" + item.Name);
                code.Append(fun3.Code);
                code.AppendLine();
            }

            code.AppendLine();
            code.AppendFormat("#endregion");
            #endregion

            editorDataAccess.Text = code.ToString();
        }

        private void GenerateSqlCode(CodeOption option)
        {
            Table table = new Table(currentTable);
            StringBuilder sql = new StringBuilder();

            StoredProcedure sqlCreateEntitiy = new CreateEntitySP(option.CreateEntityStoredProcedureName, option.Author, option.Columns, option.IdColumn, table, option.GetNewAutoGUID);
            sql.AppendLine(sqlCreateEntitiy.Sql);
            sql.AppendLine();

            StoredProcedure sqlUpdateEntitiy = new UpdateEntitySP(option.UpdateEntityStoredProcedureName, option.Author, option.Columns, option.IdColumn, table);
            sql.AppendLine(sqlUpdateEntitiy.Sql);
            sql.AppendLine();

            if (option.CreateBatOperation)
            {
                StoredProcedure sqlCreateEntitiys = new BatCreateEntity(option.CreateEntityStoredProcedureName + "_Bat", option.Author, option.Columns, option.IdColumn, table, option.GetNewAutoGUID);
                sql.AppendLine(sqlCreateEntitiys.Sql);
                sql.AppendLine();

                StoredProcedure sqlUpdateEntitiys = new BatUpdateEntitySP(option.UpdateEntityStoredProcedureName + "_Bat", option.Author, option.Columns, option.IdColumn, table);
                sql.AppendLine(sqlUpdateEntitiys.Sql);
                sql.AppendLine();
            }

            StoredProcedure sqlDeleteEntitiy = new DeleteEntitySP(option.DeleteEntityStoredProcedureName, option.Author, option.IdColumn, table);
            sql.AppendLine(sqlDeleteEntitiy.Sql);
            sql.AppendLine();

            StoredProcedure sqlGetIds = new GetIdsSP(option.GetEntityIdsStoredProcedureName, option.Author, option.IdColumn, table);
            sql.AppendLine(sqlGetIds.Sql);
            sql.AppendLine();

            StoredProcedure sqlGetEntities = new GetEntitiesSP(option.GetEntitiesStoredProcedureName, option.Author, option.Columns, option.IdColumn, table);
            sql.AppendLine(sqlGetEntities.Sql);
            sql.AppendLine();

            #region lb新增根据副键生成SQL

            StoredProcedure sqlGetAssistantEntitiesSP = new GetAssistantEntitiesSP(option.AssistantColumn, option.GetEntitiesStoredProcedureName, option.Author, option.Columns, option.IdColumn, table, option.AssistantColumnProName);
            sql.AppendLine(sqlGetAssistantEntitiesSP.SqlEntitys);
            sql.AppendLine();

            //StoredProcedure sqlGetAssistantColumns = new GetAssistantSP(option.AssistantColumn, option.GetEntityIdsStoredProcedureName, option.Author, option.IdColumn, table);
            //sql.AppendLine(sqlGetAssistantColumns.Sqls);
            //sql.AppendLine();

            #endregion

            editorStoredProcedures.Text = sql.ToString();
        }

        private void GenerateMapperExtension(CodeOption option)
        {
            var code = new StringBuilder();

            code.AppendLineFormatWithTabs(
                "private static readonly ObjectsMapper<{0}, {1}> mapper_{0}_2_{1} = ObjectMapperManager.DefaultInstance.GetMapper<{0}, {1}>();", 0, option.EntityModelName, option.DomainModelName);
            code.AppendLine();
            code.AppendLineFormatWithTabs(
                    "private static readonly ObjectsMapper<{0}, {1}> mapper_{0}_2_{1} = ObjectMapperManager.DefaultInstance.GetMapper<{0}, {1}>();", 0, option.DomainModelName, option.EntityModelName);
            code.AppendLine();

            code.AppendLineFormatWithTabs("public static {0} ConvertToDto(this {1} model)", 0, option.EntityModelName, option.DomainModelName);
            code.AppendLineWithTabs("{", 0);
            code.AppendLineFormatWithTabs("return mapper_{0}_2_{1}.Map(model);", 1, option.DomainModelName, option.EntityModelName);
            code.AppendLineWithTabs("}", 0);
            code.AppendLine();

            code.AppendLineFormatWithTabs("public static {0} ConvertToModel(this {1} entity)", 0, option.DomainModelName, option.EntityModelName);
            code.AppendLineWithTabs("{", 0);
            code.AppendLineFormatWithTabs("return mapper_{0}_2_{1}.Map(entity);", 1, option.EntityModelName, option.DomainModelName);
            code.AppendLineWithTabs("}", 0);
            code.AppendLine();


            editorMapperExtension.Text = code.ToString();
        }
        
        private void GenerateRepositoryCodeForBatchAndTrans(CodeOption option)
        {
            var code = new StringBuilder();
            code.AppendLineFormatWithTabs("#region auto {0}", 1, option.ClassName);
            code.AppendLine();

            var functionName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(option.ClassName).Replace("_", "");
            var IdName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(option.IdColumn.Name).Replace("_", "");
            IdName = IdName.Substring(0, 1).ToLower() + IdName.Substring(1);
            #region Add
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     新增{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"entity\">实体类</param>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"tran\">事物对象</param>", 1);
            code.AppendLineFormatWithTabs("public void Add{0}({1} entity, DbTransaction tran = null)", 1, functionName, option.DomainModelName);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineWithTabs("var data = new[]", 2);
            code.AppendLineWithTabs("{", 2);
            foreach (Column column in option.Columns)
            {
                if (column.IsPrimaryKey && option.GetNewAutoGUID)
                    continue;
                code.AppendLineFormatWithTabs("{0}.ColumnEnum.{1}.ToString(),", 4, option.DomainModelName, column.Name == option.ClassName ? "_" + column.Name : column.Name);
            }
            //去掉最后一个逗号
            var sb = new StringBuilder();
            sb.AppendLine(code.ToString().Substring(0, code.ToString().LastIndexOf(",", StringComparison.Ordinal)));
            code = sb;
            code.AppendLineWithTabs("};", 3);
            code.AppendLineWithTabs("if (tran == null)", 2);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 3);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineFormatWithTabs("DBTools.InsertObject(conn, entity, GetTableName({0}.TableName), db, data);", 4, option.DomainModelName);
            code.AppendLineWithTabs("}", 3);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("else", 2);
            code.AppendLineFormatWithTabs("DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName({0}.TableName), db,tran, data);", 3, option.DomainModelName);
            code.AppendLineWithTabs("}", 1);
            
            code.AppendLine();
            #endregion

            #region AddList

            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     批量新增{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"entities\">实体集合</param>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"trans\">事物对象</param>", 1);
            code.AppendLineFormatWithTabs("public void Add{0}List(List<{1}> entities, DbTransaction trans = null)", 1, functionName, option.DomainModelName);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineWithTabs("var cmd = new ComplexParams", 2);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineWithTabs("new ComplexParameter", 4);
            code.AppendLineWithTabs("{", 4);
            code.AppendLineWithTabs("Key = \"@model_list\",", 5);
            code.AppendLineWithTabs("DbType = DbType.Xml,", 5);
            code.AppendLineWithTabs("Value = ConvertUtils.ConvertModelListToXML(\"e\", entities)", 5);
            code.AppendLineWithTabs("}", 4);
            code.AppendLineWithTabs("};", 3);
            code.AppendLineWithTabs("var sql= string.Format(@\"INSERT INTO {0} SELECT", 2);
            StringBuilder addBuilder = new StringBuilder();
            foreach (Column column in option.Columns)
            {
                if (column.IsPrimaryKey && option.GetNewAutoGUID)
                    continue;
                if (column.NullAble && dbTypeName.Contains(column.DBTypeName))
                    addBuilder.AppendLineFormatWithTabs("CASE WHEN T.ts.value('@{0}', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@{0}', '{1}') END as {0},", 4, column.Name == option.ClassName ? "_" + column.Name : column.Name, column.NameWithSize);
                else
                addBuilder.AppendLineFormatWithTabs("T.ts.value('@{0}', '{1}') as {0},", 4, column.Name == option.ClassName ? "_" + column.Name : column.Name, column.NameWithSize);
            }
            code.AppendLine(addBuilder.ToString().Substring(0, addBuilder.ToString().LastIndexOf(",", StringComparison.Ordinal)));
            code.AppendLineFormatWithTabs("FROM @model_list.nodes('/es/e') T(ts)\", GetTableName({0}.TableName)); ", 4, option.DomainModelName);
            code.AppendLine();
            code.AppendLineWithTabs("if (trans == null)", 3);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 4);
            code.AppendLineWithTabs("{", 4);
            code.AppendLineFormatWithTabs("DBTools.ExecuteNonQuery(conn, sql, cmd);", 5, option.DomainModelName);
            code.AppendLineWithTabs("}", 4);
            code.AppendLineWithTabs("}", 3);
            code.AppendLineWithTabs("else", 3);
            code.AppendLineFormatWithTabs("DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);", 4, option.DomainModelName);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            #region Update
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    更新{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"entity\">实体类</param>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"tran\">事物对象</param>", 1);
            code.AppendLineFormatWithTabs("public Boolean Update{0}({1} entity,DbTransaction tran=null)", 1, functionName, option.DomainModelName);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineWithTabs("try", 2);
            code.AppendLineWithTabs("{", 2);
            
            
            code.AppendLineWithTabs("if(tran==null)", 4);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 5);
            code.AppendLineWithTabs("{", 5);
            code.AppendLineFormatWithTabs("DBTools.UpdateObject(conn, entity, GetTableName({0}.TableName), new[] {{ {0}.IdName }}, db);", 6, option.DomainModelName);
            code.AppendLineWithTabs("}", 5);
            code.AppendLineWithTabs("else", 4);
            code.AppendLineFormatWithTabs("DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName({0}.TableName), new[] {{ {0}.IdName }}, db,tran);", 5, option.DomainModelName);
            code.AppendLineWithTabs("return true;", 3);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("catch (Exception ex)", 2);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineWithTabs("Logger.Exception(ex);", 3);
            code.AppendLineWithTabs("return false;", 3);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            #region UpdateList

            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     批量更新{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"entities\">实体集合</param>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"trans\">事物对象</param>", 1);
            code.AppendLineFormatWithTabs("public void Update{0}List(List<{1}> entities, DbTransaction trans = null)", 1, functionName, option.DomainModelName);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineWithTabs("var cmd = new ComplexParams", 2);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineWithTabs("new ComplexParameter", 4);
            code.AppendLineWithTabs("{", 4);
            code.AppendLineWithTabs("Key = \"@model_list\",", 5);
            code.AppendLineWithTabs("DbType = DbType.Xml,", 5);
            code.AppendLineWithTabs("Value = ConvertUtils.ConvertModelListToXML(\"e\", entities)", 5);
            code.AppendLineWithTabs("}", 4);
            code.AppendLineWithTabs("};", 3);
            code.AppendLineWithTabs("var sql= string.Format(@\"DECLARE @TBL TABLE(", 2);
            StringBuilder upDfrBuilder = new StringBuilder();
            foreach (Column column in option.Columns)
            {
                //if (column.IsPrimaryKey && option.GetNewAutoGUID)
                //    continue;//
                upDfrBuilder.AppendLineFormatWithTabs("[{0}] {1} {2} {3},", 4, column.Name == option.ClassName ? "_" + column.Name : column.Name, column.NameWithSize,
                    column.NullAble ? "NULL" : "NOT NULL", string.IsNullOrEmpty(column.Default) ? "" : string.Format("DEFAULT {0}", column.Default));
            }
            code.AppendLine(upDfrBuilder.ToString().Substring(0, upDfrBuilder.ToString().LastIndexOf(",", StringComparison.Ordinal)) + ")");
            code.AppendLine();
            code.AppendLine();
            code.AppendLineWithTabs("INSERT INTO @TBL SELECT", 4);
            StringBuilder upAddBuilder = new StringBuilder();
            foreach (Column column in option.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    upAddBuilder.AppendLineFormatWithTabs("T.ts.value('@{0}', '{1}') as {0},", 4,
                        column.Name == option.ClassName ? "_" + column.Name : column.Name, column.NameWithSize);
                }
                else
                {
                    if (column.NullAble && dbTypeName.Contains(column.DBTypeName))
                        upAddBuilder.AppendLineFormatWithTabs("CASE WHEN T.ts.value('@{0}', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@{0}', '{1}') END as {0},", 4, column.Name == option.ClassName ? "_" + column.Name : column.Name,column.NameWithSize);
                    else
                        upAddBuilder.AppendLineFormatWithTabs("T.ts.value('@{0}', '{1}') as {0},", 4,column.Name == option.ClassName ? "_" + column.Name : column.Name, column.NameWithSize);
                }
            }
            code.AppendLine(upAddBuilder.ToString().Substring(0, upAddBuilder.ToString().LastIndexOf(",", StringComparison.Ordinal)));
            code.AppendLineWithTabs("FROM @model_list.nodes('/es/e') T(ts);", 4);
            code.AppendLine();
            code.AppendLine();
            code.AppendLineWithTabs("UPDATE {0} SET  ", 4);
            StringBuilder upSetBuilder = new StringBuilder();
            foreach (Column column in option.Columns)
            {
                if (column.IsPrimaryKey)
                    continue;
                upSetBuilder.AppendLineFormatWithTabs("[{0}] = B.{0},", 4,
                        column.Name == option.ClassName ? "_" + column.Name : column.Name);
            }
            var subString =
                upSetBuilder.ToString().Substring(0, upSetBuilder.ToString().LastIndexOf(",", StringComparison.Ordinal));
            code.AppendLine(subString);
            code.AppendLineFormatWithTabs("FROM {{0}} A,@TBL B WHERE A.{0}=B.{0} \", GetTableName({1}.TableName)); ", 4, option.IdColumn.Name, option.DomainModelName);
            code.AppendLine();
            code.AppendLineWithTabs("if (trans == null)", 3);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 4);
            code.AppendLineWithTabs("{", 4);
            code.AppendLineFormatWithTabs("DBTools.ExecuteNonQuery(conn, sql, cmd);", 5);
            code.AppendLineWithTabs("}", 4);
            code.AppendLineWithTabs("}", 3);
            code.AppendLineWithTabs("else", 3);
            code.AppendLineFormatWithTabs("DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);", 4);
            code.AppendLineWithTabs("}", 1);
            #endregion

            #region Get
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    获取{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"{0}\">主键</param>", 1, option.IdColumn.Name);
            code.AppendLineFormatWithTabs("public {0} Get{1}({2} {3})", 1, option.DomainModelName, functionName, option.IdColumn.CSTypeName, option.IdColumn.Name);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("var cmd = new CmdParams {{ {{ \"@{0}\", {0} }} }};", 2, option.IdColumn.Name);
            code.AppendLineFormatWithTabs(
                "string sql = String.Format(\"SELECT * FROM {{0}}(NOLOCK) WHERE {0}=@{0}\", GetTableName({1}.TableName));",
                2, option.IdColumn.Name, option.DomainModelName);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 2);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineFormatWithTabs("return DBTools.ExecuteReader<{0}>(conn, sql, cmd);", 3, option.DomainModelName);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion


            

            

            #region 通用查询
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    根据{0}s数组获取{1}信息列表", 1, option.IdColumn.Name, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"{0}s\">主键集合</param>", 1, option.IdColumn.Name);
            code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.ClassName);
            code.AppendLineFormatWithTabs("public Dictionary<{0}, {1}> Get{2}s(IEnumerable<{0}> {3}s)", 1, option.IdColumn.CSTypeName, option.DomainModelName, functionName, option.IdColumn.Name);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("{0}[] {2}s = {1}s as {0}[] ?? {1}s.ToArray();", 2, option.IdColumn.CSTypeName, option.IdColumn.Name, IdName);
            code.AppendLineFormatWithTabs("if (!{0}s.Any()) return null;", 2, IdName);
            code.AppendLineFormatWithTabs(
                "string sql = String.Format(\"SELECT * FROM {{0}}(NOLOCK) WHERE {1} in ('{{1}}')\",", 2,
                option.FullTableName, option.IdColumn.Name);
            code.AppendLineFormatWithTabs("GetTableName({0}.TableName), {1}s.Distinct().ToCSV(\"','\"));", 3,
                option.DomainModelName, IdName);
            code.AppendLineFormatWithTabs("using (DbConnection conn = GetConn())", 2);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineFormatWithTabs("IEnumerable<{0}> result = DBTools.ReadCollection<{0}>(conn, sql, null);", 3, option.DomainModelName);
            code.AppendLineFormatWithTabs("return result.ToDictionary(i => i.{0});", 3, option.IdColumn.Name);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            code.AppendLineFormatWithTabs("#endregion", 1);

            editorRepository.Text = code.ToString();
        }

        private void GenerateBusinessCode(CodeOption option)
        {
            var code = new StringBuilder();

            code.AppendLineFormatWithTabs(
                "internal static readonly KVStoreEntityTable<{0}, {1}> cache_{2} = KVStoreManager.GetKVStoreEntityTable<{0}, {1}>(\"{3}\");",
                0, option.IdColumn.CSTypeName, option.DomainModelName, option.ClassName,option.ModelNamespaceName+"_"+option.ClassName);
            code.AppendLine();

            code.AppendLineFormatWithTabs("#region auto {0}", 1, option.ClassName);
            code.AppendLine();

            var functionName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(option.ClassName).Replace("_", "");
            //var preix = FormatePreix(option.FullTableName.Replace("_" + option.ClassName, ""));
            var preix = GetModulePreix(option.FullTableName.Replace("_" + option.ClassName, ""));
            var repositoryPara = "";
            var repositoryPara2 = "";
            var repositoryPara3 = "";
            if (ckbServerIndex.Checked)
            {
                repositoryPara = ", int server_index";
                repositoryPara2 = "server_index";
                repositoryPara3 = ", server_index";
            }
            if (ckbDbIndex.Checked)
            {
                repositoryPara = string.IsNullOrEmpty(repositoryPara) ? ", int db_index" : repositoryPara + ", int db_index";
                repositoryPara2 = string.IsNullOrEmpty(repositoryPara2) ? "db_index" : repositoryPara2 + ", db_index";
                repositoryPara3 += ", db_index";
            }

            #region Add
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     新增{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"dto\">ViewModel</param>", 1);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("public void Add{0}({1} dto{2})", 1, functionName, option.EntityModelName, repositoryPara);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("{0}  info = dto.ConvertToModel();", 2, option.DomainModelName);
            code.AppendLineFormatWithTabs("new {0}Repository({2}).Add{1}(info);", 2, preix, functionName, repositoryPara2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            #region Update
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     更新{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"dto\">ViewModel</param>", 1);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("public void Update{0}({1} dto{2})", 1, functionName, option.EntityModelName, repositoryPara);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("{0}  info = dto.ConvertToModel();", 2, option.DomainModelName);
            code.AppendLineFormatWithTabs("new {0}Repository({2}).Update{1}(info);", 2, preix, functionName, repositoryPara2);
            code.AppendLineFormatWithTabs("{0}Common.cache_{1}.Remove(info.{2});", 2, preix, option.ClassName, option.IdColumn.Name);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            #region Get
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    获取{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"{0}\">主键</param>", 1, option.IdColumn.Name);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("public {0} Get{1}({2} {3}{4})", 1, option.EntityModelName, functionName, option.IdColumn.CSTypeName, option.IdColumn.Name, repositoryPara);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("{0} dto = {1}Common.cache_{5}.GetFromDB({2}, new {1}Repository({4}).Get{3});", 2, option.DomainModelName, preix, option.IdColumn.Name, functionName, repositoryPara2, option.ClassName);
            code.AppendLineFormatWithTabs("return dto.ConvertToDto();", 2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            #region 获取列表
            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    根据{0}s数组获取{1}信息列表", 1, option.IdColumn.Name, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"{0}s\">主键集合</param>", 1, option.IdColumn.Name);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.ClassName);
            code.AppendLineFormatWithTabs("public List<{0}> Get{1}s({2}[] {3}s{4})", 1, option.EntityModelName,
                functionName, option.IdColumn.CSTypeName, option.IdColumn.Name, repositoryPara);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("return", 2);
            code.AppendLineFormatWithTabs("{0}Common.cache_{1}.GetFromDB({2}s, new {0}Repository({4}).Get{3}s)", 3,
                preix, option.ClassName, option.IdColumn.Name, functionName, repositoryPara2);
            code.AppendLineFormatWithTabs(".Select(m => m.ConvertToDto())", 4);
            code.AppendLineFormatWithTabs(".ToList();", 4);
            code.AppendLineWithTabs("}", 1);
            code.AppendLine();
            #endregion

            if (option.IdColumn.CSTypeName != "string")
            {
                #region 通用查询
                code.AppendLineFormatWithTabs("/// <summary>", 1);
                code.AppendLineFormatWithTabs("///  通用查询", 1);
                code.AppendLineFormatWithTabs("/// </summary>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"queryEntity\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"isCache\"></param>", 1);
                if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
                if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
                code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.EntityModelName);
                code.AppendLineFormatWithTabs(
                    "public List<{0}> Get{1}DtosByPara({3} queryEntity, Boolean isCache{2})", 1,
                    option.EntityModelName, functionName, repositoryPara, option.QueryModelName);
                code.AppendLineWithTabs("{", 1);
                code.AppendLineFormatWithTabs("var qm = new QueryHelper(new {0}Repository({1}));", 2, preix,
                    repositoryPara2);
                code.AppendLineFormatWithTabs("List<{0}> ids = qm.Get{0}IDsByConditions(queryEntity, isCache);", 2,
                    Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(option.IdColumn.CSTypeName));
                code.AppendLineFormatWithTabs("if (ids == null || ids.Count == 0)", 2);
                code.AppendLineFormatWithTabs("return new List<{0}>();", 3, option.EntityModelName);
                code.AppendLineFormatWithTabs("return Get{0}s(ids.ToArray(){1});", 2, functionName, repositoryPara3);
                code.AppendLineWithTabs("}", 1);
                code.AppendLine();
                #endregion

                #region 分页通用查询
                code.AppendLineFormatWithTabs("/// <summary>", 1);
                code.AppendLineFormatWithTabs("///  分页通用查询", 1);
                code.AppendLineFormatWithTabs("/// </summary>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"queryEntity\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageIndex\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageSize\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"totalCount\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageCount\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"isCache\"></param>", 1);
                if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
                if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
                code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.EntityModelName);
                code.AppendLineFormatWithTabs("public List<{0}> Get{1}DtosByParaForPage({2} queryEntity,", 1,
                    option.EntityModelName, functionName, option.QueryModelName);
                code.AppendLineFormatWithTabs(
                    "Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache{0})", 2,
                    repositoryPara);
                code.AppendLineWithTabs("{", 1);
                code.AppendLineFormatWithTabs("var qm = new QueryHelper(new {0}Repository({1}));", 2, preix,
                    repositoryPara2);
                code.AppendLineFormatWithTabs(
                    "List<{0}> ids = qm.Get{0}IDsByConditions(queryEntity, pageIndex, pageSize, out totalCount, out pageCount, isCache);",
                    2, option.IdColumn.CSTypeName);
                code.AppendLineFormatWithTabs("if (ids == null || ids.Count == 0)", 2);
                code.AppendLineFormatWithTabs("return new List<{0}>();", 3, option.EntityModelName);
                code.AppendLineFormatWithTabs("return Get{0}s(ids.ToArray(){1});", 2, functionName, repositoryPara3);
                code.AppendLineWithTabs("}", 1);
                code.AppendLine();
                #endregion
            }
            code.AppendLineFormatWithTabs("#endregion", 1);

            editorBusiness.Text = code.ToString();
        }

        private void GenerateIServiceCode(CodeOption option)
        {
            var code = new StringBuilder();

            var functionName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(option.ClassName).Replace("_", "");
            var repositoryPara = "";
            if (ckbServerIndex.Checked)
            {
                repositoryPara = ", int server_index";
            }
            if (ckbDbIndex.Checked)
            {
                repositoryPara = string.IsNullOrEmpty(repositoryPara) ? ", int db_index" : repositoryPara + ", int db_index";
            }

            code.AppendLineFormatWithTabs("#region auto {0}", 1, option.ClassName);
            code.AppendLine();

            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     新增{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"dto\">ViewModel</param>", 1);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("void Add{0}({1} dto{2});", 1, functionName, option.EntityModelName, repositoryPara);
            code.AppendLine();

            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///     更新{0}信息", 1, option.ClassName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"dto\">ViewModel</param>", 1);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("void Update{0}({1} dto{2});", 1, functionName, option.EntityModelName, repositoryPara);
            code.AppendLine();

            code.AppendLineFormatWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("///    获取{0}信息", 1, option.EntityModelName);
            code.AppendLineFormatWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("/// <param name=\"{0}\">主键</param>", 1, option.IdColumn.Name);
            if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
            if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
            code.AppendLineFormatWithTabs("{0} Get{1}({2} {3}{4});", 1, option.EntityModelName, functionName, option.IdColumn.CSTypeName, option.IdColumn.Name, repositoryPara);
            code.AppendLine();

            if (option.IdColumn.CSTypeName != "string")
            {
                code.AppendLineFormatWithTabs("/// <summary>", 1);
                code.AppendLineFormatWithTabs("///    根据{0}s数组获取{1}信息列表", 1, option.IdColumn.Name, option.ClassName);
                code.AppendLineFormatWithTabs("/// </summary>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"{0}s\">主键集合</param>", 1, option.IdColumn.Name);
                if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
                if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
                code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.EntityModelName);
                code.AppendLineFormatWithTabs("List<{0}> Get{1}s({2}[] {3}s{4});", 1, option.EntityModelName,
                    functionName, option.IdColumn.CSTypeName, option.IdColumn.Name, repositoryPara);
                code.AppendLine();

                code.AppendLineFormatWithTabs("/// <summary>", 1);
                code.AppendLineFormatWithTabs("///  通用查询", 1);
                code.AppendLineFormatWithTabs("/// </summary>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"queryEntity\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"isCache\"></param>", 1);
                if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
                if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
                code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.EntityModelName);
                code.AppendLineFormatWithTabs(
                    "List<{0}> Get{1}DtosByPara({3} queryEntity, Boolean isCache{2});", 1,
                    option.EntityModelName, functionName, repositoryPara, option.QueryModelName);
                code.AppendLine();

                code.AppendLineFormatWithTabs("/// <summary>", 1);
                code.AppendLineFormatWithTabs("///  分页通用查询", 1);
                code.AppendLineFormatWithTabs("/// </summary>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"queryEntity\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageIndex\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageSize\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"totalCount\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"pageCount\"></param>", 1);
                code.AppendLineFormatWithTabs("/// <param name=\"isCache\"></param>", 1);
                if (ckbServerIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"server_index\">server_index</param>", 1);
                if (ckbDbIndex.Checked) code.AppendLineFormatWithTabs("/// <param name=\"db_index\">db_index</param>", 1);
                code.AppendLineFormatWithTabs("/// <returns>{0}信息列表</returns>", 1, option.EntityModelName);
                code.AppendLineFormatWithTabs("List<{0}> Get{1}DtosByParaForPage({2} queryEntity,", 1,
                    option.EntityModelName, functionName, option.QueryModelName);
                code.AppendLineFormatWithTabs(
                    "Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache{0});", 2,
                    repositoryPara);
                code.AppendLine();
            }
            code.AppendLineFormatWithTabs("#endregion", 1);

            editorIServices.Text = code.ToString();
        }

        private CodeOption CodeOption
        {
            get
            {
                Column idColumn;
                List<Column> AssistantColumnList;
                List<Column> columns = GetSelectedColumns(out idColumn, out AssistantColumnList);
                CodeOption option = new CodeOption();
                option.Author = txtAuthor.Text;
                option.ClassName = txtClassName.Text;
                List<string> AssistantColumnProName = new List<string>();
                List<string> AssistantFunName = new List<string>();
                string preix = (ckbPreNameSpace.Checked && txtModelNameSpace.Tag != null && !string.IsNullOrEmpty(txtModelNameSpace.Tag.ToString().Trim())) ? txtModelNameSpace.Tag.ToString() : "";
                foreach (Column item in AssistantColumnList)
                {
                    AssistantColumnProName.Add("up_" + preix + "_" + txtClassName.Text + "_GetEntitysBy" + item.Name + "s");
                    AssistantFunName.Add("EntitysBy" + item.Name);
                }
                option.AssistantColumnProName = AssistantColumnProName;//所需生成的副键列存储过程名集合
                option.AssistantFunName = AssistantFunName;//所需生成的副键列方法名集合
                option.ModelNamespaceName = string.IsNullOrEmpty(txtModelNameSpace.Text.Trim()) ? FormatePreix(preix) : txtModelNameSpace.Text.Trim() + "." + FormatePreix(preix);
                option.DtoNamespaceName = string.IsNullOrEmpty(txtDtoNameSpace.Text.Trim()) ? FormatePreix(preix) : txtDtoNameSpace.Text.Trim() + "." + FormatePreix(preix);
                option.QueryNamespaceName = string.IsNullOrEmpty(txtQueryNameSpace.Text.Trim()) ? FormatePreix(preix) : txtQueryNameSpace.Text.Trim() + "." + FormatePreix(preix);
                option.BllNamespaceName = string.IsNullOrEmpty(txtBllNameSpace.Text.Trim()) ? FormatePreix(preix) : txtBllNameSpace.Text.Trim() + "." + FormatePreix(preix);
                option.DalNamespaceName = string.IsNullOrEmpty(txtDalNameSpace.Text.Trim()) ? FormatePreix(preix) : txtDalNameSpace.Text.Trim() + "." + FormatePreix(preix);

                option.DalClassName = FormatePreix(txtModelNameSpace.Tag.ToString()) + "DataProvider";
                option.FullTableName = txtClassName.Tag.ToString();
                option.GetNewAutoGUID = ckNeedAutoGuid.Checked;
                option.EnableParamCache = ckParamCache.Checked;
                option.ProtoBuf = ckProtoBuf.Checked;
                option.IdColumn = idColumn;
                option.Columns = columns;
                option.AssistantColumn = AssistantColumnList;
                option.IsBllSinglon = ckSingletonBLL.Checked;
                option.IsPartial = ckPartial.Checked;

                option.InitDalNameSpace = txtDalNameSpace.Text.Trim();

                return option;
            }
        }

        public CodeOption GetCodeOption(string tablename)
        {

            List<Column> columns = DataAccess.GetColumns(tablename);
            CodeOption option = new CodeOption();
            //  option.Author = txtAuthor.Text;
            option.ClassName = Coder.Coder.ConvertTablenameToClassname(tablename);

            string preix = ckbPreNameSpace.Checked ? Coder.Coder.GetTableNamePrefix(tablename) : "";

            option.ModelNamespaceName = string.IsNullOrEmpty(txtModelNameSpace.Text.Trim()) ? FormatePreix(preix) : txtModelNameSpace.Text.Trim() + "." + FormatePreix(preix);
            option.DtoNamespaceName = string.IsNullOrEmpty(txtDtoNameSpace.Text.Trim()) ? FormatePreix(preix) : txtDtoNameSpace.Text.Trim() + "." + FormatePreix(preix);
            option.QueryNamespaceName = string.IsNullOrEmpty(txtQueryNameSpace.Text.Trim()) ? FormatePreix(preix) : txtQueryNameSpace.Text.Trim() + "." + FormatePreix(preix);
            option.BllNamespaceName = string.IsNullOrEmpty(txtBllNameSpace.Text.Trim()) ? FormatePreix(preix) : txtBllNameSpace.Text.Trim() + "." + FormatePreix(preix);
            option.DalNamespaceName = txtDalNameSpace.Text.Trim();// string.IsNullOrEmpty(txtDalNameSpace.Text.Trim()) ? FormatePreix(preix) : txtDalNameSpace.Text.Trim() + "." + FormatePreix(preix);

            option.DalClassName = FormatePreix(Coder.Coder.GetTableNamePrefix(tablename)) + "DataProvider";
            option.FullTableName = tablename;
            option.GetEntitiesStoredProcedureName = Coder.Coder.ConverTablenameToGetEntitiesStoredProcedureName(tablename);

            option.CreateEntityStoredProcedureName = Coder.Coder.ConverTablenameToActEntityStoredProcedureName(tablename, "Create");
            option.UpdateEntityStoredProcedureName = Coder.Coder.ConverTablenameToActEntityStoredProcedureName(tablename, "Update");
            option.DeleteEntityStoredProcedureName = Coder.Coder.ConverTablenameToActEntityStoredProcedureName(tablename, "Delete");


            option.GetNewAutoGUID = ckNeedAutoGuid.Checked;
            option.EnableParamCache = ckParamCache.Checked;
            option.ProtoBuf = ckProtoBuf.Checked;
            option.IdColumn = columns[0];
            option.Columns = columns;


            option.GetEntityIdsStoredProcedureName = Coder.Coder.ConverTablenameToGetIdsStoredProcedureName(tablename, option.IdColumn);

            option.IsBllSinglon = ckSingletonBLL.Checked;
            option.IsPartial = ckPartial.Checked;

            option.InitDalNameSpace = txtDalNameSpace.Text.Trim();
            return option;

        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            frmConn con = new frmConn(this.LoadData);
            con.ShowDialog();
        }

        private void tsbExportDBDoc_Click(object sender, EventArgs e)
        {
            if (DataAccess.conn == string.Empty)
            {
                MessageBox.Show("请先连接数据库");
                tsbConnect_Click(sender, e);
                return;
            }

            frmDBDocument doc = new frmDBDocument();
            doc.ShowDialog();
        }

        private void tsbExportMarkCode_Click(object sender, EventArgs e)
        {
            Class c = new EnumClass();
            editorEntity.Text = c.Code;
        }

        private void tspCodeFiles_Click(object sender, EventArgs e)
        {
            frmGeCodeFiles frm = new frmGeCodeFiles();
            frm.mainForm = this;
            frm.ShowDialog();
        }

        private string FormatePreix(string preix)
        {
            preix = preix.Replace("Fct_", "").Replace("Rel_", "").Replace("Dim_", "");
            if (string.IsNullOrEmpty(preix.Trim()))
            {
                return "";
            }
            return preix[0].ToString().ToUpper() + preix.Substring(1);
        }

        private string GetModulePreix(string preix)
        {
            preix = preix.Replace("Fct_", "").Replace("Rel_", "").Replace("Dim_", "");
            if (string.IsNullOrEmpty(preix.Trim()))
            {
                return "";
            }
            return preix[0].ToString().ToUpper() + preix.Substring(1).Split('_')[0];
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void lvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvColumns.SelectedIndices != null && lvColumns.SelectedIndices.Count > 0)//&& lvColumns.SelectedItems[0].Bounds.Location.Y==16)
            {
                Point MenuPoint = lvColumns.PointToClient(Control.MousePosition);
                Int32 Width1 = this.chTtile.Width;
                Int32 Width2 = this.condition.Width + Width1;
                if (MenuPoint.X > Width1 && MenuPoint.X < Width2)
                {
                    if (string.Empty.Equals(lvColumns.SelectedItems[0].SubItems[1].Text) && lvColumns.SelectedItems[0].Checked)
                    {
                        lvColumns.SelectedItems[0].SubItems[1].Text = "√";
                    }
                    else
                    {
                        lvColumns.SelectedItems[0].SubItems[1].Text = "";
                    }
                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void ckProtoBuf_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sln_name = ((TextBox)sender).Text;
            var subClass = "";
            if (txtSubClassName.Text != "")
                subClass = "." + txtSubClassName.Text;
            this.txtModelNameSpace.Text = sln_name + ".DomainModel" + subClass;
            this.txtDtoNameSpace.Text = sln_name + ".ViewModel" + subClass;
            this.txtDalNameSpace.Text = sln_name + ".Repository" + subClass;
            this.txtBllNameSpace.Text = sln_name + ".Service" + subClass;
            this.txtQueryNameSpace.Text = sln_name + ".QueryModel" + subClass;
        }

        private void txtSubClassName_TextChanged(object sender, EventArgs e)
        {
            string sln_name = textBox1.Text;
            var subClass = ((TextBox)sender).Text;
            if (subClass != "")
                subClass = "." + subClass;
            this.txtModelNameSpace.Text = sln_name + ".DomainModel" + subClass;
            this.txtDtoNameSpace.Text = sln_name + ".ViewModel" + subClass;
            this.txtDalNameSpace.Text = sln_name + ".Repository" + subClass;
            this.txtBllNameSpace.Text = sln_name + ".Service" + subClass;
            this.txtQueryNameSpace.Text = sln_name + ".QueryModel" + subClass;
        }
    }
}
