using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Winslop
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeUI();
            checkDonated.Checked = DonationHelper.HasDonated();
            panelForm.BackColor = checkDonated.Checked ? System.Drawing.Color.FromArgb(216, 173, 183) : System.Drawing.Color.FromArgb(195, 199, 203);
        }

        private void InitializeUI()
        {
            // Update version label
            this.lblVersionInfo.Text = $"Version {Program.GetAppVersion()} ";
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.paypal.me/Hasanovic7",
                UseShellExecute = true
            });
        }

        private void linkCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/builtbybel/Winslop/releases");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkDonated_CheckedChanged(object sender, EventArgs e)
        {
            DonationHelper.SetDonationStatus(checkDonated.Checked);
        }
    }
}