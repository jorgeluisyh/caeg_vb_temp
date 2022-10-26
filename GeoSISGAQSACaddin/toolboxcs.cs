using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.GeoprocessingUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GeoSISGAQSACaddin.settings;

namespace GeoSISGAQSACaddin
{
    public static class toolbox
    {
        //1. Variables globales
        //* _toolboxPath: Construye la ruta donde se encuentra el archivo *.tbx
        private static List<object> parameters = new List<object>();
        public static GeoSISGAQSAExceptions RuntimeError = new GeoSISGAQSAExceptions();
        public static string _toolboxPath_general = _path + @"\scripts\T00_generales.tbx";
        public static string _toolboxPath_PantallaGigante = _path + @"\scripts\T01_pantallaGigante.tbx";

        //:HERRAMIENTAS DEL MODULO GENERAL
        public static string _tool_getCartas = "getCartas";
        public static string _tool_getDistritos = "getDistritos";
        public static string _tool_clearToc = "clearToc";
        public static string _tool_getDmValues = "getDmValues";

        //:HERRAMIENTAS DEL MODULO PANTALLA GIANTE
        public static string _tool_getCartasByParam = "getCartasByParam";
        public static string _tool_graficarCapas = "graficarCapas";


        // 5. Funciones globales de toolbox
        // - Funciones que devuelven resultados y que puedes ser usados en cualquier parte del proceso

        // * GPToolDialog: Inicia un cuadro de dialogo que trae un scriptool en pantalla
        // * parametros:
        // - tool: Nombre de la herramienta
        // - modal: True: "Si requiere que la ventana invocar bloquea la ventana principal de arcamap" 
        // False: "Si no desea bloquer la ventana principal"; por defecto es False
        // - tbxpath: Ruta de un nuevo tbx; por defecto el valor apunta a la variable _toolboxPath
        public static void GPToolDialog(string tool, bool modal = false, string tbxpath = null)
        {
            try
            {
                // Si no se especifico la ruta del tbx
                if (tbxpath == null)
                    tbxpath = "";

                IGPToolCommandHelper2 pToolHelper = new GPToolCommandHelper() as IGPToolCommandHelper2;
                pToolHelper.SetToolByName(tbxpath, tool);

                if (modal)
                {
                    // Realiza la invocacion del ScriptTool bloqueando la funcionalidad de ArcMap
                    IGPMessages msgs = new GPMessages() as IGPMessages;
                    bool pok = true;
                    pToolHelper.InvokeModal(0, null/* TODO Change to default(_) if this is not a reference type */, out pok, out msgs);
                }
                else
                    // Realiza la invocacion del ScriptTool sin bloquear la funcionalidad de ArcMap
                    pToolHelper.Invoke(null/* TODO Change to default(_) if this is not a reference type */);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void CleanTableOfContents(string capaname = null)
        {
            parameters.Clear();
            if (capaname != null)
                parameters.Add(capaname);
            var response = ExecuteGP(_tool_clearToc, parameters, _toolboxPath_general);
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (int.Parse(responseJson["status"].ToString()) == 0)
            {
                RuntimeError.PythonError = responseJson["message"].ToString();
                MessageBox.Show(RuntimeError.PythonError, __title__, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            return;
        }

        //* ExecuteGP: Ejecuta una herramienta personalizada
        //* parametros:
        //   - tool: Nombre de la herramienta
        //   - parameters: True: Lista de parametros que deben ser pasados a la herramienta
        //   - tbxpath: Ruta de un nuevo tbx; por defecto el valor apunta a la variable _toolboxPath
        public static string ExecuteGP(string tool, List<object> parameters, string tbxpath = null, bool getresult = true)
        {
            try
            {
                // Si no se especifico la ruta del tbx
                if (tbxpath == null)
                    tbxpath = "";

                IGeoProcessorResult response_object = null/* TODO Change to default(_) if this is not a reference type */;

                // Dim gpEventHandler As GPMessageEventHandler = New GPMessageEventHandler()

                GeoProcessor GP = new GeoProcessor();
                GP.LogHistory = false;

                // Se registra el geoprocesor para capturar sus mensajes
                // GP.RegisterGeoProcessorEvents(gpEventHandler)

                // Agregar el evento que capturara el mensaje
                // AddHandler gpEventHandler.GPMessage, AddressOf OnGPMessage

                // Agrega la ruta el tbx
                GP.AddToolbox(tbxpath);

                // Se crea el contedor de parametros
                IVariantArray parametros = new VarArrayClass();

                // Se agregan todos los parametros
                foreach (var i in parameters)
                    parametros.Add(i);
                var response = "";
                if (getresult)
                {
                    response_object = (IGeoProcessorResult)GP.Execute(tool, parametros, null/* TODO Change to default(_) if this is not a reference type */);
                    response = response_object.ReturnValue.ToString();
                }
                else
                {
                    GP.AddOutputsToMap = true;
                    GP.Execute(tool, parametros, null/* TODO Change to default(_) if this is not a reference type */);
                    response = "Success";
                }
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }


}