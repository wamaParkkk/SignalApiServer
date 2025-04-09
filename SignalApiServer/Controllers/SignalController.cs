using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SignalApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SignalController : ControllerBase
    {
        private readonly string _connectionString = "Server=10.131.15.18;Database=EE;User Id=eeuser;Password=Amkor123!;Encrypt=False;TrustServerCertificate=True;";

        [HttpPost]
        public IActionResult Post([FromBody] SignalData data)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {data.Type} / {data.LineCode} / {data.Asset} - Red:{data.SignalRed} Yellow:{data.SignalYellow} Green:{data.SignalGreen} Spare:{data.SignalSpare} Remarks1:{data.Remarks1} Remarks2:{data.Remarks2}");

            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                
                var sql = @"
MERGE K5EE_SignalLog AS Target
USING (SELECT @Asset AS Asset) AS Source
ON Target.Asset = Source.Asset
WHEN MATCHED THEN
   UPDATE SET
       Type = @Type,
       LineCode = @LineCode,       
       SignalRed = @Red,
       SignalYellow = @Yellow,
       SignalGreen = @Green,
       SignalSpare = @Spare,
       Remarks1 = @Remarks1,
       Remarks2 = @Remarks2,
       UpdatedAt = GETDATE()
WHEN NOT MATCHED THEN
   INSERT (Type, LineCode, Asset, SignalRed, SignalYellow, SignalGreen, SignalSpare, Remarks1, Remarks2, UpdatedAt)
   VALUES (@Type, @LineCode, @Asset, @Red, @Yellow, @Green, @Spare, @Remarks1, @Remarks2, GETDATE());
";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Type", data.Type);
                cmd.Parameters.AddWithValue("@LineCode", data.LineCode);
                cmd.Parameters.AddWithValue("@Asset", data.Asset);                                
                cmd.Parameters.AddWithValue("@Red", data.SignalRed);
                cmd.Parameters.AddWithValue("@Yellow", data.SignalYellow);
                cmd.Parameters.AddWithValue("@Green", data.SignalGreen);
                cmd.Parameters.AddWithValue("@Spare", data.SignalSpare);
                cmd.Parameters.AddWithValue("@Remarks1", data.Remarks1);
                cmd.Parameters.AddWithValue("@Remarks2", data.Remarks2);
                cmd.ExecuteNonQuery();

                return Ok("DB 저장 완료 (업데이트 또는 삽입)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB 오류: {ex.Message}");
                return StatusCode(500, "DB 저장 실패");
            }
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
        public int SignalSpare { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
    }
}