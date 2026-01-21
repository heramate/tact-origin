using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoInstall
{
    /// <summary>
    /// ModeUpdate Class
    /// </summary>
    public class ModUpdate
    {
        /// <summary>
        /// 업데이트 완료 후 실행되어야 할 프로그램
        /// </summary>
        public string exeFileName;
        /// <summary>
        /// 업데이트 완료 후 실행되어야 할 프로그램 Args
        /// </summary>
        public string exeFileArgs;
        /// <summary>
        /// 프로그램정보 xml 파일이름
        /// </summary>
        public string appInfofilename;
        /// <summary>
        /// 업데이트에 실행되어야 할 프로그램
        /// </summary>
        public string exeUpdateName;
        /// <summary>
        /// Install에 실행되어야 할 프로그램
        /// </summary>
        public string exeInstallName;
        /// <summary>
        /// 업데이트 Install URL  경로
        /// </summary>
        public string installURL;       // 업데이트 서버 Install URL 경로
        /// <summary>
        /// 업데이트 서버 URL  경로
        /// </summary>
        public string updateURL;       // 업데이트 서버 URL 경로
        /// <summary>
        /// 업데이트할 버전
        /// </summary>
        public string appVersion;     
        /// <summary>
        /// 현재 update 버전
        /// </summary>
        public string updateVersion;      // 현재 Application 버전.
        /// <summary>
        /// 현재 Application 마지막 업데이트 시간
        /// </summary>
        public string updateTime;      // 마지막 업데이트 시간
        /// <summary>
        /// 업데이트 할 버전
        /// </summary>
        public string patchVersion;
        /// <summary>
        /// 현재 Application 마지막 업데이트 시간
        /// </summary>
        public string patchTime;     
        /// <summary>
        /// 설정파일 버전
        /// </summary>
        public string IniVersion;
        /// <summary>
        /// 설정파일 마지막 업데이트 시간
        /// </summary>
        public string IniUpdateTime;  
        /// <summary>
        /// 업데이트 파일을 다운로드하여 저장할 Path
        /// </summary>
        public string download_path;
        /// <summary>
        /// 업데이트 파일의 프로파일 xml 경로
        /// </summary>
        public string ProfileURL;    // 업데이트 할 버전의 xml 경로
        /// <summary>
        /// 다운로드 받을 파일 숫자
        /// </summary>
        public int filecount;
        /// <summary>
        /// 다운로드 받을 전체 파일크기 bytes
        /// </summary>
        public long totalFileSize;

        public String appExecutablePath;

        public String appPath;

        public List<XmlNode> UpdateInfos;

        public string tempPath;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        public enum E_ExeType
        {
            Install = 0,
            Update,
        }

        public ModUpdate()
        {
            // 프로그램 정보 ini 파일
            appInfofilename = "CLParam.ini";
            updateURL = "";
            installURL = "http://118.217.79.48:43213/Programs/REM01/RactClient_Download/PatchUpdate/";
            appVersion = "";
            updateTime = "";
            patchVersion = "";
            ProfileURL = "";
            filecount = 0;
            totalFileSize = 0;

            appPath = "";
            appExecutablePath = "";

            UpdateInfos = new List<XmlNode>();

            tempPath = "";
        }


        public ModUpdate(String appExePath)
        {
            // 프로그램 정보 ini 파일
            appInfofilename = "CLParam.ini";
            updateURL = "";
            installURL = "http://118.217.79.48:43213/Programs/REM01/RactClient_Download/PatchUpdate/";
            appVersion = "";
            updateTime = "";
            patchVersion = "";
            ProfileURL = "";
            filecount = 0;
            totalFileSize = 0;

            appPath = "";
            appExecutablePath = appExePath;

            UpdateInfos = new List<XmlNode>();

            tempPath = appExePath + "\\Tmp";
        }

        /// <summary>
        /// 업데이트 정보를 Set
        /// </summary>
        /// <param name="sFilename"></param>
        /// <param name="sUpdateURL"></param>
        /// <param name="sAppVersion"></param>
        /// <param name="sUpdateTime"></param>
        /// <param name="sUpdateVersion"></param>
        /// <param name="sUpdateProfileURL"></param>
        public void SetUpdateInfo(string sFilename,
                              string sUpdateURL,
                              string sAppVersion,
                              string sUpdateTime,
                              string sUpdateVersion,
                              string sUpdateProfileURL)
        {
            this.appInfofilename = sFilename;
            this.updateURL = sUpdateURL;
            this.appVersion = sAppVersion;
            this.updateTime = sUpdateTime;
            this.patchVersion = sUpdateVersion;
            this.ProfileURL = sUpdateProfileURL;
        }

        /// <summary>
        /// 업데이트 서버에 연결해서 업데이트 유무를 판단한다.
        /// </summary>
        /// <returns>true / false</returns>
        public bool IsUpdate()
        {
            try
            {
                //XmlDownLoad(this.updateURL + "Update.xml", "Update.xml");

                //GetUpdateAppInfo();     // 업데이트할 프로그램의 환경정보 로드

                XmlNode xmlNode;
                String FilePath;
                bool bFileExists = false;
                bool bVersion = false;
                for (int i = 0; i < UpdateInfos.Count; i++)
                {
                    xmlNode = UpdateInfos[i];
                    FilePath = xmlNode["UpdatePath"].InnerText.Replace("%AppPath%", Application.StartupPath) + "\\" + xmlNode["OrgFileName"].InnerText;
                    //1.CheckFile    update_path.Replace("%AppPath%", Application.StartupPath);
                    FileInfo fi = new FileInfo(FilePath);
                    //FileInfo.Exists로 파일 존재유무 확인
                    if (!fi.Exists)
                    {
                        bFileExists = true;
                        break;
                    }
                }

                string appVer = this.appVersion.Replace(".", "");
                string upVer = this.patchVersion.Replace(".", "");

                if (int.Parse(upVer) > int.Parse(appVer))
                {
                    bVersion = true;
                }

                if(bFileExists == true || bVersion == true)
                {
                    return true;
                }
            
                return false;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        public bool IsUpdate(E_ExeType exeType)
        {
            try
            {
                //XmlDownLoad(this.updateURL + "Update.xml", "Update.xml");
                //GetUpdateAppInfo();     // 업데이트할 프로그램의 환경정보 로드

                string appVer = "";
                string upVer = "";
                XmlNode xmlNode;
                String FilePath;
                bool bFileExists = false;
                bool bVersion = false;
                for (int i = 0; i < UpdateInfos.Count; i++)
                {
                    xmlNode = UpdateInfos[i];
                    FilePath = xmlNode["UpdatePath"].InnerText.Replace("%AppPath%", Application.StartupPath) + "\\" + xmlNode["OrgFileName"].InnerText;
                    //1.CheckFile    update_path.Replace("%AppPath%", Application.StartupPath);
                    FileInfo fi = new FileInfo(FilePath);
                    //FileInfo.Exists로 파일 존재유무 확인
                    if (!fi.Exists)
                    {
                        bFileExists = true;
                        break;
                    }
                }

                upVer = this.appVersion.Replace(".", "");
                if (exeType == E_ExeType.Install)
                {
                    appVer = this.updateVersion.Replace(".", "");
                }
                else if (exeType == E_ExeType.Update)
                {
                    appVer = this.patchVersion.Replace(".", "");
                }

                //프로그램(Install,Update) 버전 확인
                if (int.Parse(upVer) > int.Parse(appVer))
                {
                    bVersion = true;
                }

                if (bFileExists == true || bVersion == true)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        /// <summary>
        /// 현재 Application의 KeyRecv.ini 파일을 읽어 환경을 값을 로드한다.
        /// </summary>
        public void GetAppInfo()
        {
            try
            {
                appPath = appExecutablePath + "\\" + appInfofilename;
                FileInfo fi = new FileInfo(appPath);
                if (!fi.Exists)
                {
                    INIDownLoad(this.installURL + "CLParam.ini", "CLParam.ini");
                }

                this.installURL = iniReadValue("Url", "InstallUrl");
                this.updateURL = iniReadValue("Url", "ProgramUrl");

                this.IniVersion = iniReadValue("Program", "IniVersion");
                this.IniUpdateTime = iniReadValue("Program", "IniUpdateTime");
                this.exeInstallName = iniReadValue("Program", "ProgramInstallName");
                this.exeUpdateName = iniReadValue("Program", "ProgramUpdateName");
                this.updateVersion = iniReadValue("Program", "UpdateVersion");
                this.updateTime = iniReadValue("Program", "UpdateTime");
                this.exeFileName = iniReadValue("Program", "ProgramName");
                this.exeFileArgs = iniReadValue("Program", "ProgramArgs");
                this.patchVersion = iniReadValue("Program", "PatchVersion");
                this.patchTime = iniReadValue("Program", "PatchTime");

                //ini 파일 못 읽어오면 ""로 넘오기때문에 따로 익셉션 던져주는 코드 추가

                if(IniVersion == "" || installURL == "" || updateURL == "" || 
                    exeInstallName == "" || exeUpdateName == "" || updateVersion == "" || 
                    exeFileName == "" || exeFileArgs == "" || patchVersion == "" 
                    )
                    //if (exeFileName == "" || appVersion == "" || updateTime == "" || updateURL == "" || exeUpdateName == "" || exeInstallName == "" || installURL == "")
                {
                    throw new ApplicationException("환경설정파일을 읽을 수 없습니다.\n\n프로그램을 실행할 수 없습니다.");
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("환경설정파일을 읽을 수 없습니다.\n\n프로그램을 실행할 수 없습니다.");
            }
        }

        /// <summary>
        /// XML 파일을 업데이트 서버에서 다운로드하여 "Tmp" 디렉토리에 저장한다.
        /// </summary>
        /// <param name="url">xml 파일 url</param>
        private void FileDownLoad(string url, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse result = null;
            Stream ReceiveStream = null;
            FileStream fs = null;
            int read = 0;
            byte[] buffer = new byte[8192]; // 버퍼설정

            bool bSuccess;
            string errMSG = "";

            try
            {

                result = (HttpWebResponse)request.GetResponse();
                ReceiveStream = result.GetResponseStream(); // Response로 부터 Stream 얻음.
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                fs = new FileStream(tempPath + "\\" + fileName, FileMode.Create);
                while ((read = ReceiveStream.Read(buffer, 0, buffer.Length - 1)) > 0)
                {
                    fs.Write(buffer, 0, read);
                }
                bSuccess = true;
            }
            catch (Exception e)
            {
                errMSG = e.Message;
                bSuccess = false;
            }
            finally
            {
                if (fs != null) fs.Close();
                if (ReceiveStream != null) ReceiveStream.Close();
                if (result != null) request.Abort();
                if (request != null) request.Abort();
            }

            if (bSuccess == false)
            {
                throw new ApplicationException("서버로부터 업데이트관련 정보를 가져오는데 실패하였습니다.\n\n프로그램을 실행할 수 없습니다.");
            }
        }

        private void INIDownLoad(string url, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse result = null;
            Stream ReceiveStream = null;
            FileStream fs = null;
            int read = 0;
            byte[] buffer = new byte[8192]; // 버퍼설정

            bool bSuccess;
            string errMSG = "";

            try
            {

                result = (HttpWebResponse)request.GetResponse();
                ReceiveStream = result.GetResponseStream(); // Response로 부터 Stream 얻음.
                //Directory.CreateDirectory("Tmp");
                fs = new FileStream(fileName, FileMode.Create);
                while ((read = ReceiveStream.Read(buffer, 0, buffer.Length - 1)) > 0)
                {
                    fs.Write(buffer, 0, read);
                }
                bSuccess = true;
            }
            catch (Exception e)
            {
                errMSG = e.Message;
                bSuccess = false;
            }
            finally
            {
                if (fs != null) fs.Close();
                if (ReceiveStream != null) ReceiveStream.Close();
                if (result != null) request.Abort();
                if (request != null) request.Abort();
            }

            if (bSuccess == false)
            {
                throw new ApplicationException("서버로부터 INI 정보를 가져오는데 실패하였습니다.\n\n프로그램을 실행할 수 없습니다.");
            }
        }

        /// <summary>
        /// update.xml 파일을 읽어 update 파일정보를 파싱한다.
        /// </summary>
        public void GetUpdateAppInfo()
        {

            XmlDocument xd;
            xd = new XmlDocument();

            try
            {
                FileDownLoad(this.updateURL + "Update.xml", "Update.xml");

                xd.Load(tempPath + "\\Update.xml");
                XmlNodeList nodeList = xd.GetElementsByTagName("LastVersion");

                this.patchVersion = nodeList[0]["Version"].InnerText;
                this.ProfileURL = this.updateURL + "/" + this.patchVersion + "/Profile.xml";
                
                // profile.xml 다운로드
                FileDownLoad(this.ProfileURL, "Profile.xml");

                xd.Load(tempPath + "\\Profile.xml");
                nodeList = xd.GetElementsByTagName("File");
                this.filecount = nodeList.Count;

                this.totalFileSize = 0;
                for (int i = 0; i < this.filecount; i++)
                {
                    UpdateInfos.Add(nodeList[i]);
                    this.totalFileSize = this.totalFileSize + long.Parse(nodeList[i]["FileSize"].InnerText);
                }
                
            }
            catch (Exception e)
            {
                throw new ApplicationException("서버로부터 업데이트관련 정보를 가져오는데 실패하였습니다.\n\n프로그램을 실행할 수 없습니다.");
            }
        }

        public void GetUpdateAppInfo(E_ExeType exeType)
        {

            XmlDocument xd;
            xd = new XmlDocument();

            try
            {
                // Update.xml 다운로드
                if (exeType == E_ExeType.Install)
                    FileDownLoad(this.installURL + "Update.xml", "Update.xml");
                else if (exeType == E_ExeType.Update)
                    FileDownLoad(this.updateURL + "Update.xml", "Update.xml");

                xd.Load(tempPath + "\\Update.xml");
                XmlNodeList nodeList = xd.GetElementsByTagName("LastVersion");

                this.appVersion = nodeList[0]["Version"].InnerText;

                if (exeType == E_ExeType.Install)
                {
                    //서버에있는 설정 파일 다운로드
                    FileDownLoad(this.installURL + "CLParam.ini", "CLParam.ini");

                    this.ProfileURL = this.installURL + "/" + this.appVersion + "/Profile.xml";
                }
                else if (exeType == E_ExeType.Update)
                {
                    this.ProfileURL = this.updateURL + "/" + this.appVersion + "/Profile.xml";
                }
                
                // profile.xml 다운로드
                FileDownLoad(this.ProfileURL, "Profile.xml");

                xd.Load(tempPath + "\\Profile.xml");
                nodeList = xd.GetElementsByTagName("File");
                this.filecount = nodeList.Count;

                this.totalFileSize = 0;
                for (int i = 0; i < this.filecount; i++)
                {
                    UpdateInfos.Add(nodeList[i]);
                    this.totalFileSize = this.totalFileSize + long.Parse(nodeList[i]["FileSize"].InnerText);
                }

            }
            catch (Exception e)
            {
                throw new ApplicationException("서버로부터 업데이트관련 정보를 가져오는데 실패하였습니다.\n\n프로그램을 실행할 수 없습니다.");
            }
        }

        public void iniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.appPath);
        }

        public string iniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(1000);
            int i = GetPrivateProfileString(Section, Key, "", temp, 1000, this.appPath);
            return temp.ToString();
        }

        public string GetfileVersion(String FilePath)
        {
            string Version = "";

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(FilePath);

            if (info.FileVersion != null)
            {
                Version = info.FileVersion;
            }
           
            return Version;
        }
    }
}
