using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace WinVPNTray
{
    public partial class QueryConnections : Form
    {
        bool buttonPushed = false;
        protected Object _saveLock = new Object();

        public QueryConnections()
        {
            InitializeComponent();
        }

        private void QueryConnections_Load(object sender, EventArgs e)
        {
            foreach (NetworkInterface netFace in NetworkInterface.GetAllNetworkInterfaces())
                if(netFace.NetworkInterfaceType == NetworkInterfaceType.Ppp) lstConnections.Items.Add(netFace.Name);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(lstConnections.SelectedIndex == -1)
                MessageBox.Show("Please select a connection before pressing OK");
            else
            {
                lock (_saveLock) {
                    Properties.Settings.Default.VPNConnName = lstConnections.SelectedItem.ToString();
                    Properties.Settings.Default.Save();
                    buttonPushed = true;
                    this.Close();
                }
            }

        }

        private void QueryConnections_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!buttonPushed) Application.Exit();
        }
    }
}
