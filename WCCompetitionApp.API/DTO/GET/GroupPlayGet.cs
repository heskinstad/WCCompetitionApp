namespace WCCompetitionApp.API.Models
{
    public class GroupPlayGet
    {
        public Guid Id { get; set; }
        public Guid Team1Id { get; set; }
        public string Team1 { get; set; }
        public Guid Team2Id { get; set; }
        public string Team2 { get; set; }
        public Guid Team3Id { get; set; }
        public string Team3 { get; set; }
        public Guid Team4Id { get; set; }
        public string Team4 { get; set; }
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;
        public int Team3Score { get; set; } = 0;
        public int Team4Score { get; set; } = 0;
    }
}
