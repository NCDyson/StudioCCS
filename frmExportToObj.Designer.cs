/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 10/23/2017
 * Time: 1:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace StudioCCS
{
	partial class frmExportToOBJ
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSelectDir;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.CheckBox chkExportCollision;
		public System.Windows.Forms.TextBox txtExportPath;
		public System.Windows.Forms.CheckBox chkSplitSubModels;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnDoExport;
		public System.Windows.Forms.CheckBox chkSplitCollision;
		public System.Windows.Forms.CheckBox chkModelWithNormals;
		public System.Windows.Forms.CheckBox chkExportDummies;
		public System.Windows.Forms.CheckBox chkDumpAnime;
		
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtExportPath = new System.Windows.Forms.TextBox();
			this.btnSelectDir = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkExportDummies = new System.Windows.Forms.CheckBox();
			this.chkModelWithNormals = new System.Windows.Forms.CheckBox();
			this.chkSplitCollision = new System.Windows.Forms.CheckBox();
			this.chkSplitSubModels = new System.Windows.Forms.CheckBox();
			this.chkExportCollision = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnDoExport = new System.Windows.Forms.Button();
			this.chkDumpAnime = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.txtExportPath);
			this.groupBox1.Controls.Add(this.btnSelectDir);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(526, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Ouput Path:";
			// 
			// txtExportPath
			// 
			this.txtExportPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtExportPath.Location = new System.Drawing.Point(3, 16);
			this.txtExportPath.Name = "txtExportPath";
			this.txtExportPath.Size = new System.Drawing.Size(491, 20);
			this.txtExportPath.TabIndex = 1;
			// 
			// btnSelectDir
			// 
			this.btnSelectDir.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnSelectDir.Location = new System.Drawing.Point(494, 16);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new System.Drawing.Size(29, 21);
			this.btnSelectDir.TabIndex = 0;
			this.btnSelectDir.Text = "...";
			this.btnSelectDir.UseVisualStyleBackColor = true;
			this.btnSelectDir.Click += new System.EventHandler(this.BtnSelectDirClick);
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.chkDumpAnime);
			this.groupBox2.Controls.Add(this.chkExportDummies);
			this.groupBox2.Controls.Add(this.chkModelWithNormals);
			this.groupBox2.Controls.Add(this.chkSplitCollision);
			this.groupBox2.Controls.Add(this.chkSplitSubModels);
			this.groupBox2.Controls.Add(this.chkExportCollision);
			this.groupBox2.Location = new System.Drawing.Point(0, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(523, 75);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options:";
			// 
			// chkExportDummies
			// 
			this.chkExportDummies.Location = new System.Drawing.Point(312, 14);
			this.chkExportDummies.Name = "chkExportDummies";
			this.chkExportDummies.Size = new System.Drawing.Size(182, 24);
			this.chkExportDummies.TabIndex = 4;
			this.chkExportDummies.Text = "Export Dummies to .txt";
			this.chkExportDummies.UseVisualStyleBackColor = true;
			// 
			// chkModelWithNormals
			// 
			this.chkModelWithNormals.Location = new System.Drawing.Point(168, 45);
			this.chkModelWithNormals.Name = "chkModelWithNormals";
			this.chkModelWithNormals.Size = new System.Drawing.Size(137, 24);
			this.chkModelWithNormals.TabIndex = 3;
			this.chkModelWithNormals.Text = "Export Model Normals";
			this.chkModelWithNormals.UseVisualStyleBackColor = true;
			// 
			// chkSplitCollision
			// 
			this.chkSplitCollision.Enabled = false;
			this.chkSplitCollision.Location = new System.Drawing.Point(13, 45);
			this.chkSplitCollision.Name = "chkSplitCollision";
			this.chkSplitCollision.Size = new System.Drawing.Size(149, 24);
			this.chkSplitCollision.TabIndex = 2;
			this.chkSplitCollision.Text = "Split Collision Meshes";
			this.chkSplitCollision.UseVisualStyleBackColor = true;
			// 
			// chkSplitSubModels
			// 
			this.chkSplitSubModels.Location = new System.Drawing.Point(168, 14);
			this.chkSplitSubModels.Name = "chkSplitSubModels";
			this.chkSplitSubModels.Size = new System.Drawing.Size(137, 24);
			this.chkSplitSubModels.TabIndex = 1;
			this.chkSplitSubModels.Text = "Split Sub Model pieces";
			this.chkSplitSubModels.UseVisualStyleBackColor = true;
			// 
			// chkExportCollision
			// 
			this.chkExportCollision.Location = new System.Drawing.Point(13, 14);
			this.chkExportCollision.Name = "chkExportCollision";
			this.chkExportCollision.Size = new System.Drawing.Size(149, 24);
			this.chkExportCollision.TabIndex = 0;
			this.chkExportCollision.Text = "Export Collision Meshes";
			this.chkExportCollision.UseVisualStyleBackColor = true;
			this.chkExportCollision.CheckedChanged += new System.EventHandler(this.ChkExportCollisionCheckedChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(446, 121);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnDoExport
			// 
			this.btnDoExport.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDoExport.Enabled = false;
			this.btnDoExport.Location = new System.Drawing.Point(365, 121);
			this.btnDoExport.Name = "btnDoExport";
			this.btnDoExport.Size = new System.Drawing.Size(75, 23);
			this.btnDoExport.TabIndex = 4;
			this.btnDoExport.Text = "Export";
			this.btnDoExport.UseVisualStyleBackColor = true;
			// 
			// chkDumpAnime
			// 
			this.chkDumpAnime.Location = new System.Drawing.Point(312, 45);
			this.chkDumpAnime.Name = "chkDumpAnime";
			this.chkDumpAnime.Size = new System.Drawing.Size(202, 24);
			this.chkDumpAnime.TabIndex = 5;
			this.chkDumpAnime.Text = "Dump Anime (First Frame) to Text";
			this.chkDumpAnime.UseVisualStyleBackColor = true;
			// 
			// frmExportToOBJ
			// 
			this.AcceptButton = this.btnDoExport;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(526, 148);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDoExport);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(542, 187);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(542, 187);
			this.Name = "frmExportToOBJ";
			this.Text = "Export To .OBJ...";
			this.TopMost = true;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
