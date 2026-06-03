using AutoMapper;
using WCCompetitionApp.API.Models;

namespace RecipeApp.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamGet>();
            CreateMap<TeamPost, Team>();

            CreateMap<Match, MatchGet>();
            CreateMap<MatchPost, Match>();

            CreateMap<GroupPlay, GroupPlayGet>();
            CreateMap<GroupPlayPost, GroupPlay>();
        }
    }
}
