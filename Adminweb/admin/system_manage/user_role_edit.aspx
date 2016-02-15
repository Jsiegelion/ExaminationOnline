<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_role_edit.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.user_role_edit" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
         Layout="Fit">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="选择后关闭">
                    </f:Button>
                    <f:ToolbarFill runat="server">
                    </f:ToolbarFill>
                    <f:TwinTriggerBox ID="ttbSearchMessage" Width="160px" runat="server" ShowLabel="false"
                        EmptyText="在用户名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false"
                        OnTrigger2Click="ttbSearchMessage_Trigger2Click" OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                    </f:TwinTriggerBox>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true"
                 DataKeyNames="ID,A_CODE,A_Name" 
                 SortField="Name" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                OnPageIndexChange="Grid1_PageIndexChange" ClearSelectedRowsAfterPaging="false">
                <Columns>
                    <f:RowNumberField />
                    <f:BoundField DataField="A_NAME" SortField="Name" Width="100px" HeaderText="用户名" />
                    <f:BoundField DataField="A_TRUE_NAME" SortField="RealName" Width="100px" HeaderText="中文名" />
                    <%--<f:CheckBoxField DataField="AU_ENABLED" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true"
                        Width="50px" />--%>
                    <f:BoundField DataField="A_PHONE" SortField="Gender" Width="150px" HeaderText="手机" />
                                         <f:BoundField ExpandUnusedSpace="true" />
<%--                    <f:BoundField DataField="A_EMAIL" SortField="Email" Width="150px" HeaderText="邮箱" />
                    <f:BoundField DataField="AU_REMARK" ExpandUnusedSpace="true" HeaderText="备注" />--%>
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </f:ToolbarSeparator>
                   <%-- <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                    </f:ToolbarText>
                    <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                        runat="server">
                        <f:ListItem Text="10" Value="10" />
                        <f:ListItem Text="20" Value="20" />
                        <f:ListItem Text="50" Value="50" />
                        <f:ListItem Text="100" Value="100" />
                    </f:DropDownList>--%>
                </PageItems>
            </f:Grid>
        </Items>
    </f:Panel>
    <f:HiddenField ID="hfSelectedIDS" runat="server">
    </f:HiddenField>
    </form>
</body>
</html>

