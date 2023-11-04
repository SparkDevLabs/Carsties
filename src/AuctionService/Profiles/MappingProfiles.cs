using AuctionService.Dto;
using AutoMapper;

namespace AuctionService.Profiles;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    //map
    CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
    CreateMap<Item, AuctionDto>();
    //reverse map 
    CreateMap<CreateAuctionDto, Auction>().ForMember(x => x.Item, o => o.MapFrom(s => s));
    CreateMap<CreateAuctionDto, Item>();

  }
}
