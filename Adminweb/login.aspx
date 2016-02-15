<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.login" %>

<script>
    if (self !== top) {
        top.location.href = window.location.href;
    }
</script>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <!--主样式文件-->
    <link href="/res/css/mammothweb.min.css" rel="stylesheet" />
    <!--jquery-->
    <script type="text/javascript" src="http://libs.baidu.com/jquery/1.8.3/jquery.min.js"></script>
    <title>登录</title>
    <style>
        html {
            height: 100%;
            background: #2d5465 url("/Content/img/login-bg.png") center center no-repeat;
        }
    </style>
</head>
<body>
    <form>
        <div class="login-wrap">
            <div class="logo">
                <img src="/Content/img/login_bottom_logo.png" style="height: auto;" />
            </div>
            <div class="box">
                <div class="top-walcome">
                    <h1>欢迎回来！</h1>
                    <h2>输入以下信息登录到修车问问后台</h2>
                </div>
                <ul class="login-fild">
                    <li class="item">
                        <div class="tip"></div>
                    </li>
                    <li class="item">
                        <input name="txtUserName" type="text" id="txtUserName" class="inline-input login-field" placeholder="用户名" title="用户名" />
                    </li>
                    <li class="item">
                        <div class="tip"></div>
                    </li>
                    <li class="item">
                        <input name="txtPassword" type="password" id="txtPassword" class="inline-input login-field" placeholder="密码" title="密码" />
                    </li>
                    <li class="item"></li>
                    <li class="item">
                        <div class="login-btn" id="btnSubmit">登录</div>
                    </li>
                </ul>
                <div class="bottom-logo"></div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(function () {
            $('#btnSubmit').click(function () {
                login();
            });
            //去除JS代码
            String.prototype.trim = function () {
                return this.replace(/(^\s*)|(\s*$)/g, "");
            }

            //回车事件
            //使用回车进行搜索
            $("#txtPassword").keyup(function (e) {
                var code = event.keyCode;
                if (13 == code) {
                    login();
                }
            });
        });

        //修改人：金协民 2015年9月14日08:10:22 将登录方法独立出去
        function login() {
            var username = $("#txtUserName").val();
            var password = $("#txtPassword").val();
            var isSaveAccount = $("#isCheckAccount").attr("class") === "icon-check" ? true : false;
            if (username.trim() === "" || password.trim() === "") {
                alert("用户名，密码错误！！");
            }
            var act = "userlogin";
            $.ajax({
                type: 'POST',
                url: '/login.aspx',
                data: { "username": username, "password": password, "isSaveAccount": isSaveAccount, "act": act },
                success: function (data) {
                    setTimeout(function () {
                        if (data !== "/") {
                            location.href = data;
                        }
                        else {
                            $("#btnSubmit").attr("value", "登陆");
                            alert("用户名，密码错误！！");
                            $("#txtUserName").attr("value", "");
                            $("#txtPassword").attr("value", "");
                        }
                        $("#btnSubmit").html("登录");
                    }, 1000);
                },
                //发送请求前触发
                beforeSend: function (xhr) {
                    $("#btnSubmit").html("登录中...");
                },
                //是否使用异步发送
                async: true,
                //timeOut超时设置
                //修改：金协民 时间：2015年11月16日
                timeout: 60000,
                error: function () {
                    alert("登录超时！");
                }
            });
        }
    </script>
</body>
</html>
