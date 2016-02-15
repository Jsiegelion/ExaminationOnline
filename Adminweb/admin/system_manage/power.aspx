<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="power.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.power" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <f:PageManager ID="PageManager2" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID,P_CODE" AllowSorting="true"  
                            SortField="Name" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" EnableHeaderMenu ="false" EnableRowDoubleClickEvent="true"
                            OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange"  OnRowDoubleClick="Grid1_RowClick">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                     <Items>
                                      <f:Button ID="back" runat="server" EnablePostBack="true" Text="返回上一层" OnClick="back_Click">
                                        </f:Button>
                                    </Items>
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" Text="添加权限">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                              <Columns>
                               <f:RowNumberField />
                                <f:BoundField DataField="P_NAME" HeaderText="权限名称" DataSimulateTreeLevelField="TreeLevel"
                                    Width="200px" />
                                <f:BoundField DataField="P_CHINESE_NAME" HeaderText="权限中文名" DataSimulateTreeLevelField="TreeLevel"
                                    Width="150px" />
                                <f:BoundField ExpandUnusedSpace="true" />
                                <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" HeaderText="编辑"
                                    WindowID="Update_Demo" Title="编辑" DataIFrameUrlFields="ID,FATHER_CODE" DataIFrameUrlFormatString="/admin/system_manage/power_edit.aspx?id={0}&fathercode={1}"
                                    Width="50px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" HeaderText="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Update_Demo" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableIFrame="true" IFrameUrl="about:blank"
            Height="200" Width="360" Title="修改">
        </f:Window>
        <f:Window ID="Add_Demo" runat="server" Hidden="true"
            WindowPosition="Center" IsModal="true" Title="Popup Window 1"
            Target="Top" EnableIFrame="true"
            Height="200" Width="360">
        </f:Window>
    </form>
</body>
</html>
