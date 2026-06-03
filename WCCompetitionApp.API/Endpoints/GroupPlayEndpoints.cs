using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WCCompetitionApp.API.Models;
using WCCompetitionApp.API.Repositories;

namespace WCCompetitionApp.API.Endpoints
{
    public static class GroupPlayEndpoints
    {
        public static void ConfigureGroupPlays(this WebApplication app)
        {
            var groupPlays = app.MapGroup("/groupPlay");

            groupPlays.MapPost("/", Insert);
            groupPlays.MapGet("/", Get);
            groupPlays.MapGet("/{id}", GetById);
            //groupPlays.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<GroupPlay> repository, IMapper mapper, GroupPlayPost groupPlay)
        {
            try
            {
                var newGroupPlay = mapper.Map<GroupPlay>(groupPlay);

                await repository.Insert(newGroupPlay);

                return TypedResults.Created($"/groupPlay/{newGroupPlay.Id}", newGroupPlay);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<GroupPlay> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<GroupPlayGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<GroupPlay> repository, IMapper mapper, Guid id)
        {
            try
            {
                var groupPlay = await repository.GetById(id);

                if (groupPlay == null)
                    return Results.NotFound();

                var response = mapper.Map<GroupPlayGet>(groupPlay);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
