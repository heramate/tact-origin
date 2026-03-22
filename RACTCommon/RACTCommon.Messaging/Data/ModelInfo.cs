using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;
using MessagePack;

namespace RACTCommonClass
{
    #region ModelInfo 클래스입니다.
    /// <summary>
    ///  모델 정보 클래스입니다.
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class ModelInfo : ICloneableEx<ModelInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        public ModelInfo() { }
        public ModelInfo(ModelInfo a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public ModelInfo CompactClone() { var t = new ModelInfo(); CopyTo(this, t, true); return t; }
        public ModelInfo DeepClone() { var t = new ModelInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(ModelInfo aSource, ModelInfo aDest, bool aIsCompactClone)
        {
            aDest.ModelID = aSource.ModelID;
            aDest.ModelName = aSource.ModelName;
            aDest.PortCount = aSource.PortCount;
            aDest.ModelTypeCode = aSource.ModelTypeCode;
            aDest.ModelTypeName = aSource.ModelTypeName;
            aDest.ViewOrder = aSource.ViewOrder;
            aDest.MoreString = aSource.MoreString;
            aDest.MoreMark = aSource.MoreMark;
            aDest.SlotCount = aSource.SlotCount;
            aDest.Divergence = aSource.Divergence;
            aDest.IpTypeCd = aSource.IpTypeCd;
            aDest.CfgRestoreCommands = aSource.CfgRestoreCommands;
            aDest.DefaultConnectionCommadSet = aSource.DefaultConnectionCommadSet;
            aDest.EmbagoCmd = (ArrayList)aSource.EmbagoCmd.Clone();
        }
        #endregion

        [Key(0)] public int ModelID { get; set; } = 0;
        [Key(1)] public string ModelName { get; set; } = string.Empty;
        [Key(2)] public int PortCount { get; set; } = 0;
        [Key(3)] public int ModelTypeCode { get; set; } = 0;
        [Key(4)] public string ModelTypeName { get; set; } = string.Empty;
        [Key(5)] public int ViewOrder { get; set; } = 0;
        [Key(6)] public string MoreString { get; set; } = string.Empty;
        [Key(7)] public string MoreMark { get; set; } = string.Empty;
        [Key(8)] public ArrayList EmbagoCmd { get; set; } = new ArrayList();
        [Key(9)] public int SlotCount { get; set; } = 0;
        [Key(10)] public int Divergence { get; set; } = 0;
        [Key(11)] public int IpTypeCd { get; set; } = 1;
        [Key(12)] public CfgRestoreCommandCollection CfgRestoreCommands { get; set; } = new CfgRestoreCommandCollection();
        [Key(13)] public FACT_DefaultConnectionCommandSet DefaultConnectionCommadSet { get; set; } = new FACT_DefaultConnectionCommandSet();
    }
    #endregion

    #region ModelInfoCollection 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class ModelInfoCollection : GenericListMarshalByRef<ModelInfo>, ICloneableEx<ModelInfoCollection>
    {
        public ModelInfoCollection() { }
        public ModelInfoCollection(ModelInfoCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public ModelInfoCollection CompactClone() { var t = new ModelInfoCollection(); CopyTo(this, t, true); return t; }
        public ModelInfoCollection DeepClone() { var t = new ModelInfoCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(ModelInfoCollection aSource, ModelInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (ModelInfo m in aSource) { if (aIsCompactClone) aDest.Add(m.CompactClone()); else aDest.Add(m.DeepClone()); }
        }

        [IgnoreMember] public override ModelInfo this[int id]
        {
            get { lock (base.InnerList.SyncRoot) { foreach (ModelInfo m in base.InnerList) if (m.ModelID == id) return m; } return null; }
            set { lock (base.InnerList.SyncRoot) { for (int i = 0; i < base.InnerList.Count; i++) if (((ModelInfo)base.InnerList[i]).ModelID == id) { base.InnerList[i] = value; break; } } }
        }

        public void Remove(int id) { lock (base.InnerList.SyncRoot) { foreach (ModelInfo m in base.InnerList) if (m.ModelID == id) { base.Remove(m); break; } } }
        public bool Contains(int id) => this[id] != null;
    }
    #endregion
}
