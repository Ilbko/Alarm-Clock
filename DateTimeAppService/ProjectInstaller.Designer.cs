
using System;

namespace DateTimeAppService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "DateTimeAppService";
            this.serviceInstaller1.Description = "Служба для приложения-часов";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

            this.AfterInstall += ProjectInstaller_AfterInstall;
            this.AfterRollback += ProjectInstaller_AfterRollback;
            this.AfterUninstall += ProjectInstaller_AfterUninstall;
        }

        private void ProjectInstaller_AfterUninstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Служба деинсталлирована.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void ProjectInstaller_AfterRollback(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ошибка при установке службы.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void ProjectInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Служба установлена.");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}