using System;
using System.IO;
using System.Windows.Forms;
using InstagramPhotos.CodeGender.DB;

namespace InstagramPhotos.CodeGender
{
    public partial class frmConnStr : Form
    {
        Action load;

        public frmConnStr(Action load)
        {
            InitializeComponent();
            this.load = load;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string conn = string.Format("server={0};uid={2};pwd={3};database={1}", tbServer.Text.Trim(), tbDB.Text.Trim(), tbUID.Text.Trim(), tbPWD.Text.Trim());

            DataAccess.conn = conn;

            try
            {
                DataAccess.GetTables();
                string txt = string.Empty;
                if (File.Exists(DataAccess.configPatch))
                    txt = File.ReadAllText(DataAccess.configPatch) + "\r" + conn;
                File.WriteAllText(DataAccess.configPatch, txt);
                this.Close();
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show("还不对\r\n" + ex.Message);
            }
        }
    }
}
