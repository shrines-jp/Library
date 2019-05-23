using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary.Helper
{
	public class DataGridViewHelper
	{
		/// <summary>
		/// 데이터 수동관리시, 새행필요여부
		/// </summary>
		static bool newRowNeeded;

		/// <summary>
		/// 초기사이즈
		/// </summary>
		const int initialSize = 5000000;
		static int numberOfRows = initialSize;


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
			try
			{
				dataGridView.ClearSelection();
				dataGridView.Rows.Clear();

				dataGridView.Columns.Clear();

				dataGridView.VirtualMode = true;
				///이벤트 추가
				dataGridView.NewRowNeeded += new DataGridViewRowEventHandler(dataGridView_NewRowNeeded);
				dataGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);

				///클립보드 이벤트 추가
				dataGridView.KeyDown += new KeyEventHandler(dataGridView_KeyDown);

				///이벤트 끝

				///초기 로우 세팅
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
				//dataGridView.Columns.Add(GetNullColumn());
				//dataGridView.AutoResizeColumns();
			}
			catch (ArgumentNullException)
			{
				throw;
			}
			catch (InvalidOperationException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// 데이터 그리드뷰에서 클립보드 지원
		/// 2013.10.29 박정환
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void dataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.C)
			{
				CopyToClipboard((sender as DataGridView).CurrentCell.Value.ToString());
			}
		}

		/// <summary>
		/// 클립보드로 복사
		/// 2013.10.29 박정환
		/// </summary>
		/// <param name="value"></param>
		private static void CopyToClipboard(String value)
		{
			try
			{
				Clipboard.Clear();
				Clipboard.SetText(value, TextDataFormat.Text);
			}
			catch (System.Runtime.InteropServices.ExternalException)
			{
				throw;
			}
			catch (System.Threading.ThreadStateException)
			{
				throw;
			}
			catch (System.ComponentModel.InvalidEnumArgumentException)
			{
				throw;
			}
			catch (System.ArgumentException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// dataGridView_RowsAdded
		/// 새행이 추가된 다음에 발생한다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (newRowNeeded)
			{
				newRowNeeded = false;
				numberOfRows = numberOfRows + 1;
			}
		}

		/// <summary>
		/// dataGridView_NewRowNeeded
		/// DataGridView의 맨아래에 있는 새행으로 이동하면 발생한다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void dataGridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
		{
			newRowNeeded = true;
		}

		/// <summary>
		/// 헤더로 부터, 셀의 값을 가져온다.
		/// </summary>
		/// <param name="CellCollection"></param>
		/// <param name="HeaderText"></param>
		/// <returns></returns>
		public static object GetCellValueFromColumnHeader(DataGridViewCellCollection CellCollection, string HeaderText)
		{
			return CellCollection.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == HeaderText).Value;
		}
	}
}
