﻿using System;
using System.Windows.Forms;
using FreelancerModStudio.Properties;

namespace FreelancerModStudio
{
    internal partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            Text = String.Format(Strings.AboutText, Helper.Assembly.Name);
            lblProductName.Text = Helper.Assembly.Name;
            lblVersion.Text = String.Format(Strings.AboutVersion, Helper.Assembly.Version);
            lblCopyright.Text = Helper.Assembly.Copyright;
            lblCompanyName.Text = Helper.Assembly.Company;
            txtDescription.Text = Helper.Assembly.Description;
        }
    }
}
