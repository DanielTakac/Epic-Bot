namespace Epic_Bot{

    public class User{

        public string Username { private get; set; }
        public ulong Id { private get; set; }
        public int Xp { private get; set; }
        public int Level { private get; set; }
        public int Balance { private get; set; }

        public User() {

            this.Username = string.Empty;
            this.Id = 0;
            this.Xp = 0;
            this.Level = 0;

        }

        public User(string username, ulong id, int xp, int level, int balance) : this() {

            this.Username = username;
            this.Id = id;
            this.Xp = xp;
            this.Level = level;
            this.Balance = balance;

        }

        public string GetUsername(){
            return Username;
        }

        public ulong GetId(){
            return Id;
        }

        public int GetXp(){
            return Xp;
        }

        public int GetLevel(){
            return Level;
        }

        public int GetBalance(){
            return Balance;
        }

    }

}
