using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class CfgRestoreCommandRequestInfo : ICloneableEx<CfgRestoreCommandRequestInfo>
    {
        // 2013-01-11 - shinyn - Cfg Command 요청

        private string m_IPAddress = string.Empty;

        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }

        private int m_ModelID;

        public int ModelID
        {
            get { return m_ModelID; }
            set { m_ModelID = value; }
        }

        private E_CommandPart m_CommandPart;

        public E_CommandPart CommandPart
        {
            get { return m_CommandPart; }
            set { m_CommandPart = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public CfgRestoreCommandRequestInfo CompactClone()
        {
            CfgRestoreCommandRequestInfo tRequestInfo = new CfgRestoreCommandRequestInfo();
            CopyTo(this, tRequestInfo, false);
            return tRequestInfo;
        }

        public CfgRestoreCommandRequestInfo DeepClone()
        {
            CfgRestoreCommandRequestInfo tRequestInfo = new CfgRestoreCommandRequestInfo();
            CopyTo(this, tRequestInfo, false);
            return tRequestInfo;
        }

        private void CopyTo(CfgRestoreCommandRequestInfo aSource, CfgRestoreCommandRequestInfo aDest, bool aIsCompactClone)
        {
            aDest.CommandPart = aSource.CommandPart;
            aDest.IPAddress = aSource.IPAddress;
            aDest.ModelID = aSource.ModelID;
        }
    }

    [Serializable]
    public class CfgRestoreCommandRequestInfoCollection : GenericListMarshalByRef<CfgRestoreCommandRequestInfo>, ICloneableEx<CfgRestoreCommandRequestInfoCollection>
    {
        public CfgRestoreCommandRequestInfoCollection()
        {
        }

        public CfgRestoreCommandRequestInfoCollection(CfgRestoreCommandRequestInfoCollection aRequestInfoCollection)
        {
            CopyTo(aRequestInfoCollection, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public CfgRestoreCommandRequestInfoCollection CompactClone()
        {
            CfgRestoreCommandRequestInfoCollection tCollection = new CfgRestoreCommandRequestInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }


        public CfgRestoreCommandRequestInfoCollection DeepClone()
        {
            CfgRestoreCommandRequestInfoCollection tCollection = new CfgRestoreCommandRequestInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }

        private void CopyTo(CfgRestoreCommandRequestInfoCollection aSource, CfgRestoreCommandRequestInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (CfgRestoreCommandRequestInfo tRequestInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((CfgRestoreCommandRequestInfo)tRequestInfo.CompactClone());
                else
                    aDest.Add((CfgRestoreCommandRequestInfo)tRequestInfo.DeepClone());
            }
        }


    }
}
