using AudioSwitcher.Services;
using CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher
{
    public class MMDeviceCollectionAdapter
    {
        private ComObjectFactory comObjectFactory;
        private MMDeviceAdapter mmDeviceAdapter;

        public MMDeviceCollectionAdapter(ComObjectFactory comObjectFactory, MMDeviceAdapter mmDeviceAdapter)
        {
            this.comObjectFactory = comObjectFactory;
            this.mmDeviceAdapter = mmDeviceAdapter;
        }

        public List<AudioDevice> Convert(IMMDeviceCollection mmDeviceCollection)
        {
            List<AudioDevice> audioDevices = new List<AudioDevice>();

            var defaultDeviceId = this.GetDefaultDeviceId();

            mmDeviceCollection.GetCount(out var deviceCount);
            for (uint i = 0; i < deviceCount; i++)
            {
                mmDeviceCollection.Item(i, out var device);

                var convertedDevice = mmDeviceAdapter.Convert(device);
                convertedDevice.IsDefaultDevice = convertedDevice.Id.Equals(defaultDeviceId);

                audioDevices.Add(convertedDevice);
            }

            return audioDevices;
        }

        private string GetDefaultDeviceId()
        {
            this.comObjectFactory.IMMDeviceEnumerator().GetDefaultAudioEndpoint(
                CoreAudioApi.EDataFlow.eRender,
                CoreAudioApi.ERole.eMultimedia,
                out IMMDevice defaultDevice);

            defaultDevice.GetId(out string defaultDeviceId);

            return defaultDeviceId;
        }
    }
}
