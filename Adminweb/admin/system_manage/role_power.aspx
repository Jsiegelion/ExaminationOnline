<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_power.aspx.cs" Inherits="Mammothcode.Demo.Adminweb.admin.system_manage.role_power" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        ul.powers {
            margin: 0;
            padding: 0;
        }

            ul.powers li {
                margin: 5px 15px 5px 0;
                display: inline-block;
                min-width: 150px;
            }

                ul.powers li input {
                    vertical-align: middle;
                }

                ul.powers li label {
                    margin-left: 5px;
                }

        /* 自动换行，放置权限列表过长 */
        .x-grid-row .x-grid-cell-inner {
            white-space: normal;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" AjaxLoadingType="Mask"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Margins="0 0 0 0" Width="200px" Position="Left" Layout="Fit" BodyPadding="5px" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="R_CODE" AllowSorting="true" OnSort="Grid1_Sort"
                            SortField="Name" SortDirection="DESC" AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="R_NAME" SortField="AD_NAME" ExpandUnusedSpace="true" HeaderText="角色名称"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit" BodyPadding="5px" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableMultiSelect="true" EnableCheckBoxSelect="false" DataKeyNames="P_CODE,P_NAME"
                            EnableHeaderMenu="false" AllowPaging="false" OnRowDataBound="Grid2_RowDataBound">
                            <%--OnRowDataBound="Grid2_RowDataBound"--%>
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnSelectAll" EnablePostBack="false" runat="server" Text="全选">
                                        </f:Button>
                                        <f:Button ID="btnUnSelectAll" EnablePostBack="false" runat="server" Text="反选">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button ID="btnGroupUpdate" Icon="GroupEdit" runat="server" Text="更新当前角色的权限" OnClick="btnGroupUpdate_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GroupName" SortField="GroupName" HeaderText="分组名称" Width="120px"></f:BoundField>
                                <f:TemplateField ExpandUnusedSpace="true" ColumnID="Powers" HeaderText="权限列表">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="ddlPowers" CssClass="powers" RepeatLayout="UnorderedList" RepeatDirection="Vertical" runat="server">
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Menu ID="Menu2" runat="server">
            <f:MenuButton ID="menuSelectRows" EnablePostBack="false" runat="server" Text="全选行">
            </f:MenuButton>
            <f:MenuButton ID="menuUnselectRows" EnablePostBack="false" runat="server" Text="取消行">
            </f:MenuButton>
        </f:Menu>
    </form>
</body>
</html>

<script src="../../Content/jquery.1.8.3.js"></script>
<script>
    var grid2ID = '<%= Grid2.ClientID %>';
    var btnSelectAll = '<%= btnSelectAll.ClientID %>';
    var btnUnSelectAll = '<%= btnUnSelectAll.ClientID %>';

    var menuID = '<%= Menu2.ClientID %>';
    var menuSelectRows = '<%= menuSelectRows.ClientID %>';
    var menuUnselectRows = '<%= menuUnselectRows.ClientID %>';

    function createQtip() {
        Ext.select('.powers li span').each(function (el) {
            var qtip = el.getAttribute('data-qtip');
            el.select('input,label').set({ 'ext:qtip': qtip });
        });
    }

    F.ready(function () {
        var grid = F(grid2ID), gridDom = grid.el.dom;
        var checkboxSelector = '.powers li span input',
            selectedRowSelector = '.x-grid-row-selected',
            selectedRowCheckboxSelector = selectedRowSelector + ' ' + checkboxSelector;

        grid.addListener('viewready', function () {
            createQtip();
        });

        F(grid2ID).on('beforeitemcontextmenu', function (view, record, item, index, event) {
            F(menuID).showAt(event.getXY());
            event.stopEvent();
        });


        function selectCheckbox(checked) {
            var selectedRows = $(selectedRowSelector, gridDom);
            if (selectedRows.length) {
                $(selectedRowCheckboxSelector, gridDom).prop('checked', checked);
            } else {
                $(checkboxSelector, gridDom).prop('checked', checked);
            }
        }

        F(menuSelectRows).on('click', function () {
            selectCheckbox(true);
        });

        F(menuUnselectRows).on('click', function () {
            selectCheckbox(false);
        });


        F(btnSelectAll).on('click', function () {
            selectCheckbox(true);
        });
        F(btnUnSelectAll).on('click', function () {
            selectCheckbox(false);
        });

    });

    F.ajaxReady(function () {
        createQtip();
    });
</script>
