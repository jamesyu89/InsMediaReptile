using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using InstagramPhotos.CodeGender.DB;

namespace InstagramPhotos.CodeGender
{
    public partial class frmConn : Form
    {
        Action load;

        public frmConn(Action load)
        {
            InitializeComponent();
            this.load = load;

            if (File.Exists(DataAccess.configPatch))
                this.lstConns.DataSource = File.ReadAllLines(DataAccess.configPatch);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmConnStr con = new frmConnStr(load);
            con.ShowDialog();
            this.Close();
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            if (lstConns.SelectedIndex >= 0)
            {
                DataAccess.conn = lstConns.SelectedItem.ToString();

                try
                {
                    DataAccess.GetTables();
                    this.Close();
                    load();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("还不对\r\n" + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstConns.SelectedIndex >= 0)
            {
                lstConns.Items.Remove(lstConns.SelectedItem);

                List<string> conns = new List<string>();

                foreach (var item in lstConns.Items)
                {
                    conns.Add(item.ToString());
                }

                File.WriteAllLines(DataAccess.configPatch, conns.ToArray());
            }
        }

        private void lstConns_DoubleClick(object sender, EventArgs e)
        {
            btnConn_Click(sender, e);
        }
    }
}
