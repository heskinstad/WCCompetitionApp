namespace WCCompetitionApp.API.Models
{
    public class GroupPlay
    {
        public Guid Id { get; set; }
        public Guid Team1Id { get; set; }
        public Team Team1 { get; set; }
        public Guid Team2Id { get; set; }
        public Team Team2 { get; set; }
        public Guid Team3Id { get; set; }
        public Team Team3 { get; set; }
        public Guid Team4Id { get; set; }
        public Team Team4 { get; set; }
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;
        public int Team3Score { get; set; } = 0;
        public int Team4Score { get; set; } = 0;
    }
}
