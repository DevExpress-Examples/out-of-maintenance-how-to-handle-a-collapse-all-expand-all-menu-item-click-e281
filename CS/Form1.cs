using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Q109817 {
    public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			PivotGridControlEx pivot = new PivotGridControlEx();
			pivot.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.Optimized;
			pivot.Parent = this;
			pivot.Dock = DockStyle.Fill;
			pivot.FieldExpandedAll += new EventHandler<FieldExpandedEventArgs>(pivot_FieldExpandedAll);

			pivot.Fields.AddDataSourceColumn("Product Group", PivotArea.RowArea);
			pivot.Fields.AddDataSourceColumn("Product", PivotArea.RowArea);
			pivot.Fields.AddDataSourceColumn("Sales", PivotArea.DataArea);
			pivot.Fields[2].CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			pivot.Fields[2].CellFormat.FormatString = "c0";
			pivot.BestFitRowArea();

			DataTable table = new DataTable();
			table.Columns.Add("Product Group", typeof(string));
			table.Columns.Add("Product", typeof(string));
			table.Columns.Add("Sales", typeof(int));
			table.Rows.Add("Group A", "Almond", 1248);
			table.Rows.Add("Group A", "Apple", 2678);
			table.Rows.Add("Group A", "Apricot", 13002);
			table.Rows.Add("Group B", "Baguette", 936);
			table.Rows.Add("Group B", "Bacon", 10540);
			table.Rows.Add("Group B", "Baking Soda", 1080);
			pivot.DataSource = table.DefaultView;
		}

		void pivot_FieldExpandedAll(object sender, FieldExpandedEventArgs e) {
			MessageBox.Show("Fields are " + (e.Expanded ? "expanded": "collapsed"));
		}
	}

	public class PivotGridControlEx : PivotGridControl {
		protected override PivotGridViewInfoData CreateData() {
			return new PivotGridViewInfoDataEx(this);
		}

		public event EventHandler<FieldExpandedEventArgs> FieldExpandedAll;
		internal void RaiseFieldExpandedAll(PivotGridFieldBase field, bool expanded) {
			if(FieldExpandedAll != null)
				FieldExpandedAll(this, new FieldExpandedEventArgs((PivotGridField)field, expanded));
		}
	}

	public class PivotGridViewInfoDataEx : PivotGridViewInfoData {
		public PivotGridViewInfoDataEx(PivotGridControlEx owner) : base(owner) { }

		public new PivotGridControlEx PivotGrid { get { return (PivotGridControlEx)base.PivotGrid; } }

		public override bool ChangeFieldExpanded(PivotGridFieldBase field, bool expanded) {
			bool result = base.ChangeFieldExpanded(field, expanded);
			PivotGrid.RaiseFieldExpandedAll(field, expanded);
			return result;
		}
	}

	public class FieldExpandedEventArgs : EventArgs{
		PivotGridField field;
		bool expanded;

		public FieldExpandedEventArgs(PivotGridField field, bool expanded) {
			this.field = field;
			this.expanded = expanded;
		}

		public PivotGridField Field { get { return field; } }
		public bool Expanded { get { return expanded; } }
	}
}