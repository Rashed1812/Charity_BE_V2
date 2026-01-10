using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.GenericRepositries;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IMediationRepository : IGenericRepository<Mediation>
    {
        Task<Mediation> GetMediationByUserIdAsync(string userId);
        Task<List<Mediation>> GetAllMediationsWithRelatedDataAsync();
        Task<Mediation> GetMediationByIdWithRelatedDataAsync(int id);
        Task<List<Mediation>> GetAllMediationsWithStatsAsync(int? year = null);
        Task<Mediation> GetMediationByIdWithStatsAsync(int id, int? year = null);
        Task<int> GetCountByMediationAndYearAsync(int mediationId, int year);
    }
} 