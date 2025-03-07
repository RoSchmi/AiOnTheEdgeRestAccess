

public class MeterJson
{
    public MeterDatum? main { get; set; }
}

public class MeterDatum
{
    public string value { get; set; }
    public string raw { get; set; }
    public string pre { get; set; }
    public string error { get; set; }
    public string rate { get; set; }
    public string timestamp { get; set; }
}
