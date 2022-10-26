using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;

namespace GeoSISGAQSACaddin
{
    public static class settings
    {
        //1. Metadata
        public static string __title__ = "Geo-SISGAQSAC";
        public static string __author__ = "geocodery";
        public static string __copyright__ = "CAEG";
        public static string __credits__ = "Daniel Aguado H., Jorge Yupanqui H.";
        public static string __version__ = "1.0.1";
        public static string __maintainer__ = __credits__;
        public static string __mail__ = "CAEG";
        public static string __status__ = "Development";
        public static string __tempdir__ = System.IO.Path.GetTempPath();

        //2. Variables globales estáticas
        //Se usa arroba para determinar el escape especial como string
        public static string _path = __file__();
        public static string _scripts_path = _path + @"\scripts";
        public static string _layer_path = _scripts_path + @"\layers";
        public static string _image_path = _scripts_path + @"\img";
        public static string _path_sqlite = _scripts_path + @"\atencionpublico.db";
        public static string _path_loader = _image_path + @"\loader.gif";

        public static string _grillas_path = _image_path + @"\grilla_cartas.gif";

        //3.Variables dinámicas alterables según fin
        // EStas variables solo podrán ser alteradas manejándolas dentro del contexto que fueron creados
        public static string user;
        public static string pass;
        public static int currentModule;
        public static Dictionary<int, string> modulosDict = new Dictionary<int, string>();
        public static int controller_sesion = 0;
        public static string python_path;
        public static int radio_consulta = 1;
        public static double puntoConsulta_x;
        public static double puntoConsulta_y;


        //4. Variables globales dinámicas
        // su valor puede alterarse en todos los procesos
        public static int d_contador = 0;
        public static string d_standar_output;
        public static ProgressBar _LOADER_CONTROL;
        public static string drawLine_wkt;

        public static double xmin;
        public static double ymin;
        public static double xmax;
        public static double ymax;

        //5. Nombre de formatos Gis
        public const string f_shapefile = "shapefile";
        public const string f_featureclass = "featureclass";
        public const string f_geodatabase = "geodatabase";
        public const string f_raster = "raster";
        public const string f_workspace = "workspace";
        public const string f_table = "table";
        public const string f_file = "file";
        public const string f_mxd = "mxd";

        static string message_runtime_error = "¡Ocurrió un error inesperado!" + Environment.NewLine + "Por favor, contacte al administrador del sistema.";


        //6. Funciones globales
        //   - Funciones que devuelven resultados y que puedes ser usados en cualquier parte del proceso

        //* __file__: Obtiene la ruta actual en donde se almacena la instalacion del addin
        //* parametros: No recibe parametros

        public static string __file__()
        {
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uriBuilder = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uriBuilder.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

        public static void openFormByName(Form myForm, Control container)
        {
            bool existForm = container.Controls.Contains(myForm);
            if (existForm)
                return;
            else
                container.Controls.Clear();

            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            myForm.Size = container.Size;
            container.Controls.Add(myForm);
            myForm.Show();
            return;
        }

        public static void runProgressBar(string position = null)
        {
            if (_LOADER_CONTROL == null)
                return;
            if ((position == "ini"))
                _LOADER_CONTROL.Value = 0;
            else if ((position == "end"))
                _LOADER_CONTROL.Value = 100;
            else if ((position == null))
            {
                var number = new Random();
                _LOADER_CONTROL.Value = number.Next(30, 85);
            }
            else
            {
                int numero = 0;
                numero = int.Parse(position);
                _LOADER_CONTROL.Value = numero;
            }
        }

        static IGxObjectFilter GetFilter(string filetype)
        {
            IGxObjectFilter objfilter = null;
            switch (filetype)
            {
                case f_shapefile:
                    objfilter = new GxFilterShapefiles();
                    break;
                case f_featureclass:
                    objfilter = new GxFilterFeatureClasses();
                    break;
                case f_geodatabase:
                    objfilter = new GxFilterFileGeodatabases();
                    break;
                case f_raster:
                    objfilter = new GxFilterRasterDatasets();
                    break;
                case f_workspace:
                    objfilter = new GxFilterWorkspaces();
                    break;
                case f_table:
                    objfilter = new GxFilterTables();
                    break;
                case f_file:
                    objfilter = new GxFilterFiles();
                    break;
                case f_mxd:
                    objfilter = new GxFilterMaps();
                    break;

            }
            return objfilter;
        }


        static object openDialogBoxESRI(string filetype, string textButton = "Agregar")
        {
            IEnumGxObject pEnumGX = null/* TODO Change to default(_) if this is not a reference type */;
            IGxDialog pGxDialog = new GxDialogClass();
            pGxDialog.AllowMultiSelect = false;
            pGxDialog.Title = "Seleccionar";
            if (filetype != null)
                pGxDialog.Title = string.Format("Seleccionar {0}", filetype);
            pGxDialog.ObjectFilter = GetFilter(filetype);
            pGxDialog.ButtonCaption = textButton;

            if (!pGxDialog.DoModalOpen(0, out pEnumGX))
                return null;

            IGxObject objGxObject = pEnumGX.Next();
            return objGxObject.FullName;
        }

    }
}