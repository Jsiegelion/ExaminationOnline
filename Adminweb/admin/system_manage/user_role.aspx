<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_role.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.admin_user_manage" %>

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
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" 
                    Margins="0 0 0 0" Width="200px" Position="Left" Layout="Fit" BodyPadding="5px" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" 
                            DataKeyNames="ID,R_CODE"  SortField="Name" SortDirection="DESC" 
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="R_NAME" SortField="R_NAME" ExpandUnusedSpace="true" 
                                    HeaderText="角色名称"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" 
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
       <%--                 <f:Form ID="Form3" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                            <Rows>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TwinTriggerBox ID="ttbSearchUser" runat="server" ShowLabel="false" EmptyText="在用户名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchUser_Trigger2Click" OnTrigger1Click="ttbSearchUser_Trigger1Click">
                                        </f:TwinTriggerBox>
                                        <f:Label runat="server">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>--%>
                        <f:Grid ID="Grid2" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID,A_CODE,A_NAME" AllowSorting="true" OnSort="Grid2_Sort" SortField="Name" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid2_PreDataBound" OnRowCommand="Grid2_RowCommand" OnPageIndexChange="Grid2_PageIndexChange">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="从当前角色移除选中的用户" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" OnClick="btnNew_Click" Text="添加用户到当前角色">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <PageItems>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="A_NAME"  Width="150px" HeaderText="用户名"></f:BoundField>
                                <f:BoundField DataField="A_TRUE_NAME" SortField="ChineseName" Width="100px" HeaderText="中文名"></f:BoundField>
                                <f:BoundField DataField="A_PHONE" SortField="Gender" Width="150px" HeaderText="手机"></f:BoundField>
                                <f:BoundField ExpandUnusedSpace="true" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="从当前角色中移除此用户" ConfirmText="确定从当前角色中移除此用户？" ConfirmTarget="Top" CommandName="Delete" Width="50px"></f:LinkButtonField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="650px" Height="450px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
