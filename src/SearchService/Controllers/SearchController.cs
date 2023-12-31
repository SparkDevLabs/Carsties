﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;

    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams){

        var query = DB.PagedSearch<Item, Item>();


        if (!string.IsNullOrEmpty(searchParams.searchTerm))
        {
            query.Match(Search.Full, searchParams.searchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch 
        {
            
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))

        };


        query = searchParams.FilterBy switch 
        {
            
            "finished" => query.Match(x => x.AuctionEnd < DateTime.Now),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.Now.AddHours(6)), 
            _ => query.Match( x => x.AuctionEnd > DateTime.Now)

        };

        if(!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if(!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }


       
        

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new 
        {
            result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount

            });

    }
}

