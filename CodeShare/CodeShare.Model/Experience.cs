namespace CodeShare.Model
{
    public static class Experience
    {
        public enum Action
        {
            AddGame = 1000,
            AddWiki = 200,
            AddQuest = 200,
            AddComment = 20,
            AddProfilePhoto = 100,
            EditGame = 10,
            EditWiki = 10,
            EditQuest = 10,
            UploadImage = 10,
            UploadVideo = 10
        };

        public static readonly int LevelUpExp = 5000;

        public static int ExpToLevel(int exp)
        {
            return exp / LevelUpExp;
        }

        public static int LevelToExp(int level)
        {
            return level * LevelUpExp;
        }

        public static int ProgressExp(int exp)
        {
            return exp % LevelUpExp;
        }

        public static double ProgressExpInPercentage(int exp)
        {
            return (exp % LevelUpExp / (double)LevelUpExp) * 100.00;
        }
    }
}
