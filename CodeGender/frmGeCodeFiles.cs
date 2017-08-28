using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstagramPhotos.CodeGender.Coder;
using InstagramPhotos.CodeGender.Coder.Funtions;
using InstagramPhotos.CodeGender.Coder.Funtions.DataAccess;
using InstagramPhotos.CodeGender.Coder.StoredProcedures;
using InstagramPhotos.CodeGender.DB;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender
{
    public partial class frmGeCodeFiles : Form
    {
        private CodeOption codeOption;
        public frmMain mainForm;

        private Dictionary<string, string> tables;

        public frmGeCodeFiles()
        {
            InitializeComponent();
        }

        private void frmDBDocument_Load(object sender, EventArgs e)
        {
            var dbName = DataAccess.GetDatabase();
            tables = DataAccess.GetTablesWithCommaRemark();
            cklTables.Items.AddRange(tables.Keys.ToArray());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < cklTables.Items.Count; i++)
            {
                cklTables.SetItemChecked(i, chkAll.Checked);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
        }

        protected void SaveFile(string path, string fileName, string content)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!File.Exists(path + "\\" + fileName))
                {
                    File.AppendAllText(path + "\\" + fileName, content);
                }
            }

            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string GetCHNFieldName(string remarks)
        {
            var idx = remarks.IndexOfAny(new[] {'(', '（'}, 0);
            return idx == -1 ? remarks : remarks.Substring(0, idx);
        }

        private string GetRule(string remarks)
        {
            var idx = remarks.IndexOfAny(new[] {'(', '（'}, 0);

            return idx == -1
                ? string.Empty
                : remarks.Substring(idx + 1).Replace(")", string.Empty).Replace("）", string.Empty);
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            var save = new FolderBrowserDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = save.SelectedPath + "\\";
            }
        }

        private string FormatePreix(string preix)
        {
            return preix[0].ToString().ToUpper() + preix.Substring(1);
        }

        private void SaveDataAccessCode(string path, string content, string preix, string modelNameSpace,
            string dalNameSpace)
        {
            var className = FormatePreix(preix) + "DataProvider";
            var tabCount = 1;
            var fileContent = new StringBuilder();
            var code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Data;");
            code.AppendLine("using System.Data.SqlClient;");
            code.AppendLine("using CLFramework.Data;");
            code.AppendLine("using System.Configuration;");
            code.AppendLineFormat("using {0};", modelNameSpace);

            code.AppendLine();

            code.AppendLineFormatWithTabs("namespace {0}", 0, dalNameSpace);
            code.AppendLine("{");

            code.AppendLineWithTabs("/// <summary>", tabCount);
            code.AppendLineFormatWithTabs("/// {0}", tabCount, FormatePreix(preix) + "模块数据访问类");
            code.AppendLineWithTabs("/// </summary>", tabCount);

            code.AppendLineFormatWithTabs("public {1} class {0}: {2}", tabCount, className, "sealed", "DataProviderBase");
            code.AppendLineWithTabs("{", tabCount);

            code.AppendLineWithTabs("#region Singleton", tabCount + 1);
            code.AppendLineWithTabs("", tabCount + 1);
            code.AppendLineWithTabs(string.Format("public static {0} Instance", className), tabCount + 1);
            code.AppendLineWithTabs("{", tabCount + 1);
            code.AppendLineWithTabs("get", tabCount + 2);
            code.AppendLineWithTabs("{", tabCount + 2);
            code.AppendLineWithTabs("if (_dataProvider == null)", tabCount + 3);
            code.AppendLineWithTabs(string.Format("_dataProvider = new {0}();", className), tabCount + 4);
            code.AppendLineWithTabs(" return _dataProvider;", tabCount + 3);
            code.AppendLineWithTabs("}", tabCount + 2);
            code.AppendLineWithTabs("}", tabCount + 1);
            code.AppendLineWithTabs("", tabCount + 1);
            code.AppendLineWithTabs(string.Format(@"private {0}()", className), tabCount + 1);
            code.AppendLineWithTabs("{", tabCount + 1);
            code.AppendLineWithTabs("try", tabCount + 2);
            code.AppendLineWithTabs("{", tabCount + 2);
            code.AppendLineWithTabs(
                string.Format(@"connectionString = ConfigurationManager.ConnectionStrings[""{0}""].ConnectionString;",
                    "xTrans2ConnectString"), tabCount + 3);
            code.AppendLineWithTabs("}", tabCount + 2);
            code.AppendLineWithTabs("catch (Exception e)", tabCount + 2);
            code.AppendLineWithTabs("{", tabCount + 2);
            code.AppendLineWithTabs("throw e;", tabCount + 3);
            code.AppendLineWithTabs("}", tabCount + 2);
            code.AppendLineWithTabs("}", tabCount + 1);
            code.AppendLine();
            code.AppendLineWithTabs(string.Format("private static {0} _dataProvider;", className), tabCount + 1);

            code.AppendLineWithTabs("#endregion ", tabCount + 1);
            code.AppendLine();

            code.AppendLineWithTabs("#region Conn", tabCount + 1);
            code.AppendLineWithTabs("", tabCount + 1);
            code.AppendLineWithTabs("protected override SqlConnection GetSqlConnection()", tabCount + 1);
            code.AppendLineWithTabs("", tabCount + 1);
            code.AppendLineWithTabs("{", tabCount + 1);
            code.AppendLineWithTabs("return new SqlConnection(connectionString);", tabCount + 2);
            code.AppendLineWithTabs("}", tabCount + 1);
            code.AppendLineWithTabs("private static string connectionString;", tabCount + 1);
            code.AppendLineWithTabs("#endregion", tabCount + 1);

            code.AppendLineWithTabs("", tabCount + 1);

            code.AppendLine(content.IncreaseIndent(tabCount + 1));

            code.AppendLineWithTabs("}", tabCount);
            code.AppendLine("}");

            SaveFile(path, FormatePreix(preix) + "DataProvider.cs", code.ToString());
        }

        #region generate code

        public string GenerateDataAccessCode(CodeOption option)
        {
            var code = new StringBuilder();

            code.AppendLine();

            code.AppendFormat("#region {0}", option.ClassName);
            code.AppendLine();
            code.AppendLine();

            Function funCreateEntity = new CreateEntityDAFunc(option.ClassName, option.CreateEntityStoredProcedureName,
                option.Columns, option.IdColumn, option.EnableParamCache, option.GetNewAutoGUID);
            code.Append(funCreateEntity.Code);
            code.AppendLine();

            if (option.CreateBatOperation)
            {
                Function batfunCreateEntity = new BatCreateDAFunc(option.ClassName,
                    option.CreateEntityStoredProcedureName + "_Bat", option.Columns, option.IdColumn,
                    option.EnableParamCache, option.GetNewAutoGUID);
                code.Append(batfunCreateEntity.Code);
                code.AppendLine();
            }

            Function funUpdateEntity = new UpdateEntityDAFunc(option.ClassName, option.UpdateEntityStoredProcedureName,
                option.Columns, option.IdColumn, option.EnableParamCache);
            code.Append(funUpdateEntity.Code);
            code.AppendLine();

            if (option.CreateBatOperation)
            {
                //Function batfunUpdateEntity = new BatCreateDAFunc(option.ClassName, option.UpdateEntityStoredProcedureName + "_Bat", option.Columns, option.IdColumn, option.EnableParamCache);
                //code.Append(batfunUpdateEntity.Code);
                //code.AppendLine();
            }

            Function funDeleteEntity = new DeleteEntityDAFunc(option.ClassName, option.DeleteEntityStoredProcedureName,
                option.IdColumn);
            code.Append(funDeleteEntity.Code);
            code.AppendLine();

            if (option.WithTrans)
            {
                Function funCreateEntityT = new CreateEntityDAFunc(option.ClassName,
                    option.CreateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache,
                    option.GetNewAutoGUID, true);
                code.Append(funCreateEntityT.Code);
                code.AppendLine();

                Function batfunCreateEntityT = new BatCreateDAFunc(option.ClassName,
                    option.CreateEntityStoredProcedureName + "_Bat", option.Columns, option.IdColumn,
                    option.EnableParamCache, option.GetNewAutoGUID, true);
                code.Append(batfunCreateEntityT.Code);
                code.AppendLine();

                Function funUpdateEntityT = new UpdateEntityDAFunc(option.ClassName,
                    option.UpdateEntityStoredProcedureName, option.Columns, option.IdColumn, option.EnableParamCache,
                    true);
                code.Append(funUpdateEntityT.Code);
                code.AppendLine();

                Function funDeleteEntityT = new DeleteEntityDAFunc(option.ClassName,
                    option.DeleteEntityStoredProcedureName, option.IdColumn, true);
                code.Append(funDeleteEntityT.Code);
                code.AppendLine();
            }

            Function fun1 = new GetEntityIdsDAFunc(option.ClassName, option.GetEntityIdsStoredProcedureName,
                option.IdColumn);
            code.Append(fun1.Code);
            code.AppendLine();

            Function fun2 = new GetEntitiesDAFunc(option.ClassName, option.GetEntitiesStoredProcedureName,
                option.IdColumn, option.ClassName);
            code.Append(fun2.Code);
            code.AppendLine();

            code.AppendLine();
            code.AppendFormat("#endregion");

            return code.ToString();
        }


        public string GenerateSqlCode(CodeOption option, string tablename)
        {
            var table = new Table(tablename);
            var sql = new StringBuilder();

            StoredProcedure sqlCreateEntitiy = new CreateEntitySP(option.CreateEntityStoredProcedureName, option.Author,
                option.Columns, option.IdColumn, table, option.GetNewAutoGUID);
            sql.AppendLine(sqlCreateEntitiy.Sql);
            sql.AppendLine();

            StoredProcedure sqlUpdateEntitiy = new UpdateEntitySP(option.UpdateEntityStoredProcedureName, option.Author,
                option.Columns, option.IdColumn, table);
            sql.AppendLine(sqlUpdateEntitiy.Sql);
            sql.AppendLine();

            if (option.CreateBatOperation)
            {
                StoredProcedure sqlCreateEntitiys = new BatCreateEntity(
                    option.CreateEntityStoredProcedureName + "_Bat", option.Author, option.Columns, option.IdColumn,
                    table, option.GetNewAutoGUID);
                sql.AppendLine(sqlCreateEntitiys.Sql);
                sql.AppendLine();

                StoredProcedure sqlUpdateEntitiys =
                    new BatUpdateEntitySP(option.UpdateEntityStoredProcedureName + "_Bat", option.Author, option.Columns,
                        option.IdColumn, table);
                sql.AppendLine(sqlUpdateEntitiys.Sql);
                sql.AppendLine();
            }

            StoredProcedure sqlDeleteEntitiy = new DeleteEntitySP(option.DeleteEntityStoredProcedureName, option.Author,
                option.IdColumn, table);
            sql.AppendLine(sqlDeleteEntitiy.Sql);
            sql.AppendLine();

            StoredProcedure sqlGetIds = new GetIdsSP(option.GetEntityIdsStoredProcedureName, option.Author,
                option.IdColumn, table);
            sql.AppendLine(sqlGetIds.Sql);
            sql.AppendLine();

            StoredProcedure sqlGetEntities = new GetEntitiesSP(option.GetEntitiesStoredProcedureName, option.Author,
                option.Columns, option.IdColumn, table);
            sql.AppendLine(sqlGetEntities.Sql);
            sql.AppendLine();

            return sql.ToString();
        }


        public string GenerateInterfaceCode()
        {
            return null;
        }

        #endregion
    }
}