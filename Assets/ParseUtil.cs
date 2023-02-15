namespace DefaultNamespace
{
    public class ParseUtil
    {
        public static int Parse(string txt)
        {
            var ok = int.TryParse(txt, out int ret);
            if (ok)
            {
                return ret;
            }

            return 0;
        }
    }
}