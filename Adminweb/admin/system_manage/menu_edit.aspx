<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_edit.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.menu_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
            <%--头部--%>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" runat="server"
                            Text="关闭" OnClick="btnClose_Click">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose"
                            runat="server" Text="保存后关闭" OnClick="btnSaveClose_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <%--内容--%>
            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                    BodyPadding="10px" Title="SimpleForm">
                    <Items>
                        <f:TextBox ID="tbxAM_NAME" runat="server" Label="菜单名称" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <%--<f:TextBox ID="tbxAM_IMAGE_URL" runat="server" Label="菜单图片地址"></f:TextBox>--%>
                        <f:TextBox ID="tbxAM_NAVIGATE_URL" runat="server" Label="菜单导航地址">
                        </f:TextBox>
                        <f:TextBox ID="tbxAM_REMARK" runat="server" Label="菜单备注">
                        </f:TextBox>
                        <f:TextBox ID="tbxAM_SORTINDEX" runat="server" Label="菜单排序" ShowRedStar="true" Required="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxVIEWPOWER_ID" runat="server" Label="菜单权限名称" ShowRedStar="true">
                        </f:TextBox>
                        <%-- <f:TextBox ID="tbxPARENT_ID" runat="server" Label="菜单上级节点" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxAM_ISTREELEAF" runat="server" Label="是否是叶子节点" ShowRedStar="true">
                        </f:TextBox>--%>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
