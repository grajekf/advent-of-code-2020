using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _22a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var state = GameState.Parse(inputString);

            while (!state.IsGameFinished)
            {
                state = state.Step();
            }

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

            if (playerOneCard > playerTwoCard)
            {
                deckOneCopy.Add(playerOneCard);
                deckOneCopy.Add(playerTwoCard);
            }
            else
            {
                deckTwoCopy.Add(playerTwoCard);
                deckTwoCopy.Add(playerOneCard);
            }

            return new GameState(deckOneCopy, deckTwoCopy);
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
