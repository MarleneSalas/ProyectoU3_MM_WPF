using ClienteServidorProyectoU3.Helpers;
using ClienteServidorProyectoU3.Models;
using ClienteServidorProyectoU3.Services;
using ClienteServidorProyectoU3.Views;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClienteServidorProyectoU3.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region command

        public ICommand LoginCommand { get; set; }

        #endregion
        #region Properties
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public string Error { get; set; } = "";

        public bool IsLoading = false;
        #endregion

        //Objects
        LoginService service;
        LoginDTO loginDTO;
        Cifrado cifrado;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);  
            service = new LoginService();
            loginDTO = new();
            cifrado = new();

            //Para hacer autologin
            User = "direccion_academica@gmail.com";
            Password = "12345678";
            Login();
        }

        #region Methods
        private async void Login()
        {

            try
            {
                IsLoading = true;
                Error = "";
                Actualizar();

                if (string.IsNullOrWhiteSpace(User))
                    Error = "El nombre de usuario no debe ir vacio" + Environment.NewLine;
                if (string.IsNullOrWhiteSpace(Password))
                    Error = "La contraseña no debe ir vacia";
                if (Error == "")
                {
                    loginDTO.User = User.Trim();
                    loginDTO.Password = CifradoHash.StringToSHA512(Password);

                    var login = await service.Login(loginDTO);


                    if (login.Contains("id"))
                    {
                        User = "";
                        Password = "";
                        DepartamentoDTO? departamento = JsonConvert.DeserializeObject<DepartamentoDTO>(login);
                        if (departamento != null)
                        {
                            App.ActividadesViewModel.DefineDepto(departamento);
                            App.ActividadesViewModel.LoadData();

                            App.ActividadesViewModel.DepartamentoActual = departamento;
                            App.MainViewModel.CambiarVista(Vista.Act);
                            App.ActividadesViewModel.Actualizar();
                        }

                    }
                    else
                        Error = login;
                }
                IsLoading = false;
                Actualizar();
            }
            catch(Exception ex) 
            { 
                Error = ex.Message;
                App.MostrarError(Error);
            }
        }

        public void Logout()
        {
            App.ActividadesViewModel.DefineDepto(new DepartamentoDTO());
            App.ActividadesViewModel.Deptos.Clear();

            App.ActividadesViewModel.DepartamentoActual = new();
            App.MainViewModel.CambiarVista(Vista.Login);
        }
       
        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        #endregion

        //Events
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
