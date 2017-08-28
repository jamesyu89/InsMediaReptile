using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using InstagramPhotos.CodeGender.Coder;
using InstagramPhotos.CodeGender.DB;

namespace InstagramPhotos.CodeGender
{
    public partial class frmMain
    {
        TextEditorControl editorEntity;
        TextEditorControl editorQueryEntity;
        TextEditorControl editorDataAccess;
        TextEditorControl editorManager;
        TextEditorControl editorStoredProcedures;
        TextEditorControl editorEntityDto;
        TextEditorControl editorMapperExtension;
        TextEditorControl editorRepository;
        TextEditorControl editorBusiness;
        TextEditorControl editorIServices;
        List<string> tables = new List<string>();

        private void LoadData()
        {
            try
            {
                LoadTables();
            }
            catch
            {
                frmConnStr con = new frmConnStr(LoadTables);
                con.ShowDialog();
            }

            InitEditors();
        }

        public void LoadTables()
        {
            tvTables.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.ExpandAll();
            tvTables.Nodes.Add(nodeRoot);
            nodeRoot.Text = DataAccess.GetDatabase();
            foreach (string table in DataAccess.GetTables())
            {
                TreeNode node = new TreeNode();
                node.Text = table;
                node.Tag = "Table";
                node.Nodes.Add(string.Empty);
                node.Collapse();
                nodeRoot.Nodes.Add(node);
            }
        }

        void InitEditors()
        {
            InitEditor(ref editorEntity, "C#", tpEntityCode);
            InitEditor(ref editorEntityDto, "C#", tpEntityDto);
            InitEditor(ref editorMapperExtension, "C#", tpMapperExtension);
            InitEditor(ref editorQueryEntity, "C#", tpQueryCode);
            InitEditor(ref editorDataAccess, "C#", tpDataAccessCode);
            InitEditor(ref editorManager, "C#", tpManagerCode);
            InitEditor(ref editorStoredProcedures, "SQL", tpStoredProceduresCode);
            InitEditor(ref editorRepository, "C#", tpRepository);
            InitEditor(ref editorBusiness, "C#", tpBusiness);
            InitEditor(ref editorIServices, "C#", tpIServices);
        }

        void InitEditor(ref TextEditorControl editor, string language, TabPage page)
        {
            editor = new TextEditorControl();
            editor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(language);
            page.Controls.Add(editor);
            editor.Dock = DockStyle.Fill;
        }

        void tvTables_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            if (e.Node.Tag.Equals("Table"))
            {
                e.Node.Nodes.Clear();
                foreach (Column column in DataAccess.GetColumns(e.Node.Text))
                {
                    TreeNode node = new TreeNode();
                    e.Node.Nodes.Add(node);
                    node.Tag = "Column";
                    node.Text = string.Format("{0}({1})", column.Name, column.DBTypeName);
                }
            }
        }


        private void tvTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;
            string tablename = string.Empty;
            if (e.Node.Tag.Equals("Table"))
            {
                tablename = e.Node.Text;
            }
            else if (e.Node.Tag.Equals("Column"))
            {
                tablename = e.Node.Parent.Text;
            }

            List<Column> columns = DataAccess.GetColumns(tablename);
            lvColumns.Items.Clear();
            lvColumns.Tag = tablename;
            currentTable = tablename;
            txtClassName.Text = Coder.Coder.ConvertTablenameToClassname(tablename).Replace("_","");
            txtClassName.Tag = tablename;
            txtModelNameSpace.Tag = Coder.Coder.GetTableNamePrefix(tablename);
            btnGenerateEntity.Enabled = true;
            Column idColumn = null;
            foreach (Column column in columns)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add("");
                item.Checked = true;
                item.Tag = column;
                item.SubItems.Add(column.Name);
                item.SubItems.Add(column.IsPrimaryKey ? "√" : "");
                item.SubItems.Add(column.DBTypeName);
                item.SubItems.Add(column.Remarks);
                lvColumns.Items.Add(item);
                if (column.IsPrimaryKey)
                    idColumn = column;
            }
            }

        List<Column> GetSelectedColumns(out Column idColumn, out List<Column> AssistantColumn)
        {
            idColumn = null;
            AssistantColumn = new List<Column>();
            List<Column> columns = new List<Column>();
            foreach (ListViewItem item in lvColumns.Items)
            {
                if (item.Checked)
                {
                    Column column = (Column)item.Tag;
                    if (item.SubItems[1].Text == "√") 
                    {
                        column.IsAssistant = true;
                        AssistantColumn.Add(column);
                    }
                    columns.Add(column);
                    if (idColumn == null && column.IsPrimaryKey)
                        idColumn = column;
                }
            }
            if (idColumn == null && columns.Count > 0)
                idColumn = columns[0];
            return columns;
        }

    }
}
