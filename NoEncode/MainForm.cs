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
using System.Xml;

namespace NoEncode
{
    public partial class MainForm : Form
    {
        private const string CONFIG_ROOT_DIR_NAME = "configs";

        private static int SELECTION_START = 0;
        private static int SELECTION_LENGTH = 0;

        private string CONFIG_ROOT_PATH = String.Empty;

        private string CURRENT_WORKSPACE = String.Empty;
        private string DEFAULT_WORKSPACE_NAME = String.Empty;

        public MainForm()
        {
            InitializeComponent();

            // 初始化配置文件根目录
            CONFIG_ROOT_PATH = Directory.GetCurrentDirectory();
            CONFIG_ROOT_PATH += "\\" + CONFIG_ROOT_DIR_NAME;
            if (!Directory.Exists(CONFIG_ROOT_PATH))
                Directory.CreateDirectory(CONFIG_ROOT_PATH);
        }

        public void OnLoad(Encoding encoding)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                CURRENT_WORKSPACE = dialog.FileName;

                if (CURRENT_WORKSPACE != String.Empty && File.Exists(CURRENT_WORKSPACE))
                {
                    string fn = dialog.SafeFileName;
                    DEFAULT_WORKSPACE_NAME = GetWorkSpaceName(fn);

                    Thread mainThread = new Thread(new ThreadStart(delegate
                    {
                        using (StreamReader reader = new StreamReader(@CURRENT_WORKSPACE, encoding))
                        {
                            mainEditor.Invoke(new Action(delegate
                            {
                                mainEditor.Text = reader.ReadToEnd();
                            }));
                            reader.Close();
                        }

                        reloadStatus();
                    }));
                    mainThread.IsBackground = true;
                    mainThread.Start();

                    ShowWorkSpace(DEFAULT_WORKSPACE_NAME);
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

                    string fn = dialog.FileName;
                    fn = fn.Substring(fn.LastIndexOf("\\") + 1);
                    DEFAULT_WORKSPACE_NAME = GetWorkSpaceName(fn);

                    SaveToFile(CURRENT_WORKSPACE, buffer);
                } 
            }

            ShowWorkSpace(DEFAULT_WORKSPACE_NAME);
        }

        private void SaveToFile(string fileName, string buffer)
        {
            Thread savingThread = new Thread(new ThreadStart(delegate
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.Write(buffer);
                    writer.Flush();
                    writer.Close();
                }

                recordStatus();
            }));

            savingThread.IsBackground = true;
            savingThread.Start();
        }

        private void ShowWorkSpace(string workspace)
        {
            this.Text = workspace == String.Empty ? "NoEncode" : "NoEncoding - " + workspace;
        }

        private string GetWorkSpaceName(string fn)
        {
            if (fn == null || fn.Length <= 0)
                return String.Empty;

            string Ret = fn;
            // 可能没有扩张名
            int index = fn.LastIndexOf(".");
            if (index != -1)
            {
                Ret = fn.Substring(0, index);
            }

            return Ret;
        }

        private string GetConfigFilePath()
        {
            string Ret = null;
            if (DEFAULT_WORKSPACE_NAME != null && DEFAULT_WORKSPACE_NAME.Length >= 0)
            {
                StringBuilder dir = new StringBuilder();
                dir.Append(CONFIG_ROOT_PATH);
                dir.Append("\\");
                dir.Append(DEFAULT_WORKSPACE_NAME);
                // 创建文件夹
                Ret = dir.ToString();
                if (!Directory.Exists(Ret))
                    Directory.CreateDirectory(Ret);

                dir.Append("\\config.xml");
                Ret = dir.ToString();
            }

            return Ret;
        }

        private void reloadStatus()
        {
            string path = GetConfigFilePath();
            if(path != null && File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@path);

                XmlElement eleRoot = doc.DocumentElement;

                string ss = ((XmlElement)eleRoot.SelectSingleNode("SelectionStart")).GetAttribute("value");
                string sl = ((XmlElement)eleRoot.SelectSingleNode("SelectionLength")).GetAttribute("value");

                try
                {
                    int SelectionStart = Int32.Parse(ss);
                    int SelectionLength = Int32.Parse(sl);
                    // 定位到上次工作位置
                    mainEditor.Invoke(new Action(delegate
                    {
                        if (mainEditor.Text.Length >= SelectionStart)
                        {
                            mainEditor.Select(SelectionLength, SelectionStart);
                            mainEditor.ScrollToCaret();
                        }
                    }));
                    
                }catch(Exception) {
                    mainEditor.Select(SELECTION_LENGTH, SELECTION_START);
                }
            }
        }

        private void recordStatus()
        {
            string path = GetConfigFilePath();
            if (path != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.CreateXmlDeclaration("1.0", "utf-8", "yes");

                XmlElement eleRoot = doc.CreateElement("config");
                XmlElement eleSectionStart = doc.CreateElement("SelectionStart");
                XmlElement eleSectionLength = doc.CreateElement("SelectionLength");

                eleSectionLength.SetAttribute("value", SELECTION_START + "");
                eleSectionStart.SetAttribute("value", SELECTION_LENGTH + "");

                eleRoot.AppendChild(eleSectionStart);
                eleRoot.AppendChild(eleSectionLength);
                doc.AppendChild(eleRoot);

                doc.Save(@path);
            }
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

        private void mainEditor_MouseUp(object sender, MouseEventArgs e)
        {
            SELECTION_START = mainEditor.SelectionStart;
            SELECTION_LENGTH = mainEditor.SelectionLength;
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            OnSave(append: false);
        }

    }
}
