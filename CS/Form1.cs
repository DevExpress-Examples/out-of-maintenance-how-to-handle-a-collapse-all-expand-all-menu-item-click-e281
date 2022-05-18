using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Data;

namespace Q109817 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			PivotGridControlEx pivot = new PivotGridControlEx();
			pivot.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.Optimized;
			pivot.Parent = this;
			pivot.Dock = DockStyle.Fill;
			pivot.FieldExpandedAll += new EventHandler<FieldExpandedEventArgs>(pivot_FieldExpandedAll);

			pivot.Fields.AddDataSourceColumn("group", PivotArea.RowArea);
			pivot.Fields.AddDataSourceColumn("product", PivotArea.RowArea);
			pivot.Fields.AddDataSourceColumn("sales", PivotArea.DataArea);

			DataTable table = new DataTable();
			table.Columns.Add("group", typeof(string));
			table.Columns.Add("product", typeof(string));
			table.Columns.Add("sales", typeof(int));
			table.Rows.Add("group 1", "product 1", 1);
			table.Rows.Add("group 1", "product 2", 2);
			table.Rows.Add("group 1", "product 3", 3);
			table.Rows.Add("group 2", "product 4", 4);
			table.Rows.Add("group 2", "product 5", 5);
			table.Rows.Add("group 2", "product 6", 6);
			pivot.DataSource = table.DefaultView;
		}

		void pivot_FieldExpandedAll(object sender, FieldExpandedEventArgs e) {
			MessageBox.Show("The " + e.Field.FieldName + " field was " + (e.Expanded ? "expanded" : "collapsed"));
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