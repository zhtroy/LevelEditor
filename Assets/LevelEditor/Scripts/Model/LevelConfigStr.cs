
//这个文件中记录通用的json字段名
namespace CommonLevelEditor
{
    public class ConfigLayerId
    {
        public const string FIELDS = "fields";
        public const string ITEMS = "items";
        public const string COVERS = "covers";
        public const string TUTORIAL = "tutorial";
        public const string COLORS = "colors";
        public const string HITPOINTS = "hitpoints";
        public const string TYPES = "types";
        public const string OPTIONS = "options";
    }

    public class ConfigFieldType
    {
        public const string NORMAL = "O";
        public const string EMPTY = "-";
        public const string BACKGROUND = "X";
        public const string SPAWNER = "A";
        public const string CREATOR = "C";
        public const string SPAWN_CREATOR = "CC";
        public const string EXIT = "E";
        public const string ENTER = "N";
        public const string MAGNET_BACKGROUND = "m";
        public const string SWITCHER = "H";
        public const string PIPE = "P";
        public const string TREASURE_ENTER = "Q";
    }
}
