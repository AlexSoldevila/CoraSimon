using System.ComponentModel;

namespace CoraSimon
{
    public class SimonGameLogic : INotifyPropertyChanged
    {
        private List<string> _sequence;
        private int _currentStep;
        private Random _random;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsGameOver { get; private set; }
        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            }
        }

        private int _score;
        public int Best
        {
            get => _best;
            private set
            {
                _best = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Best)));
            }
        }
        private int _best;

        // Constructor
        public SimonGameLogic()
        {
            _sequence = new List<string>();
            _random = new Random();
            ResetGame();
        }

        // Start or reset the game
        public void ResetGame()
        {
            _sequence.Clear();
            _currentStep = 0;
            IsGameOver = false;
            Score = 0;

            // Add the first color
            AddNextColor();
        }

        // Add a new random color to the sequence
        private void AddNextColor()
        {
            var colors = new[] { "Red", "Green", "Blue", "Yellow" };
            var nextColor = colors[_random.Next(colors.Length)];
            Console.WriteLine($"Adding a new color: {nextColor}");
            _sequence.Add(nextColor);
        }

        // Get the sequence for playback
        public List<string> GetSequence()
        {
            return new List<string>(_sequence);
        }

        public int GetStep()
        {
            return _currentStep;
        }

        // Handle user input
        public bool InputColor(string color)
        {
            Console.WriteLine($"User input: {color}, current step: {_currentStep}");

            if (IsGameOver) return false;

            // Check if the user's input matches the current step
            if (_sequence[_currentStep] == color)
            {
                _currentStep++;
                // Check if the round is complete
                if (_currentStep == _sequence.Count)
                {
                    Score++;
                    if(Score > Best)
                        Best = Score;

                    _currentStep = 0;
                    AddNextColor(); // Add a new color for the next round
                }
                return true; // Correct input
            }
            else
            {
                IsGameOver = true; // Incorrect input
                return false;
            }
        }
    }
}
