/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 10/17/2017
 * Time: 2:44 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	/// <summary>
	/// Description of frmInfo.
	/// </summary>
	public partial class frmInfo : Form
	{
		public frmInfo()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void SetReportText(string _reportText)
		{
			reportText.Text = _reportText;
		}
	}
}
