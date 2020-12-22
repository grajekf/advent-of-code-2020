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

            Console.WriteLine(state.GetScore());
        }

    }

    class GameState
    {
        private IList<int> _playerOneDeck;
        private IList<int> _playerTwoDeck;

        public GameState(IEnumerable<int> playerOneDeck, IEnumerable<int> playerTwoDeck)
        {
            _playerOneDeck = playerOneDeck.ToList();
            _playerTwoDeck = playerTwoDeck.ToList();
        }

        public bool IsGameFinished => _playerOneDeck.Count == 0 || _playerTwoDeck.Count == 0;
        public int GetScore()
        {
            var deckToCheck = _playerOneDeck.Count == 0 ? _playerTwoDeck : _playerOneDeck;
            var score = 0;

            for (int i = deckToCheck.Count - 1; i >= 0; i--)
            {
                var multiplier = deckToCheck.Count - i;
                score += deckToCheck[i] * multiplier;
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
    }
}
