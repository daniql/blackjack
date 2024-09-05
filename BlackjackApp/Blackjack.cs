namespace BlackjackApp
{
    internal static class Blackjack
    {
        private static void Main()
        {
            Console.WriteLine("=============================================================");
            Console.WriteLine("                  Welcome to Blackjack!");
            Console.WriteLine("                   Dealer draws to 16");
            Console.WriteLine("=============================================================");
            var deck = CreateDeck();
            Shuffle(deck);

            List<Card> playerHand = [];
            List<Card> dealerHand = [];

            playerHand.Add(deck.Deal());
            Console.WriteLine($"Dealer has dealt you a {playerHand.Last()}");
            playerHand.Add(deck.Deal());
            Console.WriteLine($"Dealer has dealt you a {playerHand.Last()}");
            dealerHand.Add(deck.Deal());

            var playerHandStartingValue = GetHandValue(playerHand);

            if (playerHandStartingValue != 21)
            {
                Console.WriteLine($"Dealer has shown a {dealerHand.Last()}");
                var dealerShownValue = dealerHand.Last().Value;
                dealerHand.Add(deck.Deal());
                Console.WriteLine("Dealer has drawn a card and placed it face down");
                Console.WriteLine("=============================================================");
                
                var playerBusted = PlayerTurn(deck, playerHand, dealerShownValue);
            
                if (!playerBusted)
                {
                    DealerTurn(deck, dealerHand);
                    DetermineWinner(playerHand, dealerHand);
                }
            }
            else
            {
                Console.WriteLine("=============================================================");
                Console.WriteLine("Blackjack! You win!");
            }

            Console.WriteLine("Thanks for playing!");
        }

        private static void DetermineWinner(List<Card> playerHand, List<Card> dealerHand)
        {
            var playerHandValue = GetHandValue(playerHand);
            var dealerHandValue = GetHandValue(dealerHand);

            if (dealerHandValue > 21 || playerHandValue > dealerHandValue)
            {
                if (dealerHandValue > 21)
                {
                    Console.WriteLine("Dealer busts!");
                }
                else
                {
                    Console.WriteLine($"You have {playerHandValue} and the dealer has {dealerHandValue}");
                }

                Console.WriteLine("You win!");
            }
            else if (dealerHandValue > playerHandValue)
            {
                Console.WriteLine($"You have {playerHandValue} and the dealer has {dealerHandValue}");
                Console.WriteLine("You lose!");
            }
            else
            {
                Console.WriteLine($"You have {playerHandValue} and the dealer has {dealerHandValue}");
                Console.WriteLine("Push!");
            }
        }

        private static void DealerTurn(Deck deck, List<Card> dealerHand)
        {
            Console.WriteLine($"Dealer reveals their face down card, it's a {dealerHand.Last()}!");
            var handValue = GetHandValue(dealerHand);
            Console.WriteLine($"Value of dealer's hand is {handValue}");
            while (true)
            {
                switch(handValue)
                {
                    case > 21:
                        Console.WriteLine("=============================================================");
                        return;
                    case >= 16:
                        Console.WriteLine("Dealer's hand is at or above 16 and the dealer stands");
                        Console.WriteLine("==================================================");
                        return;
                    default:
                        Console.WriteLine("=============================================================");
                        dealerHand.Add(deck.Deal());
                        Console.WriteLine($"Dealer draws a {dealerHand.Last()}");
                        handValue = GetHandValue(dealerHand);
                        Console.WriteLine($"Value of dealer's hand is {handValue}");
                        break;  
                }
            }
        }

        private static bool PlayerTurn(Deck deck, List<Card> playerHand, Value dealerShownValue)
        {
            while (true)
            {
                var handValue = GetHandValue(playerHand);
                Console.WriteLine($"Dealer is showing a {dealerShownValue}");
                Console.WriteLine($"The current value of your hand is {handValue}");
                if (handValue > 21)
                {
                    Console.WriteLine("You busted!");
                    return true;
                }
                Console.WriteLine("Do you want to 'hit' or 'stand'?");
                var input = Console.ReadLine()?.ToLower();
                switch (input)
                {
                    case "hit":
                        playerHand.Add(deck.Deal());
                        Console.WriteLine($"The dealer deals you a {playerHand.Last()}");
                        Console.WriteLine("=============================================================");
                        break;
                    case "stand":
                        Console.WriteLine("=============================================================");
                        return false;
                    default:
                        Console.WriteLine("Invalid input: Please enter either 'hit' or 'stand'");
                        Console.WriteLine("=============================================================");
                        break;
                }
            }
        }

        private static int GetHandValue(List<Card> hand)
        {
            var value = 0;
            var numOfAces = 0;

            foreach (var card in hand)
            {
                if (card.Value == Value.Ace)
                {
                    value += 11;
                    numOfAces++;
                }
                else if (card.Value > Value.Ten)
                {
                    value += 10;
                }
                else
                {
                    value += (int)card.Value;
                }
            }

            while (value > 21 && numOfAces > 0)
            {
                value -= 10;
                numOfAces--;
            }

            return value;
        }

        private static Deck CreateDeck()
        {
            var deck = new Deck();
            foreach (var suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (var value in Enum.GetValues(typeof(Value)))
                {
                    deck.Cards.Add(new Card((Suit) suit, (Value) value));
                }
            }
            return deck;
        }
        
        private static void Shuffle(Deck deck)
        {
            var rnd = new Random();
            var n = deck.Cards.Count();
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n+1);
                (deck.Cards[k], deck.Cards[n]) = (deck.Cards[n], deck.Cards[k]);
            }
        }
    }
}



