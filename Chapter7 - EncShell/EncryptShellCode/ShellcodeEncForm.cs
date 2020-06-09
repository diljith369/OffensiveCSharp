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

namespace EncryptShellCode
{
    public partial class ShellcodeEncForm : Form
    {
        public ShellcodeEncForm()  
        {
            InitializeComponent();
        }

        private void btnKeyGen_Click(object sender, EventArgs e)
        {
            txtkey.Text = Convert.ToBase64String(GetKey(16));
            txtIV.Text = Convert.ToBase64String(GetIV(16));
        }


        private  byte[] GetIV(int num)
        {
            var randomBytes = new byte[num]; // 32 Bytes will give us 256 bits.

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }

        public byte[] GetKey(int size)
        {
            char[] caRandomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()".ToCharArray();
            byte[] baKey = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(baKey);
            }
            return baKey;
        }

        public byte[] Encrypt(byte[] baShellcode, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(baShellcode, encryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        private byte[] GetEncryptedShellCode(byte[] baShellCode, string strKey, string strIV)
        {
            
            byte[] baEncrypted;
            byte[] baKey, baIV;

           // HashAlgorithm hash = SHA256.Create();


           // baKey = hash.ComputeHash(Encoding.Unicode.GetBytes(strKey));

            baKey = Convert.FromBase64String(strKey);
            baIV = Convert.FromBase64String(strIV);
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                // aes.Mode = CipherMode.CBC;
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(baKey, baIV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(baShellCode);
                            cs.FlushFinalBlock();
                            baEncrypted = ms.ToArray();
                        }
                    }
                }
                
            }
            // Return encrypted data    
            return baEncrypted;
        }

        private void btnEncryptShellCode_Click(object sender, EventArgs e)
        {
            byte[] baEncrypted;
            byte[] baKey, baIV;

            // HashAlgorithm hash = SHA256.Create();


            // baKey = hash.ComputeHash(Encoding.Unicode.GetBytes(strKey));

            baKey = Convert.FromBase64String(txtkey.Text);
            baIV = Convert.FromBase64String(txtIV.Text);
            txtEncShellcode.Text = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(txtShellcode.Text), baKey, baIV));
        }
    }
}
