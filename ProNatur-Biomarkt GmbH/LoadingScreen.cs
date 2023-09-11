using System;
using System.Windows.Forms;

namespace ProNatur_Biomarkt_GmbH
{
    public partial class LoadingScreen : Form
    {
        //int default to 0 when defined
        private int loadingBarValue;

        public LoadingScreen()
        {
            InitializeComponent();
        }
        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            loadingbarTimer.Start();

        }

        private void loadingbarTimer_Tick(object sender, EventArgs e)
        {
            loadingBarValue += 1;
            labelLoadingProgress.Text = loadingBarValue.ToString() + "%";
            progressBarLoading.Value = loadingBarValue;

            if (loadingBarValue >= progressBarLoading.Maximum)
            {
                loadingbarTimer.Stop();

                MainMenuScreen mainMenuScreen = new MainMenuScreen();
                mainMenuScreen.Show();

                this.Hide();

            }
        }

    }
}
