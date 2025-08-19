using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CredentialManagement;
namespace JuanNotTheHuman.Spending.Services
{
    internal interface IUserSecretsService
    {
        string GetSecret(string key);
        void SetSecret(string key, string value);
        void RemoveSecret(string key);
    }
    internal class UserSecretsMarker { };
    internal static class UserSecretsService
    {
        public static string GetSecret(string key)
        {
            using(var cred = new Credential { Target = $"SpendingApp:{key}", PersistanceType = PersistanceType.LocalComputer })
            {
                if (cred.Exists())
                {
                    cred.Load();
                    return cred.Password;
                }
                else
                {
                    return null;
                }
            }
        }
        public static void SetSecret(string key, string value)
        {
            using(var cred = new Credential { Target = $"SpendingApp:{key}", PersistanceType = PersistanceType.LocalComputer })
            {
                cred.Password = value;
                cred.Save();
            }
        }
        public static void RemoveSecret(string key)
        {
            using(var cred = new Credential { Target = $"SpendingApp:{key}", PersistanceType = PersistanceType.LocalComputer })
            {
                if (cred.Exists())
                {
                    cred.Delete();
                }
            }
        }
    }
}
