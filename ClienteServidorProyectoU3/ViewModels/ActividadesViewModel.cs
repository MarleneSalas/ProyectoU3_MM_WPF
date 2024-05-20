using ClienteServidorProyectoU3.Helpers;
using ClienteServidorProyectoU3.Models;
using ClienteServidorProyectoU3.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClienteServidorProyectoU3.ViewModels
{
    public class ActividadesViewModel : INotifyPropertyChanged
    {

        public ICommand FiltrarCommand { get; set; }
        public ICommand VerActividadCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand VerNuevoCommand { get; set; }
        public ICommand GuardarNuevoCommand { get; set; }
        public ICommand EditarActividadCommand { get; set; }
        public ICommand EliminarActividadCommand { get; set; }
        public ICommand VerActividadesCommand { get; set; }
        public ICommand VerDepartamentosCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand VerDetallesDepartamentoCommand { get; set; }
        public ICommand EditarDeptoCommand { get; set; }
        public ICommand EliminarDeptoCommand { get; set; }
        public ICommand VerNuevoDeptoCommand { get; set; }
        public ICommand RegistraDeptoCommand { get; set; }

        public List<Estado> Estados { get; set; }
        public ObservableCollection<DepartamentoDTO> Deptos { get; set; }
        public ObservableCollection<DepartamentoDTO> DeptosHijos { get; set; }
        public MisActividadesYSubordinados ActividadesEntrantes { get; set; } //Vienen del dto
        public ObservableCollection<ActividadDTO> ActividadesFiltradas { get; set; } //van para la vista
        public DepartamentoDTO DepartamentoActual { get; set; } //Filtro
        public DepartamentoDTO DepartamentoUser { get; set; } //Este es del que inicio sesion
        public ActividadDTO ActividadActual { get; set; } //Actividad que seleccione
        public DepartamentoDTO departamentoActualDetalles { get; set; } //Depto que seleccione
        public DateTime FechaRealizacionActActual { get; set; }
        public Estado? EstadoActualAct { get; set; }
        public string NuevaEvidencia { get; set; } = "";
        public string NombreNuevoArchivo { get; set; } = "";
        public string Mensaje { get; set; } = "";
        public bool IsReadOnly { get; set; }
        ActividadService actividadService;
        DepartamentoService departamentoService;


        public ActividadesViewModel()
        {
            //Commands
            FiltrarCommand = new RelayCommand<int>(Filtrar);
            VerActividadCommand = new RelayCommand<ActividadDTO>(SetActividad);
            CancelarCommand = new RelayCommand(Cancelar);
            VerNuevoCommand = new RelayCommand(VerNuevo);
            GuardarNuevoCommand = new RelayCommand(GuardarNuevo);
            EditarActividadCommand = new RelayCommand(EditarCommand);
            EliminarActividadCommand = new RelayCommand(EliminarActividad);
            VerActividadesCommand = new RelayCommand(VerActividades);
            VerDepartamentosCommand = new RelayCommand(VerDepartamentos);
            VerDetallesDepartamentoCommand = new RelayCommand<DepartamentoDTO>(VerDetallesDepartamento);
            EditarDeptoCommand = new RelayCommand(EditarDepto);
            LogoutCommand = new RelayCommand(Logout);
            EliminarDeptoCommand = new RelayCommand(EliminarDepto);
            VerNuevoDeptoCommand = new RelayCommand(VerNuevoDepto);
            RegistraDeptoCommand = new RelayCommand(RegitsraDepto);
            //Objects and properties
            Deptos = new ObservableCollection<DepartamentoDTO>();
            Estados = new()
            {
                new Estado(){Value = 0 , Text = "Borrador"},
                new Estado(){Value = 1 , Text = "Publicado"}
                //new Estado(){Value = 2 , Text = "Borrado"}
            };

            actividadService = App.actividadService;
            departamentoService = App.departamentoService;
            DepartamentoActual = new DepartamentoDTO();
            DepartamentoUser = new();
            ActividadesFiltradas = new();
            ActividadesEntrantes = new();
            ActividadActual = new();
            EstadoActualAct = new();
            DeptosHijos = new();
            departamentoActualDetalles = new();
        }

        private async void RegitsraDepto()
        {
            try
            {
                Mensaje = "";
                if (string.IsNullOrWhiteSpace(departamentoActualDetalles.Nombre))
                    Mensaje += "Debe especificar el nombre del departamento";

                if (string.IsNullOrWhiteSpace(departamentoActualDetalles.Username))
                    Mensaje += "Debe especificar el usuario del departamento";

                if (string.IsNullOrWhiteSpace(departamentoActualDetalles.Password))
                    Mensaje += "Debe especificar la contraseña";

                if (!string.IsNullOrWhiteSpace(departamentoActualDetalles.Password))
                    departamentoActualDetalles.Password = CifradoHash.StringToSHA512(departamentoActualDetalles.Password);

                if (Mensaje == "")
                {
                    departamentoActualDetalles.IdSuperior = DepartamentoUser.Id;
                    var result = await departamentoService.Post(departamentoActualDetalles);
                    if (result == "")
                    {
                        CargarDeptos();
                        LoadData();
                        App.MainViewModel.CambiarVista(Vista.Dept);

                    }
                    else
                        Mensaje = result;
                }
                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }


        private void VerNuevoDepto()
        {
            try
            {
                departamentoActualDetalles = new();
                Mensaje = "";
                App.MainViewModel.CambiarVista(Vista.RegDept);
                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }

        async void CargarDeptos()
        {
            Deptos = new((await departamentoService.GetDeptos(DepartamentoUser.Id)));
            DeptosHijos.Clear();

            foreach (var d in Deptos)
            {
                if (d.Id != DepartamentoUser.Id)
                    DeptosHijos.Add(d);
            }
        }

        private async void EliminarDepto()
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("¿Desea eliminar este departamento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.No)
                    return;

                Mensaje = "";
                if (departamentoActualDetalles != null)
                {
                    var result = await departamentoService.Delete(departamentoActualDetalles.Id);
                    if (result == "")
                    {
                        CargarDeptos();
                        LoadData();
                        App.MainViewModel.CambiarVista(Vista.Dept);
                    }
                    else
                        Mensaje = result;
                }
                else
                    Mensaje = "Debes elegir un departamento";
                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }

        private async void EditarDepto()
        {
            try
            {
                Mensaje = "";
                if (string.IsNullOrWhiteSpace(departamentoActualDetalles.Nombre))
                    Mensaje += "Debe especificar el nombre del departamento";

                if (string.IsNullOrWhiteSpace(departamentoActualDetalles.Username))
                    Mensaje += "Debe especificar el usuario del departamento";

                if (!string.IsNullOrWhiteSpace(departamentoActualDetalles.Password))
                    departamentoActualDetalles.Password = CifradoHash.StringToSHA512(departamentoActualDetalles.Password);

                if (Mensaje == "")
                {
                    var result = await departamentoService.Put(departamentoActualDetalles);
                    if (result == "")
                    {
                        CargarDeptos();
                        LoadData();
                        App.MainViewModel.CambiarVista(Vista.Dept);

                    }
                }
                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }

        private void VerDetallesDepartamento(DepartamentoDTO departamentoEntrante)
        {
            Mensaje = "";
            departamentoActualDetalles = new()
            {
                Id = departamentoEntrante.Id,
                IdSuperior = departamentoEntrante.IdSuperior,
                Nombre = departamentoEntrante.Nombre,
                Username = departamentoEntrante.Username,
                Password = ""
            };

            App.MainViewModel.CambiarVista(Vista.DetDept);
            Actualizar();
        }

        private void Logout()
        {
            try
            {
                DefineDepto(new DepartamentoDTO());
                Deptos.Clear();
                DepartamentoActual = new();
                App.MainViewModel.CambiarVista(Vista.Login);
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }

        private void VerDepartamentos()
        {
            App.MainViewModel.CambiarVista(Vista.Dept);
        }

        private void VerActividades()
        {
            App.MainViewModel.CambiarVista(Vista.Act);
        }

        private async void EliminarActividad()
        {
            try
            {

                MessageBoxResult result = MessageBox.Show("¿Desea eliminar esta actividad?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;

                if (ActividadActual != null && ActividadActual.Id > 0)
                {
                    Mensaje = await actividadService.Delete(ActividadActual.Id);
                    if (Mensaje == "")
                    {
                        ClearProperties();
                        LoadData();
                        App.MainViewModel.CambiarVista(Vista.Act);
                    }
                }
                else
                    Mensaje = "Debe escoger una actividad a eliminar";
                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
                Actualizar();
            }
        }

        private async void EditarCommand()
        {
            try
            {
                Mensaje = "";
                //Validar
                if (string.IsNullOrWhiteSpace(ActividadActual.Titulo))
                    Mensaje = "El titulo de la actividad no debe ir vacío";

                if (string.IsNullOrWhiteSpace(ActividadActual.Descripcion))
                    Mensaje = "La descripcion no debe ir vacia";

                if (FechaRealizacionActActual > DateTime.Now.ToMexicoTime())
                    Mensaje = "La fecha de realizacion no puede ser en el futuro";

                if (DepartamentoActual == null)
                    Mensaje = "Debe especificar un departamento";

                if (EstadoActualAct == null || EstadoActualAct.Text == "")
                    Mensaje = "Debe espefificar el estado de la actividad";
                if (Mensaje == "")
                {

                    ActividadActual.Estado = EstadoActualAct.Value;
                    ActividadActual.FechaRealizacion = DateOnly.FromDateTime(FechaRealizacionActActual);
                    ActividadActual.IdDepartamento = DepartamentoUser.Id; ///Soolo puedes enviar los tuyos
                    if (NuevaEvidencia != "")
                        ActividadActual.Evidencia = NuevaEvidencia;
                    else
                        ActividadActual.Evidencia = "";
                    Mensaje = await actividadService.Put(ActividadActual);
                    if (Mensaje == "")
                    {
                        ClearProperties();
                        LoadData();

                        App.MainViewModel.CambiarVista(Vista.Act);
                    }
                }

                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
                Actualizar();
            }
        }

        private async void GuardarNuevo()
        {
            try
            {
                Mensaje = "";
                //Validar
                if (string.IsNullOrWhiteSpace(ActividadActual.Titulo))
                    Mensaje = "El titulo de la actividad no debe ir vacío";

                if (string.IsNullOrWhiteSpace(ActividadActual.Descripcion))
                    Mensaje = "La descripcion no debe ir vacia";

                if (FechaRealizacionActActual > DateTime.Now.ToMexicoTime())
                    Mensaje = "La fecha de realizacion no puede ser en el futuro";

                if (DepartamentoActual == null)
                    Mensaje = "Debe especificar un departamento";

                if (EstadoActualAct == null || EstadoActualAct.Text == "")
                    Mensaje = "Debe espefificar el estado de la actividad";
                if (string.IsNullOrWhiteSpace(NuevaEvidencia))
                    Mensaje = "Debe contener evidencia";
                if (Mensaje == "")
                {

                    ActividadActual.Estado = EstadoActualAct.Value;
                    ActividadActual.FechaRealizacion = DateOnly.FromDateTime(FechaRealizacionActActual);
                    ActividadActual.IdDepartamento = DepartamentoUser.Id; ///Soolo puedes enviar los tuyos
                    ActividadActual.Evidencia = NuevaEvidencia;
                    Mensaje = await actividadService.Post(ActividadActual);
                    if (Mensaje == "")
                    {
                        ClearProperties();
                        LoadData();

                        App.MainViewModel.CambiarVista(Vista.Act);
                    }
                }

                Actualizar();
            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
                Actualizar();
            }
        }

        private void ClearProperties()
        {
            ActividadActual = new();
            NuevaEvidencia = "";
            NombreNuevoArchivo = "";
        }

        private void VerNuevo()
        {
            try
            {
                if (DepartamentoActual != null)
                {
                    ActividadActual = new();
                    NuevaEvidencia = "";
                    NombreNuevoArchivo = "";
                    FechaRealizacionActActual = DateTime.Now.AddDays(-1);
                    App.MainViewModel.CambiarVista(Vista.RegAct);
                    Actualizar();
                }

                else
                {
                    App.MostrarError("Debe escoger un departamento");
                }

            }
            catch (Exception ex)
            {
                App.MostrarError(ex.Message);
            }
        }

        private void Cancelar()
        {
            App.MainViewModel.CambiarVista(Vista.Act);
        }

        private void SetActividad(ActividadDTO dTO)
        {

            ClearProperties();
            IsReadOnly = true;
            if (dTO != null)
            {
                if (dTO.IdDepartamento == DepartamentoUser.Id)
                    IsReadOnly = false;
                else
                    IsReadOnly = true;

                ActividadActual = new()
                {
                    Id = dTO.Id,
                    Descripcion = dTO.Descripcion,
                    IdDepartamento = dTO.IdDepartamento,
                    Estado = dTO.Estado,
                    Evidencia = dTO.Evidencia,
                    FechaActualizacion = dTO.FechaActualizacion,
                    FechaCreacion = dTO.FechaCreacion,
                    FechaRealizacion = dTO.FechaRealizacion,
                    Titulo = dTO.Titulo
                };

                FechaRealizacionActActual = dTO.FechaRealizacion.Value.ToDateTime(TimeOnly.MinValue);
                EstadoActualAct = Estados.FirstOrDefault(x => x.Value == dTO.Estado);
                NuevaEvidencia = "";
                NombreNuevoArchivo = "";
                App.MainViewModel.CambiarVista(Vista.DetAct);
                Actualizar();
            }
        }

        private void Filtrar(int idDep)
        {
            if (ActividadesEntrantes != null && ActividadesEntrantes.ActividadesSubordinadas.Count > 0)
            {
                var actSeleccionada = ActividadesEntrantes.ActividadesSubordinadas.FirstOrDefault(x => x.IdDepartamento == idDep);
                if (actSeleccionada != null)
                {
                    ActividadesFiltradas.Clear();
                    foreach (var a in actSeleccionada.Actividades)
                    {
                        ActividadesFiltradas.Add(a);
                    }
                }
            }
        }

        public void DefineDepto(DepartamentoDTO departamentoEntrante)
        {
            DepartamentoUser = departamentoEntrante;
        }

        public async void LoadData()
        {
            if (Deptos == null || Deptos.Count == 0)
            {
                //Deptos = new((await departamentoService.GetDeptos(DepartamentoUser.Id)));
                //DeptosHijos.Clear();

                //foreach (var d in Deptos)
                //{
                //    if (d.Id != DepartamentoUser.Id)
                //        DeptosHijos.Add(d);
                //}
                CargarDeptos();
            }

            ActividadesEntrantes = await actividadService.GetActividadesYSubordinadas(DepartamentoUser.Id);

            if (DepartamentoActual == null)
                Filtrar(DepartamentoUser.Id);
            else
                Filtrar(DepartamentoActual.Id);

            Actualizar();
        }

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
