/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 10/17/2017
 * Time: 2:44 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace StudioCCS
{
	partial class frmInfo
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.RichTextBox reportText;
		
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
			this.reportText = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// reportText
			// 
			this.reportText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.reportText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.reportText.Location = new System.Drawing.Point(0, 0);
			this.reportText.Name = "reportText";
			this.reportText.ReadOnly = true;
			this.reportText.Size = new System.Drawing.Size(547, 402);
			this.reportText.TabIndex = 1;
			this.reportText.Text = "";
			this.reportText.WordWrap = false;
			// 
			// frmInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(547, 402);
			this.Controls.Add(this.reportText);
			this.Name = "frmInfo";
			this.Text = "File Info Report";
			this.ResumeLayout(false);

		}
	}
}
