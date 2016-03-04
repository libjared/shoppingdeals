namespace ShoppingDeals.Controllers
{
    public static class StaticDealsDb
    {
        public static DealsDb Db { get; private set; }

        public static void Initialize(string databaseName)
        {
            if (Db == null)
            {
                Db = new DealsDb(databaseName);
            }
        }
    }
}