namespace Epic_Bot{

    public class User{

        public string Username { private get; set; }
        public ulong Id { private get; set; }
        public int Xp { private get; set; }
        public int Level { private get; set; }

        public User(){

            Username = string.Empty;
            Id = 0;
            Xp = 0;
            Level = 0;

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

    }

}
