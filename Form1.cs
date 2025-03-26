namespace ScreenBlackOut
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public Form1()
        {
            InitializeComponent();
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


        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the close
                this.Hide();     // Just hide the window instead
                trayIcon.ShowBalloonTip(1000, "Minimized to Tray", "App is still running here!", ToolTipIcon.Info);
            }
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

        private void InitializeComponent()
        {
            btnToggleBlackout = new Button();
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
            // Form1
            // 
            ClientSize = new Size(284, 261);
            Controls.Add(btnToggleBlackout);
            Name = "Form1";
            ResumeLayout(false);
        }

        private void btnToggleBlackout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Manual toggle clicked! (This will trigger blackout later)");

        }
    }
}
