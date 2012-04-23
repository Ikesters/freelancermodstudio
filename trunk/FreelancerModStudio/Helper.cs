using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FreelancerModStudio
{
    class Helper
    {
        public struct Program
        {
            public static void Start()
            {
#if DEBUG
                //DevTest.CreateTemplate(@"E:\DAT\DWN\fl\jflp\DATA"); return;
                System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                st.Start();
#endif
                //load settings
                Settings.Load();
                Template.Load();
#if DEBUG
                st.Stop();
                System.Diagnostics.Debug.WriteLine("loading settings.xml and template.xml: " + st.ElapsedMilliseconds + "ms");
#endif

                //whidbey color table (gray colors of menustrip and tabstrip)
                var whidbeyColorTable = new ProfessionalColorTable { UseSystemColors = true };
                ToolStripManager.Renderer = new ToolStripProfessionalRenderer(whidbeyColorTable);

                //install downloaded update
                if (Settings.Data.Data.General.AutoUpdate.Update.Downloaded)
                {
                    if (AutoUpdate.AutoUpdate.InstallUpdate())
                        return;
                }

                //remove installed update
                if (Settings.Data.Data.General.AutoUpdate.Update.Installed)
                    AutoUpdate.AutoUpdate.RemoveUpdate();

                //check for update
                if (Settings.Data.Data.General.AutoUpdate.Enabled && Settings.Data.Data.General.AutoUpdate.UpdateFile != null && Settings.Data.Data.General.AutoUpdate.LastCheck.Date.AddDays(Settings.Data.Data.General.AutoUpdate.CheckInterval) <= DateTime.Now.Date)
                    Update.BackgroundCheck();

                //start main form
                Application.Run(new frmMain());

                //save settings
                Settings.Save();
            }
        }

        public struct Update
        {
            public static AutoUpdate.AutoUpdate AutoUpdate = new AutoUpdate.AutoUpdate();

            public static void BackgroundCheck()
            {
                //download in thread with lowest performance
                System.Threading.Thread autoUpdateThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    Check(true, Settings.Data.Data.General.AutoUpdate.SilentDownload);
                }));
                autoUpdateThread.Priority = System.Threading.ThreadPriority.Lowest;
                autoUpdateThread.IsBackground = true;
                autoUpdateThread.Start();
            }

            public static void Check(bool silentCheck, bool silentDownload)
            {
                Uri checkFileUri;
                if (!Uri.TryCreate(Settings.Data.Data.General.AutoUpdate.UpdateFile, UriKind.Absolute, out checkFileUri))
                {
                    if (!silentCheck)
                        MessageBox.Show(String.Format(Properties.Strings.UpdatesDownloadException, Assembly.Title), Assembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Settings.Data.Data.General.AutoUpdate.LastCheck = DateTime.Now;
                    return;
                }

                AutoUpdate.CheckFileUri = checkFileUri;

                string proxy = string.Empty;
                string username = string.Empty;
                string password = string.Empty;

                if (Settings.Data.Data.General.AutoUpdate.Proxy.Enabled)
                {
                    proxy = Settings.Data.Data.General.AutoUpdate.Proxy.Uri;
                    username = Settings.Data.Data.General.AutoUpdate.Proxy.Username;
                    password = Settings.Data.Data.General.AutoUpdate.Proxy.Password;
                }

                AutoUpdate.SilentCheck = silentCheck;
                AutoUpdate.SilentDownload = silentDownload;
                AutoUpdate.SetProxy(proxy);
                AutoUpdate.SetCredentials(username, password);

                AutoUpdate.Check();
            }
        }

        public struct Template
        {
            static FreelancerModStudio.Data.Template data;

            public static void Load()
            {
                Load(System.IO.Path.Combine(Application.StartupPath, Properties.Resources.TemplatePath));
            }

            public static void Load(string file)
            {
                data = new FreelancerModStudio.Data.Template();

                try
                {
                    data.Load(file);
                    Data.SetSpecialFiles();
                }
                catch (Exception ex)
                {
                    Exceptions.Show(String.Format(Properties.Strings.TemplateLoadException, Properties.Resources.TemplatePath), ex);
                    Environment.Exit(0);
                }
            }

            public struct Data
            {
                public static int SystemFile { get; set; }
                public static int UniverseFile { get; set; }
                public static int SolarArchetypeFile { get; set; }

                public static FreelancerModStudio.Data.Template.Files Files
                {
                    get { return data.Data.Files; }
                    set { data.Data.Files = value; }
                }

                //public static FreelancerModStudio.Data.Template.CostumTypes CostumTypes
                //{
                //    get { return data.Data.CostumTypes; }
                //    set { data.Data.CostumTypes = value; }
                //}

                public static void SetSpecialFiles()
                {
                    int count = 3;

                    for (int i = 0; i < Files.Count && count > 0; i++)
                    {
                        switch (Files[i].Name.ToLower())
                        {
                            case "system":
                                SystemFile = i;
                                count--;
                                break;
                            case "universe":
                                UniverseFile = i;
                                count--;
                                break;
                            case "solar archetype":
                                SolarArchetypeFile = i;
                                count--;
                                break;
                        }
                    }
                }
            }
        }

        public struct Settings
        {
            public static Data.Settings Data;

            public static void Save()
            {
                string file = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName), Properties.Resources.SettingsPath);
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(file)))
                        Directory.CreateDirectory(Path.GetDirectoryName(file));

                    Data.Save(file);
                }
                catch (Exception ex)
                {
                    Exceptions.Show(String.Format(Properties.Strings.SettingsSaveException, Properties.Resources.SettingsPath), ex);
                }
            }

            public static void Load()
            {
                string file = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName), Properties.Resources.SettingsPath);
                Data = new Data.Settings();

                if (System.IO.File.Exists(file))
                {
                    try
                    {
                        Data.Load(file);
                    }
                    catch (Exception ex)
                    {
                        Exceptions.Show(String.Format(Properties.Strings.SettingsLoadException, Properties.Resources.SettingsPath), ex);
                    }
                }
            }

            public static string GetShortLanguage()
            {
                if (Data.Data.General.Language == FreelancerModStudio.Data.LanguageType.German)
                    return "de";

                return "en";
            }

            public static void SetShortLanguage(string language)
            {
                if (language.ToLower() == "de")
                    Data.Data.General.Language = FreelancerModStudio.Data.LanguageType.German;
                else
                    Data.Data.General.Language = FreelancerModStudio.Data.LanguageType.English;
            }
        }

        public struct Thread
        {
            public static void Start(ref System.Threading.Thread thread, System.Threading.ThreadStart threadDelegate, System.Threading.ThreadPriority priority, bool isBackground)
            {
                Abort(ref thread, true);

                thread = new System.Threading.Thread(threadDelegate) { Priority = priority, IsBackground = isBackground };
                thread.Start();
            }

            public static void Abort(ref System.Threading.Thread thread, bool wait)
            {
                if (IsRunning(ref thread))
                {
                    thread.Abort();

                    if (wait)
                        thread.Join();
                }
            }

            public static bool IsRunning(ref System.Threading.Thread thread)
            {
                return (thread != null && thread.IsAlive);
            }
        }

        public struct Compare
        {
            public static bool Size(Point checkSize, Point currentSize, bool bigger)
            {
                return Size(new Size(checkSize.X, checkSize.Y), new Size(currentSize.X, currentSize.Y), bigger);
            }

            public static bool Size(Size checkSize, Size currentSize, bool bigger)
            {
                if (bigger)
                    return (checkSize.Width >= currentSize.Width && checkSize.Height >= currentSize.Height);

                return (checkSize.Width <= currentSize.Width && checkSize.Height <= currentSize.Height);
            }
        }

        public struct Exceptions
        {
            public static void Show(Exception exception)
            {
                MessageBox.Show(Get(exception), Assembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            public static void Show(string errorDescription, Exception exception)
            {
                MessageBox.Show(errorDescription + Environment.NewLine + Environment.NewLine + Get(exception), Assembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            public static string Get(Exception exception)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(exception.Message);

                if (exception.InnerException != null)
                    stringBuilder.Append(Environment.NewLine + Environment.NewLine + Get(exception.InnerException));

                return stringBuilder.ToString();
            }
        }

        public struct Assembly
        {
            public static string Title
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        System.Reflection.AssemblyTitleAttribute titleAttribute = (System.Reflection.AssemblyTitleAttribute)attributes[0];

                        if (titleAttribute.Title != string.Empty)
                            return titleAttribute.Title;
                    }

                    return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                }
            }

            public static Version Version
            {
                get
                {
                    return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                }
            }

            public static string Description
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyDescriptionAttribute), false);
                    if (attributes.Length == 0)
                        return string.Empty;

                    return ((System.Reflection.AssemblyDescriptionAttribute)attributes[0]).Description;
                }
            }

            public static string Product
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false);
                    if (attributes.Length == 0)
                        return string.Empty;

                    return ((System.Reflection.AssemblyProductAttribute)attributes[0]).Product;
                }
            }

            public static string Copyright
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                    if (attributes.Length == 0)
                        return string.Empty;

                    return ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
                }
            }

            public static string Company
            {
                get
                {
                    object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), false);
                    if (attributes.Length == 0)
                        return string.Empty;

                    return ((System.Reflection.AssemblyCompanyAttribute)attributes[0]).Company;
                }
            }
        }
    }
}