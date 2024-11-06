using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core.Entities;
using PrintMatic.Core;
using PrintMatic.DTOS;

namespace PrintMatic.Controllers
{
    public class SizeController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SizeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllSizes")]
        public async Task<IActionResult> GetAllSizes()
        {
            try
            {
                var SizeList = await _unitOfWork.Repository<ProductSize>().GetAllAsync();
                var distinctSizes = SizeList
                                    .GroupBy(c => c.Size)
                                    .Select(g => g.First());

                var ColorsMapped = _mapper.Map<IEnumerable<ProductSize>, IEnumerable<SizeDto>>(distinctSizes);
                return Ok(ColorsMapped);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }
        }
    }
}
