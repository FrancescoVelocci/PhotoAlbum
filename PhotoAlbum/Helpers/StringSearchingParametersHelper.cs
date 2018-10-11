using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace PhotoAlbum.Helpers
{
    public class StringSearchingParametersHelper
    {
        public async Task<Func<ViewPictureHelper, bool>> StringSearchingParamters(List<string> listSearchingParameters)
        {
            string stringSearchingParamters = "p =>" + string.Join("&&", listSearchingParameters);

            var options = ScriptOptions.Default.AddReferences(typeof(ViewPictureHelper).Assembly);

            Func<ViewPictureHelper, bool> stringSearchingParamtersExpression = await
                CSharpScript.EvaluateAsync<Func<ViewPictureHelper, bool>>(stringSearchingParamters, options);

            return stringSearchingParamtersExpression;
        }
    }
}
