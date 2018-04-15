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
    public partial class UserPage : Form
    {
        FormHandler fh = FormHandler.Instance;

        public UserPage()
        {
            InitializeComponent();
        }

        public void UserPage_FormClosed()
        {
            fh.GetLogin().Close();
        }
    }
}
