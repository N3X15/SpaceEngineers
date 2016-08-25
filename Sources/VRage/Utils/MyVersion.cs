using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRage.Utils
{
    public class MyVersion
    {
        public readonly int Version;
        public readonly int SECEBuild;
        public readonly StringBuilder FormattedText;

        public MyVersion(int version)
        {
            Version = version;
            FormattedText = new StringBuilder(MyBuildNumbers.ConvertBuildNumberFromIntToString(version, SECEBuild));
        }

        public MyVersion(int version, int ceBuild)
        {
            Version = version;
            SECEBuild = ceBuild;
            FormattedText = new StringBuilder(MyBuildNumbers.ConvertBuildNumberFromIntToString(version, SECEBuild));
        }
        /*
        public static implicit operator MyVersion(int version)
        {
            return new MyVersion(version);
        }
        */

        public static implicit operator int(MyVersion version)
        {
            return version.Version;
        }

        public override string ToString()
        {
            // Other code relies on this!
            return Version.ToString();
        }
    }
}
