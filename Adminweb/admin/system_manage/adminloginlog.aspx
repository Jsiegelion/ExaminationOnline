<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminloginlog.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.adminloginlog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID"
                            AllowPaging="true" IsDatabasePaging="true" EnableHeaderMenu="false"
                            OnPageIndexChange="Grid1_PageIndexChange">
                            <Toolbars>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="A_NAME" HeaderText="管理员用户名" DataSimulateTreeLevelField="TreeLevel"
                                    Width="150px" />
                                <f:BoundField DataField="A_TRUE_NAME" HeaderText="管理员真实姓名姓名" Width="150px" />
                                <f:BoundField DataField="IP" HeaderText="登入IP" Width="170px" />
                                <f:BoundField DataField="CREATE_TIME" HeaderText="登入时间" Width="150px" />
                                <f:BoundField ExpandUnusedSpace="true" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
