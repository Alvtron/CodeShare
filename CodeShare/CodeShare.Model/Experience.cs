// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 02-03-2019
// ***********************************************************************
// <copyright file="Experience.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Class Experience.
    /// </summary>
    public static class Experience
    {
        /// <summary>
        /// Enum Action
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// The add code
            /// </summary>
            AddCode = 500,
            /// <summary>
            /// The add question
            /// </summary>
            AddQuestion = 200,
            /// <summary>
            /// The add reply
            /// </summary>
            AddReply = 50,
            /// <summary>
            /// The upload file
            /// </summary>
            UploadFile = 10,
            /// <summary>
            /// The upload image
            /// </summary>
            UploadImage = 10,
            /// <summary>
            /// The upload video
            /// </summary>
            UploadVideo = 10,
            /// <summary>
            /// The befriend
            /// </summary>
            Befriend = 50,
            /// <summary>
            /// The sign in
            /// </summary>
            SignIn = 10,
            /// <summary>
            /// The changed settings
            /// </summary>
            ChangedSettings = 5
        };

        /// <summary>
        /// The level up exp
        /// </summary>
        public static readonly int LevelUpExp = 5000;

        /// <summary>
        /// Exps to level.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns>System.Int32.</returns>
        public static int ExpToLevel(int exp)
        {
            return exp / LevelUpExp;
        }

        /// <summary>
        /// Levels to exp.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>System.Int32.</returns>
        public static int LevelToExp(int level)
        {
            return level * LevelUpExp;
        }

        /// <summary>
        /// Progresses the exp.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns>System.Int32.</returns>
        public static int ProgressExp(int exp)
        {
            return exp % LevelUpExp;
        }

        /// <summary>
        /// Progresses the exp in percentage.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns>System.Double.</returns>
        public static double ProgressExpInPercentage(int exp)
        {
            return (exp % LevelUpExp / (double)LevelUpExp) * 100.00;
        }
    }
}
