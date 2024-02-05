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
                        new Account {ClientId = accountOrnella.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();

                }
            }

        }

    }
}
