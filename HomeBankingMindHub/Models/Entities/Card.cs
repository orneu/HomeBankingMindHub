using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.ENUM;
using System.Drawing;

namespace HomeBankingMindHub.Models.Entities
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public CardType Type { get; set; }
        public CardColor Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public long ClientId { get; set; }


        /*
                public Card(long Id, string name, string lastname, CardType type, CardColor cardColor, string number, int cvv, DateTime dateTime)
                { ClientId = Id;
                    CardHolder = name + " " + lastname;
                    Type = type;
                    Color = cardColor;
                    Number = number; 
                    Cvv = cvv;
                    FromDate = dateTime;
                    ThruDate = dateTime.AddYears(4);
                }
        */

    }
}
