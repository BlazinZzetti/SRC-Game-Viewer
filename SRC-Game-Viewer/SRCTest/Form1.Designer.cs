
namespace SRCTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.srcSearchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchSRCButton = new System.Windows.Forms.Button();
            this.includeRejectedCheckBox = new System.Windows.Forms.CheckBox();
            this.includeNewCheckBox = new System.Windows.Forms.CheckBox();
            this.csvExportButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(13, 36);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(775, 373);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // srcSearchTextBox
            // 
            this.srcSearchTextBox.Location = new System.Drawing.Point(54, 10);
            this.srcSearchTextBox.Name = "srcSearchTextBox";
            this.srcSearchTextBox.Size = new System.Drawing.Size(263, 20);
            this.srcSearchTextBox.TabIndex = 1;
            this.srcSearchTextBox.Text = "Shadow the Hedgehog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Game";
            // 
            // SearchSRCButton
            // 
            this.SearchSRCButton.Location = new System.Drawing.Point(324, 9);
            this.SearchSRCButton.Name = "SearchSRCButton";
            this.SearchSRCButton.Size = new System.Drawing.Size(75, 23);
            this.SearchSRCButton.TabIndex = 3;
            this.SearchSRCButton.Text = "Search SRC";
            this.SearchSRCButton.UseVisualStyleBackColor = true;
            this.SearchSRCButton.Click += new System.EventHandler(this.SearchSRCButton_Click);
            // 
            // includeRejectedCheckBox
            // 
            this.includeRejectedCheckBox.AutoSize = true;
            this.includeRejectedCheckBox.Location = new System.Drawing.Point(406, 12);
            this.includeRejectedCheckBox.Name = "includeRejectedCheckBox";
            this.includeRejectedCheckBox.Size = new System.Drawing.Size(135, 17);
            this.includeRejectedCheckBox.TabIndex = 4;
            this.includeRejectedCheckBox.Text = "Include Rejected Runs";
            this.includeRejectedCheckBox.UseVisualStyleBackColor = true;
            // 
            // includeNewCheckBox
            // 
            this.includeNewCheckBox.AutoSize = true;
            this.includeNewCheckBox.Location = new System.Drawing.Point(547, 12);
            this.includeNewCheckBox.Name = "includeNewCheckBox";
            this.includeNewCheckBox.Size = new System.Drawing.Size(171, 17);
            this.includeNewCheckBox.TabIndex = 5;
            this.includeNewCheckBox.Text = "Include Unvarified (New) Runs";
            this.includeNewCheckBox.UseVisualStyleBackColor = true;
            // 
            // csvExportButton
            // 
            this.csvExportButton.Location = new System.Drawing.Point(13, 415);
            this.csvExportButton.Name = "csvExportButton";
            this.csvExportButton.Size = new System.Drawing.Size(110, 23);
            this.csvExportButton.TabIndex = 6;
            this.csvExportButton.Text = "Export Data to CSV";
            this.csvExportButton.UseVisualStyleBackColor = true;
            this.csvExportButton.Click += new System.EventHandler(this.csvExportButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(129, 421);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(173, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Force Variable Count for Export";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(309, 418);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.csvExportButton);
            this.Controls.Add(this.includeNewCheckBox);
            this.Controls.Add(this.includeRejectedCheckBox);
            this.Controls.Add(this.SearchSRCButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.srcSearchTextBox);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "SRC Game Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox srcSearchTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SearchSRCButton;
        private System.Windows.Forms.CheckBox includeRejectedCheckBox;
        private System.Windows.Forms.CheckBox includeNewCheckBox;
        private System.Windows.Forms.Button csvExportButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}

