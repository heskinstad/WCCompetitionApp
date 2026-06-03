namespace WCCompetitionApp.API.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid Team1Id { get; set; }
        public Team Team1 { get; set; }
        public Guid Team2Id { get; set; }
        public Team Team2 { get; set; }
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;
        public string League { get; set; }
        public bool Finished { get; set; } = false;
    }
}
