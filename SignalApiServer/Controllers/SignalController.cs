using Microsoft.AspNetCore.Mvc;

namespace SignalApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SignalData data)
        {
            var now = DateTime.Now.ToString("HH:mm:ss");
            var log = $"[{now}] Asset: {data.Asset}, Line: {data.LineCode}, " +
                      $"Red:{data.SignalRed}, Yellow:{data.SignalYellow}, Green:{data.SignalGreen}";
            Console.WriteLine(log);
            
            // Asset 기준 처리 (예: 로직 분기 가능)
            if (data.Asset == "K2025-1000000")
            {
                // 장비1 처리
            }
            else if (data.Asset == "M3001-2000000")
            {
                // 장비2 처리
            }

            return Ok("수신 완료");
        }
    }
    public class SignalData
    {
        public string Type { get; set; }
        public string LineCode { get; set; }
        public string Asset { get; set; }
        public int SignalRed { get; set; }
        public int SignalYellow { get; set; }
        public int SignalGreen { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
    }
}