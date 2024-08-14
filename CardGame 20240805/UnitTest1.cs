using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CardGame;
using System.Collections.Generic;
using System.Linq;

namespace TestCardGame
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Card_Constructor_Set_Rank_And_Suit_Correctly()
        {
            var rank = Rank.Ace;
            var suit = Suit.Hearts;
            var card = new Card(rank, suit);

            Assert.AreEqual(rank, card.Rank);
            Assert.AreEqual(suit, card.Suit);
        }

        [TestMethod]
        public void Card_ToString_Returns_Correct_String()
        {
            var card = new Card(Rank.King, Suit.Spades);
            var result = card.ToString();

            Assert.AreEqual("King of Spades", result);
        }

        [TestMethod]
        public void Card_Equals_Returns_False_For_Different_Rank_Or_Suit()
        {
            var card1 = new Card(Rank.Ace, Suit.Hearts);
            var card2 = new Card(Rank.Ace, Suit.Diamonds);
            var card3 = new Card(Rank.King, Suit.Hearts);

            Assert.IsFalse(card1.Equals(card2));
            Assert.IsFalse(card1.Equals(card3));
        }
    }

    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void Deck_Constructor_Creates_52_Cards()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.Cards.Count);
        }

        [TestMethod]
        public void Deck_Shuffle_Changes_Card_Order()
        {
            var deck = new Deck();
            var originalOrder = new List<Card>(deck.Cards.Select(card => new Card(card.Rank, card.Suit)));

            deck.Shuffle();
            var shuffledOrder = deck.Cards;

            bool orderChanged = originalOrder
                .Select((card, index) => new { Card = card, Index = index })
                .Any(item => item.Card.Rank != shuffledOrder[item.Index].Rank || item.Card.Suit != shuffledOrder[item.Index].Suit);

            Assert.IsTrue(orderChanged);
        }

        [TestMethod]
        public void Deck_Draw_Returns_Correct_Number_Of_Cards()
        {
            var deck = new Deck();
            var drawnCards = deck.Draw(5);

            Assert.AreEqual(5, drawnCards.Count);
            Assert.AreEqual(47, deck.Cards.Count);
        }

        [TestMethod]
        public void Deck_Reset_Reshuffles_Deck_To_52_Cards()
        {
            var deck = new Deck();
            deck.Draw(10); // Draw some cards
            deck.Reset();

            Assert.AreEqual(52, deck.Cards.Count);
        }
        [TestMethod]
        public void Deck_Draw_Throws_Exception_When_Drawing_More_Than_Available()
        {
            var deck = new Deck();

            Assert.ThrowsException<ArgumentException>(() => deck.Draw(53));
        }

        [TestMethod]
        public void Deck_Draw_Allows_Drawing_Exact_Number_Of_Remaining_Cards()
        {
            var deck = new Deck();
            var drawnCards = deck.Draw(52); // Draw all cards

            Assert.AreEqual(52, drawnCards.Count, "Should be able to draw exactly 52 cards.");
            Assert.AreEqual(0, deck.Cards.Count, "Deck should have 0 cards remaining after drawing all.");
        }

        [TestMethod]
        public void Deck_Shuffle_Leaves_Deck_With_Same_Number_Of_Cards()
        {
            var deck = new Deck();
            deck.Shuffle();

            Assert.AreEqual(52, deck.Cards.Count, "Deck should have 52 cards after shuffling.");
        }

        [TestMethod]
        public void Deck_Reset_Resets_Deck_After_Full_Draw()
        {
            var deck = new Deck();
            deck.Draw(52);
            deck.Reset();

            Assert.AreEqual(52, deck.Cards.Count, "Deck should be reset to 52 cards after a full draw and reset.");
        }

        [TestMethod]
        public void Deck_Reset_Resets_Deck_To_Original_State()
        {
            var deck = new Deck();
            var originalDeck = new List<Card>(deck.Cards);

            deck.Shuffle();
            deck.Reset();

            bool isOriginalOrder = originalDeck.SequenceEqual(deck.Cards);

            Assert.IsTrue(isOriginalOrder, "Deck should return to original order after reset.");
        }
    }

    [TestClass]
    public class PlayerTests
    {
        private Player player;

        [TestInitialize]
        public void Setup()
        {
            player = new Player(1, "Alice");
        }

        [TestMethod]
        public void Player_Constructor_Sets_ID_And_Name_Correctly()
        {
            Assert.AreEqual(1, player.ID);
            Assert.AreEqual("Alice", player.Name);
        }

        [TestMethod]
        public void Player_ReceiveCards_Adds_Cards_To_Hand()
        {
            var cards = new List<Card>
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ten, Suit.Clubs)
        };

            player.ReceiveCards(cards);
            var hand = player.GetHand();

            Assert.AreEqual(2, hand.Count);
            CollectionAssert.AreEqual(cards, hand);
        }

        [TestMethod]
        public void Player_PlayCard_Removes_Card_From_Hand()
        {
            var card = new Card(Rank.Ace, Suit.Hearts);
            player.ReceiveCards(new List<Card> { card });

            var playedCard = player.PlayCard(card);
            var hand = player.GetHand();

            Assert.AreEqual(card, playedCard);
            Assert.IsFalse(hand.Contains(card));
        }

        [TestMethod]
        public void Player_ShowHand_Prints_Cards_To_Console()
        {
            var cards = new List<Card>
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.King, Suit.Diamonds)
        };
            player.ReceiveCards(cards);

            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);

            player.ShowHand();
            var output = consoleOutput.ToString();

            Assert.IsTrue(output.Contains("Alice's Hand:"));
            Assert.IsTrue(output.Contains("Ace of Hearts"));
            Assert.IsTrue(output.Contains("King of Diamonds"));

            Console.SetOut(Console.Out);
            consoleOutput.Dispose();
        }

        [TestMethod]
        public void Player_ClearHand_Removes_All_Cards_From_Hand()
        {
            var cards = new List<Card>
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ten, Suit.Clubs)
        };
            player.ReceiveCards(cards);

            player.ClearHand();
            var hand = player.GetHand();

            Assert.AreEqual(0, hand.Count);
        }

        [TestMethod]
        public void Player_ReceiveCards_Allows_Multiple_Adds()
        {
            var cards1 = new List<Card>
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ten, Suit.Clubs)
        };
            var cards2 = new List<Card>
        {
            new Card(Rank.King, Suit.Diamonds),
            new Card(Rank.Queen, Suit.Spades)
        };

            player.ReceiveCards(cards1);
            player.ReceiveCards(cards2);
            var hand = player.GetHand();

            Assert.AreEqual(4, hand.Count);
            CollectionAssert.Contains(hand, cards1[0]);
            CollectionAssert.Contains(hand, cards1[1]);
            CollectionAssert.Contains(hand, cards2[0]);
            CollectionAssert.Contains(hand, cards2[1]);
        }

        [TestMethod]
        public void Player_PlayCard_Throws_Exception_When_Card_Not_In_Hand()
        {
            var cardNotInHand = new Card(Rank.King, Suit.Diamonds);

            Assert.ThrowsException<InvalidOperationException>(() => player.PlayCard(cardNotInHand));
        }
    }

    [TestClass]
    public class ScoringTests
    {
        [TestMethod]
        public void CalculateScore_Baccarat_Valid_Correctly()
        {
            var hand = new List<Card> {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Four, Suit.Spades),
                new Card(Rank.Nine, Suit.Diamonds)
            };
            int score = Scoring.CalculateScore(hand, "Baccarat");
            Assert.AreEqual(4, score); // Aces are worth 0, Four is worth 4
        }
    }

    [TestClass]
    public class BaccaratTests
    {
        [TestMethod]
        public void InitializeGame_Throws_Exception_When_Player_Count_Is_Invalid()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();

            // Test with fewer than 2 players
            Assert.ThrowsException<ArgumentException>(() => baccarat.InitializeGame(new List<Player>(), deck));

            // Test with more than 17 players
            var players = Enumerable.Range(1, 18).Select(i => new Player(i, $"Player{i}")).ToList();
            Assert.ThrowsException<ArgumentException>(() => baccarat.InitializeGame(players, deck));
        }

        [TestMethod]
        public void InitializeGame_Removes_10_J_Q_K_From_Deck()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();

            // Test with 2 players
            var players = new List<Player> { new Player(1, "Player1"), new Player(2, "Player2") };
            baccarat.InitializeGame(players, deck);

            Assert.IsFalse(deck.Cards.Any(card => card.Rank == Rank.Ten || card.Rank == Rank.Jack || card.Rank == Rank.Queen || card.Rank == Rank.King));
        }

        [TestMethod]
        public void InitializeGame_Deals_Correct_Number_Of_Cards()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();
            var players = new List<Player> { new Player(1, "Player1"), new Player(2, "Player2") };

            baccarat.InitializeGame(players, deck);

            foreach (var player in players)
            {
                Assert.AreEqual(3, player.GetHand().Count);
            }
        }

        [TestMethod]
        public void IsGameOver_Returns_True_After_Initialization()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();
            var players = new List<Player> { new Player(1, "Player1"), new Player(2, "Player2") };

            baccarat.InitializeGame(players, deck);

            Assert.IsTrue(baccarat.IsGameOver(players));
        }

        [TestMethod]
        public void DetermineWinner_Calculates_Score_Correctly()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();
            var player1 = new Player(1, "Player1");
            var player2 = new Player(2, "Player2");

            // Khởi tạo tay bài cho từng người chơi
            player1.ReceiveCards(new List<Card>
            {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Three, Suit.Diamonds) // Tổng điểm = 1 + 2 + 3 = 6
            });

            player2.ReceiveCards(new List<Card>
            {
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Five, Suit.Clubs),
                new Card(Rank.Six, Suit.Spades)  // Tổng điểm = 4 + 5 + 6 = 15 = 5
            });

            var players = new List<Player> { player1, player2 };
            baccarat.InitializeGame(players, deck);
            var winner = baccarat.DetermineWinner(players);

            Assert.AreEqual("Player1", winner.Name); // Player 1 có điểm cao hơn (6 > 5)
        }

        [TestMethod]
        public void DetermineWinner_Handles_Tie_Breaker_Based_On_Suit()
        {
            var deck = new Deck();
            var baccarat = new Baccarat();
            var player1 = new Player(1, "Player1");
            var player2 = new Player(2, "Player2");

            player1.ReceiveCards(new List<Card>
            {
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Three, Suit.Diamonds)
            });
            player2.ReceiveCards(new List<Card>
            {
                new Card(Rank.Ace, Suit.Clubs),
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Three, Suit.Hearts)
            });

            baccarat.InitializeGame(new List<Player> { player1, player2 }, deck);

            var winner = baccarat.DetermineWinner(new List<Player> { player1, player2 });

            Assert.AreEqual(player1, winner); // Player 1 has the highest suit (Hearts > Spades)
        }

        [TestMethod]
        public void CalculateScore_Correctly_Computes_Score_For_Hand()
        {
            var baccarat = new Baccarat();
            var hand = new List<Card>
            {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Three, Suit.Diamonds)
            };

            int score = Scoring.CalculateScore(hand, "Baccarat");

            Assert.AreEqual(6, score); // (1 + 2 + 3) = 6
        }

        [TestMethod]
        public void DetermineTieBreaker_Selects_Winner_Based_On_Highest_Suit()
        {
            var baccarat = new Baccarat();
            var player1 = new Player(1, "Player1");
            var player2 = new Player(2, "Player2");

            player1.ReceiveCards(new List<Card>
            {
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Three, Suit.Diamonds)
            });
            player2.ReceiveCards(new List<Card>
            {
                new Card(Rank.Ace, Suit.Clubs),
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Three, Suit.Hearts)
            });

            var winner = baccarat.DetermineTieBreaker(new List<Player> { player1, player2 });

            Assert.AreEqual(player1, winner); // Player 1 has the highest suit (Hearts > Spades)
        }
    }


}
