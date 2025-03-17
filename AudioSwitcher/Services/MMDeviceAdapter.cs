using AudioSwitcher.Services;
using CoreAudioApi;
using CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher
{
    public class MMDeviceAdapter
    {
        private static PropertyKey DEVPKEY_Device_FriendlyName = new PropertyKey { fmtid = new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), pid = 14 };

        public AudioDevice Convert(IMMDevice mmDevice)
        {
            mmDevice.GetId(out string deviceId);

            mmDevice.OpenPropertyStore(EStgmAccess.STGM_READ, out var propertyStore);
            propertyStore.GetValue(ref DEVPKEY_Device_FriendlyName, out var friendlyName);

            return new AudioDevice
            {
                Id = deviceId,
                Name = friendlyName.Value.ToString(),
            };
        }
    }
}
