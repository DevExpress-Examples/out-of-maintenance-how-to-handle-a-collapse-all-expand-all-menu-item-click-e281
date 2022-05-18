Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraPivotGrid
Imports DevExpress.XtraPivotGrid.Data

Namespace Q109817
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()

			Dim pivot As New PivotGridControlEx()
			pivot.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.Optimized
			pivot.Parent = Me
			pivot.Dock = DockStyle.Fill
			AddHandler pivot.FieldExpandedAll, AddressOf pivot_FieldExpandedAll

			pivot.Fields.AddDataSourceColumn("group", PivotArea.RowArea)
			pivot.Fields.AddDataSourceColumn("product", PivotArea.RowArea)
			pivot.Fields.AddDataSourceColumn("sales", PivotArea.DataArea)

			Dim table As New DataTable()
			table.Columns.Add("group", GetType(String))
			table.Columns.Add("product", GetType(String))
			table.Columns.Add("sales", GetType(Integer))
			table.Rows.Add("group 1", "product 1", 1)
			table.Rows.Add("group 1", "product 2", 2)
			table.Rows.Add("group 1", "product 3", 3)
			table.Rows.Add("group 2", "product 4", 4)
			table.Rows.Add("group 2", "product 5", 5)
			table.Rows.Add("group 2", "product 6", 6)
			pivot.DataSource = table.DefaultView
		End Sub

		Private Sub pivot_FieldExpandedAll(ByVal sender As Object, ByVal e As FieldExpandedEventArgs)
			If e.Expanded Then
				MessageBox.Show("The " & e.Field.FieldName & " field was " & ("expanded"))
			Else
				MessageBox.Show("The " & e.Field.FieldName & " field was " & ("collapsed"))
			End If
		End Sub
	End Class

	Public Class PivotGridControlEx
		Inherits PivotGridControl
		Protected Overrides Function CreateData() As PivotGridViewInfoData
			Return New PivotGridViewInfoDataEx(Me)
		End Function

		Public Event FieldExpandedAll As EventHandler(Of FieldExpandedEventArgs)
		Friend Sub RaiseFieldExpandedAll(ByVal field As PivotGridFieldBase, ByVal expanded As Boolean)
			RaiseEvent FieldExpandedAll(Me, New FieldExpandedEventArgs(CType(field, PivotGridField), expanded))
		End Sub
	End Class

	Public Class PivotGridViewInfoDataEx
		Inherits PivotGridViewInfoData
		Public Sub New(ByVal owner As PivotGridControlEx)
			MyBase.New(owner)
		End Sub

		Public Shadows ReadOnly Property PivotGrid() As PivotGridControlEx
			Get
				Return CType(MyBase.PivotGrid, PivotGridControlEx)
			End Get
		End Property

		Public Overrides Function ChangeFieldExpanded(ByVal field As PivotGridFieldBase, ByVal expanded As Boolean) As Boolean
			Dim result As Boolean = MyBase.ChangeFieldExpanded(field, expanded)
			PivotGrid.RaiseFieldExpandedAll(field, expanded)
			Return result
		End Function
	End Class

	Public Class FieldExpandedEventArgs
		Inherits EventArgs
		Private field_Renamed As PivotGridField
		Private expanded_Renamed As Boolean

		Public Sub New(ByVal field As PivotGridField, ByVal expanded As Boolean)
			Me.field_Renamed = field
			Me.expanded_Renamed = expanded
		End Sub

		Public ReadOnly Property Field() As PivotGridField
			Get
				Return field_Renamed
			End Get
		End Property
		Public ReadOnly Property Expanded() As Boolean
			Get
				Return expanded_Renamed
			End Get
		End Property
	End Class
End Namespace