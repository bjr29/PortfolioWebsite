using MapDataReader;

namespace Api; 

[GenerateDataReaderMapper]
public class Message {
	public int MessageId { get; set; }
	public string? NickName { get; set; }
	public string? Contents { get; set; }
	public DateTime TimeSent { get; set; }
}