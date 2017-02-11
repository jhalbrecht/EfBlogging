using System.Threading.Tasks;

namespace EfBlogging.Uwp.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}