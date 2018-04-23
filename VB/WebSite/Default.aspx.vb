Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxEditors

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	End Sub

	Protected Function GetChecked(ByVal container As GridViewGroupRowTemplateContainer) As Boolean
		For i As Integer = 0 To Grid.GetChildRowCount(container.VisibleIndex) - 1
			Dim isRowSelected As Boolean = Grid.Selection.IsRowSelectedByKey(Grid.GetChildDataRow(container.VisibleIndex, i)("ProductID")) ' index 0 is keyValue index
			If (Not isRowSelected) Then
				Return False
			End If
		Next i
		Return True
	End Function

	Protected Function GetCaptionText(ByVal container As GridViewGroupRowTemplateContainer) As String
		Dim captionText As String
		If (Not String.IsNullOrEmpty(container.Column.Caption)) Then
			captionText = container.Column.Caption
		Else
			captionText = container.Column.FieldName
		End If
		Return String.Format("{0} : {1} {2}", captionText, container.GroupText, container.SummaryText)
	End Function
	Protected Sub Grid_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
		Dim parameters() As String = e.Parameters.Split(";"c)
		Dim index As Integer = Integer.Parse(parameters(0))
		Dim isGroupRowSelected As Boolean = Boolean.Parse(parameters(1))
		For i As Integer = 0 To Grid.GetChildRowCount(index) - 1
			Dim row As DataRow = Grid.GetChildDataRow(index, i)
			Grid.Selection.SetSelectionByKey(row("ProductID"), isGroupRowSelected)
		Next i
	End Sub

	Protected Sub Grid_HtmlRowPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableRowEventArgs)
		If e.RowType = GridViewRowType.Group Then
			For Each cell As TableCell In e.Row.Cells
				For Each control As Control In cell.Controls
					Dim container As GridViewGroupRowTemplateContainer = TryCast(control, GridViewGroupRowTemplateContainer)
					If container Is Nothing Then
						Continue For
					End If
					Dim checkBox As ASPxCheckBox = CType(container.FindControl("checkBox"), ASPxCheckBox)
					checkBox.ClientSideEvents.CheckedChanged = String.Format("function(s, e){{ Grid.PerformCallback('{0};' + s.GetChecked()); }}", container.VisibleIndex)
					checkBox.Checked = GetChecked(container)
				Next control

			Next cell
		End If
	End Sub
End Class
