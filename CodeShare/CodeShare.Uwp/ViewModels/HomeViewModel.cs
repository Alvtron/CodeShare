// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="HomeViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class HomeViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class HomeViewModel : ViewModel
    {
        /// <summary>
        /// Gets the most popular codes.
        /// </summary>
        /// <value>The most popular codes.</value>
        public ObservableCollection<Code> MostPopularCodes { get; } = new ObservableCollection<Code>();

        /// <summary>
        /// Gets the newest codes.
        /// </summary>
        /// <value>The newest codes.</value>
        public ObservableCollection<Code> NewestCodes { get; } = new ObservableCollection<Code>();

        /// <summary>
        /// Gets the newest questions.
        /// </summary>
        /// <value>The newest questions.</value>
        public ObservableCollection<Question> NewestQuestions { get; } = new ObservableCollection<Question>();

        /// <summary>
        /// Gets the newest users.
        /// </summary>
        /// <value>The newest users.</value>
        public ObservableCollection<User> NewestUsers { get; } = new ObservableCollection<User>();

        /// <summary>
        /// Refreshes the most popular codes.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshMostPopularCodes()
        {
            MostPopularCodes.Clear();
            var codes = await RestApiService<Code>.Get();

            codes?.OrderByDescending(x => x.Views).ToList().ForEach(MostPopularCodes.Add);
        }

        /// <summary>
        /// Refreshes the newest codes.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshNewestCodes()
        {
            NewestCodes.Clear();
            var codes = await RestApiService<Code>.Get();

            codes?.OrderByDescending(x => x.Created).ToList().ForEach(NewestCodes.Add);
        }

        /// <summary>
        /// Refreshes the newest questions.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshNewestQuestions()
        {
            NewestQuestions.Clear();
            var questions = await RestApiService<Question>.Get();

            questions?.OrderByDescending(x => x.Created).ToList().ForEach(NewestQuestions.Add);
        }

        /// <summary>
        /// Refreshes the newest users.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshNewestUsers()
        {
            NewestUsers.Clear();
            var users = await RestApiService<User>.Get();

            users?.OrderByDescending(x => x.Created).ToList().ForEach(NewestUsers.Add);
        }
    }
}
