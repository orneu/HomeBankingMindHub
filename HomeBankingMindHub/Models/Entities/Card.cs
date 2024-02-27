using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.ENUM;

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

        public string RandomCardNumber()
        { Random rand = new Random();
        string randomCardNumber = "";
            for (int i = 0; i< 4; i++)
            {
                randomCardNumber += (1000 + rand.Next(8999));
                if (i == 3) { break; }

                randomCardNumber += "-";
            } return randomCardNumber;
        }


    public Card(Client client, CardColor cardColorAux, CardFormDTO cardFormDTO) 
            {
            Random rand = new Random();
            ClientId = client.Id;
            CardHolder = client.FirstName + " " + client.LastName;
            Type = cardFormDTO.Type.ToUpper().Equals(CardType.DEBIT.ToString()) ? CardType.DEBIT : CardType.CREDIT;
            Color = cardColorAux;
            Number = RandomCardNumber();
            Cvv = 100 + rand.Next(899);
            FromDate = DateTime.Now;
            ThruDate = DateTime.Now.AddYears(4);
            }

    }
}
