using Game1.StateManagement;
using Microsoft.Xna.Framework;

namespace Game1.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {

        Game _game {get; set; }
        public PauseMenuScreen(Game game) : base("Paused", game)
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");
            _game = game;
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_game));
        }
    }
}
