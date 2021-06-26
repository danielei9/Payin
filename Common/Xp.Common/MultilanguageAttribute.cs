using System;

namespace Xp.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MultilanguageAttribute : Attribute
    {
        public string Model { get; set; }
        public string Arguments { get; set; }

        #region Constructors
        public MultilanguageAttribute(string model, string arguments)
        {
            Model = model;
            Arguments = arguments;
        }
        #endregion Constructors
    }
}
