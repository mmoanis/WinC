using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinC
{
    public partial class FormInputPrompt : Form
    {
        public FormInputPrompt(string caption, string message)
        {
            InitializeComponent();

            lbl.Text = message;
            this.Text = caption;
        }

        private void frmInputPrompt_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public string InputText
        {
            get
            {
                return txtInput.Text;
            }
        }
    }
}
