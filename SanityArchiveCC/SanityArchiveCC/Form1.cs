using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SanityArchiveCC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Archive archive = new Archive();

        private void Open_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your directory." })
            {
                if (fbd.ShowDialog()==DialogResult.OK)
                {
                    webBrowser.Url = new Uri(fbd.SelectedPath);
                    txtPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        private void Compress_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            string path = string.Empty;
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                path = Fbd.SelectedPath;
            }
            DirectoryInfo Di = new DirectoryInfo(path);
            foreach (FileInfo Fi in Di.GetFiles())
            {
                if (Yes)
                {
                    archive.Compress(Fi);
                    File.Delete(Fi.FullName);
                }
                else
                {
                    archive.Compress(Fi);
                }
            }
        }

        private void Extract_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            string path = string.Empty;
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                path = Fbd.SelectedPath;
            }
            DirectoryInfo Di = new DirectoryInfo(path);
            foreach (FileInfo Fi in Di.GetFiles())
            {
                archive.Extract(Fi);
                File.Delete(Fi.FullName);
            }
        }

        bool Yes = false;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Yes = true;
                return;
            }
            else
            {
                Yes = false;
            }
        }
    }
}
