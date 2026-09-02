using EM.Comax.ShukHerzl.Common;

namespace Em.Comax.ShukHerzel.Client
{
    public partial class GenerateUserPasswordForm : Form
    {
        public GenerateUserPasswordForm()
        {
            InitializeComponent();
            txtPassword.Focus();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("יש להזין סיסמה.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                txtEncryptedResult.Text = EncryptManagement.PasswordEncrypt(password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בהצפנת הסיסמה: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopyPassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEncryptedResult.Text))
            {
                MessageBox.Show("אין סיסמה מוצפנת להעתקה.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Clipboard.SetText(txtEncryptedResult.Text);
                MessageBox.Show("סיסמה מוצפנת הועתקה ללוח בהצלחה!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בהעתקה ללוח: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string password = txtEncryptedPassword.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("יש להזין סיסמה.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                txtDecryptedResult.Text = EncryptManagement.PasswordDecrypt(password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בשיחזור הסיסמה: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtDecryptedResult.Text))
            {
                MessageBox.Show("אין סיסמה להעתקה.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Clipboard.SetText(txtDecryptedResult.Text);
                MessageBox.Show("סיסמה הועתקה ללוח בהצלחה!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בהעתקה ללוח: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
