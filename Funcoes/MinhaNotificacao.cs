using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace nfecreator
{
    class MinhaNotificacao
    {

        private static readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Notificar(string notificar)
        {

            //Icon icone = Icon.ExtractAssociatedIcon(_path + @"/Pics/ICONE.ICO");

            NotifyIcon notifyIcon1 = new NotifyIcon
            {
                // notifyIcon1.Icon = this.Icon;
                //Icon = icone,
                BalloonTipTitle = notificar,
                BalloonTipText = "Você fez algo de errado!",
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Warning
            };
            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.Click += new System.EventHandler(NotifyIcon1_Click);

            System.Threading.Thread.Sleep(5000);
            notifyIcon1.Visible = false;
        }

        private static void NotifyIcon1_Click(object sender, System.EventArgs e)
        {
            System.Drawing.Size windowSize =
                SystemInformation.PrimaryMonitorMaximizedWindowSize;
            System.Drawing.Point menuPoint =
                new System.Drawing.Point(windowSize.Width - 180,
                windowSize.Height - 5);

            // notifyIcon1.ContextMenu.Show(this, menuPoint);

        }

        public static void NotificarErro(string title, string notificar)
        {

            //Icon icone = Icon.ExtractAssociatedIcon(_path + @"/Pics/ICONE.ICO");

            NotifyIcon notifyIcon1 = new NotifyIcon
            {
                // notifyIcon1.Icon = this.Icon;
                //Icon = icone,
                BalloonTipTitle = notificar,
                BalloonTipText = title,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Error
            };
            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.Click += new System.EventHandler(NotifyIcon1_Click);
            System.Threading.Thread.Sleep(5000);
            notifyIcon1.Visible = false;
        }
        public static void NotificarEInfo(string title, string text)
        {
            // Usuarios usuario;
            //Icon icone = Icon.ExtractAssociatedIcon(_path + @"/Pics/ICONE.ICO");

            NotifyIcon notifyIcon1 = new NotifyIcon
            {
                // notifyIcon1.Icon = this.Icon;
                //Icon = icone,
                BalloonTipTitle = title,
                BalloonTipText = text,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info
            };
            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.Click += new System.EventHandler(NotifyIcon1_Click);
            System.Threading.Thread.Sleep(5000);
            notifyIcon1.Visible = false;
        }

        public static void NotificarAviso(string title, string text)
        {
            // Usuarios usuario;
            //Icon icone = Icon.ExtractAssociatedIcon(_path + @"/Pics/ICONE.ICO");

            NotifyIcon notifyIcon1 = new NotifyIcon
            {
                // notifyIcon1.Icon = this.Icon;
                // Icon = icone,
                BalloonTipTitle = title,
                BalloonTipText = text,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Warning
            };
            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.Click += new System.EventHandler(NotifyIcon1_Click);
            System.Threading.Thread.Sleep(5000);
            notifyIcon1.Visible = false;
        }

    }


}
