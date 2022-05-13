using System.Collections;
using System.Collections.Generic;

namespace WEditor.Utils
{
    public static class StringUtils
    {
        public static int GetIndexFromAssetName(this string assetName)
        {
            return int.Parse(assetName.Split('_')[1].ToString());
        }
    }
}
