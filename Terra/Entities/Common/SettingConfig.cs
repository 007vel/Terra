using System;
namespace Entities.Common
{
    public class SettingConfig
    {
        public string id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string ButtonName { get; set; }
        public string Desc { get; set; }
        public bool IsEnabled { get; set; }

        public override bool Equals(object obj)
        {
            if(obj!=null)
            {
                var _obj = (SettingConfig)obj;
                if(_obj!=null)
                {
                    return _obj.id == this.id;
                }
            }
            return false;
        }
    }
}
