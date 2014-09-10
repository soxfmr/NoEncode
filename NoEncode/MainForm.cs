using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace NoEncode
{
    public partial class MainForm : Form
    {
        private string CURRENT_WORKSPACE = String.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        public void OnLoad(Encoding encoding)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CURRENT_WORKSPACE = dialog.FileName;
                if (CURRENT_WORKSPACE != String.Empty)
                {
                    Thread mainThread = new Thread(new ThreadStart(delegate
                    {
                        using (StreamReader reader = new StreamReader(@CURRENT_WORKSPACE))
                        {
                            mainEditor.Invoke(new Action(delegate
                            {
                                mainEditor.Text = reader.ReadToEnd();
                            }));
                            reader.Close();
                        }
                    }));
                    mainThread.IsBackground = true;
                    mainThread.Start();
                    
                    ShowWorkSpace(CURRENT_WORKSPACE);
                }
            }
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            CodeChoose cc = new CodeChoose(this);
            cc.ShowDialog();
        }

        private void mainEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                OnSave(append : true);
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            OnSave(append : false);
        }

        private void OnSave(bool append)
        {
            string buffer = mainEditor.Text;
            if (buffer == String.Empty) return;

            if (append)
            {
                if (CURRENT_WORKSPACE == String.Empty)
                {
                    OnSave(false);
                    return;
                }
                SaveToFile(CURRENT_WORKSPACE, buffer);
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "NoEncode File|*.ne.txt";
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    CURRENT_WORKSPACE = dialog.FileName;
                    if (CURRENT_WORKSPACE == String.Empty) return;

                    SaveToFile(CURRENT_WORKSPACE, buffer);
                } 
            }

            ShowWorkSpace(CURRENT_WORKSPACE);
        }

        private void SaveToFile(string fileName, string buffer)
        {
            Thread savingThread = new Thread(new ThreadStart(delegate
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(buffer);
                    writer.Flush();
                    writer.Close();
                }

            }));

            savingThread.IsBackground = true;
            savingThread.Start();
        }

        private void ShowWorkSpace(string workspace)
        {
            this.Text = workspace == String.Empty ? "NoEncode" : "NoEncoding - " + CURRENT_WORKSPACE;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            mainEditor.Clear();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Owner, "Author: HuangYu \n\nHuangYu Copyright @ 2014", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            OnSave(append : true);
            mainEditor.Clear();

            CURRENT_WORKSPACE = String.Empty;
            ShowWorkSpace(CURRENT_WORKSPACE);
        }

    }
}
