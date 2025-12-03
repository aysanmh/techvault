using API.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult<Pagination<TDto>> CreatePagedResult<TDto>(
            IReadOnlyList<TDto> items,
            int pageIndex,
            int pageSize,
            int totalCount)
        {
            var pagination = new Pagination<TDto>(pageIndex, pageSize, totalCount, items);
            return Ok(pagination);
        }
    }
}
