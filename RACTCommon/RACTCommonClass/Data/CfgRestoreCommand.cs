using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class CfgRestoreCommand : ICloneableEx<CfgRestoreCommand>
    {
        // 2013-01-11 - shinyn - 장비 복원명령 커멘드 입니다.
        public CfgRestoreCommand()
        {
        }

        public CfgRestoreCommand(CfgRestoreCommand aCfgRestoreCommand)
        {
            CopyTo(aCfgRestoreCommand, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public CfgRestoreCommand CompactClone()
        {
            CfgRestoreCommand tCfgRestoreCommand = new CfgRestoreCommand();
            CopyTo(this, tCfgRestoreCommand, true);
            return tCfgRestoreCommand;
        }

        public CfgRestoreCommand DeepClone()
        {
            CfgRestoreCommand tCfgRestoreCommand = new CfgRestoreCommand();
            CopyTo(this, tCfgRestoreCommand, false);
            return tCfgRestoreCommand;
        }

        private void CopyTo(CfgRestoreCommand aSource, CfgRestoreCommand aDest, bool aIsCompactClone)
        {
            aDest.Cmd = aSource.Cmd;
            aDest.CmdSeq = aSource.CmdSeq;
            aDest.T_Prompt = aSource.T_Prompt;
        }

        private int m_CmdSeq = 0;

        public int CmdSeq
        {
            get { return m_CmdSeq; }
            set { m_CmdSeq = value; }
        }

        private string m_Cmd = string.Empty;

        public string Cmd
        {
            get { return m_Cmd; }
            set { m_Cmd = value; }
        }

        private string m_T_Prompt = string.Empty;

        public string T_Prompt
        {
            get { return m_T_Prompt; }
            set { m_T_Prompt = value; }
        }
    }

    [Serializable]
    public class CfgRestoreCommandCollection : GenericListMarshalByRef<CfgRestoreCommand>, ICloneableEx<CfgRestoreCommandCollection>
    {
        public CfgRestoreCommandCollection()
        {
        }

        public CfgRestoreCommandCollection(CfgRestoreCommandCollection aCfgResotreCommandCollection)
        {
            CopyTo(aCfgResotreCommandCollection, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public CfgRestoreCommandCollection CompactClone()
        {
            CfgRestoreCommandCollection tCollection = new CfgRestoreCommandCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }

        public CfgRestoreCommandCollection DeepClone()
        {
            CfgRestoreCommandCollection tCollection = new CfgRestoreCommandCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }


        private void CopyTo(CfgRestoreCommandCollection aSource, CfgRestoreCommandCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (CfgRestoreCommand tCfgRestoreCommand in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((CfgRestoreCommand)tCfgRestoreCommand.CompactClone());
                else
                    aDest.Add((CfgRestoreCommand)tCfgRestoreCommand.DeepClone());
            }
        }


    }
}
