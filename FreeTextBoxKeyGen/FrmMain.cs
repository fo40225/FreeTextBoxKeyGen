using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeTextBoxKeyGen
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private readonly byte[] encryptionKeyBytes = new byte[] { 57, 72, 66, 50, 56, 49, 70, 54 };

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                return;
            }
            string Name = txtName.Text;

            int intNameLength = Name.Length;

            string strNameLength = intNameLength.ToString();

            while (strNameLength.Length < 5)
            {
                strNameLength = strNameLength.Insert(0, "0");
            }

            string PlainText = strNameLength + Name;
            ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
            byte[] PlainArray = aSCIIEncoding.GetBytes(PlainText);

            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            ICryptoTransform transform = dESCryptoServiceProvider.CreateEncryptor(encryptionKeyBytes, encryptionKeyBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);

            string CipherText = string.Empty;

            if (PlainArray.Length % 24 == 0)
            {
                cryptoStream.Write(PlainArray, 0, PlainArray.Length);
                CipherText = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                cryptoStream.Write(PlainArray, 0, PlainArray.Length);
                cryptoStream.Write(PlainArray, 0, PlainArray.Length);
                int byteCount = (PlainArray.Length / 24 + 1) * 24;
                CipherText = Convert.ToBase64String(memoryStream.ToArray().Take(byteCount).ToArray());
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FreeTextBox License");
            sb.AppendLine("[qeUPffrT7QkE0JgbUKQPxMiBFyyieNJh]");
            sb.Append("[");
            sb.Append(CipherText);
            sb.Append("]");

            File.WriteAllText("FreeTextBox.lic", sb.ToString(), Encoding.ASCII);
        }
    }
}