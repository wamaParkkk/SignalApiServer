namespace SignalApiServer.Models
{
    public class SignalLog
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
