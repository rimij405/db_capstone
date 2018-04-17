using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class UserList : Form
    {
        public UserList()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.UserList_FormClosed);
        }

        private void UserList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
