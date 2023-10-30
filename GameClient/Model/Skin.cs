using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Model
{
    public class Skin
    {
        public ImageSource X { get; set; }
        public ImageSource O { get; set; }

        public Skin(DirectoryInfo directory)
        {
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name.Equals("skin_x.png"))
                {
                    X = ImageSource.FromFile(file.FullName);
                }
                else if (file.Name.Equals("skin_o.png"))
                {
                    O = ImageSource.FromFile(file.FullName); 
                }
            }
        }
        public Skin() { }
    }
}
