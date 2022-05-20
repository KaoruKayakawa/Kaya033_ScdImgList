Imports System.Linq
Imports System.Xml

Public Class _default
    Inherits System.Web.UI.Page

    Protected _SESSION_REP_DTTBL As String = Me.UniqueID + "_Rep_DtTbl"

    Protected _cond_chk As String
    Protected _cond_disp_chk As String
    Protected _cond_shocd_from As Long?
    Protected _cond_shocd_to As Long?
    Protected _cond_fnm_set As Dictionary(Of String, String)
    Protected _cond_pic As Dictionary(Of String, String)
    Protected _cond_keisai As Dictionary(Of String, String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Return
        End If

        initCtrl()
    End Sub

    Protected Sub initCtrl()
        lrl_capWnd.Text = SettingConfig.WindowCaption
        lrl_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()

        ddl_PicUnit.DataSource = db_SHOHIN_IMG_FILE.DDL_SIF_PIC_UNIT
        ddl_PicUnit.DataTextField = "SIF_PIC_UNIT"
        ddl_PicUnit.DataValueField = "SIF_PIC_UNIT"
        ddl_PicUnit.DataBind()

        Session(_SESSION_REP_DTTBL) = db_SHOHIN_IMG_FILE.EmptyTable_01
        setList()
    End Sub

    Protected Sub setList()
        Dim tbl1 As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        Dim tbl2 As DataTable = db_SHOHIN_IMG_FILE.EmptyTable_01
        Dim query1 = From sii In tbl1.AsEnumerable()
                     Group sii By scd = sii.Field(Of Long)("SSHM_SCD"),
                         sfnm = sii.Field(Of String)("SSHM_SFILENAME"),
                         img = sii.Field(Of Byte)("IMG"),
                         path_img = sii.Field(Of String)("PATH_IMG"),
                         dtl_img = sii.Field(Of Byte)("DTL_IMG"),
                         path_dtl_img = sii.Field(Of String)("PATH_DTL_IMG"),
                         kls_img = sii.Field(Of Byte)("KLS_IMG"),
                         path_kls_img = sii.Field(Of String)("PATH_KLS_IMG"),
                         unit = sii.Field(Of String)("SIF_PIC_UNIT"),
                         ymd = sii.Field(Of DateTime)("SIF_YMD")
                         Into grp1 = Group
                     Select tbl2.LoadDataRow(
                            New Object() {unit,
                                grp1.Min(Function(sii) sii.Field(Of Integer)("SSHM_HTCD")),
                                scd,
                                sfnm,
                                grp1.Max(Function(sii) sii.Field(Of Short)("SSHM_BUMONCD")),
                                grp1.Max(Function(sii) sii.Field(Of String)("SSHM_SHONAME")),
                                grp1.Max(Function(sii) sii.Field(Of String)("SSHM_JANCD1")),
                                grp1.Max(Function(sii) sii.Field(Of Short)("SSHM_SURYOSEIGEN")),
                                grp1.Max(Function(sii) sii.Field(Of Byte)("SSHM_KEISAIFLG")),
                                img,
                                path_img,
                                dtl_img,
                                path_dtl_img,
                                kls_img,
                                path_kls_img,
                                ymd
                            }, False)

        query1.ToArray()
        uc_ListPager_Item.Init_ListPager(New String() {rep_Header.ID}, tbl2, SettingConfig.ListRowCount)

        lrl_ymd.Text = If(tbl1.Rows.Count = 0, String.Empty, DirectCast(tbl1.Rows(0)("SIF_YMD"), DateTime).ToString("yyyy/MM/dd HH:mm"))
        btn_CsvOutput.Enabled = (tbl2.Rows.Count > 0)
    End Sub

    Protected Sub selectData()
        Session(_SESSION_REP_DTTBL) = db_SHOHIN_IMG_FILE.Select_01(_cond_chk, _cond_shocd_from, _cond_shocd_to, _cond_fnm_set, _cond_pic, _cond_keisai)

        lbl_picUnit_1.Text = _cond_disp_chk
        If _cond_shocd_from Is Nothing Then
            Me.lbl_shoCd_1.Text = "  "
        Else
            Me.lbl_shoCd_1.Text = _cond_shocd_from.Value.ToString("d")
        End If
        If _cond_shocd_to Is Nothing Then
            Me.lbl_shoCd_2.Text = "  "
        Else
            Me.lbl_shoCd_2.Text = _cond_shocd_to.Value.ToString("d")
        End If

        Dim fns As String = String.Empty
        For Each fs As String In _cond_fnm_set.Keys
            fns += "、" + fs
        Next
        If fns <> String.Empty Then
            fns = fns.Substring(1)
        End If
        Me.lbl_fnm_set.Text = fns

        Dim pics As String = String.Empty
        For Each pic As String In _cond_pic.Keys
            pics += "、" + pic
        Next
        If pics <> String.Empty Then
            pics = pics.Substring(1)
        End If
        Me.lbl_picDiv.Text = pics

        Dim kfgs As String = String.Empty
        For Each kfg As String In _cond_keisai.Keys
            kfgs += "、" + kfg
        Next
        If kfgs <> String.Empty Then
            kfgs = kfgs.Substring(1)
        End If
        Me.lbl_keisai.Text = kfgs

        setList()
    End Sub

    Protected Sub rep_Header_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType <> ListItemType.Item AndAlso e.Item.ItemType <> ListItemType.AlternatingItem Then
            Return
        End If

        Dim scd As Long = DirectCast(DataBinder.Eval(e.Item.DataItem, "SSHM_SCD"), Long)
        DirectCast(e.Item.FindControl("lbl_SSHM_SCD"), Label).Text = scd.ToString("d")

        Dim sfnm As String = DirectCast(DataBinder.Eval(e.Item.DataItem, "SSHM_SFILENAME"), String)
        DirectCast(e.Item.FindControl("lbl_SSHM_SFILENAME"), Label).Text = sfnm

        Dim lbl As Label = DirectCast(e.Item.FindControl("lbl_IMG"), Label)
        Select Case DirectCast(DataBinder.Eval(e.Item.DataItem, "IMG"), Byte)
            Case 0
                lbl.Text = "●"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_IMG"), String)
            Case 1
                lbl.Text = "〇"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_IMG"), String)
            Case Else
                lbl.Text = String.Empty
                lbl.ToolTip = String.Empty
        End Select

        lbl = DirectCast(e.Item.FindControl("lbl_DTL_IMG"), Label)
        Select Case DirectCast(DataBinder.Eval(e.Item.DataItem, "DTL_IMG"), Byte)
            Case 0
                lbl.Text = "●"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_DTL_IMG"), String)
            Case 1
                lbl.Text = "〇"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_DTL_IMG"), String)
            Case Else
                lbl.Text = String.Empty
                lbl.ToolTip = String.Empty
        End Select

        lbl = DirectCast(e.Item.FindControl("lbl_KLS_IMG"), Label)
        Select Case DirectCast(DataBinder.Eval(e.Item.DataItem, "KLS_IMG"), Byte)
            Case 0
                lbl.Text = "●"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_KLS_IMG"), String)
            Case 1
                lbl.Text = "〇"
                lbl.ToolTip = DirectCast(DataBinder.Eval(e.Item.DataItem, "PATH_KLS_IMG"), String)
            Case Else
                lbl.Text = String.Empty
                lbl.ToolTip = String.Empty
        End Select

        Dim tbl1 As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        Dim query1 = From sii In tbl1.AsEnumerable()
                     Where sii.Field(Of Long)("SSHM_SCD") = scd AndAlso sii.Field(Of String)("SSHM_SFILENAME") = sfnm
                     Select sii
                     Order By sii.Field(Of Integer)("SSHM_HTCD")

        Dim rep As Repeater = DirectCast(e.Item.FindControl("rep_Detail"), Repeater)
        rep.DataSource = query1.ToArray().CopyToDataTable()
        rep.DataBind()
    End Sub

    Protected Sub btn_Search_Click(sender As Object, e As EventArgs)
        _cond_chk = Me.ddl_PicUnit.SelectedValue
        _cond_disp_chk = Me.ddl_PicUnit.SelectedItem.Text

        If Me.tbx_shohinCd_from.Text = String.Empty Then
            _cond_shocd_from = Nothing
        Else
            _cond_shocd_from = Convert.ToInt64(Me.tbx_shohinCd_from.Text)
        End If

        If Me.tbx_shohinCd_to.Text = String.Empty Then
            _cond_shocd_to = Nothing
        Else
            _cond_shocd_to = Convert.ToInt64(Me.tbx_shohinCd_to.Text)
        End If

        _cond_fnm_set = New Dictionary(Of String, String)()
        If chk_fnm_0.Checked Then
            _cond_fnm_set.Add(chk_fnm_0.Text, chk_fnm_0.Attributes("Val"))
        End If
        If chk_fnm_1.Checked Then
            _cond_fnm_set.Add(chk_fnm_1.Text, chk_fnm_1.Attributes("Val"))
        End If

        _cond_pic = New Dictionary(Of String, String)()
        If chk_PIC.Checked Then
            _cond_pic.Add(chk_PIC.Text, chk_PIC.Attributes("DbCol"))
        End If
        If chk_DTL.Checked Then
            _cond_pic.Add(chk_DTL.Text, chk_DTL.Attributes("DbCol"))
        End If
        If chk_KLS.Checked Then
            _cond_pic.Add(chk_KLS.Text, chk_KLS.Attributes("DbCol"))
        End If

        _cond_keisai = New Dictionary(Of String, String)()
        If chk_keisai_0.Checked Then
            _cond_keisai.Add(chk_keisai_0.Text, chk_keisai_0.Attributes("DbVal"))
        End If
        If chk_keisai_1.Checked Then
            _cond_keisai.Add(chk_keisai_1.Text, chk_keisai_1.Attributes("DbVal"))
        End If
        If chk_keisai_3.Checked Then
            _cond_keisai.Add(chk_keisai_3.Text, chk_keisai_3.Attributes("DbVal"))
        End If

        selectData()
    End Sub

    Protected Sub btn_CsvOutput_Click(sender As Object, e As EventArgs)
        Dim tbl As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        Dim ls As List(Of String) = New List(Of String)(tbl.Rows.Count + 9)

        ls.Add("検証日時：　" + lrl_ymd.Text)
        ls.Add("抽出条件：　")
        ls.Add("　画像ユニット：　" + lbl_picUnit_1.Text)
        ls.Add("　商品：　" + lbl_shoCd_1.Text + "～" + lbl_shoCd_2.Text)
        ls.Add("　画像ファイルの設定：　" + lbl_fnm_set.Text)
        ls.Add("　画像種別：　" + lbl_picDiv.Text)
        ls.Add("　掲載：　" + lbl_keisai.Text)
        ls.Add("")
        ls.Add("店舗コード,部門コード,商品コード,JAN,数量制限,掲載フラグ,品名,画像ファイル,画,画(dtl),画(kls),,※ 画　〇：ファイルあり、　●：ファイルなし、　空白：対象外")

        Dim sb As StringBuilder = New StringBuilder(1000), pic_mark As String
        For Each row As DataRow In tbl.Rows
            sb.Length = 0

            sb.Append(DirectCast(row("SSHM_HTCD"), Integer).ToString("d"))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_BUMONCD"), Short).ToString("d"))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_SCD"), Long).ToString("d"))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_JANCD1"), String))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_SURYOSEIGEN"), Short).ToString("d"))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_KEISAIFLG"), Byte).ToString("d"))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_SHONAME"), String))
            sb.Append(",")
            sb.Append(DirectCast(row("SSHM_SFILENAME"), String))
            sb.Append(",")
            Select Case DirectCast(row("IMG"), Byte)
                Case 0
                    pic_mark = "●"
                Case 1
                    pic_mark = "〇"
                Case Else
                    pic_mark = String.Empty
            End Select
            sb.Append(pic_mark)
            sb.Append(",")
            Select Case DirectCast(row("DTL_IMG"), Byte)
                Case 0
                    pic_mark = "●"
                Case 1
                    pic_mark = "〇"
                Case Else
                    pic_mark = String.Empty
            End Select
            sb.Append(pic_mark)
            sb.Append(",")
            Select Case DirectCast(row("KLS_IMG"), Byte)
                Case 0
                    pic_mark = "●"
                Case 1
                    pic_mark = "〇"
                Case Else
                    pic_mark = String.Empty
            End Select
            sb.Append(pic_mark)

            ls.Add(sb.ToString())
        Next

        Dim fn As String = SettingConfig.WindowCaption + DateTime.Now.ToString("yyyyMMddHHmmss") + "）.csv"

        Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fn))
        Response.ContentType = "application/octet-stream"
        Dim enc As Encoding = Encoding.GetEncoding("Shift-JIS")

        For Each ln As String In ls
            Response.BinaryWrite(enc.GetBytes(ln + Environment.NewLine))
        Next

        Response.End()
    End Sub
End Class
