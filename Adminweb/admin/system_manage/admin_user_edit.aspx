<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_user_edit.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.admin_user_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                        <f:Button ID="btnClose" Icon="SystemClose" runat="server" Text="关闭" OnClick="btnClose_Click">
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
                        <f:TextBox ID="tbxA_NAME" runat="server" Label="用户名" Required="true"></f:TextBox>
                        <%--密码--%>
                        <f:TextBox ID="tbxPassword" runat="server" Label="密码" Required="true" TextMode="Password"></f:TextBox>
                        <%--确认密码--%>
                        <f:TextBox ID="tbxCfm_Password" runat="server" Label="确认密码" Required="true" CompareControl="tbxPassword" TextMode="Password"
                            CompareOperator="Equal" CompareMessage="两次密码应该相同">
                        </f:TextBox>
                        <f:TextBox ID="tbxA_CHINESE_NAME" runat="server" Label="用户中文名" Required="true"></f:TextBox>
                        <f:TextBox ID="tbxPhone" runat="server" Label="手机号"></f:TextBox>
                        <f:Form runat="server" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:RadioButton ID="rbtnFirst" Label="用户性别" Checked="true" GroupName="MyRadioGroup1" Text="男" runat="server" AutoPostBack="true">
                                        </f:RadioButton>
                                        <f:RadioButton ID="rbtnSecond" GroupName="MyRadioGroup1" Text="女" runat="server" AutoPostBack="true">
                                        </f:RadioButton>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
