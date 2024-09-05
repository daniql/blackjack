namespace BlackjackApp;

public class Card(Suit suit, Value value)
{
    private Suit Suit { get; } = suit;
    public Value Value { get; } = value;
    
    public override string ToString()
    {
        return $"{Value} of {Suit}";
    }
}
    
public enum Value
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}
        
public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}