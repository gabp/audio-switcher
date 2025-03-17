using AudioSwitcher.Services;
using Shortcut;
using System.Reflection;

namespace AudioSwitcher
{
    public partial class Form1 : Form
    {
        private AudioDeviceService audioDeviceService;

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private List<ToolStripMenuItem> toolStripMenuItems;

        public Form1(AudioDeviceService audioDeviceService)
        {
            this.audioDeviceService = audioDeviceService;

            InitializeComponent();

            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new ContextMenuStrip();
            this.toolStripMenuItems = new List<ToolStripMenuItem>();

            this.ClientSize = new Size(292, 266);
            this.Text = "Notify Icon Example";
            this.notifyIcon = new NotifyIcon(this.components);
            this.loadIcon();

            this.notifyIcon.Text = "Form1 (NotifyIcon example)";
            this.notifyIcon.Visible = true;

            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Click += NotifyIcon_Click;

            this.registerShortcut();
        }

        private void loadIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{this.GetType().Namespace}.playback.ico";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                this.notifyIcon.Icon = new Icon(stream);
            }
        }

        private void registerShortcut()
        {
            var hotkeyBinder = new HotkeyBinder();

            hotkeyBinder.Bind(Modifiers.Alt, Keys.Space).To(() => this.audioDeviceService.NextDevice());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            this.toolStripMenuItems.Clear();
            this.contextMenuStrip.Items.Clear();

            var devices = this.audioDeviceService.GetAudioDevices();

            devices.ForEach(device =>
            {
                var menuItem = new ToolStripMenuItem
                {
                    Text = device.Name,
                    Tag = device.Id,
                    Checked = device.IsDefaultDevice,
                };

                menuItem.Click += ToolStripItem_Click;

                this.toolStripMenuItems.Add(menuItem);
                this.contextMenuStrip.Items.Add(menuItem);
            });

            this.contextMenuStrip.Show();
        }

        private void ToolStripItem_Click(object? sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var deviceId = (string)menuItem.Tag;

            this.audioDeviceService.SetDefaultDevice(deviceId);
        }
    }
}