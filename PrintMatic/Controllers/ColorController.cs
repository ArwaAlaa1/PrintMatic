using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;

namespace PrintMatic.Controllers
{
    public class ColorController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorController( IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllColors")]
        public async Task<IActionResult> GetAllColors()
        {
            try
            {
                var ColorList = await _unitOfWork.Repository<ProductColor>().GetAllAsync();
                var distinctColors = ColorList
                                    .GroupBy(c => c.HexCode)
                                    .Select(g => g.First());

                var ColorsMapped = _mapper.Map<IEnumerable<ProductColor>, IEnumerable<ColorDto>>(distinctColors);
                return Ok(ColorsMapped);
            } catch (Exception ex) 
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }
        }
    }
}
