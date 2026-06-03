using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WCCompetitionApp.API.Models;
using WCCompetitionApp.API.Repositories;

namespace WCCompetitionApp.API.Endpoints
{
    public static class TeamEndpoints
    {
        public static void ConfigureTeams(this WebApplication app)
        {
            var teams = app.MapGroup("/team");

            teams.MapPost("/", Insert);
            teams.MapGet("/", Get);
            teams.MapGet("/{id}", GetById);
            //teams.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Team> repository, IMapper mapper, TeamPost team)
        {
            try
            {
                var newTeam = mapper.Map<Team>(team);

                await repository.Insert(newTeam);

                return TypedResults.Created($"/team/{newTeam.Id}", newTeam);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Team> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<TeamGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Team> repository, IMapper mapper, Guid id)
        {
            try
            {
                var team = await repository.GetById(id);

                if (team == null)
                    return Results.NotFound();

                var response = mapper.Map<TeamGet>(team);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
