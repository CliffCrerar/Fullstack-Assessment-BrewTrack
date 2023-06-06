using BrewTrack.Models;
namespace BrewTrack.Dto;

public class BreweriesCurrentUserStateDto
{
	public IList<BrewPub> PageData { get; set; }
	public int PageNo { get; set; }
	public int OfPages { get; set; }
}

