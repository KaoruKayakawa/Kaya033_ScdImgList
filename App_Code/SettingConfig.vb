Imports System.Xml

Public Class SettingConfig
    Protected Shared _root As XmlNode

    Shared Sub New()
        Try
            Dim fn As String
            fn = HttpContext.Current.Server.MapPath("~/") + "Setting.config"

            Dim doc As XmlDocument = New XmlDocument
            doc.Load(fn)

            _root = doc.SelectSingleNode("/configuration")
        Catch ex As Exception
            Throw New ApplicationException("Iregular Setting.config File.", ex)
        End Try
    End Sub

    Public Shared ReadOnly Property ConnectingString As String
        Get
            Return _root.SelectSingleNode("./connstring").InnerText.Trim()
        End Get
    End Property

    Public Shared ReadOnly Property WindowCaption As String
        Get
            Return _root.SelectSingleNode("./window_caption").InnerText.Trim()
        End Get
    End Property

    Public Shared ReadOnly Property ListRowCount As Integer
        Get
            Return Convert.ToInt32(_root.SelectSingleNode("./list_row_count").InnerText.Trim())
        End Get
    End Property

End Class
