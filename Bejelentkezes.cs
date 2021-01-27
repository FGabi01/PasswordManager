using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace JelszoKezelo
{
    public partial class Bejelentkezes : Form
    {
        //Inputbox a master jelszó megadásához
        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(350, 80);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Master password";
            inputBox.StartPosition = FormStartPosition.CenterScreen;

            System.Windows.Forms.Label label = new Label();
            label.Location = new System.Drawing.Point(10, 5);
            label.Size = new System.Drawing.Size(size.Width - 20, 15);
            label.Text = input;
            label.Visible = true;
            inputBox.Controls.Add(label);

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 20, 23);
            textBox.Location = new System.Drawing.Point(10, 22);
            textBox.Text = "";
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 85 - 85, 50);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Mégse";
            cancelButton.Location = new System.Drawing.Point(size.Width - 85, 50);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        public Bejelentkezes()
        {
            InitializeComponent();
            User.InitializeDB();
        }

        //Bekéri a master jelszót
        public string Masterpw()
        {
            string input;
            do
            {
                input = "Adja meg a master jelszót(min 15 karakter hosszu)";
                if (ShowInputDialog(ref input) == DialogResult.Cancel)
                {
                    return null;
                }
            }
            while (input.Length < 15);
            return input;
        }

        //SHA512 Hash létrehozása egy sztringből
        public string Hash(string Data)
        {
            var data = Encoding.UTF8.GetBytes(Data);
            byte[] result;
            SHA512 shaM = new SHA512Managed();
            result = shaM.ComputeHash(data);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in result)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }

        private void btReg_Click(object sender, EventArgs e)
        {
            //Ellenőrizzük, hogy nem-e üres bármely mező
            if (String.IsNullOrEmpty(tbFelh.Text) || String.IsNullOrEmpty(tbJelsz.Text))
            {
                MessageBox.Show("Kérem töltsön ki minden mezőt!");
                return;
            }
            usrData user = new usrData();
            user.FNev = Hash(tbFelh.Text);
            user.Jelszo = Hash(tbJelsz.Text);
            if (User.GetUser(user).Id == -1)
            {
                user.MasterPw = Hash(Masterpw());
                User.AddUser(user);
                MessageBox.Show("Sikeres regisztracio");
            }
            else
            {
                MessageBox.Show("Sikertelen regisztracio");
            }
        }

        private void btBej_Click(object sender, EventArgs e)
        {
            //Ellenőrizzük, hogy nem-e üres bármely mező
            if (String.IsNullOrEmpty(tbFelh.Text) || String.IsNullOrEmpty(tbJelsz.Text))
            {
                MessageBox.Show("Kérem töltsön ki minden mezőt!");
                return;
            }

            usrData user = new usrData();
            user.FNev = Hash(tbFelh.Text);
            user.Jelszo = Hash(tbJelsz.Text);
            user = User.GetUser(user);
            if (user.Id != -1)
            {

                MessageBox.Show($"Sikeresen bejelentkezett! ID:{user.Id}");

            }
            else
            {
                MessageBox.Show("Helytelen felhasznalonev vagy jelszo!");
            }
        }

        private void Bejelentkezes_Load(object sender, EventArgs e)
        {

        }

        private void cbJelszoMegjelenites_CheckedChanged(object sender, EventArgs e)
        {
            if(cbJelszoMegjelenites.Checked)
            {
                tbJelsz.PasswordChar = '\0';
            }
            else
            {
                tbJelsz.PasswordChar = '*';
            }
        }
    }
}
