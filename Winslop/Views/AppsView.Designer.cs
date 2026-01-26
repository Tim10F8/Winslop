namespace Winslop.Views
{
    partial class AppsView
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
            this.checkedListBoxApps = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkedListBoxApps
            // 
            this.checkedListBoxApps.BackColor = System.Drawing.Color.White;
            this.checkedListBoxApps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxApps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.checkedListBoxApps.FormattingEnabled = true;
            this.checkedListBoxApps.Items.AddRange(new object[] {
            "No inspection yet"});
            this.checkedListBoxApps.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxApps.Name = "checkedListBoxApps";
            this.checkedListBoxApps.Size = new System.Drawing.Size(503, 354);
            this.checkedListBoxApps.Sorted = true;
            this.checkedListBoxApps.TabIndex = 337;
            // 
            // AppsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBoxApps);
            this.Name = "AppsView";
            this.Size = new System.Drawing.Size(503, 354);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxApps;
    }
}
