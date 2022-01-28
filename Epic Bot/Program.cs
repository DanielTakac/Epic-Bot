namespace PepejdzaBot {

    class Program {

        public static void Main(string[] args) {

            var bot = new Bot();

            bot.RunAsync().GetAwaiter().GetResult();

        }

    }

}
