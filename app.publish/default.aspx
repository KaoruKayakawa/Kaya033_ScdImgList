<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="Kaya033_ScdImgList._default" %>

<%@ Register src="~/UserControl/ListPager.ascx" tagname="ListPager" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <%: System.Web.Optimization.Styles.Render("~/Styles/CSS") %>
        <%: System.Web.Optimization.Scripts.Render("~/Scripts/JS") %>
    </asp:PlaceHolder>
</head>
<body onresize="adjust_divListHeight();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <table style="width: 100%;">
            <colgroup>
                <col style="width: 100%;" />
            </colgroup>
            <tr>
                <td style="background-color: linen; border: solid 2px rosybrown; padding: 2px;">
                    <table style="background-color: rosybrown; color: white; width: 100%;">
                        <colgroup>
                            <col />
                            <col style="width: 20em;" />
                        </colgroup>
                        <tr>
                            <td style="padding: 2px 20px; font-size: 17px; font-weight: bold;">
                                <asp:Literal ID="lrl_capWnd" runat="server" />
                            </td>
                            <td style="text-align: right; padding-right: 20px; font-size: 11px;">Ver： <asp:Literal ID="lrl_version" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;"><td></td></tr>
            <tr>
                <td style="padding: 0 2em;">
                    <table style="width: 100%;">
                        <colgroup>
                            <col style="width: 8em;" />
                            <col style="width: 20em;" />
                            <col style="width: 8em;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td style="text-align: right; padding-right: 5px;">
                                画像ユニット：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_PicUnit" runat="server" Width="100%" AutoPostBack="false" Font-Names="'Meiryo UI', 'ＭＳ Ｐゴシック'" />
                                <asp:CustomValidator id="ddl_PicUnit_CustomValidator" runat="server" Enabled="true"
                                    ControlToValidate="ddl_PicUnit" Display="None" ValidationGroup="VarGrp1" ValidateEmptyText="true"
                                    ErrorMessage="選択して下さい" ClientValidationFunction="ddl_validateSelectValue" />
                                <ajaxToolkit:ValidatorCalloutExtender ID="ddl_PicUnit_CustomValidator_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                    runat="server" Enabled="true" TargetControlID="ddl_PicUnit_CustomValidator" />
                            </td>
                            <td style="text-align: right; padding-right: 5px;">
                                商品：
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_shohinCd_from" runat="server" Width="8em" MaxLength="12" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="tbx_shohinCd_from_FilteredTextBoxExtender" runat="server"
                                        TargetControlID="tbx_shohinCd_from" FilterType="Numbers" />
                                &nbsp;～&nbsp;
                                <asp:TextBox ID="tbx_shohinCd_to" runat="server" Width="8em" MaxLength="12" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="tbx_shohinCd_to_FilteredTextBoxExtender" runat="server"
                                        TargetControlID="tbx_shohinCd_to" FilterType="Numbers" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 2px;"><td></td></tr>
            <tr>
                <td style="padding: 0 2em;">
                    <table style="width: 100%;">
                        <colgroup>
                            <col style="width: 8em;" />
                            <col style="width: 30em;" />
                            <col style="width: 8em;" />
                            <col />
                            <col style="width: 6em;" />
                            <col style="width: 7em;" />
                        </colgroup>
                        <tr>
                            <td style="text-align: right; padding-right: 5px;">
                                画像種別：
                            </td>
                            <td>
					            <asp:CheckBox ID="chk_PIC" runat="server" Text="画" Checked="true" TextAlign="Right" DbCol="IMG" />
                                <asp:CheckBox ID="chk_DTL" runat="server" Text="画(dtl)" Checked="true" TextAlign="Right" Style="padding-left: 20px;" DbCol="DTL_IMG" />
                                <asp:CheckBox ID="chk_KLS" runat="server" Text="画(kls)" Checked="true" TextAlign="Right" Style="padding-left: 20px;" DbCol="KLS_IMG" />
                            </td>
                            <td style="text-align: right; padding-right: 5px;">
                                掲載：
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_keisai_0" runat="server" Checked="false" Text="非掲載(0)" TextAlign="Right" DbVal="0" />
                                <asp:CheckBox ID="chk_keisai_1" runat="server" Checked="true" Text="掲載(1)" TextAlign="Right" Style="padding-left: 20px;" DbVal="1" />
                                <asp:CheckBox ID="chk_keisai_3" runat="server" Checked="false" Text="隠れスポット(3)" TextAlign="Right" Style="padding-left: 20px;" DbVal="3"/>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_Search" runat="server" CssClass="btn01" Text="検 索" ValidationGroup="VarGrp1" OnClick="btn_Search_Click" />
                            </td>
                            <td style="text-align: right;">
                                <asp:UpdatePanel ID="up_2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_CsvOutput" runat="server" Enabled="false" CssClass="btn01" Text="CSV出力" OnClick="btn_CsvOutput_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btn_Search" EventName="Click" />
                                        <asp:PostBackTrigger ControlID="btn_CsvOutput" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        
        <asp:UpdatePanel ID="up_1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div style="margin-top: 5px; background-color: rosybrown; color: Black; border: solid 2px linen; padding: 2px;">
                    <table style="background-color: linen; width: 100%;">
                        <colgroup>
                            <col style="width: 100%;" />
                        </colgroup>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <colgroup>
                                        <col style="width: 7em;" />
                                        <col />
                                        <col style="width: 2em;" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3" style="padding-left: 1.5em;">
                                            [チェック日時：
											<asp:Literal ID="lrl_ymd" runat="server" />
                                            ]
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; padding-right: 2em;">条件</td>
                                        <td>
                                            [画像ユニット]&nbsp;&nbsp;<asp:Label ID="lbl_picUnit_1" runat="server" Font-Names="'Meiryo UI', 'ＭＳ Ｐゴシック'" Text="  " />
                                            ,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            [商品]&nbsp;&nbsp;<asp:Label ID="lbl_shoCd_1" runat="server" Text="  " />
                                            &nbsp;～&nbsp;
                                            <asp:Label ID="lbl_shoCd_2" runat="server" Text="&nbsp;&nbsp;" />
                                            ,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            [画像種別]&nbsp;&nbsp;<asp:Label ID="lbl_picDiv" runat="server" Text="&nbsp;&nbsp;" />
                                            ,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            [掲載]&nbsp;&nbsp;<asp:Label ID="lbl_keisai" runat="server" Text="  " />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 2px;"><td></td></tr>
                        <tr>
                            <td style="background-color: lightgrey;">
                                <asp:Repeater id="rep_Header" runat="server" OnItemDataBound="rep_Header_ItemDataBound">
                                    <HeaderTemplate>
                                        <div style="overflow-x: hidden; overflow-y: scroll; width: 100%;">
                                            <table id="tbl_List_Header" style="table-layout: fixed; border-collapse: separate; border-spacing: 0; width: 100%; background-color: Gray; overflow-wrap: break-word;">
                                                <colgroup>
                                                    <col style="width: 100%;" />
                                                </colgroup>
                                                <tr>
                                                    <td style="padding: 0;">
                                                        <table style="table-layout: fixed; border-collapse: separate; border-spacing: 1px; width: 100%; background-color: Gray;">
                                                            <colgroup>
                                                                <col style="width: 6%;" />
                                                                <col style="width: 6%;" />
                                                                <col style="width: 6%;" />
                                                                <col style="width: 8%;" />
                                                                <col style="width: 12%;" />
                                                                <col style="width: 6%;" />
                                                                <col style="width: 6%;" />
                                                                <col />
                                                                <col style="width: 12%;" />
                                                                <col style="width: 6%;" />
                                                                <col style="width: 6%;" />
                                                                <col style="width: 6%;" />
                                                            </colgroup>
                                                            <tr>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;"></th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">店</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">部門</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">商品</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">JAN</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">制限</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">掲載</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">品名</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">ファイル名</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">画</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">画(dtl)</th>
                                                                <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">画(kls)</th>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_List" style="height: 0; overflow-x: hidden; overflow-y: scroll; width: 100%;">
                                            <table id="tbl_List_Item" style="table-layout: fixed; border-collapse: separate; border-spacing: 0; width: 100%; background-color: Gray; overflow-wrap: break-word;">
                                                <colgroup>
                                                    <col style="width: 100%;" />
                                                </colgroup>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                                <tr>
                                                    <td style="padding: 0;">
                                                        <asp:Panel ID="pnl_Item_Header" runat="server" Width="100%">
                                                            <table style="table-layout: fixed; border-collapse: separate; border-spacing: 1px; width: 100%; background-color: Gray;">
                                                                <colgroup>
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 8%;" />
                                                                    <col style="width: 12%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col />
                                                                    <col style="width: 12%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 6%;" />
                                                                    <col style="width: 6%;" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td style="background-color: lightsteelblue; text-align: center; font-weight: bold;">
                                                                        <div style="cursor: pointer;">
                                                                            <asp:Label ID="lbl_Item_Header" runat="server" Width="100%" />
                                                                        </div>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_HTCD")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_BUMONCD")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <asp:Label ID="lbl_SSHM_SCD" runat="server" />
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_JANCD1")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_SURYOSEIGEN")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_KEISAIFLG")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: left; padding: 3px;">
                                                                        <%# DataBinder.Eval(Container.DataItem, "SSHM_SHONAME")%>
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <asp:Label ID="lbl_SSHM_SFILENAME" runat="server" />
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <asp:Label ID="lbl_IMG" runat="server" />
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <asp:Label ID="lbl_DTL_IMG" runat="server" />
                                                                    </td>
                                                                    <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                        <asp:Label ID="lbl_KLS_IMG" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnl_Item_Detail" runat="server" Width="100%" Height="0px" Style="background-color: lavender;">
                                                            <asp:Repeater id="rep_Detail" runat="server">
                                                                <HeaderTemplate>
                                                                    <table style="table-layout: fixed; border-collapse: separate; border-spacing: 0; width: 100%;">
                                                                        <colgroup>
                                                                            <col style="width: 100%;" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td style="padding: 0;">
                                                                                <table style="table-layout: fixed; border-collapse: separate; border-spacing: 1px; width: 100%; background-color: Gray;">
                                                                                    <colgroup>
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 8%;" />
                                                                                        <col style="width: 12%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col />
                                                                                        <col style="width: 12%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 6%;" />
                                                                                        <col style="width: 6%;" />
                                                                                    </colgroup>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td></td>
                                                                                        <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_HTCD")%>
                                                                                        </td>
                                                                                        <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_BUMONCD")%>
                                                                                        </td>
                                                                                        <td></td>
                                                                                        <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_JANCD1")%>
                                                                                        </td>
                                                                                        <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_SURYOSEIGEN")%>
                                                                                        </td>
                                                                                        <td style="color: black; background-color: white; text-align: center; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_KEISAIFLG")%>
                                                                                        </td>
                                                                                        <td style="color: black; background-color: white; text-align: left; padding: 3px;">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "SSHM_SHONAME")%>
                                                                                        </td>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>
                                                        <ajaxToolkit:CollapsiblePanelExtender ID="cpe_Item" runat="server" Enabled="True"
                                                            TargetControlID="pnl_Item_Detail" ExpandControlID="lbl_Item_Header" CollapseControlID="lbl_Item_Header" 
                                                            Collapsed="true" CollapsedText="（開く）" ExpandedText="（閉る）" TextLabelID="lbl_Item_Header" />
                                                    </td>
                                                </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </table>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="table-layout: fixed; border-collapse: separate; border-spacing: 0; width: 100%;">
                                    <colgroup>
                                        <col style="width: 50%;" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <div style="position: relative; width: 95%;">
                                                <div style="position: absolute; right: 0; top: 1em; width: 20em; background-color: palegoldenrod; border: 1px solid Gray;">
                                                    <uc1:ListPager ID="uc_ListPager_Item" runat="server" />
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="position: relative; width: 95%;">
                                                <div style="position: absolute; left: 5%; top: 1.5em; width: 100%; color: indianred; font-size: 1.1em; text-align: right;">
                                                    <span style="padding-right: 7em;">※ 画　〇：ファイルあり、　●：ファイルなし、　空白：対象外</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_Search" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="uc_ListPager_Item" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
    
    <script type="text/javascript">
    <!--
        var mng;
        function pageLoad() {
            mng = Sys.WebForms.PageRequestManager.getInstance();

            mng.remove_initializeRequest(initializeRequest_mng);
            mng.add_initializeRequest(initializeRequest_mng);

            mng.remove_endRequest(endRequest_mng);
            mng.add_endRequest(endRequest_mng);

            adjust_divListHeight();
        }
        function initializeRequest_mng(sender, args) {
            if (mng.get_isInAsyncPostBack()) {
                args.set_cancel(true);

                return;
            }

            $.sanIndicator();
        }
        function endRequest_mng(sender, args) {
            $.sanIndicator.hide();
        }

        function adjust_divListHeight() {
            var height = document.documentElement.clientHeight - 280;
            if (height < 0) {
                height = 0;
            }

            document.getElementById('div_List').style.height = height + 'px';
        }

        function ddl_validateSelectValue(oSrc, args) {
            args.IsValid = (args.Value.toString() != "");
        }
    //-->
    </script>
</body>
</html>
