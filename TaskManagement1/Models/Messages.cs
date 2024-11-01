namespace TaskManagement1.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Sunject { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsReplied { get; set; }

        public int Status { get; set; }
    }
}
