using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using MCUpdateLauncherCommon;

namespace RACTClient
{
    public class UpdateUtil
    {
        /// <summary>
        /// Remote Server URI 입니다.
        /// </summary>
        private string m_RemoteObjectUri = string.Empty;
        /// <summary>
        /// Remote Service 입니다.
        /// </summary>
        private IUpdateService m_RemoteService;
        /// <summary>
        /// Application Name 입니다.
        /// </summary>
        private string m_ApplicationName;
        /// <summary>
        /// UpdateLauncher 프로그램 관련 파일명의 공통된 단어입니다.
        /// </summary>
        private string m_UpdateLauncherFileName = "UpdateLauncher";

        /// <summary>
        /// program Args
        /// </summary>
        private string m_Args = "";

        /// <summary>
        /// UpdateUtil기본 생성자 입니다.<see cref="UpdateUtil"/>class.
        /// </summary>
        /// <param name="aOwner"> Owner 입니다.</param>
        /// <param name="aRemoteObjectUri">Remote Object URI입니다.</param>
        public UpdateUtil(string aRemoteObjectUri, string[] aArgs)
        {
            m_RemoteService = null;
            m_RemoteObjectUri = aRemoteObjectUri;

            //바탕화면 바로가기 아이콘으로 실행한 경우 자동로그인을 하지 않기 때문에 args를 사용하지 않음.
            if (aArgs == null)
                return;

            for (int i = 0; i < aArgs.Length; i++)
            {
                m_Args = string.Concat(m_Args, aArgs[i], " ");
            }
        }

        /// <summary>
        /// 서버에 접속을 시도 합니다.
        /// </summary>
        /// <returns>접속 여부 입니다.</returns>
        private bool ConnectRemoteServer()
        {
            try
            {
                m_RemoteService = (IUpdateService)Activator.GetObject(typeof(IUpdateService), m_RemoteObjectUri);

                if (m_RemoteService.GetFiles() == null)
                    return false;
            }
            catch (Exception remoteException)
            {
                System.Diagnostics.Trace.WriteLine(remoteException.Message);
                return false;
            }
            return m_RemoteService != null;
        }

        /// <summary>
        /// UpdateLauncher 파일에 대한 버전을 비교합니다.
        /// </summary>
        /// <returns></returns>
        private bool CheckUpdateFiles(bool aIsUpdateLauncher)
        {
            DirectoryInfo tCurrentDir = new DirectoryInfo(Application.StartupPath);

            UpdateListCollection tCurDirFileList = new UpdateListCollection();
            UpdateListInfo tCurDirFile = null;
            if (aIsUpdateLauncher)
            {
                foreach (FileInfo tFileInfo in tCurrentDir.GetFiles())
                {
                    if (tFileInfo.Name.IndexOf(m_UpdateLauncherFileName) > -1)
                    {
                        tCurDirFile = new UpdateListInfo(E_UpdateFileType.File, tFileInfo, GetVersion(tFileInfo));
                        tCurDirFileList.Add(tCurDirFile);
                    }
                }
            }
            else
            {
                foreach (FileInfo tFileInfo in tCurrentDir.GetFiles())
                {
                    if (tFileInfo.Name.IndexOf(m_UpdateLauncherFileName) > -1)
                        continue;

                    tCurDirFile = new UpdateListInfo(E_UpdateFileType.File, tFileInfo, GetVersion(tFileInfo));
                    tCurDirFileList.Add(tCurDirFile);
                }
            }

            if (tCurDirFileList.Value == null) return true;

            foreach (UpdateListInfo tUpdateListInfo in m_RemoteService.GetFiles())
            {
                if (aIsUpdateLauncher)
                {
                    if (tUpdateListInfo.Name.IndexOf(m_UpdateLauncherFileName) == -1)
                        continue;
                }
                else
                {
                    if (tUpdateListInfo.Name.IndexOf(m_UpdateLauncherFileName) > -1)
                        continue;
                }

                foreach (UpdateListInfo tCurDirFileInfo in tCurDirFileList.Value)
                {
                    if (tCurDirFileInfo.Name.ToLower() != tUpdateListInfo.Name.ToLower())
                        continue;

                    if (IsUpdateNecessary(tCurDirFileInfo.Version, tUpdateListInfo.Version))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Version을 얻습니다.
        /// </summary>
        /// <param name="fInfo">The f info.</param>
        /// <returns>기본 생성자 입니다.</returns>
        private string GetVersion(FileInfo aFInfo)
        {
            try
            {
                Assembly tAssembly = Assembly.LoadFile(aFInfo.FullName);
                if (tAssembly != null)
                {
                    string[] tParts = tAssembly.FullName.Split(" ".ToCharArray());
                    foreach (string tPart in tParts)
                    {
                        if (tPart.ToLower().IndexOf("version") >= 0)
                        {
                            string tRes = tPart.Trim();
                            tRes = tRes.Substring(tRes.IndexOf("=") + 1);
                            return tRes.Trim().Replace(",", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Version을 Update 여부를 확인 합니다.
        /// </summary>
        /// <param name="aLocalVersion">Local Version 입니다.</param>
        /// <param name="aRemoteVersion">Remote Version 입니다.</param>
        /// <returns>Update 여부 입니다.</returns>
        private bool IsUpdateNecessary(string aLocalVersion, string aRemoteVersion)
        {
            try
            {
                long tLocalVersion = Convert.ToInt64(aLocalVersion.Replace(".", ""));
                long tRemoteVersion = Convert.ToInt64(aRemoteVersion.Replace(".", ""));

                return tLocalVersion < tRemoteVersion;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Update를 실행 합니다.
        /// </summary>
        public E_UpdateStatus Update()
        {
            if (!ConnectRemoteServer())
            {
                return E_UpdateStatus.ServerNotConnected;
            }

            if (CheckUpdateFiles(true))
            {
                return E_UpdateStatus.OldLauncher;
            }

            //주 프로그램 업데이트 체크.
            if (CheckUpdateFiles(false))
            {
                if (MessageBox.Show(null, "새로운 Version의 파일이 있습니다. Update 하시겠습니까?", "UpdateLauncherClient", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string tUpdateAppPath = string.Concat(Application.StartupPath, "\\UpdateLauncherClient.exe");

                    Process tUpdateProcess = new Process();
                    if (m_Args.Trim().Length == 0)
                    {
                        tUpdateProcess.StartInfo = new ProcessStartInfo(tUpdateAppPath, Process.GetCurrentProcess().MainModule.FileName + " " + m_RemoteObjectUri);
                    }
                    else
                    {
                        tUpdateProcess.StartInfo = new ProcessStartInfo(tUpdateAppPath, Process.GetCurrentProcess().MainModule.FileName + " " + m_RemoteObjectUri + " " + m_Args);
                    }

                    tUpdateProcess.Start();
                    return E_UpdateStatus.OldProgram;
                }
                else
                    return E_UpdateStatus.RecentlyVersion;
            }
            else
                return E_UpdateStatus.RecentlyVersion;
        }
    }
}
