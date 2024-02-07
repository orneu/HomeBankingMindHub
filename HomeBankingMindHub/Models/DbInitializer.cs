namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                   new Client { Email = "ornellajazminpacino@gmail.com", FirstName = "Ornella", LastName = "Pacino", Password = "123456" }
                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();
            }

            if (!context.Account.Any())
            {
                var accountOrnella = context.Clients.FirstOrDefault(c => c.Email == "ornellajazminpacino@gmail.com");
                if (accountOrnella != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountOrnella.Id, CreationDate = DateTime.Now, Number = "VIN001" , Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();

                }
            }

            if (!context.Transactions.Any())
            {
                var account1 = context.Account.FirstOrDefault(c => c.Number == "VIN001");
                if (account1 != null)

                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }

        }

    }
}
