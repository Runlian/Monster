namespace Monster.Common
{
    /// <summary>
    /// 树模型
    /// </summary>
    public class TreeModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string ParentId { get; set; }
        public bool Checked { get; set; }
        public bool HasChildren { get; set; }

    }
}