namespace Assets.Scripts.GameManager
{
    public static class Properties
    {
        public const string MANA = "Mana";
        public const string HP = "HP";
        public const string MAXHP = "MAXHP";
        public const string XP = "XP";
        public const string TIME = "Time";
        public const string MONEY = "Money";
        public const string LEVEL = "Level";
        public const string POSITION = "Position";

        public const string HEALTHPOTION1 = "HealthPotion1";
        public const string HEALTHPOTION2 = "HealthPotion2";
        public const string MANAPOTION1 = "ManaPotion1";
        public const string MANAPOTION2 = "ManaPotion2";

        public const string CHEST1 = "Chest1";
        public const string CHEST2 = "Chest2";
        public const string CHEST3 = "Chest3";
        public const string CHEST4 = "Chest4";
        public const string CHEST5 = "Chest5";

        public const string SKELETON1 = "Skeleton1";
        public const string SKELETON2 = "Skeleton2";

        public const string ORC1 = "Orc1";
        public const string ORC2 = "Orc2";

        public const string DRAGON = "Dragon";

        public static int GetPropertyIndex(string propertyName)
        {
            int index = -1;
            switch (propertyName)
            {
                case Properties.MANA:
                    index = 0;
                    break;
                case Properties.HP:
                    index = 1;
                    break;
                case Properties.MAXHP:
                    index = 2;
                    break;
                case Properties.XP:
                    index = 3;
                    break;
                case Properties.TIME:
                    index = 4;
                    break;
                case Properties.MONEY:
                    index = 5;
                    break;
                case Properties.LEVEL:
                    index = 6;
                    break;
                case Properties.POSITION:
                    index = 7;
                    break;
                case Properties.HEALTHPOTION1:
                    index = 10;
                    break;
                case Properties.HEALTHPOTION2:
                    index = 11;
                    break;
                case Properties.MANAPOTION1:
                    index = 12;
                    break;
                case Properties.MANAPOTION2:
                    index = 13;
                    break;
                case Properties.CHEST1:
                    index = 15;
                    break;
                case Properties.CHEST2:
                    index = 16;
                    break;
                case Properties.CHEST3:
                    index = 17;
                    break;
                case Properties.CHEST4:
                    index = 18;
                    break;
                case Properties.CHEST5:
                    index = 19;
                    break;
                case Properties.SKELETON1:
                    index = 20;
                    break;
                case Properties.SKELETON2:
                    index = 21;
                    break;
                case Properties.ORC1:
                    index = 22;
                    break;
                case Properties.ORC2:
                    index = 23;
                    break;
                case Properties.DRAGON:
                    index = 24;
                    break;
            }

            return index;
        }
    }
}
