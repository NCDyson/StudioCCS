/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 8/11/2017
 * Time: 6:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace StudioCCS
{
	partial class FrmLoadScene
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSelectDir;
		private System.Windows.Forms.TextBox txtDir;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FolderBrowserDialog folderDialog;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ListView sceneList;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSelectDir = new System.Windows.Forms.Button();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.sceneList = new System.Windows.Forms.ListView();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSelectDir);
			this.panel1.Controls.Add(this.txtDir);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(633, 23);
			this.panel1.TabIndex = 0;
			// 
			// btnSelectDir
			// 
			this.btnSelectDir.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnSelectDir.Location = new System.Drawing.Point(606, 0);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new System.Drawing.Size(27, 23);
			this.btnSelectDir.TabIndex = 2;
			this.btnSelectDir.Text = "...";
			this.btnSelectDir.UseVisualStyleBackColor = true;
			this.btnSelectDir.Click += new System.EventHandler(this.BtnSelectDirClick);
			// 
			// txtDir
			// 
			this.txtDir.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDir.Location = new System.Drawing.Point(49, 0);
			this.txtDir.Name = "txtDir";
			this.txtDir.ReadOnly = true;
			this.txtDir.Size = new System.Drawing.Size(584, 20);
			this.txtDir.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Directory:";
			// 
			// folderDialog
			// 
			this.folderDialog.Description = "Select Directory";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnOk);
			this.panel2.Controls.Add(this.btnCancel);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 190);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(633, 32);
			this.panel2.TabIndex = 2;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnOk.Location = new System.Drawing.Point(483, 0);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 32);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(558, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 32);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.sceneList);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 23);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(633, 167);
			this.panel3.TabIndex = 4;
			// 
			// sceneList
			// 
			this.sceneList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sceneList.Location = new System.Drawing.Point(0, 0);
			this.sceneList.Name = "sceneList";
			this.sceneList.Size = new System.Drawing.Size(633, 167);
			this.sceneList.TabIndex = 0;
			this.sceneList.UseCompatibleStateImageBehavior = false;
			// 
			// FrmLoadScene
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(633, 222);
			this.ControlBox = false;
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Enabled = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmLoadScene";
			this.Text = "Load Scene:";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FrmLoadSceneLoad);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
