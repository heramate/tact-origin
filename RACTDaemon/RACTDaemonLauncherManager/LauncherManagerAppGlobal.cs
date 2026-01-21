using System;
using System.Collections.Generic;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Drawing;
using MKLibrary.Controls;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using RACTDaemonLauncher;
using MKLibrary.MKData;
using RACTDaemonProcess;
using System.Threading;


namespace RACTDaemonLauncherManager
{
    public static class LauncherManagerAppGlobal
    {
        // 2019.02.01 KwonTaeSuk 환경설정파일 통합(DaemonConfig.xml, RACTDaemonProcess.DaemonConfig.cs)
        //public static string s_LauncherConfigName = "RACTDaemonLauncherConfig.xml";
        /// <summary>
        /// TACTDaemon 환경설정 값
        /// </summary>
        public static DaemonConfig s_ManagerConfig = null;
        /// <summary>
        /// 그리드 스타일을 초기화 합니다.
        /// </summary>
        /// <param name="vGrid">스타일을 초기화 할 그리드 입니다.</param>
        public static void InitializeGridStyle(C1FlexGrid vGrid)
        {
            vGrid.Styles["Normal"].BackColor = Color.White;
            vGrid.Styles["Normal"].ForeColor = Color.FromArgb(100, 100, 100);
            vGrid.Styles["Normal"].Border.Color = Color.FromArgb(180, 180, 180);
            vGrid.Styles["Fixed"].BackColor = Color.FromArgb(152, 151, 198);
            vGrid.Styles["Fixed"].ForeColor = Color.White;
            vGrid.Styles["Fixed"].Border.Style = BorderStyleEnum.Flat;
            vGrid.Styles["Fixed"].Border.Color = Color.FromArgb(180, 180, 180);
            vGrid.Styles["EmptyArea"].BackColor = Color.FromArgb(245, 245, 245);
            vGrid.Styles["Highlight"].ForeColor = Color.White;
            vGrid.Styles["Highlight"].BackColor = Color.FromArgb(77, 80, 125);

            vGrid.AllowDragging = AllowDraggingEnum.None;
        }
        /// <summary>
        /// 그리드 추가 스타일을 그리드에 추가 합니다.
        /// </summary>
        /// <param name="vGrid">추가 스타일을 반영할 그리드 입니다.</param>
        public static void InitializeAddtionalGridStyle(C1FlexGrid vGrid)
        {
            CellStyle tCellStyle = null;

            if (!vGrid.Styles.Contains("DiffAdd"))
            {
                tCellStyle = vGrid.Styles.Add("DiffAdd", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(253, 238, 211);
                tCellStyle.ForeColor = Color.FromArgb(255, 72, 0);
            }

            if (!vGrid.Styles.Contains("DiffDel"))
            {
                tCellStyle = vGrid.Styles.Add("DiffDel", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(220, 249, 210);
                tCellStyle.ForeColor = Color.FromArgb(55, 167, 20);
            }

            if (!vGrid.Styles.Contains("DiffChange"))
            {
                tCellStyle = vGrid.Styles.Add("DiffChange", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(227, 225, 248);
                tCellStyle.ForeColor = Color.FromArgb(34, 20, 167);
            }

            if (!vGrid.Styles.Contains("DiffAddHit"))
            {
                tCellStyle = vGrid.Styles.Add("DiffAddHit", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(248, 204, 123);
                tCellStyle.ForeColor = Color.FromArgb(255, 72, 0);
            }

            if (!vGrid.Styles.Contains("DiffDelHit"))
            {
                tCellStyle = vGrid.Styles.Add("DiffDelHit", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(160, 238, 133);
                tCellStyle.ForeColor = Color.FromArgb(55, 167, 20);
            }

            if (!vGrid.Styles.Contains("DiffChangeHit"))
            {
                tCellStyle = vGrid.Styles.Add("DiffChangeHit", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(179, 175, 236);
                tCellStyle.ForeColor = Color.FromArgb(34, 20, 167);
            }

            if (!vGrid.Styles.Contains("ExceptLine"))
            {
                tCellStyle = vGrid.Styles.Add("ExceptLine", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(184, 184, 184);
                tCellStyle.ForeColor = Color.FromArgb(55, 55, 55);
            }

            if (!vGrid.Styles.Contains("ExceptLineHit"))
            {
                tCellStyle = vGrid.Styles.Add("ExceptLineHit", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(113, 113, 113);
                tCellStyle.ForeColor = Color.FromArgb(55, 55, 55);
            }

            if (!vGrid.Styles.Contains("CriticalLine"))
            {
                tCellStyle = vGrid.Styles.Add("CriticalLine", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(252, 210, 210);
                tCellStyle.ForeColor = Color.FromArgb(255, 0, 0);
            }

            if (!vGrid.Styles.Contains("CriticalLineHit"))
            {
                tCellStyle = vGrid.Styles.Add("CriticalLineHit", vGrid.Styles.Normal);
                tCellStyle.BackColor = Color.FromArgb(248, 150, 150);
                tCellStyle.ForeColor = Color.FromArgb(255, 0, 0);
            }
            //선택한 Cell 영역 HightLight를 사용하지 않는다. RowColChanged를 통해 Style에 맞게 색깔을 변경시킨다.
            vGrid.HighLight = HighLightEnum.Never;
        }

        /// <summary>
        /// 서버 System정보를 가져오기 합니다.
        /// </summary>
        /// <returns>서비 System정보 가져오기의 성공 여부 입니다.</returns>
        public static bool LoadSystemInfo()
        {
            ArrayList tSystemInfos = null;
            MKXML tXML = new MKXML();
            E_XmlError tErrorString;


            FileInfo tFileInfo = new FileInfo(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName);
            if (!tFileInfo.Exists)
            {
                MKXML.ObjectToXML(tFileInfo.FullName, new DaemonConfig());
                MessageBox.Show(string.Format("환경정보 파일이 없어 새로 생성하였습니다({0})", tFileInfo.FullName)); 
                return false;
            }

            try
            {
                tSystemInfos = MKXML.ObjectFromXML(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName, typeof(DaemonConfig), out tErrorString);
                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                LauncherManagerAppGlobal.s_ManagerConfig = (DaemonConfig)tSystemInfos[0];

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LoadSystemInfo] 예외발생 " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 쓰레드를 강제 종료합니다.
        /// </summary>
        /// <param name="aThread"></param>
        public static void StopThread(Thread aThread)
        {
            if (aThread != null)
            {
                aThread.Join(100);
                if (aThread.IsAlive)
                {
                    try
                    {
                        aThread.Abort();
                    }
                    catch (Exception) { }
                }
                aThread = null;
            }
        }
    }


}
