<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
      <%--                  <%=Crumbs()%>--%>      
        <f:PageManager ID="PageManager2" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>                      
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"  DataKeyNames="ID,AM_NAVIGATE_URL,PARENT_ID" 
                            AllowPaging="true" IsDatabasePaging="true"  EnableMultiSelect="false" EnableRowDoubleClickEvent="true" EnableHeaderMenu ="false"
                             OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange" OnRowDoubleClick="Grid1_RowClick">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                      <f:Button ID="back" runat="server" EnablePostBack="true" Text="返回上一层" OnClick="back_Click">
                                        </f:Button>
                                    </Items>
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" Text="添加菜单">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>

                              <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="AM_NAME" HeaderText="菜单名称" DataSimulateTreeLevelField="TreeLevel"
                                    Width="150px" />
                            <%--<f:BoundField DataField="AM_IMAGE_URL" HeaderText="菜单图片地址" Width="200px" />--%>
                                <f:BoundField DataField="AM_NAVIGATE_URL" HeaderText="菜单导航地址" Width="300px" />
                                <f:BoundField DataField="AM_REMARK" HeaderText="菜单备注" Width="150px" />
                                <f:BoundField DataField="AM_SORTINDEX" HeaderText="菜单排序" Width="85px" />
                             <%--   <f:BoundField DataField="PARENT_ID" HeaderText="菜单上级节点" Width="120px" />
                                <f:BoundField DataField="VIEWPOWER_ID" HeaderText="菜单权限编号" Width="120px" />
                                <f:BoundField DataField="AM_ISTREELEAF" HeaderText="是否是叶子节点" ExpandUnusedSpace="true"/>--%>
                                <f:BoundField ExpandUnusedSpace="true" />
                                <%--<f:WindowField ColumnID="powerField" TextAlign="Center" Icon="Pencil" ToolTip="添加权限" HeaderText="添加权限"
                                    WindowID="Update" Title="添加权限" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="/admin/menu/menu_power.aspx?id={0}"
                                    Width="80px" />--%>
                                <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" HeaderText="编辑"
                                    WindowID="Update_Menu" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="/admin/system_manage/menu_edit.aspx?id={0}"
                                    Width="50px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" HeaderText="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <%--<f:Window ID="Update_Demo" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="650px" Height="360" Title="菜单权限">
        </f:Window>--%>
        <f:Window ID="Update_Menu" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="650px" Height="360" Title="修改">
        </f:Window>
        <f:Window ID="Add_Menu" runat="server" Hidden="true"
            WindowPosition="Center" IsModal="true" Title="Popup Window 1" EnableMaximize="true"
            EnableResize="true" Target="Top" EnableIFrame="true"
            Height="360" Width="650px">
        </f:Window>
    </form>
</body>
</html>
