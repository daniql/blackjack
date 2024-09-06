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

            var playerHand = new List<Card> { deck.Deal(), deck.Deal()};
            var dealerHand = new List<Card> { deck.Deal(), deck.Deal()};
            
            Console.WriteLine($"Dealer has dealt you a {playerHand[0]}");
            Console.WriteLine($"Dealer has dealt you a {playerHand[1]}");

            var playerHandStartingValue = GetHandValue(playerHand);

            if (playerHandStartingValue != 21)
            {
                Console.WriteLine($"Dealer has shown a {dealerHand[0]}");
                Console.WriteLine("Dealer has drawn a card and placed it face down");
                Console.WriteLine("=============================================================");
                
                var playerBusted = PlayerTurn(deck, playerHand, dealerHand[0].Value);
            
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
                Console.WriteLine(dealerHandValue > 21 ? "Dealer busted!" : $"You have {playerHandValue} and the dealer has {dealerHandValue}");
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
            Console.WriteLine($"Dealer reveals their face down card, it's a {dealerHand[1]}!");
            var handValue = GetHandValue(dealerHand);
            Console.WriteLine($"Value of dealer's hand is {handValue}");

            while (handValue < 16)
            {
                dealerHand.Add(deck.Deal());
                Console.WriteLine($"Dealer draws a {dealerHand.Last()}");
                handValue = GetHandValue(dealerHand);
                Console.WriteLine($"Value of dealer's hand is {handValue}");
            }
            Console.WriteLine(handValue > 21 ? "Dealer busts!" : "Dealer stands");
            Console.WriteLine("=============================================================");
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
                        break;
                    case "stand":
                        Console.WriteLine("=============================================================");
                        return false;
                    default:
                        Console.WriteLine("Invalid input: Please enter either 'hit' or 'stand'");
                        break;
                }
                Console.WriteLine("=============================================================");

            }
        }

        private static int GetHandValue(List<Card> hand)
        {
            var value = hand.Sum(card => card.Value == Value.Ace ? 11 : card.Value > Value.Ten ? 10 : (int)card.Value);
            var numOfAces = hand.Count(card => card.Value == Value.Ace);

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
            deck.Cards = deck.Cards.OrderBy(_ => rnd.Next()).ToList();
        }
    }
}
