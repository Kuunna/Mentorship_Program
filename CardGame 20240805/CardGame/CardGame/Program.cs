using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{

    public enum Suit {Hearts, Diamonds, Clubs, Spades /*Bích*/}
    public enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King}

    public class Card { 
        private Suit Suit { get; set; }
        private Rank Rank { get; set; }
        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card otherCard)
            {
                return Rank == otherCard.Rank && Suit == otherCard.Suit;
            }
            return false;
        }

    }

        internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
