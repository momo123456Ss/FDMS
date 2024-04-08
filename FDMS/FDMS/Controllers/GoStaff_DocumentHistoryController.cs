using FDMS.Repository.FDHistoryRepository;
using FDMS.Repository.SystemNoficationRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoStaff_DocumentHistoryController : ControllerBase
    {
        private readonly IFDRepository _iFDRepository;

        public GoStaff_DocumentHistoryController(IFDRepository iFDRepository)
        {
            this._iFDRepository = iFDRepository;
        }
        [HttpGet("GetDocumentHistory")]
        [Authorize(Policy = "RequireGOStaff")]
        public async Task<IActionResult> GetDocumentHistory(string? search, string? date)
        {
            try
            {
                return Ok(await _iFDRepository.GetAll(search, date));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
