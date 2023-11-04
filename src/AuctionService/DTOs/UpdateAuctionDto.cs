﻿using System.ComponentModel.DataAnnotations;

namespace AuctionService.Dto;

public class UpdateAuctionDto
{

public string  Make { get; set; }  
[Required] 
public string Model { get; set; }   
[Required]
public int? Year { get; set; }
[Required]
public string Color { get; set; }  
[Required]
public int?  Mileage { get; set; }
}
