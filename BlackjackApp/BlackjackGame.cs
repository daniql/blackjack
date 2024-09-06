namespace BlackjackApp
{
    public class BlackjackGame
    {
        private Deck _deck;
        private List<Card> _playerHand;
        private List<Card> _dealerHand;

        public void Start()
        {
            Console.WriteLine("=============================================================");
            Console.WriteLine("                  Welcome to Blackjack!");
            Console.WriteLine("                   Dealer draws to 16");
            Console.WriteLine("=============================================================");

            _deck = CreateDeck();
            Shuffle(_deck);

            _playerHand = new List<Card> { _deck.Deal(), _deck.Deal() };
            _dealerHand = new List<Card> { _deck.Deal(), _deck.Deal() };

            Console.WriteLine($"Dealer has dealt you a {_playerHand[0]}");
            Console.WriteLine($"Dealer has dealt you a {_playerHand[1]}");

            var playerHandStartingValue = GetHandValue(_playerHand);

            if (playerHandStartingValue != 21)
            {
                Console.WriteLine($"Dealer has shown a {_dealerHand[0]}");
                Console.WriteLine("Dealer has drawn a card and placed it face down");
                Console.WriteLine("=============================================================");

                var playerBusted = PlayerTurn();

                if (!playerBusted)
                {
                    DealerTurn();
                    DetermineWinner();
                }
            }
            else
            {
                Console.WriteLine("=============================================================");
                Console.WriteLine("Blackjack! You win!");
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void DetermineWinner()
        {
            var playerHandValue = GetHandValue(_playerHand);
            var dealerHandValue = GetHandValue(_dealerHand);

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

        private void DealerTurn()
        {
            Console.WriteLine($"Dealer reveals their face down card, it's a {_dealerHand[1]}!");
            var handValue = GetHandValue(_dealerHand);
            Console.WriteLine($"Value of dealer's hand is {handValue}");
            if (handValue < 16) Console.WriteLine("=============================================================");

            while (handValue < 16)
            {
                _dealerHand.Add(_deck.Deal());
                Console.WriteLine($"Dealer draws a {_dealerHand.Last()}");
                handValue = GetHandValue(_dealerHand);
                Console.WriteLine($"Value of dealer's hand is {handValue}");
                if (handValue < 16) Console.WriteLine("=============================================================");
            }
            if (handValue < 21) Console.WriteLine("Dealer stands");
            Console.WriteLine("=============================================================");
        }

        private bool PlayerTurn()
        {
            while (true)
            {
                var handValue = GetHandValue(_playerHand);

                Console.WriteLine($"Dealer is showing a {_dealerHand[0].Value}");
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
                        _playerHand.Add(_deck.Deal());
                        Console.WriteLine($"The dealer deals you a {_playerHand.Last()}");
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
                    deck.Cards.Add(new Card((Suit)suit, (Value)value));
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