namespace DateBaseServices.Services
{
    public class DefaultService
    {
        protected readonly DataContext _db;

        protected DefaultService(DataContext db)
        {
            _db = db;
        }


    }
}