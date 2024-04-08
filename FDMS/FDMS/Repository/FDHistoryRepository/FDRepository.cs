using AutoMapper;
using FDMS.Entity;
using FDMS.Model;
using Microsoft.EntityFrameworkCore;

namespace FDMS.Repository.FDHistoryRepository
{
    public class FDRepository : IFDRepository
    {
        private readonly FDMSContext _context;
        private readonly IMapper _mapper;
        public FDRepository(FDMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateNew(string content)
        {
            var newDocumentHistory = new FDHistory
            {
                Content = content,
                Date = DateTime.Now,
            };
            await _context.AddAsync(newDocumentHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<APIResponse> GetAll(string searchString, string date)
        {
            var allSN = _context.FDHistorys.AsQueryable();
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
                message = "Document history.",
                msg = "Document history.",
                data = _mapper.Map<List<FDHistoryViewModel>>(result)
            };
        }
    }
}
