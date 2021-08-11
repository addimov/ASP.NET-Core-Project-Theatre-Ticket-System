using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Models.Shows;
using TheatreWebApp.Services.Shows.Models;

namespace TheatreWebApp.Services.Shows
{
    public interface IShowService
    {
        ShowQueryModel All(
            int playId = 0,
            string searchTerm = null,
            string afterDate = null,
            string beforeDate = null,
            int currentPage = 1,
            int showsPerPage = int.MaxValue,
            bool showOlder = false
            );

        AddShowFormModel PrepareForm();

        void Add(
            int playId,
            int stageId,
            string date,
            string time
            );

        EditShowFormModel PrepareEditForm(int showId);

        void Edit(EditShowFormModel show);

        bool PlayExists(int playId);

        bool StageExists(int stageId);

        bool IsDateValid(string date);

        bool IsShowAvailable(int showId);
    }
}
