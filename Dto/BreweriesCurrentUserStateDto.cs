using BrewTrack.Models;
namespace BrewTrack.Dto;

public class BreweriesCurrentUserStateDto
{
	public IList<BrewPub> Data { get; set; }
	public int LastVisitedPage { get; set; }
	public int TotalPages { get; set; }
}

