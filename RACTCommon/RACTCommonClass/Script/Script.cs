using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class Script : ICloneableEx<Script>
    {
        public static readonly string s_Send = "TACT.Send";
        public static readonly string s_WaitForString = "TACT.WaitForString";

        /// <summary>
        /// Script Name 입니다.
        /// </summary>
        private string m_Name;

        /// <summary>
        /// ID 입니다.
        /// </summary>
        private int m_ID;

        /// <summary>
        /// 그룹 ID 입니다.
        /// </summary>
        private int m_GroupID;


        /// <summary>
        /// 스크립트 타입 입니다.
        /// </summary>
        private E_ScriptType m_ScriptType = E_ScriptType.SendScript;
        /// <summary>
        /// 스크립트 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_ScriptType ScriptType
        {
            get { return m_ScriptType; }
            set { m_ScriptType = value; }
        }	


        /// <summary>
        /// 그룹 ID 가져오거나 설정 합니다.
        /// </summary>
        public int GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }

        /// <summary>
        /// Description 입니다.
        /// </summary>
        private string m_Description;

        /// <summary>
        /// 스크립트 입니다.
        /// </summary>
        private string m_RawScript ="";

        /// <summary>
        /// 스크립트 가져오거나 설정 합니다.
        /// </summary>
        public string RawScript
        {
            get { return m_RawScript; }
            set { m_RawScript = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public Script(){}
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aScriptText"></param>
        public Script(string aScriptText)
        {
            m_RawScript = aScriptText;
        }

         /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        public Script(Script aGroupInfo)
        {
            CopyTo(aGroupInfo, this, false);
        }

        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public Script CompactClone()
        {
            Script tShortenCommandInfo = new Script();
            CopyTo(this, tShortenCommandInfo, true);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public Script DeepClone()
        {
            Script tShortenCommandInfo = new Script();
            CopyTo(this, tShortenCommandInfo, false);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(Script aSource, Script aDest, bool aIsCompactClone)
        {
            aDest.ID = aSource.ID;
            aDest.Description = aSource.Description;
            aDest.Name = aSource.Name;
            aDest.RawScript = aSource.RawScript;
            aDest.GroupID = aSource.GroupID;
        }

        /// <summary>
        /// Description 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }


        /// <summary>
        /// ID 가져오거나 설정 합니다.
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        /// <summary>
        /// Script Name 가져오거나 설정 합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        
    }

    public enum E_ScriptType
    {
        SendScript,
        WaitScript
    }
}
