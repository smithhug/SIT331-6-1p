using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        void DeleteMap(global::System.Int32 id);
        List<Map> GetMaps();
        void InsertMap(Map map);
        void UpdateMap(Map map);
    }
}