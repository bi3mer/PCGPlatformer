namespace Survey
{
    public enum Scale
    {
        None,
        NotAtAll,
        Slightly,
        Moderately,
        Fairly,
        Extremely
    }

    public static class ScaleExtensions
    {
        public static int ToInt(this Scale scale)
        {
            switch (scale)
            {
                case Scale.NotAtAll:
                    return 0;
                case Scale.Slightly:
                    return 1;
                case Scale.Moderately:
                    return 2;
                case Scale.Fairly:
                    return 3;
                case Scale.Extremely:
                    return 4;
                case Scale.None:
                default:
                    return -1;
            }
        }
    }
}