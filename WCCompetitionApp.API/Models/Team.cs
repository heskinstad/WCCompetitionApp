namespace WCCompetitionApp.API.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<GroupPlay> GroupPlays { get; set; }
        public List<Match> Matches { get; set; }
    }
}
