using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TaskTimer
{
    internal partial class AboutBox : Form
    {
        private readonly Assembly _executingAssembly = Assembly.GetExecutingAssembly();

        public AboutBox()
        {
            InitializeComponent();
            SetVersionData();
        }

        private void SetVersionData()
        {
            Text = string.Format("Über {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            var filetime = File.GetLastWriteTime(Application.ExecutablePath);
            labelVersion.Text =
                string.Format("Version {0} Build {1}.{2}.{3}.{4}.{5}",
                              Application.ProductVersion,
                              filetime.Year,
                              filetime.Month.ToString().PadLeft(2, '0'),
                              filetime.Day.ToString().PadLeft(2, '0'),
                              filetime.Hour.ToString().PadLeft(2, '0'),
                              filetime.Minute.ToString().PadLeft(2, '0'));
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = File.ReadAllText("Readme.txt");
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                var attributes = _executingAssembly.GetCustomAttributes(typeof (AssemblyTitleAttribute),
                                                                        false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(_executingAssembly.CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get { return _executingAssembly.GetName().Version.ToString(); }
        }

        public string AssemblyDescription
        {
            get
            {
                var attributes = _executingAssembly.GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attributes = _executingAssembly.GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                var attributes = _executingAssembly.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                var attributes = _executingAssembly.GetCustomAttributes(typeof (AssemblyCompanyAttribute),
                                                                        false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        #endregion
    }
}