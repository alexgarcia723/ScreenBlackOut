using System.Runtime.InteropServices;


namespace ScreenBlackOut
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        // Windows message constant for hotkey pressed
        private const int WM_HOTKEY = 0x0312;

        // Modifier key flags used by Windows
        private const int MOD_ALT = 0x1;
        private const int MOD_CONTROL = 0x2;
        private const int MOD_SHIFT = 0x4;
        private const int MOD_WIN = 0x8;

        // An arbitrary ID to identify our hotkey
        private const int HOTKEY_ID = 9000;

        // Declare native Windows functions from user32.dll
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public Form1()
        {
            InitializeComponent();

            //Force handle creaion
            var unused = this.Handle;

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Screen Blackout Tool";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Exit", null, OnExit);
            trayMenu.Items.Add("Toggle", null, OnToggle);
            trayMenu.Items.Add("Configure", null, OnConfigure);

            trayIcon.ContextMenuStrip = trayMenu;

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            foreach (var screen in Screen.AllScreens)
            {
                string displayName = screen.DeviceName.Replace(@"\\.\", "");
                clbScreens.Items.Add(displayName, true);
            }


        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;

                trayIcon.ShowBalloonTip(1000, "Minimized to Tray", "App is still running here!", ToolTipIcon.Info);
            }

            UnregisterHotKey(this.Handle, HOTKEY_ID);

        }

        private void OnExit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false; // Hide the icon when exiting
            Application.Exit();       // Closes the app
        }

        private void OnToggle(object? sender, EventArgs e)
        {
            // This is where we'll trigger blackout (hook it up later)
            MessageBox.Show("Toggle clicked! (Placeholder)");
        }

        private void OnConfigure(object? sender, EventArgs e)
        {
            // If the window is hidden, show it
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                // Act as if the user clicked the button
                btnToggleBlackout.PerformClick();
            }
        }


        private void InitializeComponent()
        {
            btnToggleBlackout = new Button();
            clbScreens = new CheckedListBox();
            SuspendLayout();
            // 
            // btnToggleBlackout
            // 
            btnToggleBlackout.AccessibleName = "";
            btnToggleBlackout.BackColor = SystemColors.ControlLightLight;
            btnToggleBlackout.Location = new Point(63, 198);
            btnToggleBlackout.Name = "btnToggleBlackout";
            btnToggleBlackout.Size = new Size(138, 51);
            btnToggleBlackout.TabIndex = 0;
            btnToggleBlackout.Text = "Toggle Black Out";
            btnToggleBlackout.UseVisualStyleBackColor = false;
            btnToggleBlackout.Click += btnToggleBlackout_Click;
            // 
            // clbScreens
            // 
            clbScreens.CheckOnClick = true;
            clbScreens.FormattingEnabled = true;
            clbScreens.Location = new Point(36, 12);
            clbScreens.Name = "clbScreens";
            clbScreens.Size = new Size(200, 94);
            clbScreens.TabIndex = 1;
            // 
            // Form1
            // 
            ClientSize = new Size(284, 261);
            Controls.Add(clbScreens);
            Controls.Add(btnToggleBlackout);
            Name = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        private void btnToggleBlackout_Click(object sender, EventArgs e)
        {
            var selectedScreens = clbScreens.CheckedItems.Cast<string>().ToList();

            if (selectedScreens.Count == 0)
            {
                MessageBox.Show("No screens selected.");
            }
            else
            {
                string message = "Selected screens:\n" + string.Join("\n", selectedScreens);
                MessageBox.Show(message);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Register Ctrl + Shift + B
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (int)Keys.B);
        }
    }
}
