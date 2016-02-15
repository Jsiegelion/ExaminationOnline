<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.role" %>

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
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" 
                    BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID" 
                            AllowPaging="true" IsDatabasePaging="true"  OnRowCommand="Grid1_RowCommand" EnableHeaderMenu ="false"
                             OnPageIndexChange="Grid1_PageIndexChange">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" Text="添加角色">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="R_NAME" HeaderText="角色名称" DataSimulateTreeLevelField="TreeLevel" Width="120px" />
                                <%--<f:BoundField DataField="AD_REMARK" HeaderText="权限说明" Width="120px" />--%>
                                <f:BoundField ExpandUnusedSpace="true" />
                                <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" HeaderText="编辑"
                                    WindowID="Update_Role" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="/admin/system_manage/role_edit.aspx?id={0}"
                                    Width="50px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" HeaderText="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                            </Columns> 
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Update_Role" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableIFrame="true" IFrameUrl="about:blank"
            Height="200" Width="360" Title="修改">
        </f:Window>
        <f:Window ID="Add_Role" runat="server" Hidden="true"
            WindowPosition="Center" IsModal="true" Title="Popup Window 1"
            Target="Top" EnableIFrame="true"
            Height="200" Width="360">
            </f:Window>
    </form>
</body>
</html>
