using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Security.Credentials;
using System.Collections.ObjectModel;

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class CredentialService
    {
        /// <summary>
        /// 
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
            /// The not logged in
            /// </summary>
            NotSignedIn,
            /// <summary>
            /// The logged in
            /// </summary>
            SignedIn
        }

        /// <summary>
        /// The resource name
        /// </summary>
        private const string ResourceName = "ResPack Studio";

        /// <summary>
        /// The vault
        /// </summary>
        private static readonly PasswordVault Vault = new PasswordVault();

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static PasswordCredential Current
        {
            get
            {
                IReadOnlyList<PasswordCredential> credentialList;

                try
                {
                    credentialList = Vault.FindAllByResource(ResourceName);
                }
                catch (System.Runtime.InteropServices.COMException)
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

        private static readonly AppSettings AppSettings = new AppSettings();

        /// <summary>
        /// Gets all credentials.
        /// </summary>
        /// <value>
        /// All.
        /// </value>
        public static ObservableCollection<PasswordCredential> All => new ObservableCollection<PasswordCredential>(Vault.RetrieveAll());

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public static int Count => Vault.RetrieveAll().Count;

        /// <summary>
        /// Gets a value indicating whether the password vault is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if empty; otherwise, <c>false</c>.
        /// </value>
        public static bool Empty => Count == 0;

        /// <summary>
        /// Checks if the specified credential is available.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns></returns>
        public static bool CheckIfAvailable(PasswordCredential credential) => All.Any(c => c.UserName.Equals(credential.UserName));

        public static PasswordCredential Find(string userName) => All.FirstOrDefault(c => c.UserName.Equals(userName));

        /// <summary>
        /// Determines whether [contains] [the specified credential].
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified credential]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static PasswordCredential Create(string username, string password)
        {
            Debug.WriteLine($"Creating new credential with username {username} and password {password}.");
            var credential = new PasswordCredential(ResourceName, username, password);

            if (Find(username) is PasswordCredential existingCredential)
            {
                existingCredential.RetrievePassword();

                if (existingCredential.Password.Equals(password))
                {
                    Debug.WriteLine($"Credential {credential.UserName} already exists. No need to add to vault.");
                    AppSettings.DefaultUser = credential.UserName;
                    return credential;
                }

                Debug.WriteLine($"Credential {credential.UserName} already exists, but with a different password. Proceeds to update it with the new password.");
                Delete(existingCredential);
            }
            
            Vault.Add(credential);
            Debug.WriteLine($"Credential {credential.UserName} was added to the vault.");
            AppSettings.DefaultUser = credential.UserName;
            Debug.WriteLine($"Credential {credential.UserName} was set as Default.");

            return credential;
        }

        /// <summary>
        /// Deletes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static Response Delete(string username, string password)
        {
            return Delete(new PasswordCredential(ResourceName, username, password));
        }

        /// <summary>
        /// Deletes the specified credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns></returns>
        public static Response Delete(PasswordCredential credential)
        {
            if (!Contains(credential))
                return Response.NotFound;

            Vault.Remove(credential);

            if (credential.UserName.Equals(AppSettings.DefaultUser))
                AppSettings.DefaultUser = "";

            return Response.Removed;
        }

        /// <summary>
        /// Deletes all.
        /// </summary>
        public static void DeleteAll()
        {
            foreach (var credential in Vault.RetrieveAll())
                Vault.Remove(credential);
        }
    }
}
