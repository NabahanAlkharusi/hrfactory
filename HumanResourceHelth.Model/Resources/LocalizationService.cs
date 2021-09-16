//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;
//using Microsoft.Extensions.Globalization;

//namespace HumanResourceHelth.Model.Resources
//{
//    public class LocalizationService
//    {
//        private readonly str _localizer;

//        public LocalizationService(IStringLocalizerFactory factory)
//        {
//            var type = typeof(AppResource);
//            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
//            _localizer = factory.Create("AppResource", assemblyName.Name);
//        }

//        public LocalizedString GetLocalizedHtmlString(string key)
//        {
//            return _localizer[key];
//        }
//    }
//}
