using TheatreWebApp.Models.Plays;
using TheatreWebApp.Services.Plays.Models;

namespace TheatreWebApp.Services.Plays
{
    public interface IPlayService
    {
        PlayQueryServiceModel All(
            string searchTerm = null,
            int currentPage = 1,
            bool showHidden = false,
            int playPerPage = int.MaxValue);

        void Add(
            string name, 
            string description,
            string shortDescription,
            string credits,
            string imageUrl);

        PlayDetailsServiceModel Details(int playId);

        void ChangeVisibility(int playId);


        PlayFormModel FormDetails(int playId);

        void Edit(PlayFormModel play);
        
    }
}
