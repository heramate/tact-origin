using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKData;
using System.Windows.Forms;
using System.Collections;

namespace RACTCommonClass
{
    public abstract class ReservedStringBase
    {
        /// <summary>
        /// 이름 입니다.
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// XML 파일 이름 입니다.
        /// </summary>
        protected string m_XMLFileName;
        /// <summary>
        /// 예약어 입니다.
        /// </summary>
        protected string m_ReservedString;


        /// <summary>
        /// 설명 입니다.
        /// </summary>
        protected string m_Description;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        protected ReservedStringBase() { }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aName">이름 입니다.</param>
        protected ReservedStringBase(string aXmlName)
        {
            m_XMLFileName = aXmlName;
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aName">이름 입니다.</param>
        /// <param name="aReservedString">예약어 입니다.</param>
        protected ReservedStringBase(string aName, string aReservedString) 
        {
            m_Name = aName;
            m_ReservedString = aReservedString;
        }

        /// <summary>
        /// ReservedStringBase를 Parse한다.
        /// </summary>
        /// <param name="field">ReservedStringBase 필드 입니다.</param>
        public bool LoadXML(string aPath)
        {
            ArrayList tReservedInfos = null;
            E_XmlError tXmlError = E_XmlError.Success;
            try
            {
                tReservedInfos = MKXML.ObjectFromXML(aPath +m_XMLFileName , typeof(ReservedStringBase), out tXmlError);
                if (tReservedInfos == null) return false;
                if (tReservedInfos.Count == 0) return false;

                ReservedStringBase tReservedStringBase = (ReservedStringBase)tReservedInfos[0];
                
                m_Name = tReservedStringBase.Name;
                m_Description = tReservedStringBase.Description;
                m_ReservedString = tReservedStringBase.ReservedString; 

                return true; ;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 명령을 처리 합니다.
        /// </summary>
        public abstract string OnCommandProcess(string aSourceString, DeviceInfo aDeviceInfo);

        /// <summary>
        /// 예약어 가져오거나 설정 합니다.
        /// </summary>
        public string ReservedString
        {
            get { return m_ReservedString; }
            set { m_ReservedString = value; }
        }
        /// <summary>
        /// 이름 가져오거나 설정 합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// 설명 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        /// <summary>
        /// XML 파일 이름 가져오거나 설정 합니다.
        /// </summary>
        public string XMLFileName
        {
            get { return m_XMLFileName; }
            set { m_XMLFileName = value; }
        }

        public  ReservedStringBase CompactClone()
        {
            return null;
        }

        public ReservedStringBase DeepClone()
        {
            return null;
        }
      
    }
}
