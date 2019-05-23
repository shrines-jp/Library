using System.Windows.Forms;

namespace CommonLibrary.Style
{
	public class DataGridViewHelper
	{
		public static DataGridViewColumn GetNullColumn()
		{
			DataGridViewTextBoxColumn nullColumn = new DataGridViewTextBoxColumn();
			nullColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			return nullColumn;
		}

		/// <summary>
		/// 기본 데이터 그리드뷰 셋팅
		/// </summary>
		/// <param name="dataGridView">대상 데이터그리드뷰</param>
		/// <param name="columns">추가될 컬럼정보</param>
		/// <param name="hideColums">숨길컬럼 이름</param>
		/// <param name="dataSource">데이터 소스</param>
		public static void DataGridViewDefaultSet(DataGridView dataGridView, DataGridViewColumn[] columns, string[] hideColums, object dataSource)
		{

			dataGridView.ClearSelection();
			dataGridView.Rows.Clear();
			dataGridView.Columns.Clear();

			dataGridView.Columns.AddRange(columns);
			dataGridView.DataSource = dataSource;

			if (hideColums != null)
			{
				foreach (string colName in hideColums)
				{
					if (dataGridView.Columns[colName] != null)
						dataGridView.Columns[colName].Visible = false;
				}
			}
			//마지막 빈컬럼..
			dataGridView.Columns.Add(GetNullColumn());
			dataGridView.AutoResizeColumns();
		}

	}
}
