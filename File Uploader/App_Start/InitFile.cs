using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace File_Uploader.App_Start
{
    class INIFile
    {
        private string filePath;
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
            (
                [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
                // Note that because the key/value pars are returned as null-terminated
                // strings with the last string followed by 2 null-characters, we cannot
                // use StringBuilder.
                [In] IntPtr pReturnedString,
                [In] UInt32 nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIFile(string filePath)
        {
            this.filePath = filePath;
        }

        public void Write(string section, string key, string value)
        {
            long a = WritePrivateProfileString(section, key, value, this.filePath);
            System.Threading.Thread.Sleep(60);
        }

        public string Read(string section, string key, string def)
        {
            string strReturnVal = "";
            try
            {
                StringBuilder SB = new StringBuilder(255);
                int i = GetPrivateProfileString(section, key, def, SB, 255, this.filePath);
                strReturnVal = SB.ToString();
            }
            catch (Exception)
            {
                strReturnVal = "";
            }
            return strReturnVal;
        }

        public bool IniReadDateValue(string Section, string Key, out DateTime objDT, out string strExcp)
        {
            try
            {
                StringBuilder temp = new StringBuilder(25);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.filePath);

                objDT = new DateTime(Convert.ToInt32(temp.ToString().Substring(0, 4)), Convert.ToInt32(temp.ToString().Substring(5, 2)), Convert.ToInt32(temp.ToString().Substring(8, 2)), Convert.ToInt32(temp.ToString().Substring(11, 2)), Convert.ToInt32(temp.ToString().Substring(14, 2)), Convert.ToInt32(temp.ToString().Substring(17, 2)));
                strExcp = "";   //Added [Shubhit 03May13]
                return true;
            }
            catch (Exception excp)
            {
                objDT = DateTime.Now;
                strExcp = excp.Message.ToString();  //Added [Shubhit 03May13]
                return false;
            }
        }

        public double IniReadDoubleValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.filePath);
            double dRes;
            Double.TryParse(temp.ToString(), out dRes);
            return dRes;
        }

        public string[] GetAllKeysInIniFileSection(string strSectionName)
        {
            string[] strArray = null;
            try
            {

                // Allocate in unmanaged memory a buffer of suitable size.
                // I have specified here the max size of 32767 as documentated 
                // in MSDN.
                IntPtr pBuffer = Marshal.AllocHGlobal(32767);
                // Start with an array of 1 string only. 
                // Will embellish as we go along.

                strArray = new string[0];
                UInt32 uiNumCharCopied = 0;

                uiNumCharCopied = GetPrivateProfileSection(strSectionName, pBuffer, 32767, this.filePath);

                // iStartAddress will point to the first character of the buffer,
                int iStartAddress = pBuffer.ToInt32();
                // iEndAddress will point to the last null char in the buffer.
                int iEndAddress = iStartAddress + (int)uiNumCharCopied;

                // Navigate through pBuffer.
                while (iStartAddress < iEndAddress)
                {
                    // Determine the current size of the array.
                    int iArrayCurrentSize = strArray.Length;
                    // Increment the size of the string array by 1.
                    Array.Resize<string>(ref strArray, iArrayCurrentSize + 1);
                    // Get the current string which starts at "iStartAddress".
                    string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                    // Insert "strCurrent" into the string array.
                    strArray[iArrayCurrentSize] = strCurrent;
                    // Make "iStartAddress" point to the next string.
                    iStartAddress += (strCurrent.Length + 1);
                }

                Marshal.FreeHGlobal(pBuffer);
                pBuffer = IntPtr.Zero;
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Substring(strArray[i].LastIndexOf('=') + 1).ToLower();
                }
            }
            catch (Exception) { }//EventLog.WriteEntry("LipiWhiteListing", "Exception Message: " + ex.Message, //EventLogEntryType.Error); }

            return strArray;
        }
        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
    }
}