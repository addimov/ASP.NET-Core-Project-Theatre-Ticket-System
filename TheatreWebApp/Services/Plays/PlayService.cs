using System.Collections.Generic;
using System.Linq;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Plays;
using TheatreWebApp.Services.Plays.Models;

namespace TheatreWebApp.Services.Plays
{
    public class PlayService : IPlayService
    {
        private readonly TheatreDbContext data;

        public PlayService(TheatreDbContext data)
        {
            this.data = data;
        }

        public void Add(
            string name, 
            string description, 
            string shortDescription, 
            string credits, 
            string imageUrl)
        {
            var play = new Play
            {
                Name = name,
                Description = description,
                ShortDescription = shortDescription,
                Credits = credits,
                ImageUrl = imageUrl
            };

            if (imageUrl == null)
            {
                play.ImageUrl = "https://inventionland.com/wp-content/uploads/2018/04/theater-stage.jpg";
            }


            data.Plays.Add(play);
            data.SaveChanges();
        }

        public PlayQueryServiceModel All(
            string searchTerm = null, 
            int currentPage = 1,
            bool showHidden = false,
            int playsPerPage = int.MaxValue 
            )
        {

            var playsQuery = data.Plays.OrderByDescending(p => p.Id).AsQueryable();

            if(showHidden == false)
            {
                playsQuery = playsQuery.Where(p => p.IsHidden == false);
            }

            if (searchTerm != null)
            {
                playsQuery = playsQuery.Where(p => p.Name.Contains(searchTerm) 
                || p.Description.Contains(searchTerm) 
                || p.ShortDescription.Contains(searchTerm));
            }

            var totalPlays = playsQuery.Count();

            playsQuery = playsQuery
                .Skip((currentPage - 1) * playsPerPage)
                .Take(playsPerPage);

            var plays = playsQuery
                .Select(p => new PlayServiceModel
                {
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    ImageUrl = p.ImageUrl,
                    Id = p.Id
                })
                .ToList();

            var playModel = new PlayQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalPlays = totalPlays,
                PlaysPerPage = playsPerPage,
                SearchTerm = searchTerm,
                Plays = plays
            };

            return playModel;
        }

        public PlayDetailsServiceModel Details(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => new PlayDetailsServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    Credits = p.Credits,
                    ImageUrl = p.ImageUrl,
                    IsHidden = p.IsHidden,
                    Paragraphs = SplitIntoParagraphs(p.Description)
                })
                .FirstOrDefault();

            return play;
        }

        private static IEnumerable<string> SplitIntoParagraphs(string text)
        {
            var splittedText = text.Split('\n').ToList();

            return splittedText;
        }

        public void ChangeVisibility(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => p)
                .FirstOrDefault();

            if (play.IsHidden == false)
            {
                play.IsHidden = true;
            }
            else
            {
                play.IsHidden = false;
            }

            data.Plays.Update(play);
            data.SaveChanges();
        }

        public PlayFormModel FormDetails(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => new PlayFormModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    ShortDescription = p.ShortDescription,
                    Credits = p.Credits,
                    ImageUrl = p.ImageUrl,
                    Id = playId
                })
                .FirstOrDefault();

            return play;
        }

        public void Edit(PlayFormModel playForm)
        {
            var play = data.Plays
                .Where(p => p.Id == playForm.Id)
                .Select(p => p)
                .FirstOrDefault();

            play.Name = playForm.Name;
            play.ShortDescription = playForm.ShortDescription;
            play.Description = playForm.Description;
            play.Credits = playForm.Credits;
            play.ImageUrl = playForm.ImageUrl;

            data.Plays.Update(play);
            data.SaveChanges();
        }

        public IEnumerable<PlayServiceModel> Latest()
        {
            var plays = data.Plays
                .Select(p => new PlayServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    ImageUrl = p.ImageUrl
                })
                .OrderByDescending(p => p.Id)
                .Take(3)
                .ToList();

            return plays;
        }
    }
}
