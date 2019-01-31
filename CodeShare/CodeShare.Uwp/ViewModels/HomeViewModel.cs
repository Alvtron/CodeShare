using CodeShare.Model;
using CodeShare.RestApi;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    public class HomeViewModel
    {
        public ObservableCollection<Code> MostPopularCodes { get; private set; } = new ObservableCollection<Code>();

        public ObservableCollection<Code> NewestCodes { get; private set; } = new ObservableCollection<Code>();

        public ObservableCollection<Question> NewestQuestions { get; private set; } = new ObservableCollection<Question>();

        public ObservableCollection<User> NewestUsers { get; private set; } = new ObservableCollection<User>();

        public async Task RefreshMostPopularCodes()
        {
            MostPopularCodes.Clear();
            var codes = await RestApiService<Code>.Get();

            if (codes == null)
            {
                return;
            }

            codes.OrderByDescending(x => x.Views).ToList().ForEach(MostPopularCodes.Add);
        }

        public async Task RefreshNewestCodes()
        {
            NewestCodes.Clear();
            var codes = await RestApiService<Code>.Get();

            if (codes == null)
            {
                return;
            }

            codes.OrderByDescending(x => x.Created).ToList().ForEach(NewestCodes.Add);
        }

        public async Task RefreshNewestQuestions()
        {
            NewestQuestions.Clear();
            var questions = await RestApiService<Question>.Get();

            if (questions == null)
            {
                return;
            }

            questions.OrderByDescending(x => x.Created).ToList().ForEach(NewestQuestions.Add);
        }

        public async Task RefreshNewestUsers()
        {
            NewestUsers.Clear();
            var users = await RestApiService<User>.Get();

            if (users == null)
            {
                return;
            }

            users.OrderByDescending(x => x.Created).ToList().ForEach(NewestUsers.Add);
        }
    }
}
