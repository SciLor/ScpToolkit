using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Config;
using HidReport.Contract.Core;
using HidReport.Core;
using HidSharp.ReportDescriptors.Parser;
using log4net;
using MadWizard.WinUSBNet;
using NativeLayer.Contract;
using NativeLayer.Usb.Usb_specification;
using NativeLayer.Utilities;

namespace NativeLayer.UsbDevices
{
    /// <summary>
    ///     Low-level representation of an Scp-compatible Usb device.
    /// </summary>
    public abstract class WinUsbGamepadBase : IUsbDevice, IGamepad
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public abstract Guid DeviceClassGuid { get; }

        public string Path { get; }

        public int VendorId => Dev.Descriptor.VID;

        public int ProductId => Dev.Descriptor.PID;


        //TODO: private readonly IObservable<long> _outputReportSchedule = Observable.Interval(TimeSpan.FromMilliseconds(10), Scheduler.Default);

        private CancellationTokenSource _hidCancellationTokenSource = new CancellationTokenSource();

        private IDisposable _outputReportTask;

        private readonly TaskQueue _inputReportQueue = new TaskQueue();

        protected USBDevice Dev;

        protected WinUsbGamepadBase(string devicePath)
        {
            Path = devicePath;
            Dev = new USBDevice(devicePath);
            if (Dev == null)
                throw new Exception();

            Task.Run( ()=>HidWorker(_hidCancellationTokenSource.Token));

            //TODO: _outputReportTask = _outputReportSchedule.Subscribe(tick => Process(DateTime.Now));

            Log.Debug($"-- Started Device Instance [{Instance}] Local [{Dev}] ");

            // try to retrieve HID Report Descriptor
            var buffer = new byte[512];
            Dev.ControlOut(UsbHidRequestType.GetDescriptor, UsbHidRequest.GetDescriptor, ToValue(UsbHidClassDescriptorType.Report), 0, buffer);
            //TODO: log
            // store report descriptor
            ReportDescriptor.Parse(buffer);
        }

        void IDisposable.Dispose()
        {
            _outputReportTask?.Dispose();

            _hidCancellationTokenSource.Cancel();
            _hidCancellationTokenSource = new CancellationTokenSource();

            _outputReportTask?.Dispose();

            Dev.Dispose();
            Dev = null;
        }

        protected static ushort ToValue(UsbHidClassDescriptorType type, byte index = 0x00)
        {
            return BitConverter.ToUInt16(new[] { index, (byte)type }, 0);
        }

        protected static ushort ToValue(UsbHidReportRequestType type, UsbHidReportRequestId id)
        {
            return BitConverter.ToUInt16(new[] { (byte)id, (byte)type }, 0);
        }

        #region WinUSB wrapper methods

        protected int ReadIntPipe(byte[] buffer)
        {
            return Dev.Pipes[0].Read(buffer);
            return -1;
        }

        protected void SendTransfer(byte requestType, byte request, ushort value, byte[] buffer, ref int transfered)
        {
            Dev.ControlOut(requestType, request, value, 0, buffer);
            Dev.ControlTransfer(requestType, request, value, 0, buffer);
        }

        #endregion

        protected USBPipe IntPipeIdIn  ;
        protected USBPipe IntPipeIdOut ;
        protected USBPipe BulkPipeIdIn ;
        protected USBPipe BulkPipeIdOut;

        protected virtual bool InitializeDevice()
        {
            try
            {
                foreach (USBPipe pipe in Dev.Pipes)
                {
                    USBPipe selectedPipe = null;
                    //TODO: split
                    //UsbdPipeTypeBulk
                    //BulkPipeIdIn = pipeInfo.PipeId;
                    //BulkPipeIdOut = pipeInfo.PipeId;
                    //UsbdPipeTypeInterrupt
                    //IntPipeIdIn = pipeInfo.PipeId;
                    //IntPipeIdOut = pipeInfo.PipeId;
                    if (pipe.IsIn && pipe.Policy.AllowPartialReads)
                    {
                        BulkPipeIdIn = pipe;
                        selectedPipe = pipe;
                    }
                    else if (pipe.IsIn && pipe.Policy.AllowPartialReads)
                    {
                        IntPipeIdIn = pipe;
                        selectedPipe = pipe;
                    }
                    else if (pipe.IsOut && pipe.Policy.AllowPartialReads)
                    {
                        BulkPipeIdOut = pipe;
                        selectedPipe = pipe;
                    }
                    else if (pipe.IsOut && pipe.Policy.AllowPartialReads)
                    {
                        IntPipeIdOut = pipe;
                        selectedPipe = pipe;
                    }
                    selectedPipe?.Flush();
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0} {1}", ex.HelpLink, ex.Message);
                throw;
            }
        }
    
        /// <summary>
        ///     Worker thread polling for incoming Usb interrupts.
        /// </summary>
        /// <param name="o">Task cancellation token.</param>
        private void HidWorker(object o)
        {
            var token = (CancellationToken)o;
            var transfered = 0;
            var buffer = new byte[64];

            Log.Debug("-- Usb Device : HID_Worker_Thread Starting");

            while (!token.IsCancellationRequested)
            {
                try
                {
                    //TODO:
                    //if (ReadIntPipe(buffer, buffer.Length, ref transfered) && transfered > 0)
                    //{
                        var inputReport = ParseHidReport(buffer);
                    //}
                    OnHidReportReceived(inputReport);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Unexpected error: {0}", ex);
                }
            }

            Log.Debug("-- Usb Device : HID_Worker_Thread Exiting");
        }


        #region Protected fields

        protected byte[] Buffer = new byte[64];
        protected byte CableStatus = 0;
        protected string Instance = string.Empty;
        protected DateTime Last = DateTime.Now, Tick = DateTime.Now, Disconnect = DateTime.Now;
        protected uint PacketCounter = 0;
        protected byte PlugStatus = 0;
        protected bool Publish = false;
        protected readonly ReportDescriptorParser ReportDescriptor = new ReportDescriptorParser();

        #endregion

        public event EventHandler<IScpHidReport> HidReportEventHandler;

        protected void OnHidReportReceived(IScpHidReport report)
        {
            if (GlobalConfiguration.Instance.UseAsyncHidReportProcessing)
            {
                _inputReportQueue.Enqueue(() => Task.Run(() => { HidReportEventHandler?.Invoke(this, report); }));
            }
            else
            {
                HidReportEventHandler?.Invoke(this, report);
            }
        }

        public override string ToString()
        {
            return $"WinUsb Pad {Model} {Path} {PacketCounter:X8}";
        }

        protected abstract IScpHidReport ParseHidReport(byte[] buffer);
        public abstract void SendFeedback(IScpFeedback feedback);
        public string Model { get; protected set; }
    }
}