using AudioSwitcher.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AudioSwitcher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            ConfigureServices(services);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var form1 = serviceProvider.GetRequiredService<Form1>();
                Application.Run(form1);
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<Form1>();

            services.AddSingleton<ComObjectFactory>();
            services.AddSingleton<MMDeviceCollectionAdapter>();
            services.AddSingleton<MMDeviceAdapter>();
            services.AddSingleton<AudioDeviceService>();
        }
    }
}