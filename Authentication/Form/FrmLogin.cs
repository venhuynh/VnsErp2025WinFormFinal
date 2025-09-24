using System;

namespace Authentication.Form
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            string username = UserNameTextEdit.Text.Trim();
            string password = PasswordTextEdit.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Please enter both username and password.", "Validation Error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Login successful.", "Success",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Invalid username or password.", "Authentication Failed",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Placeholder for actual authentication logic.
            // Replace with database or service call as needed.
            return username == "admin" && password == "password";
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // Close the login form
            Close();
        }
    }
}