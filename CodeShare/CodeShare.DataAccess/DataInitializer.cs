﻿// ***********************************************************************
// Assembly         : CodeShare.DataAccess
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="DataInitializer.cs" company="CodeShare.DataAccess">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Services;
using CodeShare.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CodeShare.DataAccess
{
    /// <summary>
    /// Class DataInitializer.
    /// </summary>
    public static class DataInitializer
    {
        /// <summary>
        /// A formatted lorem ipsum string
        /// </summary>
        private const string LoremIpsumFormatted = @"{\rtf1\fbidis\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}{\f1\fnil Segoe UI;}} {\colortbl ;\red255\green255\blue255;} {\*\generator Riched20 10.0.17134}\viewkind4\uc1 \pard\tx720\cf1\b\f0\fs23\lang1044 Lorem ipsum\b0 dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Dui sapien eget mi proin sed libero enim sed. Interdum varius sit amet mattis vulputate enim nulla aliquet porttitor. Mauris pharetra et ultrices neque ornare aenean euismod elementum. Tempus iaculis urna id volutpat lacus. Felis eget nunc lobortis mattis aliquam faucibus purus in. Risus ultricies tristique nulla aliquet. Et egestas quis ipsum suspendisse. Facilisis volutpat est velit egestas dui. Vel eros donec ac odio tempor orci dapibus ultrices in. Molestie at elementum eu facilisis sed odio morbi. Odio euismod lacinia at quis risus sed vulputate odio. Viverra mauris in aliquam sem fringilla ut. Risus nullam eget felis eget. Vel turpis nunc eget lorem dolor sed viverra.\par \par \i Sapien pellentesque habitant morbi tristique senectus. Sapien nec sagittis aliquam malesuada bibendum arcu vitae. Sit amet volutpat consequat mauris nunc congue nisi vitae suscipit. Purus ut faucibus pulvinar elementum. Est ullamcorper eget nulla facilisi etiam dignissim diam. Egestas erat imperdiet sed euismod nisi porta lorem. Ultrices in iaculis nunc sed augue. Diam vel quam elementum pulvinar. Urna molestie at elementum eu. Id ornare arcu odio ut. Scelerisque eu ultrices vitae auctor eu augue ut lectus. Imperdiet proin fermentum leo vel orci porta non pulvinar. Amet consectetur adipiscing elit ut. Nec ullamcorper sit amet risus nullam eget felis eget nunc. Mauris augue neque gravida in fermentum et. Non curabitur gravida arcu ac tortor. Pharetra massa massa ultricies mi. Vitae nunc sed velit dignissim sodales ut. Pharetra sit amet aliquam id.\par \i0\par Morbi leo urna molestie at elementum eu facilisis. Suspendisse potenti nullam ac tortor vitae purus faucibus. Aliquam sem et tortor consequat id porta nibh venenatis cras. Egestas egestas fringilla phasellus faucibus scelerisque eleifend donec. Lectus arcu bibendum at varius vel pharetra vel turpis nunc. Tortor condimentum lacinia quis vel eros donec ac odio. Vulputate mi sit amet mauris commodo. Sagittis nisl rhoncus mattis rhoncus urna neque viverra. Non consectetur a erat nam. Imperdiet massa tincidunt nunc pulvinar sapien et ligula ullamcorper. Nec sagittis aliquam malesuada bibendum arcu vitae elementum. Orci a scelerisque purus semper eget. Urna duis convallis convallis tellus. Integer quis auctor elit sed vulputate mi sit. Nunc vel risus commodo viverra.\par \par Et pharetra pharetra massa massa ultricies. Ultrices in iaculis nunc sed augue lacus viverra vitae congue. Habitasse platea dictumst quisque sagittis. Sem et tortor consequat id porta. Scelerisque eu ultrices vitae auctor eu. Eget nunc lobortis mattis aliquam faucibus purus in massa. Tortor consequat id porta nibh venenatis cras sed. Aliquam eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis. Nunc lobortis mattis aliquam faucibus purus. Justo nec ultrices dui sapien eget mi proin sed. Proin fermentum leo vel orci. Semper auctor neque vitae tempus quam pellentesque. Nec feugiat nisl pretium fusce id. Libero id faucibus nisl tincidunt eget. In ante metus dictum at. Lobortis feugiat vivamus at augue. Mauris pharetra et ultrices neque ornare aenean euismod. Facilisis leo vel fringilla est. Mattis pellentesque id nibh tortor id aliquet. Pellentesque sit amet porttitor eget dolor.\par \par Et magnis dis parturient montes nascetur ridiculus mus mauris vitae. A diam maecenas sed enim ut. Phasellus faucibus scelerisque eleifend donec pretium vulputate sapien nec sagittis. Scelerisque in dictum non consectetur a erat. Diam phasellus vestibulum lorem sed risus. Arcu vitae elementum curabitur vitae nunc sed. Id volutpat lacus laoreet non curabitur gravida arcu. Ut sem nulla pharetra diam sit amet nisl. Interdum velit laoreet id donec. In massa tempor nec feugiat nisl pretium fusce. Facilisi cras fermentum odio eu feugiat. Odio euismod lacinia at quis risus. Et magnis dis parturient montes nascetur ridiculus. Ut lectus arcu bibendum at varius. Velit aliquet sagittis id consectetur. Posuere morbi leo urna molestie. Sit amet porttitor eget dolor morbi. Mi in nulla posuere sollicitudin aliquam ultrices sagittis orci a.\f1\lang1033\par }";

        /// <summary>
        /// Creates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Create(DataContext context)
        {
            context.Database.EnsureCreated();
        }

        /// <summary>
        /// Deletes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Delete(DataContext context)
        {
            context.Database.EnsureDeleted();

            Logger.WriteLine($"Deleting all files in FTP directory at {FtpService.RootDirectoryFtp}.");
            FtpService.DeleteAll();
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Seed(DataContext context)
        {
            Logger.WriteLine("Seeding database...");

            Logger.WriteLine("Adding seed-data to database.");
            var random = new Random();

            var kim = new User("Kimdrello", "kim@hiof.no", "#Kimdrello23")
            {
                Uid = new Guid("7d7a1b66-1e43-420d-9ebe-9e4402ef94c1"),
                FirstName = "Kim-Andre",
                LastName = "Engebretsen",
                Bio = LoremIpsumFormatted,
                Experience = random.Next(0, 40000),
            };

            var vegard = new User("Vegster", "vegard@hiof.no", "#Vegard22")
            {
                Uid = new Guid("b5cb8d43-cd8d-4ec0-a235-fc29b5702b49"),
                FirstName = "Vegard",
                LastName = "Strand",
                Bio = LoremIpsumFormatted,
                Experience = random.Next(0, 40000)
            };

            var christoffer = new User("Rexzore", "christoffer@hiof.no", "#Christoffer89")
            {
                Uid = new Guid("1e7a2e47-befe-44b8-b70a-23feec139174"),
                FirstName = "Christoffer",
                LastName = "Øyane",
                Bio = LoremIpsumFormatted,
                Experience = random.Next(0, 40000)
            };

            var thomas = new User("Alvtron", "thomas@hiof.no", "#Alvtron1")
            {
                Uid = new Guid("84b7d030-5101-449e-840c-83d1c3abf2c3"),
                FirstName = "Thomas",
                LastName = "Angeland",
                Country = "Norway",
                Gender = "Male",
                Bio = LoremIpsumFormatted,
                Birthday = new DateTime(1994, 1, 16),
                Website = "https://r3dcraft.net",
                Experience = random.Next(0, 40000)
            };

            thomas.SendFriendRequest(kim);
            thomas.SendFriendRequest(christoffer);
            thomas.SendFriendRequest(vegard);
            christoffer.SendFriendRequest(vegard);
            kim.SendFriendRequest(christoffer);

            using (context)
            {
                context.Users.Add(kim);
                context.Users.Add(vegard);
                context.Users.Add(christoffer);
                context.Users.Add(thomas);

                Logger.WriteLine("Adding code languages and syntax to database.");

                var fileName = @"developer_file_extensions.json";
                var json = ReadFileToString(fileName);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException($"{fileName} returned a empty string.");
                }

                var codeLanguages = JsonConvert.DeserializeObject<List<CodeLanguage>>(json);

                foreach (var codeLanguage in codeLanguages)
                {
                    context.CodeLanguages.Add(codeLanguage);
                }

                context.SaveChanges();

                kim.AcceptFriendRequest(thomas);
                vegard.AcceptFriendRequest(thomas);

                context.SaveChanges();
            }
            Logger.WriteLine("Seeding database completed.");
        }

        /// <summary>Reads the file to string.</summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.String.</returns>
        public static string ReadFileToString(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.WriteLine("Provided path was empty.");
                return null;
            }

            try
            {
                return System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath));
            }
            catch (Exception e)
            {
                Logger.WriteLine($"{e.Message}");
                return null;
            }
        }
    }
}
