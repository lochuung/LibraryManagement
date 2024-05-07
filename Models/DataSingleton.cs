namespace LibraryManagement.Models
{
    public class DataSingleton
    {
        private static DataSingleton instance;
        public static DataSingleton Instance
        {
            get
            {
                if (instance == null) instance = new DataSingleton();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public libraryEntities3 DB { get; set; }
        private DataSingleton()
        {
            DB = new libraryEntities3();
        }
    }
}