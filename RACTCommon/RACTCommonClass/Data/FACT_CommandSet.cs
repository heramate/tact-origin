using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class FACT_DefaultConnectionCommandSet 
    {

        /// <summary>
        /// 명령어 목록 입니다.
        /// </summary>
        private List<FACT_DefaultConnectionCommand> m_CommandList = new List<FACT_DefaultConnectionCommand>();
        /// <summary>
        /// 명령어 목록 속성을 가져오거나 설정합니다.
        /// </summary>
        public List<FACT_DefaultConnectionCommand> CommandList
        {
            get { return m_CommandList; }
            set { m_CommandList = value; }
        }
    }

    [Serializable]
    public class FACT_DefaultConnectionCommand
    {

        /// <summary>
        /// cmd seq 입니다.
        /// </summary>
        private int m_CMDSeq;

        /// <summary>
        /// CMD 입니다.
        /// </summary>
        private string m_CMD;

        /// <summary>
        /// Prompt 입니다.
        /// </summary>
        private string m_Prompt;

        /// <summary>
        /// Error String 입니다.
        /// </summary>
        private string m_ErrorString;
                
        //2015-09-18 hanjiyeon 추가 - TL1 접속용 명령어 
        /// <summary>
        /// TL1 CMD 입니다.
        /// </summary>
        private string m_TL1_CMD;

        //2015-09-18 hanjiyeon 추가 - TL1 접속용 프롬프트
        /// <summary>
        /// TL1 Prompt 입니다.
        /// </summary>
        private string m_TL1_Prompt;               


        /// <summary>
        /// Error String 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ErrorString
        {
            get { return m_ErrorString; }
            set { m_ErrorString = value; }
        }	


        /// <summary>
        /// Prompt 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Prompt
        {
            get { return m_Prompt; }
            set { m_Prompt = value; }
        }	

        /// <summary>
        /// CMD 속성을 가져오거나 설정합니다.
        /// </summary>
        public string CMD
        {
            get { return m_CMD; }
            set { m_CMD = value; }
        }	

        /// <summary>
        /// cmd seq 속성을 가져오거나 설정합니다.
        /// </summary>
        public int CMDSeq
        {
            get { return m_CMDSeq; }
            set { m_CMDSeq = value; }
        }

        /// <summary>
        /// TL1 Prompt 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TL1_Prompt
        {
            get { return m_TL1_Prompt; }
            set { m_TL1_Prompt = value; }
        }

        /// <summary>
        /// TL1 CMD 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TL1_CMD
        {
            get { return m_TL1_CMD; }
            set { m_TL1_CMD = value; }
        }	
    }
}
