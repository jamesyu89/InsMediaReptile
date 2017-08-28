using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstagramPhotos.CodeGender.Coder;
using InstagramPhotos.CodeGender.DB;

namespace InstagramPhotos.CodeGender
{
    public partial class frmDBDocument : Form
    {
        string dbtemplate = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>数据库结构</title>
    <style type='text/css'>
    body {
	    font: 13px/1.5 Microsoft yahei, simsun;
    }
    .styledb {
	    font-size: 19px;
    }
    .styletab {
	    font-size: 16px;
	    padding-top: 18px;
	    text-align:left;
    }
    .main-tit{
	    font-size:18px;
	    margin-bottom:5px;
	    margin-top:10px;
    }
    .sub-tit{
	    font-size:11px;
	    color:#999;
	    margin:0;
    }
    table {
	    border-collapse:collapse;
	    border-spacing:0;
    }
    .tab-data{
	    margin-top:20px;
    }
    .tab-data table tr.tit{
	    background-color:#E3EFFF;
    }
    .tab-data table tr.tit td{
	    color:#000;
	    text-align:center;
    }
    .tab-data table td {
	    border:1px solid #dddddd;
	    padding:5px;
	    color:#666;
	    text-align:left;
    }
    .tab-data tr:hover, .tab-data tr.hilite {
	    background-color: #FFFCD8;
	    color: #000000;
    }
</style>

</head>
<body>
    <div style='text-align: center'>
        <div class='styledb'>数据库名：$DataBaseName$</div>
        $TablesBody$
    </div>
<script type='text/javascript'>
var rows = document.getElementsByTagName('tr');
for (var i = 0; i < rows.length; i++) {
	if(rows[i].className == 'tit')
		continue;
	rows[i].onmouseover = function() {
		this.className += 'hilite';
	}
	rows[i].onmouseout = function() {
		this.className = this.className.replace('hilite', '');
	}
}
</script>
</body>
</html>
";
        string tabletemplate = @"
        <div class='styletab'>
            <p class='main-tit'>表名：$TableName$【<b>$TableRemark$</b>】</p>
            <p class='sub-tit'>创建时间:$CreateTime$&nbsp;&nbsp;修改时间:$ModifyTime$</p>
        </div>
        
        <div class='tab-data'>
            <table border='0' cellpadding='5' cellspacing='0' width='100%'>
                <tr class='tit'>
                                <td>序号</td>
                                <td>列名</td>
                                <td>数据类型</td>
                                <td>长度</td>
                                <td>精度</td>
                                <td>自增</td>
                                <td>键</td>
                                <td>可空</td>
                                <td>默认值</td>
                                <td>中文字段名</td>
                                <td>业务规则</td>
                            </tr>
                            $ColumnsBody$
            </table>
        </div>";

        Dictionary<string, string> tables;

        public frmDBDocument()
        {
            InitializeComponent();
        }

        private void frmDBDocument_Load(object sender, EventArgs e)
        {
            string dbName = DataAccess.GetDatabase();
            tables = DataAccess.GetTablesWithCommaRemark();
            this.cklTables.Items.AddRange(tables.Keys.ToArray());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.cklTables.Items.Count; i++)
            {
                this.cklTables.SetItemChecked(i, chkAll.Checked);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (this.ckGroupByPrefix.Checked)
            {
                int prefix = this.cklTables.CheckedItems[0].ToString().IndexOf('_');

                List<string> tableNames = new List<string>();
                foreach (var item in this.cklTables.CheckedItems)
                    tableNames.Add(item.ToString());

                foreach (var group in tableNames.GroupBy(s => s.Substring(0, prefix)))
                {
                    string doc = dbtemplate.Replace("$DataBaseName$", DataAccess.GetDatabase());

                    StringBuilder sbTables = new StringBuilder();
                    foreach (var tableName in group)
                    {
                        var colums = DataAccess.GetColumns(tableName.ToString());
                        string tb = tabletemplate;
                        tb = tb.Replace("$TableName$", tableName.ToString())
                            .Replace("$CreateTime$", colums[0].CreateTime.ToString("yyyy-MM-dd hh:mm"))
                            .Replace("$ModifyTime$", colums[0].ModifyTime.ToString("yyyy-MM-dd hh:mm"));
                        string remark;
                        tables.TryGetValue(tableName.ToString(), out remark);
                        tb = tb.Replace("$TableRemark$", remark);
                        tb = tb.Replace("$ColumnsBody$", GetColumnsBody(colums));
                        sbTables.Append(tb);
                    }

                    doc = doc.Replace("$TablesBody$", sbTables.ToString()).Replace("  ", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);

                    SaveFile(this.tbPath.Text + "数据库文档-模块【" + group.Key + "】.htm", doc);
                }

                MessageBox.Show("生成成功！");
            }
            else
            {

                string doc = dbtemplate.Replace("$DataBaseName$", DataAccess.GetDatabase());

                StringBuilder sbTables = new StringBuilder();
                foreach (var tableName in this.cklTables.CheckedItems)
                {
                    var colums = DataAccess.GetColumns(tableName.ToString());
                    string tb = tabletemplate;
                    tb = tb.Replace("$TableName$", tableName.ToString())
                        .Replace("$CreateTime$", colums[0].CreateTime.ToString("yyyy-MM-dd hh:mm"))
                        .Replace("$ModifyTime$", colums[0].ModifyTime.ToString("yyyy-MM-dd hh:mm"));
                    string remark;
                    tables.TryGetValue(tableName.ToString(), out remark);
                    tb = tb.Replace("$TableRemark$", remark);
                    tb = tb.Replace("$ColumnsBody$", GetColumnsBody(colums));
                    sbTables.Append(tb);
                }

                doc = doc.Replace("$TablesBody$", sbTables.ToString()).Replace("  ", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);

                SaveFile(this.tbPath.Text + "数据库文档.htm", doc);
                MessageBox.Show("生成成功！");
            }
        }

        private string GetColumnsBody(List<Column> colums)
        {
            StringBuilder sbColumns = new StringBuilder();

            foreach (var item in colums)
            {
                //<td>序号</td>
                //<td>列名</td>
                //<td>数据类型</td>
                //<td>长度</td>
                //<td>小数位</td>
                //<td>标识</td>
                //<td>主键</td>
                //<td>允许空</td>
                //<td>默认值</td>
                //<td>中文名</td>
                //<td>业务规则</td>
                sbColumns.AppendFormat(@"<tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                                <td>{4}</td>
                                <td>{5}</td>
                                <td>{6}</td>
                                <td>{7}</td>
                                <td>{8}</td>
                                <td align='left'>{9}</td>
                                <td align='left'>{10}</td>
                            </tr>"
                    , item.ColumnId
                    , item.Name
                    , item.DBTypeName
                    , item.Length
                    , item.Scale
                    , item.IsIdentity ? "√" : string.Empty
                    , item.IsPrimaryKey ? "√" : string.Empty
                    , item.NullAble ? "是" : "否"
                    , item.Default
                    , GetCHNFieldName(item.Remarks)
                    , GetRule(item.Remarks));
            }

            return sbColumns.ToString();
        }

        protected void SaveFile(string fileName, string content)
        {
            try
            {
                Stream stream = File.OpenWrite(fileName);
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(content);
                }
            }

            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        string GetCHNFieldName(string remarks)
        {
            int idx = remarks.IndexOfAny(new char[] { '(', '（' }, 0);
            return idx == -1 ? remarks : remarks.Substring(0, idx);
        }

        string GetRule(string remarks)
        {
            int idx = remarks.IndexOfAny(new char[] { '(', '（' }, 0);

            return idx == -1 ? string.Empty : remarks.Substring(idx + 1).Replace(")", string.Empty).Replace("）", string.Empty);
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog save = new FolderBrowserDialog();
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.tbPath.Text = save.SelectedPath+"\\";
            }
        }
    }
}