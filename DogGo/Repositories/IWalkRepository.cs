using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalks();
        Walk GetWalksById(int id);
     List <Walk> GetWalksByWalkerId(int WalkerId);
        void AddWalks(Walk walks);
        void UpdateWalks(Walk walks);
        void DeleteWalks(int walkId);
    }
}
