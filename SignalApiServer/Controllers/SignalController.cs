using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SignalApiServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SignalController : ControllerBase
    {
        private readonly string _connectionString = "Server=10.131.15.18;Database=EE;User Id=eeuser;Password=Amkor123!;Encrypt=False;TrustServerCertificate=True;";

        [HttpPost]
        public IActionResult PostSignal([FromBody] SignalLog model)
        {
            try
            {
                if (model == null)
                    return BadRequest("데이터 없음");

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {model.Type} / {model.LineCode} / {model.Asset} - Red:{model.SignalRed} Yellow:{model.SignalYellow} Green:{model.SignalGreen} Spare:{model.SignalSpare} Remarks1:{model.Remarks1} Remarks2:{model.Remarks2}");

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE K5EE_SignalLog SET Type = @Type, LineCode = @LineCode, SignalRed = @SignalRed, SignalYellow = @SignalYellow, SignalGreen = @SignalGreen, SignalSpare = @SignalSpare, Remarks1 = @Remarks1, Remarks2 = @Remarks2, UpdatedAt = GETDATE()
                                   WHERE Asset = @Asset; 
                                   IF @@ROWCOUNT = 0 
                                   BEGIN
                                   INSERT INTO K5EE_SignalLog (Asset, Type, LineCode, SignalRed, SignalYellow, SignalGreen, SignalSpare, Remarks1, Remarks2, UpdatedAt)
                                   VALUES (@Asset, @Type, @LineCode, @SignalRed, @SignalYellow, @SignalGreen, @SignalSpare, @Remarks1, @Remarks2, GETDATE());
                                   END"
                    ;
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Type", model.Type ?? "None");
                        cmd.Parameters.AddWithValue("@LineCode", model.LineCode ?? "None");
                        cmd.Parameters.AddWithValue("@Asset", model.Asset ?? "None");
                        cmd.Parameters.AddWithValue("@SignalRed", model.SignalRed);
                        cmd.Parameters.AddWithValue("@SignalYellow", model.SignalYellow);
                        cmd.Parameters.AddWithValue("@SignalGreen", model.SignalGreen);
                        cmd.Parameters.AddWithValue("@SignalSpare", model.SignalSpare);
                        cmd.Parameters.AddWithValue("@Remarks1", model.Remarks1 ?? "None");
                        cmd.Parameters.AddWithValue("@Remarks2", model.Remarks2 ?? "None");
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("DB 저장 성공");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"DB 오류: {ex.Message}");
            }
        }
    }
}