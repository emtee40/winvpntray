using System.Diagnostics;

namespace WinVPNTray
{
    class RASutils
    {
        public enum ConnectionStatus
        {
            Connected,
            Disconnected
        }

        public static ConnectionStatus GetConnectionStatus()
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "C:\\Windows\\System32\\rasdial.exe";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (output.Contains("Connected to")) return ConnectionStatus.Connected;
            return ConnectionStatus.Disconnected;
        }

        public static void ConnectVPN()
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "C:\\Windows\\System32\\rasdial.exe";
            p.StartInfo.Arguments = Properties.Settings.Default.VPNConnName;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }

        public static void DisconnectVPN()
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "C:\\Windows\\System32\\rasdial.exe";
            p.StartInfo.Arguments = Properties.Settings.Default.VPNConnName + " /DISCONNECT";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }
    }
}
