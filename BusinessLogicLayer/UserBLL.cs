using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;
using System.Text.RegularExpressions;

namespace RestaurantApp.BusinessLogicLayer
{
    public class UserBLL
    {
        private readonly UserDAL dal = new();

        public UserAccount Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new RestaurantException(
                    "Introduci adresa de email.");
            if (string.IsNullOrWhiteSpace(password))
                throw new RestaurantException(
                    "Introdu parola.");

            var user = dal.Login(email, password);
            if (user == null)
                throw new RestaurantException(
                    "Email sau parola incorecte.");
            return user;
        }

        public void Register(UserAccount user, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new RestaurantException(
                    "Prenumele nu poate fi gol.");
            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new RestaurantException(
                    "Numele nu poate fi gol.");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new RestaurantException(
                    "Adresa de email nu poate fi goala.");
            if (!IsValidEmail(user.Email))
                throw new RestaurantException(
                    "Adresa de email nu este valida.");
            if (string.IsNullOrWhiteSpace(password))
                throw new RestaurantException(
                    "Parola nu poate fi goala.");
            if (password.Length < 6)
                throw new RestaurantException(
                    "Parola trebuie sa aiba cel putin 6 caractere.");
            if (password != confirmPassword)
                throw new RestaurantException(
                    "Parolele nu coincid.");

            user.PasswordHash = password;
            dal.RegisterUser(user);
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
