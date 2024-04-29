using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapEF : IMapDataAccess, IRepository
    {
        private readonly RobotContext _context;

        public MapEF(RobotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Map> GetMaps()
        {
            return _context.Maps.ToList();
        }

        public void InsertMap(Map newMap)
        {
            newMap.Id = default;
            newMap.CreatedDate = DateTime.Now;
            newMap.ModifiedDate = DateTime.Now;
            _context.Maps.Add(newMap);
            _context.SaveChanges();
        }

        public void UpdateMap(Map updatedMap)
        {
            if (updatedMap == null)
            {
                throw new ArgumentException("Map not found");
            }

            updatedMap.ModifiedDate = DateTime.Now;
            _context.Maps.Update(updatedMap);
            _context.SaveChanges();
        }

        public void DeleteMap(int id)
        {
            var mapToDelete = _context.Maps.Find(id);
            if (mapToDelete != null)
            {
                _context.Maps.Remove(mapToDelete);
                _context.SaveChanges();
            }
        }
    }
}
