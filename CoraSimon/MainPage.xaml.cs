using CommunityToolkit.Maui.Views;

namespace CoraSimon
{
    public partial class MainPage : ContentPage
    {
        private SimonGameLogic _gameLogic;
        private bool _isPlayingSequence;

        public MainPage()
        {
            InitializeComponent();
            _gameLogic = new SimonGameLogic();
            BindingContext = _gameLogic;
        }

        // Handle the Start Button Click
        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            _gameLogic.ResetGame();
            await PlaySequence();
        }

        // Handle Color Button Clicks
        private async void OnColorButtonClicked(object sender, EventArgs e)
        {
            if (_isPlayingSequence) return; // Ignore input during sequence playback
            
            var button = (Button)sender;
            var color = GetColorNameByButton(button);
            Console.WriteLine($"Color {color} clicked.");
            
            bool isCorrect = _gameLogic.InputColor(color);
            Console.WriteLine($"Color {color} correct: {isCorrect}.");

            // Provide feedback (flash effect)
            FlashButton(button);
            if (!_gameLogic.IsGameOver)
            {
                PlaySound(color);
                if (isCorrect && _gameLogic.GetStep() == 0)
                {
                    await PlaySequence();
                }
            }
            else
            {
                PlaySound("Wrong");
                await DisplayAlert("Game Over", $"Your score: {_gameLogic.Score}", "Try Again");
            }
        }

        // Play the sequence (lights and sounds)
        private async Task PlaySequence()
        {
            _isPlayingSequence = true;
            await Task.Delay(500);

            var sequence = _gameLogic.GetSequence();
            Console.WriteLine("Playing sequence");
            Console.WriteLine(string.Join(",", sequence));
            foreach (var color in sequence)
            {
                Button button = GetButtonByColor(color);
                FlashButton(button);
                PlaySound(color); // Play the sound
                await Task.Delay(500); // Pause between colors
            }

            _isPlayingSequence = false;
        }

        // Flash a button to simulate light-up
        private async void FlashButton(Button button)
        {
            var originalColor = GetColorByButton(button);
            button.BackgroundColor = originalColor.WithLuminosity(0.8f);
            await Task.Delay(300); // Flash duration
            button.BackgroundColor = originalColor; // Restore color
        }

        // Map color names to buttons
        private Button GetButtonByColor(string color)
        {
            return color switch
            {
                "Red" => RedButton,
                "Green" => GreenButton,
                "Blue" => BlueButton,
                "Yellow" => YellowButton,
                _ => throw new ArgumentException("Invalid color")
            };
        }

        private string GetColorNameByButton(Button button)
        {
            
            if (button == RedButton) return "Red";
            else if (button == GreenButton) return "Green";
            else if (button == BlueButton) return "Blue";
            else if (button == YellowButton) return "Yellow";
            else return string.Empty;
        }

        private Color GetColorByButton(Button button)
        {
            if (button == RedButton) return Colors.Red;
            else if (button == GreenButton) return Colors.Green;
            else if (button == BlueButton) return Colors.Blue;
            else if (button == YellowButton) return Colors.Yellow;
            else return Colors.Black;
        }

        private async void PlaySound(string color)
        {
            var soundFile = color switch
            {
                "Red" => "red.mp3",
                "Green" => "green.mp3",
                "Blue" => "blue.mp3",
                "Yellow" => "yellow.mp3",
                "Wrong" => "wrong.mp3",
                _ => throw new ArgumentException("Invalid color")
            };

            SoundPlayer.Source = MediaSource.FromResource(soundFile);
            SoundPlayer.Play();
            await Task.Delay(500); // Wait for the sound to finish
        }
    }

}
