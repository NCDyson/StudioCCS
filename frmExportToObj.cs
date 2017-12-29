/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 10/23/2017
 * Time: 1:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class frmExportToOBJ : Form
	{
		public frmExportToOBJ()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void BtnSelectDirClick(object sender, EventArgs e)
		{
			using(var vfd = new SaveFileDialog())
			{
				vfd.FileName = "Export Here";
				vfd.CheckFileExists = false;
				vfd.Title = "Select directory to export to...";
				
				DialogResult result = vfd.ShowDialog();
				if(result == DialogResult.OK)
				{
					txtExportPath.Text = System.IO.Path.GetDirectoryName(vfd.FileName);
				}
				
				if(txtExportPath.Text == "")
				{
					btnDoExport.Enabled = false;
				}
				else
				{
					btnDoExport.Enabled = true;
				}
			}
		}
		void ChkExportCollisionCheckedChanged(object sender, EventArgs e)
		{
			chkSplitCollision.Enabled = chkExportCollision.Checked;
		}
	}
}
