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

        }

    }
}
