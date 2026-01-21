using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace RACTClient
{
    public class TreeNodeEx : TreeNode
    {
        #region [생성자]
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public TreeNodeEx() : base() { }
        /// <summary>
        /// 지정된 레이블 텍스트를 사용해 트리노드를 구성합니다.
        /// </summary>
        /// <param name="aText">노드이름</param>
        public TreeNodeEx(string aText) : base(aText) { }

        /// <summary>
        /// 지정된 레이블 텍스트와 자식 트리 노드를 사용하여 System.Windows.Forms.TreeNode 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="aText">새 트리 노드의 레이블 System.Windows.Forms.TreeNode.Text입니다.</param>
        /// <param name="aChildren">자식 System.Windows.Forms.TreeNode 개체로 이루어진 배열입니다.</param>
        public TreeNodeEx(string aText, TreeNodeEx[] aChildren) : base(aText, aChildren) { }

        /// <summary>
        /// 트리 노드가 선택 상태 또는 선택하지 않은 상태에 있을 때 표시할 지정된 레이블 텍스트와 이미지를 사용하여 System.Windows.Forms.TreeNode
        /// 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="aText">새 트리 노드의 레이블 System.Windows.Forms.TreeNode.Text입니다.</param>
        /// <param name="imageIndex">트리 노드를 선택하지 않은 상태일 때 표시할 System.Drawing.Image의 인덱스 값입니다.</param>
        /// <param name="selectedImageIndex">트리 노드를 선택한 상태일 때 표시할 System.Drawing.Image의 인덱스 값입니다.</param>
        public TreeNodeEx(string aText, int imageIndex, int selectedImageIndex) : base(aText, imageIndex, selectedImageIndex) { }
        /// <summary>
        /// 트리 노드가 선택 상태 또는 선택하지 않은 상태에 있을 때 표시할 지정된 레이블 텍스트, 자식 트리 노드 및 이미지를 사용하여 System.Windows.Forms.TreeNode
        /// 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="text">새 트리 노드의 레이블 System.Windows.Forms.TreeNode.Text입니다.</param>
        /// <param name="imageIndex">트리 노드를 선택하지 않은 상태일 때 표시할 System.Drawing.Image의 인덱스 값입니다.</param>
        /// <param name="selectedImageIndex">트리 노드를 선택한 상태일 때 표시할 System.Drawing.Image의 인덱스 값입니다.</param>
        /// <param name="children">자식 System.Windows.Forms.TreeNode 개체로 이루어진 배열입니다.</param>
        public TreeNodeEx(string aText, int imageIndex, int selectedImageIndex, TreeNode[] children) : base(aText, imageIndex, selectedImageIndex, children) { }
        #endregion

        /// <summary>
        /// Variable  속성을 가져오거나 설정합니다.
        /// </summary>
        [Browsable(false)]
        public new bool Checked
        {
            get { return (m_CheckState == CheckState.Checked); }
            set
            {
                SetCheckStateChange(value ? CheckState.Checked : CheckState.Unchecked);
            }
        }

        internal NodeLineType NodeLineType
        {
            get
            {
                // is this node bound to a treeview ?
                if (null != this.TreeView)
                {
                    // do we need to draw lines at all?
                    if (!this.TreeView.ShowLines) { return NodeLineType.None; }
                    if (this.CheckboxVisible) { return NodeLineType.None; }

                    if (this.Nodes.Count > 0)
                    {
                        return NodeLineType.WithChildren;
                    }
                    return NodeLineType.Straight;
                }

                // no treeview so this node will never been drawn at all
                return NodeLineType.None;
            }
        }

        private CheckState m_CheckState;
        /// <summary>
        /// CheckState  속성을 가져오거나 설정합니다.
        /// </summary>
        public CheckState CheckState
        {
            get { return m_CheckState; }

        }
        private bool m_CheckBoxVisible;
        /// <summary>
        /// CheckBoxVisible  속성을 가져오거나 설정합니다.
        /// </summary>
        public bool CheckboxVisible
        {
            get { return m_CheckBoxVisible; }
            set { m_CheckBoxVisible = value; }
        }

        internal void SetCheckStateChange(CheckState aState)
        {
            if (aState != m_CheckState)
            {
                m_CheckState = aState;

                // 자식 노드의 상태를 변경한다.
                if (this.Nodes != null && this.Nodes.Count > 0)
                {
                    foreach (TreeNodeEx tExNode in this.Nodes)
                    {
                        if (tExNode != null) tExNode.ChangeChildState(aState);
                    }
                }

                // 부모 노드의 상태를 변경한다.
                if (this.Parent != null)
                {
                    TreeNodeEx tparentNode = this.Parent as TreeNodeEx;
                    if (tparentNode != null)
                    {
                        tparentNode.ChildCheckStateChanged(aState);
                    }
                }
            }
        }
        private void ChildCheckStateChanged(CheckState aState)
        {
            bool notifyParent = false;
            CheckState tNewState = CheckState.Unchecked;

            switch (aState)
            {
                case CheckState.Indeterminate:
                    {
                        tNewState = CheckState.Indeterminate;

                        notifyParent = (tNewState != m_CheckState);
                    }
                    break;
                case CheckState.Checked:
                    tNewState = CheckState.Checked;

                    if (this.Nodes.Count > 1)
                    {
                        foreach (TreeNodeEx tNodeEx in this.Nodes)
                        {
                            if (tNodeEx != null && tNodeEx.CheckState != CheckState.Checked)
                            {
                                tNewState = CheckState.Indeterminate;
                                break;
                            }
                        }
                    }
                    notifyParent = (tNewState != m_CheckState);
                    break;
                case CheckState.Unchecked:

                    tNewState = CheckState.Unchecked;

                    if (this.Nodes.Count > 1)
                    {
                        foreach (TreeNodeEx tNodeEx in this.Nodes)
                        {
                            if (tNodeEx != null && tNodeEx.CheckState != CheckState.Unchecked)
                            {
                                tNewState = CheckState.Indeterminate;
                                break;
                            }
                        }
                    }
                    notifyParent = (tNewState != m_CheckState);
                    break;
            }

            // should we notify the parent? ( has our state changed? )
            if (notifyParent)
            {
                this.m_CheckState = tNewState;

                // notify parent
                if (this.Parent != null)
                {
                    TreeNodeEx parentNode = this.Parent as TreeNodeEx;
                    if (parentNode != null)
                    {
                        parentNode.ChildCheckStateChanged(this.m_CheckState);
                    }
                }
            }
        }

        private void ChangeChildState(CheckState aState)
        {
            // 자식 노드의 상태를 변경한다.
            m_CheckState = aState;

            if (this.Nodes != null && this.Nodes.Count > 0)
            {
                foreach (TreeNodeEx tExNode in this.Nodes)
                {
                    if (tExNode != null)
                        tExNode.ChangeChildState(aState);
                }
            }
        }



    }
}
