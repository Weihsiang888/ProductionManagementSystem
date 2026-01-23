using DxBlazorApplication7.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;

namespace DxBlazorApplication7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GetInformationController : ControllerBase
    {
        private readonly DataService _DataService;
        private List<OperatorInformation>? _OperatorInformations;
        private List<TimePeriod> _TimePeriods = new List<TimePeriod>();
        private List<WorkingList>? _WorkingLists;
        public GetInformationController(DataService dataService)
        {
            _DataService = dataService;
        }

        [HttpGet("{type}/{opid}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAsync(string type ,string opid)
        {
            if (type == "OP")
            {
                _OperatorInformations = await _DataService.GetOperatorInformationsAsync();
                return _OperatorInformations;
            }
            else if (type == "TimeStep")
            {
                _TimePeriods = await _DataService.GetTimePeriodsAsync();
                return _TimePeriods;
            }
            else if (type == "Job")
            {
                _WorkingLists = await _DataService.GetWorkingListsAsync(opid);
                return _WorkingLists;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetWorkingOrderListWrongLengthOfWorkOrder")]
        public async Task<ActionResult<List<WorkingOrderList>>> GetWorkingOrderListWrongLengthOfWorkOrder()
        {
            List<WorkingOrderList> list = await _DataService.GetWorkingOrderListWrongLengthOfWorkOrderAsync();

            return list;
        }

        [HttpPost]
        [Route("UpdateWorkingOrderAndStepList")]
        public async Task UpdateWorkingOrderAndStepList(List<WorkingOrderList> lists)
        {
            await _DataService.UpdateWorkingOrderListAndStepListAsync(lists);
        }
    }
}
