namespace Winslop.Views
{
    partial class FeaturesView
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.analyzeMarkedFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixMarkedFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreMarkedFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seperatorToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.helpMarkedFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeFeatures = new System.Windows.Forms.TreeView();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyzeMarkedFeatureToolStripMenuItem,
            this.fixMarkedFeatureToolStripMenuItem,
            this.restoreMarkedFeatureToolStripMenuItem,
            this.seperatorToolStripMenuItem,
            this.helpMarkedFeatureToolStripMenuItem});
            this.contextMenuStrip.Name = "contextManualMenu";
            this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip.Size = new System.Drawing.Size(116, 98);
            // 
            // analyzeMarkedFeatureToolStripMenuItem
            // 
            this.analyzeMarkedFeatureToolStripMenuItem.Name = "analyzeMarkedFeatureToolStripMenuItem";
            this.analyzeMarkedFeatureToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.analyzeMarkedFeatureToolStripMenuItem.Text = "Analyze";
            this.analyzeMarkedFeatureToolStripMenuItem.Click += new System.EventHandler(this.analyzeMarkedFeatureToolStripMenuItem_Click);
            // 
            // fixMarkedFeatureToolStripMenuItem
            // 
            this.fixMarkedFeatureToolStripMenuItem.Name = "fixMarkedFeatureToolStripMenuItem";
            this.fixMarkedFeatureToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.fixMarkedFeatureToolStripMenuItem.Text = "Fix";
            this.fixMarkedFeatureToolStripMenuItem.Click += new System.EventHandler(this.fixMarkedFeatureToolStripMenuItem_Click);
            // 
            // restoreMarkedFeatureToolStripMenuItem
            // 
            this.restoreMarkedFeatureToolStripMenuItem.Name = "restoreMarkedFeatureToolStripMenuItem";
            this.restoreMarkedFeatureToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.restoreMarkedFeatureToolStripMenuItem.Text = "Restore";
            this.restoreMarkedFeatureToolStripMenuItem.Click += new System.EventHandler(this.restoreMarkedFeatureToolStripMenuItem_Click);
            // 
            // seperatorToolStripMenuItem
            // 
            this.seperatorToolStripMenuItem.Name = "seperatorToolStripMenuItem";
            this.seperatorToolStripMenuItem.Size = new System.Drawing.Size(112, 6);
            // 
            // helpMarkedFeatureToolStripMenuItem
            // 
            this.helpMarkedFeatureToolStripMenuItem.Name = "helpMarkedFeatureToolStripMenuItem";
            this.helpMarkedFeatureToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.helpMarkedFeatureToolStripMenuItem.Text = "Help";
            this.helpMarkedFeatureToolStripMenuItem.Click += new System.EventHandler(this.helpMarkedFeatureToolStripMenuItem_Click);
            // 
            // treeFeatures
            // 
            this.treeFeatures.BackColor = System.Drawing.Color.White;
            this.treeFeatures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeFeatures.CheckBoxes = true;
            this.treeFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFeatures.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeFeatures.FullRowSelect = true;
            this.treeFeatures.HotTracking = true;
            this.treeFeatures.Location = new System.Drawing.Point(0, 0);
            this.treeFeatures.Name = "treeFeatures";
            this.treeFeatures.ShowLines = false;
            this.treeFeatures.ShowNodeToolTips = true;
            this.treeFeatures.ShowPlusMinus = false;
            this.treeFeatures.Size = new System.Drawing.Size(477, 398);
            this.treeFeatures.TabIndex = 1;
            this.treeFeatures.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeFeatures_AfterCheck);
            this.treeFeatures.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeFeatures_MouseDown);
            // 
            // FeaturesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeFeatures);
            this.Name = "FeaturesView";
            this.Size = new System.Drawing.Size(477, 398);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem analyzeMarkedFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixMarkedFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreMarkedFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator seperatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMarkedFeatureToolStripMenuItem;
        private System.Windows.Forms.TreeView treeFeatures;
    }
}
