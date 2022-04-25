Imports System.Drawing

Public Class ListPager
    Inherits System.Web.UI.UserControl

    Protected _SESSION_REP_CONTROLID As String
    Protected _SESSION_REP_DTTBL As String
    Protected _SESSION_PAGE_CNT As String
    Protected _SESSION_PAGE_LINECNT As String
    Protected _SESSION_PAGE_CURRENT As String

    Public ReadOnly Property CurrentPageNo As Integer
        Get
            Return DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer)
        End Get
    End Property

    Public ReadOnly Property CurrentPosition As Integer
        Get
            Return DirectCast(Me.Session(_SESSION_PAGE_LINECNT), Integer) * DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer)
        End Get
    End Property

    Public ReadOnly Property CtrlID_tbx_PageNo As String
        Get
            Return Me.tbx_PageNo.ID
        End Get
    End Property

    Public ReadOnly Property DataSource As DataTable
        Get
            Return DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        End Get
    End Property

    Public Function GetDataRow(idx As Integer) As DataRow
        Dim tbl As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)

        Return tbl.Rows(CurrentPosition + idx)
    End Function

    Public Sub Init_ListPager(id_ctrl As String(), tbl As DataTable, pageLineCnt As Integer)
        Me.Session(_SESSION_REP_CONTROLID) = id_ctrl
        Me.Session(_SESSION_REP_DTTBL) = tbl
        Me.Session(_SESSION_PAGE_CNT) = Convert.ToInt32(Math.Ceiling(tbl.Rows.Count / pageLineCnt))
        Me.Session(_SESSION_PAGE_LINECNT) = pageLineCnt

        Me.lrl_PageCnt.Text = DirectCast(Me.Session(_SESSION_PAGE_CNT), Integer).ToString("d")

        TurnPage(0)
    End Sub

    Public Sub ReDraw_List()
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer))
    End Sub

    Public Sub TurnPage(pageNo As Integer)
        Dim pageCnt As Integer = DirectCast(Me.Session(_SESSION_PAGE_CNT), Integer)
        Dim pageLineCnt As Integer = DirectCast(Me.Session(_SESSION_PAGE_LINECNT), Integer)

        If pageNo >= pageCnt Then
            pageNo = pageCnt - 1
        End If

        If pageNo > 0 Then
            Me.btn_ToPrev.Enabled = True
            Me.btn_ToPrev.ForeColor = Color.Blue
        Else
            Me.btn_ToPrev.Enabled = False
            Me.btn_ToPrev.ForeColor = Color.Gray
        End If
        If pageNo < pageCnt - 1 Then
            Me.btn_ToNext.Enabled = True
            Me.btn_ToNext.ForeColor = Color.Blue
        Else
            Me.btn_ToNext.Enabled = False
            Me.btn_ToNext.ForeColor = Color.Gray
        End If
        Me.tbx_PageNo.Text = (pageNo + 1).ToString("d")

        Dim tbl As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        Dim tblDs As DataTable

        If pageCnt = 0 Then
            tblDs = tbl
        Else
            Dim ls_row_01(tbl.Rows.Count) As DataRow
            tbl.Rows.CopyTo(ls_row_01, 0)

            Dim pos As Integer = pageLineCnt * pageNo
            Dim cnt As Integer = tbl.Rows.Count - pos
            If cnt > pageLineCnt Then
                cnt = pageLineCnt
            End If

            Dim ls_row_02(cnt) As DataRow
            Array.Copy(ls_row_01, pos, ls_row_02, 0, cnt)

            tblDs = ls_row_02.CopyToDataTable()
        End If

        Dim rep As Repeater
        For Each id As String In DirectCast(Me.Session(_SESSION_REP_CONTROLID), String())
            rep = DirectCast(Me.Parent.FindControl(id), Repeater)

            rep.DataSource = tblDs
            rep.DataBind()
        Next

        Me.Session(_SESSION_PAGE_CURRENT) = pageNo
    End Sub

    Protected Sub btn_ToPrev_Click(sender As Object, e As EventArgs)
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer) - 1)
    End Sub

    Protected Sub btn_ToNext_Click(sender As Object, e As EventArgs)
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer) + 1)
    End Sub

    Protected Sub tbx_PageNo_TextChanged(sender As Object, e As EventArgs)
        Dim pageNo As Integer
        If Me.tbx_PageNo.Text = String.Empty Then
            pageNo = 0
        Else
            pageNo = Convert.ToInt32(Me.tbx_PageNo.Text)

            If pageNo > 0 Then
                pageNo -= 1
            End If
        End If

        TurnPage(pageNo)
    End Sub

    Private Sub ListPager_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim prefix As String = Me.Page.GetType().Name + "_" + Me.GetType().Name + "_"

        _SESSION_REP_CONTROLID = prefix + "Rep_ControlId"
        _SESSION_REP_DTTBL = prefix + "Rep_DtTbl"
        _SESSION_PAGE_CNT = prefix + "Page_Cnt"
        _SESSION_PAGE_LINECNT = prefix + "Page_LineCnt"
        _SESSION_PAGE_CURRENT = prefix + "Page_Current"
    End Sub
End Class
