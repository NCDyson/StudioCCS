/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 8/11/2017
 * Time: 6:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace StudioCCS
{
	/// <summary>
	/// Description of FrmLoadScene.
	/// </summary>
	public partial class FrmLoadScene : Form
	{
		public List<String> FileNames = new List<string>();
		public string Path = "";

		public FrmLoadScene()
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
			var result = folderDialog.ShowDialog();
			if(result == DialogResult.OK)
			{
				txtDir.Text = folderDialog.SelectedPath;
				Path = folderDialog.SelectedPath;
				if(CheckDir())
				{
					btnOk.Enabled = true;
				}
			}
		}

		private bool CheckDir()
		{
			int existCount = 0;
			foreach(var fname in FileNames)
			{
				string fpath = Path + "/" + fname;
				var tmpItem = new ListViewItem(fpath);
				if(File.Exists(fpath))
				{
					existCount++;
					tmpItem.Text += " - FOUND";
				}
				else
				{
					tmpItem.Text += " - NOT FOUND";
					tmpItem.ForeColor = Color.Red;
				}
				sceneList.Items.Add(tmpItem);
			}
			
			if(existCount == FileNames.Count) return true;
			return false;
		}
		void FrmLoadSceneLoad(object sender, EventArgs e)
		{
			CheckDir();
		}
		
		
	}
}
