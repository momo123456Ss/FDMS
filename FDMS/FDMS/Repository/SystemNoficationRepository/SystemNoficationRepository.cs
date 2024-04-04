using AutoMapper;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;

namespace FDMS.Repository.SystemNoficationRepository
{
    public class SystemNoficationRepository : ISystemNoficationRepository
    {
        private readonly FDMSContext _context;
        private readonly IMapper _mapper;
        public SystemNoficationRepository(FDMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateNew(string content)
        {
            var newSystemNofication = new SystemNofication
            {
                Content = content,
                Date = DateTime.Now,
            };
            await _context.AddAsync(newSystemNofication);
            await _context.SaveChangesAsync();
        }

        public async Task<APIResponse> GetAll(string searchString, string date)
        {
            var allSN = _context.SystemNofications.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allSN = allSN.Where(a =>
                    a.Content.ToLower().Contains(searchString)
                );
            }
            if (!string.IsNullOrEmpty(date))
            {
                date = date.ToLower();
                allSN = allSN.Where(a =>
                    a.Date.ToString().ToLower().Contains(date)
                );
            }
            allSN = allSN.OrderByDescending(sn => sn.Date);
            var result = await allSN.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "System nofications.",
                msg = "System nofications.",
                data = _mapper.Map<List<SystemNoficationViewModel>>(result)
            };
        }
    }
}
