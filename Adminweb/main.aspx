<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Content/include/css/manage.css" rel="stylesheet" />
    <title>后台系统</title>
      <!--声音消息提醒-->
    <link rel="shortcut icon" href="../favicon.ico" type="image/x-icon" />
<%--<script src="https://rawgit.com/jaywcjlove/iNotify/master/src/iNotify.js"></script>--%>
<%--    <script src="Content/include/js/iNotify.js"></script>--%>
    <!--消息推送-->
    <link href="/Content/include/css/windowTips.css" rel="stylesheet" />
    <script src="/Content/jquery.1.8.3.js"></script>
    <script src="/Content/include/js/jquery.signalR-2.2.0.js"></script>
    <script src="/Content/include/js/windowTips.js"></script>
    <script src="http://101.201.223.121:8899/signalr/hubs"></script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="regionPanel" runat="server" />
        <f:RegionPanel ID="regionPanel" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="regionTop" ShowBorder="false" ShowHeader="false" Position="Top"
                    Layout="Fit" runat="server">
                    <Items>
                        <f:ContentPanel ShowBorder="false" CssClass="jumbotron" ShowHeader="false" runat="server">
                            <div class="head">
                                <img src="Content/img/pic_header_logo.png" alt="后台管理系统" />
                                <%--                                <div class="head_user">
                                    <span class="label" style="float: left;">
                                        <f:ToolbarText ID="txtUser" runat="server">
                                        </f:ToolbarText>
                                    </span>
                                    <f:ToolbarSeparator runat="server" />
                                </div>--%>
                                <%--<div class="head_user">
                                    <f:ToolbarSeparator runat="server" />
                                </div>--%>
                                <div class="head_right">
                                    <div class="r_top">
                                        <div style="display: none;">
                                            <f:ToolbarText ID="txtChineseName" runat="server">
                                            </f:ToolbarText>
                                            <f:ToolbarSeparator runat="server" />
                                        </div>
                                        <div class="head_time">
                                            <f:ToolbarText ID="txtCurrentTime" runat="server"></f:ToolbarText>
                                            <f:ToolbarText ID="txtUser" Text="maofeng" runat="server"></f:ToolbarText>
                                            <f:ToolbarFill runat="server" />
                                        </div>
                                    </div>
                                    <div class="r_bottom">
                                        <div>
                                            <f:Button ID="btnRefresh" runat="server" Icon="Reload" ToolTip="刷新主区域内容" EnablePostBack="false">
                                            </f:Button>
                                            <f:ToolbarSeparator runat="server" />
                                        </div>
                                        <div>
                                            <f:Toolbar runat="server"></f:Toolbar>
                                        </div>
                                        <div>
                                            <f:Button ID="btnExit" runat="server" Icon="UserRed" Text="安全退出" OnClick="btnExit_Click"></f:Button>
                                            <%--  OnClick="btnExit_Click" --%>
                                        </div>
                                        <div>
                                            <f:Button ID="btnUpdate" runat="server" Icon="Pencil" Text="修改密码" OnClick="btnUpdate_OnClick"></f:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="regionLeft" Split="true" EnableCollapse="true" Width="200px"
                    ShowHeader="true" Title="系统菜单" Layout="Fit" Position="Left" runat="server">
                </f:Region>
                <f:Region EnableCollapse="true" ShowHeader="false" Layout="Fit" Position="Bottom" runat="server">
                    <Items>
                        <f:ContentPanel ShowBorder="false" CssClass="jumbotron" ShowHeader="false" runat="server">
                            <div class="bottom">杭州曼码科技有限公司</div>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Position="Center" IFrameName="mainPage"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="首页" EnableIFrame="true" IFrameUrl="/admin/default.aspx"
                                    Icon="House" runat="server">
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" EnableIFrame="true"
            EnableResize="true" EnableMaximize="true" IFrameUrl="about:blank" Width="650px"
            Height="450px">
        </f:Window>
         <f:Window runat="server" ID="updateWindow" Title="修改密码" EnableResize="true" EnableMaximize="true" Width="360" Height="240" Hidden="true">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:HiddenField runat="server" ID="HF_WE_CODE"></f:HiddenField>
                        <f:Button runat="server" Text="保存" ID="btnSave" ValidateForms="panel5" OnClick="btnSave_OnClick"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="panel5" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="30px 0 0 20px ">
                    <Items>
                        <%--原密码--%>
                        <f:TextBox ID="tbxOriPassword" runat="server" Label="原密码" Required="true" TextMode="Password"></f:TextBox>
                        <%--密码--%>
                        <f:TextBox ID="tbxPassword" runat="server" Label="密码" Required="true" TextMode="Password"></f:TextBox>
                        <%--确认密码--%>
                        <f:TextBox ID="tbxCfm_Password" runat="server" Label="确认密码" Required="true" CompareControl="tbxPassword" TextMode="Password"
                            CompareOperator="Equal" CompareMessage="两次密码应该相同">
                        </f:TextBox>
                    </Items>
                </f:Panel>
            </Items>
        </f:Window>
    </form>
         <div id="pop" style="display: none;">
        <div id="popHead">
            <a id="popClose" title="关闭">关闭</a>
            <img src="Content/img/message.png" />
            <h2>最新消息</h2>
        </div>
        <div id="popContent">
            <dl>
                <dt id="popTitle"></dt>
                <dd id="popIntro"></dd>
            </dl>
        </div>
    </div>
    <script src="res/js/main.js" type="text/javascript"></script>
</body>
<script type="text/javascript">

    $(function () {

               
                 $.connection.hub.url = "http://101.201.223.121:8899/signalr";
                 var handler = $.connection.xiuchewenwenHub;// 生成客户端hub代理
                 // 添加客户端hub方法以供服务端调用
                 // 向列表中添加航班信息
                 handler.client.showMessage = function (msg) {
                     windowTips(null, null, msg.content);
                     messageSound(msg.typecode, msg.content);
                 }

                 //日志输出，以备调试使用
                 $.connection.hub.logging = true;
                 // 开启hub连接
                 $.connection.hub.start();
             });

             //声音消息提醒
             //创建人：孙佳杰  创建时间:2015.8.5
    function messageSound(type,message) {
        if (type == "CommonMemberRegister") {
            inotifyCommonMemberRegister(message);
        } else if (type == "RepairFactoryMemberRegister") {
            inotifyRepairFactoryMemberRegister(message);
        }
    }
    //http://jslite.io/iNotify/#实例六

    //普通用户注册的消息
    var iN = new iNotify().init()
    var iN = new iNotify({
        effect: 'flash',
        interval: 500,
        message: "有消息拉",
        audio: {
            file: [ 'msg1.mp3']
        },
        notification: {
            title: "通知！",
            body: '您来了一条新消息'
        }
    });

    //修理厂用户注册的消息
    var iN1 = new iNotify().init()
    var iN1 = new iNotify({
        effect: 'flash',
        interval: 500,
        message: "有消息拉",
        audio: {
            file: ['msg2.mp3']
        },
        notification: {
            title: "通知！",
            body: '您来了一条新消息'
        }
    });

    //普通用户注册的消息
    function inotifyCommonMemberRegister(value) {
        iN.setFavicon(10).setTitle('普通用户注册新通知').notify({
            title: "普通用户注册新通知",
            body: value
        })
        iN.player();
    }
    //修理厂用户注册的消息
    function inotifyRepairFactoryMemberRegister(value) {
        iN1.setFavicon(10).setTitle('修理厂用户注册新通知').notify({
            title: "修理厂用户注册新通知",
            body: value
        })
        iN1.player();
    }

</script>
</html>
