Imports System.Data
Imports System.Data.SqlClient

Public Class db_SHOHIN_IMG_FILE

    Public Shared ReadOnly Property EmptyTable_01 As DataTable
        Get
            Dim sb As StringBuilder = New StringBuilder(1000)
            sb.AppendLine("SELECT TOP(0) *")
            sb.AppendLine("FROM vi_SHOHIN_IMG_FILE_2;")

            Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
            Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)

            Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim tbl As DataTable = New DataTable()
            adp.FillSchema(tbl, SchemaType.Mapped)

            adp.Dispose()
            cmd.Dispose()
            cn.Dispose()

            Return tbl
        End Get
    End Property

    Public Shared Function Select_01(chk As String, scd_from As Long?, scd_to As Long?, pics As Dictionary(Of String, String), keisai As Dictionary(Of String, String)) As DataTable
        Dim sb As StringBuilder = New StringBuilder(1000)
        sb.AppendLine("SELECT *")
        sb.AppendLine("FROM vi_SHOHIN_IMG_FILE_2")
        sb.AppendLine("WHERE SIF_PIC_UNIT = @SIF_PIC_UNIT")
        sb.AppendLine(" AND (SSHM_SCD >= @SSHM_SCD_FROM OR @SSHM_SCD_FROM IS NULL)")
        sb.AppendLine(" AND (SSHM_SCD <= @SSHM_SCD_TO OR @SSHM_SCD_TO IS NULL)")

        If pics.Count = 0 Then
            sb.AppendLine(" AND 0 = 1")
        Else
            sb.Append(" AND (")

            Dim s1 As String = String.Empty
            For Each pic As String In pics.Values
                s1 += " OR " + pic + " = 0"
            Next

            sb.AppendLine(s1.Substring(4) + ")")
        End If

        If keisai.Count = 0 Then
            sb.AppendLine(" AND 0 = 1")
        Else
            sb.Append(" AND SSHM_KEISAIFLG IN (")

            Dim s2 As String = String.Empty
            For Each kfg As String In keisai.Values
                s2 += ", " + kfg
            Next

            sb.AppendLine(s2.Substring(2) + ")")
        End If

        sb.AppendLine("ORDER BY SSHM_SCD, SSHM_HTCD;")

        Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)

        Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)
        cmd.CommandTimeout = 100

        Dim prm As SqlParameter
        prm = cmd.Parameters.Add(New SqlParameter("@SIF_PIC_UNIT", SqlDbType.NVarChar, 100))
        prm.Value = chk
        prm = cmd.Parameters.Add(New SqlParameter("@SSHM_SCD_FROM", SqlDbType.BigInt))
        If scd_from Is Nothing Then
            prm.Value = DBNull.Value
        Else
            prm.Value = scd_from.Value
        End If
        prm = cmd.Parameters.Add(New SqlParameter("@SSHM_SCD_TO", SqlDbType.BigInt))
        If scd_to Is Nothing Then
            prm.Value = DBNull.Value
        Else
            prm.Value = scd_to.Value
        End If

        Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
        Dim tbl As DataTable = New DataTable()
        adp.Fill(tbl)

        adp.Dispose()
        cmd.Dispose()
        cn.Dispose()

        Return tbl
    End Function

    Public Shared ReadOnly Property DDL_SIF_PIC_UNIT As DataTable
        Get
            Dim sb As StringBuilder = New StringBuilder(1000)
            sb.AppendLine("SELECT DISTINCT SIF_PIC_UNIT")
            sb.AppendLine("FROM vi_SHOHIN_IMG_FILE_2")
            sb.AppendLine("ORDER BY SIF_PIC_UNIT;")

            Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
            Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)

            Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim tbl As DataTable = New DataTable()
            adp.Fill(tbl)

            adp.Dispose()
            cmd.Dispose()
            cn.Dispose()

            Return tbl
        End Get
    End Property

End Class
