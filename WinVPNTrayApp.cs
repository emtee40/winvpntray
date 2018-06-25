using System;
using System.Windows.Forms;

namespace WinVPNTray
{
    class WinVPNTrayApp : ApplicationContext
    {
        private NotifyIcon vpnTray;

        protected Object _changeStateLock = new Object();

        public WinVPNTrayApp()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();

            if (Properties.Settings.Default.VPNConnName == "NONE")
                new QueryConnections().ShowDialog();

            // Start VPN
            VPNStartCmd_Click(null, null);
        }

        private void InitializeComponent()
        {
            vpnTray = new NotifyIcon();
            vpnTray.BalloonTipText = "WinVPNTray";
            GetVPNStatus();

            vpnTray.ContextMenu= new ContextMenu();

            // Start MenuItem
            MenuItem vpnStartCmd = new MenuItem();
            vpnStartCmd.Text = "Start VPN";
            vpnStartCmd.Click += VPNStartCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnStartCmd);

            // Restart MenuItem
            MenuItem vpnRestartCmd = new MenuItem();
            vpnRestartCmd.Text = "Restart VPN";
            vpnRestartCmd.Click += VPNRestartCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnRestartCmd);

            // Stop MenuItem
            MenuItem vpnStopCmd = new MenuItem();
            vpnStopCmd.Text = "Stop VPN";
            vpnStopCmd.Click += VPNStopCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnStopCmd);

            // Add separator
            vpnTray.ContextMenu.MenuItems.Add("-");

            // Exit MenuItem
            MenuItem vpnExitCmd = new MenuItem();
            vpnExitCmd.Text = "Exit";
            vpnExitCmd.Click += VpnExitCmd_Click; ;
            vpnTray.ContextMenu.MenuItems.Add(vpnExitCmd);

            vpnTray.Visible = true;
        }



        private void VPNStartCmd_Click(object sender, EventArgs e)
        {
            lock (_changeStateLock)
            {
                if (RASutils.GetConnectionStatus() == RASutils.ConnectionStatus.Connected) return;
                RASutils.ConnectVPN();
                GetVPNStatus();
            }
        }

        private void VPNRestartCmd_Click(object sender, EventArgs e)
        {
            lock (_changeStateLock)
            {
                if (RASutils.GetConnectionStatus() != RASutils.ConnectionStatus.Connected) return;
                DialogResult q = MessageBox.Show("Are you sure you want to restart the VPN service?", "Restart VPN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (q == DialogResult.No) return;

                RASutils.DisconnectVPN();
                GetVPNStatus();

                if (RASutils.GetConnectionStatus() != RASutils.ConnectionStatus.Disconnected) return;
                RASutils.ConnectVPN();
                GetVPNStatus();
            }
        }

        private void VPNStopCmd_Click(object sender, EventArgs e)
        {
            lock (_changeStateLock)
            {
                if (RASutils.GetConnectionStatus() != RASutils.ConnectionStatus.Connected) return;
                DialogResult q = MessageBox.Show("Are you sure you want to stop the VPN service?", "Stop VPN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (q == DialogResult.No) return;

                RASutils.DisconnectVPN();
                GetVPNStatus();
            }
        }

        private void GetVPNStatus()
        {
  
            switch (RASutils.GetConnectionStatus())
            {
                case RASutils.ConnectionStatus.Connected:
                    vpnTray.Icon = Properties.Resources.on;
                    break;
                default:
                    vpnTray.Icon = Properties.Resources.off;
                    break;
            }
        }

        private void VpnExitCmd_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            //Cleanup so that the icon will be removed when the application is closed
            vpnTray.Visible = false;
        }

    }
}
