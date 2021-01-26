using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using Microsoft.VisualBasic;


namespace JelszoKezelo
{
    public partial class Bejelentkezes : Form
    {
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
            cancelButton.Text = "&Cancel";
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
            usrData felhasznalo = new usrData();
            InitializeComponent();
        }

        public string Masterpw()
        {
            string input;
            do
            {
                //input = Interaction.InputBox("Kérem adjon meg egy jelszót mellyel a jelszavait titkosítja.(Legalább 15 karakter hosszú)", "MasterPassword", "");
                input = "Adja meg a master jelszót(min 15 karakter hosszu)";
                if(ShowInputDialog(ref input) == DialogResult.Cancel)
                {
                    return null;
                }
            }
            while (input.Length < 15);
            return input;
        }

        public int Registration(string fnevHash, string jelszoHash)
        {
            if (!File.Exists("data.txt"))
                File.Create("data.txt");
            else
            {
                StreamReader rdr = new StreamReader("data.txt");
                while(!rdr.EndOfStream)
                {
                    string[] data = rdr.ReadLine().Split(':');
                    if(data[0] == fnevHash)
                    {
                        rdr.Close();
                        return 1;
                    }
                }
                rdr.Close();
            }

            string master = Masterpw();
            if (master == null) return 1;
            StreamWriter wr = File.AppendText("data.txt");
            wr.WriteLine($"{fnevHash}:{jelszoHash}:{Hash(master)}");
            wr.Close();
            return 0;

        }

        public int Login(string fnevHash, string jelszoHash)
        {
            if(File.Exists("data.txt"))
            {
                StreamReader rdr = new StreamReader("data.txt");
                while(!rdr.EndOfStream)
                {
                    string[] data = rdr.ReadLine().Split(':');
                    if(data[0] == fnevHash && data[1] == jelszoHash)
                    {
                        rdr.Close();
                        return 0;
                    }
                }
                rdr.Close();
            }
            return 1;
        }

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

        public int CheckIfEmpty(TextBox tb)
        {
            if(tb.Text == "")
                return 0;
            else
                return 1; 
        }

        private void btReg_Click(object sender, EventArgs e)
        {
            if (CheckIfEmpty(tbFelh) == 0 || CheckIfEmpty(tbJelsz) == 0)
            {
                lbTest.Text = "Kérem töltsön ki minden mezőt!";
                lbTest.Visible = true;
                return;
            }
            string fnevHash = Hash(tbFelh.Text);
            string jelszoHash = Hash(tbJelsz.Text);
            if(Registration(fnevHash, jelszoHash) == 0)
            {
                lbTest.Text = "Sikeres regisztracio";
            }
            else
            {
                lbTest.Text = "Sikertelen regisztracio";
            }
            lbTest.Visible = true;
        }

        private void btBej_Click(object sender, EventArgs e)
        {
            if(CheckIfEmpty(tbFelh) == 0 || CheckIfEmpty(tbJelsz) == 0)
            {
                lbTest.Text = "Kérem töltsön ki minden mezőt!";
                lbTest.Visible = true;
                return;
            }
            string fnevHash = Hash(tbFelh.Text);
            string jelszoHash = Hash(tbJelsz.Text);
            if(Login(fnevHash, jelszoHash) == 0)
            {
                lbTest.Text = "Sikeresen bejelentkezett";
            }
            else
            {
                lbTest.Text = "Helytelen felhasznalonev v jelszo";
            }
            lbTest.Visible = true;
        }

        private void Bejelentkezes_Load(object sender, EventArgs e)
        {

        }
    }
}
