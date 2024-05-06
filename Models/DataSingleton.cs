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
        public LibraryEntities DB { get; set; }
        private DataSingleton()
        {
            DB = new LibraryEntities();
        }
    }
}