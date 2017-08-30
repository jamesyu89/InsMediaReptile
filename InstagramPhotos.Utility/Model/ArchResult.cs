namespace InstagramPhotos.Utility.Model
{
    /// <summary>
    /// 易果盖亚框架公共返回实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArchResult<T>
    {
        public bool State { get; set; }

        public int ResultCode { get; set; }

        public string ErrorMessage { get; set; }

        public T ResultObj { get; set; }
    }
}
