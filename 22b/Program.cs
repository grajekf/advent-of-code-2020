using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _22a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var state = GameState.Parse(inputString);
            // var state = new GameState(new int[] { 9, 2, 6, 3, 1 }, new int[] { 5, 8, 4, 7, 10 });

            state = GameState.RunGame(state);
            Console.WriteLine("Game finished!");

            Console.WriteLine(state);

            Console.WriteLine(state.GetScore(Player.One));
            Console.WriteLine(state.GetScore(Player.Two));

        }


    }

    struct GameState
    {
        private IEnumerable<int> _playerOneDeck;
        private IEnumerable<int> _playerTwoDeck;

        public GameState(IEnumerable<int> playerOneDeck, IEnumerable<int> playerTwoDeck)
        {
            _playerOneDeck = playerOneDeck.ToList();
            _playerTwoDeck = playerTwoDeck.ToList();
        }

        public bool IsGameFinished => _playerOneDeck.Count() == 0 || _playerTwoDeck.Count() == 0;
        public Player GetGameWinner()
        {
            if (_playerOneDeck.Count() == 0)
                return Player.Two;

            return Player.One;
        }
        public int GetScore(Player player)
        {
            var deckToCheck = player == Player.Two ? _playerTwoDeck : _playerOneDeck;
            var score = 0;

            for (int i = deckToCheck.Count() - 1; i >= 0; i--)
            {
                var multiplier = deckToCheck.Count() - i;
                score += deckToCheck.ElementAt(i) * multiplier;
            }

            return score;
        }

        public GameState Step()
        {
            var deckOneCopy = _playerOneDeck.ToList();
            var playerOneCard = deckOneCopy.First();
            deckOneCopy.RemoveAt(0);

            var deckTwoCopy = _playerTwoDeck.ToList();
            var playerTwoCard = deckTwoCopy.First();
            deckTwoCopy.RemoveAt(0);

            var winner = GetStepWinner(playerOneCard, playerTwoCard);

            if (winner == Player.One)
            {
                deckOneCopy.Add(playerOneCard);
                deckOneCopy.Add(playerTwoCard);
            }

            if (winner == Player.Two)
            {
                deckTwoCopy.Add(playerTwoCard);
                deckTwoCopy.Add(playerOneCard);
            }

            return new GameState(deckOneCopy, deckTwoCopy);
        }

        public Player GetStepWinner(int playerOneCard, int playerTwoCard)
        {
            if (CanRecurse(playerOneCard, playerTwoCard))
            {
                return RunGame(StateForRecursion(playerOneCard, playerTwoCard)).GetGameWinner();
            }

            if (playerOneCard > playerTwoCard)
                return Player.One;

            if (playerTwoCard > playerOneCard)
                return Player.Two;

            //should not happen...
            throw new Exception("Both players have equal cards!");
        }

        public static GameState RunGame(GameState state)
        {
            HashSet<GameState> stateHistory = new HashSet<GameState>();

            while (!state.IsGameFinished)
            {
                if (stateHistory.Contains(state))
                {
                    return state;
                }
                stateHistory.Add(state);
                state = state.Step();
            }

            return state;
        }

        private bool CanRecurse(int playerOneCard, int playerTwoCard)
        {
            return _playerOneDeck.Count() - 1 >= playerOneCard && _playerTwoDeck.Count() - 1 >= playerTwoCard;
        }

        private GameState StateForRecursion(int playerOneCard, int playerTwoCard)
        {
            var newDeckOne = _playerOneDeck.Skip(1).Take(playerOneCard).ToList();
            var newDeckTwo = _playerTwoDeck.Skip(1).Take(playerTwoCard).ToList();

            return new GameState(newDeckOne, newDeckTwo);
        }

        public static GameState Parse(string input)
        {
            var inputSplit = input.Split("\r\n\r\n");
            var playerOneDeckString = inputSplit[0];
            var playerTwoDeckString = inputSplit[1];

            var playerOneCards = playerOneDeckString.Split("\r\n").Skip(1).Select(int.Parse);
            var playerTwoCards = playerTwoDeckString.Split("\r\n").Skip(1).Select(int.Parse);

            return new GameState(playerOneCards, playerTwoCards);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Players 1's deck: {string.Join(", ", _playerOneDeck)}");
            sb.AppendLine($"Players 2's deck: {string.Join(", ", _playerTwoDeck)}");

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is GameState state &&
                   _playerOneDeck.SequenceEqual(state._playerOneDeck) &&
                   _playerTwoDeck.SequenceEqual(state._playerTwoDeck);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_playerOneDeck.HashCodeFromContents(), _playerTwoDeck.HashCodeFromContents());
        }

        public static bool operator ==(GameState x, GameState y) => x.Equals(y);
        public static bool operator !=(GameState x, GameState y) => !(x == y);
    }

    enum Player
    {
        One,
        Two
    }

    static class IEnumerableExtensions
    {
        public static int HashCodeFromContents<T>(this IEnumerable<T> seq)
        {
            var result = 17;
            foreach (var item in seq)
            {
                result = (result * 23) + item.GetHashCode();
            }

            return result;
        }
    }
}
