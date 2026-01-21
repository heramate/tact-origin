using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class DeviceCfgSaveInfo : ICloneableEx<DeviceCfgSaveInfo>
    {
        
        private string m_IPAdress = string.Empty;

        public string IPAddress
        {
            get { return m_IPAdress; }
            set { m_IPAdress = value; }
        }

        private CfgSaveInfoCollection m_CfgSaveInfoCollection = new CfgSaveInfoCollection();

        public CfgSaveInfoCollection CfgSaveInfoCollection
        {
            get { return m_CfgSaveInfoCollection; }
            set { m_CfgSaveInfoCollection = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public DeviceCfgSaveInfo CompactClone()
        {
            DeviceCfgSaveInfo tInfo = new DeviceCfgSaveInfo();
            CopyTo(this, tInfo, true);
            return tInfo;
        }


        public DeviceCfgSaveInfo DeepClone()
        {
            DeviceCfgSaveInfo tInfo = new DeviceCfgSaveInfo();
            CopyTo(this, tInfo, false);
            return tInfo;
        }

        private void CopyTo(DeviceCfgSaveInfo aSource, DeviceCfgSaveInfo aDest, bool aIsCompactClone)
        {
            aDest.IPAddress = aSource.IPAddress;
            aDest.CfgSaveInfoCollection = aSource.CfgSaveInfoCollection;
        }
    }

    [Serializable]
    public class DeviceCfgSaveInfoCollection : GenericListMarshalByRef<DeviceCfgSaveInfo>, ICloneableEx<DeviceCfgSaveInfoCollection>
    {
        public DeviceCfgSaveInfoCollection()
        {
        }

        public DeviceCfgSaveInfoCollection(DeviceCfgSaveInfoCollection aDeviceCfgSaveInfos)
        {
            CopyTo(aDeviceCfgSaveInfos, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public DeviceCfgSaveInfoCollection CompactClone()
        {
            DeviceCfgSaveInfoCollection tCollection = new DeviceCfgSaveInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }

        public DeviceCfgSaveInfoCollection DeepClone()
        {
            DeviceCfgSaveInfoCollection tCollection = new DeviceCfgSaveInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }


        private void CopyTo(DeviceCfgSaveInfoCollection aSource, DeviceCfgSaveInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (DeviceCfgSaveInfo tDeviceCfgSaveInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((DeviceCfgSaveInfo)tDeviceCfgSaveInfo.CompactClone());
                else
                    aDest.Add((DeviceCfgSaveInfo)tDeviceCfgSaveInfo.DeepClone());
            }
        }
    }

}
