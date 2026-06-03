using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WCCompetitionApp.API.Models;
using WCCompetitionApp.API.Repositories;

namespace WCCompetitionApp.API.Endpoints
{
    public static class MatchEndpoints
    {
        public static void ConfigureMatches(this WebApplication app)
        {
            var matches = app.MapGroup("/match");

            matches.MapPost("/", Insert);
            matches.MapGet("/", Get);
            matches.MapGet("/{id}", GetById);
            //matchs.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Match> repository, IMapper mapper, MatchPost match)
        {
            try
            {
                var newMatch = mapper.Map<Match>(match);

                await repository.Insert(newMatch);

                return TypedResults.Created($"/match/{newMatch.Id}", newMatch);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Match> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<MatchGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Match> repository, IMapper mapper, Guid id)
        {
            try
            {
                var match = await repository.GetById(id);

                if (match == null)
                    return Results.NotFound();

                var response = mapper.Map<MatchGet>(match);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
