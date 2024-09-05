namespace BlackjackApp;

public class Deck
{
    public List<Card> Cards {get; set;} = [];
    
    public Card Deal()
    {
        var card = Cards[0];
        Cards.RemoveAt(0);
        return card;
    }
}