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
        public void Card_Constructor_Should_Set_Values_Correctly()
        {
            var rank = Rank.Ace;
            var suit = Suit.Hearts;
            var card = new Card(rank, suit);

            Assert.AreEqual(rank, card.Rank);
            Assert.AreEqual(suit, card.Suit);
        }

        [TestMethod]
        public void Card_ToString_Should_Return_Expected_String()
        {
            var card = new Card(Rank.King, Suit.Spades);
            var result = card.ToString();

            Assert.AreEqual("King of Spades", result);
        }
    }

    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void Deck_Constructor_Should_Create_52_Cards()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.Cards.Count);
        }

        [TestMethod]
        public void Deck_Shuffle_Should_Change_Card_Order()
        {
            var deck = new Deck();
            var originalOrder = new List<Card>(deck.Cards.Select(card => new Card(card.Rank, card.Suit)));

            deck.Shuffle();
            var shuffledOrder = deck.Cards;

            bool orderChanged = false;
            for (int i = 0; i < originalOrder.Count; i++)
                if (originalOrder[i].Rank != shuffledOrder[i].Rank || originalOrder[i].Suit != shuffledOrder[i].Suit)
                {
                    orderChanged = true;
                    break;
                }

            Assert.IsTrue(orderChanged, "Deck order should be shuffled.");
        }

        [TestMethod]
        public void Deck_Draw_Should_Return_Correct_Number_Of_Cards()
        {
            var deck = new Deck();
            var drawnCards = deck.Draw(5);

            Assert.AreEqual(5, drawnCards.Count);
            Assert.AreEqual(47, deck.Cards.Count); // 52 - 5 = 47 cards remaining
        }

        [TestMethod]
        public void Deck_Reset_Should_Reshuffle_Deck_To_52_Cards()
        {
            var deck = new Deck();
            deck.Draw(10); // Draw some cards
            deck.Reset();

            Assert.AreEqual(52, deck.Cards.Count);
        }
    }

    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Player_Constructor_Should_Set_ID_And_Name_Correctly()
        {
            int id = 1;
            string name = "Alice";

            var player = new Player(id, name);

            Assert.AreEqual(id, player.ID);
            Assert.AreEqual(name, player.Name);
        }

        [TestMethod]
        public void Player_ReceiveCards_Should_Add_Cards_To_Hand()
        {
            var player = new Player(1, "Alice");
            var cards = new List<Card>
            {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Ten, Suit.Clubs)
            };

            player.ReceiveCards(cards);
            var hand = player.GetHand();

            Assert.AreEqual(2, hand.Count);
            CollectionAssert.Contains(hand, cards[0]);
            CollectionAssert.Contains(hand, cards[1]);
        }

        [TestMethod]
        public void Player_PlayCard_Should_Remove_Card_From_Hand()
        {
            var player = new Player(1, "Alice");
            var card = new Card(Rank.Ace, Suit.Hearts);
            player.ReceiveCards(new List<Card> { card });

            var playedCard = player.PlayCard(card);
            var hand = player.GetHand();

            Assert.AreEqual(card, playedCard);
            Assert.IsFalse(hand.Contains(card));
        }

        [TestMethod]
        public void Player_ShowHand_Should_Print_Cards_To_Console()
        {
            var player = new Player(1, "Alice");
            var card = new Card(Rank.Ace, Suit.Hearts);
            player.ReceiveCards(new List<Card> { card });

            // Redirect Console output to a StringWriter to test the output
            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);

            player.ShowHand();
            var output = consoleOutput.ToString();

            Assert.IsTrue(output.Contains("Alice's Hand:"));
            Assert.IsTrue(output.Contains(card.ToString()));

            // Clean up
            Console.SetOut(Console.Out);
            consoleOutput.Dispose();
        }

        [TestMethod]
        public void Player_ClearHand_Should_Empty_The_Hand()
        {
            var player = new Player(1, "Alice");
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
    }
}
