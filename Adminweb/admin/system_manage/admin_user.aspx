<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_user.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.admin_user" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" 
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" EnableHeaderMenu="false"
                            DataKeyNames="ID" AllowPaging="true" IsDatabasePaging="true" 
                            OnPageIndexChange="Grid1_PageIndexChange" OnRowCommand="Grid1_RowCommand">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" Text="添加后台用户">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="A_NAME" HeaderText="用户账号" DataSimulateTreeLevelField="TreeLevel"
                                    Width="80px" />
                                <f:BoundField DataField="A_PHONE" HeaderText="用户手机" Width="120px" />
                                <f:BoundField DataField="A_TRUE_NAME" HeaderText="用户姓名" Width="120px" />
                                <f:BoundField ExpandUnusedSpace="true" />
                                <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" HeaderText="编辑"
                                    WindowID="Update_Admin_User" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="/admin/system_manage/admin_user_edit.aspx?id={0}"
                                    Width="50px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" HeaderText="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Update_Admin_User" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Height="350" Width="400" Title="修改">
        </f:Window>
        <f:Window ID="Add_Admin_User" runat="server" Hidden="true"
            WindowPosition="Center" IsModal="true" Title="Popup Window 1"
            EnableResize="true" Target="Top" EnableIFrame="true"
            Height="350" Width="400">
        </f:Window>
    </form>
</body>
</html>
