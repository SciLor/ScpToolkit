using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativeLayer.Contract;
using RootHub;

namespace Test
{
    [TestClass]
    public class TestDeviceListener
    {
        private bool _arrived = false;
        private bool _disconnected = false;
        [TestMethod]
        //You need to manually connect disconnect item
        public async Task TestMethod1()
        {
            DeviceEventListener listener = new DeviceEventListener();
            listener.DeviceChangedEventHandler+=ListenerOnDeviceChangedEventHandler;
            while (!(_arrived && _disconnected))
            {
                await Task.Delay(1000);
            }
        }

        private void ListenerOnDeviceChangedEventHandler(object sender, DeviceEventListener.DeviceChangedEventArgs eventArgs)
        {
            Trace.WriteLine($"Event received {eventArgs}");
            if(eventArgs.Type == Notified.Arrival)
                _arrived = true;
            if (eventArgs.Type == Notified.QueryRemove)
                _disconnected = true;
        }
    }
}
