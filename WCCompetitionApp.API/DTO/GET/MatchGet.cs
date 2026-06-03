namespace WCCompetitionApp.API.Models
{
    public class MatchGet
    {
        public Guid Id { get; set; }
        public Guid Team1Id { get; set; }
        public string Team1 { get; set; }
        public Guid Team2Id { get; set; }
        public string Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public string League { get; set; }
        public bool Finished { get; set; } = false;
    }
}
