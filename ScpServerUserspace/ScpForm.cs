using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HidReport.Contract.Enums;
using log4net;
using NativeBusControl;
using NativeLayer.Contract;
using NativeLayer.Exceptions;
using ScpServer.Properties;

namespace ScpServer
{
    public partial class ScpForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RootHub.RootHub _rootHub = new RootHub.RootHub();
        private void For_Load(object sender, EventArgs e)
        {
            Icon = Resources.Scp_All;

            Log.DebugFormat("++ {0} [{1}]", Assembly.GetExecutingAssembly().Location,
                Assembly.GetExecutingAssembly().GetName().Version);

            tmrUpdate.Enabled = true;
            btnStart_Click(sender, e);
        }

        private void For_Close(object sender, FormClosingEventArgs e)
        {
            _rootHub.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (_rootHub.Start())
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
            }
            catch (RootHubAlreadyStartedException rhex)
            {
                Log.Fatal(rhex.Message);
                MessageBox.Show(rhex.Message, "Error starting server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_rootHub.Stop())
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvDebug.Items.Clear();
        }

        private void btnMotor_Click(object sender, EventArgs e)
        {
            var Target = (Button) sender;
            byte Left = 0x00, Right = 0x00;

            if (Target == btnBoth)
            {
                Left = 0xFF;
                Right = 0xFF;
            }
            else if (Target == btnLeft) Left = 0xFF;
            else if (Target == btnRight) Right = 0xFF;

            foreach (var pad in _rootHub.Pads.Where(p=> p.State == DsState.Connected))
            {
                pad.Rumble(Left, Right);
            }
        }

        private void btnPair_Click(object sender, EventArgs e)
        {
            foreach (var pad in _rootHub.Pads)
            {
                pad.Pair(_rootHub.BluetoothHostAddress);
            }
        }

        protected void btnDisconnect_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < Pad.Length; index++)
            {
                if (Pad[index].Checked)
                {
                    _rootHub.Pads[index].Disconnect();
                    break;
                }
            }
        }

        protected void btnSuspend_Click(object sender, EventArgs e)
        {
            _rootHub.Suspend();
        }

        protected void btnResume_Click(object sender, EventArgs e)
        {
            _rootHub.Resume();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            bool bSelected = false, bDisconnect = false, bPair = false;

            lblHost.Text = _rootHub.Dongle;
            lblHost.Enabled = btnStop.Enabled;

            for (var index = 0; index < Pad.Length; index++)
            {
                Pad[index].Text = _rootHub.Pads[index].ToString();
                Pad[index].Enabled = _rootHub.Pads[index].State == DsState.Connected;
                Pad[index].Checked = Pad[index].Enabled && Pad[index].Checked;

                bSelected = bSelected || Pad[index].Checked;
                bDisconnect = bDisconnect || _rootHub.Pads[index].Connection == DsConnection.Bluetooth;

                bPair = bPair ||
                        (Pad[index].Checked && _rootHub.Pads[index].Connection == DsConnection.Usb &&
                         _rootHub.BluetoothHostAddress != null
                         && !_rootHub.BluetoothHostAddress.Equals(_rootHub.Pads[index].HostAddress));
            }

            btnBoth.Enabled = btnLeft.Enabled = btnRight.Enabled = btnOff.Enabled = bSelected && btnStop.Enabled;

            btnPair.Enabled = bPair && bSelected && btnStop.Enabled && _rootHub.Pairable;

            btnClear.Enabled = lvDebug.Items.Count > 0;
        }

        private void lvDebug_Enter(object sender, EventArgs e)
        {
            ThemeUtil.UpdateFocus(lvDebug.Handle);
        }

        private void Button_Enter(object sender, EventArgs e)
        {
            ThemeUtil.UpdateFocus(((Button) sender).Handle);
        }
    }
}