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
using System.Security.Cryptography;

namespace SanityArchiveCC
{
    public partial class Form1 : Form
    {
        string key;

        public Form1()
        {
            InitializeComponent();
            key = generateKey();
        }

        private string generateKey()
        {
            DESCryptoServiceProvider desCrypt = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypt.Key);
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

        

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            txtEncFile.Text = ofd.FileName;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            txtEncDest.Text = sfd.FileName;

            encrypt(txtEncFile.Text, txtEncDest.Text, key);
        }

        private void encrypt(string input, string output, string strhash)
        {
            FileStream inStream, outSream;
            CryptoStream CryStream;
            TripleDESCryptoServiceProvider tdc = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteText;

            inStream = new FileStream(input, FileMode.Open, FileAccess.Read);
            outSream = new FileStream(output, FileMode.Create, FileAccess.Write);

            byteHash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strhash));
            byteText = File.ReadAllBytes(input);

            md5.Clear();

            tdc.Key = byteHash;
            tdc.Mode = CipherMode.ECB;

            CryStream = new CryptoStream(outSream, tdc.CreateEncryptor(), CryptoStreamMode.Write);

            int bytesRead;
            long lenght, position = 0;
            lenght = inStream.Length;
            while (position < lenght)
            {
                bytesRead = inStream.Read(byteText, 0, byteText.Length);
                position += bytesRead;

                CryStream.Write(byteText, 0, bytesRead);
            }

            inStream.Close();
            outSream.Close();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            txtDecFile.Text = ofd.FileName;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            txtDecDest.Text = sfd.FileName;

            decrypt(txtDecFile.Text, txtDecDest.Text, key);
        }

        private void decrypt(string input, string output, string strhash)
        {
            FileStream inStream, outSream;
            CryptoStream CryStream;
            TripleDESCryptoServiceProvider tdc = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteText;

            inStream = new FileStream(input, FileMode.Open, FileAccess.Read);
            outSream = new FileStream(output, FileMode.Create, FileAccess.Write);

            byteHash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strhash));
            byteText = File.ReadAllBytes(input);

            md5.Clear();

            tdc.Key = byteHash;
            tdc.Mode = CipherMode.ECB;

            CryStream = new CryptoStream(outSream, tdc.CreateDecryptor(), CryptoStreamMode.Write);

            int bytesRead;
            long lenght, position = 0;
            lenght = inStream.Length;
            while (position < lenght)
            {
                bytesRead = inStream.Read(byteText, 0, byteText.Length);
                position += bytesRead;

                CryStream.Write(byteText, 0, bytesRead);
            }

            inStream.Close();
            outSream.Close();
        }
    }
}
