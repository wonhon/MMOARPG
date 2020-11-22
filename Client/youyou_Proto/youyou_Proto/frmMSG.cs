using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReadExcel
{
    public partial class frmMSG : Form
    {
        public frmMSG()
        {
            InitializeComponent();

            LoadXml();

            LoadScriptTemplate();
        }

        private void LoadXml()
        {
            groupProtoInfo.Enabled = false;
            DataMgr.Instance.LoadXml();

            if (DataMgr.Instance.MenuList != null)
            {
                for (int i = 0; i < DataMgr.Instance.MenuList.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.Text = string.Format("{0}", DataMgr.Instance.MenuList[i].MenuName);




                    NodeTag tag = new NodeTag();
                    tag.IsMenu = true;
                    tag.Menu = DataMgr.Instance.MenuList[i];

                    //========================================================

                    {
                        if (tag.Menu.HasChild)
                        {
                            List<MyProto> lstProto = tag.Menu.ProtoList;

                            if (lstProto != null && lstProto.Count > 0)
                            {
                                for (int j = 0; j < lstProto.Count; j++)
                                {
                                    MyProto proto = lstProto[j];

                                    NodeTag protoTag = new NodeTag();
                                    protoTag.IsMenu = false;
                                    protoTag.Proto = proto;

                                    TreeNode nodeProto = new TreeNode();
                                    nodeProto.Text = string.Format("({0}){1}", proto.ProtoCode, proto.ProtoCnName);
                                    nodeProto.Tag = protoTag;
                                    node.Nodes.Add(nodeProto);
                                }
                            }
                        }
                    }

                    //========================================================

                    node.Tag = tag;
                    myTree.Nodes.Add(node);


                    myTree.Refresh();
                }
            }
        }

        private void LoadScriptTemplate()
        {
            DataMgr.Instance.LoadScriptTemplate();
        }

        private TreeNode m_CurrentSelectNode = null;

        #region 菜单操作
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            frmModelEdit frmModelEdit = new frmModelEdit();
            if (frmModelEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //DATA
                MyMenu menu = new MyMenu();

                menu.MenuId = DateTime.Now.Ticks;
                menu.MenuName = frmModelEdit.ModelName;
                DataMgr.Instance.AddMenu(menu);
                DataMgr.Instance.SaveXml();


                //UI
                NodeTag _tag = new NodeTag();
                _tag.IsMenu = true;
                _tag.Menu = menu;

                TreeNode node = new TreeNode();
                node.Text = string.Format("{0}", menu.MenuName);
                node.Tag = _tag;
                myTree.Nodes.Add(node);
                myTree.Refresh();
                myTree.Focus();
                myTree.SelectedNode = node;
            }
        }

        //双击
        private void myTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_CurrentSelectNode = myTree.SelectedNode;
            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;

            if (tag == null) return;
            if (tag.IsMenu)
            {
                frmModelEdit frmModelEdit = new frmModelEdit();

                frmModelEdit.ModelName = tag.Menu.MenuName;

                if (frmModelEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tag.Menu.MenuName = frmModelEdit.ModelName;

                    //DATA
                    DataMgr.Instance.UpdateMenu(tag.Menu);
                    DataMgr.Instance.SaveXml();

                    //UI
                    m_CurrentSelectNode.Text = string.Format("{0}", tag.Menu.MenuName);
                    myTree.Refresh();
                    myTree.Focus();
                    myTree.SelectedNode = m_CurrentSelectNode;
                }
            }
        }

        private void btnDelMenu_Click(object sender, EventArgs e)
        {
            m_CurrentSelectNode = myTree.SelectedNode;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;
            if (tag == null) return;

            if (tag.IsMenu)
            {
                if (MessageBox.Show("您确定要删除此模块吗？谨慎操作", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    //DATA
                    DataMgr.Instance.DeleteMenu(tag.Menu);
                    DataMgr.Instance.SaveXml();

                    //UI
                    myTree.Nodes.Remove(m_CurrentSelectNode);
                    myTree.Refresh();
                }
            }
            else
            {
                MessageBox.Show("这个按钮只能删除模块");
            }
        }
        #endregion

        //================================协议操作=====================================

        #region btnAddProto_Click 添加协议
        /// <summary>
        /// 添加协议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProto_Click(object sender, EventArgs e)
        {
            m_CurrentSelectNode = myTree.SelectedNode;

            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;
            if (tag == null) return;

            if (!tag.IsMenu)
            {
                MessageBox.Show("您需要在某个模块下添加协议");
                return;
            }

            //DATA
            MyProto proto = new MyProto();
            proto.ProtoID = DateTime.Now.Ticks;
            proto.ProtoCode = "-1";
            proto.ProtoCnName = "新建协议";
            proto.ProtoEnName = "";
            proto.ProtoDesc = "";
            proto.MenuID = tag.Menu.MenuId;

            DataMgr.Instance.AddProto(proto, tag.Menu.MenuId);
            DataMgr.Instance.SaveXml();


            //UI
            NodeTag _tag = new NodeTag();
            _tag.IsMenu = false;
            _tag.Proto = proto;

            TreeNode node = new TreeNode();
            node.Text = string.Format("({0}){1}", proto.ProtoCode, proto.ProtoCnName);
            node.Tag = _tag;
            m_CurrentSelectNode.Nodes.Add(node);

            myTree.Refresh();
            myTree.Focus();
            myTree.SelectedNode = node;

            m_CurrentSelectNode.Expand();

            m_CurrentSelectNode = node;
            ShowProtoInfo(proto);
        }
        #endregion

        private MyProto m_CurrentProto;

        private void ShowProtoInfo(MyProto proto)
        {
            if (proto == null) return;

            m_CurrentProto = proto;

            groupProtoInfo.Enabled = true;

            this.txtProtoCode.Text = proto.ProtoCode;
            this.txtProtoEnName.Text = proto.ProtoEnName;
            this.txtProtoCnName.Text = proto.ProtoCnName;
            this.txtProtoDesc.Text = proto.ProtoDesc;

            this.protoCategory.SelectedValue = proto.ProtoCategory == null ? "" : proto.ProtoCategory;
            this.checkBoxCSharp.Checked = proto.IsCSharp;
            this.checkBoxLua.Checked = proto.IsLua;

            //====================================

            List<MyProtoAttr> lst = DataMgr.Instance.GetProtoArrtList(proto.ProtoID, proto.MenuID);

            dvGrid.Rows.Clear();

            for (int i = 0; i < lst.Count; i++)
            {
                DataGridViewRow dataGridViewRow = dvGrid.Rows[0].Clone() as DataGridViewRow;


                dataGridViewRow.Tag = lst[i];

                dataGridViewRow.Cells[0].Value = lst[i].AttType;
                dataGridViewRow.Cells[1].Value = lst[i].AttEnName;
                dataGridViewRow.Cells[2].Value = lst[i].AttCnName;
                dataGridViewRow.Cells[3].Value = lst[i].AttIsLoop;
                dataGridViewRow.Cells[4].Value = lst[i].AttToLoop;
                dataGridViewRow.Cells[5].Value = lst[i].AttToBool;
                dataGridViewRow.Cells[6].Value = lst[i].AttToBoolResult;
                dataGridViewRow.Cells[7].Value = lst[i].AttToCus;
                dataGridViewRow.Cells[8].Value = lst[i].AttIsExistsProto;

                dvGrid.Rows.Add(dataGridViewRow);
            }
        }

        private void btnSaveProto_Click(object sender, EventArgs e)
        {
            SaveProto();
        }

        private void SaveProto()
        {
            if (m_CurrentSelectNode == null) return;

            if (this.txtProtoCode.Text.Trim() == "-1" || string.IsNullOrEmpty(this.txtProtoCode.Text.Trim()))
            {
                MessageBox.Show("请输入协议编码 只能是数字");
                this.txtProtoCode.Focus();
                return;
            }

            int protoCode = 0;
            int.TryParse(this.txtProtoCode.Text, out protoCode);

            if (protoCode < 1)
            {
                MessageBox.Show("请输入协议编码 只能是数字");
                this.txtProtoCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtProtoEnName.Text.Trim()))
            {
                MessageBox.Show("请输入协议英文名称");
                this.txtProtoEnName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtProtoCnName.Text.Trim()))
            {
                MessageBox.Show("请输入协议中文名称");
                this.txtProtoCnName.Focus();
                return;
            }

            m_CurrentProto.ProtoCode = protoCode.ToString();
            m_CurrentProto.ProtoEnName = this.txtProtoEnName.Text.Trim();
            m_CurrentProto.ProtoCnName = this.txtProtoCnName.Text.Trim();
            m_CurrentProto.ProtoDesc = this.txtProtoDesc.Text.Trim();
            m_CurrentProto.ProtoCategory = this.protoCategory.SelectedValue.ToString();
            m_CurrentProto.IsCSharp = checkBoxCSharp.Checked;
            m_CurrentProto.IsLua = checkBoxLua.Checked;

            DataMgr.Instance.UpdateProto(m_CurrentProto);


            //UI
            m_CurrentSelectNode.Text = string.Format("({0}){1}", m_CurrentProto.ProtoCode, m_CurrentProto.ProtoCnName);

            myTree.Refresh();
            myTree.Focus();
            myTree.SelectedNode = m_CurrentSelectNode;

            //========================保存数据表===========================

            List<MyProtoAttr> lst = new List<MyProtoAttr>();

            int id = 1;
            for (int i = 0; i < dvGrid.Rows.Count; i++)
            {
                id++;
                lst.Add(new MyProtoAttr()
                {
                    AttID = dvGrid.Rows[i].Tag == null ? DateTime.Now.Ticks + id : ((MyProtoAttr)dvGrid.Rows[i].Tag).AttID,
                    AttType = dvGrid.Rows[i].Cells[0].Value == null ? "" : dvGrid.Rows[i].Cells[0].Value.ToString(),
                    AttEnName = dvGrid.Rows[i].Cells[1].Value == null ? "" : dvGrid.Rows[i].Cells[1].Value.ToString(),
                    AttCnName = dvGrid.Rows[i].Cells[2].Value == null ? "" : dvGrid.Rows[i].Cells[2].Value.ToString(),
                    AttIsLoop = dvGrid.Rows[i].Cells[3].Value == null ? false : (bool)dvGrid.Rows[i].Cells[3].Value,
                    AttToLoop = dvGrid.Rows[i].Cells[4].Value == null ? "" : dvGrid.Rows[i].Cells[4].Value.ToString(),
                    AttToBool = dvGrid.Rows[i].Cells[5].Value == null ? "" : dvGrid.Rows[i].Cells[5].Value.ToString(),
                    AttToBoolResult = dvGrid.Rows[i].Cells[6].Value == null ? false : (bool)dvGrid.Rows[i].Cells[6].Value,
                    AttToCus = dvGrid.Rows[i].Cells[7].Value == null ? "" : dvGrid.Rows[i].Cells[7].Value.ToString(),
                    AttIsExistsProto = dvGrid.Rows[i].Cells[8].Value == null ? false : (bool)dvGrid.Rows[i].Cells[8].Value
                });
            }

            //把最后一个移除
            lst.RemoveAt(lst.Count - 1);

            DataMgr.Instance.ProtoDataSave(lst, m_CurrentProto.ProtoID, m_CurrentProto.MenuID);

            DataMgr.Instance.SaveXml();
        }

        private void btnDelProto_Click(object sender, EventArgs e)
        {
            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;
            if (tag == null) return;

            if (!tag.IsMenu)
            {
                if (MessageBox.Show("您确定要删除此模块吗？谨慎操作", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    //DATA
                    DataMgr.Instance.DeleteProto(tag.Proto);
                    DataMgr.Instance.SaveXml();

                    //UI
                    m_CurrentSelectNode.Parent.Nodes.Remove(m_CurrentSelectNode);
                    myTree.Refresh();
                    m_CurrentSelectNode = null;
                }
            }
            else
            {
                MessageBox.Show("这个按钮只能删除协议");
            }
        }

        private void myTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_CurrentSelectNode = e.Node;

            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;

            if (tag == null) return;
            if (!tag.IsMenu)
            {
                ShowProtoInfo(tag.Proto);
            }
            else
            {
                groupProtoInfo.Enabled = false;
            }
        }

        //========================上下移动节点=============================================

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;

            if (tag == null) return;
            if (tag.IsMenu)
            {
                if (DataMgr.Instance.MoveMenu(tag.Menu, true))
                {
                    DataMgr.Instance.SaveXml();

                    int oldIndex = m_CurrentSelectNode.Index;
                    myTree.Nodes.RemoveAt(oldIndex);
                    myTree.Nodes.Insert(oldIndex - 1, m_CurrentSelectNode);

                    myTree.Refresh();
                    myTree.Focus();
                    myTree.SelectedNode = m_CurrentSelectNode;
                }
            }
            else
            {
                if (DataMgr.Instance.MoveProto(tag.Proto, true))
                {
                    DataMgr.Instance.SaveXml();

                    int oldIndex = m_CurrentSelectNode.Index;

                    TreeNode parent = m_CurrentSelectNode.Parent;

                    parent.Nodes.RemoveAt(oldIndex);
                    parent.Nodes.Insert(oldIndex - 1, m_CurrentSelectNode);

                    myTree.Refresh();
                    myTree.Focus();
                    myTree.SelectedNode = m_CurrentSelectNode;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (m_CurrentSelectNode == null) return;

            NodeTag tag = m_CurrentSelectNode.Tag as NodeTag;

            if (tag == null) return;
            if (tag.IsMenu)
            {
                if (DataMgr.Instance.MoveMenu(tag.Menu, false))
                {
                    DataMgr.Instance.SaveXml();

                    int oldIndex = m_CurrentSelectNode.Index;
                    myTree.Nodes.RemoveAt(oldIndex);
                    myTree.Nodes.Insert(oldIndex + 1, m_CurrentSelectNode);

                    myTree.Refresh();
                    myTree.Focus();
                    myTree.SelectedNode = m_CurrentSelectNode;
                }
            }
            else
            {
                if (DataMgr.Instance.MoveProto(tag.Proto, false))
                {
                    DataMgr.Instance.SaveXml();

                    int oldIndex = m_CurrentSelectNode.Index;

                    TreeNode parent = m_CurrentSelectNode.Parent;
                    parent.Nodes.RemoveAt(oldIndex);
                    parent.Nodes.Insert(oldIndex + 1, m_CurrentSelectNode);

                    myTree.Refresh();
                    myTree.Focus();
                    myTree.SelectedNode = m_CurrentSelectNode;
                }
            }
        }

        private void btnMovePrevAtt_Click(object sender, EventArgs e)
        {
            if (this.dvGrid.SelectedRows.Count <= 0) return;

            //上移 取本行的第一个索引号 把选择的行 加入本索引号-1的地方

            //选择的行
            DataGridViewSelectedRowCollection rows = this.dvGrid.SelectedRows;

            List<DataGridViewRow> lst = new List<DataGridViewRow>();

            int index = -1; //找出最小的索引值

            for (int i = 0; i < rows.Count; i++)
            {
                if (i == 0)
                {
                    index = rows[i].Index;
                }
                else
                {
                    if (rows[i].Index < index)
                    {
                        index = rows[i].Index;
                    }
                }
            }

            //如果已经是0的位置 直接返回
            if (index == 0) return;

            for (int i = 0; i < rows.Count; i++)
            {
                lst.Add(rows[i]);
                dvGrid.Rows.Remove(rows[i]);
            }

            int toIndex = index - 1;

            for (int i = 0; i < lst.Count; i++)
            {
                dvGrid.Rows.Insert(toIndex, lst[i]);
            }

            for (int i = 0; i < dvGrid.Rows.Count; i++)
            {
                dvGrid.Rows[i].Selected = false;
            }

            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].Selected = true;
            }
        }

        private void btnMoveNextAtt_Click(object sender, EventArgs e)
        {
            if (this.dvGrid.SelectedRows.Count <= 0) return;

            //上移 取本行的第一个索引号 把选择的行 加入本索引号-1的地方

            //选择的行
            DataGridViewSelectedRowCollection rows = this.dvGrid.SelectedRows;

            List<DataGridViewRow> lst = new List<DataGridViewRow>();

            int index = -1; //找出最小的索引值
            int maxIndex = -1; //最大索引

            for (int i = 0; i < rows.Count; i++)
            {
                if (i == 0)
                {
                    index = rows[i].Index;
                    maxIndex = index;
                }
                else
                {
                    if (rows[i].Index < index)
                    {
                        index = rows[i].Index;
                    }
                    else if (rows[i].Index > index)
                    {
                        maxIndex = rows[i].Index;
                    }
                }
            }

            //如果已经是0的位置 直接返回
            if (maxIndex == dvGrid.Rows.Count - 2) return;

            for (int i = 0; i < rows.Count; i++)
            {
                lst.Add(rows[i]);
                dvGrid.Rows.Remove(rows[i]);
            }

            int toIndex = index + 1;

            for (int i = 0; i < lst.Count; i++)
            {
                dvGrid.Rows.Insert(toIndex, lst[i]);
            }

            for (int i = 0; i < dvGrid.Rows.Count; i++)
            {
                dvGrid.Rows[i].Selected = false;
            }

            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].Selected = true;
            }
        }

        private void btnDelAtt_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除此字段吗？谨慎操作", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                DataGridViewSelectedRowCollection rows = this.dvGrid.SelectedRows;

                for (int i = 0; i < rows.Count; i++)
                {
                    dvGrid.Rows.Remove(rows[i]);
                }
            }
        }

        private void btnAttpreview_Click(object sender, EventArgs e)
        {
            //预览前需要先保存
            SaveProto();

            List<MyProtoAttr> lst = DataMgr.Instance.GetProtoArrtList(m_CurrentProto.ProtoID, m_CurrentProto.MenuID);

            StringBuilder sbr = new StringBuilder();

            sbr.Append("{\r\n");
            for (int i = 0; i < lst.Count; i++)
            {
                MyProtoAttr attr = lst[i];

                if (attr.AttIsUsed) continue;

                if (attr.AttType == "bool" || attr.AttType == "byte" || attr.AttType == "short" || attr.AttType == "int" || attr.AttType == "long" || attr.AttType == "string" || attr.AttType == "char" || attr.AttType == "float" || attr.AttType == "decimal")
                {
                    if (string.IsNullOrEmpty(attr.AttToCus))
                    {
                        sbr.AppendFormat("      {0} {1} //{2}\r\n", attr.AttType, attr.AttEnName, attr.AttCnName);
                    }
                }

                //如果是布尔变量
                if (attr.AttType == "bool")
                {
                    #region 布尔
                    bool isHasSuccess = false;
                    //查找隶属于这个bool成功的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, attr.AttEnName, true);
                        if (list != null && list.Count > 0)
                        {
                            isHasSuccess = true;
                            sbr.AppendFormat("      if({0})\r\n", attr.AttEnName);
                            sbr.Append("      {\r\n");
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbr.AppendFormat("            {0} {1} //{2}\r\n", list[j].AttType, list[j].AttEnName, list[j].AttCnName);
                            }
                            sbr.Append("      }\r\n");
                        }
                    }

                    //查找隶属于这个bool失败的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, attr.AttEnName, false);
                        if (list != null && lst.Count > 0)
                        {
                            if (isHasSuccess)
                            {
                                sbr.AppendFormat("      else\r\n", attr.AttEnName);
                            }
                            else
                            {
                                //如果没有成功项
                                sbr.AppendFormat("      if(!{0})\r\n", attr.AttEnName);
                            }
                            sbr.Append("      {\r\n");
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbr.AppendFormat("            {0} {1} //{2}\r\n", list[j].AttType, list[j].AttEnName, list[j].AttCnName);
                            }
                            sbr.Append("      }\r\n");
                        }
                    }
                    #endregion
                }
                else if ((attr.AttType == "byte" || attr.AttType == "short" || attr.AttType == "int" || attr.AttType == "long") && attr.AttIsLoop)
                {
                    #region 循环项目

                    sbr.AppendFormat("      for(int i=0;i<{0};i++)\r\n", attr.AttEnName);
                    sbr.Append("      {\r\n");
                    //查找隶属于这个循环项的
                    {
                        List<MyProtoAttr> list = lst.Where(p => p.AttToLoop.Equals(attr.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (list != null && list.Count > 0)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbr.AppendFormat("            {0} {1} //{2}\r\n", list[j].AttType, list[j].AttEnName, list[j].AttCnName);
                            }
                        }
                    }
                    sbr.Append("      }\r\n");

                    #endregion
                }
                else if (attr.AttType != "byte" && attr.AttType != "short" && attr.AttType != "int" && attr.AttType != "long" && attr.AttType != "string" && attr.AttType != "char" && attr.AttType != "float" && attr.AttType != "decimal" && attr.AttType != "bool" && attr.AttType != "ushort")
                {
                    //查找隶属于这个自定义的项
                    List<MyProtoAttr> list = lst.Where(p => p.AttToCus.Equals(attr.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    if (list != null && list.Count > 0)
                    {
                        //如果是自定义
                        if (!attr.AttIsLoop)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbr.AppendFormat("      {0} {1}.{2} //{3}\r\n", list[j].AttType, attr.AttEnName, list[j].AttEnName, list[j].AttCnName);
                            }
                        }
                        else
                        {
                            sbr.AppendFormat("      int {0}Count {1}\r\n", attr.AttEnName, attr.AttCnName);
                            sbr.AppendFormat("      for(int i=0;i<{0}Count;i++)\r\n", attr.AttEnName);
                            sbr.Append("      {\r\n");
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbr.AppendFormat("            {0} {1}.{2} //{3}\r\n", list[j].AttType, attr.AttEnName, list[j].AttEnName, list[j].AttCnName);
                            }
                            sbr.Append("      }\r\n");
                        }
                    }
                }
            }
            sbr.Append("}\r\n");
            this.txtAttpreview.Text = sbr.ToString();
        }

        //根据隶书布尔名称和结果查找项
        private List<MyProtoAttr> GetListByToBoolName(List<MyProtoAttr> lst, string boolName, bool isTrue)
        {
            return lst.Where(p => p.AttToBool.Equals(boolName, StringComparison.CurrentCultureIgnoreCase)).Where(p => p.AttToBoolResult == isTrue).ToList();
        }

        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            CreateProto(m_CurrentProto, true);
            CreateServerProto(m_CurrentProto, true);
            CreateProtoHandler(m_CurrentProto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outType">0=对应的lua类型, 1=Lua默认值</param>
        /// <returns></returns>
        private string ToLuaType(string type, int outType)
        {
            string str = string.Empty;
            string str1 = string.Empty;
            switch (type)
            {
                case "short":
                    str = "number";
                    str1 = "0";
                    break;
                case "ushort":
                    str = "number";
                    str1 = "0";
                    break;
                case "int":
                    str = "number";
                    str1 = "0";
                    break;
                case "uint":
                    str = "number";
                    str1 = "0";
                    break;
                case "long":
                    str = "number";
                    str1 = "0";
                    break;
                case "ulong":
                    str = "number";
                    str1 = "0";
                    break;
                case "float":
                    str = "number";
                    str1 = "0";
                    break;
                case "string":
                    str = "string";
                    str1 = "\"\"";
                    break;
                case "bool":
                    str = "boolean";
                    str1 = "false";
                    break;
                case "byte":
                    str = "number";
                    str1 = "0";
                    break;
            }

            if (outType == 0)
            {
                return str;
            }
            else if (outType == 1)
            {
                return str1;
            }
            else
            {
                return str;
            }
        }

        private string ChangeTypeName(string type)
        {
            string str = string.Empty;

            switch (type)
            {
                case "short":
                    str = "Short()";
                    break;
                case "ushort":
                    str = "UShort()";
                    break;
                case "int":
                    str = "Int()";
                    break;
                case "uint":
                    str = "UInt()";
                    break;
                case "long":
                    str = "Long()";
                    break;
                case "ulong":
                    str = "ULong()";
                    break;
                case "float":
                    str = "Float()";
                    break;
                case "string":
                    str = "UTF8String()";
                    break;
                case "bool":
                    str = "Bool()";
                    break;
                case "byte":
                    str = "Byte()";
                    break;
            }

            return str;
        }

        #region CreateServerProto 生成服务器端协议
        /// <summary>
        /// 生成服务器端协议
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="isSave"></param>
        private void CreateServerProto(MyProto proto, bool isSave = false)
        {
            if (isSave)
            {
                //生成协议代码
                SaveProto();
            }

            List<MyProtoAttr> lst = DataMgr.Instance.GetProtoArrtList(proto.ProtoID, proto.MenuID);

            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "GameServer.Proto");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            sbr = sbr.Replace("#Describte#", proto.ProtoDesc);
            sbr = sbr.Replace("#ScriptName#", proto.ProtoEnName);
            sbr = sbr.Replace("#ProtoCodeDef#", proto.ProtoCode);
            if (proto.ProtoCategory.Equals("C2MS"))
            {
                sbr = sbr.Replace("#PacketType#", "CS");
            }
            else if (proto.ProtoCategory.Equals("MS2C"))
            {
                sbr = sbr.Replace("#PacketType#", "SC");
            }

            #region 基础类型结构
            StringBuilder attributeSbr = new StringBuilder();
            int index = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                var item = lst[i];
                if (!string.IsNullOrEmpty(item.AttToLoop))
                {
                    index++;

                    //这里是循环项的从属属性
                    attributeSbr.AppendFormat("        /// <summary>\r\n");
                    attributeSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                    attributeSbr.AppendFormat("        /// </summary>\r\n");
                    attributeSbr.AppendFormat("        /// </summary>\r\n");
                    attributeSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                    attributeSbr.AppendFormat("        public List<{0}> {1}List;\r\n", item.AttType, item.AttEnName);
                    attributeSbr.AppendFormat("\r\n");
                }
                else
                {
                    //生成标准的基本属性
                    if (string.IsNullOrEmpty(item.AttToCus) && item.AttIsExistsProto == false)
                    {
                        index++;

                        attributeSbr.AppendFormat("        /// <summary>\r\n");
                        attributeSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                        attributeSbr.AppendFormat("        /// </summary>\r\n");
                        attributeSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                        attributeSbr.AppendFormat("        public {0} {1};\r\n", item.AttType, item.AttEnName);
                        attributeSbr.AppendFormat("\r\n");
                    }
                }
            }

            sbr = sbr.Replace("#Attribute#", attributeSbr.ToString().Trim());
            #endregion

            #region 生成自定义类型结构
            StringBuilder customSbr = new StringBuilder();
            foreach (var item in lst)
            {
                //如果是自定义属性类型
                if (item.AttType != "byte" && item.AttType != "short" && item.AttType != "int" && item.AttType != "long" && item.AttType != "string" && item.AttType != "char" && item.AttType != "float" && item.AttType != "decimal" && item.AttType != "bool" && item.AttType != "ushort")
                {
                    customSbr.AppendFormat("\r\n");
                    //判断是不是已有协议
                    if (item.AttIsExistsProto)
                    {
                        index++;

                        #region 是已有协议
                        //判断是不是循环项
                        if (item.AttIsLoop)
                        {
                            customSbr.AppendFormat("        /// <summary>\r\n");
                            customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                            customSbr.AppendFormat("        /// </summary>\r\n");
                            customSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                            customSbr.AppendFormat("        public List<{0}> {1};\r\n\r\n", item.AttType, item.AttEnName);
                        }
                        else
                        {
                            customSbr.AppendFormat("        /// <summary>\r\n");
                            customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                            customSbr.AppendFormat("        /// </summary>\r\n");
                            customSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                            customSbr.AppendFormat("        public {0} {1};\r\n\r\n", item.AttType, item.AttEnName);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不是已有协议
                        customSbr.AppendFormat("        /// <summary>\r\n");
                        customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                        customSbr.AppendFormat("        /// </summary>\r\n");
                        customSbr.AppendFormat("        [Serializable, ProtoContract(Name = {0})]\r\n", string.Format("@\"{0}\"", item.AttType));
                        customSbr.AppendFormat("        public class {0}\r\n", item.AttType);
                        customSbr.AppendFormat("        {{\r\n");

                        //====================隶属于此自定义项的属性=================
                        //查找隶属于这个循环项的
                        {
                            List<MyProtoAttr> list = lst.Where(p => p.AttToCus.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            if (list != null && list.Count > 0)
                            {
                                for (int j = 0; j < list.Count; j++)
                                {
                                    //把已经提前用的了设置一下
                                    lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;
                                    customSbr.AppendFormat("            /// <summary>\r\n");
                                    customSbr.AppendFormat("            /// {0}\r\n", list[j].AttCnName);
                                    customSbr.AppendFormat("            /// </summary>\r\n");
                                    customSbr.AppendFormat("            [ProtoMember({0}, Name = @\"{1}\")]\r\n", j + 1, list[j].AttEnName);
                                    customSbr.AppendFormat("            public {0} {1};\r\n", list[j].AttType, list[j].AttEnName);
                                    customSbr.AppendFormat("\r\n");
                                }
                            }
                        }
                        customSbr.AppendFormat("        }}\r\n");
                        #endregion
                    }
                }
            }

            sbr = sbr.Replace("#CustomAttribute#", customSbr.ToString());
            #endregion

            if (proto.IsCSharp)
            {
                using (FileStream fs = new FileStream(string.Format("{0}/{1}.cs", txtServerProtoPath.Text, proto.ProtoEnName), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbr.ToString());
                    }
                }
            }
        }
        #endregion

        #region CreateProto 生成客户端协议
        /// <summary>
        /// 生成客户端协议
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="isSave"></param>
        private void CreateProto(MyProto proto, bool isSave = false)
        {
            if (isSave)
            {
                //生成协议代码
                SaveProto();
            }

            CreateCSharpProto(proto);
            CreateLuaProto(proto);
        }

        private void CreateCSharpProto(MyProto proto)
        {
            List<MyProtoAttr> lst = DataMgr.Instance.GetProtoArrtList(proto.ProtoID, proto.MenuID);

            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "SuperBiomass.Network");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            sbr = sbr.Replace("#Describte#", proto.ProtoDesc);
            sbr = sbr.Replace("#ScriptName#", proto.ProtoEnName);
            sbr = sbr.Replace("#ProtoCodeDef#", proto.ProtoCode);
            if (proto.ProtoCategory.Equals("C2MS"))
            {
                sbr = sbr.Replace("#PacketType#", "CS");
            }
            else if (proto.ProtoCategory.Equals("MS2C"))
            {
                sbr = sbr.Replace("#PacketType#", "SC");
            }

            #region 基础类型结构
            StringBuilder attributeSbr = new StringBuilder();
            int index = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                var item = lst[i];
                if (!string.IsNullOrEmpty(item.AttToLoop))
                {
                    index++;

                    //这里是循环项的从属属性
                    attributeSbr.AppendFormat("        /// <summary>\r\n");
                    attributeSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                    attributeSbr.AppendFormat("        /// </summary>\r\n");
                    attributeSbr.AppendFormat("        /// </summary>\r\n");
                    attributeSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                    attributeSbr.AppendFormat("        public List<{0}> {1}List;\r\n", item.AttType, item.AttEnName);
                    attributeSbr.AppendFormat("\r\n");
                }
                else
                {
                    //生成标准的基本属性
                    if (string.IsNullOrEmpty(item.AttToCus) && item.AttIsExistsProto == false)
                    {
                        index++;

                        attributeSbr.AppendFormat("        /// <summary>\r\n");
                        attributeSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                        attributeSbr.AppendFormat("        /// </summary>\r\n");
                        attributeSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                        attributeSbr.AppendFormat("        public {0} {1};\r\n", item.AttType, item.AttEnName);
                        attributeSbr.AppendFormat("\r\n");
                    }
                }
            }

            sbr = sbr.Replace("#Attribute#", attributeSbr.ToString().Trim());
            #endregion

            #region 生成自定义类型结构
            StringBuilder customSbr = new StringBuilder();
            foreach (var item in lst)
            {
                //如果是自定义属性类型
                if (item.AttType != "byte" 
                    && item.AttType != "short" 
                    && item.AttType != "int" 
                    && item.AttType != "long" 
                    && item.AttType != "string" 
                    && item.AttType != "char" 
                    && item.AttType != "float" 
                    && item.AttType != "decimal" 
                    && item.AttType != "bool" 
                    && item.AttType != "ushort")
                {
                    customSbr.AppendFormat("\r\n");
                    //判断是不是已有协议
                    if (item.AttIsExistsProto)
                    {
                        index++;

                        #region 是已有协议
                        //判断是不是循环项
                        if (item.AttIsLoop)
                        {
                            customSbr.AppendFormat("        /// <summary>\r\n");
                            customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                            customSbr.AppendFormat("        /// </summary>\r\n");
                            customSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                            customSbr.AppendFormat("        public List<{0}> {1};\r\n\r\n", item.AttType, item.AttEnName);
                        }
                        else
                        {
                            customSbr.AppendFormat("        /// <summary>\r\n");
                            customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                            customSbr.AppendFormat("        /// </summary>\r\n");
                            customSbr.AppendFormat("        [ProtoMember({0}, Name = @\"{1}\")]\r\n", index, item.AttEnName);
                            customSbr.AppendFormat("        public {0} {1};\r\n\r\n", item.AttType, item.AttEnName);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不是已有协议
                        customSbr.AppendFormat("        /// <summary>\r\n");
                        customSbr.AppendFormat("        /// {0}\r\n", item.AttCnName);
                        customSbr.AppendFormat("        /// </summary>\r\n");
                        customSbr.AppendFormat("        [Serializable, ProtoContract(Name = {0})]\r\n", string.Format("@\"{0}\"", item.AttType));
                        customSbr.AppendFormat("        public class {0}\r\n", item.AttType);
                        customSbr.AppendFormat("        {{\r\n");

                        //====================隶属于此自定义项的属性=================
                        //查找隶属于这个循环项的
                        {
                            List<MyProtoAttr> list = lst.Where(p => p.AttToCus.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            if (list != null && list.Count > 0)
                            {
                                for (int j = 0; j < list.Count; j++)
                                {
                                    //把已经提前用的了设置一下
                                    lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;
                                    customSbr.AppendFormat("            /// <summary>\r\n");
                                    customSbr.AppendFormat("            /// {0}\r\n", list[j].AttCnName);
                                    customSbr.AppendFormat("            /// </summary>\r\n");
                                    customSbr.AppendFormat("            [ProtoMember({0}, Name = @\"{1}\")]\r\n", j + 1, list[j].AttEnName);
                                    customSbr.AppendFormat("            public {0} {1};\r\n", list[j].AttType, list[j].AttEnName);
                                    customSbr.AppendFormat("\r\n");
                                }
                            }
                        }
                        customSbr.AppendFormat("        }}\r\n");
                        #endregion
                    }
                }
            }

            sbr = sbr.Replace("#CustomAttribute#", customSbr.ToString());
            #endregion

            if (proto.IsCSharp)
            {
                using (FileStream fs = new FileStream(string.Format("{0}/{1}.cs", txtCSharpProtoPath.Text, proto.ProtoEnName), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbr.ToString());
                    }
                }
            }
        }

        private void CreateLuaProto(MyProto proto)
        {
            List<MyProtoAttr> lst = DataMgr.Instance.GetProtoArrtList(proto.ProtoID, proto.MenuID);

            StringBuilder sbrLua = new StringBuilder();
            sbrLua.AppendFormat("--{0}\r\n", proto.ProtoCnName);
            sbrLua.AppendFormat("{0}Proto = {{ ProtoCode = {1}, ", proto.ProtoEnName, proto.ProtoCode);

            foreach (var item in lst)
            {
                if (item.AttIsExistsProto == false)
                {
                    if (!string.IsNullOrEmpty(item.AttToLoop))
                    {
                        //这里是循环项的从属属性

                        sbrLua.AppendFormat("{0}Table = {{ }}, ", item.AttEnName);
                    }
                    else
                    {
                        //生成标准的基本属性
                        if (string.IsNullOrEmpty(item.AttToCus))
                        {
                            sbrLua.AppendFormat("{0} = {1}, ", item.AttEnName, ToLuaType(item.AttType, 1));
                        }
                    }
                }
                else
                {
                    sbrLua.AppendFormat("{0} = nil, ", item.AttEnName);
                }
            }
            sbrLua.Remove(sbrLua.Length - 2, 2);
            sbrLua.Append(" }\r\n");


            sbrLua.AppendFormat("local this = {0}Proto;\r\n", proto.ProtoEnName);
            sbrLua.Append("\r\n");
            sbrLua.AppendFormat("{0}Proto.__index = {0}Proto;\r\n", proto.ProtoEnName);
            sbrLua.Append("\r\n");
            sbrLua.AppendFormat("function {0}Proto.New()\r\n", proto.ProtoEnName);
            sbrLua.Append("    local self = { };\r\n");
            sbrLua.AppendFormat("    setmetatable(self, {0}Proto);\r\n", proto.ProtoEnName);
            sbrLua.Append("    return self;\r\n");
            sbrLua.Append("end\r\n\r\n");

            //协议名字
            sbrLua.AppendFormat("function {0}Proto.GetProtoName()\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("    return \"{0}\";\r\n", proto.ProtoEnName);
            sbrLua.Append("end\r\n");

            #region 生成自定义类型结构
            foreach (var item in lst)
            {
                //如果是自定义属性类型
                if (item.AttType != "byte" && item.AttType != "short" && item.AttType != "int" && item.AttType != "long" && item.AttType != "string" && item.AttType != "char" && item.AttType != "float" && item.AttType != "decimal" && item.AttType != "bool" && item.AttType != "ushort")
                {
                    //判断是不是已有协议
                    if (item.AttIsExistsProto)
                    {
                        #region 是已有协议
                        //判断是不是循环项
                        #endregion
                    }
                    else
                    {
                        #region 不是已有协议
                        string strForNextLua = "";
                        //====================隶属于此自定义项的属性=================
                        //查找隶属于这个循环项的
                        {
                            List<MyProtoAttr> list = lst.Where(p => p.AttToCus.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            if (list != null && list.Count > 0)
                            {
                                for (int j = 0; j < list.Count; j++)
                                {
                                    //把已经提前用的了设置一下
                                    lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;
                                    strForNextLua += string.Format("{0} = {1}, ", list[j].AttEnName, ToLuaType(list[j].AttType, 1));
                                }
                            }
                        }

                        //======================lua协议代码开始
                        sbrLua.Append("\r\n");
                        sbrLua.Append("\r\n");
                        sbrLua.AppendFormat("--定义{0}\r\n", item.AttCnName);
                        sbrLua.AppendFormat("{0} = {{ {1} }}\r\n", item.AttEnName, strForNextLua.TrimEnd(',', ' '));
                        sbrLua.AppendFormat("{0}.__index = {0};\r\n", item.AttEnName);
                        sbrLua.AppendFormat("function {0}.New()\r\n", item.AttEnName);
                        sbrLua.Append("    local self = { };\r\n");
                        sbrLua.AppendFormat("    setmetatable(self, {0});\r\n", item.AttEnName);
                        sbrLua.Append("    return self;\r\n");
                        sbrLua.Append("end\r\n");
                        //======================lua协议代码结束
                        #endregion
                    }
                }
            }
            #endregion

            #region ToArray 方法
            sbrLua.Append("\r\n");
            sbrLua.Append("\r\n");
            sbrLua.Append("--发送协议\r\n");
            sbrLua.AppendFormat("function {0}Proto.SendProto(proto, isChild)\r\n", proto.ProtoEnName);
            sbrLua.Append("\r\n");
            sbrLua.Append("    local ms = nil;\r\n");
            sbrLua.Append("\r\n");
            sbrLua.Append("    if (isChild == nil or isChild == false) then\r\n");
            sbrLua.Append("        ms = CS.YouYou.GameEntry.Socket.SocketSendMS;\r\n");
            sbrLua.Append("        ms:SetLength(0);\r\n");
            sbrLua.Append("        ms:WriteUShort(proto.ProtoCode);\r\n");
            sbrLua.Append("    else\r\n");
            sbrLua.Append("        ms = CS.YouYou.GameEntry.Lua:DequeueMemoryStream();\r\n");
            sbrLua.Append("        ms.SetLength(0);\r\n");
            sbrLua.Append("    end\r\n\r\n");

            //写入数据流
            foreach (var item in lst)
            {
                if (item.AttIsUsed) continue;

                //如果不是已有协议
                if (item.AttIsExistsProto == false)
                {
                    sbrLua.AppendFormat("    ms:Write{0}(proto.{1});\r\n", ChangeTypeName(item.AttType).Replace("()", ""), item.AttEnName);
                }

                if (item.AttType == "bool")
                {
                    //如果是布尔变量
                    #region 布尔
                    bool isHasSuccess = false;
                    //查找隶属于这个bool成功的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, item.AttEnName, true);
                        if (list != null && list.Count > 0)
                        {
                            isHasSuccess = true;
                            sbrLua.AppendFormat("    if(proto.{0}) then\r\n", item.AttEnName);

                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbrLua.AppendFormat("        ms:Write{0}({1});\r\n", ChangeTypeName(list[j].AttType).Replace("()", ""), list[j].AttEnName);

                            }
                        }
                    }

                    //查找隶属于这个bool失败的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, item.AttEnName, false);
                        if (list != null && lst.Count > 0)
                        {
                            if (isHasSuccess)
                            {
                                sbrLua.Append("        else\r\n");
                            }
                            else
                            {
                                //如果没有成功项
                                sbrLua.AppendFormat("    if(not proto.{0}) then\r\n", item.AttEnName);
                            }

                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                sbrLua.AppendFormat("        ms:Write{0}({1});\r\n", ChangeTypeName(list[j].AttType).Replace("()", ""), list[j].AttEnName);
                            }
                        }
                    }

                    sbrLua.AppendFormat("    end\r\n");
                    #endregion
                }
                else if ((item.AttType == "byte" || item.AttType == "short" || item.AttType == "int" || item.AttType == "long") && item.AttIsLoop)
                {
                    #region 循环项目
                    sbrLua.AppendFormat("    for i = 1, proto.{0}, 1 do\r\n", item.AttEnName);

                    //查找隶属于这个循环项的
                    {
                        List<MyProtoAttr> list = lst.Where(p => p.AttToLoop.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (list != null && list.Count > 0)
                        {
                            sbrLua.AppendFormat("        local item = proto.{0}Table[i];\r\n", list[0].AttEnName);
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //如果是自定义属性类型
                                if (list[j].AttType != "byte" && list[j].AttType != "short" && list[j].AttType != "int" && list[j].AttType != "long" && list[j].AttType != "string" && list[j].AttType != "char" && list[j].AttType != "float" && list[j].AttType != "decimal" && list[j].AttType != "bool" && list[j].AttType != "ushort")
                                {
                                    {
                                        {
                                            List<MyProtoAttr> list2 = lst.Where(p => p.AttToCus.Equals(list[j].AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                                            if (list2 != null && list2.Count > 0)
                                            {
                                                for (int l = 0; l < list2.Count; l++)
                                                {
                                                    //把已经提前用的了设置一下
                                                    lst.Where(p => p.AttID == list2[l].AttID).First().AttIsUsed = true;
                                                    sbrLua.AppendFormat("        ms:Write{0}(item.{2});\r\n", ChangeTypeName(list2[l].AttType).Replace("()", ""), list[j].AttEnName, list2[l].AttEnName);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //然后写到这个bool变量下
                                    sbrLua.AppendFormat("        ms:Write{0}(item);\r\n", ChangeTypeName(list[j].AttType).Replace("()", ""), list[j].AttEnName, list[j].AttCnName);
                                }
                            }
                        }
                    }

                    sbrLua.Append("    end\r\n");
                    #endregion
                }
                else if (item.AttIsExistsProto)
                {
                    //如果是已有协议
                    if (item.AttIsLoop)
                    {
                        //如果是循环项

                        sbrLua.AppendFormat("\r\n");
                        sbrLua.AppendFormat("    if (proto.{0} ~= nil) then\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        local len_{0} = #proto.{0};\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        for i = 1, len_{0} do\r\n", item.AttEnName);
                        sbrLua.AppendFormat("            local _{0}Proto = proto.{1}[i];\r\n", item.AttType, item.AttEnName);
                        sbrLua.AppendFormat("            local _buff_Curr = {0}Proto.SendProto(_{0}Proto, true);\r\n", item.AttType);
                        sbrLua.AppendFormat("            ms:WriteInt(_buff_Curr.Length);\r\n");
                        sbrLua.AppendFormat("            ms:Write(_buff_Curr, 0, _buff_Curr.Length);\r\n");
                        sbrLua.AppendFormat("        end\r\n");
                        sbrLua.AppendFormat("    else\r\n");
                        sbrLua.AppendFormat("        ms:WriteInt(0);\r\n");
                        sbrLua.AppendFormat("    end\r\n");
                    }
                    else
                    {
                        sbrLua.AppendFormat("\r\n");
                        sbrLua.AppendFormat("    if (proto.{0} ~= nil) then\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        local buff_{0} = {1}Proto.SendProto(proto.{0}, true);\r\n", item.AttEnName, item.AttType);
                        sbrLua.AppendFormat("        ms:WriteInt(buff_{0}.Length);\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        ms:Write(buff_{0}, 0, buff_{0}.Length);\r\n", item.AttEnName);
                        sbrLua.AppendFormat("    else\r\n");
                        sbrLua.AppendFormat("        ms:WriteInt(0);\r\n");
                        sbrLua.AppendFormat("    end\r\n");
                    }
                }
            }

            sbrLua.Append("\r\n");
            sbrLua.Append("    if (isChild == nil or isChild == false) then\r\n");
            sbrLua.Append("        if (CS.YouYou.GameEntry.Lua.DebugLogProto == true) then\r\n");
            sbrLua.Append("            print(string.format(\"<color=#ffa200>发送消息:</color><color=#FFFB80>%s %s</color>\", this.GetProtoName(), proto.ProtoCode));\r\n");
            sbrLua.Append("            print(string.format(\"<color=#ffdeb3>==>>%s</color>\", json.encode(proto)));\r\n");
            sbrLua.Append("        end\r\n");
            sbrLua.Append("\r\n");
            sbrLua.Append("        CS.YouYou.GameEntry.Socket:SendMainMsg(ms:ToArray());\r\n");
            sbrLua.Append("    else\r\n");
            sbrLua.Append("        local retBuffer = ms:ToArray();\r\n");
            sbrLua.Append("        CS.YouYou.GameEntry.Lua:EnqueueMemoryStream(ms);\r\n");
            sbrLua.Append("        return retBuffer;\r\n");
            sbrLua.Append("    end\r\n");
            sbrLua.Append("end\r\n");
            #endregion

            #region GetProto 方法
            sbrLua.Append("\r\n");
            sbrLua.Append("\r\n");
            sbrLua.Append("--解析协议\r\n");
            sbrLua.AppendFormat("function {0}Proto.GetProto(buffer, isChild)\r\n", proto.ProtoEnName);
            sbrLua.Append("\r\n");
            sbrLua.AppendFormat("    local proto = {0}Proto.New(); --实例化一个协议对象\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("    local ms = nil;\r\n");
            sbrLua.AppendFormat("    if (isChild == nil or isChild == false) then\r\n");
            sbrLua.AppendFormat("        ms = CS.YouYou.GameEntry.Lua:LoadSocketReceiveMS(buffer);\r\n");
            sbrLua.AppendFormat("    else\r\n");
            sbrLua.AppendFormat("        ms = CS.YouYou.GameEntry.Lua:DequeueMemoryStreamAndLoadBuffer(buffer);\r\n");
            sbrLua.AppendFormat("    end\r\n");
            sbrLua.Append("\r\n");

            //从数据流读取
            foreach (var item in lst)
            {
                if (item.AttIsUsed) continue;

                if (item.AttIsExistsProto == false)
                {
                    if (item.AttType == "byte")
                    {
                        sbrLua.AppendFormat("    proto.{0} = ms:Read{1};\r\n", item.AttEnName, ChangeTypeName(item.AttType));
                    }
                    else
                    {
                        sbrLua.AppendFormat("    proto.{0} = ms:Read{1};\r\n", item.AttEnName, ChangeTypeName(item.AttType));
                    }
                }

                if (item.AttType == "bool")
                {
                    //如果是布尔变量
                    #region 布尔
                    bool isHasSuccess = false;
                    //查找隶属于这个bool成功的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, item.AttEnName, true);
                        if (list != null && list.Count > 0)
                        {
                            isHasSuccess = true;
                            sbrLua.AppendFormat("    if(proto.{0}) then\r\n", item.AttEnName);
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //然后写到这个bool变量下
                                if (list[j].AttType == "byte")
                                {
                                    sbrLua.AppendFormat("        proto.{0} = ms:Read{1};\r\n", list[j].AttEnName, ChangeTypeName(list[j].AttType));
                                }
                                else
                                {
                                    sbrLua.AppendFormat("        proto.{0} = ms:Read{1};\r\n", list[j].AttEnName, ChangeTypeName(list[j].AttType));
                                }
                            }
                        }
                    }

                    //查找隶属于这个bool失败的
                    {
                        List<MyProtoAttr> list = GetListByToBoolName(lst, item.AttEnName, false);
                        if (list != null && lst.Count > 0)
                        {
                            if (isHasSuccess)
                            {
                                sbrLua.Append("        else\r\n");
                            }
                            else
                            {
                                //如果没有成功项
                                sbrLua.AppendFormat("    if(not proto.{0}) then\r\n", item.AttEnName);
                            }

                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                if (list[j].AttType == "byte")
                                {
                                    //然后写到这个bool变量下
                                    sbrLua.AppendFormat("        proto.{0} = ms:Read{1};\r\n", list[j].AttEnName, ChangeTypeName(list[j].AttType));
                                }
                                else
                                {
                                    sbrLua.AppendFormat("        proto.{0} = ms:Read{1};\r\n", list[j].AttEnName, ChangeTypeName(list[j].AttType));
                                }
                            }
                        }
                    }

                    sbrLua.AppendFormat("    end\r\n");
                    #endregion
                }
                else if ((item.AttType == "byte" || item.AttType == "short" || item.AttType == "int" || item.AttType == "long") && item.AttIsLoop)
                {
                    #region 循环项目
                    //======================================
                    //1.定义列表
                    {
                        List<MyProtoAttr> list = lst.Where(p => p.AttToLoop.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (list != null && list.Count > 0)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                sbrLua.AppendFormat("	proto.{0}Table = {{}};\r\n", list[j].AttEnName);
                            }
                        }
                    }
                    //======================================

                    sbrLua.AppendFormat("    for i = 1, proto.{0}, 1 do\r\n", item.AttEnName);

                    //查找隶属于这个循环项的
                    {
                        List<MyProtoAttr> list = lst.Where(p => p.AttToLoop.Equals(item.AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (list != null && list.Count > 0)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                //把已经提前用的了设置一下
                                lst.Where(p => p.AttID == list[j].AttID).First().AttIsUsed = true;

                                //如果是自定义属性类型
                                if (list[j].AttType != "byte" && list[j].AttType != "short" && list[j].AttType != "int" && list[j].AttType != "long" && list[j].AttType != "string" && list[j].AttType != "char" && list[j].AttType != "float" && list[j].AttType != "decimal" && list[j].AttType != "bool" && list[j].AttType != "ushort")
                                {
                                    {
                                        {
                                            List<MyProtoAttr> list2 = lst.Where(p => p.AttToCus.Equals(list[j].AttEnName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                                            if (list2 != null && list2.Count > 0)
                                            {
                                                sbrLua.AppendFormat("        local _{1} = {0}.New();\r\n", list[j].AttEnName, list[j].AttEnName);

                                                for (int l = 0; l < list2.Count; l++)
                                                {
                                                    //把已经提前用的了设置一下
                                                    lst.Where(p => p.AttID == list2[l].AttID).First().AttIsUsed = true;

                                                    if (list2[l].AttType == "byte")
                                                    {
                                                        sbrLua.AppendFormat("        _{0}.{1} = ms:Read{2}();\r\n", list[j].AttEnName, list2[l].AttEnName, ChangeTypeName(list2[l].AttType).Replace("()", ""));
                                                    }
                                                    else
                                                    {
                                                        sbrLua.AppendFormat("        _{0}.{1} = ms:Read{2}();\r\n", list[j].AttEnName, list2[l].AttEnName, ChangeTypeName(list2[l].AttType).Replace("()", ""));
                                                    }
                                                }

                                                sbrLua.AppendFormat("        proto.{0}Table[#proto.{0}Table+1] = _{0};\r\n", list[j].AttEnName);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (list[j].AttType == "byte")
                                    {
                                        //然后写到这个bool变量下
                                        sbrLua.AppendFormat("        local _{1} = ms:Read{2}();  --{3}\r\n", list[j].AttType, list[j].AttEnName, ChangeTypeName(list[j].AttType).Replace("()", ""), list[j].AttCnName);
                                    }
                                    else
                                    {
                                        sbrLua.AppendFormat("        local _{1} = ms:Read{2}();  --{3}\r\n", list[j].AttType, list[j].AttEnName, ChangeTypeName(list[j].AttType).Replace("()", ""), list[j].AttCnName);
                                    }
                                    sbrLua.AppendFormat("        proto.{0}Table[#proto.{0}Table+1] = _{0};\r\n", list[j].AttEnName);
                                }
                            }
                        }
                    }

                    sbrLua.Append("    end\r\n");
                    #endregion
                }
                else if (item.AttIsExistsProto)
                {
                    //如果是已有协议
                    if (item.AttIsLoop)
                    {
                        //如果是循环项
                        sbrLua.AppendFormat("\r\n");
                        sbrLua.AppendFormat("    local len_{0} = ms:ReadInt();\r\n", item.AttEnName);
                        sbrLua.AppendFormat("    if (len_{0} > 0) then\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        proto.{0} = {{ }};\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        for i = 1, len_{0} do\r\n", item.AttEnName);
                        sbrLua.AppendFormat("            local _len_{0} = ms:ReadInt();\r\n", item.AttEnName);
                        sbrLua.AppendFormat("            if (_len_{0} > 0) then\r\n", item.AttEnName);
                        sbrLua.AppendFormat("                local _buff_{0} = CS.YouYou.GameEntry.Lua:GetByteArray(ms, _len_{0});\r\n", item.AttEnName);
                        sbrLua.AppendFormat("                proto.{0}[i] = {1}Proto.GetProto(_buff_{0}, true);\r\n", item.AttEnName, item.AttType);
                        sbrLua.AppendFormat("            end\r\n");
                        sbrLua.AppendFormat("        end\r\n");
                        sbrLua.AppendFormat("    end\r\n");
                    }
                    else
                    {
                        //不是循环项
                        sbrLua.AppendFormat("\r\n");
                        sbrLua.AppendFormat("    local len_{0} = ms:ReadInt();\r\n", item.AttEnName);
                        sbrLua.AppendFormat("    if (len_{0} > 0) then\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        local buff_{0} = CS.YouYou.GameEntry.Lua:GetByteArray(ms, len_{0});\r\n", item.AttEnName);
                        sbrLua.AppendFormat("        proto.{0} = {1}Proto.GetProto(buff_{0}, true);\r\n", item.AttEnName, item.AttType);
                        sbrLua.AppendFormat("    end\r\n");
                    }
                }
            }

            sbrLua.Append("\r\n");
            sbrLua.Append("    if (isChild == nil or isChild == false) then\r\n");
            sbrLua.AppendFormat("        if (CS.YouYou.GameEntry.Lua.DebugLogProto == true) then\r\n");
            sbrLua.AppendFormat("            print(string.format(\"<color=#00eaff>接收消息:</color><color=#00ff9c>%s %s</color>\", this.GetProtoName(), proto.ProtoCode));\r\n");
            sbrLua.AppendFormat("            print(string.format(\"<color=#c5e1dc>==>>%s</color>\", json.encode(proto)));\r\n");
            sbrLua.AppendFormat("        end\r\n");
            sbrLua.Append("    end\r\n");
            sbrLua.Append("\r\n");
            sbrLua.AppendFormat("    return proto;\r\n");
            sbrLua.AppendFormat("end");
            #endregion

            if (proto.IsLua)
            {
                using (FileStream fs = new FileStream(string.Format("{0}/{1}Proto.bytes", txtLuaProtoPath.Text, proto.ProtoEnName), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbrLua.ToString());
                    }
                }
            }
        }
        #endregion

        #region CreateProtoHandler 生成客户端协议处理类
        /// <summary>
        /// 生成ProtoHandler
        /// </summary>
        /// <param name="proto"></param>
        private void CreateProtoHandler(MyProto proto)
        {
            if (proto.ProtoCategory.Equals("C2MS") || proto.ProtoCategory.Equals("C2SS"))
            {
                CreateServerCSharpProtoHandler(proto);
            }

            if (proto.ProtoCategory.Equals("MS2C") || proto.ProtoCategory.Equals("SS2C"))
            {
                CreateClientCSharpProtoHandler(proto);
                CreateClientLuaProtoHandler(proto);
            }
        }

        private void CreateServerCSharpProtoHandler(MyProto proto)
        {
            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoHandlerScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "GameServer.Network");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            sbr = sbr.Replace("#Describte#", proto.ProtoDesc);
            sbr = sbr.Replace("#ScriptName#", proto.ProtoEnName);
            sbr = sbr.Replace("#ProtoCodeDef#", proto.ProtoCode);

            string csharpPath = string.Format("{0}/{1}Handler.cs", txtServerProtoPath.Text, proto.ProtoEnName);
            if (proto.IsCSharp && !File.Exists(csharpPath))
            {
                using (FileStream fs = new FileStream(csharpPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbr.ToString().Trim());
                    }
                }
            }
        }

        private void CreateClientCSharpProtoHandler(MyProto proto)
        {
            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoHandlerScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "SuperBiomass.Network");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            sbr = sbr.Replace("#Describte#", proto.ProtoDesc);
            sbr = sbr.Replace("#ScriptName#", proto.ProtoEnName);
            sbr = sbr.Replace("#ProtoCodeDef#", proto.ProtoCode);

            string csharpPath = string.Format("{0}/{1}Handler.cs", txtCSharpProtoHandlerPath.Text, proto.ProtoEnName);
            if (proto.IsCSharp && !File.Exists(csharpPath))
            {
                using (FileStream fs = new FileStream(csharpPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbr.ToString().Trim());
                    }
                }
            }
        }

        private void CreateClientLuaProtoHandler(MyProto proto)
        {
            if (!proto.ProtoCategory.Equals("MS2C") && !proto.ProtoCategory.Equals("SS2C")) return;

            StringBuilder sbrLua = new StringBuilder();
            sbrLua.AppendFormat("--{0}（工具只生成一次）\r\n", proto.ProtoCnName);
            sbrLua.AppendFormat("{0}Handler = {{ }};\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("\r\n");
            sbrLua.AppendFormat("local this = {0}Handler;\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("\r\n");
            sbrLua.AppendFormat("function {0}Handler.On{0}(buffer)\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("    local proto = {0}Proto.GetProto(buffer);\r\n", proto.ProtoEnName);
            sbrLua.AppendFormat("end");

            string luaPath = string.Format("{0}/{1}Handler.bytes", txtLuaProtoHandlerPath.Text, proto.ProtoEnName);
            if (proto.IsLua && !File.Exists(luaPath))
            {
                using (FileStream fs = new FileStream(luaPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sbrLua.ToString());
                    }
                }
            }
        }
        #endregion

        #region CreateProtoCodeDef 生成协议编号类
        private void CreateProtoCodeDef()
        {
            //CreateClientCSharpProtoCodeDef();
            CreateClientLuaProtoCodeDef();
            CreateServerProtoCodeDef();
        }

        private void CreateClientCSharpProtoCodeDef()
        {
            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoCodeDefScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "SuperBiomass.Network");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

            StringBuilder sbrContent = new StringBuilder();
            foreach (var menu in DataMgr.Instance.MenuList)
            {
                foreach (var proto in menu.ProtoList)
                {
                    if (proto.IsCSharp)
                    {
                        sbrContent.AppendFormat("        /// <summary>\r\n");
                        sbrContent.AppendFormat("        /// {0}\r\n", proto.ProtoCnName);
                        sbrContent.AppendFormat("        /// </summary>\r\n");
                        sbrContent.AppendFormat("        public const ushort {0} = {1};\r\n", proto.ProtoEnName, proto.ProtoCode);
                        sbrContent.AppendFormat("\r\n");
                    }
                }
            }
            sbr = sbr.Replace("#ProtoCodeDefContent#", sbrContent.ToString().Trim());

            using (FileStream fs = new FileStream(string.Format("{0}/ProtoCodeDef.cs", txtCSharpProtoPath.Text), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }

        private void CreateClientLuaProtoCodeDef()
        {
            StringBuilder sbrLua = new StringBuilder();
            sbrLua.AppendFormat("ProtoCode =\r\n");
            sbrLua.AppendFormat("{{\r\n");

            string strLuaRequire = "";

            foreach (var menu in DataMgr.Instance.MenuList)
            {
                foreach (var proto in menu.ProtoList)
                {
                    if (proto.IsLua)
                    {
                        sbrLua.AppendFormat("    {0} = {1},\r\n", proto.ProtoEnName, proto.ProtoCode);
                        strLuaRequire += string.Format("require \"DataNode/Proto/{0}Proto\";\r\n", proto.ProtoEnName);
                        if (proto.ProtoCategory.Equals("MS2C") || proto.ProtoCategory.Equals("SS2C"))
                        {
                            //如果是到客户端的
                            strLuaRequire += string.Format("require \"DataNode/ProtoHandler/{0}Handler\";\r\n", proto.ProtoEnName);
                        }
                    }
                }
            }
            sbrLua.Remove(sbrLua.Length - 3, 3);

            sbrLua.AppendFormat("\r\n}}\r\n");
            sbrLua.Append(strLuaRequire);

            using (FileStream fs = new FileStream(string.Format("{0}/ProtoCodeDef.bytes", txtLuaProtoPath.Text), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbrLua.ToString());
                }
            }
        }

        private void CreateServerProtoCodeDef()
        {
            StringBuilder sbr = new StringBuilder(DataMgr.Instance.ClientProtoCodeDefScriptTemplateStr);
            sbr = sbr.Replace("#NameSpace#", "GameServer.Network");
            sbr = sbr.Replace("#Author#", "WonHon");
            sbr = sbr.Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

            StringBuilder sbrContent = new StringBuilder();
            foreach (var menu in DataMgr.Instance.MenuList)
            {
                foreach (var proto in menu.ProtoList)
                {
                    if (proto.IsCSharp)
                    {
                        sbrContent.AppendFormat("        /// <summary>\r\n");
                        sbrContent.AppendFormat("        /// {0}\r\n", proto.ProtoCnName);
                        sbrContent.AppendFormat("        /// </summary>\r\n");
                        sbrContent.AppendFormat("        public const ushort {0} = {1};\r\n", proto.ProtoEnName, proto.ProtoCode);
                        sbrContent.AppendFormat("\r\n");
                    }
                }
            }
            sbr = sbr.Replace("#ProtoCodeDefContent#", sbrContent.ToString().Trim());

            using (FileStream fs = new FileStream(string.Format("{0}/ProtoCodeDef.cs", txtServerProtoPath.Text), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }
        #endregion

        private void btnCreateAll_Click(object sender, EventArgs e)
        {
            DataMgr.Instance.LoadXml();

            CreateProtoCodeDef();

            foreach (var menu in DataMgr.Instance.MenuList)
            {
                foreach (var proto in menu.ProtoList)
                {
                    CreateProto(proto);
                    CreateServerProto(proto);
                    CreateProtoHandler(proto);
                }
            }

            MessageBox.Show("代码生成完毕");
        }

        private void frmMSG_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>("C2MS", "客户端->主服务器"));
            lst.Add(new KeyValuePair<string, string>("MS2C", "主服务器->客户端"));
            lst.Add(new KeyValuePair<string, string>("C2SS", "客户端->同步服务器"));
            lst.Add(new KeyValuePair<string, string>("SS2C", "同步服务器->客户端"));
            protoCategory.DataSource = lst;
            protoCategory.ValueMember = "key";
            protoCategory.DisplayMember = "value";

            //======加载路径设置======
            LoadConfig();
        }

        private void LoadConfig()
        {
            string configPath = Application.StartupPath + "/config.txt";

            if (File.Exists(configPath))
            {
                string str = "";
                using (FileStream fs = new FileStream(configPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        str = sr.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    string[] arr = str.Split('\n');
                    if (arr.Length >= 5)
                    {
                        txtCSharpProtoPath.Text = arr[0];
                        txtCSharpProtoHandlerPath.Text = arr[1];
                        txtLuaProtoPath.Text = arr[2];
                        txtLuaProtoHandlerPath.Text = arr[3];
                        txtServerProtoPath.Text = arr[4];
                    }
                }
            }
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            string configPath = Application.StartupPath + "/config.txt";
            if (File.Exists(configPath))
            {
                //如果配置文件存在 先删除
                File.Delete(configPath);
            }

            //创建文本
            StringBuilder sbr = new StringBuilder();
            sbr.Append(txtCSharpProtoPath.Text);
            sbr.Append("\n");
            sbr.Append(txtCSharpProtoHandlerPath.Text);
            sbr.Append("\n");
            sbr.Append(txtLuaProtoPath.Text);
            sbr.Append("\n");
            sbr.Append(txtLuaProtoHandlerPath.Text);
            sbr.Append("\n");
            sbr.Append(txtServerProtoPath.Text);

            using (FileStream fs = new FileStream(configPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }
    }
}