// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="CredentialService.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using Windows.Security.Credentials;
using System.Collections.ObjectModel;
using CodeShare.Utilities;
using System;

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// Class CredentialService.
    /// </summary>
    public static class CredentialService
    {
        /// <summary>
        /// Enum Response
        /// </summary>
        public enum Response
        {
            /// <summary>
            /// The exist
            /// </summary>
            Exist,
            /// <summary>
            /// The not found
            /// </summary>
            NotFound,
            /// <summary>
            /// The added
            /// </summary>
            Added,
            /// <summary>
            /// The removed
            /// </summary>
            Removed,
            /// <summary>
            /// The not signed in
            /// </summary>
            NotSignedIn,
            /// <summary>
            /// The signed in
            /// </summary>
            SignedIn
        }

        /// <summary>
        /// The resource name
        /// </summary>
        private const string ResourceName = "CodeShare";

        /// <summary>
        /// The vault
        /// </summary>
        private static readonly PasswordVault Vault = new PasswordVault();

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <value>The current.</value>
        public static PasswordCredential Current
        {
            get
            {
                IReadOnlyList<PasswordCredential> credentialList;

                try
                {
                    credentialList = Vault.FindAllByResource(ResourceName);
                }
                catch (Exception)
                {
                    // No credentials found with the resource name.
                    return null;
                }

                switch (credentialList.Count)
                {
                    case 0:
                        // There's no credentials stored in the vault.
                        return null;
                    case 1:
                        // There's exaclty one credential stored in the vault.
                        credentialList[0].RetrievePassword();
                        return credentialList[0];
                }

                // There are multiple credentials. Check if there is a default credential.
                if (!string.IsNullOrWhiteSpace(AppSettings.DefaultUser))
                {
                    var credential = Vault.Retrieve(ResourceName, AppSettings.DefaultUser);
                    credential.RetrievePassword();
                    return credential;
                }

                // TEMPORARY SOLUTION: Pick the first credential.
                credentialList[0].RetrievePassword();
                return credentialList[0];
            }
        }

        /// <summary>
        /// The application settings
        /// </summary>
        private static readonly AppSettings AppSettings = new AppSettings();

        /// <summary>
        /// Gets all credentials.
        /// </summary>
        /// <value>Images.</value>
        public static ObservableCollection<PasswordCredential> All => new ObservableCollection<PasswordCredential>(Vault.RetrieveAll());

        /// <summary>
        /// The amount of credentials in the vault.
        /// </summary>
        /// <value>The amount of credentials in the vault.</value>
        public static int Count => Vault.RetrieveAll().Count;

        /// <summary>
        /// Gets a value indicating whether the password vault is empty.
        /// </summary>
        /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
        public static bool Empty => Count == 0;

        /// <summary>
        /// Checks if the specified credential is available.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckIfAvailable(PasswordCredential credential) => All.Any(c => c.UserName.Equals(credential.UserName));

        /// <summary>
        /// Finds a credential that matches the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>PasswordCredential.</returns>
        public static PasswordCredential Find(string userName) => All.FirstOrDefault(c => c.UserName.Equals(userName));

        /// <summary>
        /// Determines whether the vault containst the specified credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns><c>true</c> if the vault contains the specified credential; otherwise, <c>false</c>.</returns>
        public static bool Contains(PasswordCredential credential)
        {
            if (Empty) return false;

            credential.RetrievePassword();

            foreach (var c in All)
            {
                c.RetrievePassword();
                if (credential.UserName.Equals(c.UserName) && credential.Password.Equals(c.Password))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a new credential to the vault.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>PasswordCredential.</returns>
        public static PasswordCredential Create(string userName, string password)
        {
            Logger.WriteLine($"Creating new credential with username {userName} and password {password}.");
            var credential = new PasswordCredential(ResourceName, userName, password);

            if (Find(userName) is PasswordCredential existingCredential)
            {
                existingCredential.RetrievePassword();

                if (existingCredential.Password.Equals(password))
                {
                    Logger.WriteLine($"Credential {credential.UserName} already exists. No need to add to vault.");
                    AppSettings.DefaultUser = credential.UserName;
                    return credential;
                }

                Logger.WriteLine($"Credential {credential.UserName} already exists, but with a different password. Proceeds to update it with the new password.");
                Delete(existingCredential);
            }
            
            Vault.Add(credential);
            Logger.WriteLine($"Credential {credential.UserName} was added to the vault.");
            AppSettings.DefaultUser = credential.UserName;
            Logger.WriteLine($"Credential {credential.UserName} was set as Default.");

            return credential;
        }

        /// <summary>
        /// Deletes the a password credential that matches the specified user name and password.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Response.</returns>
        public static Response Delete(string userName, string password)
        {
            return Delete(new PasswordCredential(ResourceName, userName, password));
        }

        /// <summary>
        /// Deletes the specified credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Response.</returns>
        public static Response Delete(PasswordCredential credential)
        {
            if (!Contains(credential))
            {
                return Response.NotFound;
            }

            Vault.Remove(credential);

            if (credential.UserName.Equals(AppSettings.DefaultUser))
            {
                AppSettings.DefaultUser = "";
            }

            return Response.Removed;
        }

        /// <summary>
        /// Deletes all stored credentials in the vault.
        /// </summary>
        public static void DeleteAll()
        {
            foreach (var credential in Vault.RetrieveAll())
            {
                Vault.Remove(credential);
            }
        }
    }
}
