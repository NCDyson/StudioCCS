/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 11/8/2017
 * Time: 5:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace StudioCCS
{
	partial class frmEditBone
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPosX;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtScaleZ;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtScaleY;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtScaleX;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtRotZ;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtRotY;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtRotX;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPosZ;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPosY;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadPoseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem savePoseToolStripMenuItem;
		private System.Windows.Forms.TreeView treeBones;
		private System.Windows.Forms.Label lblBoneName;
		private System.Windows.Forms.ToolStripMenuItem clearRotationValuesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearScaleValuesToolStripMenuItem;
		
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblBoneName = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtRotZ = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtRotY = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtRotX = new System.Windows.Forms.TextBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtScaleZ = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txtScaleY = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtScaleX = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPosZ = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPosY = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPosX = new System.Windows.Forms.TextBox();
			this.treeBones = new System.Windows.Forms.TreeView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadPoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.savePoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearRotationValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearScaleValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 276F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.treeBones, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(449, 231);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblBoneName);
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.btnUpdate);
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(176, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(270, 225);
			this.panel1.TabIndex = 1;
			// 
			// lblBoneName
			// 
			this.lblBoneName.Location = new System.Drawing.Point(5, 5);
			this.lblBoneName.Name = "lblBoneName";
			this.lblBoneName.Size = new System.Drawing.Size(256, 15);
			this.lblBoneName.TabIndex = 9;
			this.lblBoneName.Text = "Bone Name Here";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.txtRotZ);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.txtRotY);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.txtRotX);
			this.groupBox3.Location = new System.Drawing.Point(4, 119);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(130, 90);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Rotation";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 67);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(18, 20);
			this.label7.TabIndex = 5;
			this.label7.Text = "Z";
			// 
			// txtRotZ
			// 
			this.txtRotZ.Location = new System.Drawing.Point(30, 64);
			this.txtRotZ.Name = "txtRotZ";
			this.txtRotZ.Size = new System.Drawing.Size(90, 20);
			this.txtRotZ.TabIndex = 4;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 44);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(18, 20);
			this.label8.TabIndex = 3;
			this.label8.Text = "Y";
			// 
			// txtRotY
			// 
			this.txtRotY.Location = new System.Drawing.Point(30, 41);
			this.txtRotY.Name = "txtRotY";
			this.txtRotY.Size = new System.Drawing.Size(90, 20);
			this.txtRotY.TabIndex = 2;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 22);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(18, 20);
			this.label9.TabIndex = 1;
			this.label9.Text = "X";
			// 
			// txtRotX
			// 
			this.txtRotX.Location = new System.Drawing.Point(30, 19);
			this.txtRotX.Name = "txtRotX";
			this.txtRotX.Size = new System.Drawing.Size(90, 20);
			this.txtRotX.TabIndex = 0;
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdate.Location = new System.Drawing.Point(194, 201);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 8;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.BtnUpdateClick);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.txtScaleZ);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.txtScaleY);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.txtScaleX);
			this.groupBox4.Location = new System.Drawing.Point(140, 23);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(130, 90);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Scale";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 67);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(18, 20);
			this.label10.TabIndex = 5;
			this.label10.Text = "Z";
			// 
			// txtScaleZ
			// 
			this.txtScaleZ.Location = new System.Drawing.Point(30, 64);
			this.txtScaleZ.Name = "txtScaleZ";
			this.txtScaleZ.Size = new System.Drawing.Size(90, 20);
			this.txtScaleZ.TabIndex = 4;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 44);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(18, 20);
			this.label11.TabIndex = 3;
			this.label11.Text = "Y";
			// 
			// txtScaleY
			// 
			this.txtScaleY.Location = new System.Drawing.Point(30, 41);
			this.txtScaleY.Name = "txtScaleY";
			this.txtScaleY.Size = new System.Drawing.Size(90, 20);
			this.txtScaleY.TabIndex = 2;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 22);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(18, 20);
			this.label12.TabIndex = 1;
			this.label12.Text = "X";
			// 
			// txtScaleX
			// 
			this.txtScaleX.Location = new System.Drawing.Point(30, 19);
			this.txtScaleX.Name = "txtScaleX";
			this.txtScaleX.Size = new System.Drawing.Size(90, 20);
			this.txtScaleX.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtPosZ);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtPosY);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtPosX);
			this.groupBox1.Location = new System.Drawing.Point(4, 23);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(130, 90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Position";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(18, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Z";
			// 
			// txtPosZ
			// 
			this.txtPosZ.Location = new System.Drawing.Point(30, 64);
			this.txtPosZ.Name = "txtPosZ";
			this.txtPosZ.Size = new System.Drawing.Size(90, 20);
			this.txtPosZ.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Y";
			// 
			// txtPosY
			// 
			this.txtPosY.Location = new System.Drawing.Point(30, 41);
			this.txtPosY.Name = "txtPosY";
			this.txtPosY.Size = new System.Drawing.Size(90, 20);
			this.txtPosY.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "X";
			// 
			// txtPosX
			// 
			this.txtPosX.Location = new System.Drawing.Point(30, 19);
			this.txtPosX.Name = "txtPosX";
			this.txtPosX.Size = new System.Drawing.Size(90, 20);
			this.txtPosX.TabIndex = 0;
			// 
			// treeBones
			// 
			this.treeBones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeBones.Location = new System.Drawing.Point(3, 3);
			this.treeBones.Name = "treeBones";
			this.treeBones.Size = new System.Drawing.Size(167, 225);
			this.treeBones.TabIndex = 2;
			this.treeBones.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeBonesAfterSelect);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.editToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(449, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.loadPoseToolStripMenuItem,
			this.savePoseToolStripMenuItem,
			this.clearRotationValuesToolStripMenuItem,
			this.clearScaleValuesToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// loadPoseToolStripMenuItem
			// 
			this.loadPoseToolStripMenuItem.Name = "loadPoseToolStripMenuItem";
			this.loadPoseToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadPoseToolStripMenuItem.Text = "Load Pose";
			this.loadPoseToolStripMenuItem.Click += new System.EventHandler(this.LoadPoseToolStripMenuItemClick);
			// 
			// savePoseToolStripMenuItem
			// 
			this.savePoseToolStripMenuItem.Name = "savePoseToolStripMenuItem";
			this.savePoseToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.savePoseToolStripMenuItem.Text = "Save Pose";
			this.savePoseToolStripMenuItem.Click += new System.EventHandler(this.SavePoseToolStripMenuItemClick);
			// 
			// clearRotationValuesToolStripMenuItem
			// 
			this.clearRotationValuesToolStripMenuItem.Name = "clearRotationValuesToolStripMenuItem";
			this.clearRotationValuesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.clearRotationValuesToolStripMenuItem.Text = "Clear Rotation Values";
			this.clearRotationValuesToolStripMenuItem.Click += new System.EventHandler(this.ClearRotationValuesToolStripMenuItemClick);
			// 
			// clearScaleValuesToolStripMenuItem
			// 
			this.clearScaleValuesToolStripMenuItem.Name = "clearScaleValuesToolStripMenuItem";
			this.clearScaleValuesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.clearScaleValuesToolStripMenuItem.Text = "Clear Scale Values";
			this.clearScaleValuesToolStripMenuItem.Click += new System.EventHandler(this.ClearScaleValuesToolStripMenuItemClick);
			// 
			// frmEditBone
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(449, 255);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "frmEditBone";
			this.Text = "frmEditBone";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEditBoneFormClosed);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
