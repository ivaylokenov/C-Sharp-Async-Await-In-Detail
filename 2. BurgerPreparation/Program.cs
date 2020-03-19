namespace BurgerPreparation
{
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            SyncCooker.Work();
            // await AllAtOnceCooker.Work();
            // await AsyncCooker.Work();
        }
    }
}
