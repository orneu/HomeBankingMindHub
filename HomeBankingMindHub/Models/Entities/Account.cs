using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Repositories.Implemetation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeBankingMindHub.Models.Entities
{
    public class Account
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public Client Client { get; set; }
        public long ClientId { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

        public int[] RandomAccountNumber()
        {
            Random rand = new Random();
            int[] randomAccountNumber = new int[8];
            for (int i = 0; i < 8; i++)
            {
                randomAccountNumber[i] = rand.Next(10);
            }
            return randomAccountNumber;
        }

        public Account(Client client)
        {
            Number = "VIN" + string.Join("", RandomAccountNumber());
            CreationDate = DateTime.Now;
            ClientId = client.Id;
            Balance = 0;
        }
        public Account(long clientId, DateTime creationDate, string number, double balance)
        {
            Number = number;
            CreationDate = creationDate;
            ClientId = clientId;
            Balance = balance;
           
        }
}
}
