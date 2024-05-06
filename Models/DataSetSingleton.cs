namespace LibraryManagement.Models
{
    public class DataSetSingleton
    {
        private static DataSetSingleton instance;
        public static DataSetSingleton Instance
        {
            get
            {
                if (instance == null) instance = new DataSetSingleton();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public LibraryEntities DB { get; set; }
        private DataSetSingleton()
        {
            DB = new LibraryEntities();
        }
    }
}