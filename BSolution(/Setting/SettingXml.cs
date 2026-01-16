using BSolution_.Grab;
using Common.Util.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolution_.Setting
{
    public class SettingXml
    {
        private const string SETTING_DIR = "Setup";
        private const string SETTING_FILE_NAME = @"Setup\Setting.xml";

        private static SettingXml _setting;

        public static SettingXml Inst
        {
            get
            {
                if (_setting == null)
                    Load();

                return _setting;
            }
        }


        public static void Load()
        {
            if (_setting != null)
                return;

            string settingFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, SETTING_FILE_NAME);
            if (File.Exists(settingFilePath) == true)
            {
                _setting = XmlHelper.LoadXml<SettingXml>(settingFilePath);
            }

            if (_setting is null)
            {
                _setting = CreateDefaultInstance();
            }
        }

        public static void Save()
        {
            string settingFilePath = Path.Combine(Environment.CurrentDirectory, SETTING_FILE_NAME);
            if (!File.Exists(settingFilePath))
            {
                string setupDir = Path.Combine(Environment.CurrentDirectory, SETTING_DIR);

                if (!Directory.Exists(setupDir))
                    Directory.CreateDirectory(setupDir);

                FileStream fs = File.Create(settingFilePath);
                fs.Close();
            }

            XmlHelper.SaveXml(settingFilePath, Inst);
        }
        private static SettingXml CreateDefaultInstance()
        {
            SettingXml setting = new SettingXml();
            setting.ModelDir = @"d:\Models";
            return setting;
        }

        public SettingXml() { }

        public string MachineName { get; set; } = "BSolution";

        public string ModelDir { get; set; } = "";

        public string ImageDir { get; set; } = "";

        public CameraType CamType { get; set; } = CameraType.WebCam;

        public bool CycleMode { get; set; } = false;
    }
}
