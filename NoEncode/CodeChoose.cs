using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoEncode
{
    public partial class CodeChoose : Form
    {
        private Form parent = null;
        public CodeChoose(Form parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string encType = commBoxEncoding.Text;
            if (encType == "") return;

            try
            {
                Encoding encoding = Encoding.GetEncoding(encType);
                
                MainForm frmMain = (MainForm)parent;
                frmMain.OnLoad(encoding);
            }
            catch (ArgumentException) {
                MessageBox.Show(Owner, "Invalid encoding!", "MessageBox", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
    }
}
