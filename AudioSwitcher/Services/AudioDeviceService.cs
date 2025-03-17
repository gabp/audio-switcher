using CoreAudioApi;
using CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher.Services
{
    public class AudioDeviceService
    {
        private ComObjectFactory comObjectFactory;
        private MMDeviceCollectionAdapter mmDeviceCollectionAdapter;

        private List<string> whiteListedNames = new List<string> { "Focusrite", "VW246" };

        public AudioDeviceService(ComObjectFactory comObjectFactory, MMDeviceCollectionAdapter mmDeviceCollectionAdapter)
        {
            this.comObjectFactory = comObjectFactory;
            this.mmDeviceCollectionAdapter = mmDeviceCollectionAdapter;
        }

        public List<AudioDevice> GetAudioDevices()
        {
            this.comObjectFactory.IMMDeviceEnumerator().EnumAudioEndpoints(
                EDataFlow.eRender,
                EDeviceState.DEVICE_STATE_ACTIVE,
                out IMMDeviceCollection deviceCollection);

            var audioDevices = this.mmDeviceCollectionAdapter.Convert(deviceCollection);

            return audioDevices;
        }

        public void SetDefaultDevice(string deviceId)
        {
            this.comObjectFactory.IPolicyConfig().SetDefaultEndpoint(
                deviceId,
                ERole.eMultimedia);
        }

        public void NextDevice()
        {
            var currentDevices = this.GetAudioDevices();

            var defaultIndex = currentDevices.FindIndex(d => d.IsDefaultDevice);

            if (defaultIndex == -1 || currentDevices.Count == 0)
            {
                this.SetDefaultDevice(currentDevices[0].Id);
                return;
            }

            for (int i=1; i<currentDevices.Count; i++)
            {
                var nextIndex = (defaultIndex + i) % currentDevices.Count;

                if (this.isDeviceWhiteListed(currentDevices[nextIndex].Name))
                {
                    this.SetDefaultDevice(currentDevices[nextIndex].Id);
                    return;
                }
            }
        }

        private bool isDeviceWhiteListed(string name)
        {
            return this.whiteListedNames.Any(n => name.Contains(n));
        }
    }
}
