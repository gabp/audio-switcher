using CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher.Services
{
    public class ComObjectFactory
    {
        private IMMDeviceEnumerator? mmDeviceEnumerator;
        private IPolicyConfig? policyConfig;

        public IMMDeviceEnumerator IMMDeviceEnumerator()
        {
            if (this.mmDeviceEnumerator == null)
            {
                var type = Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"));
                this.mmDeviceEnumerator = (IMMDeviceEnumerator)Activator.CreateInstance(type);
            }

            return this.mmDeviceEnumerator;
        }

        public IPolicyConfig IPolicyConfig()
        {
            if (this.policyConfig == null)
            {
                var type = Type.GetTypeFromCLSID(new Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9"));
                this.policyConfig = (IPolicyConfig)Activator.CreateInstance(type);
            }

            return this.policyConfig;
        }
    }
}
